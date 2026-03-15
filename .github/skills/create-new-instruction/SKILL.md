---
name: create-new-instruction
description: Guides adding a brand-new opcode/instruction to the Cryptex VM. Use when the user asks to add, implement, or create a new VM instruction, opcode, or operation.
metadata:
  author: Pixel-Fire-Organization
  version: "1.0"
---

# Create a New Instruction

Use this skill when adding a brand-new opcode/instruction to the Cryptex VM.

## Checklist

1. **Define the opcode** in `Cryptex/VM/Execution/OpCodes.cs` — add a new enum member following the existing naming pattern.
2. **Create the instruction class** under the appropriate sub-folder of `Cryptex/VM/Execution/Instructions/`:
   - `MathInstructions/` for arithmetic
   - `BitwiseInstructions/` for bitwise operations
   - `MemoryInstructions/` for memory management
   - `VMControlInstructions/` for VM control flow
   - Create a new sub-folder if the instruction does not fit any existing category.
3. **Implement the instruction class** — follow the single-class-per-opcode pattern used by every existing instruction.
4. **Register the instruction** in the dispatch/execution switch in `Cryptex/VM/Execution/Script.cs` (or wherever opcodes are dispatched).
5. **Add opcode metadata** (description and arguments) to `CryptexScriptInspector/OpCodeDescriptions.cs` and `CryptexScriptInspector/OpCodeArguments.cs` if the instruction should appear in the GUI inspector.
6. **Write unit tests** in `Cryptex.Test/InstructionsTests/<Name>InstructionTest.cs` covering:
   - Correct input values (happy path)
   - Wrong argument types
   - Too few arguments
   - Too many arguments
   - No arguments
   - Any instruction-specific edge cases
7. **Run the full test suite** to verify no regressions: `dotnet test --configuration Release`.
8. **Run the linter** to ensure code style compliance: follow `.editorconfig` rules.

## Naming Conventions

- Opcode enum member: `PascalCase` (e.g., `Mul`, `MulF`, `Shl`)
- Instruction class: `<Name>Instruction` (e.g., `MulInstruction`, `MulFInstruction`)
- Test class: `<Name>InstructionTest` (e.g., `MulInstructionTest`)
- Test file: `Cryptex.Test/InstructionsTests/<Name>InstructionTest.cs`

## Example — Adding a `Mod` (modulo) Instruction

```csharp
// 1. OpCodes.cs
public enum OpCodes
{
    // ... existing entries ...
    Mod,
}

// 2. MathInstructions/ModInstruction.cs
namespace Cryptex.VM.Execution.Instructions.MathInstructions;

internal sealed class ModInstruction
{
    internal static void Execute(ExecutorMemory memory, string args)
    {
        // parse args, validate, operate on memory slots
    }
}
```
