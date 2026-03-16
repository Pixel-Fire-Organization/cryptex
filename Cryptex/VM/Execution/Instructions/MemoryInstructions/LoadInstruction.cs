using Cryptex.Exceptions;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.VM.Execution.Instructions.MemoryInstructions;

internal sealed class LoadInstruction : IInstruction
{
    public OpCodes OpCode => OpCodes.Load;

    public void Execute(ScriptInstruction c, Executor vm)
    {
        if (c.Args.Length == 0 || c.Args.Length > 2)
            throw new VMRuntimeException(ErrorCodes.VM2002_IncorrectAmountOfArgumentsSuppliedToInstruction);

        if (c.Args[0].Type != InstructionArgumentType.MemoryAddress)
            throw new VMRuntimeException(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction);

        var destSlot = c.Args[0].Value;

        if (c.Args.Length == 1)
        {
            vm.GetMemory().RemoveSlot(destSlot);
            return;
        }

        var source = c.Args[1];
        switch (source.Type)
        {
            case InstructionArgumentType.MemoryAddress:
                var memVal = vm.GetMemory().GetSlot(source.Value);
                if (memVal.IsUndefined)
                    vm.GetMemory().RemoveSlot(destSlot);
                else
                    vm.GetMemory().SetSlot(destSlot, memVal);
                break;

            case InstructionArgumentType.Constant:
            case InstructionArgumentType.HexConstant:
                vm.GetMemory().SetSlot(destSlot, vm.GetConstant(source.Value));
                break;

            default:
                throw new VMRuntimeException(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction);
        }
    }
}

