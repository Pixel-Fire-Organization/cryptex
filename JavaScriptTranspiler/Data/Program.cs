using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

using JavaScriptTranspiler.Data.Statements;

namespace JavaScriptTranspiler.Data;

public class Program : INode
{
    [JsonPropertyName("type")] public string Type { get; }
    
    [JsonPropertyName("start")] public long Start { get; }
    [JsonPropertyName("end")] public long End { get; }

    //public SourceLocation Loc { get; }
    [JsonPropertyName("body"), JsonConverter(typeof(StatementConverter))] public IStatement[] Body { get; }
    [JsonPropertyName("sourceType")] public string SourceType { get; }

    [JsonConstructor]
    public Program(string type, long start, long end, IStatement[] body, string sourceType)
    {
        Type = type;
        Start = start;
        End = end;
        Body = new IStatement[body.Length];
        
        Array.Copy(body, Body, body.Length);

        SourceType = sourceType;
    }
}

public class StatementConverter : JsonConverter<IStatement[]>
{
    public override IStatement[]? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        IStatement? s;
        var statement = reader.GetString();

        if (statement is null)
            return null;

        if ((s = JsonSerializer.Deserialize<ExpressionStatement>(statement)) is not null)
            return s;
        if ((s = JsonSerializer.Deserialize<BlockStatement>(statement)) is not null)
            return s;
        if ((s = JsonSerializer.Deserialize<EmptyStatement>(statement)) is not null)
            return s;

        return null;
    }

    public override void Write(Utf8JsonWriter writer, IStatement[] value, JsonSerializerOptions options)
    {
        //TODO
    }
}