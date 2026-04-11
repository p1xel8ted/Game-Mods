# Changelog

## 2.1.11 | 11 April 2026

- Translations are now loaded from editable JSON files in the lang folder
- Users can modify or contribute translations by editing the JSON files — do not rename or move them
- Fixed several translation errors across multiple languages
- Main menu now shows "BepInEx Modded" in the version text
- Improved compatibility across different BepInEx versions

## 2.1.10

- Fixed game freezing when cancelling the exit-to-menu dialog
- Fixed organ duplication when using the BSS organ enhancer with shared inventory
- Newly built or destroyed storage containers now refresh the shared inventory automatically
- Quarry crafting stations can now access shared inventory from other zones when Exclude Quarry is enabled
- Mod is now standalone — no longer requires GYK Helper

## 2.1.7

- Quarry is now totally independent when Exclude Quarry enabled (as is the mill etc.). If you want to craft something in those zones, you will need to have the materials on hand or in storage within those zones.

## 2.1.6

- Fix for not being able to see quarry inventories when crafting from the quarry zone with Exclude Quarry enabled
- Fix for apple trees and bushes gaining access to shared inventories (just caused unnecessary overhead)

## 2.1.5

- Allow/disallow zombies access to shared inventory (means the can only access storage in the same zone as them - game default).
- Stack sizes and inventory sizes can be adjusted without restart.
- Fixed player inventory not increasing
- Probs some others

## 2.1.4

- Allow/disallow zombies access to shared inventory (means the can only access storage in the same zone as them - game default).
- Stack sizes and inventory sizes can be adjusted without restart.
- Fixed player inventory not increasing
- Probs some other stuff

## 2.0.2

- - Refugee happiness should be correct on the refugee build desk.
- - Stockpiles should be visible correctly.

## 2.0

- - Russian localization corrections (Credits Sonju).
- - Fixed bug that would cause grave items to be stackable despite having the option turned off.
- - Rewrote multi-inventory stuff (core of the mod), should notice an FPS gain compared to previous release.
- - Potential fix for NPC inventories (when found in the "wilderness") appearing in player inventory.
- - Potential fix for toolbelt shenanigans.

