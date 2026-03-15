# Reg

## Signature

```asm
reg $A, $B
```

## Description

Sets the current chunk's memory region to the specified addresses.

## Remarks

- `$A` must be the low bound.
- `$B` must be the upper bound.
- `$A < $B`
- Every read/write to other regions(when this instruction is executed) will throw an error.

## Example

```asm
reg $0, $1
```