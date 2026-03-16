using static Cryptex.Benchmarks.ScriptRunner;

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
        m_loadScript = Build("load", Constants,
            new ScriptInstruction(OpCodes.Load, [Mem(1), Const(0)]));

        m_freeScript = Build("free", Constants,
            new ScriptInstruction(OpCodes.Load, [Mem(1), Const(0)]),
            new ScriptInstruction(OpCodes.Free, [Mem(1)]));

        m_loadFreeScript = Build("load_free_reload", Constants,
            new ScriptInstruction(OpCodes.Load, [Mem(1), Const(0)]),
            new ScriptInstruction(OpCodes.Free, [Mem(1)]),
            new ScriptInstruction(OpCodes.Load, [Mem(1), Const(1)]));
    }

    [Benchmark] public bool Load()         => Run(m_loadScript);
    [Benchmark] public bool Free()         => Run(m_freeScript);
    [Benchmark] public bool LoadFreeLoad() => Run(m_loadFreeScript);
}
