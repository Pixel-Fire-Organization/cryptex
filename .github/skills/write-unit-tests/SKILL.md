---
name: write-unit-tests
description: Guides writing xUnit unit tests for Cryptex VM instructions. Use when creating or updating test files in the Cryptex.Test project, or when asked to add tests for an instruction.
metadata:
  author: Pixel-Fire-Organization
  version: "1.0"
---

# Write Unit Tests for Cryptex

Follow this guide when writing or updating xUnit tests in the `Cryptex.Test` project.

## Test Framework

- **xUnit 2.9.3** — use `[Fact]` for parameterless tests and `[Theory]` + `[InlineData]` for data-driven tests.
- **coverlet.collector** is configured for coverage; no extra setup needed.
- No mocking framework is used — test against the real VM.

## File and Class Structure

| Convention | Example |
|-----------|---------|
| Test file location | `Cryptex.Test/InstructionsTests/<Name>InstructionTest.cs` |
| Test class name | `<Name>InstructionTest` (e.g., `AddInstructionTest`) |
| Test class modifier | `public sealed class` |
| Namespace | `Cryptex.Test.InstructionsTests` |
| Test method name | `Test<Instruction>_<Scenario>` (e.g., `TestAdd_CorrectValues`) |

## Script Model Overview

Scripts are built from structured objects — not text strings:

- `ScriptInstruction(OpCodes code, ScriptInstructionArgument[] args)` — represents a single instruction.
- `ScriptInstructionArgument(int value, InstructionArgumentType type)` — a typed argument.
  - `InstructionArgumentType.MemoryAddress` — `value` is the memory slot index (e.g., `1` for `$1`).
  - `InstructionArgumentType.Constant` — `value` is the index into the script's constants table.
  - `InstructionArgumentType.Label` — `value` is the index into the jump block.
  - `InstructionArgumentType.Empty` — no argument (use `ScriptInstructionArgument.DEFAULT`).
- `ScriptChunk(string chunkName, ScriptInstruction[] instructions)`
- `Script(string scriptName, ScriptChunk[] chunks)`

## Standard Test Pattern

Every instruction test follows this four-step pattern:

```csharp
// 1. Build script
ScriptInstruction[] instructions =
[
    new ScriptInstruction(OpCodes.LoadImm,
        [new ScriptInstructionArgument(1, InstructionArgumentType.MemoryAddress),
         new ScriptInstructionArgument(5, InstructionArgumentType.Constant)]),
    new ScriptInstruction(OpCodes.LoadImm,
        [new ScriptInstructionArgument(2, InstructionArgumentType.MemoryAddress),
         new ScriptInstructionArgument(6, InstructionArgumentType.Constant)]),
    new ScriptInstruction(OpCodes.Add,
        [new ScriptInstructionArgument(1, InstructionArgumentType.MemoryAddress),
         new ScriptInstructionArgument(2, InstructionArgumentType.MemoryAddress)]),
];

ScriptChunk mainChunk = new ScriptChunk("main", instructions);
Script script = new Script("script", [mainChunk]);

// 2. Execute
Executor executor = new Executor(script);
Assert.True(executor.BeginExecution());   // or Assert.False for error cases

// 3. Read memory
string? memoryValue1 = executor.GetValueInMemory(1);
string? memoryValue2 = executor.GetValueInMemory(2);

// 4. Assert
Assert.NotNull(memoryValue1);
Assert.Equal("11", memoryValue1);
```

## Required Test Cases for Every Instruction

Each instruction (and each variant, e.g., `Add` and `AddF`) must have tests for:

| Scenario | Expected `BeginExecution()` result | Notes |
|----------|------------------------------------|-------|
| Correct values (happy path) | `true` | Verify output memory slot(s) |
| Wrong argument type (e.g., `Constant` where `MemoryAddress` is expected) | `false` | Memory unchanged |
| Too few arguments | `false` | Memory unchanged |
| Too many arguments | `false` | Memory unchanged |
| No arguments (empty args array) | `false` | Memory unchanged |
| Instruction-specific edge cases | varies | e.g., division by zero, overflow |

## Naming Test Methods

Use the pattern `Test<Instruction>_<Scenario>`:

```
TestAdd_CorrectValues
TestAddf_CorrectValues
TestAdd_ArgumentNotMemory
TestAdd_TooFewArguments
TestAdd_TooMuchArguments
TestAdd_NoArguments
```

## Memory Slot Conventions

- Memory slots are addressed by integer index (e.g., slot `1`, slot `2`) in `ScriptInstructionArgument`.
- `GetValueInMemory(int)` returns the slot value as a `string?`; always assert `NotNull` before asserting the value.
- Integer results are returned as decimal strings (e.g., `"11"`).
- Float results are formatted to two decimal places (e.g., `"11.50"`).

## GlobalUsings

The `Cryptex.Test` project uses `GlobalUsings.cs` to import common namespaces. Check it before adding redundant `using` directives.

## Running Tests

```bash
# Run all tests
dotnet test --configuration Release

# Run a specific test class
dotnet test --configuration Release --filter "FullyQualifiedName~AddInstructionTest"

# Run a single test method
dotnet test --configuration Release --filter "FullyQualifiedName~AddInstructionTest.TestAdd_CorrectValues"
```

