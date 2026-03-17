using Cryptex.Exceptions;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.VM.Execution.Instructions.IntegratedFunctionInstructions;

internal sealed class ReadLineInstruction : IInstruction
{

    public void Execute(ScriptInstruction c, Executor vm)
    {
        if (c.Args.Length != 1)
            throw new VmRuntimeException(ErrorCodes.Vm2002IncorrectAmountOfArgumentsSuppliedToInstruction);

        if (c.Args[0].Type != InstructionArgumentType.MemoryAddress)
            throw new VmRuntimeException(ErrorCodes.Vm2003InvalidArgumentTypeSpecifiedForInstruction);

        var input = Console.ReadLine() ?? string.Empty;
        vm.GetMemory().SetSlot(c.Args[0].Value, VmValue.FromString(input));
    }
}

