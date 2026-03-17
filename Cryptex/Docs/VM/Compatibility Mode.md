# Compatibility Mode

## Overview

Cryptex guarantees **full backwards compatibility**: any script authored for an older VM version will always execute correctly on a newer VM. When an instruction's behaviour changes between versions, the VM selects the implementation that matches the script's target version.

In the opposite direction — a script authored for a **newer** VM version running on an **older** VM — the VM enters **Compatibility Mode**. Execution continues using the current VM's instruction behaviour, but a warning is emitted because the results may not be identical to what the newer VM would produce.

---

## When Compatibility Mode Activates

Compatibility Mode is active when:

```
script.VMVersion > Executor.VM_VERSION
```

This can happen if:
- A script was compiled for a future VM version.
- A library or tool distributes scripts targeting a newer Cryptex release than the one installed.

A script with `VMVersion <= Executor.VM_VERSION` is always considered fully compatible and executes without any warnings.

---

## How Compatibility Mode Behaves

1. **Validation** — `ScriptValidator.Validate` returns a result with `IsValid = true` and a `HasWarnings = true` flag. The warning carries the code `ScriptValidationWarningCode.CompatibilityModeActive` and a human-readable message describing the version gap.

2. **Runtime warning** — The first call to `ExecuteScript()` or `ExecuteChunk()` on an affected `Executor` instance emits a single warning via `PrintingDelegates.WriteWarning`. Subsequent calls on the same instance are silent.

3. **Instruction dispatch** — The VM dispatches every instruction using its own (older) implementation. If an instruction changed behaviour between the script's target version and the current VM version, the current behaviour is used. See [Versioned Instructions](#versioned-instructions) for how this is handled going forward.

4. **Execution continues** — Compatibility Mode does not abort execution. `ExecuteScript()` / `ExecuteChunk()` return their normal `bool` success value.

### Detecting Compatibility Mode Programmatically

```csharp
// After validation:
var result = ScriptValidator.Validate(script);
if (result.HasWarnings)
{
    foreach (var warning in result.Warnings)
        Console.WriteLine($"[WARN] {warning.Code}: {warning.Message}");
}

// After constructing an executor:
var executor = new Executor(script);
if (executor.IsInCompatibilityMode)
{
    // The script targets a newer VM. Proceed with caution.
}
```

---

## Backwards Compatibility Guarantee

Every released VM version **must** be able to execute all scripts written for any earlier VM version with identical results. This is enforced by version-aware instruction dispatch:

- `OpCodesExtensions.GetByCode(opCode, scriptVersion)` accepts the script's target version.
- `ScriptChunk.Execute` passes `vm.ScriptVersion` to every instruction lookup.
- When a VM version introduces a behaviour change to an existing instruction, the old implementation is preserved as a versioned variant (e.g. `AddInstructionV1`) and returned when `scriptVersion` is within the range where the old behaviour was canonical.

This means a script written for VM version 1 will produce **byte-for-byte identical results** on any later VM version.

---

## Versioned Instructions

When an instruction's behaviour must change in a new VM version, follow this process:

1. Keep the existing implementation class as-is (e.g. `AddInstruction` remains the V1 behaviour).
2. Create a new class for the changed behaviour (e.g. `AddInstructionV2`).
3. In `OpCodesExtensions.GetByCode`, add a version guard:

```csharp
case OpCodes.Add when scriptVersion < 2:
    return new AddInstruction();       // V1 behaviour — used for V1 scripts on V2+ VM
case OpCodes.Add:
    return new AddInstructionV2();     // V2+ behaviour
```

4. Update `OpCodeCatalog.Get` if the opcode's argument signature changed.
5. Document the behavioural change in the opcode's Markdown reference page under a **Version History** section.

---

## The Script Upgrade System

The upgrade system lives **outside** the `Cryptex` library. Any external tool can upgrade a script to a newer VM version using the public API:

```csharp
// 1. Load the script (validated — may report CompatibilityModeActive warning)
var script = ScriptLoader.LoadAndValidate(path, out var result);

// 2. Inspect warnings to know what changed
foreach (var warning in result.Warnings)
    Console.WriteLine(warning.Message);

// 3. Rebuild the script targeting the current VM version
var upgraded = ScriptComposer.FromExisting(script)
    .WithVmVersion(Executor.VM_VERSION)
    // Apply any instruction transformations required by the version gap here.
    .Build();

// 4. Save the upgraded script
ScriptLoader.Save(upgraded, outputPath);
```

Instruction-level transformations (e.g. rewriting opcodes that changed signature) must be implemented by the upgrader for each version gap it bridges. Cryptex exposes all necessary types (`Script`, `ScriptChunk`, `ScriptInstruction`, `ScriptInstructionArgument`, `ScriptComposer`) to make this straightforward.

---

## Version History of This Document

| VM Version | Change |
|:----------:|--------|
| 1          | Initial compatibility mode framework established. |

