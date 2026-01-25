# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build Commands

```bash
# Build this mod only
dotnet build "SkipOfTheLamb/SkipOfTheLamb.csproj"

# Build with Thunderstore packaging
dotnet build "SkipOfTheLamb/SkipOfTheLamb.csproj" -c Release-Thunderstore
```

No tests or linting configured.

## Project Overview

SkipOfTheLamb (v0.1.0) is a minimal BepInEx plugin that skips intro cinematics in Cult of the Lamb. It provides two configurable features:

1. **Skip Intros** - Bypasses developer splash screens on game start (`LoadMainMenu.Start`)
2. **Skip Crown Video** - Skips the crown cutscene (`IntroDeathSceneManager.GiveCrown`)

Both features are disabled by default and controlled via config file or BepInEx Configuration Manager (F1) if installed.

## Architecture

The entire mod is 3 source files:

- **Plugin.cs** - Plugin entry point, config definitions, CultOfQoL conflict detection
- **Patches.cs** - Two Harmony prefix patches (one per feature)
- **GlobalUsings.cs** - Minimal imports (BepInEx, HarmonyLib, MMTools)

### Shared Code

Links `ConfigurationManagerAttributes.cs` from `../Shared/` for Configuration Manager UI support.

### CultOfQoL Conflict Detection

Plugin.cs checks at startup whether CultOfQoL is loaded (via `Chainloader.PluginInfos`) and logs a warning, since CultOfQoL has overlapping skip-intro features in its `SkipIntros.cs` patch.

## Key Game Methods Patched

- **`LoadMainMenu.Start()`** - Original plays splash screen videos then transitions to Main Menu. Patch skips directly to "Main Menu" scene via `MMTransition.Play()`. Uses a static `_hasSkippedDevIntros` flag to run only once (prevents infinite loop since the Main Menu scene triggers `LoadMainMenu.Start` again).

- **`IntroDeathSceneManager.GiveCrown()`** - Original starts a coroutine playing animations and video. Patch calls `VideoComplete()` directly and sets `HadInitialDeathCatConversation = true`.

## Thunderstore Package

Package directory: `../Thunderstore/skipofthelamb/`

Dependencies:
- `BepInEx-BepInExPack_CultOfTheLamb-5.4.2101`
