# Crash

## Signature

```asm
crash X
```

## Description

Tells the VM to error with error code: X. 

### Remarks
X must be a valid [error code](../../ErrorCodes.md).

## Example

```asm
crash 35
```