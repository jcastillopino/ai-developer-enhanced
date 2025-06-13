using Microsoft.AspNetCore.Components;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

#pragma warning disable SKEXP0070 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

namespace BlazorAI.Components.Pages;

public enum Provider
{
    Local,
    Remote,
    GitHub
}

public partial class Chat
{
    private ChatHistory? chatHistory;
    private Kernel? kernel;
    private OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new();
    private Provider selectedProvider = Provider.Local; // Default to local provider
    private string selectedModel = "llama3.2:3b"; // Default model for ollama
    private string azureModelName = string.Empty; // Model name for Azure OpenAI
    private string githubModelName = string.Empty; // Model name for GitHub
    private string errorMessage = string.Empty; // Error message for invalid configurations

    [Inject]
    public required IConfiguration Configuration { get; set; }
    [Inject]
    private ILoggerFactory LoggerFactory { get; set; } = null!;
    protected async Task InitializeSemanticKernel(Provider? provider = null)
    {
        chatHistory = [];

        // Use provided provider or current selected provider
        Provider currentProvider = provider ?? selectedProvider;

        // Clear any previous error messages
        errorMessage = string.Empty;

        // Challenge 02 - Configure Semantic Kernel
        var kernelBuilder = Kernel.CreateBuilder();

        try
        {
            // Challenge 02 - Add OpenAI Chat Completion
            // Determine the configuration prefix based on provider type
            switch (currentProvider)
            {
                case Provider.Local:
                    // Ollama in Container
                    var ollamaApiUrl = Configuration["OLLAMA_API_URL"] ?? "http://localhost:11434";
                    kernelBuilder.AddOllamaChatCompletion(selectedModel, new Uri(ollamaApiUrl));
                    chatHistory.AddSystemMessage(
                        "You are a helpful AI assistant. You can answer questions, provide information, and assist with various tasks. " +
                        "If you don't know the answer, you can say 'I don't know'." +
                        "Please use the tools provided when appropriate, do not guess."
                    );
                    break;

                case Provider.Remote:
                    // Remote Azure OpenAI
                    string azureModelToUse = !string.IsNullOrEmpty(azureModelName) ? azureModelName :
                        Configuration["AOI_DEPLOYMODEL"] ?? throw new InvalidOperationException("Azure OpenAI model configuration is missing");

                    kernelBuilder.AddAzureOpenAIChatCompletion(
                        deploymentName: azureModelToUse,
                        endpoint: Configuration["AOI_ENDPOINT"] ?? throw new InvalidOperationException("Azure OpenAI endpoint configuration is missing"),
                        apiKey: Configuration["AOI_API_KEY"] ?? throw new InvalidOperationException("Azure OpenAI API key configuration is missing")
                    );
                    break;

                case Provider.GitHub:
                    // GitHub Models
                    string githubModelToUse = !string.IsNullOrEmpty(githubModelName) ? githubModelName :
                        Configuration["GITHUB_DEPLOYMODEL"] ?? throw new InvalidOperationException("GitHub model configuration is missing");

                    kernelBuilder.AddOpenAIChatCompletion(
                        modelId: githubModelToUse,
                        endpoint: new Uri(Configuration["GITHUB_ENDPOINT"] ?? throw new InvalidOperationException("GitHub endpoint configuration is missing")),
                        apiKey: Configuration["GITHUB_API_KEY"] ?? throw new InvalidOperationException("GitHub API key configuration is missing")
                    );
                    break;

                default:
                    throw new ArgumentException($"Unknown provider: {currentProvider}");
            }
        }
        catch (Exception ex)
        {
            errorMessage = $"Configuration error: {ex.Message}";
            StateHasChanged();
            return;
        }

        // Add Logger for Kernel
        kernelBuilder.Services.AddSingleton(LoggerFactory);

        // Challenge 03 and 04 - Services Required
        kernelBuilder.Services.AddHttpClient();

        // Challenge 05 - Register Azure AI Foundry Text Embeddings Generation


        // Challenge 05 - Register Search Index


        // Challenge 07 - Add Azure AI Foundry Text To Image


        // Challenge 02 - Finalize Kernel Builder
        kernel = kernelBuilder.Build();

        // Challenge 03, 04, 05, & 07 - Add Plugins
        await AddPlugins();

        // Challenge 03 - Create OpenAIPromptExecutionSettings
        openAIPromptExecutionSettings = new()
        {
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
        };
    }
    private Task AddPlugins()
    {
        // Challenge 03 - Add Time Plugin
        kernel!.Plugins.AddFromType<Plugins.TimePlugin>("TimePlugin");

        // Challenge 04 - Import OpenAPI Spec

        // Challenge 05 - Add Search Plugin

        // Challenge 07 - Text To Image Plugin

        return Task.CompletedTask;
    }
    private async Task SendMessage()
    {
        if (string.IsNullOrWhiteSpace(newMessage))
        {
            return;
        }        // Initialize the kernel if it hasn't been initialized yet
        if (kernel == null)
        {
            await InitializeSemanticKernel();
        }

        if (chatHistory == null)
        {
            return;
        }

        // Check if there are any configuration errors
        if (!string.IsNullOrEmpty(errorMessage))
        {
            return;
        }

        // This tells Blazor the UI is going to be updated.
        StateHasChanged();
        loading = true;
        // Copy the user message to a local variable and clear the newMessage field in the UI
        var userMessage = newMessage;
        newMessage = string.Empty;

        try
        {
            // Challenge 02 - Update Chat History
            chatHistory.AddUserMessage(userMessage);

            // Challenge 02 - Retrieve the chat completion service
            var chatCompletionService = kernel!.GetRequiredService<IChatCompletionService>();
            // Challenge 02 - Send a message to the chat completion service
            var response = await chatCompletionService!.GetChatMessageContentsAsync(
                chatHistory,
                executionSettings: openAIPromptExecutionSettings,
                kernel: kernel
            );

            // Challenge 02 - Add Response to the Chat History object
            chatHistory.AddAssistantMessage(response.FirstOrDefault()?.Content ?? "No response");

            // Clear any previous error messages on successful response
            errorMessage = string.Empty;
        }
        catch (Microsoft.SemanticKernel.HttpOperationException ex) when (ex.Message.Contains("401"))
        {
            // Handle 401 Unauthorized specifically
            errorMessage = "Authentication failed. Please check your API key and endpoint configuration.";
            // Remove the user message since the request failed
            if (chatHistory.Count > 0 && chatHistory.Last().Role.ToString() == "user")
            {
                chatHistory.RemoveAt(chatHistory.Count - 1);
            }
            // Restore the user's message to the input field
            newMessage = userMessage;
        }
        catch (Microsoft.SemanticKernel.HttpOperationException ex) when (ex.Message.Contains("429"))
        {
            // Handle rate limiting
            errorMessage = "Rate limit exceeded. Please wait a moment before trying again.";
            // Remove the user message since the request failed
            if (chatHistory.Count > 0 && chatHistory.Last().Role.ToString() == "user")
            {
                chatHistory.RemoveAt(chatHistory.Count - 1);
            }
            // Restore the user's message to the input field
            newMessage = userMessage;
        }
        catch (Microsoft.SemanticKernel.HttpOperationException ex)
        {
            // Handle other HTTP errors
            var statusMatch = System.Text.RegularExpressions.Regex.Match(ex.Message, @"Status: (\d+)");
            var statusCode = statusMatch.Success ? statusMatch.Groups[1].Value : "Unknown";
            errorMessage = $"API request failed with status {statusCode}. Please check your configuration and try again.";
            // Remove the user message since the request failed
            if (chatHistory.Count > 0 && chatHistory.Last().Role.ToString() == "user")
            {
                chatHistory.RemoveAt(chatHistory.Count - 1);
            }
            // Restore the user's message to the input field
            newMessage = userMessage;
        }
        catch (Exception ex)
        {
            // Handle any other unexpected errors
            errorMessage = $"An unexpected error occurred: {ex.Message}";
            // Remove the user message since the request failed
            if (chatHistory.Count > 0 && chatHistory.Last().Role.ToString() == "user")
            {
                chatHistory.RemoveAt(chatHistory.Count - 1);
            }
            // Restore the user's message to the input field
            newMessage = userMessage;
        }
        finally
        {
            loading = false;
            StateHasChanged();
        }
    }
    private async Task OnProviderChanged(Provider value)
    {
        // Only reinitialize if the provider has changed
        if (value == selectedProvider)
        {
            return;
        }
        selectedProvider = value;
        
        // Initialize model names from configuration if not already set
        if (value == Provider.Remote && string.IsNullOrEmpty(azureModelName))
        {
            azureModelName = Configuration["AOI_DEPLOYMODEL"] ?? "";
        }
        else if (value == Provider.GitHub && string.IsNullOrEmpty(githubModelName))
        {
            githubModelName = Configuration["GITHUB_DEPLOYMODEL"] ?? "";
        }
        
        // Clear the chat history when switching providers
        chatHistory?.Clear();
        await InitializeSemanticKernel();
        StateHasChanged();
    }    private async Task OnModelChanged(string value)
    {
        // Only reinitialize if the model has changed
        if (value == selectedModel)
        {
            return;
        }
        selectedModel = value;
        // Clear the chat history when switching models
        chatHistory?.Clear();
        await InitializeSemanticKernel();
        StateHasChanged();
    }
    
    private async Task OnAzureModelChanged(string value)
    {
        if (value == azureModelName)
        {
            return;
        }
        azureModelName = value;
        if (selectedProvider == Provider.Remote)
        {
            chatHistory?.Clear();
            await InitializeSemanticKernel();
            StateHasChanged();
        }
    }
    
    private async Task OnGitHubModelChanged(string value)
    {
        if (value == githubModelName)
        {
            return;
        }
        githubModelName = value;
        if (selectedProvider == Provider.GitHub)
        {
            chatHistory?.Clear();
            await InitializeSemanticKernel();
            StateHasChanged();
        }
    }

    private void ClearError()
    {        errorMessage = string.Empty;
        StateHasChanged();
    }

    private async Task OnProviderStringChanged(string value)
    {
        Provider newProvider = value switch
        {
            "local" => Provider.Local,
            "remote" => Provider.Remote,
            "github" => Provider.GitHub,
            _ => Provider.Local
        };
        
        await OnProviderChanged(newProvider);
    }
}

