using Cryptex.VM.Execution;
using Cryptex.VM.Execution.OperationCodes;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.VM.Composition;

public sealed class ScriptChunkComposer
{
    private string m_name;
    private readonly List<ScriptInstruction> m_instructions;

    public ScriptChunkComposer(string chunkName)
    {
        m_name = chunkName;
        m_instructions = [];
    }

    internal static ScriptChunkComposer FromExisting(ScriptChunk chunk)
    {
        var composer = new ScriptChunkComposer(chunk.ChunkName);
        foreach (var instruction in chunk.Instructions)
            composer.m_instructions.Add(instruction);
        return composer;
    }

    public int Count => m_instructions.Count;

    public ScriptChunkComposer WithName(string chunkName)
    {
        m_name = chunkName;
        return this;
    }

    public ScriptChunkComposer Emit(OpCodes opCode, params ScriptInstructionArgument[] args)
    {
        m_instructions.Add(new ScriptInstruction(opCode, args));
        return this;
    }

    public ScriptChunkComposer Emit(ScriptInstruction instruction)
    {
        m_instructions.Add(instruction);
        return this;
    }

    public ScriptChunkComposer InsertAt(int index, ScriptInstruction instruction)
    {
        m_instructions.Insert(index, instruction);
        return this;
    }

    public ScriptChunkComposer ReplaceAt(int index, ScriptInstruction instruction)
    {
        m_instructions[index] = instruction;
        return this;
    }

    public ScriptChunkComposer RemoveAt(int index)
    {
        m_instructions.RemoveAt(index);
        return this;
    }

    public ScriptInstruction this[int index] => m_instructions[index];

    public ScriptChunk Build() => new(m_name, [.. m_instructions]);
}

