using Microsoft.AspNetCore.Components;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

#pragma warning disable SKEXP0040 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
#pragma warning disable SKEXP0020 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
#pragma warning disable SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
#pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
#pragma warning disable SKEXP0070 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

namespace BlazorAI.Components.Pages;

public partial class Chat
{
    private ChatHistory? chatHistory;
    private Kernel? kernel;
    private OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new();
    private bool isLocalProvider = true; // Default to local provider
    private string selectedModel = "llama3.2:3b"; // Default model

    [Inject]
    public required IConfiguration Configuration { get; set; }
    [Inject]
    private ILoggerFactory LoggerFactory { get; set; } = null!;

    protected async Task InitializeSemanticKernel(bool useLocalProvider = true)
    {
        chatHistory = [];
        isLocalProvider = useLocalProvider;        // Challenge 02 - Configure Semantic Kernel
        var kernelBuilder = Kernel.CreateBuilder();

        // Challenge 02 - Add OpenAI Chat Completion
        // Determine the configuration prefix based on provider type
        string prefix = useLocalProvider ? "LOCALFOUNDRY_" : "AOI_";

        
          if (useLocalProvider)
        {
            // Ollama in Container
            var ollamaApiUrl = Configuration["OLLAMA_API_URL"] ?? "http://localhost:11434";
            kernelBuilder.AddOllamaChatCompletion(selectedModel, new Uri(ollamaApiUrl));
            chatHistory.AddSystemMessage(
                "You are a helpful AI assistant. You can answer questions, provide information, and assist with various tasks. " +
                "If you don't know the answer, you can say 'I don't know'." +
                "Please use the tools provided when appropriate, do not guess."
            );
            // Local Foundry
            // kernelBuilder.AddOpenAIChatCompletion(
            //     modelId: Configuration[$"{prefix}DEPLOYMODEL"]!,
            //     endpoint: new Uri(Configuration[$"{prefix}ENDPOINT"]!),
            //     apiKey: Configuration[$"{prefix}API_KEY"]!
            // );
        }
        else
        {
            // Remote Azure OpenAI
            kernelBuilder.AddAzureOpenAIChatCompletion(
                deploymentName: Configuration[$"{prefix}DEPLOYMODEL"]!,
                endpoint: Configuration[$"{prefix}ENDPOINT"]!,
                apiKey: Configuration[$"{prefix}API_KEY"]!
            );
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
        }

        // Initialize the kernel if it hasn't been initialized yet
        if (kernel == null)
        {
            await InitializeSemanticKernel(isLocalProvider);
        }

        if (chatHistory == null)
        {
            return;
        }

        // This tells Blazor the UI is going to be updated.
        StateHasChanged();
        loading = true;
        // Copy the user message to a local variable and clear the newMessage field in the UI
        var userMessage = newMessage;
        newMessage = string.Empty;

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

        loading = false;
    }

    private async Task OnProviderChanged(string value)
    {
        bool newIsLocalProvider = value == "local";

        // Only reinitialize if the provider has changed
        if (newIsLocalProvider == isLocalProvider)
        {
            return;
        }
        isLocalProvider = newIsLocalProvider;
        // Clear the chat history when switching providers
        chatHistory?.Clear();
        await InitializeSemanticKernel(isLocalProvider);
        StateHasChanged();
    }

    private async Task OnModelChanged(string value)
    {
        // Only reinitialize if the model has changed
        if (value == selectedModel)
        {
            return;
        }
        selectedModel = value;
        // Clear the chat history when switching models
        chatHistory?.Clear();
        await InitializeSemanticKernel(isLocalProvider);
        StateHasChanged();
    }
}

