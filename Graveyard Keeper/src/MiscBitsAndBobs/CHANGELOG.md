# Changelog

## 2.3.5 | 17 April 2026

- Fixed Chinese translations not loading
- Added an update notice on the main menu that flags when this mod (or others in this collection) has a newer version on Nexus. Click an entry to open its Nexus page. Toggle off in settings if you'd rather not see it
- Tidied up the settings menu — every toggle now has a plain-English description that explains what it actually does, and dependent sliders (player/porter speed) now show underneath their parent toggle with a tree indent
- Reordered the sections into a clearer layout: Advanced, Audio, UI, Gameplay, Movement, Church, Misc. Your existing settings are migrated automatically
- Improved debug logging for easier bug reports — when Debug Logging is on, the log now records each decision the mod makes (letterbox suppression, intro skip, prayer removal, tavern oven fuel changes, zombie pyre inputs, Halloween event scheduling, sermon line substitution, Sprint Reloaded detection, and church visitor eviction steps)
- Logging only runs when Debug Logging is on, so there's no performance cost when it's off

## 2.3.4 | 15 April 2026

- Fixed "Remove Cinematic Letterboxing" not working

## 2.3.3 | 12 April 2026

- Moved some features out into Rip In Patches
- Fixed non-English translations not loading — the mod was showing English regardless of your game language
- Language changes in the game options are now picked up immediately without needing to restart
- The Advanced section now appears at the top of the settings list instead of the bottom, and its Debug Logging option is always visible (was hidden by default)
- Enabling Debug logging now shows a one-time in-game dialog warning you it's on, so you don't forget it's enabled

## 2.3.2 | 11 April 2026

- Translations are now loaded from editable JSON files in the lang folder
- Users can modify or contribute translations by editing the JSON files — do not rename or move them
- Fixed several translation errors across multiple languages
- Main menu now shows "BepInEx Modded" in the version text
- Added player judder fix — smooths out stuttering/jittering when moving (can be toggled off in config)
- Added footprint performance improvements — caps maximum footprints to prevent memory buildup over long play sessions
- Added configurable footprint fade speeds for outside, inside, and rain
- Fixed a game bug where footprints pop out of existence instead of fading smoothly
- Added Evict Church Visitors — force stuck church visitors to leave via the config menu
- Improved compatibility across different BepInEx versions

## 2.3.1

- Fixed compatibility issue with Pray the Day Away
- Mod is now standalone — no longer requires GYK Helper

## 2.2.8

- Fixed/added option for mute on alt tab, and allow the game to run in the background

## 1

- Initial release

