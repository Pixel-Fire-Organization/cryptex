using System.Numerics;
using Cryptex.Exceptions;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.VM.Execution.Instructions.BitwiseInstructions;

/// <summary>
///     Shared operand-loading logic for binary bitwise instructions (<c>and</c>, <c>or</c>, <c>xor</c>).
/// </summary>
internal static class BinaryBitwiseHelper
{
    /// <summary>
    ///     Validates argument count and types, then reads and returns both integer operands.
    ///     Both arguments must be <see cref="InstructionArgumentType.MemoryAddress" /> holding an integer.
    /// </summary>
    internal static (BigInteger a, BigInteger b) ReadOperands(in ScriptInstruction c, Executor vm)
    {
        if (c.Args.Length != 2)
            throw new VMRuntimeException(ErrorCodes.VM2002_IncorrectAmountOfArgumentsSuppliedToInstruction);

        if (c.Args[0].Type != InstructionArgumentType.MemoryAddress ||
            c.Args[1].Type != InstructionArgumentType.MemoryAddress)
            throw new VMRuntimeException(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction);

        var memory = vm.GetMemory();
        var valA = memory.GetSlot(c.Args[0].Value);
        var valB = memory.GetSlot(c.Args[1].Value);

        if (valA.IsUndefined || valB.IsUndefined)
            throw new VMRuntimeException(ErrorCodes.VM2007_InvalidMemoryLocationSpecifiedAsArgument);

        if (!valA.IsInteger || !valB.IsInteger)
            throw new VMRuntimeException(ErrorCodes.VM2011_InvalidDataTypeAtSpecifiedLocation);

        return (valA.AsInteger(), valB.AsInteger());
    }

    internal static ExecutorMemory GetMemory(Executor vm) => vm.GetMemory();
}