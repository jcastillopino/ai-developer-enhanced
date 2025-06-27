# My AI Developer Workshop

*A personal learning journey through Azure AI Foundry and Semantic Kernel fundamentals*

Read an article about the work in this repo: [Prototyping Smarter with Aspire, Ollama & GitHub Models](https://code-smarter.com/prototyping-smarter-with-aspire-ollama-and-github-models)

## About This Repository

This repository contains my personal exploration and implementation of the AI Developer workshop materials, focusing on Azure AI Foundry and Semantic Kernel fundamentals. It includes solutions and experiments across multiple programming languages including C#, Python, and Java.

Check out [Stable Code Branch](https://github.com/jcastillopino/ai-developer-enhanced/tree/Stable-Code) for an stable version. I'm actively testing in main 😊

## Original Workshop Content

This workshop consists of eight challenges designed to encourage learning and research in AI development. If you want a deeper understanding of how to implement an AI solution but have little or no experience with OpenAI or Semantic Kernel, these challenges provide an excellent foundation.

## Learning Objectives

Through these materials, I'm learning how to:

- Build a simple chat using Semantic Kernel and C# or Python or Java
- Add plugins and enable auto calling to create Planners
- Import existing APIs using OpenAPI
- Implement Retrieval Augmented Generation (RAG)
  - Document Chunking
  - Grounding AI
- Working with Image generation
- Multi-Agent workflows

## Repository Structure

This repository is organized into three main technology stacks:

### .NET/C# (`/Dotnet/`)
- **BlazorAI**: Main Blazor application with AI integration
- **Aspire**: .NET Aspire orchestration for cloud-native development
- **eShopLite**: E-commerce sample with AI enhancements
- **WorkItems**: Work item management system

### Python (`/Python/`)
- Core Python implementations
- Multi-agent systems
- Plugin architectures
- Data processing and RAG implementations

### Java (`/Java/`)
- Java-based implementations of the workshop challenges
- Maven project structure

## Challenges

- Challenge 00: Prerequisites - Prepare your workstation to work with Azure.
  - [C#](./Dotnet/challenges/Challenge-00.md)
  - [Python](./Python/challenges/Challenge-00.md)
  - [Java](./Java/challenges/Challenge-00.md)

- Challenge 01: Azure AI Foundry Fundamentals - Deploy an Azure AI Foundry Model, practice prompt engineering.
  - [C#](./Dotnet/challenges/Challenge-01.md)
  - [Python](./Python/challenges/Challenge-01.md)
  - [Java](./Java/challenges/Challenge-01.md)

- Challenge 02: Semantic Kernel Fundamentals - Connect your OpenAI model, test your application.
  - [C#](./Dotnet/challenges/Challenge-02.md)
  - [Python](./Python/challenges/Challenge-02.md)
  - [Java](./Java/challenges/Challenge-02.md)

- Challenge 03: Plugins - Create Semantic Kernel plugins, enable auto function calling, explore planners.
  - [C#](./Dotnet/challenges/Challenge-03.md)
  - [Python](./Python/challenges/Challenge-03.md)
  - [Java](./Java/challenges/Challenge-03.md)

- Challenge 04: Import Plugin using OpenAPI - Import an API using an OpenAPI specification.
  - [C#](./Dotnet/challenges/Challenge-04.md)
  - [Python](./Python/challenges/Challenge-04.md)

- Challenge 05: Retrieval-Augmented Generation (RAG) - Implement document chunking & embedding, enhance AI responses with external sources.
  - [C#](./Dotnet/challenges/Challenge-05.md)
  - [Python](./Python/challenges/Challenge-05.md)
  - [Java](./Java/challenges/Challenge-04.md)

- Challenge 06: Responsible AI: Exploring Content Filters in Azure AI Foundry - Configure, test, and customize content filters.
  - [C#](./Dotnet/challenges/Challenge-06.md)
  - [Python](./Python/challenges/Challenge-06.md)

- Challenge 07: Image Generation using DALL-E - Work with text-to-image models, build an image generating plugin.
  - [C#](./Dotnet/challenges/Challenge-07.md)
  - [Python](./Python/challenges/Challenge-07.md)

- Challenge 08: Multi-Agent Systems - Create a multi-agent conversation, implement in Semantic Kernel.
  - [C#](./Dotnet/challenges/Challenge-08.md)
  - [Python](./Python/challenges/Challenge-08.md)

- Challenge 09: Infuse existing Apps with AI - Enhance the Product Search functionality using Semantic Kernel.
  - [C#](./Dotnet/challenges/Challenge-09.md)

- Challenge 10: Enrich user experience with Semantic Kernel - Enhance the Product Search page with a creative AI response.
  - [C#](./Dotnet/challenges/Challenge-10.md)

### Here are the [Workshop Slides](./Dotnet/challenges/Resources/Lectures.pdf)

### Complete Code Solutions - [Download Zip](https://github.com/microsoft/ai-developer/raw/refs/heads/main/misc/finalresult.zip)

## Getting Started

1. **Prerequisites**: Start with Challenge 00 to set up your development environment
2. **Choose Your Language**: Pick C#, Python, or Java based on your preference
3. **Work Through Challenges**: Complete challenges sequentially for the best learning experience
4. **Experiment**: Feel free to modify and extend the solutions

## Personal Notes and Modifications

*This section will contain my personal insights, modifications, and learnings as I progress through the workshop.*

## Original Contributors

This workshop was created by the Microsoft team including:
- [Chris McKee](https://github.com/ChrisMcKee1)
- [Randy Patterson](https://github.com/RandyPatterson)
- [Zack Way](https://github.com/seiggy)
- [Vivek Mishra](https://github.com/mishravivek-ms)
- [Travis Terrell](https://github.com/travisterrell)
- [Eric Rhoads](https://github.com/ecrhoads)
- [Wael Kdouh](https://github.com/waelkdouh)
- [Munish Malhotra](https://github.com/munishm)
- [Brijraj Singh](https://github.com/brijrajsingh)
- [Linda M Thomas](https://github.com/lindamthomas)
- [Suman More](https://github.com/sumanmore257)

## License

This project maintains the original MIT License from the source workshop materials.
