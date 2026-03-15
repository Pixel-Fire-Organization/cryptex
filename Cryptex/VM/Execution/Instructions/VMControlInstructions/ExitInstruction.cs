using Cryptex.Exceptions;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.VM.Execution.Instructions.VMControlInstructions;

/// <summary>
///     <c>exit X</c> — stops VM execution with exit code <c>X</c>.
///     <para>X must be a decimal-integer constant (<c>#N</c>).</para>
/// </summary>
internal sealed class ExitInstruction : IInstruction
{
    public OpCodes OpCode => OpCodes.Exit;

    public void Execute(ScriptInstruction c, Executor vm)
    {
        if (c.Args.Length != 1 || c.Args[0].Type == InstructionArgumentType.Empty)
            throw new VMRuntimeException(ErrorCodes.VM2002_IncorrectAmountOfArgumentsSuppliedToInstruction);

        if (c.Args[0].Type != InstructionArgumentType.Constant)
            throw new VMRuntimeException(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction);

        var val = vm.GetConstantOrThrow(in c, c.Args[0].Value);

        if (!val.IsInteger)
            throw new VMRuntimeException(ErrorCodes.VM2005_DecimalArgumentIsNotANumber);

        vm.ExitInstructionCall(val.AsInteger());
    }
}