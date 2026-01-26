# Trait Control

Control your followers' traits with this quality-of-life mod.

## Features

### No Negative Traits
- Automatically replaces negative traits with positive ones when followers are created
- Backs up original traits and can restore them if feature is disabled
- Configurable options for which traits to include (Immortal, Disciple, unlocked traits only)

### Trait Weights (Trait Probability Control)
Control how likely each trait is to appear on new followers. Each trait has a weight slider from 0 to 10.

**How it works:**
- **Higher weight = more likely to appear** - Want more Faithful followers? Slide it up to 5 or 10.
- **Lower weight = less likely** - Don't want Materialistic followers? Slide it down.
- **Weight of 0 = disabled** - That trait will never appear on new followers.
- All traits default to 1.0 (equal chance).

**Examples:**
- Set "Faithful" to 5.0, leave others at 1.0 → Faithful is 5x more likely than other traits
- Set "Materialistic" to 0 → Materialistic will never appear
- Set all negative traits to 0, positive traits to 10 → Only positive traits will appear

The trait list is dynamically generated from the game, so new traits added by the developers will automatically appear in the config.

## Configuration

All settings are configurable via the in-game mod settings menu (F1) or the BepInEx config file.

## Incompatibilities

This mod is incompatible with "Nothing Negative" or any other mod that modifies the trait system.

## Donate

If you enjoy the mod, please consider a donating [here](https://ko-fi.com/p1xel8ted) or using the button below.

[![KoFiLogo](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/p1xel8ted)

## Installation

* Install [BepInExPack CultOfTheLamb](https://thunderstore.io/c/cult-of-the-lamb/p/BepInEx/BepInExPack_CultOfTheLamb/)
* Install [Configuration Manager](https://thunderstore.io/c/cult-of-the-lamb/p/p1xel8ted/BepInEx_Configuration_Manager/)
* Place the plugin DLL into your "...\Cult of the Lamb\BepInEx\plugins" folder.

## Issues

* Please report issues on the mod page or GitHub.
