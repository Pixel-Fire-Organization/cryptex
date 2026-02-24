# Jeq

## Signature

```asm
jeq L
```

## Description

Moves to label `L`.

### Remarks

- The `Compare` flag in the VM must be `Equals`.
- `L` must be in the current code chunk.

## Example

```asm
jmp 1
```