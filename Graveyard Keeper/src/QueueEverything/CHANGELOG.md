# Changelog

## 2.1.13 | 16 April 2026

- Fixed Chinese translations not loading
- Added extra background logging to help track down a reported issue with placing new graves at the graveyard build desk. No gameplay changes — if you've hit the issue, the next time it happens your `BepInEx/LogOutput.log` will contain enough detail (lines tagged `[QE-Diag]`) for the mod author to identify and fix the cause in the next update.

## 2.1.12 | 15 April 2026

- Setting changes now apply immediately — no more restarting the game to turn features on or off.
- Auto-crafting is now opt-in per workbench. Pick which benches you want running on their own under the new Auto-Craft section: Alchemy, Cooking, Study, Metalwork, Morgue, Carpentry, Sermons, Printing, Winemaking, Pottery, or Misc. Everything starts off — enable the ones you actually want.
- Sermons at the church pulpit no longer run on their own unless you opt in. If you liked the old behaviour, switch on Auto-Craft → Sermons.
- Renamed "Make Everything Auto" to "Enable Auto-Craft". It's the top-level switch — turn it off to pause the whole feature without losing your per-workbench picks.
- Retired the old "Make Hand Tasks Auto" option. The new per-workbench toggles cover what it used to do, and more.
- Added a new Balance section with a "Half Research Point Outputs" toggle. Controls whether auto-crafted recipes yield half the usual blood, fat, skin, heart and intestine points. It's on by default (matching how the mod has always behaved) — turn it off if you want full yields on auto-crafted recipes. Regular hand crafts are unaffected either way.
- Settings are now grouped into Advanced, Auto-Craft, Balance, and Convenience sections so the options are easier to find.
- Stopped auto-converting a pile of workbenches that had been sneaking through: quarry mining at the Stone Steep and Marble Steep, building-repair benches (broken mill, bee-garden table, swamp bridge, vendor tent/stall), decor placements (lanterns, garden of stones), tier upgrades (furnace, distillation cube, lantern tiers), and soul-crafting & necromancy workbenches. These stay as proper hand crafts like the game intends.

## 2.1.11 | 12 April 2026

- The Advanced section now appears at the top of the settings list instead of the bottom, and its Debug Logging option is always visible (was hidden by default)
- Enabling Debug logging now shows a one-time in-game dialog warning you it's on, so you don't forget it's enabled
- Fixed non-English translations not loading — the mod was showing English regardless of your game language
- Language changes in the game options are now picked up immediately without needing to restart

## 2.1.10 | 11 April 2026

- Fixed hotkeys and input features not working for some users
- Improved compatibility across different BepInEx versions

## 2.1.9

- Fixed auto-craft interfering with zombie workers and dropping items on the ground
- Fixed wine barrels and similar recipes breaking when Auto Max Multi-Quality Crafts was enabled
- Mod is now standalone — no longer requires GYK Helper

## 1

- Initial release

