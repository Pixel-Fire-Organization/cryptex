using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

// Use a shorter job in CI environments to avoid multi-minute runs.
// BenchmarkDotNet applies MaxRelativeError(0.10) so that each benchmark
// retries until its measurement standard error is ≤ 10 % of the mean —
// the same 5–10 % tolerance the project requires.
var ciMode  = Environment.GetEnvironmentVariable("CI") == "true";
var baseJob = ciMode ? Job.Dry : Job.Default;

var config = ManualConfig
    .Create(DefaultConfig.Instance)
    .AddJob(baseJob.WithMaxRelativeError(0.10));

BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, config);
