using Cryptex.Exceptions;
using Cryptex.VM.Execution.OperationCodes;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.VM.Execution.Instructions.MathInstructions;

internal sealed class IncFInstruction : IInstruction
{
    public OpCodes OpCode => OpCodes.IncF;
    public int ScriptVersion { get; }

    internal IncFInstruction(int scriptVersion) => ScriptVersion = scriptVersion;

    public void Execute(ScriptInstruction c, Executor vm)
    {
        if (c.Args.Length != 1)
            throw new VMRuntimeException(ErrorCodes.VM2002_IncorrectAmountOfArgumentsSuppliedToInstruction);

        if (c.Args[0].Type != InstructionArgumentType.MemoryAddress)
            throw new VMRuntimeException(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction);

        var val = vm.GetMemory().GetSlot(c.Args[0].Value);
        if (!val.IsFloat)
            throw new VMRuntimeException(ErrorCodes.VM2011_InvalidDataTypeAtSpecifiedLocation);

        vm.GetMemory().SetSlot(c.Args[0].Value, VMValue.FromFloat(val.AsFloat() + 1m));
    }
}

