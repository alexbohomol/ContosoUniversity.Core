# Copilot instructions for Contoso University

This repository is a pet project named **Contoso University**. It is based on the canonical ASP.NET / Entity Framework demo application and has been evolved into a modern .NET playground for practicing architecture, testing, infrastructure, CI/CD, observability, and deployment concepts.

The repository contains two functionally similar implementations of the same university domain:

- `apps/monolith` — ASP.NET Core MVC modular monolith.
- `apps/mservices` — microservices-style implementation with MVC frontend, APIs, workers, HTTP clients, RabbitMQ, and Docker Compose.

## General rules

Always inspect the actual repository code before making claims about architecture, behavior, dependencies, or deployment.

Do not infer implementation details from general ASP.NET Core, EF Core, Docker, or microservices conventions when the repository can be checked.

Prefer small, focused changes that preserve the existing structure, naming style, and architectural boundaries.

Documentation must be written in English.

When modifying code, update or add tests at the appropriate level.

When answering architecture questions, distinguish between:

- .NET projects;
- runtime services;
- deployable units;
- domain modules;
- data access modules;
- test-only infrastructure.

## Important repository boundaries

For `apps/monolith`:

- The MVC application is the main deployable unit.
- The MVC application may directly register and use application/data modules.
- The monolith has an AWS ECS/Fargate QA deployment path.

For `apps/mservices`:

- `ContosoUniversity.Mvc` is a frontend and should communicate with backend services through typed HTTP clients.
- The MVC frontend should not directly depend on EF Core data modules.
- Cross-domain cleanup workflows use RabbitMQ/MassTransit events and background workers.
- A dedicated cloud deployment path is not currently implemented in the same way as the monolith ECS setup.

## Style

Keep explanations practical and repository-specific.

Avoid enterprise buzzwords unless they describe the actual implementation.

If the code is missing, inconclusive, or inconsistent, say so explicitly.
