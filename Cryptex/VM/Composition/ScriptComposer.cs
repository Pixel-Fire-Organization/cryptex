using Cryptex.VM.Execution;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.VM.Composition;

public sealed class ScriptComposer
{
    private string m_name;
    private string m_entryPoint;
    private int m_vmVersion;
    private readonly List<ScriptChunk> m_chunks;
    private readonly List<VMValue> m_constants;

    private ScriptComposer(string name)
    {
        m_name = name;
        m_entryPoint = "main";
        m_vmVersion = Executor.VM_VERSION;
        m_chunks = [];
        m_constants = [];
    }

    public static ScriptComposer Create(string name = "script") => new(name);

    public static ScriptComposer FromExisting(Script script)
    {
        var composer = new ScriptComposer(script.ScriptName)
        {
            m_entryPoint = script.EntryPointName,
            m_vmVersion = script.VMVersion
        };

        foreach (var chunk in script.Chunks)
            composer.m_chunks.Add(chunk);

        var constantCount = script.ConstantsBlock.Count;
        for (var i = 0; i < constantCount; i++)
            composer.m_constants.Add(script.ConstantsBlock.Get(i));

        return composer;
    }

    public ScriptComposer WithName(string name)
    {
        m_name = name;
        return this;
    }

    public ScriptComposer WithEntryPoint(string entryPoint)
    {
        m_entryPoint = entryPoint;
        return this;
    }

    public ScriptComposer WithVmVersion(int version)
    {
        m_vmVersion = version;
        return this;
    }

    public int ConstantCount => m_constants.Count;

    public int AddConstant(VMValue value)
    {
        m_constants.Add(value);
        return m_constants.Count - 1;
    }

    public VMValue GetConstant(int index) => m_constants[index];

    public ScriptComposer RemoveConstant(int index)
    {
        m_constants.RemoveAt(index);
        return this;
    }

    public bool HasChunk(string chunkName) => m_chunks.Any(chunk => chunk.ChunkName == chunkName);

    public ScriptComposer AddChunk(ScriptChunk chunk)
    {
        m_chunks.Add(chunk);
        return this;
    }

    public ScriptComposer AddChunk(string chunkName, Action<ScriptChunkComposer> configure)
    {
        var chunkComposer = new ScriptChunkComposer(chunkName);
        configure(chunkComposer);
        m_chunks.Add(chunkComposer.Build());
        return this;
    }

    public ScriptComposer RemoveChunk(string chunkName)
    {
        for (var i = 0; i < m_chunks.Count; i++)
        {
            if (m_chunks[i].ChunkName != chunkName) continue;
            m_chunks.RemoveAt(i);
            return this;
        }

        return this;
    }

    public ScriptComposer ReplaceChunk(string chunkName, Action<ScriptChunkComposer> configure)
    {
        ScriptChunk? existing = null;
        var index = -1;
        for (var i = 0; i < m_chunks.Count; i++)
        {
            if (m_chunks[i].ChunkName != chunkName) continue;
            existing = m_chunks[i];
            index = i;
            break;
        }

        var chunkComposer = existing is not null
            ? ScriptChunkComposer.FromExisting(existing)
            : new ScriptChunkComposer(chunkName);

        configure(chunkComposer);

        if (index >= 0)
            m_chunks[index] = chunkComposer.Build();
        else
            m_chunks.Add(chunkComposer.Build());

        return this;
    }

    public Script Build() => new(m_name, m_vmVersion, m_entryPoint, [.. m_chunks], [.. m_constants]);
}