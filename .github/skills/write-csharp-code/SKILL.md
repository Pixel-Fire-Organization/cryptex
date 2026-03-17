---
name: write-csharp-code
description: Provides C# coding conventions and standards for the Cryptex repository. Use when writing, modifying, or reviewing C# source files to ensure consistency with the established style, naming, and architectural rules.
metadata:
  author: Pixel-Fire-Organization
  version: "1.0"
---

# Write C# Code for Cryptex

Follow these conventions precisely when writing or modifying C# code in this repository.

## Language and SDK

- Target framework: `net10.0` (core library and tests).
- Build SDK: .NET 10.0 (see CI configuration).
- Language version: C# 14 (latest features enabled via SDK default).
- Nullable reference types **must** be enabled (`#nullable enable` or project-level `<Nullable>enable</Nullable>`).
- Implicit usings are enabled; do not add redundant `using` directives that are already globally imported.

## Naming Conventions

| Symbol                   | Convention                   | Example                                              |
|--------------------------|------------------------------|------------------------------------------------------|
| Private instance field   | `m_camelCase`                | `m_script`, `m_memory`, `m_VMExited`                 |
| Private static field     | `s_camelCase`                | `s_instance`                                         |
| Internal/public constant | `UPPER_SNAKE_CASE`           | `MAX_FUNCTION_ARGS`                                  |
| Public property          | `PascalCase`                 | `ExitCode`, `DumpMemory()`                           |
| Public method            | `PascalCase`                 | `BeginExecution()`, `GetValueInMemory()`             |
| Private method           | `PascalCase`                 | `ParseArguments()`                                   |
| Type parameter           | `T` or `T<Descriptor>`       | `TValue`                                             |
| Namespace                | `Cryptex.<Layer>.<Sublayer>` | `Cryptex.VM.Execution.Instructions.MathInstructions` |

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

## Serialization (MessagePack)

- Script model classes (`Script`, `ScriptChunk`, `ScriptInstruction`, `ScriptInstructionArgument`) are serialized with **MessagePack**.
- Annotate serializable classes with `[MessagePackObject(keyAsPropertyName: true)]`.
- Properties that must not be serialized carry `[IgnoreMember]`.
- Do not add new serialization attributes unless a type is part of the binary script format.

## SOLID Design Principles

Every C# class in this repository must follow SOLID:

| Principle                 | Rule                                                                                                                      |
|---------------------------|---------------------------------------------------------------------------------------------------------------------------|
| **Single Responsibility** | Each class has exactly one reason to change. Instruction classes encapsulate exactly one opcode's behaviour.              |
| **Open/Closed**           | Add new behaviour via new classes; never modify an existing instruction class to handle an unrelated opcode.              |
| **Liskov Substitution**   | Every `IInstruction` implementation must be fully substitutable — the executor never down-casts or type-checks instances. |
| **Interface Segregation** | `IInstruction` is intentionally minimal (`OpCode` + `Execute`). Do not add methods that only some instructions need.      |
| **Dependency Inversion**  | Instructions depend on `Executor` (the abstraction); never reach into concrete internal state directly.                   |

## AOT Compatibility

The `Cryptex` library is published with **Native AOT** (`dotnet publish -p:PublishAOT=true`). The following constructs are **strictly forbidden** in the `Cryptex` project:

### Forbidden

- `System.Reflection` APIs — `Type.GetMethod()`, `MethodInfo.Invoke()`, `Activator.CreateInstance()`, `GetType().GetProperties()`, etc.
- `dynamic` keyword
- `System.Reflection.Emit` — no runtime IL generation
- `System.Linq.Expressions` that compile delegates at runtime (`.Compile()`)
- Unbound generic type lookups that cannot be resolved at compile time
- Any API annotated `[RequiresUnreferencedCode]` or `[RequiresDynamicCode]` in the .NET documentation

### Allowed

- Generic type constraints resolved at compile time
- `typeof(T)`, `nameof(...)`, `sizeof(T)`
- `BitConverter` static methods
- `Span<T>`, `ReadOnlySpan<T>`, `stackalloc`
- Abstract classes and virtual dispatch (devirtualised by the AOT compiler)
- `unsafe` code blocks (the project already enables `<AllowUnsafeBlocks>true</AllowUnsafeBlocks>`)

## Instruction Performance

Instructions are on the VM hot path — called for every opcode executed. Keep `Execute()` lean:

- **No allocations** — avoid `new`, LINQ, `string.Format`, or `StringBuilder` inside `Execute()`; pre-compute and cache anything reusable as `static readonly` fields on the instruction class.
- **No boxing** — do not cast value types (e.g., `long`, `double`) to `object`.
- **Parse arguments once** — read and validate `c.Args` at the top of `Execute()`, then operate; do not re-read in multiple branches.
- **Early exit on error** — throw `VMRuntimeException` as soon as invalid state is detected; never continue after detecting an error.

## Build Verification

After any C# change, run the following as the **final verification step**:

```bash
dotnet build --configuration Release -p:TreatWarningsAsErrors=true
```

This surfaces AOT-incompatible API usage, nullable reference type warnings, and trim-unsafe calls. **Do not consider a C# change complete until this command passes with zero warnings.**

## Numeric Types

- Use `BigInteger` (from `System.Numerics`) for integer memory values in the VM — this is the established convention.
- Use `decimal` for floating-point memory values where precision matters.

## Do Not

- Do not add new NuGet packages without explicit approval.
- Do not use `dynamic`, reflection, or any AOT-incompatible API — see the **AOT Compatibility** section above for the full forbidden list.
- Do not suppress nullable warnings with `!` unless the null-safety is provably guaranteed by surrounding logic.
- Do not use `Console.Write*` in the `Cryptex` library project — use `PrintingDelegates` instead.
- Do not skip the `TreatWarningsAsErrors` build verification step before finalising any change.
