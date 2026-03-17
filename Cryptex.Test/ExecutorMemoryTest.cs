namespace Cryptex.Test;

public sealed class ExecutorMemoryTest
{
    [Fact]
    public void TestGetSlot_ValidSlot()
    {
        ExecutorMemory memory = new ExecutorMemory();
        memory.SetSlot(0, VmValue.FromInteger(0)); // write any value first
        // Overwrite with a known integer to match original "abc" intent (slot exists).
        memory.SetSlot(0, VmValue.FromInteger(42));

        Assert.False(memory.GetSlot(0).IsUndefined);
    }

    [Fact]
    public void TestGetSlot_InvalidSlot()
    {
        ExecutorMemory memory = new ExecutorMemory();
        memory.SetSlot(0, VmValue.FromInteger(42));

        Assert.True(memory.GetSlot(1).IsUndefined);
    }

    [Fact]
    public void TestSetSlot()
    {
        ExecutorMemory memory = new ExecutorMemory();
        memory.SetSlot(0, VmValue.FromInteger(42));

        Assert.False(memory.GetSlot(0).IsUndefined);
        Assert.Equal(VmValue.FromInteger(42), memory.GetSlot(0));
    }

    [Fact]
    public void TestRemoveSlot_InvalidSlot()
    {
        ExecutorMemory memory = new ExecutorMemory();
        memory.SetSlot(0, VmValue.FromInteger(42));

        Assert.True(memory.RemoveSlot(1).IsUndefined);
    }

    [Fact]
    public void TestRemoveSlot_ValidSlot()
    {
        ExecutorMemory memory = new ExecutorMemory();
        memory.SetSlot(0, VmValue.FromInteger(42));

        VmValue removed = memory.RemoveSlot(0);
        Assert.False(removed.IsUndefined);
        Assert.Equal(VmValue.FromInteger(42), removed);
    }

    [Fact]
    public void TestDumpMemory()
    {
        ExecutorMemory memory = new ExecutorMemory();
        memory.SetSlot(0, VmValue.FromInteger(0));   // "0" — represents "abc" slot presence
        memory.SetSlot(1, VmValue.FromInteger(1));
        memory.SetSlot(2, VmValue.FromInteger(2));

        string dump = memory.DumpMemory();
        Assert.Contains("[0]:", dump);
        Assert.Contains("[1]:", dump);
        Assert.Contains("[2]:", dump);
    }

    [Fact]
    public void TestDumpMemory_EmptyMemory()
    {
        ExecutorMemory memory = new ExecutorMemory();

        Assert.Equal("<EMPTY>", memory.DumpMemory());
    }
}
