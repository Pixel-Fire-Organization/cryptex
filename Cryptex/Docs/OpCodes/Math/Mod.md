# Mod

## Signature

```asm
mod $A, $B
```

## Description

Performs the modulo operation on `$A` with `$B`. Stores the result in `$A`.

### Remarks

Both must be integer.

`$A = [$A] % [$B]`

## Example

```asm
mod $0, $1
```