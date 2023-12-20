namespace CryptexScriptInspector;

internal sealed class OpCodeDescriptions
{
    private const string TC = InlineTextCreator.TOKEN_COLOR;
    private const string TH = InlineTextCreator.TOKEN_HYPERLINK;
    private const string TE = InlineTextCreator.TOKEN_END;
    
    ///////////////////////////////////////////////
    /// VM
    ///////////////////////////////////////////////
    
    public const string TERM_DESC = $"""
    Signature: {TC}Cyan;[term]{TE}
    Description: Should never be used. Only for VM.
    Arguments: <none>
""";

    public const string NOP_DESC = $"""
    Signature: {TC}Cyan;[nop #arg1]{TE}
    Description: Pauses execution for X milliseconds.
    Arguments: 
        {TC}Chocolate;arg1{TE} → must be an integer value.                                      
""";

    public const string EXIT_DESC = $"""
    Signature: {TC}Cyan;[exit #arg1]{TE}
    Description: Exits the executed program with a specified code.
    Arguments: 
        {TC}Chocolate;arg1{TE} → must be an integer value.                                    
""";

    public const string CRASH_DESC = $"""
    Signature: {TC}Cyan;[crash #arg1]{TE}
    Description: Crashes the VM with an error code. {TH}https://google.bg;Error Codes.{TE}
    Arguments: 
        {TC}Chocolate;arg1{TE} → must be an integer value.                                    
""";
    
    ///////////////////////////////////////////////
    /// MATH
    ///////////////////////////////////////////////
    
    public const string INC_DESC = $"""
    Signature: {TC}Cyan;[inc $arg1]{TE}
    Description: Increments the value in the specified memory location. Value must be an integer.
    Arguments: 
        {TC}Chocolate;arg1{TE} → must be a memory location.                                    
""";
    
    public const string ADD_DESC = $"""
    Signature: {TC}Cyan;[add $arg1, $arg2]{TE}
    Description: Adds the value in location 1 and location 2 and saves the result in location 1. Values must be a integers.
    Arguments: 
        {TC}Chocolate;arg1{TE} → must be a memory location.                                    
        {TC}Chocolate;arg2{TE} → must be a memory location.                                    
""";
    
    public const string SUB_DESC = $"""
    Signature: {TC}Cyan;[sub $arg1, $arg2]{TE}
    Description: Subtracts the value in location 1 and location 2 and saves the result in location 1. Values must be a integers.
    Arguments: 
        {TC}Chocolate;arg1{TE} → must be a memory location.                                    
        {TC}Chocolate;arg2{TE} → must be a memory location.                                    
""";
    
    public const string DEC_DESC = $"""
    Signature: {TC}Cyan;[dec $arg1]{TE}
    Description: Decrements the value in the specified memory location. Value must be an integer.
    Arguments: 
        {TC}Chocolate;arg1{TE} → must be a memory location.                                    
""";
    
    public const string MUL_DESC = $"""
    Signature: {TC}Cyan;[mul $arg1]{TE}
    Description: Multiplies the value in location 1 and location 2 and saves the result in location 1. Values must be an integers.
    Arguments: 
        {TC}Chocolate;arg1{TE} → must be a memory location.                                    
        {TC}Chocolate;arg2{TE} → must be a memory location.                                   
""";
    
    public const string DIV_DESC = $"""
    Signature: {TC}Cyan;[div $arg1]{TE}
    Description: Divides the value in location 1 and location 2 and saves the result in location 1. Values must be a integers.
    Arguments: 
        {TC}Chocolate;arg1{TE} → must be a memory location.                                    
        {TC}Chocolate;arg2{TE} → must be a memory location.                                    
""";
    
        public const string INCF_DESC = $"""
    Signature: {TC}Cyan;[inc $arg1]{TE}
    Description: Increments the value in the specified memory location. Value must be a floating point.
    Arguments: 
        {TC}Chocolate;arg1{TE} → must be a memory location.                                    
""";
    
    public const string ADDF_DESC = $"""
    Signature: {TC}Cyan;[add $arg1, $arg2]{TE}
    Description: Adds the value in location 1 and location 2 and saves the result in location 1. Values must be a floating point.
    Arguments: 
        {TC}Chocolate;arg1{TE} → must be a memory location.                                    
        {TC}Chocolate;arg2{TE} → must be a memory location.                                    
""";
    
    public const string SUBF_DESC = $"""
    Signature: {TC}Cyan;[sub $arg1, $arg2]{TE}
    Description: Subtracts the value in location 1 and location 2 and saves the result in location 1. Values must be a floating point.
    Arguments: 
        {TC}Chocolate;arg1{TE} → must be a memory location.                                    
        {TC}Chocolate;arg2{TE} → must be a memory location.                                    
""";
    
    public const string DECF_DESC = $"""
    Signature: {TC}Cyan;[dec $arg1]{TE}
    Description: Decrements the value in the specified memory location. Value must be a floating point.
    Arguments: 
        {TC}Chocolate;arg1{TE} → must be a memory location.                                    
""";
    
    public const string MULF_DESC = $"""
    Signature: {TC}Cyan;[mul $arg1]{TE}
    Description: Multiplies the value in location 1 and location 2 and saves the result in location 1. Values must be a floating point.
    Arguments: 
        {TC}Chocolate;arg1{TE} → must be a memory location.                                    
        {TC}Chocolate;arg2{TE} → must be a memory location.                                   
""";
    
    public const string DIVF_DESC = $"""
    Signature: {TC}Cyan;[div $arg1]{TE}
    Description: Divides the value in location 1 and location 2 and saves the result in location 1. Values must be a floating point.
    Arguments: 
        {TC}Chocolate;arg1{TE} → must be a memory location.                                    
        {TC}Chocolate;arg2{TE} → must be a memory location.                                    
""";
    
    public const string MOD_DESC = $"""
    Signature: {TC}Cyan;[mod $arg1, $arg2]{TE}
    Description: Performs the modulo operation on the value in location 1 and location 2 and saves the result in location 1. Values must be a integers.
    Arguments: 
        {TC}Chocolate;arg1{TE} → must be a memory location.                                    
        {TC}Chocolate;arg2{TE} → must be a memory location.                                    
""";
    
    ///////////////////////////////////////////////
    /// FUNCTION
    ///////////////////////////////////////////////
    
    public const string ARG_DESC = $"""
    {TC}Red;Work in progress | To be determined!{TE}                                    
""";
    
    public const string EXEC_DESC = $"""
    Signature: {TC}Cyan;[exec $arg1, arg2]{TE}
    Description: Calls the function specified in argument 2 and saves the result in memory location - argument 1.
    Arguments: 
        {TC}Chocolate;arg1{TE} → must be a memory location.                                    
        {TC}Chocolate;arg2{TE} → must be a label. This function is a from a registered class.                                    
""";
    
    public const string CALL_DESC = $"""
    Signature: {TC}Cyan;[call $arg1, arg2]{TE}
    Description: Calls the function specified in argument 2 and saves the result in memory location - argument 1.
    Arguments: 
        {TC}Chocolate;arg1{TE} → must be a memory location.                                    
        {TC}Chocolate;arg2{TE} → must be a label. This function is a part of the script -- it is a chunk.                                
""";
    
    public const string RET_DESC = $"""
    Signature: {TC}Cyan;[ret]{TE}
    Description: Returns from the current chunk. If you need to return a value use the {TC}Cyan;[res]{TE}  instruction to set the return value.
    Arguments: <none>                               
""";
    
    public const string RES_DESC = $"""
    Signature: {TC}Cyan;[res ^arg1]{TE}
    Description: Sets the return value for the current chunk.
    Arguments: 
        {TC}Chocolate;arg1{TE} → can be anything.                      
""";
    
    ///////////////////////////////////////////////
    /// MEMORY
    ///////////////////////////////////////////////
    
    public const string LOAD_DESC = $"""
    Signature: {TC}Cyan;[load $arg1, ^arg2]{TE}
    Description: Used to load a value to a memory location or swap two memory locations.
    Arguments: 
        {TC}Chocolate;arg1{TE} → must be a memory address.                                    
        {TC}Chocolate;arg2{TE} → can be everything. If memory address, it will swap the contents of arg1 and arg2.                                    
""";
}
