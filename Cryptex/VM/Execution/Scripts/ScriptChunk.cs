using Cryptex.Exceptions;
using MessagePack;

namespace Cryptex.VM.Execution.Scripts;

[MessagePackObject(true)]
public sealed class ScriptChunk
{
    public ScriptChunk(string chunkName, ScriptInstruction[] instructions)
    {
        ChunkName = chunkName;

        Instructions = new ScriptInstruction[instructions.Length];
        for (var i = 0; i < instructions.Length; i++)
        {
            Instructions[i] = instructions[i];
        }
    }

    public ScriptInstruction[] Instructions { get; set; }
    public string ChunkName { get; set; }

    internal void Execute(Executor vm)
    {
        var ip = 0;
        while (ip < Instructions.Length)
        {
            if (vm.HasExitBeenCalled())
                return;

            var instruction = Instructions[ip];
            var inst = instruction.Code.GetByCode();
            if (inst is null)
                throw new VMRuntimeException(ErrorCodes.VM2008_InvalidInstructionFoundInScriptChunk);

            inst.Execute(instruction, vm);

            if (vm.TryConsumeJump(out var target))
            {
                if ((uint)target >= (uint)Instructions.Length)
                    throw new VMRuntimeException(ErrorCodes.VM2012_InstructionArgumentIsOutOfRange);
                ip = target;
            }
            else
            {
                ip++;
            }
        }
    }
}