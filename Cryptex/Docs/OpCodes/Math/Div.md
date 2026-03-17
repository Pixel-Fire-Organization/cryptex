# Div

## Signature

```asm
div $A, $B
```

## Description

Divides two integer values at `$A` and `$B`. Stores the result in `$A`.

### Remarks

`$A = [$A] / [$B]`

## Example

```asm
div $0, $1
```