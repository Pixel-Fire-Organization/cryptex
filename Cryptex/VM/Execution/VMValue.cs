using System.Globalization;
using System.Numerics;
using Cryptex.Exceptions;

namespace Cryptex.VM.Execution;

/// <summary>
///     A discriminated-union value type used throughout the VM for both constants and memory slots.
/// </summary>
public readonly struct VMValue : IEquatable<VMValue>
{
    public static readonly VMValue Undefined = default;

    private readonly BigInteger m_intValue;
    private readonly decimal m_floatValue;
    private readonly string? m_stringValue;

    private VMValue(VMValueKind kind, BigInteger intValue, decimal floatValue, string? stringValue)
    {
        Kind = kind;
        m_intValue = intValue;
        m_floatValue = floatValue;
        m_stringValue = stringValue;
    }

    public static VMValue FromInteger(BigInteger value)
        => new(VMValueKind.Integer, value, 0, null);

    public static VMValue FromFloat(decimal value)
        => new(VMValueKind.Float, default, value, null);

    public static VMValue FromString(string value)
        => new(VMValueKind.String, default, 0, value);

    public VMValueKind Kind { get; }

    public bool IsUndefined => Kind == VMValueKind.Undefined;
    public bool IsInteger   => Kind == VMValueKind.Integer;
    public bool IsFloat     => Kind == VMValueKind.Float;
    public bool IsString    => Kind == VMValueKind.String;

    public BigInteger AsInteger()
    {
        if (Kind != VMValueKind.Integer)
            throw new VMRuntimeException(ErrorCodes.VM2011_InvalidDataTypeAtSpecifiedLocation);
        return m_intValue;
    }

    public decimal AsFloat()
    {
        if (Kind != VMValueKind.Float)
            throw new VMRuntimeException(ErrorCodes.VM2011_InvalidDataTypeAtSpecifiedLocation);
        return m_floatValue;
    }

    public string AsString()
    {
        if (Kind != VMValueKind.String)
            throw new VMRuntimeException(ErrorCodes.VM2011_InvalidDataTypeAtSpecifiedLocation);
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
            VMValueKind.Integer => m_intValue.ToString(CultureInfo.InvariantCulture),
            VMValueKind.Float   => m_floatValue.ToString(CultureInfo.InvariantCulture),
            VMValueKind.String  => m_stringValue ?? string.Empty,
            _                   => string.Empty
        };

    /// <inheritdoc />
    public bool Equals(VMValue other)
    {
        if (Kind != other.Kind)
            return false;

        return Kind switch
        {
            VMValueKind.Integer => m_intValue == other.m_intValue,
            VMValueKind.Float   => m_floatValue == other.m_floatValue,
            VMValueKind.String  => m_stringValue == other.m_stringValue,
            _                   => true // both Undefined
        };
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is VMValue other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() =>
        Kind switch
        {
            VMValueKind.Integer => HashCode.Combine(Kind, m_intValue),
            VMValueKind.Float   => HashCode.Combine(Kind, m_floatValue),
            VMValueKind.String  => HashCode.Combine(Kind, m_stringValue),
            _                   => (int)Kind
        };

    /// <inheritdoc cref="Equals(VMValue)" />
    public static bool operator ==(VMValue left, VMValue right) => left.Equals(right);

    /// <inheritdoc cref="Equals(VMValue)" />
    public static bool operator !=(VMValue left, VMValue right) => !left.Equals(right);
}