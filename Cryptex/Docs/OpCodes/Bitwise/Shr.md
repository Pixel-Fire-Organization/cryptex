# Shr

## Signature

```asm
shr $A, X
```

## Description

Shits the value at `$A` to the right `X` amount of times.

## Remarks

`$A = [$A] >> X`

## Example

```asm
shr $0, 1
```