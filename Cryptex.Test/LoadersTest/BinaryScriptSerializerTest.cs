using Cryptex.VM.Execution.OperationCodes;
using Cryptex.VM.Execution.Scripts.Loaders;
using MessagePack;

namespace Cryptex.Test.LoadersTest;

public sealed class BinaryScriptSerializerTest
{
    private static readonly BinaryScriptSerializer Serializer = new();

    private static Script AddScript() => Args.Build("add_script",
        [VMValue.FromInteger(5), VMValue.FromInteger(6)],
        new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
        new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(1)]),
        new ScriptInstruction(OpCodes.Add,  [Args.Mem(1), Args.Mem(2)]));

    private static Script FloatScript() => Args.Build("float_script",
        [VMValue.FromFloat(5.5m), VMValue.FromFloat(2.5m)],
        new ScriptInstruction(OpCodes.Load,  [Args.Mem(1), Args.Const(0)]),
        new ScriptInstruction(OpCodes.Load,  [Args.Mem(2), Args.Const(1)]),
        new ScriptInstruction(OpCodes.AddF,  [Args.Mem(1), Args.Mem(2)]));

    // A chunk with no instructions: the execution loop completes immediately and succeeds.
    private static Script EmptyScript() => new Script("empty_script",
        [new ScriptChunk("main", [])], []);

    private static Script NopScript() => new Script("nop_script",
        [new ScriptChunk("main", [new ScriptInstruction(OpCodes.Nop)])], []);

    [Fact]
    public void Serialize_ProducesMagicHeader()
    {
        var data = Serializer.Serialize(NopScript());

        Assert.Equal(0x43, data[0]); // C
        Assert.Equal(0x52, data[1]); // R
        Assert.Equal(0x58, data[2]); // X
        Assert.Equal(0x42, data[3]); // B
    }

    [Fact]
    public void RoundTrip_PreservesMetadata()
    {
        var original = new Script("my_script", Executor.VM_VERSION, "entry",
            [new ScriptChunk("main", [new ScriptInstruction(OpCodes.Nop)])], []);

        var loaded = Serializer.Deserialize(Serializer.Serialize(original));

        Assert.NotNull(loaded);
        Assert.Equal("my_script",           loaded.ScriptName);
        Assert.Equal("entry",               loaded.EntryPointName);
        Assert.Equal(Executor.VM_VERSION,   loaded.VMVersion);
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
        var loaded = Serializer.Deserialize(Serializer.Serialize(AddScript()));

        Assert.NotNull(loaded);
        Assert.Equal(OpCodes.Load, loaded.Chunks[0].Instructions[0].Code);
        Assert.Equal(OpCodes.Load, loaded.Chunks[0].Instructions[1].Code);
        Assert.Equal(OpCodes.Add,  loaded.Chunks[0].Instructions[2].Code);
    }

    [Fact]
    public void RoundTrip_WithIntegerConstants_ExecutesCorrectly()
    {
        var loaded = Serializer.Deserialize(Serializer.Serialize(AddScript()));

        Assert.NotNull(loaded);
        var executor = new Executor(loaded);
        Assert.True(executor.ExecuteScript());
        Assert.Equal(VMValue.FromInteger(11), executor.GetValueInMemory(1));
        Assert.Equal(VMValue.FromInteger(6),  executor.GetValueInMemory(2));
    }

    [Fact]
    public void RoundTrip_WithFloatConstants_ExecutesCorrectly()
    {
        var loaded = Serializer.Deserialize(Serializer.Serialize(FloatScript()));

        Assert.NotNull(loaded);
        var executor = new Executor(loaded);
        Assert.True(executor.ExecuteScript());
        Assert.Equal(VMValue.FromFloat(8.0m), executor.GetValueInMemory(1));
    }

    [Fact]
    public void Deserialize_FallsBackToLegacyFormat()
    {
        // Simulate a file written by the old ScriptFileLoader (raw MessagePack, no magic, no constants).
        var original = EmptyScript();
        var legacyBytes = MessagePackSerializer.Serialize(original);

        var loaded = Serializer.Deserialize(legacyBytes);

        Assert.NotNull(loaded);
        Assert.Equal(original.ScriptName, loaded.ScriptName);
        Assert.True(new Executor(loaded).ExecuteScript());
    }

    [Fact]
    public void Deserialize_ReturnsNull_ForCorruptPayload()
    {
        // Valid magic header, but the payload after it is garbage.
        byte[] corrupt = [0x43, 0x52, 0x58, 0x42, 0xFF, 0xFE, 0xFD];

        Assert.Null(Serializer.Deserialize(corrupt));
    }

    [Fact]
    public void CanDeserialize_ReturnsTrue_ForBinaryData()
    {
        byte[] data = [0x43, 0x52, 0x58, 0x42, 0x00];
        Assert.True(Serializer.CanDeserialize(data));
    }

    [Fact]
    public void CanDeserialize_ReturnsFalse_ForJsonData() => Assert.False(Serializer.CanDeserialize("{}"u8));

    [Fact]
    public void CanDeserialize_ReturnsFalse_ForEmptyData() => Assert.False(Serializer.CanDeserialize(ReadOnlySpan<byte>.Empty));
}



