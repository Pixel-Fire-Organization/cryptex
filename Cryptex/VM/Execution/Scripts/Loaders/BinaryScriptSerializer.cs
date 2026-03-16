using System.Globalization;
using System.Numerics;
using Cryptex.VM.Execution;
using MessagePack;

namespace Cryptex.VM.Execution.Scripts.Loaders;

public sealed class BinaryScriptSerializer : IScriptSerializer
{
    // Magic header "CRXB" distinguishes the extended format (with constants) from the legacy raw MessagePack.
    private static ReadOnlySpan<byte> Magic => [0x43, 0x52, 0x58, 0x42];

    public ScriptFormat Format => ScriptFormat.Binary;

    public bool CanDeserialize(ReadOnlySpan<byte> data)
        => data.Length > 0 && data[0] != (byte)'{';

    public byte[] Serialize(Script script)
    {
        var envelope = BuildEnvelope(script);
        var serialized = MessagePackSerializer.Serialize(envelope);
        var result = new byte[Magic.Length + serialized.Length];
        Magic.CopyTo(result);
        serialized.CopyTo(result, Magic.Length);
        return result;
    }

    public Script? Deserialize(byte[] data)
    {
        if (data.Length >= Magic.Length && data.AsSpan().StartsWith(Magic))
            return DeserializeEnvelope(data);
        
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

    private static ScriptBinaryEnvelope BuildEnvelope(Script script)
    {
        var constantCount = script.ConstantsBlock.Count;
        var constants = new ScriptConstantEntry[constantCount];
        for (var i = 0; i < constantCount; i++)
        {
            var value = script.ConstantsBlock.Get(i);
            constants[i] = new ScriptConstantEntry { Kind = (byte)value.Kind, Value = value.ToString() };
        }
        return new ScriptBinaryEnvelope { Script = script, Constants = constants };
    }

    private static Script? DeserializeEnvelope(byte[] data)
    {
        try
        {
            var payload = data.AsSpan(Magic.Length).ToArray();
            var envelope = MessagePackSerializer.Deserialize<ScriptBinaryEnvelope>(payload);
            if (envelope.Script is null)
                return null;

            var constants = ReconstructConstants(envelope.Constants ?? []);
            return new Script(
                envelope.Script.ScriptName,
                envelope.Script.VMVersion,
                envelope.Script.EntryPointName,
                envelope.Script.Chunks,
                constants);
        }
        catch (Exception ex)
        {
            PrintingDelegates.WriteException(ex);
            return null;
        }
    }

    private static VMValue[] ReconstructConstants(ScriptConstantEntry[] entries)
    {
        var result = new VMValue[entries.Length];
        for (var i = 0; i < entries.Length; i++)
        {
            var entry = entries[i];
            result[i] = (VMValueKind)entry.Kind switch
            {
                VMValueKind.Integer => VMValue.FromInteger(BigInteger.Parse(entry.Value, CultureInfo.InvariantCulture)),
                VMValueKind.Float   => VMValue.FromFloat(decimal.Parse(entry.Value, CultureInfo.InvariantCulture)),
                VMValueKind.String  => VMValue.FromString(entry.Value),
                _                   => VMValue.Undefined
            };
        }
        return result;
    }
}

[MessagePackObject(keyAsPropertyName: true, AllowPrivate = true)]
internal sealed class ScriptBinaryEnvelope
{
    public Script? Script { get; set; }
    public ScriptConstantEntry[] Constants { get; set; } = [];
}

[MessagePackObject(keyAsPropertyName: true, AllowPrivate = true)]
internal sealed class ScriptConstantEntry
{
    public byte Kind { get; set; }
    public string Value { get; set; } = "";
}


