# Monolith skill

Use this skill when working under `apps/monolith`.

## Overview

`apps/monolith` contains the modular monolith implementation of Contoso University.

The main solution is:

```text
apps/monolith/ContosoUniversity.sln
```

The main executable application is:

```text
apps/monolith/src/ContosoUniversity.Mvc
```

## Architecture

The monolith is a single deployable ASP.NET Core MVC application.

It is internally split into:

- `ContosoUniversity.Mvc` — MVC web application.
- `ContosoUniversity.Application` — application contracts, commands, queries, validation behavior.
- `ContosoUniversity.Domain` — domain entities and behavior.
- `ContosoUniversity.Data` — shared EF Core / SQL Server infrastructure.
- `ContosoUniversity.Data.Courses.Reads`
- `ContosoUniversity.Data.Courses.Writes`
- `ContosoUniversity.Data.Departments.Reads`
- `ContosoUniversity.Data.Departments.Writes`
- `ContosoUniversity.Data.Students.Reads`
- `ContosoUniversity.Data.Students.Writes`

The MVC application registers the application and data modules in the same process.

Do not describe this implementation as multiple runtime services.

## Runtime

The local Docker Compose setup includes:

- `web`
- `mssql`
- `mssql-migrator`
- `aspire-dashboard`

The web container depends on:

- healthy SQL Server;
- successfully completed database migrator.

The MVC application exposes:

```text
/health/readiness
/health/liveness
```

## Data access

The implementation uses SQL Server and EF Core.

Read/write access is separated by named connection strings:

```text
Courses-RO
Courses-RW
Students-RO
Students-RW
Departments-RO
Departments-RW
```

Common SQL Server connection settings are composed through `SqlConnectionStringBuilder`.

## Observability

The monolith contains OpenTelemetry and Serilog configuration.

The local compose setup includes Aspire Dashboard as an OTLP endpoint.

When changing health endpoints, telemetry, or logging, check:

- `Program.cs`;
- Docker Compose environment variables;
- ECS deployment environment variables.

## Deployment

The monolith has an AWS ECS/Fargate QA deployment path.

Relevant files:

```text
.github/workflows/deploy-mnlth-prepare-artifacts.yml
.github/workflows/deploy-mnlth-qa-aws-ecs-run.yml
.github/workflows/deploy-mnlth-qa-aws-ecs-stop.yml
apps/monolith/iac/envs/qa-aws-ecs
apps/monolith/iac/modules/networking
```

The deployment uses Docker images published to GitHub Container Registry:

```text
ghcr.io/alexbohomol/cuweb
ghcr.io/alexbohomol/mssql-migrator
```

The SQL Server deployment is container-based and runs in ECS/Fargate. Do not describe it as RDS.

## Local setup

Typical local startup:

Navigate to `apps/monolith` folder and run
```bash
docker compose up --build --wait
```

The MVC application is mapped to:

```text
http://localhost:10000
https://localhost:10001
```

SQL Server is mapped to:

```text
localhost,1477
```

## Testing

The solution contains unit, integration, e2e, and system tests.

Prefer:

- domain behavior -> unit tests;
- MVC/database integration -> integration tests;
- browser/user workflows -> e2e tests.
