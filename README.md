# Contoso University

Contoso University is a pet project based on the canonical ASP.NET / Entity Framework demo application. The original sample has been evolved into a modern .NET codebase that is useful for experimenting with application architecture, data access patterns, integration testing, observability, containerization, CI, and deployment automation in a deliberately small domain.

The repository contains two functionally similar implementations of the same university information system:

- [Monolith](apps/monolith/README.md) — an ASP.NET Core MVC modular monolith.
- [Microservices](apps/mservices/README.md) — a microservices-style implementation with separate APIs, background workers, HTTP clients, and RabbitMQ messaging.

Both implementations model the same broad Contoso University domain: courses, departments, instructors, students, enrollments, and related administrative workflows. The purpose of the repository is not to provide a production-ready university system, but to keep a compact playground where architectural and infrastructure ideas can be tested without the weight of a real enterprise application.

## Repository layout

```text
.
├── apps
│   ├── monolith      # Modular monolith implementation
│   └── mservices     # Microservices-style implementation
├── database          # SQL Server initialization / migration scripts
├── definitions.json  # RabbitMQ definitions used by the microservices setup
└── rabbitmq.conf     # RabbitMQ configuration used by the microservices setup
```

## Implementations

### Monolith

The monolith is a single deployable ASP.NET Core MVC application split internally into application, domain, data, and read/write data-access modules. It uses SQL Server, EF Core, MediatR, FluentValidation, OpenTelemetry, Serilog, Docker Compose, and GitHub Actions. It also contains infrastructure-as-code for deploying a QA environment to AWS ECS/Fargate.

Read more: [apps/monolith/README.md](apps/monolith/README.md)

### Microservices

The microservices implementation keeps the MVC frontend but moves backend responsibilities into separate services: `Courses.Api`, `Departments.Api`, `Students.Api`, and matching workers for asynchronous cross-domain consistency. Services communicate through HTTP and RabbitMQ events via MassTransit. The local environment is orchestrated with Docker Compose and includes SQL Server, RabbitMQ, APIs, workers, and the MVC frontend.

Read more: [apps/mservices/README.md](apps/mservices/README.md)

## Technology highlights

The repository currently uses:

- .NET 10 target framework across the application solutions.
- ASP.NET Core MVC and Minimal APIs.
- Entity Framework Core with SQL Server.
- MediatR and FluentValidation for application workflows and validation.
- Docker Compose for local multi-container environments.
- RabbitMQ and MassTransit in the microservices implementation.
- OpenTelemetry and Serilog in the monolith implementation.
- GitHub Actions for build, style, unit, integration, and deployment workflows.
- Terraform for the monolith AWS ECS/Fargate QA environment.

## Local development

Each implementation has its own solution, Docker Compose setup, and README:

- [Monolith local setup](apps/monolith/README.md#local-setup)
- [Microservices local setup](apps/mservices/README.md#local-setup)

At a high level, the expected prerequisites are:

- .NET SDK compatible with the repository target framework.
- Docker with Docker Compose support.
- A developer HTTPS certificate when running the MVC application in containers.

## CI and quality gates

The repository contains separate GitHub Actions workflows for the two implementations:

- Monolith PR checks build the solution, run unit tests, validate formatting/style/analyzers, and run integration tests.
- Microservices PR checks build the solution, run unit tests, validate formatting/style/analyzers, and run integration tests for the MVC frontend, APIs, and workers.

Warnings and code analysis are treated strictly in the solution-level build configuration.

## Deployment

The monolith contains an AWS ECS/Fargate QA deployment path with Docker images published to GitHub Container Registry and infrastructure defined in Terraform. See [Monolith deployment](apps/monolith/README.md#deployment).

The microservices implementation currently has a complete Docker Compose local runtime and CI checks. A cloud deployment path is not described in the repository in the same way as the monolith ECS setup. See [Microservices deployment notes](apps/mservices/README.md#deployment-notes).

## Origin

The starting point for this project is the canonical Contoso University tutorial application. This repository continues that idea as a modernized, experimental .NET playground rather than a direct tutorial copy.
