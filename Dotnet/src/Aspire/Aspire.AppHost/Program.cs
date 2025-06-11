var builder = DistributedApplication.CreateBuilder(args);

// Add Ollama container
var ollama = builder.AddContainer("ollama", "ollama/ollama:latest")
    .WithHttpEndpoint(port: 11434, targetPort: 11434, name: "http-api")
    .WithVolume("ollama-data", "/root/.ollama")
    .WithEnvironment("OLLAMA_HOST", "0.0.0.0:11434")
    .WithEnvironment("OLLAMA_MODELS_PATH", "/root/.ollama/models")
    .WithEnvironment("OLLAMA_CUDA", "1")
    .WithEnvironment("OLLAMA_FLASH_ATTENTION", "1")
    .WithEnvironment("OLLAMA_NUM_THREADS", "8")
    .WithEnvironment("OLLAMA_MAX_LOADED_MODELS", "2")
    .WithEnvironment("OLLAMA_DEBUG", "1") // opcional, para logs detallados
    .WithContainerRuntimeArgs("--gpus", "all"); // habilita GPU en Docker
    

var workitems = builder.AddProject<Projects.WorkItems>("workitems-api");

builder.AddProject<Projects.BlazorAI>("blazor-aichat")
    .WithExternalHttpEndpoints()
    .WithReference(workitems)
    .WaitFor(workitems)
    .WaitFor(ollama)
    .WithEnvironment("WORKITEMS_BASE_URL", workitems.GetEndpoint("https"))
    .WithEnvironment("OPEN_API_DOC_ROUTE", "/openapi/v1.json")
    .WithEnvironment("OLLAMA_API_URL", ollama.GetEndpoint("http-api"));

builder.Build().Run();
