using System.Numerics;
using Cryptex.Exceptions;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.VM.Execution.Instructions.MathInstructions;

/// <summary>
///     <c>mod $A, $B</c> — stores <c>[$A] % [$B]</c> in slot <c>$A</c>.
///     Both operands must be integers.
/// </summary>
internal sealed class ModInstruction : IInstruction
{
    public OpCodes OpCode => OpCodes.Mod;

    public void Execute(ScriptInstruction c, Executor vm)
    {
        if (c.Args.Length != 2)
            throw new VMRuntimeException(ErrorCodes.VM2002_IncorrectAmountOfArgumentsSuppliedToInstruction);

        if (c.Args[0].Type != InstructionArgumentType.MemoryAddress ||
            c.Args[1].Type != InstructionArgumentType.MemoryAddress)
            throw new VMRuntimeException(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction);

        var memory = vm.GetMemory();
        var a = memory.GetSlot(c.Args[0].Value);
        var b = memory.GetSlot(c.Args[1].Value);

        if (a.IsUndefined || b.IsUndefined)
            throw new VMRuntimeException(ErrorCodes.VM2007_InvalidMemoryLocationSpecifiedAsArgument);

        if (!a.IsInteger || !b.IsInteger)
            throw new VMRuntimeException(ErrorCodes.VM2011_InvalidDataTypeAtSpecifiedLocation);

        var divisor = b.AsInteger();
        if (divisor == BigInteger.Zero)
            throw new VMRuntimeException(ErrorCodes.VM2011_InvalidDataTypeAtSpecifiedLocation);

        memory.SetSlot(c.Args[0].Value, VMValue.FromInteger(a.AsInteger() % divisor));
    }
}