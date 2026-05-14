# GitHub Actions skill

Use this skill when creating, changing, or reviewing GitHub Actions workflows.

## General principles

Keep monolith and microservices workflows separate unless there is a clear reason to share logic.

Respect path filters.

Avoid triggering expensive workflows for unrelated changes.

Prefer explicit working directories based on repository variables.

## Existing solution path variables

The repository uses variables such as:

```text
MONOLITH_SLN_PATH
MSERVICES_SLN_PATH
```

Use the appropriate one for each implementation.

## Monolith PR checks

The monolith PR-check workflow is scoped to changes under:

```text
apps/monolith/**
database/**
.github/**
```

It performs:

- checkout;
- .NET setup;
- restore;
- build;
- unit tests;
- format checks;
- integration tests.

## Microservices PR checks

The microservices PR-check workflow is scoped to changes under:

```text
apps/mservices/**
database/**
.github/**
```

It performs:

- checkout;
- .NET setup;
- restore;
- build;
- unit tests;
- format checks;
- integration tests for MVC, APIs, and workers.

## Formatting

The repository uses strict formatting/analyzer checks.

Expected commands include:

```bash
dotnet format whitespace --no-restore --exclude ./src/*/Migrations --verify-no-changes
dotnet format style --no-restore --exclude ./src/*/Migrations --verify-no-changes
dotnet format analyzers --no-restore --exclude ./src/*/Migrations --verify-no-changes
```

## Deployment workflows

The monolith has deployment workflows for:

- building and pushing Docker images;
- running the QA AWS ECS/Fargate environment;
- stopping the QA AWS ECS/Fargate environment.

When working with deployment workflows, check:

- image names;
- tags;
- registry login;
- AWS credentials;
- ECS task definition family;
- container names;
- service names;
- cluster names;
- desired counts;
- wait flags.

## Security

Do not hardcode secrets in workflow files.

Use GitHub Secrets or repository/environment variables.

Do not expose credentials in logs.

Use placeholders in documentation.
