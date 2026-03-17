using Cryptex.Exceptions;
using Cryptex.VM.Execution.OperationCodes;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.VM.Execution.Instructions.VMControlInstructions;

internal sealed class GetErrorInstruction : IInstruction
{
    internal GetErrorInstruction(int scriptVersion) { }
    public OpCodes OpCode => OpCodes.GetError;

    public void Execute(ScriptInstruction c, Executor vm)
    {
        if (c.Args.Length != 1)
            throw new VMRuntimeException(ErrorCodes.VM2002_IncorrectAmountOfArgumentsSuppliedToInstruction);

        if (c.Args[0].Type != InstructionArgumentType.MemoryAddress)
            throw new VMRuntimeException(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction);

        var errorCode = vm.ConsumeError();
        vm.GetMemory().SetSlot(c.Args[0].Value, VMValue.FromInteger((int)errorCode));
    }
}

