---
mode: ask
---

Perform a thorough code review of the changes in the current working branch or the specified files.

## Review Checklist

### Correctness
- [ ] Does the logic correctly implement the intended behaviour?
- [ ] Are all edge cases handled (empty arguments, wrong argument types, too few/many arguments)?
- [ ] Do all new or modified instructions follow the single-responsibility pattern?

### C# Code Quality
- [ ] Are private fields prefixed with `m_` (e.g., `m_script`, `m_memory`)?
- [ ] Are constants in `UPPER_SNAKE_CASE`?
- [ ] Are new classes `sealed` unless inheritance is explicitly required?
- [ ] Is `internal` visibility used for implementation details?
- [ ] Are nullable reference types handled correctly (no unguarded `!` suppressions)?
- [ ] Does library code use `PrintingDelegates.WriteError` instead of `Console.Write*`?
- [ ] Are domain-specific exceptions from `Cryptex.Exceptions/` used instead of generic ones?

### Tests
- [ ] Does every new instruction or changed instruction have a corresponding test class in `Cryptex.Test/InstructionsTests/`?
- [ ] Are happy-path, wrong-type, too-few-args, too-many-args, and no-args scenarios covered?
- [ ] Do tests assert `NotNull` before asserting equality on `GetValueInMemory()` results?

### CI/CD
- [ ] Do new or modified workflows use the minimum required permissions?
- [ ] Is `actions/checkout@v4` (or latest pinned version) used?
- [ ] Is NuGet caching configured with `actions/cache@v4`?
- [ ] Do workflows that run code inspection reuse `.github/actions/resharper-inspect`?

### General
- [ ] Is there any dead code, commented-out code, or debug statements left in?
- [ ] Are there any new NuGet package additions that need justification?
- [ ] Does the change require updates to `CryptexScriptInspector/OpCodeDescriptions.cs` or `OpCodeArguments.cs`?

Provide specific, actionable feedback for each issue found, referencing the file name and line number where applicable.
