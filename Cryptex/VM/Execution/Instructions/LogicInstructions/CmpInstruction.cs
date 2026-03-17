using Cryptex.Exceptions;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.VM.Execution.Instructions.LogicInstructions;

internal sealed class CmpInstruction : IInstruction
{
    internal CmpInstruction() { }

    public void Execute(ScriptInstruction c, Executor vm)
    {
        if (c.Args.Length != 2)
            throw new VmRuntimeException(ErrorCodes.VM2002_IncorrectAmountOfArgumentsSuppliedToInstruction);

        if (c.Args[0].Type != InstructionArgumentType.MemoryAddress ||
            c.Args[1].Type != InstructionArgumentType.MemoryAddress)
            throw new VmRuntimeException(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction);

        var aVal = vm.GetMemory().GetSlot(c.Args[0].Value);
        var bVal = vm.GetMemory().GetSlot(c.Args[1].Value);

        // ReSharper disable once MergeIntoPattern
        if (aVal.Kind != bVal.Kind || (!aVal.IsInteger && !aVal.IsFloat))
            throw new VmRuntimeException(ErrorCodes.VM2011_InvalidDataTypeAtSpecifiedLocation);

        var cmp = aVal.IsInteger
            ? aVal.AsInteger().CompareTo(bVal.AsInteger())
            : aVal.AsFloat().CompareTo(bVal.AsFloat());

        vm.SetCompareFlag(cmp == 0 ? CompareFlag.Equals
                        : cmp > 0  ? CompareFlag.Greater
                                   : CompareFlag.Less);
    }
}

