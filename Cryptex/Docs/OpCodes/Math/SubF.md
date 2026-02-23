# SubF

## Signature

```asm
subF $A, $B
```

## Description

Subtracts two floating values at `$A` and `$B`. Stores the result in `$A`.

### Remarks
`$A = [$A] - [$B]`

## Example

```asm
subF $0, $1
```