# SubImm

## Signature

```asm
subImm $A, X
```

## Description

Subtracts integer value at `$A` and constant `X`. Stores the result in `$A`.

### Remarks
`$A = [$A] - [X]`

## Example

```asm
subImm $0, 1
```