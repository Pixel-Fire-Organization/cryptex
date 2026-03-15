# Shl

## Signature

```asm
shl $A, X
```

## Description

Shits the value at `$A` to the left `X` amount of times.

## Remarks

`$A = [$A] << X`

## Example

```asm
shl $0, 1
```