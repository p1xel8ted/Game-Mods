# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Repository Structure

This is a **game modification repository** containing C# BepInEx plugins for multiple indie games. The repository is organized by game directories, each containing one or more mods:

### Major Game Collections
- **Cult of the Lamb/** - Active development with solution-based architecture
- **Graveyard Keeper/** - Extensive collection of quality-of-life mods
- **Sun Haven/** - Multiple enhancement mods
- Individual game directories for other titles (DRG Survivor, Fabledom, etc.)

### Shared Architecture
- **Shared/** directories contain common utilities used across mods in the same game
- **libs/** directories contain game-specific Unity assemblies and dependencies
- **Directory.Build.props** files define MSBuild properties for each game collection

## Build System

### MSBuild Configuration
Each game collection uses MSBuild with centralized configuration:

**Build Commands:**
```bash
# Build entire solution
dotnet build "GameName/GameSolution.sln"

# Build specific project
dotnet build "GameName/ProjectName/ProjectName.csproj"

# Build for release
dotnet build "GameName/GameSolution.sln" -c Release
```

### Target Frameworks
- **Cult of the Lamb**: .NET Framework 4.8.1 (`net481`)
- **Graveyard Keeper**: .NET Standard 2.0 (`netstandard2.0`)
- **Sun Haven**: .NET Framework 4.8.1 (`net481`)

### Output Paths
Projects build directly to game directories:
- Debug/Release outputs go to `G:\SteamLibrary\steamapps\common\[GameName]\BepInEx\plugins\[ModName]`

## Development Patterns

### BepInEx Plugin Structure
All mods follow BepInEx conventions:
```csharp
[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public class Plugin : BaseUnityPlugin
{
    // Plugin implementation
}
```

### Harmony Patching
Most mods use Harmony for runtime patching:
- Patches are organized in `Patches/` subdirectories by category
- Common categories: `Gameplay/`, `UI/`, `Systems/`, `Structures/`

### Shared Code Patterns
- Extension methods in `Shared/Extensions.cs`
- Helper utilities in `Shared/Helpers.cs` 
- Configuration management through BepInEx ConfigFile system
- Logging follows standard BepInEx ManualLogSource patterns

### Code Conventions
- Uses latest C# language features where supported
- Unsafe code blocks enabled for memory operations
- Private references disabled (CopyLocal=False) for game assemblies
- Nullable disabled across projects

## Key Dependencies

### BepInEx Framework
- **BepInEx.Core** (5.4.21) - Core BepInEx functionality
- **BepInEx.BaseLib** (5.4.21) - Base libraries for plugins

### Additional Packages
- **Resource.Embedder** - For embedding resources in assemblies
- **HarmonyX** - Runtime method patching (implicit via BepInEx)

## Working with Projects

### Adding New Mods
1. Create project directory under appropriate game folder
2. Copy and modify existing `.csproj` structure
3. Reference shared libraries if available
4. Follow game-specific namespace conventions

### Game-Specific Libraries
Each game has a `libs/` directory containing:
- Unity engine assemblies
- Game-specific assemblies
- Third-party dependencies (Sirenix, etc.)

### Solution Management
Major game collections use Visual Studio solutions:
- **Cult of the Lamb**: `CultOfTheLambMods.sln`
- **Graveyard Keeper**: `GYKBepInEx5.sln`
- **Sun Haven**: `SunHaven.sln`

## Distribution

Projects include packaging for mod distribution platforms:
- Thunderstore (Cult of the Lamb mods)
- NexusMods integration
- Release builds generate mod packages automatically