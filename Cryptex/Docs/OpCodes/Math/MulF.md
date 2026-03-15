# MulF

## Signature

```asm
mulF $A, $B
```

## Description

Multiplies two floating values at `$A` and `$B`. Stores the result in `$A`.

### Remarks
`$A = [$A] * [$B]`

## Example

```asm
mulF $0, $1
```