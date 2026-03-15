# Term

## Signature

```asm
term
```

## Description

Terminates the execution of the VM. 

### Remarks
This opcode must be never used by a programmer. It is used as a failsafe of the VM when the chunk has invalid data.

## Example

```asm
term
```