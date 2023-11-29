namespace Cryptex.VM;

public static class ExternalRegister
{
    private static Dictionary<string, ExternalExecutor> m_externalExecutors = new();
}
