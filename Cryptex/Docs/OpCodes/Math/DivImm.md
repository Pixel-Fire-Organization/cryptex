# DivImm

## Signature

```asm
divImm $A, X
```

## Description

Divides integer value at $A and constant X. Stores the result in $A.

### Remarks
`$A = [$A] / [X]`

## Example

```asm
divImm $0, 1
```