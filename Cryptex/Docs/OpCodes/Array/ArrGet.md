# ArrGet

> [!IMPORTANT]
> This API is still in the planning phase and may change in the future.

## Signature

```asm
arrget $A, $B, X 
```

## Description

Gets a value at index `X` from an array stored at `$B`. Stores the value at `$A`.

## Example

```asm
arrget $0, $1, 3
```