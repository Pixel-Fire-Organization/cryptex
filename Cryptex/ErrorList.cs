using System.Collections.Frozen;

using Cryptex.Exceptions;
using Cryptex.VM;

namespace Cryptex;

internal static class ErrorList
{
    private static FrozenDictionary<ErrorCodes, string> ErrorMessages { get; } = new Dictionary<ErrorCodes, string>
    {
        { ErrorCodes.SYS0000_ErrorCodeNotFound, "Error code is not found! Maybe the error code is not included with the other error messages? Code: " },
        
        { ErrorCodes.VM1000_NullEmptyStringForExternalFunction, "Tried to invoke an external function, but provided name that is null or empty!" },
        
        { ErrorCodes.VM2000_NoChunkFoundToExecute, "Tried to execute a script chunk that doesn't exist!" },
        { ErrorCodes.VM2001_WrongOpCodePassedForScriptOpCode, "Script chunk instruction opcode is not equal to vm instruction!" },
        { ErrorCodes.VM2002_IncorrectAmountOfArgumentsSuppliedToInstruction, "Instruction was supplied with incorrect amount of arguments!" },
        { ErrorCodes.VM2003_InvalidArgumentTypeSpecifiedForInstruction, "Instruction was supplied with invalid argument!" },
        { ErrorCodes.VM2004_MemoryArgumentIsNotANumber, "Instruction was supplied with memory argument that isn't a memory location!" },
        { ErrorCodes.VM2005_DecimalArgumentIsNotANumber, "Instruction was supplied with decimal argument that isn't a decimal number!" },
        { ErrorCodes.VM2006_HexArgumentIsNotANumber, "Instruction was supplied with hex argument that isn't a hex number!" },
        { ErrorCodes.VM2007_InvalidMemoryLocationSpecifiedAsArgument, "Instruction was supplied with an argument that isn't a valid memory location!" },
        { ErrorCodes.VM2008_InvalidInstructionFoundInScriptChunk, "Invalid instruction was found in a script chunk!" },
        { ErrorCodes.VM2009_ArgumentsWithMismatchedTypesSpecified, "Arguments with mismatched types supplied to an instruction!" },
        { ErrorCodes.VM2010_HexArgumentCannotBeAFloatingPointNumber, "Floating numbers cannot be in hex format!" },
        { ErrorCodes.VM2011_InvalidDataTypeAtSpecifiedLocation, "Data at memory location is not valid or not in the specified format!" },
    }.ToFrozenDictionary();

    public static void WriteError(ErrorCodes code, ErrorCodes? extra = null, bool fatal = false)
    {
        if (!ErrorMessages.TryGetValue(ErrorCodes.SYS0000_ErrorCodeNotFound, out string? sys1000Message))
            throw new RequiredMessageNotFoundException(ErrorCodes.SYS0000_ErrorCodeNotFound);

        if (!ErrorMessages.ContainsKey(code))
        {
            WriteError(ErrorCodes.SYS0000_ErrorCodeNotFound, code, true);
            return;
        }

        ErrorHandler.WriteError(code == ErrorCodes.SYS0000_ErrorCodeNotFound
                                    ? $"{sys1000Message}{(extra is null ? "<Extra is null>" : extra.ToString())}"
                                    : ErrorMessages[code]);
        if (fatal)
        {
            Thread.Sleep(2000);
            Environment.Exit(-1);
        }
    }
}
