using Cryptex.Exceptions;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.VM.Execution.Instructions.MemoryInstructions;

/// <summary>
///     <c>load $A, $B | load $A, #V | load $A, %H | load $A</c>
/// </summary>
internal sealed class LoadInstruction : IInstruction
{
    public OpCodes OpCode => OpCodes.Load;

    public void Execute(ScriptInstruction c, Executor vm)
    {
        if (c.Args.Length is 0 or > 2)
            throw new VMRuntimeException(ErrorCodes.VM2002_IncorrectAmountOfArgumentsSuppliedToInstruction);

        if (c.Args[0].Type != InstructionArgumentType.MemoryAddress)
            throw new VMRuntimeException(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction);

        var memory = vm.GetMemory();
        var targetSlot = c.Args[0].Value;

        // Single-argument form: free the slot (equivalent to `free $A`).
        if (c.Args.Length == 1)
        {
            memory.RemoveSlot(targetSlot);
            return;
        }

        switch (c.Args[1].Type)
        {
            case InstructionArgumentType.MemoryAddress:
                var srcVal = memory.GetSlot(c.Args[1].Value);
                if (srcVal.IsUndefined)
                    memory.RemoveSlot(targetSlot);
                else
                    memory.SetSlot(targetSlot, srcVal);
                break;

            // Decimal and hex constants are both pre-parsed into VMValue by the argument
            // parser. GetConstantOrThrow propagates any deferred error (e.g. %5.5).
            case InstructionArgumentType.Constant:
            case InstructionArgumentType.HexConstant:
                var constVal = vm.GetConstantOrThrow(in c, c.Args[1].Value);
                memory.SetSlot(targetSlot, constVal);
                break;

            default:
                throw new VMRuntimeException(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction);
        }
    }
}