using Cryptex.Exceptions;
using MessagePack;

namespace Cryptex.VM.Execution.Scripts;

/// <summary>
///     Top-level container for a Cryptex script.
///     Holds the named <see cref="ScriptChunk" /> array, the pre-parsed
///     <see cref="ConstantsBlock" />, and execution metadata.
/// </summary>
[MessagePackObject(true)]
public sealed class Script
{
    public Script()
        : this("DEFAULT_SCRIPT", Executor.VM_VERSION, "main", [], [])
    {
    }

    public Script(string scriptName, ScriptChunk[] chunks)
        : this(scriptName, Executor.VM_VERSION, "main", chunks, [])
    {
    }

    public Script(string scriptName, ScriptChunk[] chunks, VMValue[] constants)
        : this(scriptName, Executor.VM_VERSION, "main", chunks, constants)
    {
    }

    public Script(string scriptName, int vmVersion, string entryPointName, ScriptChunk[] chunks, VMValue[] constants)
    {
        ScriptName = scriptName;
        EntryPointName = entryPointName;
        VMVersion = vmVersion;

        Chunks = new ScriptChunk[chunks.Length];
        for (var i = 0; i < chunks.Length; i++)
            Chunks[i] = chunks[i];
        
        ConstantsBlock = new ConstantsBlock(constants);
    }

    public int VMVersion { get; set; }
    public string ScriptName { get; set; }
    public string EntryPointName { get; set; }
    public ScriptChunk[] Chunks { get; set; }

    [IgnoreMember]
    internal ConstantsBlock ConstantsBlock { get; }

    internal ScriptChunk? GetChunk(string chunkName) => Chunks.FirstOrDefault(chunk => chunk.ChunkName == chunkName);

    internal void Execute(Executor vm, string chunkName = "main")
    {
        var chunk = GetChunk(chunkName);
        if (chunk is not null)
            chunk.Execute(vm);
        else
            throw new VMRuntimeException(ErrorCodes.VM2000_NoChunkFoundToExecute);
    }
}