# Byte Code

## Styling

* `#x` - x is a numerical literal
* `$x` - x is a memory location
* `x` - x is a label

## List of instructions

### VM instructions

| Name       |                                                                       Description                                                                        |    Page    | ID | Last Change |
|------------|:--------------------------------------------------------------------------------------------------------------------------------------------------------:|:----------:|:--:|:-----------:|
| term       | Terminates the execution of the VM. This opcode must be never used by a programmer. It is used as a fail safe of the VM when the chunk has invalid data. | [](Add.md) | 0  |    Never    |
| nop `#x`   |                                                    Tells the VM to wait for x ms. x = [0; (2^32) - 1]                                                    | [](Add.md) | 1  |    Never    |
| exit `#x`  |                         Stops the execution of the VM with exit code: x. x = 0 -> exited successfully; x != 0 -> error occurred.                         | [](Add.md) | 2  |    Never    |
| crash `#x` |                                         Tells the VM to error with error code: X. X must be a valid error code.                                          | [](Add.md) | 3  |    Never    |

### Math instructions

| Name          |                             Description                             |    Page    | ID | Last Change |
|---------------|:-------------------------------------------------------------------:|:----------:|:--:|:-----------:|
| Inc `$x`      |                    Increments the value in `$x`.                    | [](Add.md) | 4  |    Never    |
| Add `$x, $y`  |          Adds two numbers together, result stored in `$x`.          | [](Add.md) | 5  |    Never    |
| Sub `$x, $y`  |            Subtracts the numbers, result stored in `$x`.            | [](Add.md) | 6  |    Never    |
| Dec `$x`      |                    Decrements the value in `$x`.                    | [](Add.md) | 7  |    Never    |
| Mul `$x, $y`  |         Multiplies the value in `$x` by the value in `$y`.          | [](Add.md) | 8  |    Never    |
| Div `$x, $y`  |           Divides the value in `$x` by the value in `$y`.           | [](Add.md) | 9  |    Never    |
| IncF `$x`     |           Same as `inc`, but for floating point numbers.            | [](Add.md) | 10 |    Never    |
| AddF `$x, $y` |           Same as `add`, but for floating point numbers.            | [](Add.md) | 11 |    Never    |
| SubF `$x, $y` |           Same as `sub`, but for floating point numbers.            | [](Add.md) | 12 |    Never    |
| DecF `$x`     |           Same as `dec`, but for floating point numbers.            | [](Add.md) | 13 |    Never    |
| MulF `$x. $y` |           Same as `mul`, but for floating point numbers.            | [](Add.md) | 14 |    Never    |
| DivF `$x, $y` |           Same as `div`, but for floating point numbers.            | [](Add.md) | 15 |    Never    |
| Mod `$x, $y`  | Performs a modulo operation in `$x` by `$y`. Both must be integers. | [](Add.md) | 16 |    Never    |

### Function instructions

| Name                |                                                                                 Description                                                                                 |    Page    | ID | Last Change |
|---------------------|:---------------------------------------------------------------------------------------------------------------------------------------------------------------------------:|:----------:|:--:|:-----------:|
| Arg                 |                                                                                 ***W I P***                                                                                 | [](Add.md) | 17 |    Never    | 
| Exec `$x, Foo::Bar` |                          Executes a function from a registered external class. If the function returns a value it will be saved in location `$x`.                           | [](Add.md) | 18 |    Never    |
| Call `$x, fun`      |             Stops the execution of the current chunk, and starts execution of another. If the new chunk has a return value, the return value is pushed in `$x`.             | [](Add.md) | 19 |    Never    |
| Ret                 | Stops the execution of the current chunk, and returns to the previous. If this is the last chunk, then the execution stops. Clears all memory locations used in this chunk. | [](Add.md) | 20 |    Never    |
| Res `#x`            |                                 Sets the return value. When ret is called, this value is passed to the location of the `Call` instruction.                                  | [](Add.md) | 21 |    Never    |

### Logic instructions

| Name         |                                       Description                                        |    Page    | ID | Last Change |
|--------------|:----------------------------------------------------------------------------------------:|:----------:|:--:|:-----------:|
| Cmp `$x, $y` |    Compares the values in `$x` and `$y`. Next instruction must be a jump instruction.    | [](Add.md) | 22 |    Never    | 
| Jmp `label`  |     Jumps (no condition) to the specified label. `label` must be in the same chunk.      | [](Add.md) | 23 |    Never    |
| Jeq `label`  |         Jumps (equal) to the specified label. `label` must be in the same chunk.         | [](Add.md) | 24 |    Never    |
| Jnq `label`  |       Jumps (not equal) to the specified label. `label` must be in the same chunk.       | [](Add.md) | 25 |    Never    |
| Jls `label`  |       Jumps (less than) to the specified label. `label` must be in the same chunk.       | [](Add.md) | 26 |    Never    |
| Jgr `label`  |     Jumps (greater than) to the specified label. `label` must be in the same chunk.      | [](Add.md) | 27 |    Never    |
| Jge `label`  | Jumps (greater or equal than) to the specified label. `label` must be in the same chunk. | [](Add.md) | 28 |    Never    |
| Jle `label`  |  Jumps (less or equal than) to the specified label. `label` must be in the same chunk.   | [](Add.md) | 29 |    Never    |

### Bitwise instructions

| Name         |                             Description                              |    Page    | ID | Last Change |
|--------------|:--------------------------------------------------------------------:|:----------:|:--:|:-----------:|
| Shl `$x, #y` |             Shifts left the value in `$x` by `#y` times.             | [](Add.md) | 30 |    Never    | 
| Shr `$x, #y` |            Shifts right the value in `$x` by `#y` times.             | [](Add.md) | 31 |    Never    |
| And `$x, $y` | Performs `and`<sup>1</sup> operation on the values in `$x` and `$y`. | [](Add.md) | 32 |    Never    |
| Or `$x, $y`  | Performs `or`<sup>1</sup> operation on the values in `$x` and `$y`.  | [](Add.md) | 33 |    Never    |
| Xor `$x, $y` | Performs `xor`<sup>1</sup> operation on the values in `$x` and `$y`. | [](Add.md) | 34 |    Never    |
| Not `$x`     |                      Inverts the value in `$x`.                      | [](Add.md) | 35 |    Never    |

#### Additional info

1. Truth tables.

| X | Y | And `(X&Y)` | Or `(X|Y)` | Xor `(X^Y)` |
|---|---|:-----------:|:----------:|:-----------:|
| 0 | 0 | 0 | 0 | 0 |
| 1 | 0 | 0 | 1 | 1 |
| 0 | 1 | 0 | 1 | 1 |
| 1 | 1 | 1 | 1 | 0 |