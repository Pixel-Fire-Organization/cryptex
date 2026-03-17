using System.Text;
using Cryptex.Exceptions;

namespace Cryptex.VM.Execution;

/// <summary>
///     The VM's working memory.
/// </summary>
public sealed class ExecutorMemory
{
    // PAGE_SIZE must be a power of two so that / and % compile to bit shifts.
    private const int PAGE_SIZE = 1024;
    private const int PAGE_SHIFT = 10; // log₂(PAGE_SIZE)
    private const int PAGE_MASK = PAGE_SIZE - 1;

    private readonly List<VmValue[]> m_pages = [];
    
    public void SetSlot(int slot, VmValue value)
    {
        if (slot < 0)
            throw new VmRuntimeException(ErrorCodes.Vm2007InvalidMemoryLocationSpecifiedAsArgument);

        var pageIndex = slot >> PAGE_SHIFT;
        EnsurePage(pageIndex);
        m_pages[pageIndex][slot & PAGE_MASK] = value;
    }
    
    public VmValue GetSlot(int slot)
    {
        if (slot < 0)
            return VmValue.Undefined;

        var pageIndex = slot >> PAGE_SHIFT;
        return pageIndex < m_pages.Count
            ? m_pages[pageIndex][slot & PAGE_MASK]
            : VmValue.Undefined;
    }
    
    public VmValue RemoveSlot(int slot)
    {
        if (slot < 0)
            return VmValue.Undefined;

        var pageIndex = slot >> PAGE_SHIFT;
        if (pageIndex >= m_pages.Count)
            return VmValue.Undefined;

        var offset = slot & PAGE_MASK;
        var val = m_pages[pageIndex][offset];
        m_pages[pageIndex][offset] = VmValue.Undefined;
        return val;
    }

    /// <summary>
    ///     Returns a human-readable dump of every set memory slot, in ascending slot order.
    ///     Returns <c>"&lt;EMPTY&gt;"</c> when no slots have been written.
    /// </summary>
    public string DumpMemory()
    {
        // Fast path: check for any non-undefined value before building a StringBuilder.
        if (!HasAnyValue())
            return "<EMPTY>";

        StringBuilder sb = new();

        for (var pageIndex = 0; pageIndex < m_pages.Count; pageIndex++)
        {
            var page = m_pages[pageIndex];
            for (var offset = 0; offset < PAGE_SIZE; offset++)
            {
                var val = page[offset];
                if (val.IsUndefined)
                    continue;

                var slot = (pageIndex << PAGE_SHIFT) | offset;
                sb.Append('[');
                sb.Append(slot);
                sb.Append("]: `");
                sb.Append(val.ToString());
                sb.Append("`\n");
            }
        }

        return sb.ToString();
    }

    private bool HasAnyValue()
    {
        foreach (var page in m_pages)
        {
            foreach (var value in page)
            {
                if (!value.IsUndefined)
                    return true;
            }
        }

        return false;
    }

    private void EnsurePage(int pageIndex)
    {
        while (m_pages.Count <= pageIndex)
        {
            m_pages.Add(new VmValue[PAGE_SIZE]);
        }
    }
}