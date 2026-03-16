namespace Cryptex.VM.Execution.Scripts.Loaders;

public interface IScriptSerializer
{
    ScriptFormat Format { get; }

    bool CanDeserialize(ReadOnlySpan<byte> data);

    byte[] Serialize(Script script);

    Script? Deserialize(byte[] data);
}

