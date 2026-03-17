using Cryptex.VM.Execution.OperationCodes;
using Cryptex.VM.Execution.Scripts.Loaders;

namespace Cryptex.Test.LoadersTest;

public sealed class JsonScriptSerializerTest
{
    private static readonly JsonScriptSerializer Serializer = new();

    // A chunk with no instructions: the execution loop completes immediately and succeeds.
    private static Script EmptyScript() => new Script("empty_script",
        [new ScriptChunk("main", [])], []);

    private static Script NopScript() => new Script("nop_script",
        [new ScriptChunk("main", [new ScriptInstruction(OpCodes.Nop)])], []);

    [Fact]
    public void Serialize_ProducesJsonBytes()
    {
        var data = Serializer.Serialize(NopScript());

        Assert.Equal((byte)'{', data[0]);
    }

    [Fact]
    public void RoundTrip_PreservesMetadata()
    {
        var original = new Script("my_script", Executor.VmVersion, "entry",
            [new ScriptChunk("main", [new ScriptInstruction(OpCodes.Nop)])], []);

        var loaded = Serializer.Deserialize(Serializer.Serialize(original));

        Assert.NotNull(loaded);
        Assert.Equal("my_script",          loaded.ScriptName);
        Assert.Equal("entry",              loaded.EntryPointName);
        Assert.Equal(Executor.VmVersion,  loaded.VmVersion);
    }

    [Fact]
    public void RoundTrip_PreservesMultipleChunks()
    {
        var original = new Script("multi", [
            new ScriptChunk("main",   [new ScriptInstruction(OpCodes.Nop)]),
            new ScriptChunk("helper", [new ScriptInstruction(OpCodes.Nop), new ScriptInstruction(OpCodes.Nop)])
        ], []);

        var loaded = Serializer.Deserialize(Serializer.Serialize(original));

        Assert.NotNull(loaded);
        Assert.Equal(2,        loaded.Chunks.Length);
        Assert.Equal("main",   loaded.Chunks[0].ChunkName);
        Assert.Equal("helper", loaded.Chunks[1].ChunkName);
        Assert.Single(         loaded.Chunks[0].Instructions);
        Assert.Equal(2,        loaded.Chunks[1].Instructions.Length);
    }

    [Fact]
    public void RoundTrip_PreservesInstructionCodes()
    {
        var original = new Script("test", [
            new ScriptChunk("main", [
                new ScriptInstruction(OpCodes.Nop),
                new ScriptInstruction(OpCodes.Nop)
            ])
        ], []);

        var loaded = Serializer.Deserialize(Serializer.Serialize(original));

        Assert.NotNull(loaded);
        Assert.Equal(OpCodes.Nop, loaded.Chunks[0].Instructions[0].Code);
        Assert.Equal(OpCodes.Nop, loaded.Chunks[0].Instructions[1].Code);
    }

    [Fact]
    public void RoundTrip_ExecutesCorrectly()
    {
        var loaded = Serializer.Deserialize(Serializer.Serialize(EmptyScript()));

        Assert.NotNull(loaded);
        Assert.True(new Executor(loaded).ExecuteScript());
    }

    [Fact]
    public void Deserialize_ReturnsNull_ForCorruptJson()
    {
        var corrupt = "{not: valid json!!!"u8.ToArray();

        Assert.Null(Serializer.Deserialize(corrupt));
    }

    [Fact]
    public void CanDeserialize_ReturnsTrue_ForJsonData() => Assert.True(Serializer.CanDeserialize("{}"u8));

    [Fact]
    public void CanDeserialize_ReturnsFalse_ForBinaryData()
    {
        byte[] binary = [0x43, 0x52, 0x58, 0x42];
        Assert.False(Serializer.CanDeserialize(binary));
    }

    [Fact]
    public void CanDeserialize_ReturnsFalse_ForEmptyData() => Assert.False(Serializer.CanDeserialize(ReadOnlySpan<byte>.Empty));
}



