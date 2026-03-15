---
name: create-new-instruction
description: Guides adding a brand-new opcode/instruction to the Cryptex VM. Use when the user asks to add, implement, or create a new VM instruction, opcode, or operation.
metadata:
  author: Pixel-Fire-Organization
  version: "1.0"
---

# Create a New Instruction

Use this skill when adding a brand-new opcode/instruction to the Cryptex VM.

> **Before adding a new opcode — ask yourself:** *"Can this operation be expressed by sequencing existing instructions?"*
> If yes, **do not add a new opcode**. Document the composition pattern instead.
>
> ✅ Correct — sum 10 elements: compose `load`, `add`, `jmp`  
> ❌ Wrong — add a `SumArray` opcode  
>
> The VM instruction set is intentionally minimal. New opcodes must represent operations that are **impossible** to achieve by composing existing primitives.

## Checklist

1. **Define the opcode** in `Cryptex/VM/Execution/OpCodes.cs` — add a new enum member (`OpCodes : byte`) following the existing naming pattern and inline comment style.
2. **Create the instruction class** under the appropriate sub-folder of `Cryptex/VM/Execution/Instructions/`:
   - `MathInstructions/` for arithmetic
   - `BitwiseInstructions/` for bitwise operations
   - `MemoryInstructions/` for memory management
   - `VMControlInstructions/` for VM control flow
   - `FunctionInstructions/` for function call/return
   - `LogicInstructions/` for comparison and jump instructions
   - Create a new sub-folder if the instruction does not fit any existing category.
3. **Implement `IInstruction`** — every instruction class must implement `Cryptex.VM.Execution.Instructions.IInstruction`:
   - `OpCodes OpCode { get; }` — return the exact opcode this class handles.
   - `void Execute(ScriptInstruction c, Executor vm)` — perform the instruction logic.
   - Access arguments via `c.Args` (array of `ScriptInstructionArgument`). Each argument has:
     - `int Value` — index into the constants block (for `Constant`), or the memory address (for `MemoryAddress`), or label index (for `Label`).
     - `InstructionArgumentType Type` — `Constant`, `MemoryAddress`, `Label`, or `Empty`.
   - **Performance rules for `Execute()`** (hot path — called for every opcode):
     - No allocations: avoid `new`, LINQ, `string.Format`, or `StringBuilder`.
     - No boxing: do not cast value types to `object`.
     - Validate and read all arguments at the top; throw `VMRuntimeException` immediately on any error.
     - Early exit: never continue execution after detecting an invalid state.
   - **AOT rules** — do not use `dynamic`, reflection, `Activator.CreateInstance`, or any API annotated `[RequiresDynamicCode]` / `[RequiresUnreferencedCode]`.
4. **Register the instruction** in `Cryptex/VM/Execution/OpCodesExtensions.cs` — add a `case` to the `GetByCode()` switch that returns `new YourInstruction()`.
5. **Add opcode metadata** to `CryptexScriptInspector/OpCodeDescriptions.cs` and `CryptexScriptInspector/OpCodeArguments.cs` if the instruction should appear in the GUI inspector.
6. **Add documentation** in `Cryptex/Docs/OpCodes/<Category>/<Name>.md` following the existing template (Signature, Description, Remarks, Example).
7. **Update the opcode table** in `Cryptex/Docs/OpCodes/OpCodes.md` — mark `Implemented: ✔` and set the `Since (VM Version)`.
8. **Write unit tests** in `Cryptex.Test/InstructionsTests/<Name>InstructionTest.cs` covering:
   - Correct input values (happy path)
   - Wrong argument types
   - Too few arguments
   - Too many arguments
   - No arguments
   - Any instruction-specific edge cases
9. **Run the full test suite** to verify no regressions: `dotnet test --configuration Release`.
10. **Run `TreatWarningsAsErrors` build** — the final gate before merging:
    ```bash
    dotnet build --configuration Release -p:TreatWarningsAsErrors=true
    ```
    This surfaces AOT-incompatible API usage, nullable warnings, and trim-unsafe calls. All warnings must be resolved.
11. **Run the linter** to ensure code style compliance: follow `.editorconfig` rules.

## Naming Conventions

- Opcode enum member: `PascalCase` (e.g., `Mul`, `MulF`, `AddImm`, `Shl`)
- Instruction class: `<Name>Instruction` (e.g., `MulInstruction`, `AddImmInstruction`)
- Test class: `<Name>InstructionTest` (e.g., `MulInstructionTest`)
- Test file: `Cryptex.Test/InstructionsTests/<Name>InstructionTest.cs`
- Doc file: `Cryptex/Docs/OpCodes/<Category>/<Name>.md`

## Argument Types Reference

| `InstructionArgumentType` | Meaning | `Value` interpretation |
|--------------------------|---------|------------------------|
| `Constant` | A literal constant from the Constants Block | Index into the script's constants table |
| `MemoryAddress` | A memory slot (`$A`) | Integer memory slot index |
| `Label` | A jump target label (`L`) | Index into the Jump Block |
| `Empty` | No argument / placeholder | Ignored |

## Example — Adding a `Mod` (modulo) Instruction

```csharp
// 1. OpCodes.cs — add the enum member
public enum OpCodes : byte
{
    // ... existing entries ...
    Mod, //[mod $1, $2 | $1 = $1 % $2]  Modulo of two integers; result in $1.
}

// 2. MathInstructions/ModInstruction.cs
namespace Cryptex.VM.Execution.Instructions.MathInstructions;

internal sealed class ModInstruction : IInstruction
{
    public OpCodes OpCode => OpCodes.Mod;

    public void Execute(ScriptInstruction c, Executor vm)
    {
        // Validate argument count and types, then operate on memory
        if (c.Args.Length != 2)
            throw new VMRuntimeException(ErrorCodes.VM2002_IncorrectAmountOfArgumentsSuppliedToInstruction);
        if (c.Args[0].Type != InstructionArgumentType.MemoryAddress ||
            c.Args[1].Type != InstructionArgumentType.MemoryAddress)
            throw new VMRuntimeException(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction);

        ExecutorMemory memory = vm.GetMemory();
        // ... read slots, perform modulo, write result back ...
    }
}

// 3. OpCodesExtensions.cs — register it
case OpCodes.Mod:
    return new ModInstruction();
```

