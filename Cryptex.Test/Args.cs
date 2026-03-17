namespace Cryptex.Test;

/// <summary>
///     Factory helpers for building <see cref="ScriptInstructionArgument" /> values
///     and assembling <see cref="Script" /> instances in tests.
/// </summary>
internal static class Args
{
    internal static ScriptInstructionArgument Mem(int slot)
        => new(slot, InstructionArgumentType.MemoryAddress);

    internal static ScriptInstructionArgument Const(int index)
        => new(index, InstructionArgumentType.Constant);

    internal static ScriptInstructionArgument Label(int instructionIndex)
        => new(instructionIndex, InstructionArgumentType.Label);

    internal static Script Build(string name, VmValue[] constants, params ScriptInstruction[] instructions)
    {
        var chunk = new ScriptChunk("main", instructions);
        return new Script(name, [chunk], constants);
    }
}

