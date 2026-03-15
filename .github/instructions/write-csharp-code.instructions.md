---
applyTo: "**/*.cs"
---

# Skill: Write C# Code for Cryptex

Follow these conventions precisely when writing or modifying C# code in this repository.

## Language and SDK

- Target framework: `net8.0` (core library and tests), `net8.0-windows` (WPF inspector).
- Build SDK: .NET 10.0 (see CI configuration).
- Language version: C# 12 (latest features enabled via SDK default).
- Nullable reference types **must** be enabled (`#nullable enable` or project-level `<Nullable>enable</Nullable>`).
- Implicit usings are enabled; do not add redundant `using` directives that are already globally imported.

## Naming Conventions

| Symbol | Convention | Example |
|--------|-----------|---------|
| Private instance field | `m_camelCase` | `m_script`, `m_memory`, `m_VMExited` |
| Private static field | `s_camelCase` | `s_instance` |
| Internal/public constant | `UPPER_SNAKE_CASE` | `MAX_FUNCTION_ARGS` |
| Public property | `PascalCase` | `ExitCode`, `DumpMemory()` |
| Public method | `PascalCase` | `BeginExecution()`, `GetValueInMemory()` |
| Private method | `PascalCase` | `ParseArguments()` |
| Type parameter | `T` or `T<Descriptor>` | `TValue` |
| Namespace | `Cryptex.<Layer>.<Sublayer>` | `Cryptex.VM.Execution.Instructions.MathInstructions` |

## Class Design

- Prefer `sealed` classes. Only unseal when inheritance is explicitly required.
- Keep classes small and single-responsibility — one instruction per class.
- Use `internal` visibility for implementation classes; expose only what is part of the public API.
- Do not use static classes for stateful logic; use instances.

## Error Handling

- Throw domain-specific exceptions from `Cryptex.Exceptions/` rather than generic `Exception` or `InvalidOperationException`.
- Catch `VMRuntimeException` at the `Executor` level; let it propagate from instruction classes.
- Use `PrintingDelegates.WriteError` to report runtime errors to the configured output delegate — do not call `Console.WriteLine` directly in library code.

## Code Style

- Align assignments and declarations when there are multiple consecutive lines (matches the existing `.editorconfig` style).
- Place `using` directives outside the namespace (file-scoped namespaces are preferred: `namespace Cryptex.VM.Execution;`).
- One blank line between methods; no trailing blank lines at the end of a file.
- Expression-bodied members are acceptable for simple single-expression properties and methods (e.g., `public string DumpMemory() => m_memory.DumpMemory();`).
- Braces on their own lines (Allman style).
- Unsafe code is permitted only in `Cryptex/Cryptex.csproj` where `<AllowUnsafeBlocks>true</AllowUnsafeBlocks>` is set.

## Numeric Types

- Use `BigInteger` (from `System.Numerics`) for integer memory values in the VM — this is the established convention.
- Use `decimal` for floating-point memory values where precision matters.

## Do Not

- Do not add new NuGet packages without explicit approval.
- Do not use `dynamic` or reflection unless absolutely necessary.
- Do not suppress nullable warnings with `!` unless the null-safety is provably guaranteed by surrounding logic.
- Do not use `Console.Write*` in the `Cryptex` library project — use `PrintingDelegates` instead.
