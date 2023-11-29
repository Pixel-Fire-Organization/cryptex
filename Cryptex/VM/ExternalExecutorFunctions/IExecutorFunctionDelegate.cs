namespace Cryptex.VM.ExternalExecutorFunctions;

public interface IExecutorFunctionDelegate
{
    object? Execute(object?[]? @params);
}
