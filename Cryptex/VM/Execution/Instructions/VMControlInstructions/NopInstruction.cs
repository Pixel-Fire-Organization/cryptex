using Cryptex.Exceptions;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.VM.Execution.Instructions.VMControlInstructions;

/// <summary>
///     <c>nop X</c> — suspends VM execution for <c>X</c> milliseconds.
///     <para>X must be a decimal-integer constant (<c>#N</c>); range [0, 2^31−1].</para>
/// </summary>
internal sealed class NopInstruction : IInstruction
{
    public OpCodes OpCode => OpCodes.Nop;

    public void Execute(ScriptInstruction c, Executor vm)
    {
        if (c.Args.Length != 1 || c.Args[0].Type == InstructionArgumentType.Empty)
            throw new VMRuntimeException(ErrorCodes.VM2002_IncorrectAmountOfArgumentsSuppliedToInstruction);

        if (c.Args[0].Type != InstructionArgumentType.Constant)
            throw new VMRuntimeException(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction);

        var val = vm.GetConstantOrThrow(in c, c.Args[0].Value);

        if (!val.IsInteger)
            throw new VMRuntimeException(ErrorCodes.VM2005_DecimalArgumentIsNotANumber);

        var ms = val.AsInteger();
        if (ms < 0 || ms > int.MaxValue)
            throw new VMRuntimeException(ErrorCodes.VM2012_InstructionArgumentIsOutOfRange);

        Thread.Sleep((int)ms);
    }
}