using System.Globalization;
using System.Numerics;
using Cryptex.Exceptions;

namespace Cryptex.VM.Execution;

/// <summary>
///     A discriminated-union value type used throughout the VM for both constants and memory slots.
/// </summary>
public readonly struct VmValue : IEquatable<VmValue>
{
    public static readonly VmValue Undefined = default;

    private readonly BigInteger m_intValue;
    private readonly decimal m_floatValue;
    private readonly string? m_stringValue;

    private VmValue(VmValueKind kind, BigInteger intValue, decimal floatValue, string? stringValue)
    {
        Kind = kind;
        m_intValue = intValue;
        m_floatValue = floatValue;
        m_stringValue = stringValue;
    }

    public static VmValue FromInteger(BigInteger value)
        => new(VmValueKind.Integer, value, 0, null);

    public static VmValue FromFloat(decimal value)
        => new(VmValueKind.Float, default, value, null);

    public static VmValue FromString(string value)
        => new(VmValueKind.String, default, 0, value);

    public VmValueKind Kind { get; }

    public bool IsUndefined => Kind == VmValueKind.Undefined;
    public bool IsInteger   => Kind == VmValueKind.Integer;
    public bool IsFloat     => Kind == VmValueKind.Float;
    public bool IsString    => Kind == VmValueKind.String;

    public BigInteger AsInteger()
    {
        if (Kind != VmValueKind.Integer)
            throw new VmRuntimeException(ErrorCodes.VM2011_InvalidDataTypeAtSpecifiedLocation);
        return m_intValue;
    }

    public decimal AsFloat()
    {
        if (Kind != VmValueKind.Float)
            throw new VmRuntimeException(ErrorCodes.VM2011_InvalidDataTypeAtSpecifiedLocation);
        return m_floatValue;
    }

    public string AsString()
    {
        if (Kind != VmValueKind.String)
            throw new VmRuntimeException(ErrorCodes.VM2011_InvalidDataTypeAtSpecifiedLocation);
        return m_stringValue!;
    }

    /// <summary>
    ///     Returns the canonical string representation of this value, matching the format used
    ///     by the string-based API for backward compatibility:
    ///     <list type="bullet">
    ///         <item>Integer: decimal digits (e.g. <c>"11"</c>, <c>"-6"</c>).</item>
    ///         <item>Float: decimal with preserved scale (e.g. <c>"11.50"</c>, <c>"6.5"</c>).</item>
    ///     </list>
    /// </summary>
    public override string ToString() =>
        Kind switch
        {
            VmValueKind.Integer => m_intValue.ToString(CultureInfo.InvariantCulture),
            VmValueKind.Float   => m_floatValue.ToString(CultureInfo.InvariantCulture),
            VmValueKind.String  => m_stringValue ?? string.Empty,
            _                   => string.Empty
        };

    /// <inheritdoc />
    public bool Equals(VmValue other)
    {
        if (Kind != other.Kind)
            return false;

        return Kind switch
        {
            VmValueKind.Integer => m_intValue == other.m_intValue,
            VmValueKind.Float   => m_floatValue == other.m_floatValue,
            VmValueKind.String  => m_stringValue == other.m_stringValue,
            _                   => true // both Undefined
        };
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is VmValue other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() =>
        Kind switch
        {
            VmValueKind.Integer => HashCode.Combine(Kind, m_intValue),
            VmValueKind.Float   => HashCode.Combine(Kind, m_floatValue),
            VmValueKind.String  => HashCode.Combine(Kind, m_stringValue),
            _                   => (int)Kind
        };

    /// <inheritdoc cref="Equals(VmValue)" />
    public static bool operator ==(VmValue left, VmValue right) { return left.Equals(right); }

    /// <inheritdoc cref="Equals(VmValue)" />
    public static bool operator !=(VmValue left, VmValue right) { return !left.Equals(right); }
}