using Cryptex.Exceptions;
using Cryptex.VM.Execution.OperationCodes;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.VM.Execution.Instructions.LogicInstructions;

internal sealed class CmpInstruction : IInstruction
{
    internal CmpInstruction(int scriptVersion) { }
    public OpCodes OpCode => OpCodes.Cmp;

    public void Execute(ScriptInstruction c, Executor vm)
    {
        if (c.Args.Length != 2)
            throw new VMRuntimeException(ErrorCodes.VM2002_IncorrectAmountOfArgumentsSuppliedToInstruction);

        if (c.Args[0].Type != InstructionArgumentType.MemoryAddress ||
            c.Args[1].Type != InstructionArgumentType.MemoryAddress)
            throw new VMRuntimeException(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction);

        var aVal = vm.GetMemory().GetSlot(c.Args[0].Value);
        var bVal = vm.GetMemory().GetSlot(c.Args[1].Value);

        if (aVal.Kind != bVal.Kind || (!aVal.IsInteger && !aVal.IsFloat))
            throw new VMRuntimeException(ErrorCodes.VM2011_InvalidDataTypeAtSpecifiedLocation);

        int cmp;
        if (aVal.IsInteger)
            cmp = aVal.AsInteger().CompareTo(bVal.AsInteger());
        else
            cmp = aVal.AsFloat().CompareTo(bVal.AsFloat());

        vm.SetCompareFlag(cmp == 0 ? CompareFlag.Equals
                        : cmp > 0  ? CompareFlag.Greater
                                   : CompareFlag.Less);
    }
}

