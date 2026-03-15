# Copilot Instructions for Cryptex

## Project Overview

Cryptex is a .NET 8.0 virtual machine (VM) and scripting engine library written in C#. The solution consists of four projects:

- **Cryptex** — Core class library implementing the VM execution engine, instruction set, memory model, and data types. Targets `net8.0` with AOT (Ahead-of-Time) compilation support.
- **Cryptex.Test** — xUnit unit test project covering all instructions and VM components.
- **CryptexTester** — Console application for manually exercising the library.
- **CryptexScriptInspector** — WPF desktop application (`net8.0-windows`) providing a GUI for script inspection.

## Architecture

### Core VM (`Cryptex/VM/`)

- **`Execution/`** — `Executor`, `ExecutorMemory`, `OpCodes`, `OpCodesExtensions`
- **`Execution/Scripts/`** — Script model:
  - `Script` — top-level container (`[MessagePackObject]`), holds `ScriptChunk[]`, `ScriptName`, `EntryPointName`, `VMVersion`
  - `ScriptChunk` — named block of `ScriptInstruction[]`
  - `ScriptInstruction` — a single opcode + `ScriptInstructionArgument[]`
  - `ScriptInstructionArgument` — typed argument: `Value` (int) + `InstructionArgumentType` (`Empty`, `Constant`, `MemoryAddress`, `Label`)
  - `Loaders/ScriptFileLoader` — deserializes binary MessagePack `.script` files
- **`Execution/Instructions/`** — All instructions implement `IInstruction` (`OpCode` property + `Execute(ScriptInstruction, Executor)` method), grouped by category:
  - `MathInstructions/` — `Add`, `Sub`, `Mul`, `Div`, `Inc`, `Dec` (and float variants `AddF`, `SubF`, etc.; and immediate variants `AddImm`, `SubImm`, `MulImm`, `DivImm`, `Mod`, `ModImm`)
  - `BitwiseInstructions/` — `And`, `Or`, `Xor`, `Not`, `Shl`, `Shr`
  - `MemoryInstructions/` — `Load`, `LoadImm`, `Free`, `Reg`, `UnReg`
  - `VMControlInstructions/` — `Nop`, `Exit`, `Crash`, `Term`, `GetError`
  - `FunctionInstructions/` — `Arg`, `Exec`, `Call`, `Ret`, `Res`
  - `LogicInstructions/` — `Cmp`, `Jmp`, `Jeq`, `Jnq`, `Jls`, `Jgr`, `Jge`, `Jle`
- **`ExternalExecutor/`** — Delegate-based external function integration
- **Argument notation** (used in opcode docs and assembly):
  - `X, Y, Z` — constants fetched from the **Constants Block** (integer index into the script's constant table)
  - `$A, $B, $C` — memory address arguments (`InstructionArgumentType.MemoryAddress`)
  - `L` — label fetched from the **Jump Block** (`InstructionArgumentType.Label`)
- **VM Flags** — two runtime flags managed by the executor:
  - `Compare` — set by `cmp`; read and cleared by jump instructions; values: `None`, `Equals`, `NotEquals`, `Greater`, `Less`, `GreaterEquals`, `LessEquals`
  - `Error` — set by any instruction on data error; cleared/transferred to memory by `geterror`

### Serialization
- Scripts are serialized/deserialized using **MessagePack** (`MessagePack` NuGet package v3.1.4).
- All script model classes carry `[MessagePackObject(keyAsPropertyName: true)]` attributes.
- Use `ScriptFileLoader.LoadScript(byte[])` or `ScriptFileLoader.LoadScript(string path)` to load scripts.

### Documentation
- `Cryptex/Docs/` — per-opcode Markdown reference pages, organized by category (VM, Math, Memory, Bitwise, Logic, Function, String, Array).
- `Cryptex/Docs/OpCodes/OpCodes.md` — master opcode table with implementation status and VM version.
- `Cryptex/Docs/VM/VM Flags.md` — flag lifecycle documentation.
- `Cryptex/Docs/ErrorCodes.md` — all VM error codes and their messages.

### Error Handling
- `ErrorCodes.cs` — VM error code enumeration (e.g., `VM2002_IncorrectAmountOfArgumentsSuppliedToInstruction`)
- `Exceptions/` — Custom exceptions: `VMRuntimeException`, `InvalidDataType`, `TerminateInstructionFoundException`, `RequiredMessageNotFoundException`

## Technology Stack

| Component | Details |
|-----------|---------|
| Language | C# 12, .NET 8.0 |
| SDK | .NET 10.0 SDK (build/CI) |
| Test Framework | xUnit 2.9.3 |
| Test Coverage | coverlet.collector |
| Serialization | MessagePack v3.1.4 (binary script format) |
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

### SOLID Design

All C# code in `Cryptex` must follow SOLID principles:

- **Single Responsibility** — each class has exactly one reason to change. Instruction classes encapsulate exactly one opcode's behaviour.
- **Open/Closed** — add new behaviour by adding new classes; do not modify existing instruction implementations to support unrelated operations.
- **Liskov Substitution** — every `IInstruction` implementation must be substitutable for any other; the executor must not down-cast or type-check instances.
- **Interface Segregation** — `IInstruction` is intentionally minimal (`OpCode` + `Execute`). Do not bloat it.
- **Dependency Inversion** — instructions depend on `Executor` (abstraction), not on concrete memory dictionaries or internal VM state directly.

### AOT Compatibility

The `Cryptex` library is published with Native AOT (`dotnet publish -p:PublishAOT=true`). The following are **strictly forbidden**:

- `System.Reflection` APIs (`Type.GetMethod`, `MethodInfo.Invoke`, `Activator.CreateInstance`, etc.)
- `dynamic` keyword
- `System.Reflection.Emit` (no runtime IL generation)
- `System.Linq.Expressions` that compile to delegates at runtime
- Unbound generic type operations that cannot be resolved at compile time
- Any attribute or API that requires a runtime metadata manifest not trimmed by the AOT linker

When in doubt, check whether the API is tagged `[RequiresUnreferencedCode]` or `[RequiresDynamicCode]` in the .NET docs — if it is, do not use it.

### Instruction Reuse and Composition

**Never add a new opcode to implement a compound operation** that can already be expressed using existing opcodes. The VM instruction set is intentionally minimal; complex algorithms must be composed from primitives.

> ✅ Correct — sum 10 elements: use `load`, `add`, `jmp`  
> ❌ Wrong — add a `SumArray` opcode to the VM

When reviewing a proposed new opcode ask: *"Can this be achieved by sequencing existing instructions?"*. If yes, reject the new opcode and document the composition pattern instead.

### Instruction Performance

Instructions execute on every VM tick and lie in the hot path. Follow these rules:

- **No allocations in `Execute`** — avoid `new`, LINQ, `string.Format`, or `StringBuilder` inside `Execute()`; pre-compute or cache anything reusable.
- **No boxing** — do not cast value types to `object` inside `Execute()`.
- **Parse once** — read and validate arguments at the top of `Execute()`; do not re-read them in multiple branches.
- **Early exit on error** — throw `VMRuntimeException` as soon as invalid state is detected; do not continue partial execution.

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
- Tests construct `ScriptInstruction[]` (with `ScriptInstructionArgument[]`), build a `ScriptChunk` and `Script`, create an `Executor`, call `BeginExecution()`, and assert on memory slot values via `GetValueInMemory(int)`.
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

# Final verification — build with TreatWarningsAsErrors to surface AOT and nullable warnings
dotnet build --configuration Release -p:TreatWarningsAsErrors=true

# AOT publish for a specific platform (example: linux-x64)
dotnet publish Cryptex/Cryptex.csproj -c Release -r linux-x64 -p:PublishAOT=true -p:TreatWarningsAsErrors=true
```

> **Important:** Always run the `TreatWarningsAsErrors` build as the final step before considering any C# change complete. AOT-incompatible APIs, unused nullability suppressions, and trim-unsafe calls will surface here and must be resolved before merging.

## Copilot Skills and Prompts Available

| File | Purpose |
|------|---------|
| `.github/skills/create-new-instruction/SKILL.md` | How to add a new VM instruction |
| `.github/skills/write-csharp-code/SKILL.md` | C# coding standards for this repo |
| `.github/skills/write-unit-tests/SKILL.md` | How to write xUnit tests for instructions |
| `.github/skills/github-cicd/SKILL.md` | CI/CD workflow authoring guidance |
| `.github/prompts/build-project.prompt.md` | Prompt to build the project |
| `.github/prompts/test-project.prompt.md` | Prompt to run all tests |
| `.github/prompts/code-review.prompt.md` | Prompt to perform a code review |
