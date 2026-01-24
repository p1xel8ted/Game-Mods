# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build Commands

```bash
# Build all mods
dotnet build "CultOfTheLambMods.sln"

# Build a specific mod
dotnet build "CultOfQoL/CultOfQoL.csproj"
dotnet build "Rebirth/Rebirth.csproj"
dotnet build "Namify/Namify.csproj"

# Release build
dotnet build "CultOfTheLambMods.sln" -c Release

# Release + Thunderstore packaging (updates manifest version, creates zip)
dotnet build "CultOfTheLambMods.sln" -c Release-Thunderstore
```

Output goes directly to `G:\SteamLibrary\steamapps\common\Cult of the Lamb\BepInEx\plugins\<ProjectName>` for all configurations. The `Release-Thunderstore` configuration additionally copies DLLs/assets to `Thunderstore/<modname>/plugins/`, updates `manifest.json` version, and creates a versioned zip archive. There are no tests or linting configured.

## Solution Architecture

Three mods sharing common utilities via linked source files:

- **CultOfQoL** (v2.3.1) - Large quality-of-life collection (~50 features) with extensive Harmony patches
- **Rebirth** (v0.2.1) - Follower rebirth mechanic, depends on COTL_API (0.2.8)
- **Namify** (v0.2.1) - Custom naming/labeling for game entities

### Shared Code (Linked Source Files)

Mods link source files from `Shared/` rather than referencing a compiled assembly:
- **Extensions.cs** - Collection/string/GameObject extensions, meal type checks
- **Helpers.cs** - Storefront detection, follower task routing, diagnostic logging
- **ConfigurationManagerAttributes.cs** - BepInEx Configuration Manager UI attributes

Each `.csproj` includes these via `<Compile Include="..\Shared\File.cs" Link="File.cs"/>`.

### Patch Organization (CultOfQoL)

Patches are in `CultOfQoL/Patches/` organized by game system:
- `Followers/` - Level-up, traits, follower behavior
- `Gameplay/` - Fishing, rituals, tarot, pickups, interactions
- `Player/` - Necklaces, fleece manager, player mechanics
- `Structures/` - Healing bay, prisons, totems
- `Systems/` - Auto-collect, game speed, saves, weather

### Plugin Configuration Pattern

CultOfQoL's `Plugin.cs` defines config entries in numbered sections (01-20) using BepInEx's ConfigFile system. Patches read these static config properties to conditionally apply modifications. All config UI is handled via the BepInEx Configuration Manager dependency.

### GlobalUsings

Each mod has a `GlobalUsings.cs` with project-wide `global using` statements. CultOfQoL imports ~38 namespaces including Lamb.UI, Map, MMTools, DOTween, and HarmonyLib. Rebirth imports COTL_API namespaces (CustomFollowerCommand, CustomInventory, CustomMission, etc.).

## Key Technical Details

- **Target:** .NET Framework 4.8.1, C# preview language version
- **NuGet Source:** `https://nuget.bepinex.dev/v3/index.json` (configured in NuGet.Config)
- **SDK:** .NET 8.0.0 (global.json, roll-forward latest major)
- **All references are CopyLocal=False** - only the mod DLL is output
- **Game/Unity assemblies** are in `libs/` and referenced directly in `.csproj` files (not via NuGet). All assemblies are **publicized** (private members exposed as public), so there is no need for reflection to access game internals. **Never use reflection** unless strictly required (e.g., inside a transpiler to get MethodInfo for IL patching).
- **Original game source** is in `libs/game_code/Assembly-CSharp/`. Always reference this when investigating mod bugs, issues, or optimization requests to understand the vanilla game behavior being patched. **Never assume what a game method does based on its name alone** — always read the actual method body in the game code before making claims about its behavior or purpose.
- **Harmony patching preference:** Prefer transpilers over prefix patches that skip the original method (`return false`). Transpilers preserve the original method's logic and only modify the specific behavior that needs changing, reducing the risk of missing side effects or future game updates adding new logic to the method.

## Workflow

- **Always explain before implementing:** After identifying a problem and devising a solution, explain what the problem is and how the proposed fix addresses it. Let the user decide whether to proceed with the implementation.

## Git

- **Never add Co-Authored-By lines** to commit messages.

## Distribution

Thunderstore packages in `Thunderstore/<modname>/` containing manifest.json, icon.png, README.md, CHANGELOG.md, and the compiled plugin DLL. The CultOfQoL mod depends on `BepInEx-BepInExPack_CultOfTheLamb-5.4.2101` and `p1xel8ted-BepInEx_Configuration_Manager-18.3.0`.
