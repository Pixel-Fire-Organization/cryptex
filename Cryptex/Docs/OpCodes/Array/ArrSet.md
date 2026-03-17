# ArrSet

> [!IMPORTANT]
> This API is still in the planning phase and may change in the future.

## Signature

```asm
arrset $A, $B, X
```

## Description

Sets the element at position `X` of the array at `$B` to the value at `$A`.

## Example

```asm
arrset $0, $1, 5
```