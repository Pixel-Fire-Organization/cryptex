# Exec

## Signature

```asm
exec $A, E
```

## Description

Executes a function from a registered external class.

## Remarks

- If the function returns a value it will be saved in `$A`.
- If the location `E` isn't an external label, the VM **will** terminate.

## Example

```asm
exec $0, 1
```