# Audit workflow

Audit maintained developer documentation and agent instructions against the repository's current implementation. Discovery must be exhaustive within the requested scope, but the audit remains non-mutating except for its report.

## Scope

Inspect, when relevant:

- root and implementation README files;
- all `AGENTS.md` files and compatibility adapters;
- all repository skills and their referenced workflows;
- other maintained Markdown documentation;
- solutions, projects, package references, source layout, and deployable units;
- `.editorconfig`, `Directory.Build.props`, Dockerfiles, Compose files, environment configuration, and infrastructure definitions;
- test projects, frameworks, fixtures, and infrastructure profiles;
- GitHub Actions workflows, triggers, path filters, jobs, matrices, quality gates, and deployment behavior.

For `audit --changed <base>`, start with the Git diff against `<base>`. Expand beyond the diff only to validate facts transitively affected by those changes. Record the supplied base in the report.

## Evidence collection

Run `node tools/agent/validate-agent-docs.mjs` as the deterministic baseline. Use additional read-only or non-destructive commands to verify claims where useful. Do not run full test suites, delete Docker volumes, deploy, or perform other expensive or state-changing operations without separate authorization.

Check at least:

- referenced files, directories, solutions, projects, links, and Markdown anchors;
- names of services, containers, images, packages, frameworks, and technologies;
- target frameworks and shared build or style policies;
- restore, build, test, format, Docker, and deployment commands;
- ports, endpoints, health checks, dependencies, environment variables, certificates, SQL Server, and RabbitMQ configuration;
- no-container, Testcontainers, and Docker Compose test workflows;
- architectural boundaries and documented runtime topology;
- CI and deployment claims versus executable workflows and infrastructure;
- stale terminology, duplicated facts, conflicting sources of truth, and missing operational guidance that materially affects autonomous agent work.

Distinguish accuracy problems from stylistic preferences. Do not create a finding solely because prose could be worded differently.

Do not assume either side of a mismatch is correct. Classify the recommended direction as:

- update documentation;
- change implementation or configuration;
- owner decision required.

Use `owner decision required` when repository evidence cannot establish intent.

## Priorities

- `P0`: instructions are undiscoverable or can cause unsafe, destructive, or fundamentally invalid agent behavior.
- `P1`: material drift can cause failed builds/tests, incorrect changes, misleading architecture decisions, or missed quality gates.
- `P2`: maintainability, duplication, ambiguity, or efficiency issues that do not normally block correct work immediately.

## Report

Write `.agent-reports/documentation-audit.md`, replacing the previous report. Include:

1. audit date, scope, base revision when applicable, and commands executed;
2. executive summary;
3. confirmed findings;
4. findings requiring more evidence or an owner decision;
5. important missing documentation;
6. duplication and source-of-truth conflicts;
7. checked areas with no findings;
8. recommended remediation order;
9. compact fix manifest.

For every finding record:

- stable ID such as `DOC-P1-001`;
- status: `open`, `approved`, `fixed`, `verified`, `blocked`, or `rejected`;
- priority and category;
- exact files and line references;
- documented claim and observed repository state;
- concrete evidence;
- recommended direction;
- developer and agent impact;
- confidence: `high`, `medium`, or `low`;
- expected remediation scope and validation.

Preserve an existing finding's ID when the same discrepancy is found again. Do not silently discard unresolved findings: mark findings that no longer apply as `rejected` and explain why.

Return a concise chat summary with finding counts by priority, the report path, validation limitations, and the IDs recommended for the first remediation pass.
