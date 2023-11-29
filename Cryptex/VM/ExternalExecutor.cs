using System.Reflection;

using Cryptex.VM.ExternalExecutorFunctions;

namespace Cryptex.VM;

public sealed class ExternalExecutorFunction
{
    public   string                    Name     { get; }
    internal IExecutorFunctionDelegate Function { get; }

    public ExternalExecutorFunction(string functionName, IExecutorFunctionDelegate function)
    {
        Name     = functionName;
        Function = function;
    }

    public void Invoke() { Function.Execute(null); }

    public void Invoke(object?[] @params) { Function.Execute(@params); }

    public void Invoke<T1>(T1 param)
        where T1 : unmanaged
    {
        Function.Execute(new[] { (object)param });
    }

    public void Invoke<T1, T2>(T1 param1, T2 param2)
        where T1 : unmanaged where T2 : unmanaged
    {
        Function.Execute(new[] { (object)param1, (object)param2 });
    }

    public void Invoke<T1, T2, T3>(T1 param1, T2 param2, T3 param3)
        where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged
    {
        Function.Execute(new[] { (object)param1, (object)param2, (object)param3 });
    }
}

public sealed class ExternalExecutor
{
    private List<ExternalExecutorFunction> m_registeredFunctions;
    public  string                         ReferenceName { get; }

    public ExternalExecutor(string referenceName, IEnumerable<ExternalExecutorFunction> registeredFunctions)
    {
        ReferenceName         = referenceName;
        m_registeredFunctions = registeredFunctions.ToList();
    }

    public void Invoke(string functionName, object?[]? @params = null)
    {
        if (string.IsNullOrEmpty(functionName))
            ErrorHandler.WriteError("VM1000: ");

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
