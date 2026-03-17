# Exit

## Signature

```asm
exit X
```

## Description

Stops the execution of the VM with exit code: `X`.

### Remarks

X = 0 -> exited successfully; X != 0 -> error occurred.

## Example

```asm
exit 0
```