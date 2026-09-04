---
name: testing
description: Select, add, run, or review tests for changes in either Contoso University implementation. Use when behavior changes, tests fail, coverage is requested, or verification scope must be chosen.
---

# Testing

Read the owning implementation's `AGENTS.md`, solution structure, affected production code, and nearby tests before choosing a test level.

Use the lowest level that proves the behavior:

- unit tests for domain behavior, value objects, pure logic, and validation;
- integration tests for ASP.NET Core endpoints, EF Core/SQL Server behavior, typed HTTP clients, RabbitMQ consumers, workers, and dependency wiring;
- acceptance/e2e tests for browser-visible workflows that are not adequately proven below;
- system tests for assembled Compose runtime health and cross-process behavior.

The repository uses xUnit and NUnit depending on the project, plus Reqnroll, Playwright, Testcontainers, WireMock.Net, FluentDocker, and ASP.NET Core test hosting. Match the framework and conventions of the existing test project. Do not edit generated `*.feature.cs` files.

Run focused tests first, for example from an implementation directory:

```bash
dotnet test path/to/TestProject.csproj
```

Build the owning solution when shared contracts, project references, or production compilation may be affected. Run broader suites only when their additional coverage is relevant and required dependencies are available.

Before finishing, state which tests ran, their outcomes, and why the selected scope is sufficient. If Docker or another dependency prevents a test, report the exact limitation instead of claiming full verification.

