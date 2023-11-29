namespace Cryptex.VM.Execution.OpCodeLogic;

internal sealed class ArgInstruction : IInstruction
{
    public  OpCodes        OpCode => OpCodes.Arg;
    
    public object? Execute(ScriptChunkOpCode c, ExecutorMemory memory)
    {
        return null;
    }
}
