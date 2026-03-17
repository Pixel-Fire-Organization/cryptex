using Cryptex.Exceptions;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.VM.Execution;

/// <summary>
///     An immutable, script-bound table of pre-parsed <see cref="VmValue" /> constants.
/// </summary>
public sealed class ConstantsBlock
{
    private readonly VmValue[] m_values;

    public ConstantsBlock(VmValue[] values)
    {
        m_values = new VmValue[values.Length];
        values.CopyTo(m_values, 0);
    }

    public int Count => m_values.Length;

    /// <summary>
    ///     Returns the <see cref="VmValue" /> at <paramref name="index" />.
    ///     Never returns a null reference; never returns <see cref="VmValue.Undefined" /> for
    ///     a block built from a valid <see cref="Script.ConstantsBlock" /> array.
    /// </summary>
    /// <exception cref="VmRuntimeException">
    ///     Thrown with <see cref="ErrorCodes.VM2006_ConstantsIndexOutOfRange" /> when
    ///     <paramref name="index" /> is out of range.
    /// </exception>
    public VmValue Get(int index) => (uint)index >= (uint)m_values.Length
        ? throw new VmRuntimeException(ErrorCodes.VM2006_ConstantsIndexOutOfRange)
        : m_values[index];
}