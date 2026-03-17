# Read

## Signature

```asm
read $A
```

## Description

Reads a char from the console and saves it in `$A` as an integer.

### Remarks

- If the input from the user isn't an integer, the `Error` flag will be set.

## Example

```asm
read $1
```