# Changelog

## 0.1.8 | 18 April 2026

- Incense burners now burn forever and auto-light on placement, just like candelabras
- Added a separate keybind for extinguishing the nearest lit incense and recovering the unused incense. Unbound by default — set a key or controller button in the Controls section if you want to use it. The candle keybind (C / D-Pad Up) only touches candles, so candle-only players keep their existing experience
- The candle keybind entry was renamed to "Extinguish Candle Keybind" (and the controller button to "Extinguish Candle Controller Button") to make room for the incense pair. Your existing custom bindings are migrated automatically — nothing to reconfigure
- Fixed Chinese translations not loading
- Added an update notice on the main menu that flags when this mod (or others in this collection) has a newer version on Nexus. Click an entry to open its Nexus page. Toggle off in settings if you'd rather not see it
- The Advanced section's Debug Logging option is now always visible (was hidden by default)
- Tidied up the settings menu with clearer, player-friendly descriptions and a new section layout (Advanced / Candles & Incenses / Church / Controls)
- Existing settings are migrated automatically — your keybinds, distance, and column toggle are preserved
- Translations are now loaded from JSON files in the mod's `lang/` folder, so they're easier to fix up or contribute to
- Improved debug logging so a log file is enough to diagnose issues — turn on Debug Logging before reporting a bug
- A one-time reminder now pops up in-game when Debug Logging is left on, so it doesn't stay on forever by accident

## 0.1.7 | 11 April 2026

- Fixed hotkeys and input features not working for some users
- Improved compatibility across different BepInEx versions

## 0.1.6

- Mod is now standalone — no longer requires GYK Helper

## 0.1.4

- Lang corrections

## 0.1.3

- Added option to toggle the visibility of the church columns
- Adjustments to prevent candles from burning still

## 0.1.2

- Changed distance setting to a slider, increments of 0.25
- Implemented arrow pointer, pointing to the nearest lit candle

## 0.1.1

- Fixed not being able to remove candles after saving and re-loading
- Fixed not being able to craft candles

## 0.1.0

- Initial release

