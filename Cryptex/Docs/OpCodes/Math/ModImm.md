# ModImm

## Signature

```asm
modImm $A, X
```

## Description

Performs the modulo operation on $A with the constant X. Stores the result in $A.

### Remarks
Both must be integer.

`$A = [$A] % [X]`

## Example

```asm
modImm $0, 1
```