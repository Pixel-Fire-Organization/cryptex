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

## Standard Test Pattern

Every instruction test follows this four-step pattern:

```csharp
// 1. Build script
ScriptChunk mainChunk = new ScriptChunk("main", new[]
{
    new ScriptChunkOpCode(OpCodes.Load, "$1, #5"),
    new ScriptChunkOpCode(OpCodes.Load, "$2, #6"),
    new ScriptChunkOpCode(OpCodes.Add,  "$1, $2"),
});
Script script = new Script("script", new[] { mainChunk });

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
| Wrong argument type (e.g., literal `#n` where `$slot` expected) | `false` | Memory unchanged |
| Too few arguments | `false` | Memory unchanged |
| Too many arguments | `false` | Memory unchanged |
| No arguments (empty string) | `false` | Memory unchanged |
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

- Memory slots are referenced as `$<n>` (e.g., `$1`, `$2`) in script arguments.
- Literal integer values are prefixed with `#` (e.g., `#5`, `#42`).
- Literal float values use `.` decimal separator (e.g., `#5.25`).
- `GetValueInMemory(int)` returns the slot value as a `string?`; always assert `NotNull` before asserting the value.
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
