using Cryptex.Exceptions;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.VM.Execution.Instructions.LogicInstructions;

internal sealed class JmpInstruction : IInstruction
{

    public void Execute(ScriptInstruction c, Executor vm)
    {
        if (c.Args.Length != 1)
            throw new VmRuntimeException(ErrorCodes.Vm2002IncorrectAmountOfArgumentsSuppliedToInstruction);

        if (c.Args[0].Type != InstructionArgumentType.Label)
            throw new VmRuntimeException(ErrorCodes.Vm2003InvalidArgumentTypeSpecifiedForInstruction);

        vm.RequestJump(c.Args[0].Value);
    }
}

