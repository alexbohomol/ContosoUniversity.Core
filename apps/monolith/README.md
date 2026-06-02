# Contoso University — Monolith

This directory contains the modular monolith implementation of Contoso University.

It is a single deployable ASP.NET Core MVC application, but the code is split into application, domain, data infrastructure, and feature-oriented read/write data-access projects. The implementation is useful for practicing modular monolith design, EF Core data access, validation, MediatR-based application workflows, integration testing, observability, Docker-based local environments, and AWS ECS/Fargate deployment automation.

## Solution layout

```text
apps/monolith
├── ContosoUniversity.sln
├── docker-compose.yml
├── docker-compose.override.yml
├── iac
│   ├── envs
│   │   ├── qa-aws-ebt
│   │   └── qa-aws-ecs
│   └── modules/networking
├── src
│   ├── ContosoUniversity.Mvc
│   ├── ContosoUniversity.Application
│   ├── ContosoUniversity.Domain
│   ├── ContosoUniversity.Data
│   ├── ContosoUniversity.Data.Courses.Reads
│   ├── ContosoUniversity.Data.Courses.Writes
│   ├── ContosoUniversity.Data.Departments.Reads
│   ├── ContosoUniversity.Data.Departments.Writes
│   ├── ContosoUniversity.Data.Students.Reads
│   └── ContosoUniversity.Data.Students.Writes
└── test
    ├── unit
    ├── integration
    ├── e2e
    └── system
```

## Architecture overview

The monolith has one web entry point: `ContosoUniversity.Mvc`.

Internally, the solution is organized into several layers:

- `ContosoUniversity.Mvc` — ASP.NET Core MVC application with controllers, views, view models, filters, exception handling, health endpoints, and Docker packaging.
- `ContosoUniversity.Application` — application-level contracts, commands, queries, MediatR handlers, and validation behavior.
- `ContosoUniversity.Domain` — domain entities and domain behavior for courses, departments, instructors, students, enrollments, and related concepts.
- `ContosoUniversity.Data` — shared EF Core / SQL Server infrastructure and connection string composition.
- `ContosoUniversity.Data.*.Reads` — read-side data access modules.
- `ContosoUniversity.Data.*.Writes` — write-side data access modules.

The MVC application registers all data modules in the same process. Controllers use MediatR commands/queries and repositories to execute application workflows.

## Runtime dependencies

The local Docker Compose environment contains:

- `web` — the MVC application.
- `mssql` — SQL Server container.
- `mssql-migrator` — database initialization / migration container built from the repository `database` directory.
- `aspire-dashboard` — .NET Aspire dashboard used as an OTLP endpoint in the local compose setup.

The web container depends on a healthy SQL Server container and a successfully completed migrator container before starting.

## Configuration

The MVC application uses named connection strings for read/write access by domain area:

```text
Courses-RO
Courses-RW
Students-RO
Students-RW
Departments-RO
Departments-RW
```

Common SQL Server options are composed through the `SqlConnectionStringBuilder` configuration section. In Docker Compose, `SqlConnectionStringBuilder__DataSource` is overridden to point to the `mssql` service.

The application exposes health endpoints:

```text
/health/readiness
/health/liveness
```

## Local setup

### Prerequisites

- .NET SDK compatible with the solution target framework.
- Docker with Docker Compose support.
- Local developer HTTPS certificate tooling if HTTPS container access is needed.

### Prepare a development certificate

The Dockerfile expects a certificate file at:

```text
apps/monolith/src/ContosoUniversity.Mvc/cert.pfx
```

Generate it with `dotnet dev-certs` from the MVC project directory. Use the same certificate password as the one configured for the local Docker Compose environment, or change the compose environment variable accordingly.

```bash
cd apps/monolith/src/ContosoUniversity.Mvc
```
```bash
dotnet dev-certs https -ep cert.pfx -p Test1234!
```

### Start the local environment

From `apps/monolith`:

```bash
docker compose up --build --wait
```

The compose override maps the MVC application to:

```text
http://localhost:10000
https://localhost:10001
```

SQL Server is exposed on the host as:

```text
localhost,1477
```

### Stop the local environment

```bash
docker compose down
```

To remove volumes as well:

```bash
docker compose down -v
```

## Running tests

From `apps/monolith`:

```bash
dotnet restore
dotnet build --no-restore
dotnet test --no-build
```

The solution contains unit, integration, e2e, and system test projects. Integration tests use ASP.NET Core test infrastructure and SQL Server test containers. Acceptance tests use Playwright, NUnit, SpecFlow, and Docker-based infrastructure.

## CI

The monolith PR-check workflow builds the solution, runs unit tests, validates whitespace/style/analyzer formatting with `dotnet format`, and runs integration tests.

The workflow is scoped to changes under:

```text
apps/monolith/**
database/**
.github/**
```

## Deployment

The monolith includes an AWS ECS/Fargate QA deployment path.

### Artifact workflow

The GitHub Actions workflow `MNLTH / Deploy / Artifacts` builds and pushes Docker images to GitHub Container Registry:

```text
ghcr.io/alexbohomol/cuweb
ghcr.io/alexbohomol/mssql-migrator
```

Images are tagged with a short Git commit SHA. The default branch also receives the `latest` tag.

### Infrastructure

Terraform configuration for the QA environment is located under:

```text
apps/monolith/iac/envs/qa-aws-ecs
```

It defines:

- VPC and public subnets through the `networking` module.
- ECS cluster.
- ECS/Fargate task definitions for `web`, `mssql`, and `mssql-migrator`.
- ECS services for the web application and SQL Server container.
- CloudWatch log groups.
- EFS storage for ASP.NET Core Data Protection keys.
- Cloud Map service discovery for SQL Server.
- Application Load Balancer.
- HTTP and HTTPS listeners.
- HTTP-to-HTTPS redirect.
- Route 53 alias record for the configured domain.

The default domain configured in Terraform is:

```text
qa-aws-ecs.mnlth.university.contoso.name
```

### Run workflow

The workflow `MNLTH / qa-aws-ecs / Run` is manually triggered and can:

- resolve the image tag to deploy;
- start the SQL Server ECS service;
- run the database migrator as a one-off ECS task;
- start or update the web ECS service;
- scale the web service through the `web_desired_count` input.

It also supports an `update_web_only` mode for updating only the web service.

### Stop workflow

The workflow `MNLTH / qa-aws-ecs / Stop` is manually triggered and scales down:

```text
contoso-mnlth-web-service   -> desired-count 0
contoso-mnlth-mssql-service -> desired-count 0
```

This makes the QA environment suitable for temporary use and cost control.

## Notes

The SQL Server deployment in this archetype is intentionally container-based and runs in ECS/Fargate. It is useful for experimentation and short-lived environments, but it should not be confused with a managed database service such as Amazon RDS.
