# Cryptex

A .NET virtual machine and scripting engine library.

[![Build](https://github.com/Pixel-Fire-Organization/cryptex/actions/workflows/dotnet-build.yml/badge.svg)](https://github.com/Pixel-Fire-Organization/cryptex/actions/workflows/dotnet-build.yml)
[![Stress Tests & Benchmarks](https://github.com/Pixel-Fire-Organization/cryptex/actions/workflows/stress-and-benchmark.yml/badge.svg)](https://github.com/Pixel-Fire-Organization/cryptex/actions/workflows/stress-and-benchmark.yml)

## Overview

Cryptex is a .NET virtual machine library that provides a full instruction set for integer and floating-point arithmetic, bitwise operations,
conditional jumps, and memory management. Scripts are serialized with [MessagePack](https://github.com/MessagePack-CSharp/MessagePack-CSharp) and
executed by a lightweight stack-less VM.

## Projects

| Project              | Description                                               |
|----------------------|-----------------------------------------------------------|
| `Cryptex`            | Core VM library — instruction set, executor, memory model |
| `Cryptex.Test`       | xUnit unit and stress tests                               |
| `Cryptex.Benchmarks` | BenchmarkDotNet benchmarks for every instruction          |
| `CryptexTester`      | Console app for manual testing                            |

## CI / Workflows

| Workflow                                                                | Trigger | What it does                                                             |
|-------------------------------------------------------------------------|---------|--------------------------------------------------------------------------|
| [.NET Core Build](.github/workflows/dotnet-build.yml)                   | Push    | Restore → Build → Unit tests                                             |
| [Stress Tests & Benchmarks](.github/workflows/stress-and-benchmark.yml) | Push    | Stress tests + BenchmarkDotNet benchmarks; results uploaded as artifacts |
| [Publish](.github/workflows/publish-cryptex-lib.yml)                    | Manual  | AOT publish for win-x64, linux-x64, osx-arm64, osx-x64                   |

## Stress Tests

Stress tests live in `Cryptex.Test/StressTests/` and are tagged `[Trait("Category", "Stress")]`.  
They verify that every instruction produces **consistent results** under extreme conditions:

- Very large integer values (up to `BigInteger.Pow(2, 127)`)
- 10_000-iteration VM loops testing convergence and loop termination
- Edge cases: zero masks, double negation, all-bits-set patterns

Run locally:

```bash
dotnet test --configuration Release --filter "Category=Stress"
```

## Benchmarks

Benchmarks are in `Cryptex.Benchmarks/` and use [BenchmarkDotNet](https://benchmarkdotnet.org/).  
Each instruction has a dedicated `[Benchmark]` method. Results are exported as JSON and Markdown, uploaded as GitHub Actions artifacts, and written to the workflow job summary.

The benchmark runner enforces a **10 % maximum relative error** threshold — if measurements for any instruction are too noisy the CI step fails.

Run locally (full precision):

```bash
dotnet run --project Cryptex.Benchmarks --configuration Release -- --filter *
```

Run quickly (dry mode):

```bash
dotnet run --project Cryptex.Benchmarks --configuration Release -- --filter * --job dry
```

## Build

```bash
dotnet restore
dotnet build --configuration Release
dotnet test --configuration Release
```
