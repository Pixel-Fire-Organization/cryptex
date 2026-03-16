using Cryptex.Exceptions;
using MessagePack;

namespace Cryptex.VM.Execution.Scripts;

[MessagePackObject(true)]
public sealed class Script
{
    private string[] m_constants = [];

    public Script(string scriptName, ScriptChunk[] chunks, VMValue[] constants)
        : this(scriptName, "main", chunks, constants)
    {
    }

    public Script(string scriptName, string entryPointName, ScriptChunk[] chunks, VMValue[] constants)
    {
        ScriptName = scriptName;
        EntryPointName = entryPointName;

        Chunks = new ScriptChunk[chunks.Length];
        for (var i = 0; i < chunks.Length; i++)
            Chunks[i] = chunks[i];

        m_constants = [];
        ConstantsBlock = new ConstantsBlock(constants);
    }

    public Script(string scriptName, ScriptChunk[] chunks)
        : this(scriptName, "main", chunks, (string[])[])
    {
    }

    public Script(string scriptName, ScriptChunk[] chunks, string[] constants)
        : this(scriptName, "main", chunks, constants)
    {
    }

    public Script(string scriptName, string entryPointName, ScriptChunk[] chunks)
        : this(scriptName, entryPointName, chunks, (string[])[])
    {
    }

    public Script(string scriptName, string entryPointName, ScriptChunk[] chunks, string[] constants)
    {
        ScriptName = scriptName;
        EntryPointName = entryPointName;

        Chunks = new ScriptChunk[chunks.Length];
        for (var i = 0; i < chunks.Length; i++)
        {
            Chunks[i] = chunks[i];
        }

        // Build the raw array first so BuildConstantsBlock sees the full content.
        m_constants = new string[constants.Length];
        for (var i = 0; i < constants.Length; i++)
        {
            m_constants[i] = constants[i];
        }

        ConstantsBlock = BuildConstantsBlock(m_constants);
    }

    public int VMVersion { get; set; }
    public string ScriptName { get; set; }
    public string EntryPointName { get; set; }
    public ScriptChunk[] Chunks { get; set; }

    /// <summary>
    ///     The raw string Constants Block (serialised by MessagePack).
    ///     Setting this property rebuilds <see cref="ConstantsBlock" /> immediately so that
    ///     the block is always consistent with the stored strings.
    /// </summary>
    public string[] Constants
    {
        get => m_constants;
        set
        {
            m_constants = value ?? [];
            ConstantsBlock = BuildConstantsBlock(m_constants);
        }
    }

    /// <summary>
    ///     The pre-parsed, immutable Constants Block derived from <see cref="Constants" />.
    ///     Built once — either in the constructor or when the <see cref="Constants" />
    ///     property setter is called by MessagePack during deserialisation.
    /// </summary>
    [IgnoreMember]
    internal ConstantsBlock ConstantsBlock { get; private set; } = ConstantsBlock.Empty;

    internal ScriptChunk? GetChunk(string chunkName) => Chunks.FirstOrDefault(chunk => chunk.ChunkName == chunkName);

    internal void Execute(Executor vm, string chunkName = "main")
    {
        var chunk = GetChunk(chunkName);
        if (chunk is not null)
            chunk.Execute(vm);
        else
            throw new VMRuntimeException(ErrorCodes.VM2000_NoChunkFoundToExecute);
    }

    private static ConstantsBlock BuildConstantsBlock(string[] constants)
    {
        if (constants.Length == 0)
            return ConstantsBlock.Empty;

        var values = new VMValue[constants.Length];
        for (var i = 0; i < constants.Length; i++)
        {
            if (!VMValue.TryParse(constants[i], out values[i]))
                values[i] = VMValue.FromError(ErrorCodes.VM2004_MemoryArgumentIsNotANumber);
        }

        return new ConstantsBlock(values);
    }
}