# Jle

## Signature

```asm
jle L
```

## Description

Moves to label `L`.

### Remarks

- The `Compare` flag in the VM must be `LessEquals`.
- `L` must be in the current code chunk.

## Example

```asm
jle 1
```