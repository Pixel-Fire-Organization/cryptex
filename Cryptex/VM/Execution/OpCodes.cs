namespace Cryptex.VM.Execution;

public enum OpCodes
{
    //VM opcodes.
    Nop,   //[nop #10]      Tells the VM to wait for X ms. X = [0; (2^32) - 1]
    Exit,  //[exit #0]      Stops the execution of the VM with exit code: X. X = 0 -> exited successfully; X != 0 -> error occured.
    Crash, //[crash #2000]  Tells the VM to error with error code: X. X must be a valid error code.

    //Math opcodes.
    Inc, //[inc $1 | $1 = $1 + 1]           Increments a (decimal/hex) value at memory location.
    Add, //[add $1, $2 | $1 = $1 + $2]      Adds two (decimal/hex) values at memory location1 & location2 and stores the result into location1.
    Sub, //[sub $1, $2 | $1 = $1 - $2]      Subtracts two (decimal/hex) values at memory location1 & location2 and stores the result into location1.
    Dec, //[dec $1 | $1 = $1 - 1]           Decrements a (decimal/hex) value at memory location.
    IncF, //[inc $1 | $1 = $1 + 1]          Increments a (floating point) value at memory location.
    AddF, //[add $1, $2 | $1 = $1 + $2]     Adds two (floating point) values at memory location1 & location2 and stores the result into location1.
    SubF, //[sub $1, $2 | $1 = $1 - $2]     Subtracts two (floating point) values at memory location1 & location2 and stores the result into location1.
    DecF, //[dec $1 | $1 = $1 - 1]          Decrements a (floating point) value at memory location.

    //Function opcodes.
    Arg,  //WIP, idea: use this opcode to load the arguments of a function in memory before calling it.
    Exec, //[exec $1, Foo::Bar | Calls `Bar` in `Foo`]  Executes a function from a registered external class. If the function returns a value it will be saved in location `arg1`.
    Call, //[call $1, fun1 | $1 = fun1()]               Stops the execution of the current chunk, and starts execution of another. If the new chunk has a return value, the return value is pushed in memory specified by the instruction.
    Ret,  //[ret]                                       Stops the execution of the current chunk, and returns to the previous. If this is the last chunk, then the execution stops. Clears all memory locations used in this chunk.
    Res,  //[res #27 | If call address is $1, $1 = #27] Sets the return value. When ret is called, this value is passed to the location of the `Call` instruction.

    //Logic opcodes.
    Cmp, //[cmp $1, $2] Compares the values in memory location1 & location2. Sets a flag in the VM. Next instruction must be a jump.
    Jmp, //[jmp label1] Moves to the label specified in `arg1`. `arg1` must be in the current code chunk.
    Jeq, //[jeq label1] Moves to the label specified in `arg1` if the condition(equals) is true. `arg1` must be in the current code chunk.
    Jnq, //[jnq label1] Moves to the label specified in `arg1` if the condition(not equals) is true. `arg1` must be in the current code chunk.
    Jls, //[jls label1] Moves to the label specified in `arg1` if the condition(less) is true. `arg1` must be in the current code chunk.
    Jgr, //[jgr label1] Moves to the label specified in `arg1` if the condition(greater) is true. `arg1` must be in the current code chunk.
    Jge, //[jge label1] Moves to the label specified in `arg1` if the condition(greater or equal) is true. `arg1` must be in the current code chunk.
    Jle, //[jle label1] Moves to the label specified in `arg1` if the condition(less or equal) is true. `arg1` must be in the current code chunk.

    //Bitwise opcodes.
    Shl, //[shl $2, #2 | $2 = $2 << 2]  Shits the value at memory location to the left X amount of times.
    Shr, //[shr $2, #2 | $2 = $2 >> 2]  Shits the value at memory location to the right X amount of times.
    And, //[and $2, $3 | $2 = $2 & $3]  Does `And` operation to the values at memory location1 & location2 and stores the result in memory location1.
    Or,  //[or $2, $3 | $2 = $2 | $3]   Does `Or` operation to the value at memory location1 & location2 and stores the result in memory location1.
    Not, //[not $2 | $2 = ~$2]          Does `Not` operation to the value at memory location and stores the result in the same memory location.
    Xor, //[xor $2, $3 | $2 = $2 ^ $3]  Does `Not` operation to the value at memory location1 & location2 and stores the result in memory location1.

    //Memory opcodes.
    Load, //[load $1, #1 | $1 = #1 | load $1, $2 | if: $2 = #5 -> $1 = #5] Sets a memory location's value.
    Free, //[free $1 | $1 = null] Deletes a memory location's value.

    //Array opcodes.
    ArrAccess, //[arraccess $1, $2, #3] Gets the element at position `arg3` of array at location `arg2` and stores it in location `arg1`. 
    ArrCreate, //[arrcreate $1, #10]    Creates an array at location `arg1` with the size of `arg2`.
    ArrFree,   //[arrfree $1]           Deletes array in location `arg1`

    //String opcodes.
    StrCreate, //[strcreate $1, #20]        Creates a string with `arg2` length at location `arg1`.
    StrSub,    //[strsub $2, $1, #0, #5]    Runs function substring on the string in location `arg2` and places the result in location `arg1`. `arg3` is start, `arg4` is end.
    StrAppend, //[strappend $1, $2]         Appends the string in location `arg2` to the end of the string in location `arg1`.
    StrFree,   //[strfree $1]               Deletes string in location `arg1`.
    StrNum,    //[strnum $2, $1]            Converts the string in location `arg2` to a number and places it in location `arg1`.
    StrChar,   //[strchar $2, $1, #3]       Gets the character at position `arg3` of the string located in `arg2` and places it in location `arg2`. 

}
