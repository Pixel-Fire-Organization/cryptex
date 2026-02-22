# Mul

## Signature

```asm
mul $A, $B
```

## Description

Multiplies two integer values at $A and $B. Stores the result in $A.

### Remarks
`$A = [$A] * [$B]`

## Example

```asm
mul $0, $1
```