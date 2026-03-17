namespace Cryptex.VM.ExternalExecutor;

public interface IExecutorFunctionDelegate
{
    void Execute(object?[]? @params);
}