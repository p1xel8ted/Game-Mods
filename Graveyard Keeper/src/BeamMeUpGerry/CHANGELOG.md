# Changelog

## 3.1.5 | 18 April 2026

- Fixed area-gated buildings (such as the quarry mining hut) being impossible to build after teleporting to a zone you hadn't walked into yet. Teleport now triggers the same area-discovery and area-unlock events the game fires when you arrive on foot or via a story warp
- Fixed Chinese translations not loading
- Added an update notice on the main menu that flags when this mod (or others in this collection) has a newer version on Nexus. Click an entry to open its Nexus page. Toggle off in settings if you'd rather not see it
- Settings pane tidied up: sections now use a cleaner `── Name ──` style, sub-options sit indented under their parent toggle, and every entry has a proper description. Your existing values carry across unchanged

## 3.1.4 | 12 April 2026

- Enabling Debug logging now shows a one-time in-game dialog warning you it's on, so you don't forget it's enabled
- Fixed non-English translations not loading — the mod was showing English regardless of your game language
- Language changes in the game options are now picked up immediately without needing to restart

## 3.1.3 | 11 April 2026

- Main menu now shows "BepInEx Modded" in the version text
- Fixed hotkeys and input features not working for some users
- Improved compatibility across different BepInEx versions

## 3.1.2

- Fixed custom teleport locations not appearing after loading from JSON files
- Improved zone detection for locations like lighthouse, quarry, and refugee camp
- Mod is now standalone — no longer requires GYK Helper

## 3.1.0

- Fixed custom locations not appearing in any list
- Added option to toggle removal of locations the player hasn't "seen"

## 3.0.9

- Reworked troublesome locations not appearing. You will need to re-walk to the felling site/zombie sawmill and the coal vein near the quarry for them to appear.
- Fixed multiple pages not working as intended for non-English locales.

## 3.0.7

- Fixed coal vein not appearing. For reference, this is the one near the quarry.
- You still need to visit each location after removing the blockages for them to appear in the list.
- Fixed a bug with the Farmer/Mill visibility

## 3.0.6

- Fixed some locations being available prior to removing the blockage (near the bees and the one further west).
- Fixed being able to use the teleport list without having the teleport stone
- Fixed not being able to use B/Back/Escape to exit the menu

## 3.0.5

- Fixed some starting locations not being available. YOU STILL NEED TO UNLOCK THEM i.e. unblock the road to the quarry
- Fix for page generation issue where pages generated was more than max pages.

## 1.0

- Initial release

