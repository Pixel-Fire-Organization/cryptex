namespace Cryptex;

public enum ErrorCodes
{
    SYS0000_ErrorCodeNotFound = 0,

    VM1000_NullEmptyStringForExternalFunction = 1000,

    VM2000_NoChunkFoundToExecute = 2000,
    VM2001_WrongOpCodePassedForScriptOpCode,
    VM2002_IncorrectAmountOfArgumentsSuppliedToInstruction,
    VM2003_InvalidArgumentTypeSpecifiedForInstruction,
    VM2004_MemoryArgumentIsNotANumber,
    VM2005_DecimalArgumentIsNotANumber,
    VM2006_ConstantsIndexOutOfRange,
    VM2007_InvalidMemoryLocationSpecifiedAsArgument,
    VM2008_InvalidInstructionFoundInScriptChunk,
    VM2009_ArgumentsWithMismatchedTypesSpecified,
    VM2010_HexArgumentCannotBeAFloatingPointNumber,
    VM2011_InvalidDataTypeAtSpecifiedLocation,
    VM2012_InstructionArgumentIsOutOfRange,
    VM2013_UnknownInstructionOverloadSpecified,
    VM2014_InvalidInputProvided,
    VM2015_DivisionByZero,
}

public static class ErrorCodesExtensions
{
    public static string ToMessage(this ErrorCodes code)
    {
        switch (code)
        {
            case ErrorCodes.SYS0000_ErrorCodeNotFound:
                return "Error code is not found! Maybe the error code is not included with the other error messages? Code: ";

            case ErrorCodes.VM1000_NullEmptyStringForExternalFunction:
                return "Tried to invoke an external function, but provided name that is null or empty!";

            case ErrorCodes.VM2000_NoChunkFoundToExecute:
                return "Tried to execute a script chunk that doesn't exist!";
            case ErrorCodes.VM2001_WrongOpCodePassedForScriptOpCode:
                return "Script chunk instruction opcode is not equal to vm instruction!";
            case ErrorCodes.VM2002_IncorrectAmountOfArgumentsSuppliedToInstruction:
                return "Instruction was supplied with incorrect amount of arguments!";
            case ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction:
                return "Instruction was supplied with invalid argument!";
            case ErrorCodes.VM2004_MemoryArgumentIsNotANumber:
                return "Instruction was supplied with memory argument that isn't a memory location!";
            case ErrorCodes.VM2005_DecimalArgumentIsNotANumber:
                return "Instruction was supplied with decimal argument that isn't a decimal number!";
            case ErrorCodes.VM2006_ConstantsIndexOutOfRange:
                return "A constant was being fetched with invalid index by an instruction!";
            case ErrorCodes.VM2007_InvalidMemoryLocationSpecifiedAsArgument:
                return "Instruction was supplied with an argument that isn't a valid memory location!";
            case ErrorCodes.VM2008_InvalidInstructionFoundInScriptChunk:
                return "Invalid instruction was found in a script chunk!";
            case ErrorCodes.VM2009_ArgumentsWithMismatchedTypesSpecified:
                return "Arguments with mismatched types supplied to an instruction!";
            case ErrorCodes.VM2010_HexArgumentCannotBeAFloatingPointNumber:
                return "Floating numbers cannot be in hex format!";
            case ErrorCodes.VM2011_InvalidDataTypeAtSpecifiedLocation:
                return "Data at memory location is not valid or not in the specified format!";
            case ErrorCodes.VM2012_InstructionArgumentIsOutOfRange:
                return "An argument of an instruction is out of it's range!";
            case ErrorCodes.VM2013_UnknownInstructionOverloadSpecified:
                return "An unknown instruction overload is specified in an instruction!";
            case ErrorCodes.VM2014_InvalidInputProvided:
                return "The provided input could not be converted to the expected data type.";
            case ErrorCodes.VM2015_DivisionByZero:
                return "Division by zero attempted.";
        }

        return string.Empty;
    }
}