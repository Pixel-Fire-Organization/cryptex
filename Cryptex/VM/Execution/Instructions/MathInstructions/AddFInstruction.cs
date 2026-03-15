using Cryptex.Exceptions;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.VM.Execution.Instructions.MathInstructions;

/// <summary>
///     <c>addf $A, $B</c> — adds the decimal value at slot <c>$B</c> to the decimal value at slot <c>$A</c>.
///     Both slots must hold float-format values (string contains a decimal point).
///     Result is stored in <c>$A</c>.
/// </summary>
internal sealed class AddFInstruction : IInstruction
{
    public OpCodes OpCode => OpCodes.AddF;

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

        memory.SetSlot(c.Args[0].Value, VMValue.FromFloat(a.AsFloat() + b.AsFloat()));
    }
}