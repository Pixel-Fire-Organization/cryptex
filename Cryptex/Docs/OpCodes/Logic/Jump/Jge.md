# Jge

## Signature

```asm
jge L
```

## Description

Moves to label `L`.

### Remarks

- The `Compare` flag in the VM must be `GreaterEquals`.
- `L` must be in the current code chunk.

## Example

```asm
jge 1
```