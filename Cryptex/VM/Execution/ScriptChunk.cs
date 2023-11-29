namespace Cryptex.VM.Execution;

public sealed class ScriptChunk
{
    private readonly ScriptChunkOpCode[] m_instructions;

    public string Name { get; }

    public ScriptChunk(string chunkName, ScriptChunkOpCode[] instructions)
    {
        Name = chunkName;

        m_instructions = new ScriptChunkOpCode[instructions.Length];
        for (int i = 0; i < instructions.Length; i++)
        {
            m_instructions[i] = instructions[i];
        }
    }

    internal void Execute(ExecutorMemory memory)
    {
        foreach (var instruction in m_instructions)
        {
            var inst = instruction.Code.GetByCode();
            if (inst is null)
                ErrorList.WriteError(ErrorCodes.VM2008_InvalidInstructionFoundInScriptChunk, fatal: true);

            inst?.Execute(instruction, memory);
        }
    }
}
