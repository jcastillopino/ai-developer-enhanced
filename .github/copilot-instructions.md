# AI Developer Workshop - Copilot Instructions

## Project Overview
This repository contains a comprehensive workshop for learning Azure AI Foundry and Semantic Kernel fundamentals using .NET. The repository is hosted in github: jcastillopino/ai-developer-enhanced

## Key Technologies
- **Azure AI Foundry**: Cloud-based AI service for deploying and managing AI models
- **Semantic Kernel**: Microsoft's lightweight SDK for building AI-infused applications
- **Blazor Server**: For the chat application UI
- **ASP.NET Core**: For API development
- **.NET Aspire**: For application orchestration and service discovery
- **Azure AI Search**: For implementing RAG (Retrieval-Augmented Generation)
- **OpenAPI**: For API integration and plugin development

## Project Structure

### Core Applications
- `src/BlazorAI/`: Main chat application demonstrating Semantic Kernel integration
- `src/eShopLite/`: E-commerce application for AI search integration
- `src/WorkItems/`: Demo API with OpenAPI specification for plugin development
- `src/Aspire/`: Orchestration and service discovery

## Development Guidelines

### When working with Semantic Kernel:
- Always use dependency injection for services
- Implement proper chat history management
- Use the kernel builder pattern for configuration
- Enable automatic function calling for plugins
- Follow plugin naming conventions (no spaces, under 64 characters)

### When working with Azure AI services:
- Store sensitive configuration in appsettings.Developer.json and create placeholders in appsettings.json
- Use proper endpoint URL format: `https://<deployment-name>.openai.azure.com`
- Implement proper error handling for API calls
- Consider content filtering and responsible AI practices

### Plugin Development:
- Use `[KernelFunction]` attribute for plugin methods
- Provide clear descriptions for function parameters
- Implement proper parameter validation
- Use `IHttpClientFactory` for external API calls
- Follow the established plugin folder structure

### API Development:
- Include comprehensive OpenAPI documentation
- Use proper HTTP status codes
- Implement CORS for cross-origin requests
- Provide clear endpoint descriptions and examples

## Common Patterns

### Semantic Kernel Setup:
```csharp
var kernelBuilder = Kernel.CreateBuilder();
kernelBuilder.AddAzureOpenAIChatCompletion(
    deploymentModel, endpoint, apiKey);
var kernel = kernelBuilder.Build();
```

### Plugin Registration:
```csharp
kernel.Plugins.AddFromObject(new MyPlugin(), "PluginName");
```

### Chat Completion:
```csharp
var chatService = kernel.GetRequiredService<IChatCompletionService>();
var response = await chatService.GetChatMessageContentsAsync(chatHistory);
```

## File-Specific Context

### BlazorAI Application
- Main learning application for Semantic Kernel
- Implements chat interface with plugin support
- Contains examples for time, weather, and geocoding plugins
- Demonstrates automatic function calling

### eShopLite Application
- Production-like e-commerce application
- Shows AI integration in existing applications
- Implements semantic search using Azure AI Search
- Demonstrates AI-generated content

### WorkItems API
- RESTful API with OpenAPI specification
- Used for teaching API integration as plugins
- Includes CRUD operations for work items
- Demonstrates proper API documentation

When providing assistance, prioritize educational value and follow the progressive learning structure of the challenges.
