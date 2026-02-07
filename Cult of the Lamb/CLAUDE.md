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

## Compacting
Before any compact operation (manual or auto), ALWAYS create or update mod-specific summary files in `work_summaries/` using the format `summary-<modname>-YYYY-MM-DD.md` (e.g., `work_summaries/summary-cultofqol-2026-02-07.md`, `work_summaries/summary-mysticassistantredux-2026-02-07.md`). Include:
- Current task and goal
- Recent changes made
- Open issues/bugs
- Key decisions
- Files modified

If work spans multiple mods, create a separate summary for each mod. Do this BEFORE the compact happens, not after.

## Solution Architecture

Nine mods sharing common utilities via linked source files:

- **CultOfQoL** - Large quality-of-life collection (~50 features) with extensive Harmony patches
- **MysticAssistantRedux** - Deterministic Mystic Shop: buy specific rewards instead of spinning the wheel. Handles necklaces, doctrine stones, talisman pieces, follower skins, decorations, tarot cards, relics, clothing, and fleeces. Gated Apple Arcade and DLC content.
- **Rebirth** - Follower rebirth mechanic with token-based subsequent rebirths; depends on COTL_API
- **Namify** - Custom naming/labeling for game entities
- **QuickMenus** - Hotkey-based quick access to game menus (F1-F4: follower forms, build, tailor, upgrades). Minimal mod, not yet published to Thunderstore.
- **SkipOfTheLamb** - Intro-skip plugin (detects CultOfQoL to avoid conflicts)
- **SkipOfTheLambLite** - Lightweight version of intro-skip
- **TraitControl** - Trait replacement and weighted trait selection system
- **GlyphOverride** - Glyph/control override functionality

### MysticAssistantRedux Architecture

Shop items are defined in `InventoryInfo.GetShopItemTypeList()` and purchase handling is in `MysticShopPatches.GivePlayerBoughtItem()` via a switch statement. `InventoryManager` tracks quantities and overbuy state per shop session. DLC content is gated behind `DataManager.Instance.MAJOR_DLC`; Apple Arcade content behind `Plugin.EnableAppleArcadeContent.Value` (config, default off). The `ExclusiveContent` class handles runtime injection of Apple Arcade assets (follower skins, clothing, fleeces, decorations).

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
- `Misc/` - Uncategorized patches
- `Player/` - Necklaces, fleece manager, player mechanics
- `Structures/` - Healing bay, mating tent, prisons, totems
- `Systems/` - Auto-collect, game speed, saves, weather
- `UI/` - Menu cleanup, notifications, post-processing, intros

### Plugin Configuration Pattern

CultOfQoL uses a **partial class split**: `Configs.cs` declares all `ConfigEntry<T>` properties, and `Plugin.cs` binds them in `Awake()` with section, description, and order. Config entries are organized into numbered sections (01-20) visible in BepInEx Configuration Manager. Performance-sensitive patches use `ConfigCache` (cached values with dirty-key invalidation via `SettingChanged` handlers) instead of reading `ConfigEntry.Value` directly.

### GlobalUsings

Each mod has a `GlobalUsings.cs` with project-wide `global using` statements. CultOfQoL imports ~38 namespaces including Lamb.UI, Map, MMTools, DOTween, and HarmonyLib. Rebirth imports COTL_API namespaces (CustomFollowerCommand, CustomInventory, CustomMission, etc.).

## Key Technical Details

- **Target:** .NET Framework 4.8.1, C# preview language version
- **NuGet Source:** `https://nuget.bepinex.dev/v3/index.json` (configured in NuGet.Config)
- **SDK:** .NET 8.0.0 (global.json, roll-forward latest major)
- **All references are CopyLocal=False** - only the mod DLL is output
- **Game/Unity assemblies** are in `libs/` and referenced directly in `.csproj` files (not via NuGet). All assemblies are **publicized** (private members exposed as public), so there is no need for reflection to access game internals. **Never use reflection** unless strictly required (e.g., inside a transpiler to get MethodInfo for IL patching).
- **Original game source** is in `libs/game_code/Assembly-CSharp/`. Always reference this when investigating mod bugs, issues, or optimization requests to understand the vanilla game behavior being patched. **Never assume what a game method does based on its name alone** — always read the actual method body in the game code before making claims about its behavior or purpose.
- **CRITICAL: Always verify against game code.** When answering ANY question about vanilla game behavior (what traits are in a list, how a system works, what values are used, etc.), DO NOT GUESS — read the actual game code first. This applies to questions, not just implementation work.
- **Harmony patching preference:** Prefer transpilers over prefix patches that skip the original method (`return false`). Transpilers preserve the original method's logic and only modify the specific behavior that needs changing, reducing the risk of missing side effects or future game updates adding new logic to the method.
- **Braces:** Always use braces for control flow statements (`if`, `for`, `foreach`, `while`, etc.), except when the body is a single `return`, `continue`, or `break` statement.

## Workflow

- **Always explain before implementing:** After identifying a problem and devising a solution, explain what the problem is and how the proposed fix addresses it. Let the user decide whether to proceed with the implementation.
- **Check summary files:** When investigating bugs or looking for previous fixes, check `work_summaries/summary-<modname>-*.md` files for the relevant mod. These contain detailed notes on recent development work, including problems, solutions, root cause analysis, and files modified.

## Git

- **Never add Co-Authored-By lines** to commit messages.

## Changelogs

- **Never create a new version entry** in changelogs unless explicitly instructed by the user. Add changes to the existing unreleased version entry.
- If unsure whether to create a new version or add to an existing one, ask the user before continuing.
- **Use player-facing language**: Write changelog entries from the user's perspective, describing what they experience (e.g., "Fixed player getting stuck when petting animals") rather than technical implementation details (e.g., "Added null check for state field in PetIE coroutine").
- **Development fixes vs user-facing features**: Don't mention bugs that were fixed during development of a new feature before it's released. If a feature is being added for the first time in a version, only describe the final working feature - users never experienced the broken version. Example: Don't say "Added feature X" and "Fixed bug in feature X" in the same release; just say "Added feature X" with the correct specifications.
- **Technical details belong elsewhere**: Implementation notes, bug root causes, and fix details should go in commit messages and `summary-*.md` files, not user-facing changelogs. Changelogs describe the final experience, not the development journey.
- **Always include dates**: Every version entry must have a date in DD/MM/YYYY format (e.g., `### 2.3.5 - 27/01/2026`), even if multiple versions are released on the same day.

## Version Bumps

When bumping versions for any mod, update all three locations:
1. `<ModName>/<ModName>.csproj` - `<Version>` element
2. `<ModName>/Plugin.cs` - `PluginVer` constant
3. `Thunderstore/<thunderstore-dir>/manifest.json` - `version_number` field

Thunderstore directory names per mod: `cult` (CultOfQoL), `mysticassistantredux`, `rebirth`, `namify`, `quickmenus`, `skipofthelamb`, `skipofthelamblite`, `traitcontrol`, `glyphoverride`.

## Distribution

Thunderstore packages in `Thunderstore/<modname>/` containing manifest.json, icon.png, README.md, CHANGELOG.md, and the compiled plugin DLL. The CultOfQoL mod depends on `BepInEx-BepInExPack_CultOfTheLamb-5.4.2101`.

### Documentation Files

When updating "readme" or documentation, always update BOTH files:
- `Thunderstore/<modname>/README.md` - Markdown format for Thunderstore
- `Thunderstore/<modname>/nexusmods_description.txt` - BBCode format for NexusMods

Keep content in sync between both files, adjusting only for format differences (Markdown vs BBCode).

