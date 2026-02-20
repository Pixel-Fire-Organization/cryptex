namespace JavaScriptTranspiler.Data;

public record struct SourceLocation(string? Source, Position Start, Position End);