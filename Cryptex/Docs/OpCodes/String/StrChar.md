# StrChar

> [!IMPORTANT]
> This API is still in the planning phase and may change in the future. 

## Signature

```asm
strchar $A, $B, X
```

## Description

Gets the character at position `X` of the string located in `$B` and places it in location `$A`.

### Remarks

Valid operation:
- `X` is between 0, string's length - 1.
- `$B` - must be a string.

If this operation fails, the `Error` flag in the VM is set.

## Example

```asm
strchar $0, $1
```