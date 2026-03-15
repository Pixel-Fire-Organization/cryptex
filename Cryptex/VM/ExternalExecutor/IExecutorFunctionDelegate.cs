namespace Cryptex.VM.ExternalExecutor;

public interface IExecutorFunctionDelegate
{
    object? Execute(object?[]? @params);
}