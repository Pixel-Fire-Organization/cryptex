using Cryptex.Exceptions;

namespace Cryptex.VM.Execution;

public sealed class Executor
{
    internal const   int            MAX_FUNCTION_ARGS = 16;
    private readonly Script         m_script;
    private          ExecutorMemory m_memory;
    private          bool           m_VMExited = false;

    public Executor(Script script)
    {
        m_script = script;
        m_memory = new ExecutorMemory();
    }

    /// <summary>
    /// Begins executing the specified script.
    /// </summary>
    /// <returns>true - if the script executed successfully, false - otherwise.</returns>
    public bool BeginExecution()
    {
        try
        {
            //Will start to execute at the chunk with name "main" -- will error if it is not present.
            m_script.Execute(this);
        } catch(VMRuntimeException ex)
        {
            ErrorHandler.WriteError.Invoke($"Execution of script threw a runtime exception: {ex.Message}");
            return false;
        }

        return true;
    }

    public bool ExecuteChunk(string chunkName = "main")
    {
        if (m_script.GetChunk(chunkName) is null)
            return false;

        try { m_script.Execute(this, chunkName); } catch(VMRuntimeException ex)
        {
            ErrorHandler.WriteError.Invoke($"Execution of script threw a runtime exception: {ex.Message}");
            return false;
        }

        return true;
    }

    internal ExecutorMemory GetMemory() => m_memory;
    
    public string DumpMemory() => m_memory.DumpMemory();

    public string? GetValueInMemory(int location) => m_memory.GetSlot(location);

    internal void ExitInstructionCall() { m_VMExited = true; }

    internal bool HasExitBeenCalled() { return m_VMExited; }
}
