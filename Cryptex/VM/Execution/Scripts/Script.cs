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
        : this("DEFAULT_SCRIPT", Executor.VmVersion, "main", [], [])
    {
    }

    public Script(string scriptName, ScriptChunk[] chunks)
        : this(scriptName, Executor.VmVersion, "main", chunks, [])
    {
    }

    public Script(string scriptName, ScriptChunk[] chunks, VmValue[] constants)
        : this(scriptName, Executor.VmVersion, "main", chunks, constants)
    {
    }

    public Script(string scriptName, int vmVersion, string entryPointName, ScriptChunk[] chunks, VmValue[] constants)
    {
        ScriptName = scriptName;
        EntryPointName = entryPointName;
        VmVersion = vmVersion;

        Chunks = new ScriptChunk[chunks.Length];
        for (var i = 0; i < chunks.Length; i++)
            Chunks[i] = chunks[i];

        ConstantsBlock = new ConstantsBlock(constants);
    }

    public int VmVersion { get; init; }
    public string ScriptName { get; init; }
    public string EntryPointName { get; init; }
    public ScriptChunk[] Chunks { get; init; }

    [IgnoreMember]
    internal ConstantsBlock ConstantsBlock { get; }

    internal ScriptChunk? GetChunk(string chunkName) => Chunks.FirstOrDefault(chunk => chunk.ChunkName == chunkName);

    internal void Execute(Executor vm, string chunkName = "main")
    {
        var chunk = GetChunk(chunkName);
        if (chunk is not null)
            chunk.Execute(vm);
        else
            throw new VmRuntimeException(ErrorCodes.Vm2000NoChunkFoundToExecute);
    }
}
