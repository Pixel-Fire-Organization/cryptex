using Cryptex.Exceptions;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.VM.Execution.Instructions.MemoryInstructions;

internal sealed class LoadInstruction : IInstruction
{

    public void Execute(ScriptInstruction c, Executor vm)
    {
        if (c.Args.Length != 2)
            throw new VmRuntimeException(ErrorCodes.Vm2002IncorrectAmountOfArgumentsSuppliedToInstruction);

        if (c.Args[0].Type != InstructionArgumentType.MemoryAddress)
            throw new VmRuntimeException(ErrorCodes.Vm2003InvalidArgumentTypeSpecifiedForInstruction);

        var destSlot = c.Args[0].Value;

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
                vm.GetMemory().SetSlot(destSlot, vm.GetConstant(source.Value));
                break;

            default:
                throw new VmRuntimeException(ErrorCodes.Vm2003InvalidArgumentTypeSpecifiedForInstruction);
        }
    }
}

