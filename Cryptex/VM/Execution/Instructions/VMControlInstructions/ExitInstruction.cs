using System.Numerics;

using Cryptex.Exceptions;
using Cryptex.VM.Execution.DataTypes;

namespace Cryptex.VM.Execution.Instructions.VMControlInstructions;

internal sealed class ExitInstruction : IInstruction
{
    public OpCodes OpCode => OpCodes.Exit;

    public void Execute(ScriptChunkOpCode c, Executor vm)
    {
        if (c.Code != OpCode)
            throw new VMRuntimeException(ErrorCodes.VM2001_WrongOpCodePassedForScriptOpCode);

        string[] args = CryptexDataConverter.SplitInstructionArguments(c.Args, 1);

        //ARG1

        string argument = args[0];
        if (!argument.StartsWith(IInstruction.DECIMAL_VALUE_PREFIX))
            throw new VMRuntimeException(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction);

        string arg = argument.Remove(0, 1);
        if (!CryptexDataConverter.IsIntegerNumber(arg))
            throw new VMRuntimeException(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction);

        BigInteger exitCode = CryptexDataConverter.GetIntegerNumber(arg);
        vm.ExitInstructionCall(exitCode);
    }
}
