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

|      OpCode Name      | Implemented | Since (VM Version) |
|:---------------------:|:-----------:|:------------------:|
| [Shl](Bitwise/Shl.md) |      ✕      |       1.0.0        |
| [Shr](Bitwise/Shr.md) |      ✕      |       1.0.0        |
| [And](Bitwise/And.md) |      ✕      |       1.0.0        |
|  [Or](Bitwise/Or.md)  |      ✕      |       1.0.0        | 
| [Xor](Bitwise/Xor.md) |      ✕      |       1.0.0        |
| [Not](Bitwise/Not.md) |      ✕      |       1.0.0        |

### Memory opcodes.

|         OpCode Name          | Implemented | Since (VM Version) |
|:----------------------------:|:-----------:|:------------------:|
|    [Load](Memory/Load.md)    |      ✔      |       1.0.0        |
| [LoadImm](Memory/LoadImm.md) |      ✕      |       1.0.0        |
|    [Free](Memory/Free.md)    |      ✔      |       1.0.0        |
|     [Reg](Memory/Reg.md)     |      ✕      |       1.0.0        |
|   [UnReg](Memory/UnReg.md)   |      ✕      |       1.0.0        |

### Array opcodes.

> [!IMPORTANT]
> This API is still in the planning phase and may change in the future.

|           OpCode Name           | Implemented | Since (VM Version) |
|:-------------------------------:|:-----------:|:------------------:|
| [ArrCreate](Array/ArrCreate.md) |      ✕      |         -          |
|   [ArrFree](Array/ArrFree.md)   |      ✕      |         -          |
|    [ArrLen](Array/ArrLen.md)    |      ✕      |         -          |
|    [ArrGet](Array/ArrGet.md)    |      ✕      |         -          |
|    [ArrSet](Array/ArrSet.md)    |      ✕      |         -          |

### String opcodes.

> [!IMPORTANT]
> This API is still in the planning phase and may change in the future.

| OpCode Name | Implemented | Since (VM Version) |
|:-----------:|:-----------:|:------------------:|
|  StrCreate  |      ✕      |         -          |
|   StrSub    |      ✕      |         -          |
|  StrAppend  |      ✕      |         -          |
|   StrFree   |      ✕      |         -          |
|   StrNum    |      ✕      |         -          |
|   StrChar   |      ✕      |         -          |

### Integrated functions.

    Print,    // [print $1]     Prints the contents in the specified memory location in a readable format.
    Read,     // [read $1]      Reads a char from the console and saves it in the specified memory location as an integer.
    ReadLine, // [readline $1]  Reads a string from the console (every character until enter is pressed) and saves it in the specified memory location.
    Random,   // [random $1]    Saves an integer random number in the specified memory location.
    RandomF,  // [randomf $1]   Saves a floating random number [0.0, 1.0] in the specified memory location.