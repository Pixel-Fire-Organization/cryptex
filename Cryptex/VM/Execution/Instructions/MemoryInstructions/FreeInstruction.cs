using Cryptex.Exceptions;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.VM.Execution.Instructions.MemoryInstructions;

/// <summary>
///     <c>free $A</c> — removes the value at memory slot <c>$A</c>.
///     Throws if the slot does not exist.
/// </summary>
internal sealed class FreeInstruction : IInstruction
{
    public OpCodes OpCode => OpCodes.Free;

    public void Execute(ScriptInstruction c, Executor vm)
    {
        if (c.Args.Length != 1 || c.Args[0].Type == InstructionArgumentType.Empty)
            throw new VMRuntimeException(ErrorCodes.VM2002_IncorrectAmountOfArgumentsSuppliedToInstruction);

        if (c.Args[0].Type != InstructionArgumentType.MemoryAddress)
            throw new VMRuntimeException(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction);

        var removed = vm.GetMemory().RemoveSlot(c.Args[0].Value);

        if (removed.IsUndefined)
            throw new VMRuntimeException(ErrorCodes.VM2007_InvalidMemoryLocationSpecifiedAsArgument);
    }
}