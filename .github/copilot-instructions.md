# Copilot Instructions for Cryptex

## Project Overview

Cryptex is a .NET 8.0 virtual machine (VM) and scripting engine library written in C#. The solution consists of four projects:

- **Cryptex** — Core class library implementing the VM execution engine, instruction set, memory model, and data types. Targets `net8.0` with AOT (Ahead-of-Time) compilation support.
- **Cryptex.Test** — xUnit unit test project covering all instructions and VM components.
- **CryptexTester** — Console application for manually exercising the library.
- **CryptexScriptInspector** — WPF desktop application (`net8.0-windows`) providing a GUI for script inspection.

## Architecture

### Core VM (`Cryptex/VM/`)

- **`Execution/`** — `Executor`, `ExecutorMemory`, `Script`, `ScriptChunk`, `ScriptChunkOpCode`, `OpCodes`
- **`Execution/Instructions/`** — Modular instruction implementations grouped by category:
  - `MathInstructions/` — `Add`, `Sub`, `Mul`, `Div`, `Inc`, `Dec` (and float variants `AddF`, `SubF`, etc.)
  - `BitwiseInstructions/` — `And`, `Or`, `Xor`, `Not`, `Shl`, `Shr`
  - `MemoryInstructions/` — `Load`, `Free`
  - `VMControlInstructions/` — `Nop`, `Exit`, `Crash`, `Term`
- **`ExternalExecutor/`** — Delegate-based external function integration
- **`Loaders/`** — `ScriptFileLoader` for loading `.script` files
- **`DataTypes/`** — Type system helpers: `ArrayTypeHelper`, `CryptexDataConverter`

### Error Handling
- `ErrorCodes.cs` — VM error code enumeration
- `Exceptions/` — Custom exceptions: `VMRuntimeException`, `InvalidDataType`, `TerminateInstructionFoundException`

## Technology Stack

| Component | Details |
|-----------|---------|
| Language | C# 12, .NET 8.0 |
| SDK | .NET 10.0 SDK (build/CI) |
| Test Framework | xUnit 2.9.3 |
| Test Coverage | coverlet.collector |
| GUI | WPF (Windows-only) |
| Code Quality | JetBrains ReSharper InspectCode, `.editorconfig` |
| CI/CD | GitHub Actions |
| Compilation | Native AOT (cross-platform: win-x64, linux-x64, osx-arm64, osx-x64) |

## Coding Conventions

- **Nullable reference types** are enabled (`<Nullable>enable</Nullable>`).
- **Implicit usings** are enabled.
- **Unsafe code** is allowed in the `Cryptex` core library.
- Member naming follows the pattern:
  - Private fields: `m_camelCase` (e.g., `m_script`, `m_memory`)
  - Private constants / `internal const`: `UPPER_SNAKE_CASE` (e.g., `MAX_FUNCTION_ARGS`)
  - Public properties and methods: `PascalCase`
- Classes and their members are `sealed` where possible.
- Instruction classes follow a consistent single-responsibility pattern — one class per opcode variant.
- All code style rules are enforced via `.editorconfig`.

## CI/CD Pipelines

| Workflow | Trigger | Purpose |
|----------|---------|---------|
| `.github/workflows/dotnet-build.yml` | Push | Restore, build (`Release`), and test |
| `.github/workflows/inspect-code.yml` | Push | Run ReSharper InspectCode; upload SARIF |
| `.github/workflows/publish-cryptex-lib.yml` | `workflow_dispatch` | AOT-publish for all platforms; create GitHub Release |

- All workflows run inside `mcr.microsoft.com/dotnet/sdk:10.0` container (or `actions/setup-dotnet@v4` for AOT matrix).
- The shared action `.github/actions/resharper-inspect/action.yml` encapsulates ReSharper execution and SARIF upload.
- Build and publish always use `--configuration Release`.

## Testing Conventions

- Every instruction (opcode) has a dedicated test class in `Cryptex.Test/InstructionsTests/`.
- Test class naming: `<Instruction>InstructionTest` (e.g., `AddInstructionTest`).
- Tests construct a `ScriptChunk`, build a `Script`, create an `Executor`, call `BeginExecution()`, and assert on memory slot values via `GetValueInMemory(int)`.
- Each instruction is tested for: correct values, wrong argument types, too few arguments, too many arguments, and no arguments.
- Use `Assert.True`/`Assert.False` for `BeginExecution()` return value; use `Assert.Equal` for memory slot content (always strings).
- No mocking frameworks are used — tests exercise the real VM.

## Key Build Commands

```bash
# Restore dependencies
dotnet restore

# Build in Release mode
dotnet build --configuration Release

# Run all tests
dotnet test --configuration Release

# AOT publish for a specific platform (example: linux-x64)
dotnet publish Cryptex/Cryptex.csproj -c Release -r linux-x64 -p:PublishAOT=true -p:TreatWarningsAsErrors=true
```

## Copilot Skills and Prompts Available

| File | Purpose |
|------|---------|
| `.github/instructions/create-new-instruction.instructions.md` | How to add a new VM instruction |
| `.github/instructions/write-csharp-code.instructions.md` | C# coding standards for this repo |
| `.github/instructions/write-unit-tests.instructions.md` | How to write xUnit tests for instructions |
| `.github/instructions/github-cicd.instructions.md` | CI/CD workflow authoring guidance |
| `.github/prompts/build-project.prompt.md` | Prompt to build the project |
| `.github/prompts/test-project.prompt.md` | Prompt to run all tests |
| `.github/prompts/code-review.prompt.md` | Prompt to perform a code review |
