# Work Summary - Multi-Mod Changes - 2026-02-08

## BepInDependency: Hard Dependency on ConfigurationManager 18.4.1 (COMPLETE)

### What Changed
Made ConfigurationManager a hard dependency (minimum version 18.4.1) for all Cult of the Lamb mods except SkipOfTheLambLite.

### Changes by Mod
| Mod | Previous State | Change |
|-----|---------------|--------|
| MysticAssistantRedux | No BepInDependency at all | Added `[BepInDependency("com.bepis.bepinex.configurationmanager", "18.4.1")]` |
| QuickMenus | SoftDependency flag | Changed to `"18.4.1"` version string |
| SkipOfTheLamb | SoftDependency flag | Changed to `"18.4.1"` version string |
| CultOfQoL | Already correct | No change |
| Rebirth | Already correct | No change |
| Namify | Already correct | No change |
| TraitControl | Already correct | No change |
| GlyphOverride | Already correct | No change |
| SkipOfTheLambLite | Excluded | No change |

### Files Modified
- `MysticAssistantRedux/Plugin.cs`
- `QuickMenus/Plugin.cs`
- `SkipOfTheLamb/Plugin.cs`
