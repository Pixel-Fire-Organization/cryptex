# Constants Block

## Overview

The **Constants Block** is a read-only table of string values stored at the `Script` level.
Instructions reference entries by zero-based index via
[`InstructionArgumentType.Constant`](../../VM/Execution/Scripts/InstructionArgumentType.cs) and
[`InstructionArgumentType.HexConstant`](../../VM/Execution/Scripts/InstructionArgumentType.cs)
arguments.

---

## Storage

```csharp
// Script.Constants is the global constants block for binary .script files.
public string[] Constants { get; set; }
```

When a script is deserialized from a binary `.script` file (via `ScriptFileLoader`), all constants
are stored in `Script.Constants` and individual instructions carry only integer indices into that
array.

When a script is built in code using the **convenience string constructor**
(`new ScriptInstruction(OpCodes.Load, "$1, #5")`), constant values are stored in the instruction's
own `ScriptInstruction.LocalConstants` array (not serialized by MessagePack).

---

## Argument Notation

| Notation | `InstructionArgumentType` | `Value` in `ScriptInstructionArgument` | Stored string                            |
|----------|---------------------------|----------------------------------------|------------------------------------------|
| `#V`     | `Constant`                | Index into constants table             | Verbatim `V` (e.g. `"5"`, `"5.25"`)      |
| `%H`     | `HexConstant`             | Index into constants table             | Raw hex digits `H` (e.g. `"7f"`, `"10"`) |

> **Note:** `%H` constants store the **raw hex string** (without the `%` prefix).  
> Instructions that accept hex operands (e.g. `load`, `and`, `or`, `xor`) call  
> `long.TryParse(hex, NumberStyles.HexNumber, ...)` to convert at execution time.  
> Hex strings that contain a `.` are rejected with  
> `VM2010_HexArgumentCannotBeAFloatingPointNumber`.

---

## Lookup Order

The `Executor.GetConstant(in ScriptInstruction, int index)` method resolves constants in this order:

1. **`ScriptInstruction.LocalConstants`** — inline constants from the convenience constructor.
2. **`Script.Constants`** — the script-level constants block (binary `.script` files).

A `VMRuntimeException(VM2012_InstructionArgumentIsOutOfRange)` is thrown when the index is out of
range in both sources.

---

## Integer vs. Float Type Convention

Because all values in VM memory are stored as strings, the VM uses a simple convention to
distinguish integer values from floating-point values:

| Condition              | Interpretation                                   |
|------------------------|--------------------------------------------------|
| String contains no `.` | **Integer** — parseable by `BigInteger.TryParse` |
| String contains a `.`  | **Float** — parseable by `decimal.TryParse`      |

Instructions that operate only on integers (e.g. `add`, `and`, `shl`) will fail with
`VM2011_InvalidDataTypeAtSpecifiedLocation` if a slot holds a float string.
Float-only instructions (e.g. `addf`, `incf`) require the `.` to be present.

---

## Usage by Instruction Categories

| Category               | Accepts `Constant` | Accepts `HexConstant` | Notes                                |
|------------------------|--------------------|-----------------------|--------------------------------------|
| `load`                 | ✔                  | ✔                     | Second arg may be any source         |
| `and`, `or`, `xor`     | ✔                  | ✔                     | Second arg may be any integer source |
| `nop`, `exit`, `crash` | ✔                  | ✗                     | Decimal integers only                |
| `shl`, `shr`           | ✔                  | ✗                     | Shift amount: decimal integer only   |

---

## Example — Building a Script with Script-Level Constants

```csharp
// Script-level constants block (for binary .script files or explicit construction):
string[] constants = ["42", "3.14"];

Script script = new Script(
    "example",
    [
        new ScriptChunk("main",
        [
            // loadImm $1, X  — stores constants[0] = "42" in slot 1.
            new ScriptInstruction(OpCodes.Load,
            [
                new ScriptInstructionArgument(1, InstructionArgumentType.MemoryAddress),
                new ScriptInstructionArgument(0, InstructionArgumentType.Constant),
            ]),
        ])
    ],
    constants);
```

## Example — Convenience String Constructor (tests / tooling)

```csharp
// Each instruction stores its own LocalConstants — no shared Script.Constants needed.
ScriptChunk main = new ScriptChunk("main",
[
    new ScriptInstruction(OpCodes.Load, "$1, #42"),    // LocalConstants = ["42"]
    new ScriptInstruction(OpCodes.Load, "$2, %2a"),    // LocalConstants = ["2a"] (hex 42)
    new ScriptInstruction(OpCodes.Add,  "$1, $2"),     // no constants
]);

Script script = new Script("demo", [main]);
```

