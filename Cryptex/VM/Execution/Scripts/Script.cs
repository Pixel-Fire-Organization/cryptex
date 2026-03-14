using Cryptex.Exceptions;
using MessagePack;

namespace Cryptex.VM.Execution.Scripts;

[MessagePackObject(keyAsPropertyName: true)]
public sealed class Script
{
    public int VMVersion { get; set; }
    public string ScriptName { get; set; }
    public string EntryPointName { get; set; }
    public ScriptChunk[] Chunks { get; set; }

    public Script(string scriptName, ScriptChunk[] chunks)
        : this(scriptName, "main", chunks)
    {
    }

    public Script(string scriptName, string entryPointName, ScriptChunk[] chunks)
    {
        ScriptName = scriptName;
        EntryPointName = entryPointName;

        Chunks = new ScriptChunk[chunks.Length];
        for (int i = 0; i < chunks.Length; i++)
        {
            Chunks[i] = chunks[i];
        }
    }

    internal ScriptChunk? GetChunk(string chunkName) => Chunks.FirstOrDefault(chunk => chunk.ChunkName == chunkName);

    internal void Execute(Executor vm, string chunkName = "main")
    {
        ScriptChunk? chunk = GetChunk(chunkName);
        if (chunk is not null)
            chunk.Execute(vm);
        else
            throw new VMRuntimeException(ErrorCodes.VM2000_NoChunkFoundToExecute);
    }
}