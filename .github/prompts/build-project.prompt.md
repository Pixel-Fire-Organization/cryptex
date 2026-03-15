---
mode: ask
---

Build the Cryptex solution in Release configuration and report any errors.

Run the following commands in order and summarise the output:

```bash
dotnet restore
dotnet build --no-restore --configuration Release
```

Report:
- Whether the restore succeeded and how many packages were restored.
- Whether the build succeeded or failed.
- All warnings and errors, grouped by project.
- Suggested fixes for any build errors.
