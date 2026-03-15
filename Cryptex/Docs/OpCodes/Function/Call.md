# Call

## Signature

```asm
call $A, L
```

## Description

Stops the execution of the current chunk, and starts execution of another.

## Remarks

- If the new chunk has a return value, it will be saved in `$A`.
- If the location `L` isn't a label, the VM **will** terminate.

## Example

```asm
call $0, 1
```