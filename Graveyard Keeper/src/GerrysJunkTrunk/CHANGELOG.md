# Changelog

## 1.9.7 | 17 April 2026

- Fixed Chinese translations not loading
- Added an update notice on the main menu that flags when this mod (or others in this collection) has a newer version on Nexus. Click an entry to open its Nexus page. Toggle off in settings if you'd rather not see it

## 1.9.6 | 15 April 2026

- Fixed newly-built trunks not working as a shipping box — they stored items like a normal storage, didn't sell anything at midnight, and the craft stayed available so you could build multiple. Caused by yesterday's 1.9.5 fix clearing the build marker too early

## 1.9.5 | 14 April 2026

- Fixed the HUD and your character controls staying gone after Gerry's midnight visit if something interrupted him (sleeping, loading a save, etc.) — they now always come back
- Camera no longer keeps following the spot where Gerry was after he's left
- Fixed a rare case where backing out of placing a shipping box and then building a regular wooden storage right after could tag that storage as your shipping box by mistake
- Added a Cinematic Mode toggle in the Gerry settings: turn it off if you'd rather Gerry's midnight visit happen in the background while you keep playing, instead of pausing the game and hiding the HUD

## 1.9.4 | 12 April 2026

- Fixed non-English translations not loading — the mod was showing English regardless of your game language
- Language changes in the game options are now picked up immediately without needing to restart
- The Advanced section now appears at the top of the settings list instead of the bottom, and its Debug Logging option is always visible (was hidden by default)
- Enabling Debug logging now shows a one-time in-game dialog warning you it's on, so you don't forget it's enabled

## 1.9.3 | 11 April 2026

- Translations are now loaded from editable JSON files in the lang folder
- Users can modify or contribute translations by editing the JSON files — do not rename or move them
- Fixed several translation errors across multiple languages
- Main menu now shows "BepInEx Modded" in the version text
- Fixed hotkeys and input features not working for some users
- Improved compatibility across different BepInEx versions

## 1.9.2

- Fixed HUD occasionally not restoring after the midnight sale animation
- Gerry sale animation no longer triggers during NPC conversations or cutscenes
- Trunk sale prices now better match vendor pricing and no longer overpay for certain items
- Mod is now standalone — no longer requires GYK Helper

## 1.7

- - Russian localization corrections (Credits Sonju).
- - Gerry will now actually sell the items if spawning Gerry is disabled.
- - Fixed bug that could cause crafted items to be deleted instead of being dumped in a chest.
- - Fixed total chest value being wrong when adding up the tooltips manually.
- - Removed need to spawn/destroy copy vendors to check pricing each time.

