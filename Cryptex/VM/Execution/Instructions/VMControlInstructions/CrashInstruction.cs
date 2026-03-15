using Cryptex.Exceptions;
using Cryptex.VM.Execution.Scripts;

namespace Cryptex.VM.Execution.Instructions.VMControlInstructions;

/// <summary>
///     <c>crash X</c> — immediately terminates the VM with the given error code <c>X</c>.
///     <para>X must be a decimal-integer constant (<c>#N</c>) whose value is an <see cref="ErrorCodes" /> member.</para>
/// </summary>
internal sealed class CrashInstruction : IInstruction
{
    public OpCodes OpCode => OpCodes.Crash;

    public void Execute(ScriptInstruction c, Executor vm)
    {
        if (c.Args.Length != 1 || c.Args[0].Type == InstructionArgumentType.Empty)
            throw new VMRuntimeException(ErrorCodes.VM2002_IncorrectAmountOfArgumentsSuppliedToInstruction);

        if (c.Args[0].Type != InstructionArgumentType.Constant)
            throw new VMRuntimeException(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction);

        var val = vm.GetConstantOrThrow(in c, c.Args[0].Value);

        if (!val.IsInteger)
            throw new VMRuntimeException(ErrorCodes.VM2005_DecimalArgumentIsNotANumber);

        throw new VMRuntimeException((ErrorCodes)(int)val.AsInteger());
    }
}