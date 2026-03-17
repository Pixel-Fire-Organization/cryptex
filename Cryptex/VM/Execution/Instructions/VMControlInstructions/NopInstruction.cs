using Cryptex.Exceptions;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.VM.Execution.Instructions.VMControlInstructions;

internal sealed class NopInstruction : IInstruction
{
    internal NopInstruction() { }

    public void Execute(ScriptInstruction c, Executor vm)
    {
        if (c.Args.Length != 1)
            throw new VmRuntimeException(ErrorCodes.VM2002_IncorrectAmountOfArgumentsSuppliedToInstruction);

        if (c.Args[0].Type != InstructionArgumentType.Constant)
            throw new VmRuntimeException(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction);

        var ms = vm.GetConstant(c.Args[0].Value);
        if (!ms.IsInteger)
            throw new VmRuntimeException(ErrorCodes.VM2011_InvalidDataTypeAtSpecifiedLocation);

        var msValue = ms.AsInteger();
        if (msValue < 0 || msValue > int.MaxValue)
            throw new VmRuntimeException(ErrorCodes.VM2012_InstructionArgumentIsOutOfRange);

        Thread.Sleep((int)msValue);
    }
}

