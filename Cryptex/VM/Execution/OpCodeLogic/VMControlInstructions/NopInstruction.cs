using System.Numerics;

using Cryptex.VM.Execution.DataTypes;

namespace Cryptex.VM.Execution.OpCodeLogic.VMControlInstructions;

internal sealed class NopInstruction : IInstruction
{
    public OpCodes OpCode => OpCodes.Nop;

    public object? Execute(ScriptChunkOpCode c, ExecutorMemory memory)
    {
        if (c.Code != OpCode)
            ErrorList.WriteError(ErrorCodes.VM2001_WrongOpCodePassedForScriptOpCode, fatal: true);

        string[]? args = CryptexDataConverter.SplitInstructionArguments(c.Args, 1);
        if (args is null)
            return null;

        //ARG1

        string argument = args[0];
        if (!argument.StartsWith(IInstruction.DECIMAL_VALUE_PREFIX))
            ErrorList.WriteError(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction, fatal: true);

        string arg = argument.Remove(0, 1);
        if (!CryptexDataConverter.IsIntegerNumber(arg))
            ErrorList.WriteError(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction, fatal: true);

        BigInteger time = CryptexDataConverter.GetIntegerNumber(arg);
        if (time < 0)
            time = BigInteger.Abs(time);

        if (time < int.MinValue)
            time = int.MinValue;
        else if (time > int.MaxValue)
            time = int.MaxValue;

        Thread.Sleep((int)time);

        return null;
    }
}
