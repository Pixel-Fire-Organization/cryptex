using System.Numerics;
using Cryptex.Exceptions;
using Cryptex.VM.Execution.OperationCodes;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.VM.Execution.Instructions.IntegratedFunctionInstructions;

internal sealed class ReadInstruction : IInstruction
{
    public OpCodes OpCode => OpCodes.Read;
    public int ScriptVersion { get; }

    internal ReadInstruction(int scriptVersion) => ScriptVersion = scriptVersion;

    public void Execute(ScriptInstruction c, Executor vm)
    {
        if (c.Args.Length != 1)
            throw new VMRuntimeException(ErrorCodes.VM2002_IncorrectAmountOfArgumentsSuppliedToInstruction);

        if (c.Args[0].Type != InstructionArgumentType.MemoryAddress)
            throw new VMRuntimeException(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction);

        var input = Console.ReadLine() ?? string.Empty;
        if (!BigInteger.TryParse(input, out var value))
        {
            vm.SetError(ErrorCodes.VM2014_InvalidInputProvided);
            return;
        }

        vm.GetMemory().SetSlot(c.Args[0].Value, VMValue.FromInteger(value));
    }
}

