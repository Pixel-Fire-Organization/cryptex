# Xor

## Signature

```asm
xor $A, $B
```

## Description

Performs the `xor` operation with the value `$A` and `$B`. Stores the result in `$A`.

## Remarks

`$A = [$A] ^ [$B]`

## Example

```asm
xor $0, $1
```