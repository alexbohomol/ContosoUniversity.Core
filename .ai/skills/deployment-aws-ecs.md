# AWS ECS deployment skill

Use this skill when working with the monolith AWS ECS/Fargate deployment.

## Scope

This skill applies to:

```text
apps/monolith
```

The microservices implementation does not currently have an equivalent cloud deployment path in the repository.

## Deployment model

The monolith QA deployment uses:

- GitHub Actions;
- GitHub Container Registry;
- Terraform;
- AWS ECS/Fargate;
- Application Load Balancer;
- Route 53;
- ACM certificate;
- CloudWatch Logs;
- EFS;
- Cloud Map service discovery;
- SQL Server container in ECS.

## Artifact workflow

The artifact workflow builds and pushes:

```text
ghcr.io/alexbohomol/cuweb
ghcr.io/alexbohomol/mssql-migrator
```

Images are tagged with:

- short Git SHA;
- `latest` for the default branch.

## Terraform environment

Terraform environment:

```text
apps/monolith/iac/envs/qa-aws-ecs
```

Networking module:

```text
apps/monolith/iac/modules/networking
```

The default app name is:

```text
contoso-mnlth
```

The default AWS region is:

```text
eu-central-1
```

The default configured domain is:

```text
qa-aws-ecs.mnlth.university.contoso.name
```

## ECS services

The environment defines ECS services for:

```text
contoso-mnlth-web-service
contoso-mnlth-mssql-service
```

The database migrator is run as a one-off ECS task.

## SQL Server

SQL Server is deployed as a container in ECS/Fargate.

Do not describe the current implementation as RDS.

Cloud Map provides the internal database DNS name:

```text
mssql.contoso.local
```

## Web service

The web service is exposed through an Application Load Balancer.

The target group health check path is:

```text
/health/readiness
```

The web task mounts EFS for ASP.NET Core Data Protection keys at:

```text
/var/dpkeys
```

## Run workflow

The run workflow can:

- resolve image tag;
- start SQL Server service;
- run database migrator;
- start or update web service;
- set web desired count;
- optionally update only the web service.

## Stop workflow

The stop workflow scales down:

```text
contoso-mnlth-web-service
contoso-mnlth-mssql-service
```

to desired count `0`.

## Safety rules

Be careful with security group changes.

Do not open public database access unless there is an explicit reason.

Do not hardcode AWS credentials, database credentials, tokens, or certificate material.

Use variables and secrets.

If proposing production improvements, distinguish them from the current implementation.
