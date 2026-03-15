# StrAppend

> [!IMPORTANT]
> This API is still in the planning phase and may change in the future. 

## Signature

```asm
strappend $A, $B
```

## Description

Appends the string in `$B` to the end of the string in `$A`.

### Remarks

Valid operation:
- `$A` - must be a string.
- `$B` - must be a string.

`$A = [$A] + [$B]`

## Example

```asm
strappend $0, $1
```