# Ollama Integration in Aspire

This project now includes an Ollama container integration for local LLM capabilities. 

## Getting Started with Ollama

When you first run the Aspire application, the Ollama container will be started automatically. However, you'll need to manually pull the required model after the container starts.

To pull the model, you can either:

1. Access the Ollama container in the Aspire dashboard and run the following command:
   ```bash
   ollama pull llama3.2:3b
   ```

2. Use Docker directly:
   ```powershell
   docker exec -it [ollama-container-name] ollama pull llama3.2:3b
   ```

## Configuration

The Ollama API URL is configured in both `appsettings.json` and `appsettings.Development.json` as `OLLAMA_API_URL`. The default URL is `http://localhost:11434` when running locally.

In the Aspire environment, the container URL is automatically passed to the BlazorAI application.

## Using Ollama in the Chat Component

The Chat component is configured to use the Ollama LLM when the "Local" provider is selected. It uses the `llama3.2:3b` model by default.

```csharp
var ollamaApiUrl = Configuration["OLLAMA_API_URL"] ?? "http://localhost:11434";
kernelBuilder.AddOllamaChatCompletion("llama3.2:3b", new Uri(ollamaApiUrl));
```
