# Load

## Signature

```asm
load $A, $B
```

## Description

Sets the value of `$A` to the value from `$B`.

## Remarks

- If `$B` is empty/unused, this behaves like [`free`](Free.md).

## Example

```asm
load $0, $1
```