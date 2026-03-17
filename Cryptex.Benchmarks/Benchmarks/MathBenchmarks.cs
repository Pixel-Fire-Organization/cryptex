using Cryptex.VM.Execution.OperationCodes;

namespace Cryptex.Benchmarks.Benchmarks;

/// <summary>
///     Benchmarks for math instructions: Add, AddF, AddImm, Sub, SubF, SubImm,
///     Mul, MulF, MulImm, Div, DivF, DivImm, Inc, IncF, Dec, DecF, Mod, ModImm.
///     Scripts are built once in GlobalSetup; only ExecuteScript() is timed.
/// </summary>
[MemoryDiagnoser]
public class MathBenchmarks
{
    // Shared constants for all scripts (int at 0-3, float at 4-5).
    private static readonly VmValue[] Constants =
    [
        VmValue.FromInteger(5),   // const[0]
        VmValue.FromInteger(6),   // const[1]
        VmValue.FromInteger(3),   // const[2]
        VmValue.FromInteger(2),   // const[3]
        VmValue.FromFloat(5.5m),  // const[4]
        VmValue.FromFloat(6.5m)   // const[5]
    ];

    private Script m_addScript    = null!;
    private Script m_addFScript   = null!;
    private Script m_addImmScript = null!;
    private Script m_subScript    = null!;
    private Script m_subFScript   = null!;
    private Script m_subImmScript = null!;
    private Script m_mulScript    = null!;
    private Script m_mulFScript   = null!;
    private Script m_mulImmScript = null!;
    private Script m_divScript    = null!;
    private Script m_divFScript   = null!;
    private Script m_divImmScript = null!;
    private Script m_incScript    = null!;
    private Script m_incFScript   = null!;
    private Script m_decScript    = null!;
    private Script m_decFScript   = null!;
    private Script m_modScript    = null!;
    private Script m_modImmScript = null!;

    [GlobalSetup]
    public void Setup()
    {
        // Integer operations — const[0]=5, const[1]=6
        m_addScript = Build("add", Constants,
            new ScriptInstruction(OpCodes.Load,   [Mem(1), Const(0)]),
            new ScriptInstruction(OpCodes.Load,   [Mem(2), Const(1)]),
            new ScriptInstruction(OpCodes.Add,    [Mem(1), Mem(2)]));

        m_addImmScript = Build("addimm", Constants,
            new ScriptInstruction(OpCodes.Load,   [Mem(1), Const(0)]),
            new ScriptInstruction(OpCodes.AddImm, [Mem(1), Const(1)]));

        m_subScript = Build("sub", Constants,
            new ScriptInstruction(OpCodes.Load,   [Mem(1), Const(1)]),
            new ScriptInstruction(OpCodes.Load,   [Mem(2), Const(0)]),
            new ScriptInstruction(OpCodes.Sub,    [Mem(1), Mem(2)]));

        m_subImmScript = Build("subimm", Constants,
            new ScriptInstruction(OpCodes.Load,   [Mem(1), Const(1)]),
            new ScriptInstruction(OpCodes.SubImm, [Mem(1), Const(0)]));

        m_mulScript = Build("mul", Constants,
            new ScriptInstruction(OpCodes.Load,   [Mem(1), Const(0)]),
            new ScriptInstruction(OpCodes.Load,   [Mem(2), Const(1)]),
            new ScriptInstruction(OpCodes.Mul,    [Mem(1), Mem(2)]));

        m_mulImmScript = Build("mulimm", Constants,
            new ScriptInstruction(OpCodes.Load,   [Mem(1), Const(0)]),
            new ScriptInstruction(OpCodes.MulImm, [Mem(1), Const(1)]));

        // const[1]=6 / const[2]=3 = 2
        m_divScript = Build("div", Constants,
            new ScriptInstruction(OpCodes.Load,   [Mem(1), Const(1)]),
            new ScriptInstruction(OpCodes.Load,   [Mem(2), Const(2)]),
            new ScriptInstruction(OpCodes.Div,    [Mem(1), Mem(2)]));

        m_divImmScript = Build("divimm", Constants,
            new ScriptInstruction(OpCodes.Load,   [Mem(1), Const(1)]),
            new ScriptInstruction(OpCodes.DivImm, [Mem(1), Const(2)]));

        m_incScript = Build("inc", Constants,
            new ScriptInstruction(OpCodes.Load, [Mem(1), Const(0)]),
            new ScriptInstruction(OpCodes.Inc,  [Mem(1)]));

        m_decScript = Build("dec", Constants,
            new ScriptInstruction(OpCodes.Load, [Mem(1), Const(1)]),
            new ScriptInstruction(OpCodes.Dec,  [Mem(1)]));

        // const[1]=6, const[2]=3
        m_modScript = Build("mod", Constants,
            new ScriptInstruction(OpCodes.Load, [Mem(1), Const(1)]),
            new ScriptInstruction(OpCodes.Load, [Mem(2), Const(2)]),
            new ScriptInstruction(OpCodes.Mod,  [Mem(1), Mem(2)]));

        m_modImmScript = Build("modimm", Constants,
            new ScriptInstruction(OpCodes.Load,   [Mem(1), Const(1)]),
            new ScriptInstruction(OpCodes.ModImm, [Mem(1), Const(2)]));

        // Float operations — const[4]=5.5m, const[5]=6.5m
        m_addFScript = Build("addf", Constants,
            new ScriptInstruction(OpCodes.Load,   [Mem(1), Const(4)]),
            new ScriptInstruction(OpCodes.Load,   [Mem(2), Const(5)]),
            new ScriptInstruction(OpCodes.AddF,   [Mem(1), Mem(2)]));

        m_subFScript = Build("subf", Constants,
            new ScriptInstruction(OpCodes.Load,   [Mem(1), Const(5)]),
            new ScriptInstruction(OpCodes.Load,   [Mem(2), Const(4)]),
            new ScriptInstruction(OpCodes.SubF,   [Mem(1), Mem(2)]));

        m_mulFScript = Build("mulf", Constants,
            new ScriptInstruction(OpCodes.Load,   [Mem(1), Const(4)]),
            new ScriptInstruction(OpCodes.Load,   [Mem(2), Const(5)]),
            new ScriptInstruction(OpCodes.MulF,   [Mem(1), Mem(2)]));

        // const[5]=6.5 / const[4]=5.5
        m_divFScript = Build("divf", Constants,
            new ScriptInstruction(OpCodes.Load,   [Mem(1), Const(5)]),
            new ScriptInstruction(OpCodes.Load,   [Mem(2), Const(4)]),
            new ScriptInstruction(OpCodes.DivF,   [Mem(1), Mem(2)]));

        m_incFScript = Build("incf", Constants,
            new ScriptInstruction(OpCodes.Load, [Mem(1), Const(4)]),
            new ScriptInstruction(OpCodes.IncF, [Mem(1)]));

        m_decFScript = Build("decf", Constants,
            new ScriptInstruction(OpCodes.Load, [Mem(1), Const(5)]),
            new ScriptInstruction(OpCodes.DecF, [Mem(1)]));
    }

    [Benchmark] public bool Add()    => Run(m_addScript);
    [Benchmark] public bool AddF()   => Run(m_addFScript);
    [Benchmark] public bool AddImm() => Run(m_addImmScript);
    [Benchmark] public bool Sub()    => Run(m_subScript);
    [Benchmark] public bool SubF()   => Run(m_subFScript);
    [Benchmark] public bool SubImm() => Run(m_subImmScript);
    [Benchmark] public bool Mul()    => Run(m_mulScript);
    [Benchmark] public bool MulF()   => Run(m_mulFScript);
    [Benchmark] public bool MulImm() => Run(m_mulImmScript);
    [Benchmark] public bool Div()    => Run(m_divScript);
    [Benchmark] public bool DivF()   => Run(m_divFScript);
    [Benchmark] public bool DivImm() => Run(m_divImmScript);
    [Benchmark] public bool Inc()    => Run(m_incScript);
    [Benchmark] public bool IncF()   => Run(m_incFScript);
    [Benchmark] public bool Dec()    => Run(m_decScript);
    [Benchmark] public bool DecF()   => Run(m_decFScript);
    [Benchmark] public bool Mod()    => Run(m_modScript);
    [Benchmark] public bool ModImm() => Run(m_modImmScript);
}

