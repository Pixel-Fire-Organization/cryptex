using System.Globalization;
using System.Numerics;
using Cryptex.Exceptions;

namespace Cryptex.VM.Execution;

/// <summary>
///     A discriminated-union value type used throughout the VM for both constants and memory slots.
///     Replaces the previous <see langword="string" />-based representation to eliminate repeated
///     parsing on every instruction execution.
/// </summary>
/// <remarks>
///     <para>
///         For integers in the range <c>(-2³¹, 2³¹)</c> the underlying <see cref="BigInteger" />
///         stores the value without any heap allocation (it fits in a single <see langword="int" />
///         field). Larger integers allocate a small managed array — this is the same behaviour as
///         <see cref="BigInteger" /> itself.
///     </para>
///     <para>
///         A <see cref="VMValueKind.Error" /> value is a deferred parse error created by
///         <see cref="ParseHex" /> when the raw hex string is invalid. The error is surfaced at
///         execution time by <see cref="Executor.GetConstantOrThrow" />.
///     </para>
/// </remarks>
public readonly struct VMValue : IEquatable<VMValue>
{
    /// <summary>The default, uninitialized value. Represents an empty / not-set slot.</summary>
    public static readonly VMValue Undefined = default;


    private readonly BigInteger m_intValue; // Integer kind: the integer value; Error kind: (int)ErrorCodes
    private readonly decimal m_floatValue; // Float kind: the decimal value


    private VMValue(VMValueKind kind, BigInteger intValue, decimal floatValue)
    {
        Kind = kind;
        m_intValue = intValue;
        m_floatValue = floatValue;
    }


    /// <summary>Creates a <see cref="VMValueKind.Integer" /> value.</summary>
    public static VMValue FromInteger(BigInteger value)
        => new(VMValueKind.Integer, value, 0);

    /// <summary>Creates a <see cref="VMValueKind.Float" /> value.</summary>
    public static VMValue FromFloat(decimal value)
        => new(VMValueKind.Float, default, value);

    /// <summary>Creates a <see cref="VMValueKind.Error" /> value carrying a deferred error code.</summary>
    internal static VMValue FromError(ErrorCodes error)
        => new(VMValueKind.Error, (int)error, 0);


    /// <summary>
    ///     Parses a decimal string (e.g. <c>"5"</c>, <c>"-3"</c>, <c>"5.25"</c>) into a
    ///     <see cref="VMValue" />.
    ///     Strings containing <c>'.'</c> are parsed as <see cref="VMValueKind.Float" />;
    ///     all others as <see cref="VMValueKind.Integer" />.
    /// </summary>
    public static bool TryParse(string text, out VMValue value)
    {
        if (text.Contains('.'))
        {
            if (decimal.TryParse(text, NumberStyles.Number, CultureInfo.InvariantCulture, out var d))
            {
                value = FromFloat(d);
                return true;
            }

            value = Undefined;
            return false;
        }

        if (BigInteger.TryParse(text, out var i))
        {
            value = FromInteger(i);
            return true;
        }

        value = Undefined;
        return false;
    }

    /// <summary>
    ///     Parses a raw hexadecimal string (the digits after <c>%</c>, e.g. <c>"7f"</c>)
    ///     into a <see cref="VMValueKind.Integer" /> value, or returns a
    ///     <see cref="VMValueKind.Error" /> value when the string is invalid.
    /// </summary>
    /// <remarks>
    ///     Hex strings containing a <c>'.'</c> produce
    ///     <see cref="ErrorCodes.VM2010_HexArgumentCannotBeAFloatingPointNumber" />;
    ///     other non-hex strings produce
    ///     <see cref="ErrorCodes.VM2006_HexArgumentIsNotANumber" />.
    ///     The error is surfaced at execution time, not at parse time, so that
    ///     script construction never throws unexpectedly.
    /// </remarks>
    internal static VMValue ParseHex(string hexText)
    {
        if (hexText.Contains('.'))
            return FromError(ErrorCodes.VM2010_HexArgumentCannotBeAFloatingPointNumber);

        if (!long.TryParse(hexText, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var hexVal))
            return FromError(ErrorCodes.VM2006_HexArgumentIsNotANumber);

        return FromInteger(hexVal);
    }


    /// <summary>The kind of this value.</summary>
    public VMValueKind Kind { get; }

    /// <summary>Returns <see langword="true" /> when this slot has no value.</summary>
    public bool IsUndefined => Kind == VMValueKind.Undefined;

    /// <summary>Returns <see langword="true" /> when this value holds an integer.</summary>
    public bool IsInteger => Kind == VMValueKind.Integer;

    /// <summary>Returns <see langword="true" /> when this value holds a decimal float.</summary>
    public bool IsFloat => Kind == VMValueKind.Float;

    /// <summary>Returns <see langword="true" /> when this is a deferred parse error.</summary>
    internal bool IsError => Kind == VMValueKind.Error;

    /// <summary>The deferred error code. Only meaningful when <see cref="IsError" /> is <see langword="true" />.</summary>
    internal ErrorCodes ErrorCode => (ErrorCodes)(int)m_intValue;


    /// <summary>
    ///     Returns the integer value.
    /// </summary>
    /// <exception cref="VMRuntimeException">
    ///     Thrown with <see cref="ErrorCodes.VM2011_InvalidDataTypeAtSpecifiedLocation" /> when
    ///     this value is not an integer.
    /// </exception>
    public BigInteger AsInteger()
    {
        if (Kind != VMValueKind.Integer)
            throw new VMRuntimeException(ErrorCodes.VM2011_InvalidDataTypeAtSpecifiedLocation);
        return m_intValue;
    }

    /// <summary>
    ///     Returns the decimal float value.
    /// </summary>
    /// <exception cref="VMRuntimeException">
    ///     Thrown with <see cref="ErrorCodes.VM2011_InvalidDataTypeAtSpecifiedLocation" /> when
    ///     this value is not a float.
    /// </exception>
    public decimal AsFloat()
    {
        if (Kind != VMValueKind.Float)
            throw new VMRuntimeException(ErrorCodes.VM2011_InvalidDataTypeAtSpecifiedLocation);
        return m_floatValue;
    }


    /// <summary>
    ///     Returns the canonical string representation of this value, matching the format used
    ///     by the string-based API for backward compatibility:
    ///     <list type="bullet">
    ///         <item>Integer: decimal digits (e.g. <c>"11"</c>, <c>"-6"</c>).</item>
    ///         <item>Float: decimal with preserved scale (e.g. <c>"11.50"</c>, <c>"6.5"</c>).</item>
    ///         <item>Undefined / Error: <see cref="string.Empty" />.</item>
    ///     </list>
    /// </summary>
    public override string ToString() =>
        Kind switch
        {
            VMValueKind.Integer => m_intValue.ToString(CultureInfo.InvariantCulture),
            VMValueKind.Float => m_floatValue.ToString(CultureInfo.InvariantCulture),
            _ => string.Empty
        };


    /// <inheritdoc />
    public bool Equals(VMValue other)
    {
        if (Kind != other.Kind)
            return false;

        return Kind switch
        {
            VMValueKind.Integer => m_intValue == other.m_intValue,
            VMValueKind.Float => m_floatValue == other.m_floatValue,
            VMValueKind.Error => m_intValue == other.m_intValue,
            _ => true // both Undefined
        };
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is VMValue other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() =>
        Kind switch
        {
            VMValueKind.Integer => HashCode.Combine(Kind, m_intValue),
            VMValueKind.Float => HashCode.Combine(Kind, m_floatValue),
            _ => (int)Kind
        };

    /// <inheritdoc cref="Equals(VMValue)" />
    public static bool operator ==(VMValue left, VMValue right)
    {
        return left.Equals(right);
    }

    /// <inheritdoc cref="Equals(VMValue)" />
    public static bool operator !=(VMValue left, VMValue right)
    {
        return !left.Equals(right);
    }
}