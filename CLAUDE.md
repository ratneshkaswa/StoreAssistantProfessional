# CLAUDE.md

Guidance for Claude Code when working in this repo.

## What this is

**Store Assistant Professional** — a fresh rewrite of the older `StoreAssistantPro` project (sibling folder `../StoreAssistantPro/`, kept read-only for reference). Same domain (Windows POS for Indian retail clothing), same stack, but starting from a minimal base and growing only what's actually needed.

## Current state

Single project. First-run setup + placeholder home. No persistence yet.

```
src/StoreAssistantProfessional/
├── StoreAssistantProfessional.csproj
├── App.xaml / App.xaml.cs               # WPF bootstrap + DI
├── MainWindow.xaml / MainWindow.xaml.cs # Hosts BlazorWebView; disables WebView2 DevTools
├── Components/
│   ├── _Imports.razor
│   ├── Routes.razor                     # Router + Mud providers
│   ├── Layout/MainLayout.razor
│   └── Pages/
│       ├── Home.razor                   # @page "/" + admin-unlock dialog
│       └── FirstRun.razor               # @page "/setup"
├── Services/
│   ├── SetupService.cs                  # PBKDF2 PIN hashing (600k iter) + atomic setup.json
│   ├── SessionService.cs                # Role = User | Admin (lock-guarded)
│   └── PinRules.cs                      # Weak-PIN detection
└── wwwroot/
    ├── index.html
    └── css/app.css
```

## Stack

- `net10.0-windows10.0.19041.0` (Windows 10 2004+ required for WebView2 composition path)
- WPF + `Microsoft.AspNetCore.Components.WebView.Wpf` 10.0.1
- MudBlazor 9.4.0
- WebView2 Runtime (ships with Windows 11; Evergreen Runtime required on Windows 10)
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
- **MudBlazor first.** Stock components only; no custom CSS beyond `app.css` basics; no utility classes. A custom `MudTheme` with brand colors is OK (warm palette for retail feel) — changes to the theme happen in `Theme/BrandTheme.cs`, not inline on components.
- **Locale = `en-IN`** (set in `App.xaml.cs`). Currency symbol `₹`. English-only — no i18n planned.

## Reference

Old codebase: `../StoreAssistantPro/CLAUDE.md` — has the full schema (v69), migrator pattern, audit chain, security model, multi-station setup. Read it when you need to understand *what* the old app did. Don't use it as a blueprint.
