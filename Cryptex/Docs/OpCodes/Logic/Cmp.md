# Cmp

## Signature

```asm
cmp $A, $B
```

## Description

Compares the values in memory location1 & location2.

### Remarks

- Sets the `Compare` flag in the VM. 
- Next instruction must be a jump instruction. Else, the VM **will** terminate.

## Example

```asm
cmp $A, $B
```