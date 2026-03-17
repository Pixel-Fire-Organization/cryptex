using System.Numerics;
using Cryptex.Exceptions;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.VM.Execution;

public sealed class Executor
{
    public static int VM_VERSION { get; } = 1; // This must be changed on each publish. 

    private readonly ExecutorMemory m_memory;
    private readonly Script m_script;
    private BigInteger m_exitCode = 0;
    private bool m_vmExited;

    private CompareFlag m_compareFlag;
    private bool m_hasError;
    private ErrorCodes m_errorCode;
    private bool m_jumpPending;
    private int m_jumpTarget;
    private bool m_compatibilityWarningEmitted;

    public Executor(Script script)
    {
        m_script = script;
        m_memory = new ExecutorMemory();
    }

    /// <summary>
    ///     <c>true</c> when the loaded script targets a VM version newer than this runtime.
    ///     The executor will run using the current VM's instruction behaviour and emit a
    ///     warning at the start of execution. See Docs/VM/Compatibility Mode.md for details.
    /// </summary>
    public bool IsInCompatibilityMode => m_script.VMVersion > VM_VERSION;

    /// <summary>The VM version the loaded script was authored for.</summary>
    internal int ScriptVersion => m_script.VMVersion;

    public bool ExecuteScript()
    {
        WarnIfInCompatibilityMode();

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
        WarnIfInCompatibilityMode();

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

    public VMValue GetValueInMemory(int location) => m_memory.GetSlot(location);
    
    internal VMValue GetConstant(int index) => m_script.ConstantsBlock.Get(index);

    internal void ExitInstructionCall(BigInteger code)
    {
        m_vmExited = true;
        m_exitCode = code;
    }

    internal bool HasExitBeenCalled() => m_vmExited;

    internal CompareFlag GetCompareFlag() => m_compareFlag;
    internal void SetCompareFlag(CompareFlag flag) => m_compareFlag = flag;
    internal void ClearCompareFlag() => m_compareFlag = CompareFlag.None;

    internal void SetError(ErrorCodes code)
    {
        m_hasError = true;
        m_errorCode = code;
    }

    internal bool HasError() => m_hasError;

    internal ErrorCodes ConsumeError()
    {
        if (!m_hasError)
            return ErrorCodes.SYS0000_ErrorCodeNotFound;

        var code = m_errorCode;
        m_hasError = false;
        m_errorCode = default;
        return code;
    }

    internal void RequestJump(int instructionIndex)
    {
        m_jumpPending = true;
        m_jumpTarget = instructionIndex;
    }

    internal bool TryConsumeJump(out int target)
    {
        if (!m_jumpPending)
        {
            target = 0;
            return false;
        }

        target = m_jumpTarget;
        m_jumpPending = false;
        return true;
    }

    private void WarnIfInCompatibilityMode()
    {
        if (!IsInCompatibilityMode || m_compatibilityWarningEmitted)
            return;

        m_compatibilityWarningEmitted = true;
        PrintingDelegates.WriteWarning(
            $"Script '{m_script.ScriptName}' targets VM version {m_script.VMVersion}, " +
            $"but the current VM is version {VM_VERSION}. " +
            "Running in compatibility mode — instruction behaviour may differ from the target version. " +
            "See Docs/VM/Compatibility Mode.md for details.");
    }
}