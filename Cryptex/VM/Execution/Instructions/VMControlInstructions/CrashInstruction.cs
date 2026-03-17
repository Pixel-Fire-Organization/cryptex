using Cryptex.Exceptions;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.VM.Execution.Instructions.VMControlInstructions;

internal sealed class CrashInstruction : IInstruction
{
    internal CrashInstruction() { }

    public void Execute(ScriptInstruction c, Executor vm)
    {
        if (c.Args.Length != 1)
            throw new VmRuntimeException(ErrorCodes.VM2002_IncorrectAmountOfArgumentsSuppliedToInstruction);

        if (c.Args[0].Type != InstructionArgumentType.Constant)
            throw new VmRuntimeException(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction);

        var raw = vm.GetConstant(c.Args[0].Value);
        if (!raw.IsInteger)
            throw new VmRuntimeException(ErrorCodes.VM2011_InvalidDataTypeAtSpecifiedLocation);

        var codeValue = raw.AsInteger();
        if (codeValue < int.MinValue || codeValue > int.MaxValue)
            throw new VmRuntimeException(ErrorCodes.VM2012_InstructionArgumentIsOutOfRange);

        var code = (ErrorCodes)(int)codeValue;
        if (string.IsNullOrEmpty(code.ToMessage()))
            throw new VmRuntimeException(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction);

        throw new VmRuntimeException(code);
    }
}

