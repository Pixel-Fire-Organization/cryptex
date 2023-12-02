using System.Globalization;

using Cryptex.Exceptions;
using Cryptex.VM.Execution.DataTypes;

namespace Cryptex.VM.Execution.OpCodeLogic.MathInstructions;

internal sealed class AddSubInstruction : IInstruction
{
    public OpCodes OpCode     => OpCodes.Add;
    public OpCodes OpCodeSub  => OpCodes.Sub;
    public OpCodes OpCodeAddF => OpCodes.AddF;
    public OpCodes OpCodeSubF => OpCodes.SubF;

    public enum InstructionFunction { Add, Subtract }

    public enum ExpectedType { Integer, Floating }

    private readonly InstructionFunction m_function;
    private readonly ExpectedType        m_expectedType;

    public AddSubInstruction(InstructionFunction f, ExpectedType expectedType)
    {
        m_function     = f;
        m_expectedType = expectedType;
    }

    public void Execute(ScriptChunkOpCode c, Executor vm)
    {
        switch (m_function)
        {
            case InstructionFunction.Add when (c.Code != OpCode && c.Code != OpCodeAddF):
            case InstructionFunction.Subtract when (c.Code != OpCodeSub && c.Code != OpCodeSubF):
                throw new VMRuntimeException(ErrorCodes.VM2001_WrongOpCodePassedForScriptOpCode);
        }

        var args = CryptexDataConverter.SplitInstructionArguments(c.Args, 2);

        //ARG1

        string argument1 = args[0];
        if (!argument1.StartsWith(IInstruction.MEMORY_ADDRESS_PREFIX))
            throw new VMRuntimeException(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction);

        int location1 = CryptexDataConverter.ParseArgumentToMemoryLocation(argument1);
        if (!CryptexDataConverter.IsValidMemoryLocation(vm.GetMemory(), location1) || 
            CryptexDataConverter.GetDataTypeAtMemoryLocation(vm.GetMemory(), location1) != DataTypes.DataTypes.Number)
            throw new VMRuntimeException(ErrorCodes.VM2007_InvalidMemoryLocationSpecifiedAsArgument);

        //ARG2

        string argument2 = args[1];
        if (!argument2.StartsWith(IInstruction.MEMORY_ADDRESS_PREFIX))
            throw new VMRuntimeException(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction);

        int location2 = CryptexDataConverter.ParseArgumentToMemoryLocation(argument2);
        if (!CryptexDataConverter.IsValidMemoryLocation(vm.GetMemory(), location2) || 
            CryptexDataConverter.GetDataTypeAtMemoryLocation(vm.GetMemory(), location2) != DataTypes.DataTypes.Number)
            throw new VMRuntimeException(ErrorCodes.VM2007_InvalidMemoryLocationSpecifiedAsArgument);

        //Check for mismatched arguments. INT+INT or FLT+FLT allowed!

        if (!CryptexDataConverter.AreMemoryValuesOneTypeNumbers(vm.GetMemory(), location1, location2))
            throw new VMRuntimeException(ErrorCodes.VM2009_ArgumentsWithMismatchedTypesSpecified);

        string result = m_expectedType == ExpectedType.Integer
                            ? CalculateInteger(vm.GetMemory(), location1, location2)
                            : CalculateFloating(vm.GetMemory(), location1, location2);

        vm.GetMemory().SetSlot(location1, result);
    }

    private string CalculateFloating(ExecutorMemory memory, int slot1, int slot2)
    {
        var vd1 = CryptexDataConverter.GetMemoryValueAsFloating(memory, slot1);
        var vd2 = CryptexDataConverter.GetMemoryValueAsFloating(memory, slot2);

        if (vd1 is null || vd2 is null)
            throw new VMRuntimeException(ErrorCodes.VM2011_InvalidDataTypeAtSpecifiedLocation);

        return m_function == InstructionFunction.Add
                   ? (vd1.Value + vd2.Value).ToString(CultureInfo.InvariantCulture)
                   : (vd1.Value - vd2.Value).ToString(CultureInfo.InvariantCulture);
    }

    private string CalculateInteger(ExecutorMemory memory, int slot1, int slot2)
    {
        var vi1 = CryptexDataConverter.GetMemoryValueAsInteger(memory, slot1);
        var vi2 = CryptexDataConverter.GetMemoryValueAsInteger(memory, slot2);

        if (vi1 is null || vi2 is null)
            throw new VMRuntimeException(ErrorCodes.VM2011_InvalidDataTypeAtSpecifiedLocation);

        return m_function == InstructionFunction.Add
                   ? (vi1.Value + vi2.Value).ToString(CultureInfo.InvariantCulture)
                   : (vi1.Value - vi2.Value).ToString(CultureInfo.InvariantCulture);
    }
}
