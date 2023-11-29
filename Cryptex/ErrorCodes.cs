namespace Cryptex;

public enum ErrorCodes
{
    SYS1000_ErrorCodeNotFound,
    
    VM1000_NullEmptyStringForExternalFunction,
    
    VM2000_NoChunkFoundToExecute,
    VM2001_WrongOpCodePassedForScriptOpCode,
    VM2002_IncorrectAmountOfArgumentsSuppliedToInstruction,
    VM2003_InvalidArgumentTypeSpecifiedForInstruction,
    VM2004_MemoryArgumentIsNotANumber,
    VM2005_DecimalArgumentIsNotANumber,
    VM2006_HexArgumentIsNotANumber,
    VM2007_InvalidMemoryLocationSpecifiedAsArgument,
    VM2008_InvalidInstructionFoundInScriptChunk,
    VM2009_ArgumentsWithMismatchedTypesSpecified,
    VM2010_HexArgumentCannotBeAFloatingPointNumber,
    VM2011_InvalidDataTypeAtSpecifiedLocation
}
