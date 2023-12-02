using Cryptex.Exceptions;

namespace Cryptex.VM.Execution;

public sealed class Script
{
    private readonly ScriptChunk[] m_chunks;

    public string Name { get; }

    public Script(string scriptName, ScriptChunk[] chunks)
    {
        Name = scriptName;

        m_chunks = new ScriptChunk[chunks.Length];
        for (int i = 0; i < chunks.Length; i++) { m_chunks[i] = chunks[i]; }
    }

    internal ScriptChunk? GetChunk(string chunkName)
    {
        foreach (var chunk in m_chunks)
            if (chunk.Name == chunkName)
                return chunk;

        return null;
    }

    internal void Execute(Executor vm, string chunkName = "main")
    {
        ScriptChunk? chunk = GetChunk(chunkName);
        if (chunk is not null)
            chunk.Execute(vm);
        else
            throw new VMRuntimeException(ErrorCodes.VM2000_NoChunkFoundToExecute);
    }
}
