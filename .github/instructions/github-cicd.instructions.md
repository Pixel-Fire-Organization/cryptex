---
applyTo: ".github/workflows/**,.github/actions/**"
---

# Skill: GitHub CI/CD for Cryptex

Follow this guide when creating or modifying GitHub Actions workflows and composite actions in this repository.

## Existing Workflows

| File | Trigger | Purpose |
|------|---------|---------|
| `dotnet-build.yml` | `push` | Build (`Release`) and run xUnit tests |
| `inspect-code.yml` | `push` | ReSharper InspectCode → SARIF → GitHub Security tab |
| `publish-cryptex-lib.yml` | `workflow_dispatch` | AOT-publish cross-platform binaries; create GitHub Release |

## Shared Composite Action

`.github/actions/resharper-inspect/action.yml` is a reusable composite action that:
1. Caches NuGet packages.
2. Installs and runs JetBrains ReSharper `InspectCode`.
3. Uploads the SARIF report to the GitHub Security tab.
4. Uploads the SARIF as a workflow artifact.

**Always use this action** for any workflow that performs code-quality inspection — do not duplicate the ReSharper steps.

## Runtime Environment

- **Container**: `mcr.microsoft.com/dotnet/sdk:10.0` for Linux jobs that only need the .NET SDK.
- **Native AOT matrix jobs**: use `actions/setup-dotnet@v4` with `dotnet-version: '10.0.x'` because containers are not supported on macOS/Windows runners.
- **Ubuntu runner**: `ubuntu-latest` for all Linux jobs.
- **macOS Intel runner**: `macos-15-intel` for `osx-x64` AOT builds.

## Permissions

Apply the **minimum required permissions** to each workflow:

| Permission | When to add |
|-----------|-------------|
| `contents: read` | Always (checkout) |
| `contents: write` | Only for workflows that create releases or push commits |
| `security-events: write` | Required for SARIF upload via `github/codeql-action/upload-sarif` |
| `actions: read` | Required alongside `security-events: write` for SARIF upload |

## NuGet Caching

Always cache NuGet packages to reduce build times:

```yaml
- name: Cache dotnet packages
  uses: actions/cache@v4
  with:
    path: ~/.nuget/packages
    key: ${{ runner.os }}-dotnet-${{ hashFiles('**/*.csproj', '**/*.sln') }}
    restore-keys: |
      ${{ runner.os }}-dotnet-
```

## Standard Build Steps

```yaml
- name: Run Build and Test
  run: |
    dotnet restore
    dotnet build --no-restore --configuration Release
    dotnet test --no-build --configuration Release
```

## AOT Publish Steps

```yaml
- name: Install Linux AOT dependencies
  if: runner.os == 'Linux'
  run: sudo apt-get update && sudo apt-get install -y clang zlib1g-dev

- name: Publish Cryptex
  run: |
    dotnet publish Cryptex/Cryptex.csproj \
      -c Release \
      -r ${{ matrix.rid }} \
      -p:PublishAOT=true \
      -p:TreatWarningsAsErrors=true \
      --output ./publish-folder/${{ matrix.rid }}
```

## Release Management

- Use the `gh` CLI (available on all GitHub-hosted runners) to create and manage releases.
- Always check whether a release already exists before creating to make the workflow idempotent.
- Include a cleanup job (`if: failure()`) to delete a partially-created release if any publish matrix job fails.

## Adding a New Workflow

1. Create the file under `.github/workflows/<name>.yml`.
2. Set the minimum required `permissions`.
3. Use `actions/checkout@v4` as the first step.
4. Reuse `.github/actions/resharper-inspect` for any code-quality checks.
5. Use `mcr.microsoft.com/dotnet/sdk:10.0` container for Linux-only .NET jobs.
6. Pin all third-party action versions to a specific tag (e.g., `@v4`).

## Adding a New Composite Action

1. Create a directory under `.github/actions/<action-name>/`.
2. Add `action.yml` with `runs.using: "composite"`.
3. Document all `inputs` with `description` and `required` fields.
4. Reference the action from workflows with `uses: ./.github/actions/<action-name>`.
