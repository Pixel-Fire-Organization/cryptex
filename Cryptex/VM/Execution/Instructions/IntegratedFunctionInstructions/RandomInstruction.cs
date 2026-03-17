using System.Numerics;
using Cryptex.Exceptions;
using Cryptex.VM.Execution.OperationCodes;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.VM.Execution.Instructions.IntegratedFunctionInstructions;

internal sealed class RandomInstruction : IInstruction
{
    public OpCodes OpCode => OpCodes.Random;
    public int ScriptVersion { get; }

    internal RandomInstruction(int scriptVersion) => ScriptVersion = scriptVersion;

    public void Execute(ScriptInstruction c, Executor vm)
    {
        if (c.Args.Length != 1)
            throw new VMRuntimeException(ErrorCodes.VM2002_IncorrectAmountOfArgumentsSuppliedToInstruction);

        if (c.Args[0].Type != InstructionArgumentType.MemoryAddress)
            throw new VMRuntimeException(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction);

        var value = new BigInteger(Random.Shared.NextInt64());
        vm.GetMemory().SetSlot(c.Args[0].Value, VMValue.FromInteger(value));
    }
}

