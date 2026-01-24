# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Purpose

This is the **decompiled game source code** for Cult of the Lamb, exported from `Assembly-CSharp.dll`. It serves as a **read-only reference** for the mod projects in the parent `Cult of the Lamb/` directory (CultOfQoL, Rebirth, Namify). This code is not compiled or modified directly — it exists solely to understand vanilla game behavior when developing Harmony patches.

## Usage Context

When working on mods in the parent directory, always consult this code to:
- Understand the actual implementation of methods being patched (never guess from names alone)
- Identify side effects that a prefix/transpiler must preserve
- Find fields, enums, and data structures used by game systems
- Trace call chains to understand how game systems interact

## Code Organization

All source is under `Assembly-CSharp/` with ~3,600 C# files. Key namespaces relevant to modding:

### Core Game Logic (root of Assembly-CSharp/)
- **DataManager.cs** - Central game state manager (13K lines), save/load, progression tracking
- **FollowerBrain.cs** - Follower AI, tasks, traits, thoughts (5K lines)
- **Health.cs** - Health system for all entities
- **Interaction_*.cs** files - Player interaction handlers for various objects
- **Structures_*.cs** files - Building/structure definitions and behavior
- **FollowerCommands/** pattern - Task execution logic for followers

### Lamb/ Namespace
Primary game UI and systems:
- **Lamb.UI/** - All UI panels (menus, altars, kitchens, rituals, tarot, etc.)
- Subdirectories match UI screen categories (BuildMenu, PauseMenu, MainMenu, Rituals, etc.)

### Map/ Namespace
World/dungeon generation and room management.

### Other Namespaces
- **I2/** - Localization system (I2 Localization). Mostly generated string tables.
- **Rewired/** - Input system bindings
- **MMTools/, MMBiomeGeneration/, MMRoomGeneration/** - Procedural generation utilities
- **Spine/** - 2D skeletal animation
- **Flockade/** - Flockade minigame implementation
- **MessagePack/** - Serialization (mostly generated resolvers)
- **src/** - Additional utility code

### lib/ Directory
Pre-compiled DLL dependencies referenced by the `.csproj` (DOTween, Sirenix, FMOD, A* Pathfinding, etc.). These are not source code.

## Key Patterns in Game Code

- Classes extend `BaseMonoBehaviour` (Unity MonoBehaviour wrapper)
- Heavy use of Unity coroutines (`IEnumerator` methods with `yield return`)
- Serialization via `[SerializeField]` and MessagePack attributes
- State machines for follower behavior and interactions
- Event-driven architecture with static event delegates
- Enum-heavy design for item types, follower traits, structure types, etc.

## Important Notes

- Files contain `#nullable disable` pragma (decompiler artifact)
- Large generated files exist (localization tables, MessagePack resolvers, MonoScript types) — these are not useful for modding reference
- The `.csproj` is exported metadata only; this project is not meant to be built
