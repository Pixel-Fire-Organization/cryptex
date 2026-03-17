using Cryptex.VM.Execution.OperationCodes;

namespace Cryptex.Benchmarks.Benchmarks;

/// <summary>
///     Benchmarks for VM-control instructions: Nop, Exit.
/// </summary>
[MemoryDiagnoser]
public class VmControlBenchmarks
{
    private static readonly VmValue[] Constants =
    [
        VmValue.FromInteger(0) // const[0] = 0 — Nop sleep ms and Exit code
    ];

    private Script m_nopScript  = null!;
    private Script m_exitScript = null!;

    [GlobalSetup]
    public void Setup()
    {
        // Nop requires a Constant arg specifying sleep milliseconds (0 = no sleep).
        m_nopScript = Build("nop", Constants,
            new ScriptInstruction(OpCodes.Nop, [Const(0)]));

        // Exit sets the VM exit code from a Constant and stops execution.
        m_exitScript = Build("exit", Constants,
            new ScriptInstruction(OpCodes.Exit, [Const(0)]));
    }

    [Benchmark] public bool Nop()  => Run(m_nopScript);
    [Benchmark] public bool Exit() => Run(m_exitScript);
}
