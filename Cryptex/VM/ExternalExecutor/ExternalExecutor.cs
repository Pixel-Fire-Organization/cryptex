namespace Cryptex.VM.ExternalExecutor;

public sealed class ExternalExecutor
{
    private readonly List<ExternalExecutorFunction> m_registeredFunctions;

    public ExternalExecutor(string referenceName, IEnumerable<ExternalExecutorFunction> registeredFunctions)
    {
        ReferenceName = referenceName;
        m_registeredFunctions = registeredFunctions.ToList();
    }

    public string ReferenceName { get; }

    public void Invoke(string functionName, object?[]? @params = null)
    {
        if (string.IsNullOrEmpty(functionName))
            PrintingDelegates.WriteError("VM1000: ");

        foreach (var fn in m_registeredFunctions)
        {
            if (fn.Name == functionName)
            {
                if (@params is null)
                    fn.Invoke();
                else
                    fn.Invoke(@params);
            }
        }
    }
}