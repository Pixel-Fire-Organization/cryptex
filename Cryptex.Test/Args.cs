using Cryptex.VM.Execution.Scripts;

namespace Cryptex.Test;

/// <summary>
///     Factory helpers for building <see cref="ScriptInstructionArgument" /> values in tests.
/// </summary>
internal static class Args
{
    internal static ScriptInstructionArgument Mem(int slot)
        => new(slot, InstructionArgumentType.MemoryAddress);

    internal static ScriptInstructionArgument Const(int index)
        => new(index, InstructionArgumentType.Constant);

    internal static ScriptInstructionArgument HexConst(int index)
        => new(index, InstructionArgumentType.HexConstant);
}

