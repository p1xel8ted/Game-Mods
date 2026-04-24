# Changelog

## 2.1.14 | 18 April 2026

- Fixed a duplicate "Player (7/7)" widget appearing next to your bag when you opened the inventory — that was your tool belt, which the game already shows in its own dedicated slot strip
- New "Hide Bag Widgets" option (UI section, on by default) — bags and backpacks you're carrying no longer get their own expanded widget in the inventory panel. Right-click the bag icon to open it as before. Turn the option off if you prefer the old inline look
- Fixed Chinese translations not loading
- Added an update notice on the main menu that flags when this mod (or others in this collection) has a newer version on Nexus. Click an entry to open its Nexus page. Toggle off in settings if you'd rather not see it
- New option under "Collect Drops On Game Load": choose whether loose loot gets pulled straight into your pockets (old behaviour) or piled up next to your house instead, so you can sort it yourself. The pile-by-the-house mode also gathers large items that the vacuum previously skipped
- New "Near-House Dump Zone Radius" slider (default 8 tiles) — items already piled near your house stay put on load instead of being re-shuffled every time
- Vendor windows now open much faster when "Show Only Personal Inventory" is on
- Vendor windows now respect the Hide Stockpile/Tavern/Soul/Warehouse Shop Widgets toggles
- Fixed the "Inventory Dimming" toggle being inverted — ticking it now actually turns the grey-out on
- Items outside your personal inventory are no longer dimmed at vendors when the full shared list is showing
- New "Player Loot Magnet Range" slider — crank it up to suck in drops from further away. Vanilla is 1.8 tiles; the new range is 1.8 to 20 tiles
- Bag widgets now lay out in the same 5-column grid as every other inventory widget, instead of squeezing into 3 columns
- Quest items and story-critical drops are no longer scooped up or piled by the load-time loot sweep

### Inventory size

- The single Additional Inventory Space slider has been split into two — one for your player inventory and one for chests, racks, and other containers. Want a tight player carry but huge chests? Now you can. Your existing slider value is migrated to both sliders on first launch
- Both sliders now allow `0` as the minimum (= no extra slots, same as vanilla). Previously the lowest you could go was `+1`
- Changing either slider now resizes existing containers in the world too, not just newly-built ones. Before, building a chest with the slider at +20 then lowering the slider would leave that chest at +20 forever
- If you reduce a slider below the number of items in your inventory (or in any chest), you'll now see a Yes/No prompt summarising what's about to happen. Yes drops the items that no longer fit on the ground next to each affected container (your inventory items at your feet, chest items at the chest). No rolls the slider back so nothing is lost. Previously, items that no longer fit would silently disappear from view (their data was safe but hidden by the smaller slot count)
- Saves where items were already hidden by the silent-shrink bug from previous versions will reveal them as soon as you next open the affected container
- The BepInEx Configuration Manager window now closes automatically when the shrink prompt appears, so you can answer it without the slider overlay covering the buttons
- Turning off Modify Inventory Size now actually keeps your player inventory at 20 slots and it stays there. Previously it could snap back to the bigger size as soon as you built or destroyed a chest, or when drops were picked up on load
- Turning Modify Inventory Size **on** now actually keeps your player inventory at the bigger size, too. Previously the extra slots could silently disappear mid-play — your items were still saved but hidden from view, and you couldn't pick up anything new until the mod next re-applied your slider
- Changes to the size sliders or the master toggle now apply straight away, instead of waiting until you next interact with something

### Other fixes

- Fixed quarry and zombie mill containers vanishing from the quarry/mill themselves when crafting there, if you'd already crafted from somewhere else with their Exclude options on. Previously you had to build or destroy a chest to get them back
- Turning off "Don't Show Empty Rows In Inventory" now actually shows the empty rows in your personal inventory — previously the option only worked for chests
- Dragging any slider in the Configuration Manager no longer hitches the game while you drag — the change is now applied once per frame instead of on every tiny movement of the slider

### UI and diagnostics

- The mod's settings are reorganised in Configuration Manager: parent toggles are followed by their dependent options indented with `└` underneath, and the section names use a cleaner `── Name ──` style. Your existing setting values are preserved across the rename — no config reset needed
- The BepInEx log file now always includes how long each inventory check took. If you ever notice hitching, you can share the log without first needing to enable Debug Logging and reproduce the problem

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

