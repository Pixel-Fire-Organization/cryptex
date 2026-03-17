using Cryptex.Exceptions;
using Cryptex.VM.Execution.OperationCodes;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.VM.Execution.Instructions.LogicInstructions;

internal sealed class JnqInstruction : IInstruction
{
    internal JnqInstruction(int scriptVersion) { }
    public OpCodes OpCode => OpCodes.Jnq;

    public void Execute(ScriptInstruction c, Executor vm)
    {
        if (c.Args.Length != 1)
            throw new VMRuntimeException(ErrorCodes.VM2002_IncorrectAmountOfArgumentsSuppliedToInstruction);

        if (c.Args[0].Type != InstructionArgumentType.Label)
            throw new VMRuntimeException(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction);

        var flag = vm.GetCompareFlag();
        vm.ClearCompareFlag();

        if (flag != CompareFlag.Equals && flag != CompareFlag.None)
            vm.RequestJump(c.Args[0].Value);
    }
}

