namespace Cryptex.VM.Execution;

/// <summary>Discriminates the kind of value held by a <see cref="VMValue" />.</summary>
public enum VMValueKind : byte
{
    /// <summary>Slot has not been written; no value is present.</summary>
    Undefined = 0,

    /// <summary>An arbitrary-precision integer (<see cref="System.Numerics.BigInteger" />).</summary>
    Integer,

    /// <summary>A decimal floating-point value (<see cref="decimal" />).</summary>
    Float,

    /// <summary>
    ///     A deferred parse error produced during argument construction.
    ///     The associated <see cref="ErrorCodes" /> value is stored internally and thrown at
    ///     execution time when the constant is first read.
    /// </summary>
    Error
}