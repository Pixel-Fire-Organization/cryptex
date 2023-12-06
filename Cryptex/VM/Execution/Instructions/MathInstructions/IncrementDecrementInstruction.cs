using System.Globalization;
using System.Numerics;

using Cryptex.Exceptions;
using Cryptex.VM.Execution.DataTypes;

namespace Cryptex.VM.Execution.Instructions.MathInstructions;

internal sealed class IncrementDecrementInstruction : IInstruction
{
    public OpCodes OpCode     => OpCodes.Inc;
    public OpCodes OpCodeDec  => OpCodes.Dec;
    public OpCodes OpCodeIncF => OpCodes.IncF;
    public OpCodes OpCodeDecF => OpCodes.DecF;

    public enum InstructionFunction { Increment, Decrement }

    public enum ExpectedType { Integer, Floating }

    private readonly InstructionFunction m_function;
    private readonly ExpectedType        m_expectedType;

    public IncrementDecrementInstruction(InstructionFunction function, ExpectedType expectedType)
    {
        m_function     = function;
        m_expectedType = expectedType;
    }

    public void Execute(ScriptChunkOpCode c, Executor vm)
    {
        switch (m_function)
        {
            case InstructionFunction.Increment when (c.Code != OpCode && c.Code != OpCodeIncF):
            case InstructionFunction.Decrement when (c.Code != OpCodeDec && c.Code != OpCodeDecF):
                throw new VMRuntimeException(ErrorCodes.VM2001_WrongOpCodePassedForScriptOpCode);
        }

        var args = CryptexDataConverter.SplitInstructionArguments(c.Args, 1);

        string argument = args[0];
        if (!argument.StartsWith(IInstruction.MEMORY_ADDRESS_PREFIX))
            throw new VMRuntimeException(ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction);

        int location = CryptexDataConverter.ParseArgumentToMemoryLocation(argument);
        if (!CryptexDataConverter.IsValidMemoryLocation(vm.GetMemory(), location) || 
            CryptexDataConverter.GetDataTypeAtMemoryLocation(vm.GetMemory(), location) != DataTypes.DataTypes.Number)
            throw new VMRuntimeException(ErrorCodes.VM2007_InvalidMemoryLocationSpecifiedAsArgument);

        string result = m_expectedType == ExpectedType.Integer
                            ? CalculateInteger(vm.GetMemory(), location)
                            : CalculateDecimal(vm.GetMemory(), location);

        vm.GetMemory().SetSlot(location, result);
    }

    string CalculateInteger(ExecutorMemory memory, int slot)
    {
        BigInteger? val = CryptexDataConverter.GetMemoryValueAsInteger(memory, slot);

        if (val is null)
            throw new VMRuntimeException(ErrorCodes.VM2011_InvalidDataTypeAtSpecifiedLocation);

        return m_function == InstructionFunction.Increment
                   ? (val.Value + 1).ToString(CultureInfo.InvariantCulture)
                   : (val.Value - 1).ToString(CultureInfo.InvariantCulture);
    }

    string CalculateDecimal(ExecutorMemory memory, int slot)
    {
        decimal? val = CryptexDataConverter.GetMemoryValueAsFloating(memory, slot);

        if (val is null)
            throw new VMRuntimeException(ErrorCodes.VM2011_InvalidDataTypeAtSpecifiedLocation);

        return m_function == InstructionFunction.Increment
                   ? (val.Value + 1).ToString(CultureInfo.InvariantCulture)
                   : (val.Value - 1).ToString(CultureInfo.InvariantCulture);
    }
}
