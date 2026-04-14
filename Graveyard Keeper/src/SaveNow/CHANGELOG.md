# Changelog

## 2.5.10 | 14 April 2026

- Fixed auto-save filenames colliding: two autosaves on the same day could silently overwrite each other. New saves use a correct date format; existing saves are unaffected
- Save list now sorts every time. Previously it silently did nothing unless you flipped a toggle, which is why autosaves crowded out your latest manual save
- Sort mode (Game Time or Real Time) and direction (Descending or Ascending) are now dropdowns instead of two confusingly-named toggles
- Added "Pin Last Played To Top" — keeps the save you most recently loaded or saved at the top regardless of sort. Off by default
- Default Maximum Saves Visible raised from 3 to 20
- Save Interval is now a 1-60 minute slider instead of raw seconds
- New File On Auto Save now defaults to off, so your save list no longer fills up with a new file every 10 minutes
- Settings menu reorganised with nested sub-options and rewritten descriptions. Existing values are preserved

## 2.5.9 | 12 April 2026

- Fixed non-English translations not loading — the mod was showing English regardless of your game language
- Language changes in the game options are now picked up immediately without needing to restart
- The Advanced section now appears at the top of the settings list instead of the bottom, and its Debug Logging option is always visible (was hidden by default)
- Enabling Debug logging now shows a one-time in-game dialog warning you it's on, so you don't forget it's enabled

## 2.5.8 | 11 April 2026

- Fixed save key and auto-save not working for players early in the game
- Fixed exit menu freeze when pressing "No" on the save confirmation dialog
- Fixed dungeon detection not always working correctly
- Save key now works regardless of BepInEx HideManagerGameObject setting
- Translations are now loaded from editable JSON files in the lang folder
- Users can modify or contribute translations by editing the JSON files — do not rename or move them
- Fixed several translation errors across multiple languages
- Main menu now shows "BepInEx Modded" in the version text
- Improved compatibility across different BepInEx versions

## 2.5.7

- Clarified that Auto Save and Save On New Day are independent settings
- Fixed save list disappearing when Maximum Saves Visible was set to 0
- Fixed auto-save timer permanently stopping after certain conditions
- Fixed "stop auto-save" not actually stopping a save in progress
- Fixed multiple auto-save timers running at the same time
- Fixed save list crashing when two saves had the same timestamp
- Mod is now standalone — no longer requires GYK Helper

## 2.5.5

- Reworked the Sort by last modified setting

## 2.5.3

- Fixed auto-save continuing to save if you disable it mid game.

## 1

- Initial release

