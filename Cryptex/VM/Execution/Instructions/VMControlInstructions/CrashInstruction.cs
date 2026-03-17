using Cryptex.Exceptions;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.VM.Execution.Instructions.VMControlInstructions;

internal sealed class CrashInstruction : IInstruction
{

    public void Execute(ScriptInstruction c, Executor vm)
    {
        if (c.Args.Length != 1)
            throw new VmRuntimeException(ErrorCodes.Vm2002IncorrectAmountOfArgumentsSuppliedToInstruction);

        if (c.Args[0].Type != InstructionArgumentType.Constant)
            throw new VmRuntimeException(ErrorCodes.Vm2003InvalidArgumentTypeSpecifiedForInstruction);

        var raw = vm.GetConstant(c.Args[0].Value);
        if (!raw.IsInteger)
            throw new VmRuntimeException(ErrorCodes.Vm2011InvalidDataTypeAtSpecifiedLocation);

        var codeValue = raw.AsInteger();
        if (codeValue < int.MinValue || codeValue > int.MaxValue)
            throw new VmRuntimeException(ErrorCodes.Vm2012InstructionArgumentIsOutOfRange);

        var code = (ErrorCodes)(int)codeValue;
        if (string.IsNullOrEmpty(code.ToMessage()))
            throw new VmRuntimeException(ErrorCodes.Vm2003InvalidArgumentTypeSpecifiedForInstruction);

        throw new VmRuntimeException(code);
    }
}

