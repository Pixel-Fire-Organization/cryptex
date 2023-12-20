namespace Cryptex.VM.Loaders;

internal sealed class ScriptFile
{
    public int               VMVersion      { get; }
    public string            ScriptName     { get; }
    public string            EntryPointName { get; }
    public ScriptFileChunk[] Chunks         { get; }

    private ScriptFile(int version, string name, string entry, ScriptFileChunk[] chunks)
    {
        VMVersion      = version;
        ScriptName     = name;
        EntryPointName = entry;
        Chunks         = new ScriptFileChunk[chunks.Length];
        Array.Copy(chunks, Chunks, Chunks.Length);
    }

    public static ScriptFile LoadFromBytes(byte[] data)
    {
        int counter = 0;

        int vmVersion = BitConverter.ToInt32(data, counter);
        counter   += sizeof(int);

        int nameLen = BitConverter.ToInt32(data, counter);
        counter += sizeof(int);
        string name = BitConverter.ToString(data, counter, nameLen);
        counter += nameLen;

        int entryLen = BitConverter.ToInt32(data, counter);
        counter += sizeof(int);
        string entry = BitConverter.ToString(data, counter, entryLen);
        counter += entryLen;

        int chunkLen = BitConverter.ToInt32(data, counter);
        counter += sizeof(int);

        var chunks = new ScriptFileChunk[chunkLen];
        for (int i = 0; i < chunkLen; i++)
        {
            var chunk = ScriptFileChunk.LoadFromBytes(data, counter, out int size);
            if (chunk is not null)
                chunks[i] = chunk.Value;

            counter += size;
        }

        return new ScriptFile(vmVersion, name, entry, chunks);
    }
}
