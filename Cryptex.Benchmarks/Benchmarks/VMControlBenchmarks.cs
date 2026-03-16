using BenchmarkDotNet.Attributes;
using Cryptex.VM.Execution;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.Benchmarks.Benchmarks;

/// <summary>
///     Benchmarks for VM-control instructions: Nop, Exit.
/// </summary>
[MemoryDiagnoser]
public class VMControlBenchmarks
{
    private static readonly VMValue[] Constants =
    [
        VMValue.FromInteger(0) // const[0] = 0 — Nop sleep ms and Exit code
    ];

    private Script m_nopScript  = null!;
    private Script m_exitScript = null!;

    [GlobalSetup]
    public void Setup()
    {
        // Nop requires a Constant arg specifying sleep milliseconds (0 = no sleep).
        m_nopScript = Build("nop",
            new ScriptInstruction(OpCodes.Nop, [Const(0)]));

        // Exit sets the VM exit code from a Constant and stops execution.
        m_exitScript = Build("exit",
            new ScriptInstruction(OpCodes.Exit, [Const(0)]));
    }

    [Benchmark] public bool Nop()  => Run(m_nopScript);
    [Benchmark] public bool Exit() => Run(m_exitScript);

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

    private static ScriptInstructionArgument Const(int index)
        => new(index, InstructionArgumentType.Constant);
}
