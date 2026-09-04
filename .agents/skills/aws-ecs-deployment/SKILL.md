---
name: aws-ecs-deployment
description: Analyze, change, validate, or document the monolith QA deployment on AWS ECS/Fargate. Use for its Terraform, deployment workflows, task definitions, services, networking, images, DNS, logging, or persistent storage.
---

# AWS ECS deployment

This workflow applies to the monolith QA ECS/Fargate path. It does not imply that the microservices implementation has an equivalent cloud deployment.

Read before changing deployment behavior:

- `apps/monolith/AGENTS.md` and `apps/monolith/README.md`;
- `apps/monolith/iac/envs/qa-aws-ecs`;
- relevant modules under `apps/monolith/iac/modules`;
- `.github/workflows/deploy-mnlth-*.yml`.

Current invariants to verify against code:

- web and SQL Server run as ECS/Fargate services;
- the migrator runs as a one-off task;
- SQL Server is container-based, not RDS;
- Cloud Map supplies internal database discovery;
- the web target uses `/health/readiness`;
- EFS stores ASP.NET Core Data Protection keys;
- application and migrator images are published to GHCR.

Start with non-mutating checks such as `terraform fmt -check` and `terraform validate` in the environment directory when tooling and initialization are available. Review image tags, task families, container names, cluster/service names, desired counts, health checks, DNS, certificates, security groups, and secrets as one connected deployment contract.

Do not provision, deploy, scale, stop, dispose, or otherwise mutate AWS or GitHub state without explicit user authorization. Distinguish current implementation from proposed production improvements.

