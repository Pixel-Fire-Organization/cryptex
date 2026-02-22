# MulImm

## Signature

```asm
mulImm $A, X
```

## Description

Multiplies integer value at $A and constant X. Stores the result in $A.

### Remarks
`$A = [$A] * [X]`

## Example

```asm
mulImm $0, 1
```