using System.Numerics;

using Cryptex.VM.Execution.DataTypes;

namespace Cryptex.VM.Execution.OpCodeLogic.VMControlInstructions;

internal sealed class ExitInstruction : IInstruction
{
    public OpCodes OpCode => OpCodes.Exit;

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

        BigInteger exitCode = CryptexDataConverter.GetIntegerNumber(arg);

        //TODO: rework the IInstruction::Execute to include the VM that is the executor of this script, so it can tell it that the script requested an exit.

        return null;
    }
}
