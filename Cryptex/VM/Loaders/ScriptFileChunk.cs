namespace Cryptex.VM.Loaders;

internal unsafe struct ScriptFileChunk
{
    public string                       ChunkName    { get; }
    public ScriptFileChunkInstruction[] Instructions { get; }

    public ScriptFileChunk()
        : this("INVALID", new[] { new ScriptFileChunkInstruction() }) { }

    private ScriptFileChunk(string name, ScriptFileChunkInstruction[] instructions)
    {
        if (name.Length >= LoaderConstants.CHUNK_NAME_SIZE || instructions.Length >= LoaderConstants.MAX_CHUNK_INSTRUCTIONS)
            throw new ArgumentException("Supplied arguments to the script file chunk are out of their range!");

        ChunkName    = name;
        Instructions = new ScriptFileChunkInstruction[instructions.Length];
        Array.Copy(instructions, Instructions, Instructions.Length);
    }

    public static ScriptFileChunk? LoadFromBytes(byte[] data, int start, out int actualSize)
    {
        actualSize = 0;
        int nameLen = BitConverter.ToInt32(data, start);
        start      += sizeof(int);
        actualSize += sizeof(int);
        string name = BitConverter.ToString(data, start, nameLen);
        start      += nameLen;
        actualSize += nameLen;

        int instrCount = BitConverter.ToInt32(data, start);
        if (instrCount >= LoaderConstants.MAX_CHUNK_INSTRUCTIONS)
            return null;

        start      += sizeof(int);
        actualSize += sizeof(int);

        ScriptFileChunkInstruction[] instructions = new ScriptFileChunkInstruction[instrCount];

        for (int i = 0; i < instrCount; i++)
        {
            var ins = ScriptFileChunkInstruction.LoadFromBytes(data, start, out int size);
            if (ins is not null)
                instructions[i] = ins.Value;

            start      += size;
            actualSize += size;
        }

        return new ScriptFileChunk(name, instructions);
    }
}
