# Ret

## Signature

```asm
ret
```

## Description

Stops the execution of the current chunk, and returns to the previous.

### Remarks

- If this is the last chunk, then the execution stops.
- Clears all memory locations used in this chunk.

## Example

```asm
ret
```