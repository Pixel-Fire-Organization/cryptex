using Cryptex.Exceptions;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.VM.Execution;

/// <summary>
///     An immutable, script-bound table of pre-parsed <see cref="VMValue" /> constants.
/// </summary>
public sealed class ConstantsBlock
{
    public static readonly ConstantsBlock Empty = new([]);

    private readonly VMValue[] m_values;

    public ConstantsBlock(VMValue[] values)
    {
        m_values = new VMValue[values.Length];
        values.CopyTo(m_values, 0);
    }

    public int Count => m_values.Length;

    /// <summary>
    ///     Returns the <see cref="VMValue" /> at <paramref name="index" />.
    ///     Never returns a null reference; never returns <see cref="VMValue.Undefined" /> for
    ///     a block built from a valid <see cref="Script.ConstantsBlock" /> array.
    /// </summary>
    /// <exception cref="VMRuntimeException">
    ///     Thrown with <see cref="ErrorCodes.VM2006_ConstantsIndexOutOfRange" /> when
    ///     <paramref name="index" /> is out of range.
    /// </exception>
    public VMValue Get(int index) => (uint)index >= (uint)m_values.Length
        ? throw new VMRuntimeException(ErrorCodes.VM2006_ConstantsIndexOutOfRange)
        : m_values[index];
}