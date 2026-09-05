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

Derive the test framework and dependencies from the selected test project and match its existing conventions. Do not edit generated `*.feature.cs` files; change the source `.feature` file or bindings.

## Non-unit test lifecycle

Before running an integration, acceptance/e2e, or system test project:

1. Verify that the Docker daemon is available. Start it if it is not running, then confirm that Docker commands succeed.
2. From the affected implementation directory, run `docker compose down --volumes` to remove containers, networks, and disposable test volumes left by earlier runs. This deletes data stored in that implementation's local Compose volumes.
3. Before building the Compose images, verify that `src/ContosoUniversity.Mvc/cert.pfx` exists. If it is missing, generate it with the command documented in the respective implementation's `README.md`.
4. Run `docker compose build` from the implementation directory so image creation does not consume the test fixture's startup timeout. The build may be skipped only when the existing images were built from the current code and none of their Docker build inputs have changed.
5. Enter the selected test project's directory and run the project-scoped restore, build, and test sequence below.
6. After the test finishes or fails, return to the implementation directory and run `docker compose down --volumes` again so the next test starts from a clean environment.

Run multiple non-unit test projects sequentially and complete this lifecycle for each project. Do not allow two test projects to manage the same Compose environment concurrently.

## Project-scoped test command

Run tests at test-project scope, not solution scope. Change to the directory that contains the selected test project's `.csproj`, then run:

```bash
cd path/to/TestProject
dotnet restore
dotnet build --no-restore
dotnet test --no-build
```

Do not use `dotnet test` on the `.slnx`: it can start multiple test projects in parallel, including projects that compete for the same Docker Compose resources. To run several test projects, invoke them one at a time from their respective directories.

This project-scoped sequence is self-contained and builds the test project together with its referenced production projects. Build the owning solution separately only when the task requires verification that projects outside that dependency graph still compile. Run additional test projects only when their coverage is relevant and required dependencies are available.

Before finishing, state which tests ran, their outcomes, and why the selected scope is sufficient. If Docker or another dependency prevents a test, report the exact limitation instead of claiming full verification.
