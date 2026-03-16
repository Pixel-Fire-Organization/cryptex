namespace Cryptex.VM.Execution.Scripts.Loaders;

public static class ScriptLoader
{
    private static readonly IScriptSerializer[] Serializers =
    [
        new JsonScriptSerializer(),
        new BinaryScriptSerializer(),
    ];

    public static Script? Load(byte[] data)
    {
        var span = data.AsSpan();
        foreach (var serializer in Serializers)
        {
            if (serializer.CanDeserialize(span))
                return serializer.Deserialize(data);
        }
        return null;
    }

    public static Script? Load(string path)
    {
        if (string.IsNullOrEmpty(path) || !File.Exists(path))
            return null;
        return Load(File.ReadAllBytes(path));
    }

    public static byte[] Save(Script script, ScriptFormat format = ScriptFormat.Binary)
        => GetSerializer(format).Serialize(script);

    public static void Save(Script script, string path, ScriptFormat format = ScriptFormat.Binary)
        => File.WriteAllBytes(path, Save(script, format));

    private static IScriptSerializer GetSerializer(ScriptFormat format)
    {
        foreach (var serializer in Serializers)
        {
            if (serializer.Format == format)
                return serializer;
        }
        return new BinaryScriptSerializer();
    }
}

