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
    VM2006_HexArgumentIsNotANumber,
    VM2007_InvalidMemoryLocationSpecifiedAsArgument,
    VM2008_InvalidInstructionFoundInScriptChunk,
    VM2009_ArgumentsWithMismatchedTypesSpecified,
    VM2010_HexArgumentCannotBeAFloatingPointNumber,
    VM2011_InvalidDataTypeAtSpecifiedLocation,
    VM2012_InstructionArgumentIsOutOfRange
}
