using Cryptex.Exceptions;
using Cryptex.VM.Execution.OperationCodes;
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
            Instructions[i] = instructions[i];
    }

    public ScriptInstruction[] Instructions { get; init; }
    public string ChunkName { get; init; }

    internal void Execute(Executor vm)
    {
        var ip = 0;
        while (ip < Instructions.Length)
        {
            if (vm.HasExitBeenCalled())
                return;

            var instruction = Instructions[ip];
            var info = instruction.Code.GetInfo();
            if (info.Instruction is null)
                throw new VmRuntimeException(ErrorCodes.Vm2008InvalidInstructionFoundInScriptChunk);

            info.Instruction.Execute(instruction, vm);

            if (vm.TryConsumeJump(out var target))
            {
                if ((uint)target >= (uint)Instructions.Length)
                    throw new VmRuntimeException(ErrorCodes.Vm2012InstructionArgumentIsOutOfRange);
                ip = target;
            }
            else
            {
                ip++;
            }
        }
    }
}
