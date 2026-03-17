namespace Cryptex;

public enum ErrorCodes
{
    Sys0000ErrorCodeNotFound = 0,

    Vm1000NullEmptyStringForExternalFunction = 1000,

    Vm2000NoChunkFoundToExecute = 2000,
    Vm2001WrongOpCodePassedForScriptOpCode,
    Vm2002IncorrectAmountOfArgumentsSuppliedToInstruction,
    Vm2003InvalidArgumentTypeSpecifiedForInstruction,
    Vm2004MemoryArgumentIsNotANumber,
    Vm2005DecimalArgumentIsNotANumber,
    Vm2006ConstantsIndexOutOfRange,
    Vm2007InvalidMemoryLocationSpecifiedAsArgument,
    Vm2008InvalidInstructionFoundInScriptChunk,
    Vm2009ArgumentsWithMismatchedTypesSpecified,
    Vm2010HexArgumentCannotBeAFloatingPointNumber,
    Vm2011InvalidDataTypeAtSpecifiedLocation,
    Vm2012InstructionArgumentIsOutOfRange,
    Vm2013UnknownInstructionOverloadSpecified,
    Vm2014InvalidInputProvided,
    Vm2015DivisionByZero,
}

public static class ErrorCodesExtensions
{
    public static string ToMessage(this ErrorCodes code)
    {
        switch (code)
        {
            case ErrorCodes.Sys0000ErrorCodeNotFound:
                return "Error code is not found! Maybe the error code is not included with the other error messages? Code: ";

            case ErrorCodes.Vm1000NullEmptyStringForExternalFunction:
                return "Tried to invoke an external function, but provided name that is null or empty!";

            case ErrorCodes.Vm2000NoChunkFoundToExecute:
                return "Tried to execute a script chunk that doesn't exist!";
            case ErrorCodes.Vm2001WrongOpCodePassedForScriptOpCode:
                return "Script chunk instruction opcode is not equal to vm instruction!";
            case ErrorCodes.Vm2002IncorrectAmountOfArgumentsSuppliedToInstruction:
                return "Instruction was supplied with incorrect amount of arguments!";
            case ErrorCodes.Vm2003InvalidArgumentTypeSpecifiedForInstruction:
                return "Instruction was supplied with invalid argument!";
            case ErrorCodes.Vm2004MemoryArgumentIsNotANumber:
                return "Instruction was supplied with memory argument that isn't a memory location!";
            case ErrorCodes.Vm2005DecimalArgumentIsNotANumber:
                return "Instruction was supplied with decimal argument that isn't a decimal number!";
            case ErrorCodes.Vm2006ConstantsIndexOutOfRange:
                return "A constant was being fetched with invalid index by an instruction!";
            case ErrorCodes.Vm2007InvalidMemoryLocationSpecifiedAsArgument:
                return "Instruction was supplied with an argument that isn't a valid memory location!";
            case ErrorCodes.Vm2008InvalidInstructionFoundInScriptChunk:
                return "Invalid instruction was found in a script chunk!";
            case ErrorCodes.Vm2009ArgumentsWithMismatchedTypesSpecified:
                return "Arguments with mismatched types supplied to an instruction!";
            case ErrorCodes.Vm2010HexArgumentCannotBeAFloatingPointNumber:
                return "Floating numbers cannot be in hex format!";
            case ErrorCodes.Vm2011InvalidDataTypeAtSpecifiedLocation:
                return "Data at memory location is not valid or not in the specified format!";
            case ErrorCodes.Vm2012InstructionArgumentIsOutOfRange:
                return "An argument of an instruction is out of it's range!";
            case ErrorCodes.Vm2013UnknownInstructionOverloadSpecified:
                return "An unknown instruction overload is specified in an instruction!";
            case ErrorCodes.Vm2014InvalidInputProvided:
                return "The provided input could not be converted to the expected data type.";
            case ErrorCodes.Vm2015DivisionByZero:
                return "Division by zero attempted.";
        }

        return string.Empty;
    }
}
