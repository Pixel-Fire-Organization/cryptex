using System.Numerics;

using Cryptex.Exceptions;
using Cryptex.VM.Execution.DataTypes;

namespace Cryptex.VM.Execution.Instructions.BitwiseInstructions;

internal sealed class NotInstruction : IInstruction
{
    public OpCodes OpCode => OpCodes.Not;

    public void Execute(ScriptChunkOpCode c, Executor vm)
    {
        if (c.Code != OpCode)
            throw new VMRuntimeException(ErrorCodes.VM2001_WrongOpCodePassedForScriptOpCode);

        var args = CryptexDataConverter.SplitInstructionArguments(c.Args, 1);

        //ARG1

        string argument1 = args[0];
        if (!argument1.StartsWith(IInstruction.MEMORY_ADDRESS_PREFIX))
            throw new VMRuntimeException(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction);

        int location1 = CryptexDataConverter.ParseArgumentToMemoryLocation(argument1);
        if (!CryptexDataConverter.IsValidMemoryLocation(vm.GetMemory(), location1) ||
            CryptexDataConverter.GetDataTypeAtMemoryLocation(vm.GetMemory(), location1) != DataTypes.DataTypes.Number)
            throw new VMRuntimeException(ErrorCodes.VM2007_InvalidMemoryLocationSpecifiedAsArgument);
        
        if (CryptexDataConverter.GetMemoryValueAsInteger(vm.GetMemory(), location1) is null)
            throw new VMRuntimeException(ErrorCodes.VM2011_InvalidDataTypeAtSpecifiedLocation);

        BigInteger valueToOperate = CryptexDataConverter.GetMemoryValueAsInteger(vm.GetMemory(), location1) ?? 0;

        vm.GetMemory().SetSlot(location1, (~valueToOperate).ToString());
    }
}
