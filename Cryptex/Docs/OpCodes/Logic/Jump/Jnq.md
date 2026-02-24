# Jnq

## Signature

```asm
jnq L
```

## Description

Moves to label `L`.

### Remarks

- The `Compare` flag in the VM must be `NotEquals`.
- `L` must be in the current code chunk.

## Example

```asm
jnq 1
```