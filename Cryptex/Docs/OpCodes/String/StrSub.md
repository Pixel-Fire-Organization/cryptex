# StrSub

> [!IMPORTANT]
> This API is still in the planning phase and may change in the future.

## Signature

```asm
strsub $A, $B, X, Y
```

## Description

Runs function `substring` on the string in location `$B` and places the result in location `$A`, where `X` is start and `Y` is end.

## Example

```asm
strsub $0, $1, 6, 7
```