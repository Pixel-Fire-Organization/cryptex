# OpCodes

## Notations

- `X, Y, Z` - numeric arguments, fetched from the `Constants Block`
- `$A, $B, $C` - memory location arguments
- `[X]` - value of constant in location `X`
- `[$A]` - value of memory location `$A`
- `L` - label, fetched from the `Jump Block`
- `E` - external function, fetched from the `External Executor`

## OP Codes by categories

### VM opcodes.

|     OpCode Name      | Implemented | Since (VM Version) |
|:--------------------:|:-----------:|:------------------:|
|  [Term](VM/Term.md)  |      ✔      |       1.0.0        |
|   [Nop](VM/Nop.md)   |      ✔      |       1.0.0        |
|  [Exit](VM/Exit.md)  |      ✔      |       1.0.0        |
| [Crash](VM/Crash.md) |      ✔      |       1.0.0        |

### Math opcodes.

|       OpCode Name        | Implemented | Since (VM Version) |
|:------------------------:|:-----------:|:------------------:|
|    [Inc](Math/Inc.md)    |      ✔      |       1.0.0        |
|    [Dec](Math/Dec.md)    |      ✔      |       1.0.0        |
|    [Add](Math/Add.md)    |      ✔      |       1.0.0        |
|    [Sub](Math/Sub.md)    |      ✔      |       1.0.0        |
|    [Mul](Math/Mul.md)    |      ✔      |       1.0.0        |
|    [Div](Math/Div.md)    |      ✔      |       1.0.0        |
| [AddImm](Math/AddImm.md) |      ✕      |       1.0.0        |
| [SubImm](Math/SubImm.md) |      ✕      |       1.0.0        |
| [MulImm](Math/MulImm.md) |      ✕      |       1.0.0        |
| [DivImm](Math/DivImm.md) |      ✕      |       1.0.0        |
|   [IncF](Math/IncF.md)   |      ✔      |       1.0.0        |
|   [DecF](Math/DecF.md)   |      ✔      |       1.0.0        |
|   [AddF](Math/AddF.md)   |      ✔      |       1.0.0        |
|   [SubF](Math/SubF.md)   |      ✔      |       1.0.0        |
|   [MulF](Math/MulF.md)   |      ✔      |       1.0.0        |
|   [DivF](Math/DivF.md)   |      ✔      |       1.0.0        |
|    [Mod](Math/Mod.md)    |      ✔      |       1.0.0        |
| [ModImm](Math/ModImm.md) |      ✕      |       1.0.0        |

### Function opcodes.

|       OpCode Name        | Implemented | Since (VM Version) |
|:------------------------:|:-----------:|:------------------:|
|  [Arg](Function/Arg.md)  |      ✕      |       1.0.0        |
| [Exec](Function/Exec.md) |      ✕      |       1.0.0        |
| [Call](Function/Call.md) |      ✕      |       1.0.0        |
|  [Ret](Function/Ret.md)  |      ✕      |       1.0.0        |
|  [Res](Function/Res.md)  |      ✕      |       1.0.0        |

### Logic opcodes.

|       OpCode Name        | Implemented | Since (VM Version) |
|:------------------------:|:-----------:|:------------------:|
|   [Cmp](Logic/Cmp.md)    |      ✕      |       1.0.0        | 
| [Jmp](Logic/Jump/Jmp.md) |      ✕      |       1.0.0        | 
| [Jeq](Logic/Jump/Jeq.md) |      ✕      |       1.0.0        | 
| [Jnq](Logic/Jump/Jnq.md) |      ✕      |       1.0.0        | 
| [Jls](Logic/Jump/Jls.md) |      ✕      |       1.0.0        | 
| [Jgr](Logic/Jump/Jgr.md) |      ✕      |       1.0.0        | 
| [Jge](Logic/Jump/Jge.md) |      ✕      |       1.0.0        | 
| [Jle](Logic/Jump/Jle.md) |      ✕      |       1.0.0        | 

### Bitwise opcodes.

    Shl, //[shl $2, #2 | $2 = $2 << 2]  Shits the value at memory location to the left X amount of times.
    Shr, //[shr $2, #2 | $2 = $2 >> 2]  Shits the value at memory location to the right X amount of times.
    And, //[and $2, $3 | $2 = $2 & $3]  Does `And` operation to the values at memory location1 & location2 and stores the result in memory location1.
    Or,  //[or $2, $3 | $2 = $2 | $3]   Does `Or` operation to the value at memory location1 & location2 and stores the result in memory location1.
    Xor, //[xor $2, $3 | $2 = $2 ^ $3]  Does `Xor` operation to the value at memory location1 & location2 and stores the result in memory location1.
    Not, //[not $2 | $2 = ~$2]          Does `Not` operation to the value at memory location and stores the result in the same memory location.

### Memory opcodes.

    Load,  //[load $1, #1 | $1 = #1 | load $1, $2 | if: $2 = #5 -> $1 = #5] Sets a memory location's value.
    Free,  //[free $1 | $1 = null]                                          Deletes a memory location's value.
    Reg,   //[reg $1, $10]                                                  Sets the current chunk's memory region to the specified addresses. arg1 must be the low bound and arg2 must be the upper bound. Every read/write to other regions(when this instruction is executed) will throw an error.
    UnReg, //[unreg]                                                        Removes the region that is bound to the current chunk. If nothing is bound, nothing happens.

### Array opcodes.

    ArrAccess, //[arraccess $1, $2, #3] Gets the element at position `arg3` of array at location `arg2` and stores it in location `arg1`. 
    ArrCreate, //[arrcreate $1, #10]    Creates an array at location `arg1` with the size of `arg2`.
    ArrFree,   //[arrfree $1]           Deletes array in location `arg1`
    ArrLen,    //[arrlen $1, $2]        Sets the value at `arg1` with the length of the array in `arg2`.
    ArrSet,    //[arrset $1, $2, #3]    Sets the element at position `arg3` of the array at `arg2` to the value at `arg1`.

### String opcodes.

    StrCreate, //[strcreate $1, #20]        Creates a string with `arg2` length at location `arg1`.
    StrSub,    //[strsub $2, $1, #0, #5]    Runs function substring on the string in location `arg2` and places the result in location `arg1`. `arg3` is start, `arg4` is end.
    StrAppend, //[strappend $1, $2]         Appends the string in location `arg2` to the end of the string in location `arg1`.
    StrFree,   //[strfree $1]               Deletes string in location `arg1`.
    StrNum,    //[strnum $2, $1]            Converts the string in location `arg2` to a number and places it in location `arg1`.
    StrChar,   //[strchar $2, $1, #3]       Gets the character at position `arg3` of the string located in `arg2` and places it in location `arg1`.

### Integrated functions.

    Print,    // [print $1]     Prints the contents in the specified memory location in a readable format.
    Read,     // [read $1]      Reads a char from the console and saves it in the specified memory location as an integer.
    ReadLine, // [readline $1]  Reads a string from the console (every character until enter is pressed) and saves it in the specified memory location.
    Random,   // [random $1]    Saves an integer random number in the specified memory location.
    RandomF,  // [randomf $1]   Saves a floating random number [0.0, 1.0] in the specified memory location.