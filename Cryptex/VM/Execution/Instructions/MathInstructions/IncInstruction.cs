using System.Numerics;
using Cryptex.Exceptions;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.VM.Execution.Instructions.MathInstructions;

/// <summary>
///     <c>inc $A</c> — increments the integer value at slot <c>$A</c> by one.
/// </summary>
internal sealed class IncInstruction : IInstruction
{
    public OpCodes OpCode => OpCodes.Inc;

    public void Execute(ScriptInstruction c, Executor vm)
    {
        if (c.Args.Length != 1 || c.Args[0].Type == InstructionArgumentType.Empty)
            throw new VMRuntimeException(ErrorCodes.VM2002_IncorrectAmountOfArgumentsSuppliedToInstruction);

        if (c.Args[0].Type != InstructionArgumentType.MemoryAddress)
            throw new VMRuntimeException(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction);

        var memory = vm.GetMemory();
        var val = memory.GetSlot(c.Args[0].Value);

        if (val.IsUndefined)
            throw new VMRuntimeException(ErrorCodes.VM2007_InvalidMemoryLocationSpecifiedAsArgument);

        if (!val.IsInteger)
            throw new VMRuntimeException(ErrorCodes.VM2011_InvalidDataTypeAtSpecifiedLocation);

        memory.SetSlot(c.Args[0].Value, VMValue.FromInteger(val.AsInteger() + BigInteger.One));
    }
}