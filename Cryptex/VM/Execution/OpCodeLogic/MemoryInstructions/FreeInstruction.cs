using Cryptex.VM.Execution.DataTypes;

namespace Cryptex.VM.Execution.OpCodeLogic.MemoryInstructions;

internal sealed class FreeInstruction : IInstruction
{
    public OpCodes OpCode => OpCodes.Free;

    public object? Execute(ScriptChunkOpCode c, ExecutorMemory memory)
    {
        if (c.Code != OpCode)
            ErrorList.WriteError(ErrorCodes.VM2001_WrongOpCodePassedForScriptOpCode, fatal: true);

        string[]? args = CryptexDataConverter.SplitInstructionArguments(c.Args, 1);
        if (args is null)
            return null;

        //ARG1

        string argument1 = args[0];
        if (!argument1.StartsWith(IInstruction.MEMORY_ADDRESS_PREFIX))
            ErrorList.WriteError(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction, fatal: true);

        int location1 = CryptexDataConverter.ParseArgumentToMemoryLocation(argument1);
        
        if(!CryptexDataConverter.IsValidMemoryLocation(memory, location1))
            ErrorList.WriteError(ErrorCodes.VM2007_InvalidMemoryLocationSpecifiedAsArgument);

        memory.RemoveSlot(location1);

        return null;
    }
}
