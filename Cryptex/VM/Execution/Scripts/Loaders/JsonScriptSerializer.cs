using System.Text;
using MessagePack;

namespace Cryptex.VM.Execution.Scripts.Loaders;

public sealed class JsonScriptSerializer : IScriptSerializer
{
    public ScriptFormat Format => ScriptFormat.Json;

    public bool CanDeserialize(ReadOnlySpan<byte> data)
        => data.Length > 0 && data[0] == (byte)'{';

    public byte[] Serialize(Script script)
    {
        var msgpackBytes = MessagePackSerializer.Serialize(script);
        return Encoding.UTF8.GetBytes(MessagePackSerializer.ConvertToJson(msgpackBytes));
    }

    public Script? Deserialize(byte[] data)
    {
        try
        {
            var msgpackBytes = MessagePackSerializer.ConvertFromJson(Encoding.UTF8.GetString(data));
            return MessagePackSerializer.Deserialize<Script>(msgpackBytes);
        }
        catch (Exception ex)
        {
            PrintingDelegates.WriteException(ex);
            return null;
        }
    }
}
