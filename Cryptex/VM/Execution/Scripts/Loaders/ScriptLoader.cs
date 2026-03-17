using Cryptex.VM.Execution.Scripts.Validation;

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
        var script = Deserialize(data);
        if (script is null)
            return null;

        var result = ScriptValidator.Validate(script);
        return result.IsValid ? script : null;
    }

    public static Script? Load(string path)
    {
        if (string.IsNullOrEmpty(path) || !File.Exists(path))
            return null;
        return Load(File.ReadAllBytes(path));
    }

    public static Script? LoadAndValidate(byte[] data, out ScriptValidationResult validationResult)
    {
        var script = Deserialize(data);
        if (script is null)
        {
            validationResult = ScriptValidationResult.Unloadable;
            return null;
        }

        validationResult = ScriptValidator.Validate(script);
        return validationResult.IsValid ? script : null;
    }

    public static Script? LoadAndValidate(string path, out ScriptValidationResult validationResult)
    {
        if (string.IsNullOrEmpty(path) || !File.Exists(path))
        {
            validationResult = ScriptValidationResult.Unloadable;
            return null;
        }

        return LoadAndValidate(File.ReadAllBytes(path), out validationResult);
    }

    private static Script? Deserialize(byte[] data)
    {
        var span = data.AsSpan();
        foreach (var serializer in Serializers)
        {
            if (serializer.CanDeserialize(span))
                return serializer.Deserialize(data);
        }
        return null;
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

