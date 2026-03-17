# Jls

## Signature

```asm
jls L
```

## Description

Moves to label `L`.

### Remarks

- The `Compare` flag in the VM must be `Less`.
- `L` must be in the current code chunk.

## Example

```asm
jls 1
```