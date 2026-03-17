using Cryptex.Exceptions;
using Cryptex.VM.Execution.OperationCodes;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.VM.Execution.Instructions.MathInstructions;

internal sealed class AddImmInstruction : IInstruction
{
    internal AddImmInstruction(int scriptVersion) { }
    public OpCodes OpCode => OpCodes.AddImm;

    public void Execute(ScriptInstruction c, Executor vm)
    {
        if (c.Args.Length != 2)
            throw new VMRuntimeException(ErrorCodes.VM2002_IncorrectAmountOfArgumentsSuppliedToInstruction);

        if (c.Args[0].Type != InstructionArgumentType.MemoryAddress ||
            c.Args[1].Type != InstructionArgumentType.Constant)
            throw new VMRuntimeException(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction);

        var aVal = vm.GetMemory().GetSlot(c.Args[0].Value);
        var xVal = vm.GetConstant(c.Args[1].Value);

        if (!aVal.IsInteger || !xVal.IsInteger)
            throw new VMRuntimeException(ErrorCodes.VM2011_InvalidDataTypeAtSpecifiedLocation);

        vm.GetMemory().SetSlot(c.Args[0].Value, VMValue.FromInteger(aVal.AsInteger() + xVal.AsInteger()));
    }
}

