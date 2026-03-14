using Cryptex.VM.Execution;

namespace Cryptex.VM.Loaders;

internal struct ScriptFileChunkInstruction
{
    public OpCodes InstructionOpCode    { get; }
    public string  InstructionArguments { get; }

    public ScriptFileChunkInstruction()
        : this(OpCodes.Term, "") { }

    private ScriptFileChunkInstruction(OpCodes code, string args)
    {
        InstructionOpCode    = code;
        InstructionArguments = args;
    }

    public static ScriptFileChunkInstruction? LoadFromBytes(byte[] data, int start, out int actualSize)
    {
        actualSize = 0;
        OpCodes opCode = (OpCodes)BitConverter.ToInt32(data, start);
        start      += sizeof(byte);
        actualSize += sizeof(byte);

        int argsLen = BitConverter.ToInt32(data, start);
        start      += sizeof(int);
        actualSize += sizeof(int);

        if (argsLen >= LoaderConstants.INSTRUCTION_ARG_SIZE)
            return null;

        string args = BitConverter.ToString(data, start, argsLen);
        actualSize += argsLen;

        return new ScriptFileChunkInstruction(opCode, args);
    }
}
