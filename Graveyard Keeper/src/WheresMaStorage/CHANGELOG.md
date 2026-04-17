# Changelog

## 2.1.14 | 17 April 2026

- Fixed Chinese translations not loading
- Added an update notice on the main menu that flags when this mod (or others in this collection) has a newer version on Nexus. Click an entry to open its Nexus page. Toggle off in settings if you'd rather not see it
### Inventory size

- The single Additional Inventory Space slider has been split into two — one for your player inventory and one for chests, racks, and other containers. Want a tight player carry but huge chests? Now you can. Your existing slider value is migrated to both sliders on first launch
- Both sliders now allow `0` as the minimum (= no extra slots, same as vanilla). Previously the lowest you could go was `+1`
- Changing either slider now resizes existing containers in the world too, not just newly-built ones. Before, building a chest with the slider at +20 then lowering the slider would leave that chest at +20 forever
- If you reduce a slider below the number of items in your inventory (or in any chest), you'll now see a Yes/No prompt summarising what's about to happen. Yes drops the items that no longer fit on the ground next to each affected container (your inventory items at your feet, chest items at the chest). No rolls the slider back so nothing is lost. Previously, items that no longer fit would silently disappear from view (their data was safe but hidden by the smaller slot count)
- Saves where items were already hidden by the silent-shrink bug from previous versions will reveal them as soon as you next open the affected container
- The BepInEx Configuration Manager window now closes automatically when the shrink prompt appears, so you can answer it without the slider overlay covering the buttons
- Turning off Modify Inventory Size now actually keeps your player inventory at 20 slots. Previously it could snap back to the bigger size as soon as you built or destroyed a chest, or when drops were picked up on load
- Changes to the size sliders or the master toggle now apply straight away, instead of waiting until you next interact with something

### Other fixes

- Fixed quarry and zombie mill containers vanishing from the quarry/mill themselves when crafting there, if you'd already crafted from somewhere else with their Exclude options on. Previously you had to build or destroy a chest to get them back
- Turning off "Don't Show Empty Rows In Inventory" now actually shows the empty rows in your personal inventory — previously the option only worked for chests
- Dragging any slider in the Configuration Manager no longer hitches the game while you drag — the change is now applied once per frame instead of on every tiny movement of the slider

### UI and diagnostics

- The mod's settings are reorganised in Configuration Manager: parent toggles are followed by their dependent options indented with `└` underneath, and the section names use a cleaner `── Name ──` style. Your existing setting values are preserved across the rename — no config reset needed
- The log now always records how long inventory checks take, so any hitching can be diagnosed without enabling Debug Logging first

## 2.1.13 | 12 April 2026

- Fixed the +20 inventory slots option not being respected — disabling it now correctly keeps your player inventory at 20 slots, even after loading a save
- If you had previously enabled +20 slots, disabling the option and loading your save now properly restores the standard 20-slot inventory

## 2.1.12 | 12 April 2026

- Fixed merchants showing empty trade tabs after installing the mod
- Fixed the "Exclude Zombie Mill From Shared Inventory" option not actually hiding zombie mill items from crafting elsewhere
- Fixed sin shards stacking to 999 on soul containers, which was breaking gratitude point crafting
- Fixed non-English translations not loading — the mod was showing English regardless of your game language
- Language changes in the game options are now picked up immediately without needing to restart
- The Advanced section's Debug Logging option is now always visible (was hidden by default)
- Enabling Debug logging now shows a one-time in-game dialog warning you it's on, so you don't forget it's enabled

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

