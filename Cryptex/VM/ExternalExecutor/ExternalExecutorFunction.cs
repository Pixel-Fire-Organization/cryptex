namespace Cryptex.VM.ExternalExecutor;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
public sealed class ExternalExecutorFunction
{
    public ExternalExecutorFunction(string functionName, IExecutorFunctionDelegate function)
    {
        Name = functionName;
        Function = function;
    }

    public string Name { get; }
    private IExecutorFunctionDelegate Function { get; }

    public void Invoke() => Function.Execute(null);

    public void Invoke(object?[] @params) => Function.Execute(@params);

    public void Invoke<T1>(T1 param)
        where T1 : unmanaged =>
        Function.Execute([param]);

    public void Invoke<T1, T2>(T1 param1, T2 param2)
        where T1 : unmanaged where T2 : unmanaged =>
        Function.Execute([param1, param2]);

    public void Invoke<T1, T2, T3>(T1 param1, T2 param2, T3 param3)
        where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged =>
        Function.Execute([param1, param2, param3]);
}