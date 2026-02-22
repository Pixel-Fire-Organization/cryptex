# Add

## Signature

```asm
add $A, $B
```

## Description

Adds two integer values at $A and $B. Stores the result in $A.

### Remarks
`$A = [$A] + [$B]`

## Example

```asm
add $0, $1
```