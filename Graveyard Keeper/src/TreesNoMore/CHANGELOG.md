# Changelog

## 2.5.12 | 17 April 2026

- Fixed Chinese translations not loading
- Added an update notice on the main menu that flags when this mod (or others in this collection) has a newer version on Nexus. Click an entry to open its Nexus page. Toggle off in settings if you'd rather not see it
- Settings menu reorganised into clearer sections with full player-facing descriptions on every option; existing setting values are migrated automatically
- Debug log output is more useful for bug reports — every step the mod takes is now traceable when Debug Logging is on

## 2.5.11 | 13 April 2026

- Fixed a bug where every time a chopped stump was re-rendered (entering/leaving zones, save reloads), the mod would add a duplicate entry for the same stump, write the entire tracking file to disk, and log a noisy "Saved/Removed duplicate" pair. The mod now skips stumps it already knows about
- The "Loaded N trees", "Saved N trees", and "Removed N duplicate trees" log lines are now only emitted when Debug Logging is enabled — the regular log stays clean

## 2.5.10 | 12 April 2026

- Fixed non-English translations not loading — the mod was showing English regardless of your game language
- Language changes in the game options are now picked up immediately without needing to restart
- The Advanced section's Debug Logging option is now always visible (was hidden by default)
- Enabling Debug logging now shows a one-time in-game dialog warning you it's on, so you don't forget it's enabled

## 2.5.9 | 11 April 2026

- Added translations for 11 languages (de, es, fr, it, ja, ko, pl, pt-BR, ru, zh-CN)
- Translations are loaded from editable JSON files in the lang folder — do not rename or move them
- Main menu now shows "BepInEx Modded" in the version text
- Improved compatibility across different BepInEx versions

## 2.5.8

- Fixed tree removal not working correctly in some cases
- Fixed duplicate tree tracking
- Mod is now standalone — no longer requires GYK Helper

## 2.5.6

- Trees are tracked per save file now. After first update, trees will reset (due to aforementioned file change)

## 1

- Initial release

