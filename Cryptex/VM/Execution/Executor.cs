namespace Cryptex.VM.Execution;

public sealed class Executor
{
    internal const   int            MAX_FUNCTION_ARGS = 16;
    private readonly Script         m_script;
    private          ExecutorMemory m_memory;

    public Executor(Script script)
    {
        m_script = script;
        m_memory = new ExecutorMemory();
    }

    public void BeginExecution()
    {
        //Will start to execute at the chunk with name "main" -- will error if it is not present.
        m_script.Execute(m_memory);
    }

    public void ExecuteChunk(string chunkName = "main")
    {
        if(m_script.GetChunk(chunkName) is null)
            return;
        
        m_script.Execute(m_memory, chunkName);
    }

    public string DumpMemory() => m_memory.DumpMemory();

    public string? GetValueInMemory(int location) => m_memory.GetSlot(location);
}
