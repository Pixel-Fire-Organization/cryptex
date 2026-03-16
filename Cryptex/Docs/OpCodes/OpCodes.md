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
|  [Term](VM/Term.md)  |      ✔      |         1          |
|   [Nop](VM/Nop.md)   |      ✔      |         1          |
|  [Exit](VM/Exit.md)  |      ✔      |         1          |
| [Crash](VM/Crash.md) |      ✔      |         1          |

### Math opcodes.

|       OpCode Name        | Implemented | Since (VM Version) |
|:------------------------:|:-----------:|:------------------:|
|    [Inc](Math/Inc.md)    |      ✔      |         1          |
|    [Dec](Math/Dec.md)    |      ✔      |         1          |
|    [Add](Math/Add.md)    |      ✔      |         1          |
|    [Sub](Math/Sub.md)    |      ✔      |         1          |
|    [Mul](Math/Mul.md)    |      ✔      |         1          |
|    [Div](Math/Div.md)    |      ✔      |         1          |
| [AddImm](Math/AddImm.md) |      ✔      |         1          |
| [SubImm](Math/SubImm.md) |      ✔      |         1          |
| [MulImm](Math/MulImm.md) |      ✔      |         1          |
| [DivImm](Math/DivImm.md) |      ✔      |         1          |
|   [IncF](Math/IncF.md)   |      ✔      |         1          |
|   [DecF](Math/DecF.md)   |      ✔      |         1          |
|   [AddF](Math/AddF.md)   |      ✔      |         1          |
|   [SubF](Math/SubF.md)   |      ✔      |         1          |
|   [MulF](Math/MulF.md)   |      ✔      |         1          |
|   [DivF](Math/DivF.md)   |      ✔      |         1          |
|    [Mod](Math/Mod.md)    |      ✔      |         1          |
| [ModImm](Math/ModImm.md) |      ✔      |         1          |

### Function opcodes.

|       OpCode Name        | Implemented | Since (VM Version) |
|:------------------------:|:-----------:|:------------------:|
|  [Arg](Function/Arg.md)  |      ✕      |         -          |
| [Exec](Function/Exec.md) |      ✕      |         -          |
| [Call](Function/Call.md) |      ✕      |         -          |
|  [Ret](Function/Ret.md)  |      ✕      |         -          |
|  [Res](Function/Res.md)  |      ✕      |         -          |

### Logic opcodes.

|       OpCode Name        | Implemented | Since (VM Version) |
|:------------------------:|:-----------:|:------------------:|
|   [Cmp](Logic/Cmp.md)    |      ✔      |         1          | 
| [Jmp](Logic/Jump/Jmp.md) |      ✔      |         1          | 
| [Jeq](Logic/Jump/Jeq.md) |      ✔      |         1          | 
| [Jnq](Logic/Jump/Jnq.md) |      ✔      |         1          | 
| [Jls](Logic/Jump/Jls.md) |      ✔      |         1          | 
| [Jgr](Logic/Jump/Jgr.md) |      ✔      |         1          | 
| [Jge](Logic/Jump/Jge.md) |      ✔      |         1          | 
| [Jle](Logic/Jump/Jle.md) |      ✔      |         1          |

### Bitwise opcodes.

|      OpCode Name      | Implemented | Since (VM Version) |
|:---------------------:|:-----------:|:------------------:|
| [Shl](Bitwise/Shl.md) |      ✔      |         1          |
| [Shr](Bitwise/Shr.md) |      ✔      |         1          |
| [And](Bitwise/And.md) |      ✔      |         1          |
|  [Or](Bitwise/Or.md)  |      ✔      |         1          | 
| [Xor](Bitwise/Xor.md) |      ✔      |         1          |
| [Not](Bitwise/Not.md) |      ✔      |         1          |

### Memory opcodes.

|         OpCode Name          | Implemented | Since (VM Version) |
|:----------------------------:|:-----------:|:------------------:|
|    [Load](Memory/Load.md)    |      ✔      |         1          |
| [LoadImm](Memory/LoadImm.md) |      ✔      |         1          |
|    [Free](Memory/Free.md)    |      ✔      |         1          |
|     [Reg](Memory/Reg.md)     |      ✕      |         -          |
|   [UnReg](Memory/UnReg.md)   |      ✕      |         -          |

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

|           OpCode Name            | Implemented | Since (VM Version) |
|:--------------------------------:|:-----------:|:------------------:|
| [StrCreate](String/StrCreate.md) |      ✕      |         -          |
|    [StrSub](String/StrSub.md)    |      ✕      |         -          |
| [StrAppend](String/StrAppend.md) |      ✕      |         -          |
|   [StrFree](String/StrFree.md)   |      ✕      |         -          |
|    [StrNum](String/StrNum.md)    |      ✕      |         -          |
|   [StrChar](String/StrChar.md)   |      ✕      |         -          |

### Integrated functions.

|                 OpCode Name                 | Implemented | Since (VM Version) |
|:-------------------------------------------:|:-----------:|:------------------:|
|    [Print](IntegratedFunctions/Print.md)    |      ✔      |         1          |
|     [Read](IntegratedFunctions/Read.md)     |      ✔      |         1          |
| [ReadLine](IntegratedFunctions/ReadLine.md) |      ✔      |         1          |
|   [Random](IntegratedFunctions/Random.md)   |      ✔      |         1          |
|  [RandomF](IntegratedFunctions/RandomF.md)  |      ✔      |         1          |