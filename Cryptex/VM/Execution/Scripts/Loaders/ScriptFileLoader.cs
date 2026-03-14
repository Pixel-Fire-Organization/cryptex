using MessagePack;

namespace Cryptex.VM.Execution.Scripts.Loaders;

public static class ScriptFileLoader
{
    public static Script? LoadScript(byte[] data)
    {
        try
        {
            return MessagePackSerializer.Deserialize<Script>(data);
        }
        catch (Exception ex)
        {
            PrintingDelegates.WriteException(ex);
            return null;
        }
    }

    public static Script? LoadScript(string path)
    {
        if (string.IsNullOrEmpty(path) || !File.Exists(path))
            return null;

        var data = File.ReadAllBytes(path);
        return LoadScript(data);
    }
}