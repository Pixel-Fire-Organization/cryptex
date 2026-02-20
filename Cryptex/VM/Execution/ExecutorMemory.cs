using System.Text;

namespace Cryptex.VM.Execution;

public sealed class ExecutorMemory
{
    private readonly Dictionary<int, string> m_memory = new();

    public void SetSlot(int slot, string value) { m_memory[slot] = value; }

    public string? GetSlot(int slot) => m_memory.TryGetValue(slot, out var val) ? val : null;

    public string? RemoveSlot(int slot)
    {
        string? val = null;

        if (m_memory.TryGetValue(slot, out var value))
        {
            val = value;
            m_memory.Remove(slot);
        }

        return val;
    }

    public string DumpMemory()
    {
        if (m_memory.Count == 0)
            return "<EMPTY>";
        
        StringBuilder sb = new();
        foreach (var i in m_memory)
        {
            sb.Append('[');
            sb.Append(i.Key);
            sb.Append("]: `");
            sb.Append(i.Value);
            sb.Append("`\n");
        }

        return sb.ToString();
    }
}
