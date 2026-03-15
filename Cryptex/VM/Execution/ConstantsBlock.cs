using Cryptex.Exceptions;

namespace Cryptex.VM.Execution;

/// <summary>
///     An immutable, script-bound table of pre-parsed <see cref="VMValue" /> constants.
/// </summary>
/// <remarks>
///     <para>
///         Constants are parsed exactly once — either during
///         <see cref="Scripts.Script" /> construction (for inline/programmatic scripts) or on first
///         access after MessagePack deserialization (lazy build from <c>string[]</c>). After that,
///         every constant lookup is a simple array index, with no heap allocation and no parsing.
///     </para>
///     <para>
///         After the script is loaded and validated, <see cref="Get" /> never returns a null or
///         empty value. An out-of-range index throws
///         <see cref="ErrorCodes.VM2012_InstructionArgumentIsOutOfRange" />; an invalid constant
///         string stored in <see cref="Scripts.Script.Constants" /> is represented by a
///         <see cref="VMValueKind.Error" /> <see cref="VMValue" /> which is surfaced at execution time
///         by <see cref="Executor.GetConstantOrThrow" />.
///     </para>
/// </remarks>
public sealed class ConstantsBlock
{
    /// <summary>A reusable empty constants block (no constants defined).</summary>
    public static readonly ConstantsBlock Empty = new([]);

    private readonly VMValue[] m_values;

    /// <param name="values">
    ///     The pre-parsed constant values. A defensive copy is made so that the caller cannot
    ///     mutate the block after construction.
    /// </param>
    public ConstantsBlock(VMValue[] values)
    {
        m_values = new VMValue[values.Length];
        values.CopyTo(m_values, 0);
    }

    /// <summary>The number of constants in this block.</summary>
    public int Count => m_values.Length;

    /// <summary>
    ///     Returns the <see cref="VMValue" /> at <paramref name="index" />.
    ///     Never returns a null reference; never returns <see cref="VMValue.Undefined" /> for
    ///     a block built from a valid <see cref="Scripts.Script.Constants" /> array.
    /// </summary>
    /// <exception cref="VMRuntimeException">
    ///     Thrown with <see cref="ErrorCodes.VM2012_InstructionArgumentIsOutOfRange" /> when
    ///     <paramref name="index" /> is out of range.
    /// </exception>
    public VMValue Get(int index)
    {
        if ((uint)index >= (uint)m_values.Length)
            throw new VMRuntimeException(ErrorCodes.VM2012_InstructionArgumentIsOutOfRange);

        return m_values[index];
    }
}