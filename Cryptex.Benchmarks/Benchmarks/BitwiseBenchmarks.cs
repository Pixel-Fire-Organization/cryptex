using BenchmarkDotNet.Attributes;
using Cryptex.VM.Execution;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.Benchmarks.Benchmarks;

/// <summary>
///     Benchmarks for bitwise instructions: And, Or, Xor, Not, Shl, Shr.
/// </summary>
[MemoryDiagnoser]
public class BitwiseBenchmarks
{
    private static readonly VMValue[] Constants =
    [
        VMValue.FromInteger(0xFF00), // const[0]
        VMValue.FromInteger(0x00FF), // const[1]
        VMValue.FromInteger(1)       // const[2] — shift amount for Shl/Shr
    ];

    private Script m_andScript  = null!;
    private Script m_orScript   = null!;
    private Script m_xorScript  = null!;
    private Script m_notScript  = null!;
    private Script m_shlScript  = null!;
    private Script m_shrScript  = null!;

    [GlobalSetup]
    public void Setup()
    {
        m_andScript = Build("and",
            new ScriptInstruction(OpCodes.Load, [Mem(1), Const(0)]),
            new ScriptInstruction(OpCodes.Load, [Mem(2), Const(1)]),
            new ScriptInstruction(OpCodes.And,  [Mem(1), Mem(2)]));

        m_orScript = Build("or",
            new ScriptInstruction(OpCodes.Load, [Mem(1), Const(0)]),
            new ScriptInstruction(OpCodes.Load, [Mem(2), Const(1)]),
            new ScriptInstruction(OpCodes.Or,   [Mem(1), Mem(2)]));

        m_xorScript = Build("xor",
            new ScriptInstruction(OpCodes.Load, [Mem(1), Const(0)]),
            new ScriptInstruction(OpCodes.Load, [Mem(2), Const(1)]),
            new ScriptInstruction(OpCodes.Xor,  [Mem(1), Mem(2)]));

        m_notScript = Build("not",
            new ScriptInstruction(OpCodes.Load, [Mem(1), Const(0)]),
            new ScriptInstruction(OpCodes.Not,  [Mem(1)]));

        // Shl/Shr: shift amount must be a Constant arg.
        m_shlScript = Build("shl",
            new ScriptInstruction(OpCodes.Load, [Mem(1), Const(0)]),
            new ScriptInstruction(OpCodes.Shl,  [Mem(1), Const(2)]));  // shift by const[2]=1

        m_shrScript = Build("shr",
            new ScriptInstruction(OpCodes.Load, [Mem(1), Const(0)]),
            new ScriptInstruction(OpCodes.Shr,  [Mem(1), Const(2)]));  // shift by const[2]=1
    }

    [Benchmark] public bool And() => Run(m_andScript);
    [Benchmark] public bool Or()  => Run(m_orScript);
    [Benchmark] public bool Xor() => Run(m_xorScript);
    [Benchmark] public bool Not() => Run(m_notScript);
    [Benchmark] public bool Shl() => Run(m_shlScript);
    [Benchmark] public bool Shr() => Run(m_shrScript);

    // ── Helpers ─────────────────────────────────────────────────────────────

    private static bool Run(Script script)
    {
        var executor = new Executor(script);
        return executor.ExecuteScript();
    }

    private static Script Build(string name, params ScriptInstruction[] instructions)
    {
        var chunk = new ScriptChunk("main", instructions);
        return new Script(name, [chunk], Constants);
    }

    private static ScriptInstructionArgument Mem(int slot)
        => new(slot, InstructionArgumentType.MemoryAddress);

    private static ScriptInstructionArgument Const(int index)
        => new(index, InstructionArgumentType.Constant);
}
