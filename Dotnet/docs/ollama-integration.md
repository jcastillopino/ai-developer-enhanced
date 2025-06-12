# Ollama Integration in Aspire

This project uses the **CommunityToolkit.Aspire.Hosting.Ollama** extension for seamless local LLM integration with native Aspire service discovery and automatic model management.

## Architecture Overview

The Ollama service is configured in the Aspire AppHost using the modern CommunityToolkit extension, which provides:

- **Automatic Model Management**: Models are downloaded automatically on first startup
- **GPU Support**: Native NVIDIA/AMD GPU acceleration
- **Service Discovery**: Automatic endpoint configuration through Aspire references
- **Web UI**: Integrated OpenWebUI for model management
- **Persistent Storage**: Docker volume for model persistence

## Getting Started with Ollama

### Automatic Setup

The Ollama service starts automatically when you run the Aspire application. No manual model pulling is required!

```csharp
// Aspire AppHost configuration
var ollama = builder.AddOllama("ollama")
    .WithDataVolume()                           // Persistent model storage
    .WithGPUSupport(OllamaGpuVendor.Nvidia)     // Native GPU support
    .AddModel("qwen3", "qwen3:8b")              // Automatic model management
    .WithOpenWebUI();                           // Management UI
```

### What Happens on First Run

1. **Container Startup**: Ollama container starts with GPU support enabled
2. **Model Download**: The `qwen3:8b` model is automatically downloaded
3. **Service Registration**: Ollama endpoint is registered with Aspire service discovery
4. **Web UI Available**: OpenWebUI becomes accessible for model management

## Configuration

### Aspire Service Discovery

The Ollama API URL is **automatically configured** through Aspire service references. No manual configuration needed!

```csharp
// In Aspire AppHost - automatic service discovery
builder.AddProject<Projects.BlazorAI>("blazor-aichat")
    .WithReference(ollama)     // Automatic OLLAMA_API_URL configuration
    .WaitFor(ollama);          // Ensures Ollama starts first
```

### GPU Support

GPU acceleration is automatically configured for NVIDIA cards:

- **Docker Runtime**: Automatically adds `--gpus all` argument
- **CUDA Support**: Native CUDA integration for model inference
- **Performance**: Significantly faster model responses with GPU acceleration

### Model Management

Models are managed through the CommunityToolkit extension:

- **Automatic Download**: Models specified in `.AddModel()` are downloaded on startup
- **Persistent Storage**: Models persist across container restarts
- **Version Control**: Specific model versions can be specified
- **Multiple Models**: Multiple models can be added with additional `.AddModel()` calls

## Using Ollama in the Chat Component

The Chat component automatically receives the Ollama endpoint through Aspire service discovery:

```csharp
// Automatic endpoint resolution - no manual URL configuration needed
var ollamaApiUrl = Configuration["OLLAMA_API_URL"]; // Set by Aspire automatically
kernelBuilder.AddOllamaChatCompletion("qwen3:8b", new Uri(ollamaApiUrl));
```

## Available Interfaces

### Ollama API
- **Endpoint**: Automatically configured via `OLLAMA_API_URL`
- **Models**: `qwen3:8b` (automatically downloaded)
- **GPU Acceleration**: Enabled for NVIDIA cards

### OpenWebUI (Management Interface)
- **Access**: Available through Aspire dashboard
- **Features**: Model management, chat interface, settings
- **URL**: Automatically configured and exposed

## Troubleshooting

### Model Not Available
If the model isn't immediately available, wait for the automatic download to complete. Check the Aspire dashboard logs for download progress.

### GPU Not Detected
Ensure NVIDIA drivers and Docker Desktop GPU support are properly configured on your system.

### Container Issues
Check the Aspire dashboard for container logs and status. The CommunityToolkit extension provides better error handling than manual container setups.

## Benefits Over Manual Setup

✅ **Automatic Model Management** - No manual `ollama pull` commands  
✅ **Native GPU Support** - Automatic CUDA configuration  
✅ **Service Discovery** - Automatic endpoint resolution  
✅ **Better Logging** - Integrated with Aspire telemetry  
✅ **Simplified Configuration** - Declarative setup in AppHost  
✅ **Web UI Integration** - OpenWebUI included by default  

## Package Requirements

Add the CommunityToolkit.Aspire.Hosting.Ollama package to your Aspire AppHost project:

```xml
<PackageReference Include="CommunityToolkit.Aspire.Hosting.Ollama" Version="9.5.1-beta.306" />
```

## Migration from Manual Setup

If migrating from a manual Docker container setup:

1. **Remove manual container configuration** from Program.cs
2. **Add CommunityToolkit package** to your project
3. **Replace container setup** with `.AddOllama()` extension
4. **Remove manual environment variables** - Aspire handles them automatically
5. **Update model references** to match the new model name (e.g., `qwen3:8b`)

This provides a much more maintainable and Aspire-native integration compared to manual Docker container management.
