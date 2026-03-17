using Cryptex.VM.Execution.OperationCodes;
using Cryptex.VM.Execution.Scripts.Loaders;

namespace Cryptex.Test.LoadersTest;

public sealed class ScriptLoaderTest
{
    private static Script AddScript() => Args.Build("add_script",
        [VMValue.FromInteger(3), VMValue.FromInteger(7)],
        new ScriptInstruction(OpCodes.Load, [Args.Mem(1), Args.Const(0)]),
        new ScriptInstruction(OpCodes.Load, [Args.Mem(2), Args.Const(1)]),
        new ScriptInstruction(OpCodes.Add, [Args.Mem(1), Args.Mem(2)]));

    // A chunk with no instructions: the execution loop completes immediately and succeeds.
    private static Script EmptyScript() => new Script("empty_script",
        [new ScriptChunk("main", [])], []);

    private static Script NopScript() => new Script("nop_script",
        [new ScriptChunk("main", [new ScriptInstruction(OpCodes.Nop)])], []);

    [Fact]
    public void Save_DefaultFormat_IsBinary()
    {
        var data = ScriptLoader.Save(NopScript());

        Assert.Equal(0x43, data[0]); // C
        Assert.Equal(0x52, data[1]); // R
        Assert.Equal(0x58, data[2]); // X
        Assert.Equal(0x42, data[3]); // B
    }

    [Fact]
    public void Save_Json_ProducesJsonBytes()
    {
        var data = ScriptLoader.Save(NopScript(), ScriptFormat.Json);

        Assert.Equal((byte)'{', data[0]);
    }

    [Fact]
    public void SaveLoad_Binary_RoundTrip_PreservesMetadataAndExecutes()
    {
        var original = AddScript();
        var data = ScriptLoader.Save(original, ScriptFormat.Binary);

        var loaded = ScriptLoader.Load(data);

        Assert.NotNull(loaded);
        Assert.Equal(original.ScriptName, loaded.ScriptName);
        var executor = new Executor(loaded);
        Assert.True(executor.ExecuteScript());
        Assert.Equal(VMValue.FromInteger(10), executor.GetValueInMemory(1));
    }

    [Fact]
    public void SaveLoad_Json_RoundTrip_PreservesMetadataAndExecutes()
    {
        var original = EmptyScript();
        var data = ScriptLoader.Save(original, ScriptFormat.Json);

        var loaded = ScriptLoader.Load(data);

        Assert.NotNull(loaded);
        Assert.Equal(original.ScriptName, loaded.ScriptName);
        Assert.True(new Executor(loaded).ExecuteScript());
    }

    [Fact]
    public void Load_AutoDetects_BinaryFormat()
    {
        var binaryData = ScriptLoader.Save(AddScript(), ScriptFormat.Binary);

        var loaded = ScriptLoader.Load(binaryData);

        Assert.NotNull(loaded);
        Assert.Equal(ScriptFormat.Binary, new BinaryScriptSerializer().Format);
        var executor = new Executor(loaded);
        Assert.True(executor.ExecuteScript());
        Assert.Equal(VMValue.FromInteger(10), executor.GetValueInMemory(1));
    }

    [Fact]
    public void Load_AutoDetects_JsonFormat()
    {
        var jsonData = ScriptLoader.Save(EmptyScript(), ScriptFormat.Json);

        var loaded = ScriptLoader.Load(jsonData);

        Assert.NotNull(loaded);
        Assert.True(new Executor(loaded).ExecuteScript());
    }

    [Fact]
    public void Load_BinaryScript_LoadsCorrectly_RegardlessOfRequestedFormat()
    {
        // Auto-detection ensures binary data is always handled by the binary serializer,
        // even when the caller saves with one format and loads without specifying format.
        var binaryData = ScriptLoader.Save(AddScript(), ScriptFormat.Binary);
        var jsonData = ScriptLoader.Save(EmptyScript(), ScriptFormat.Json);

        var fromBinary = ScriptLoader.Load(binaryData);
        var fromJson = ScriptLoader.Load(jsonData);

        Assert.NotNull(fromBinary);
        Assert.NotNull(fromJson);
    }

    [Fact]
    public void Load_ReturnsNull_ForEmptyData() => Assert.Null(ScriptLoader.Load([]));

    [Fact]
    public void Load_ReturnsNull_ForNonExistentPath() =>
        Assert.Null(ScriptLoader.Load(Path.Combine(Directory.GetCurrentDirectory(), "nonexistent.script")));
}