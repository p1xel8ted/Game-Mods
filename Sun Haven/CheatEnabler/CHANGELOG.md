# Changelog

## 0.3.7

- Updated for game version 3.0.2b

## 0.3.6

- Minor update for 2.5.1b (no functional changes).

## 0.3.5

- Minor update for 2.0.2c (no functional changes).

## 0.3.4

- Update for 2.0.2c

## 0.3.3

- Update for 1.7

## 0.3.2

- Fix for language dictionary errors

## 0.3.1

- Fix for netstandard missing issue, hopefully.

## 0.3.0

- Added descriptions for new cheats added in 1.5 /unlockteleport /lockteleport
- Use /printallteleportlocations to see valid options

## 0.2.9

- Descriptions are now stored externally, in 16 languages. See 'CheatEnablerLang` folder.
- Lynn now has a special message on first run of the mod.
- Improvements to certain built-in commands around error messages. Fixed some that don't work at all.

## 0.2.8

- Patcher no longer required for adding descriptions to existing commands.

## 0.2.7

- Added checks with clearer messaging regarding try to add DLC items via Cheat Enabler

## 0.2.6

- Fixed CTD issue

## 0.2.5

- KeepAlive is now a hard dependency, meaning the mod will not load without it. The BepInEx log window will notify you accordingly. Do not raise a bug report.

## 0.2.4

- Fix for custom commands not being loaded.

## 0.2.3

- Added various /print cmds to assist with other commands
- Built-in commands now have descriptions. Use `/man additem` for example to see.
- Published command manual, https://github.com/p1xel8ted/Game-Mods/blob/main/Sun%20Haven/CheatEnablerCommands.md

## 0.2.2

- Added /saveallitems command. Will save a text file in the game's directory (and open it) of an alphabetical list of all items with their item code - name - localized name -
- Added /finditemid. Search any part of the item name to return possible matches.

