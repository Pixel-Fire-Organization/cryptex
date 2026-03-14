using Cryptex.Exceptions;

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

    internal void Execute(Executor vm)
    {
        foreach (var instruction in m_instructions)
        {
            if(vm.HasExitBeenCalled()) 
                return;
            
            var inst = instruction.Code.GetByCode();
            if (inst is null)
                throw new VMRuntimeException(ErrorCodes.VM2008_InvalidInstructionFoundInScriptChunk);

            inst?.Execute(instruction, vm);
        }
    }
}
