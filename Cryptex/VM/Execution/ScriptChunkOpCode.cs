namespace Cryptex.VM.Execution;

public struct ScriptChunkOpCode
{
    public OpCodes Code { get; }
    public string  Args { get; }

    public ScriptChunkOpCode()
    {
        Code = OpCodes.Crash;
        Args = "INVALID_OPCODE";
    }

    public ScriptChunkOpCode(OpCodes code)
    {
        Code = code;
        Args = string.Empty;
    }
    
    public ScriptChunkOpCode(OpCodes code, string args)
    {
        Code = code;
        Args = args;
    }
}
