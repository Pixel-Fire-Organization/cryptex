using Cryptex.VM.Execution;

namespace Cryptex.Test;

public sealed class ExecutorMemoryTest
{
    [Fact]
    public void TestGetSlot_ValidSlot()
    {
        ExecutorMemory memory = new ExecutorMemory();
        memory.SetSlot(0, "abc");

        Assert.NotNull(memory.GetSlot(0));
        Assert.Equal("abc", memory.GetSlot(0));
    }

    [Fact]
    public void TestGetSlot_InvalidSlot()
    {
        ExecutorMemory memory = new ExecutorMemory();
        memory.SetSlot(0, "abc");

        Assert.Null(memory.GetSlot(1));
    }

    [Fact]
    public void TestSetSlot()
    {
        ExecutorMemory memory = new ExecutorMemory();
        memory.SetSlot(0, "abc");

        Assert.NotNull(memory.GetSlot(0));
        Assert.Equal("abc", memory.GetSlot(0));
    }

    [Fact]
    public void TestRemoveSlot_InvalidSlot()
    {
        ExecutorMemory memory = new ExecutorMemory();
        memory.SetSlot(0, "abc");

        Assert.Null(memory.RemoveSlot(1));
    }

    [Fact]
    public void TestRemoveSlot_ValidSlot()
    {
        ExecutorMemory memory = new ExecutorMemory();
        memory.SetSlot(0, "abc");

        Assert.Equal("abc", memory.RemoveSlot(0));
    }
    
    [Fact]
    public void TestDumpMemory()
    {
        ExecutorMemory memory = new ExecutorMemory();
        memory.SetSlot(0, "abc");
        memory.SetSlot(1, "def");
        memory.SetSlot(2, "ghi");

        Assert.Equal("[0]: `abc`\n[1]: `def`\n[2]: `ghi`\n", memory.DumpMemory());
    }
    
    [Fact]
    public void TestDumpMemory_EmptyMemory()
    {
        ExecutorMemory memory = new ExecutorMemory();

        Assert.Equal("<EMPTY>", memory.DumpMemory());
    }
}
