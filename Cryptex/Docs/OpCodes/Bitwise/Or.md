# Or

## Signature

```asm
or $A, $B
```

## Description

Performs the `Or` operation with the value `$A` and `$B`. Stores the result in `$A`.

## Remarks

`$A = [$A] | [$B]`

## Example

```asm
or $0, $1
```