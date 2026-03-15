using System.Numerics;
using Cryptex.Exceptions;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.VM.Execution;

public sealed class Executor
{
    internal const int MAX_FUNCTION_ARGS = 16;
    private readonly ExecutorMemory m_memory;
    private readonly Script m_script;
    private BigInteger m_exitCode = 0;
    private bool m_vmExited;

    public Executor(Script script)
    {
        m_script = script;
        m_memory = new ExecutorMemory();
    }

    /// <summary>
    ///     Begins executing the specified script.
    /// </summary>
    /// <returns><see langword="true" /> if the script executed successfully; <see langword="false" /> otherwise.</returns>
    public bool BeginExecution()
    {
        try
        {
            m_script.Execute(this);
        }
        catch (VMRuntimeException ex)
        {
            PrintingDelegates.WriteError.Invoke($"Execution of script threw a runtime exception: {ex.Message}");
            m_exitCode = -1;
            return false;
        }
        catch (TerminateInstructionFoundException)
        {
            PrintingDelegates.WriteError(
                "Critical error - a `term` instruction found in the current script chunk! Recovering from this might be impossible.");
            m_exitCode = -2;
            return false;
        }

        return true;
    }

    public bool ExecuteChunk(string chunkName = "main")
    {
        if (m_script.GetChunk(chunkName) is null)
            return false;

        try
        {
            m_script.Execute(this, chunkName);
        }
        catch (VMRuntimeException ex)
        {
            PrintingDelegates.WriteError.Invoke($"Execution of script threw a runtime exception: {ex.Message}");
            return false;
        }

        return true;
    }

    internal ExecutorMemory GetMemory() => m_memory;

    public string DumpMemory() => m_memory.DumpMemory();

    public BigInteger GetExitCode() => m_exitCode;

    /// <summary>
    ///     Returns the <see cref="VMValue" /> stored in memory slot <paramref name="location" />,
    ///     or <see cref="VMValue.Undefined" /> if the slot has never been written.
    /// </summary>
    public VMValue GetValueInMemory(int location) => m_memory.GetSlot(location);

    internal void ExitInstructionCall(BigInteger code)
    {
        m_vmExited = true;
        m_exitCode = code;
    }

    internal bool HasExitBeenCalled() => m_vmExited;

    /// <summary>
    ///     Resolves a constant by <paramref name="index" />, returning a raw
    ///     <see cref="VMValue" /> (which may be <see cref="VMValueKind.Error" />).
    /// </summary>
    /// <remarks>
    ///     Lookup order:
    ///     <list type="number">
    ///         <item>The instruction's <see cref="Scripts.ScriptInstruction.LocalConstants" /> (from the convenience string constructor).</item>
    ///         <item>The script-level <see cref="ConstantsBlock" /> (from binary <c>.script</c> files).</item>
    ///     </list>
    /// </remarks>
    internal VMValue GetConstant(in ScriptInstruction instruction, int index)
    {
        if (instruction.LocalConstants is { } local && (uint)index < (uint)local.Length)
            return local[index];

        return m_script.ConstantsBlock.Get(index);
    }

    /// <summary>
    ///     Resolves a constant and throws immediately if it carries a deferred parse error.
    ///     Use this inside instruction <c>Execute</c> methods for all constant lookups.
    /// </summary>
    /// <exception cref="VMRuntimeException">
    ///     Thrown with the deferred <see cref="ErrorCodes" /> when the constant is a
    ///     <see cref="VMValueKind.Error" /> value, or with
    ///     <see cref="ErrorCodes.VM2012_InstructionArgumentIsOutOfRange" /> when the index is
    ///     out of range.
    /// </exception>
    internal VMValue GetConstantOrThrow(in ScriptInstruction instruction, int index)
    {
        var val = GetConstant(in instruction, index);

        if (val.IsError)
            throw new VMRuntimeException(val.ErrorCode);

        return val;
    }
}