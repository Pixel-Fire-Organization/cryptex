using Cryptex.Exceptions;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.VM.Execution.Instructions.LogicInstructions;

internal sealed class CmpInstruction : IInstruction
{
    public void Execute(ScriptInstruction c, Executor vm)
    {
        if (c.Args.Length != 2)
            throw new VmRuntimeException(ErrorCodes.Vm2002IncorrectAmountOfArgumentsSuppliedToInstruction);

        if (c.Args[0].Type != InstructionArgumentType.MemoryAddress ||
            c.Args[1].Type != InstructionArgumentType.MemoryAddress)
            throw new VmRuntimeException(ErrorCodes.Vm2003InvalidArgumentTypeSpecifiedForInstruction);

        var aVal = vm.GetMemory().GetSlot(c.Args[0].Value);
        var bVal = vm.GetMemory().GetSlot(c.Args[1].Value);

        if (aVal.Kind is not (VmValueKind.Integer or VmValueKind.Float) || aVal.Kind != bVal.Kind)
            throw new VmRuntimeException(ErrorCodes.Vm2011InvalidDataTypeAtSpecifiedLocation);

        var cmp = aVal.IsInteger
            ? aVal.AsInteger().CompareTo(bVal.AsInteger())
            : aVal.AsFloat().CompareTo(bVal.AsFloat());

        vm.SetCompareFlag(cmp == 0 ? CompareFlag.Equals
                        : cmp > 0  ? CompareFlag.Greater
                                   : CompareFlag.Less);
    }
}
