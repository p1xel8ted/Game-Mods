# Changelog

## 0.2.0

- New cursor-based detection mode (default) — tools now switch based on what you're aiming at instead of what you walk near
- Works with both mouse and controller (right stick aiming)
- Legacy proximity mode still available as a config option
- Fixed an issue where doors, tree shaking, and NPC interactions could stop working
- Fixed watering can detection when no watering can is on the action bar
- Fixed incorrect scythe entries in the tool list
- Added a switch cooldown setting to prevent rapid tool flickering

## 0.0.9

- Minor update for 2.5.1b (no functional changes).

## 0.0.8

- Minor update for 2.0.2c (no functional changes).

## 0.0.7

- KeepAlive is now a hard dependency, meaning the mod will not load without it. The BepInEx log window will notify you accordingly. Do not raise a bug report.

## 0.0.6

- Updated for 1.4+

## 0.0.5

- Added combat/enemy detection improvements to control switching when enemies are present.
- Some config categories have change which will reset the related settings. i.e. check your config

## 0.0.4

- Can now handle multiple of the same tool; it will pick the highest you can use.
- For watering cans, same as above, but it will pick the highest that has water available
- If you have no watering cans with water, it will select the highest you can use so you can refill it. It will prioritize empty watering cans over the fishing rod when you go to a water source
- Added ability to adjust fishing rod switch range (was initially to prevent switching when you're in combat near a water source, but the below solves that now).
- Removed combat switching. Auto switch is disabled while player is in combat.

## 0.0.3

- Fixed water can bug.
- Watering Can is now prioritized over the fishing rod if its empty and you're near water

