# CLAUDE.md

Guidance for Claude Code when working in this repo.

## What this is

**Store Assistant Professional** — a fresh rewrite of the older `StoreAssistantPro` project (sibling folder `../StoreAssistantPro/`, kept read-only for reference). Same domain (Windows POS for Indian retail clothing), same stack, but starting from a minimal base and growing only what's actually needed.

## Current state

Day 1. A single project. A welcome page. Nothing else.

```
src/StoreAssistantProfessional/
├── Program.cs                           # Photino entry
├── StoreAssistantProfessional.csproj
├── Components/
│   ├── _Imports.razor
│   ├── Routes.razor
│   ├── Layout/MainLayout.razor
│   └── Pages/FirstRun.razor             # @page "/" — welcome
└── wwwroot/
    ├── index.html
    └── css/app.css
```

## Stack

- `net10.0-windows`
- Photino.Blazor 4.0.13
- MudBlazor 9.3.0
- SQLite + EF Core — **not yet added**; add when the first page needs persistence.

## Commands

```
dotnet restore src/StoreAssistantProfessional/StoreAssistantProfessional.csproj
dotnet build   src/StoreAssistantProfessional/StoreAssistantProfessional.csproj -c Release
dotnet run     --project src/StoreAssistantProfessional -c Release
```

## Rules of engagement

- **No premature structure.** One project until pain demands a split. No `Core/Services/UI/Desktop` split until it earns its keep.
- **No premature abstractions.** `TabScopedComponent`, `ObservableState`, `MigrationHelper`, chained-hash audit ledger, etc. from the old repo stay in the old repo. Port concepts only when the feature being ported actually needs them.
- **No copy-paste.** Reference the old code for ideas, but rewrite — don't drag tech debt forward.
- **100% stock MudBlazor.** No custom CSS beyond `app.css` basics. No custom theme. No utility classes.
- **Locale = `en-IN`** (set in `Program.cs`). Currency symbol `₹`.

## Reference

Old codebase: `../StoreAssistantPro/CLAUDE.md` — has the full schema (v69), migrator pattern, audit chain, security model, multi-station setup. Read it when you need to understand *what* the old app did. Don't use it as a blueprint.
