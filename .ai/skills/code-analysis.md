# Code analysis skill

Use this skill when analyzing implementation details, architecture, dependencies, runtime behavior, CI, Docker, tests, or deployment.

## Required behavior

Before answering questions about the implementation, inspect the relevant files.

Do not answer from memory or generic framework knowledge if the repository can be checked.

Base conclusions on the current repository state.

When analyzing a feature or component, identify:

- the relevant solution;
- the executable project;
- related class libraries;
- configuration files;
- Docker Compose services;
- tests;
- CI workflows;
- deployment files, if any.

## Avoid

Do not say that something is implemented unless it is visible in the repository.

Do not describe intended architecture as actual architecture.

Do not assume that a `.csproj` boundary is a runtime service boundary.

Do not assume that a service is independently deployable unless there is evidence.

Do not infer production-readiness from local Docker Compose.

## When code is inconclusive

If the repository does not contain enough information, say so explicitly.

Use wording such as:

- "I cannot confirm this from the current repository."
- "The code shows X, but I do not see Y implemented."
- "This appears to be intended, but I do not see the runtime wiring."
- "The next files to check would be..."

## Good output structure

For analysis answers, prefer this order:

1. What was checked.
2. What the code shows.
3. What conclusion follows.
4. What remains uncertain.
5. Suggested next step, if needed.
