using Aspire.Hosting;
using CommunityToolkit.Aspire.Hosting.Ollama;

var builder = DistributedApplication.CreateBuilder(args);

// Use Community Toolkit Ollama extension with full features
var ollamaService = builder.AddOllama("ollama") // Represents the Ollama service
    .WithDataVolume()                           // Persistent model storage
    .WithGPUSupport(OllamaGpuVendor.Nvidia)     // Native GPU support
    .WithOpenWebUI();                            // Management UI

var qwen3Model = ollamaService.AddModel("qwen3", "qwen3:8b"); // Represents the specific model in Ollama
var llama32Model = ollamaService.AddModel("llama32", "llama3.2:3b"); // Represents the Ollama 3.2 model

var workitems = builder.AddProject<Projects.WorkItems>("workitems-api");

builder.AddProject<Projects.BlazorAI>("blazor-aichat")
    .WithExternalHttpEndpoints()
    .WithReference(workitems)
    .WithReference(ollamaService) // Reference the Ollama service
    .WaitFor(workitems)
    .WaitFor(qwen3Model)         // Wait for the qwen3 model to be ready
    .WaitFor(llama32Model)       // Wait for the llama 3.2 model to be ready
    .WithEnvironment("WORKITEMS_BASE_URL", workitems.GetEndpoint("https"))
    .WithEnvironment("OLLAMA_API_URL", ollamaService.GetEndpoint("http")) // Get endpoint from the Ollama service
    .WithEnvironment("OPEN_API_DOC_ROUTE", "/openapi/v1.json");

builder.Build().Run();
