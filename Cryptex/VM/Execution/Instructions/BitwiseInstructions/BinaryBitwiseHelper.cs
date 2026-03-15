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
    ///     Arg 0 must be a <see cref="InstructionArgumentType.MemoryAddress" /> holding an integer.
    ///     Arg 1 may be <see cref="InstructionArgumentType.MemoryAddress" />,
    ///     <see cref="InstructionArgumentType.Constant" />, or
    ///     <see cref="InstructionArgumentType.HexConstant" />.
    /// </summary>
    internal static (BigInteger a, BigInteger b) ReadOperands(in ScriptInstruction c, Executor vm)
    {
        if (c.Args.Length != 2)
            throw new VMRuntimeException(ErrorCodes.VM2002_IncorrectAmountOfArgumentsSuppliedToInstruction);

        if (c.Args[0].Type != InstructionArgumentType.MemoryAddress)
            throw new VMRuntimeException(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction);

        var memory = vm.GetMemory();
        var valA = memory.GetSlot(c.Args[0].Value);

        if (valA.IsUndefined)
            throw new VMRuntimeException(ErrorCodes.VM2007_InvalidMemoryLocationSpecifiedAsArgument);

        if (!valA.IsInteger)
            throw new VMRuntimeException(ErrorCodes.VM2011_InvalidDataTypeAtSpecifiedLocation);

        var b = ReadSecondOperand(in c, vm, memory);
        return (valA.AsInteger(), b);
    }

    internal static ExecutorMemory GetMemory(Executor vm) => vm.GetMemory();

    private static BigInteger ReadSecondOperand(
        in ScriptInstruction c, Executor vm, ExecutorMemory memory)
    {
        switch (c.Args[1].Type)
        {
            case InstructionArgumentType.MemoryAddress:
            {
                var valB = memory.GetSlot(c.Args[1].Value);
                if (valB.IsUndefined)
                    throw new VMRuntimeException(ErrorCodes.VM2007_InvalidMemoryLocationSpecifiedAsArgument);
                if (!valB.IsInteger)
                    throw new VMRuntimeException(ErrorCodes.VM2011_InvalidDataTypeAtSpecifiedLocation);
                return valB.AsInteger();
            }

            case InstructionArgumentType.Constant:
            {
                // Pre-parsed: a float constant (e.g. #10.5) will have IsInteger == false.
                var constVal = vm.GetConstantOrThrow(in c, c.Args[1].Value);
                if (!constVal.IsInteger)
                    throw new VMRuntimeException(ErrorCodes.VM2011_InvalidDataTypeAtSpecifiedLocation);
                return constVal.AsInteger();
            }

            case InstructionArgumentType.HexConstant:
            {
                // GetConstantOrThrow propagates any deferred error (e.g. %5.5 → VM2010).
                var hexVal = vm.GetConstantOrThrow(in c, c.Args[1].Value);
                if (!hexVal.IsInteger)
                    throw new VMRuntimeException(ErrorCodes.VM2011_InvalidDataTypeAtSpecifiedLocation);
                return hexVal.AsInteger();
            }

            default:
                throw new VMRuntimeException(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction);
        }
    }
}