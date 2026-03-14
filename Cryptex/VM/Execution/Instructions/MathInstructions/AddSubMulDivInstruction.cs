using System.Globalization;

using Cryptex.Exceptions;
using Cryptex.VM.Execution.DataTypes;

namespace Cryptex.VM.Execution.Instructions.MathInstructions;

internal sealed class AddSubMulDivInstruction : IInstruction
{
    public OpCodes OpCode     => OpCodes.Add;
    public OpCodes OpCodeSub  => OpCodes.Sub;
    public OpCodes OpCodeAddF => OpCodes.AddF;
    public OpCodes OpCodeSubF => OpCodes.SubF;
    public OpCodes OpCodeMul  => OpCodes.Mul;
    public OpCodes OpCodeDiv  => OpCodes.Div;
    public OpCodes OpCodeMulF => OpCodes.MulF;
    public OpCodes OpCodeDivF => OpCodes.DivF;

    public enum InstructionFunction { Add, Subtract, Multiply, Divide }

    public enum ExpectedType { Integer, Floating }

    private readonly InstructionFunction m_function;
    private readonly ExpectedType        m_expectedType;

    public AddSubMulDivInstruction(InstructionFunction f, ExpectedType expectedType)
    {
        m_function     = f;
        m_expectedType = expectedType;
    }

    public void Execute(ScriptChunkOpCode c, Executor vm)
    {
        switch (m_function)
        {
            case InstructionFunction.Add when m_expectedType == ExpectedType.Floating && c.Code != OpCodeAddF:
            case InstructionFunction.Add when m_expectedType == ExpectedType.Integer && c.Code != OpCode:
            case InstructionFunction.Subtract when m_expectedType == ExpectedType.Floating && c.Code != OpCodeSubF:
            case InstructionFunction.Subtract when m_expectedType == ExpectedType.Integer && c.Code != OpCodeSub:
            case InstructionFunction.Multiply when m_expectedType == ExpectedType.Floating && c.Code != OpCodeMulF:
            case InstructionFunction.Multiply when m_expectedType == ExpectedType.Integer && c.Code != OpCodeMul:
            case InstructionFunction.Divide when m_expectedType == ExpectedType.Floating && c.Code != OpCodeDivF:
            case InstructionFunction.Divide when m_expectedType == ExpectedType.Integer && c.Code != OpCodeDiv:
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

        if (result == string.Empty)
            throw new VMRuntimeException(ErrorCodes.VM2013_UnknownInstructionOverloadSpecified);

        vm.GetMemory().SetSlot(location1, result);
    }

    private string CalculateFloating(ExecutorMemory memory, int slot1, int slot2)
    {
        var vd1 = CryptexDataConverter.GetMemoryValueAsFloating(memory, slot1);
        var vd2 = CryptexDataConverter.GetMemoryValueAsFloating(memory, slot2);

        if (vd1 is null || vd2 is null)
            throw new VMRuntimeException(ErrorCodes.VM2011_InvalidDataTypeAtSpecifiedLocation);

        switch (m_function)
        {
            case InstructionFunction.Add:
                return (vd1.Value + vd2.Value).ToString(CultureInfo.InvariantCulture);
            case InstructionFunction.Subtract:
                return (vd1.Value - vd2.Value).ToString(CultureInfo.InvariantCulture);
            case InstructionFunction.Multiply:
                return (vd1.Value * vd2.Value).ToString(CultureInfo.InvariantCulture);
            case InstructionFunction.Divide:
                return (vd1.Value / vd2.Value).ToString(CultureInfo.InvariantCulture);
        }

        return string.Empty;
    }

    private string CalculateInteger(ExecutorMemory memory, int slot1, int slot2)
    {
        var vi1 = CryptexDataConverter.GetMemoryValueAsInteger(memory, slot1);
        var vi2 = CryptexDataConverter.GetMemoryValueAsInteger(memory, slot2);

        if (vi1 is null || vi2 is null)
            throw new VMRuntimeException(ErrorCodes.VM2011_InvalidDataTypeAtSpecifiedLocation);

        switch (m_function)
        {
            case InstructionFunction.Add:
                return (vi1.Value + vi2.Value).ToString(CultureInfo.InvariantCulture);
            case InstructionFunction.Subtract:
                return (vi1.Value - vi2.Value).ToString(CultureInfo.InvariantCulture);
            case InstructionFunction.Multiply:
                return (vi1.Value * vi2.Value).ToString(CultureInfo.InvariantCulture);
            case InstructionFunction.Divide:
                return (vi1.Value / vi2.Value).ToString(CultureInfo.InvariantCulture);
        }

        return string.Empty;
    }
}
