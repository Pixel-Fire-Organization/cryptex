using Cryptex.Exceptions;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.VM.Execution.Instructions.IntegratedFunctionInstructions;

internal sealed class RandomFInstruction : IInstruction
{

    public void Execute(ScriptInstruction c, Executor vm)
    {
        if (c.Args.Length != 1)
            throw new VmRuntimeException(ErrorCodes.Vm2002IncorrectAmountOfArgumentsSuppliedToInstruction);

        if (c.Args[0].Type != InstructionArgumentType.MemoryAddress)
            throw new VmRuntimeException(ErrorCodes.Vm2003InvalidArgumentTypeSpecifiedForInstruction);

        var value = (decimal)Random.Shared.NextDouble();
        vm.GetMemory().SetSlot(c.Args[0].Value, VmValue.FromFloat(value));
    }
}

