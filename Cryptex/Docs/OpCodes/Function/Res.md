# Res

## Signature

```asm
res X
```

## Description

Sets a chunk's return value.

### Remarks

- When `ret` is called, this value is passed to the same memory location as the one from the `Call` instruction.
- Each `ret` and `call` instructions must match, else the VM **will** terminate.

## Example

```asm
res 1
```