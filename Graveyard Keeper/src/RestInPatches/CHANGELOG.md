# Changelog

## 0.1.1 | 16 April 2026

- Fixes the mouse cursor sometimes vanishing and refusing to come back until you sleep or restart the game. Mostly reported on Linux.

## 0.1.0 | 12 April 2026

- First release.
- Restores the small left/right arrow icons that disappeared from the craft window's quantity buttons after a recent game update. The buttons still worked — they just had no arrow glyph on them. This fix brings the arrows back.
- Smooths out player movement by enabling physics interpolation so the character no longer snaps to grid lines.
- Adds a tunable cap on the number of footprints kept on the map, and fixes a vanilla bug where footprints pop out of existence instead of fading smoothly.
- Adds options to keep the game running when its window isn't focused, and to mute the audio while unfocused.
- Silently swallows a handful of harmless exceptions in the item tooltip, world-object interaction, tree prefab instantiation, and HUD update code that could otherwise nag your BepInEx log or cause a crash on edge-case save data.
