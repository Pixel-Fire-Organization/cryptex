using Cryptex.Exceptions;
using Cryptex.VM.Execution.OperationCodes;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.VM.Execution.Instructions.IntegratedFunctionInstructions;

internal sealed class RandomFInstruction : IInstruction
{
    public OpCodes OpCode => OpCodes.RandomF;
    public int ScriptVersion { get; }

    internal RandomFInstruction(int scriptVersion) => ScriptVersion = scriptVersion;

    public void Execute(ScriptInstruction c, Executor vm)
    {
        if (c.Args.Length != 1)
            throw new VMRuntimeException(ErrorCodes.VM2002_IncorrectAmountOfArgumentsSuppliedToInstruction);

        if (c.Args[0].Type != InstructionArgumentType.MemoryAddress)
            throw new VMRuntimeException(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction);

        var value = (decimal)Random.Shared.NextDouble();
        vm.GetMemory().SetSlot(c.Args[0].Value, VMValue.FromFloat(value));
    }
}

