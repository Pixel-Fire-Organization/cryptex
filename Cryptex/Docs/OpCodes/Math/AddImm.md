# AddImm

## Signature

```asm
addImm $A, X
```

## Description

Adds integer value at `$A` and constant `X`. Stores the result in `$A`.

### Remarks

`$A = [$A] + [X]`

## Example

```asm
addImm $0, 1
```