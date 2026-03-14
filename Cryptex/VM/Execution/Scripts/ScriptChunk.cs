using Cryptex.Exceptions;
using MessagePack;

namespace Cryptex.VM.Execution.Scripts;

[MessagePackObject(keyAsPropertyName: true)]
public sealed class ScriptChunk
{
    public ScriptInstruction[] Instructions { get; set; }
    public string ChunkName { get; set; }

    public ScriptChunk(string chunkName, ScriptInstruction[] instructions)
    {
        ChunkName = chunkName;

        Instructions = new ScriptInstruction[instructions.Length];
        for (int i = 0; i < instructions.Length; i++)
        {
            Instructions[i] = instructions[i];
        }
    }

    internal void Execute(Executor vm)
    {
        foreach (var instruction in Instructions)
        {
            if (vm.HasExitBeenCalled())
                return;

            var inst = instruction.Code.GetByCode();
            if (inst is null)
                throw new VMRuntimeException(ErrorCodes.VM2008_InvalidInstructionFoundInScriptChunk);

            inst?.Execute(instruction, vm);
        }
    }
}