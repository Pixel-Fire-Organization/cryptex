using Cryptex.Exceptions;
using Cryptex.VM.Execution.DataTypes;

namespace Cryptex.VM.Execution.OpCodeLogic.MemoryInstructions;

internal sealed class FreeInstruction : IInstruction
{
    public OpCodes OpCode => OpCodes.Free;

    public void Execute(ScriptChunkOpCode c, Executor vm)
    {
        if (c.Code != OpCode)
            throw new VMRuntimeException(ErrorCodes.VM2001_WrongOpCodePassedForScriptOpCode);

        string[] args = CryptexDataConverter.SplitInstructionArguments(c.Args, 1);

        //ARG1

        string argument1 = args[0];
        if (!argument1.StartsWith(IInstruction.MEMORY_ADDRESS_PREFIX))
            throw new VMRuntimeException(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction);

        int location1 = CryptexDataConverter.ParseArgumentToMemoryLocation(argument1);
        
        if(!CryptexDataConverter.IsValidMemoryLocation(vm.GetMemory(), location1))
            throw new VMRuntimeException(ErrorCodes.VM2007_InvalidMemoryLocationSpecifiedAsArgument);

        vm.GetMemory().RemoveSlot(location1);
    }
}
