# Testing skill

Use this skill when adding, changing, or reviewing tests.

## General principles

Choose the test level based on where the behavior lives.

Do not replace meaningful integration tests with unit tests when the behavior depends on ASP.NET Core hosting, EF Core, SQL Server, RabbitMQ, HTTP clients, or Docker runtime wiring.

Do not add broad end-to-end tests for behavior that can be covered reliably at the domain, API, or worker level.

## Test levels

### Unit tests

Use unit tests for:

- domain behavior;
- value objects;
- pure application logic;
- validation rules;
- command/query handler logic when dependencies can be isolated safely.

### Integration tests

Use integration tests for:

- ASP.NET Core endpoints;
- EF Core repositories;
- SQL Server behavior;
- RabbitMQ consumers;
- typed HTTP clients;
- worker handlers;
- service wiring.

### E2E / acceptance tests

Use e2e tests for:

- browser-visible user workflows;
- MVC page navigation;
- form submissions;
- high-level smoke scenarios.

## Existing tooling

The repository uses several testing tools depending on the implementation and test level:

- xUnit
- NUnit
- SpecFlow
- Playwright
- Testcontainers.MsSql
- Testcontainers.RabbitMq
- WireMock.Net
- Ductus.FluentDocker
- Microsoft.AspNetCore.Mvc.Testing

## Monolith testing

`apps/monolith` contains:

- unit tests;
- integration tests;
- e2e tests;
- system tests.

Integration tests cover the MVC application and database-dependent behavior.

Acceptance tests use Playwright, NUnit, SpecFlow, and Docker-based infrastructure.

## Microservices testing

`apps/mservices` contains tests for:

- MVC frontend;
- Courses API;
- Departments API;
- Students API;
- Courses worker;
- Departments worker;
- Students worker.

When changing a worker consumer, add or update worker integration tests.

When changing an API endpoint, add or update API integration tests.

When changing typed HTTP clients, consider WireMock-based tests.

## CI expectations

Before considering a change complete, the relevant solution should be able to run:

```bash
dotnet restore
dotnet build --no-restore
dotnet test --no-build
```

If a change affects formatting or analyzers, expect `dotnet format` checks to run in CI.
