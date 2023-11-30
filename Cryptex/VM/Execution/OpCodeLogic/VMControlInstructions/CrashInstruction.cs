using System.Numerics;

using Cryptex.VM.Execution.DataTypes;

namespace Cryptex.VM.Execution.OpCodeLogic.VMControlInstructions;

internal sealed class CrashInstruction : IInstruction
{
    public OpCodes OpCode => OpCodes.Crash;

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

        BigInteger code = CryptexDataConverter.GetIntegerNumber(arg);

        if (!Enum.TryParse(code.ToString(), true, out ErrorCodes eCode))
            ErrorList.WriteError(ErrorCodes.VM2012_InstructionArgumentIsOutOfRange, fatal: true);

        ErrorList.WriteError(eCode, fatal: true);

        return null;
    }
}
