using System.Text;
using Cryptex.Exceptions;

namespace Cryptex.VM.Execution;

/// <summary>
///     The VM's working memory.
/// </summary>
/// <remarks>
///     <para>
///         Memory is divided into fixed-size <em>pages</em> of <see cref="PAGE_SIZE" /> slots each.
///         A page is allocated on first write to any slot within it; pages are never freed during
///         execution. This gives O(1) reads and writes without the rehashing cost of a dictionary.
///     </para>
///     <para>
///         An unwritten slot contains <see cref="VMValue.Undefined" />.
///         After a script is loaded and validated, instructions that read a slot they previously
///         wrote will always get a non-undefined value — no null checks are needed past the initial
///         undefined guard.
///     </para>
/// </remarks>
public sealed class ExecutorMemory
{
    // PAGE_SIZE must be a power of two so that / and % compile to bit shifts.
    private const int PAGE_SIZE = 1024;
    private const int PAGE_SHIFT = 10; // log₂(PAGE_SIZE)
    private const int PAGE_MASK = PAGE_SIZE - 1;

    private readonly List<VMValue[]> m_pages = [];

    /// <summary>Stores <paramref name="value" /> at <paramref name="slot" />.</summary>
    /// <exception cref="VMRuntimeException">Thrown for negative slot indices.</exception>
    public void SetSlot(int slot, VMValue value)
    {
        if (slot < 0)
            throw new VMRuntimeException(ErrorCodes.VM2007_InvalidMemoryLocationSpecifiedAsArgument);

        var pageIndex = slot >> PAGE_SHIFT;
        EnsurePage(pageIndex);
        m_pages[pageIndex][slot & PAGE_MASK] = value;
    }

    /// <summary>
    ///     Returns the value at <paramref name="slot" />, or <see cref="VMValue.Undefined" />
    ///     if the slot has never been written.
    /// </summary>
    public VMValue GetSlot(int slot)
    {
        if (slot < 0)
            return VMValue.Undefined;

        var pageIndex = slot >> PAGE_SHIFT;
        return pageIndex < m_pages.Count
            ? m_pages[pageIndex][slot & PAGE_MASK]
            : VMValue.Undefined;
    }

    /// <summary>
    ///     Clears <paramref name="slot" /> and returns the value that was stored there.
    ///     Returns <see cref="VMValue.Undefined" /> if the slot was never written.
    /// </summary>
    public VMValue RemoveSlot(int slot)
    {
        if (slot < 0)
            return VMValue.Undefined;

        var pageIndex = slot >> PAGE_SHIFT;
        if (pageIndex >= m_pages.Count)
            return VMValue.Undefined;

        var offset = slot & PAGE_MASK;
        var val = m_pages[pageIndex][offset];
        m_pages[pageIndex][offset] = VMValue.Undefined;
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
        for (var p = 0; p < m_pages.Count; p++)
        {
            var page = m_pages[p];
            for (var o = 0; o < PAGE_SIZE; o++)
            {
                if (!page[o].IsUndefined)
                    return true;
            }
        }

        return false;
    }

    private void EnsurePage(int pageIndex)
    {
        while (m_pages.Count <= pageIndex)
        {
            m_pages.Add(new VMValue[PAGE_SIZE]);
        }
    }
}