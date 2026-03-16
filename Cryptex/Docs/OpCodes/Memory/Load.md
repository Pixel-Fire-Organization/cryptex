# Load

## Signature

```asm
load $A, $B
load $A, X
load $A
```

## Description

Sets the value of `$A` to the value from the second operand.

The second operand may be:
- `$B` — a memory address; copies the value from `$B` into `$A`. If `$B` is empty/unused, this behaves like [`free`](Free.md).
- `X` — a constant from the Constants Block (integer or float).

If called with only one argument (`load $A`), the slot at `$A` is freed — equivalent to [`free $A`](Free.md).

## Example

```asm
load $0, $1
load $0, 42
load $0
```