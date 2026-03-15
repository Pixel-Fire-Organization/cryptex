---
mode: ask
---

Run the Cryptex test suite in Release configuration and report the results.

Run the following command and summarise the output:

```bash
dotnet test --configuration Release --verbosity normal
```

Report:
- Total number of tests run.
- Number of tests that passed, failed, and were skipped.
- Full details of any failing tests, including the test name, the assertion that failed, and the stack trace.
- Suggested fixes for any failing tests.
