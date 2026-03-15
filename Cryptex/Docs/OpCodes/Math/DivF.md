# DivF

## Signature

```asm
divF $A, $B
```

## Description

Divides two floating values at `$A` and `$B`. Stores the result in `$A`.

### Remarks
`$A = [$A] / [$B]`

## Example

```asm
divF $0, $1
```