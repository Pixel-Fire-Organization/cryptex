using BenchmarkDotNet.Attributes;
using Cryptex.VM.Execution;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.Benchmarks.Benchmarks;

/// <summary>
///     Benchmarks for memory instructions: Load, Free.
/// </summary>
[MemoryDiagnoser]
public class MemoryBenchmarks
{
    private static readonly VMValue[] Constants =
    [
        VMValue.FromInteger(42),  // const[0]
        VMValue.FromInteger(99)   // const[1]
    ];

    private Script m_loadScript      = null!;
    private Script m_freeScript      = null!;
    private Script m_loadFreeScript  = null!;

    [GlobalSetup]
    public void Setup()
    {
        m_loadScript = Build("load",
            new ScriptInstruction(OpCodes.Load, [Mem(1), Const(0)]));

        m_freeScript = Build("free",
            new ScriptInstruction(OpCodes.Load, [Mem(1), Const(0)]),
            new ScriptInstruction(OpCodes.Free, [Mem(1)]));

        m_loadFreeScript = Build("load_free_reload",
            new ScriptInstruction(OpCodes.Load, [Mem(1), Const(0)]),
            new ScriptInstruction(OpCodes.Free, [Mem(1)]),
            new ScriptInstruction(OpCodes.Load, [Mem(1), Const(1)]));
    }

    [Benchmark] public bool Load()         => Run(m_loadScript);
    [Benchmark] public bool Free()         => Run(m_freeScript);
    [Benchmark] public bool LoadFreeLoad() => Run(m_loadFreeScript);

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
