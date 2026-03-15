# And

## Signature

```asm
and $A, $B
```

## Description

Performs the `And` operation with the value `$A` and `$B`. Stores the result in `$A`.

## Remarks

`$A = [$A] & [$B]`

## Example

```asm
and $0, $1
```