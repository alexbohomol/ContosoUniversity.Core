# Documentation skill

Use this skill when creating or updating README files, architecture notes, setup guides, deployment notes, PR descriptions, ADRs, or inline documentation.

## Language

Documentation must be written in English.

Conversation with the repository owner may happen in Ukrainian, but committed documentation should be English unless explicitly requested otherwise.

## Style

Write documentation for a developer who opens the repository for the first time.

Prefer concise but useful explanations.

Avoid marketing language.

Avoid vague claims like "enterprise-grade", "production-ready", or "highly scalable" unless the repository contains concrete implementation evidence.

It is acceptable and useful to say that this is a pet project / playground.

## Factuality

Documentation must be based on the current repository structure and code.

Do not describe planned functionality as existing functionality.

If one implementation has a deployment path and another does not, state that clearly.

If a setup step depends on local secrets, certificates, or credentials, use placeholders instead of real-looking values.

## README structure

For root README files, include:

- project purpose;
- repository layout;
- available implementations;
- technology highlights;
- local development pointers;
- CI/testing overview;
- deployment overview;
- links to implementation-specific README files.

For implementation README files, include:

- purpose of this implementation;
- solution layout;
- architecture overview;
- runtime dependencies;
- configuration;
- local setup;
- testing;
- CI;
- deployment notes, if applicable.

## Links

Prefer relative links inside the repository.

Examples:

```md
[Monolith](apps/monolith/README.md)
[Microservices](apps/mservices/README.md)
```

## Security

Do not document real secrets, real passwords, real tokens, or real credentials.

When commands require sensitive values, use placeholders:

```bash
dotnet dev-certs https -ep cert.pfx -p <local-dev-certificate-password>
```
