# Jgr

## Signature

```asm
jgr L
```

## Description

Moves to label `L`.

### Remarks

- The `Compare` flag in the VM must be `Greater`.
- `L` must be in the current code chunk.

## Example

```asm
jgr 1
```