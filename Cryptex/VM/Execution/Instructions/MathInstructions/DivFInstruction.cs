using Cryptex.Exceptions;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.VM.Execution.Instructions.MathInstructions;

/// <summary>
///     <c>divf $A, $B</c> — divides the decimal value at slot <c>$A</c> by the decimal value at slot <c>$B</c>.
///     Both slots must hold float-format values (string contains a decimal point).
///     Result is stored in <c>$A</c>, formatted with two decimal places.
/// </summary>
internal sealed class DivFInstruction : IInstruction
{
    public OpCodes OpCode => OpCodes.DivF;

    public void Execute(ScriptInstruction c, Executor vm)
    {
        if (c.Args.Length != 2)
            throw new VMRuntimeException(ErrorCodes.VM2002_IncorrectAmountOfArgumentsSuppliedToInstruction);

        if (c.Args[0].Type != InstructionArgumentType.MemoryAddress ||
            c.Args[1].Type != InstructionArgumentType.MemoryAddress)
            throw new VMRuntimeException(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction);

        var memory = vm.GetMemory();
        var a = memory.GetSlot(c.Args[0].Value);
        var b = memory.GetSlot(c.Args[1].Value);

        if (a.IsUndefined || b.IsUndefined)
            throw new VMRuntimeException(ErrorCodes.VM2007_InvalidMemoryLocationSpecifiedAsArgument);

        if (!a.IsFloat || !b.IsFloat)
            throw new VMRuntimeException(ErrorCodes.VM2011_InvalidDataTypeAtSpecifiedLocation);

        var divisor = b.AsFloat();
        if (divisor == 0m)
            throw new VMRuntimeException(ErrorCodes.VM2011_InvalidDataTypeAtSpecifiedLocation);

        // Round to two decimal places to produce a stable, compact string representation.
        var result = Math.Round(a.AsFloat() / divisor, 2, MidpointRounding.AwayFromZero);
        memory.SetSlot(c.Args[0].Value, VMValue.FromFloat(result));
    }
}