# StrNum

> [!IMPORTANT]
> This API is still in the planning phase and may change in the future. 

## Signature

```asm
strnum $A, $B
```

## Description

Converts the string in location `$B` to a number and places it in location `$A`.

### Remarks

Valid operation:
- `$B` - must be a number in a string.

If this operation fails, the `Error` flag in the VM is set.

## Example

```asm
strnum $0, $1
```