namespace Cryptex.Benchmarks;

/// <summary>
///     Shared helpers for building and executing benchmark scripts.
///     All benchmark classes use these instead of duplicating the same private methods.
/// </summary>
internal static class ScriptRunner
{
    internal static bool Run(Script script)
    {
        var executor = new Executor(script);
        return executor.ExecuteScript();
    }

    internal static Script Build(string name, VmValue[] constants, params ScriptInstruction[] instructions)
    {
        var chunk = new ScriptChunk("main", instructions);
        return new Script(name, [chunk], constants);
    }

    internal static ScriptInstructionArgument Mem(int slot)
        => new(slot, InstructionArgumentType.MemoryAddress);

    internal static ScriptInstructionArgument Const(int index)
        => new(index, InstructionArgumentType.Constant);

    internal static ScriptInstructionArgument Label(int instructionIndex)
        => new(instructionIndex, InstructionArgumentType.Label);
}
