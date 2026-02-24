# VM Flags

## Compare

Used to coordinate logical jumps.

### Set Condition

The `cmp` instruction sets the flag (write-only) depending on the outcome of the arguments.

### Clear Condition

- All [Jump Instruction](../OpCodes/Logic/JumpInstructions.md) read the `Compare` flag. 
- If the `Required Condition` is met, the jump is executed.
- After the flag is read, the flag is cleared -- set to value `None`.
- ***The `Jmp` instruction doesn't read the compare flag. Other actions are still performed.***

### Values

|     Name      |                   Description                    |
|:-------------:|:------------------------------------------------:|
|     None      | Initial and default value. No action considered. |
|    Equals     |              Two values are equal.               |
|   NotEquals   |          Two values are **not** equal.           |
|    Greater    |      Left value is greater then the right.       |
|     Less      |        Left value is less then the right.        |
| GreaterEquals |   Left value is greater or equal to the right.   |
|  LessEquals   |    Left value is less or equal to the right.     |