# Trait Control

Control your followers' traits with this quality-of-life mod.

*Originally part of [Cult of QoL](https://thunderstore.io/c/cult-of-the-lamb/p/p1xel8ted/Cult_of_QoL_Collection/), these trait features grew substantial enough to deserve their own dedicated mod.*

## Features

### Trait Replacement
- Automatically replaces negative traits with positive ones on all followers (existing and new)
- Backs up original traits and can restore them if feature is disabled
- Option to use only unlocked traits for replacements (enabled by default)
- Option to pull from all trait pools instead of the game's separate pools
- Exclusive traits (like Lazy/Industrious) can be replaced with their counterpart (default behavior) or a random trait

### Unique Traits
Control whether special/crossover traits can appear in trait pools:
- **Immortal** - normally a special reward
- **Disciple** - normally a special reward
- **Dont Starve** - crossover trait (follower doesn't need to eat)
- **Blind** - crossover trait
- **Born To The Rot** - crossover trait

Each unique trait has two options:
- **Include** - allows the trait to appear in trait pools
- **Guarantee** - new followers will always receive this trait (ignores weights). Only one follower can have this trait unless "Allow Multiple Unique Traits" is enabled.

Additional option:
- **Allow Multiple Unique Traits** - normally only one follower can have each unique/single-use trait (Immortal, Disciple, Lazy, Snorer, etc.). Enable this to allow multiple followers to have the same trait.

### Trait Count
Control how many traits followers receive:
- **Minimum Traits** (2-8, default: 2) - the minimum number of traits a new follower will have
- **Maximum Traits** (2-8, default: 3) - the maximum number of traits a new follower will have
- **Randomize Traits on Re-indoctrination** - when re-indoctrinating an existing follower at the altar, randomize their traits using the configured min/max (vanilla only changes appearance/name)

Vanilla behavior is 2-3 traits for new followers (randomly chosen between min and max), with a cap of 6 during re-indoctrination. Increase these values to give followers more traits (limited to 8 due to UI constraints). The maximum setting also affects re-indoctrination, removing the vanilla cap of 6.

- **Trait Reroll via Reeducation** - adds the Re-educate command to normal followers (dissenters already have this in vanilla). Using it will re-roll their traits using your configured min/max and weights. Works with "Enable Trait Replacement" to replace negative traits. A notification displays showing the follower's name and trait count change.

- **Protect Trait Count on Reroll** - ensures followers don't end up with fewer traits than they started with when using reeducation or reindoctrination. If the reroll results in fewer traits, additional traits are added to match the original count.

**Note:** If "Use Unlocked Traits Only" is enabled and you don't have enough traits unlocked, followers may receive fewer traits than the minimum. A warning will appear in the log when this happens.

### Trait Weights
Control how likely each trait is to appear on new followers. Each trait has a weight slider from 0 to 100. Weights are relative to each other.

- **Higher weight = more likely relative to other traits**
- **Lower weight = less likely relative to other traits**
- **Weight of 0 = disabled** - that trait will never appear
- All traits default to 1.0 (equal chance)
- Sliders snap to 0.05 increments for precise control (values below 0.1 snap to 0)

**Localization:** Trait names display in your current game language alongside the internal name, e.g. "Lover of Cold (Chionophile)". Names update automatically when you change languages. Traits with missing translations are marked with an asterisk (*), indicating they are not fully implemented in the game itself.

**Understanding the math:** With "Use All Traits Pool" enabled, there are ~85 traits competing. If all are at weight 1 and you set Immortal to weight 50, the probability is: 50/(84+50) ≈ 37% per follower. At weight 100: 100/(84+100) ≈ 54% per follower.

The trait list is dynamically generated from the game, so new traits added by the developers will automatically appear in the config.

### Trait Categories

Each trait in the configuration shows which game lists it belongs to at the end of the description, e.g. "Found in: Starting, Rare, Faithful". Traits not in any pool show "Granted via other means (rituals, events, etc.)". This helps you understand which traits are safe to disable without affecting unlocks.

| Category | Description |
|----------|-------------|
| Starting | Naturally appears on new followers (default pool) |
| Rare | 20% chance to appear instead of a third starting trait |
| Faithful | Granted when a follower becomes faithful (spider web interaction) |
| Unique | Special reward traits - only one follower can have each (Immortal, Disciple, etc.) |
| Single | Only one follower in your cult can have this trait at a time |
| Sin | Sin traits (requires Pleasure Shrine from Sins of the Flesh DLC) |
| DLC | Requires DLC content to be active |
| Winter | Winter/seasons-specific traits |
| Event | Granted through gameplay events (marriage, parenting, criminal, etc.) |
| Unlock | Requires at least one boss defeated |

Traits can belong to multiple categories. For example, `[Single, Starting]` means the trait appears in the starting pool but only one follower can have it.

### Excluded Traits

Some traits are excluded from the weights list because they require special game state, are granted through gameplay events, or already apply cult-wide:

**Always Excluded** (cannot be randomly assigned):

| Trait | Reason |
|-------|--------|
| Spy | Requires SpyJoinedDay to be set or followers leave immediately |
| BishopOfCult | Story-related, granted when converting a bishop |
| Doctrine Traits | Traits you've unlocked via doctrines (Fertility, Allegiance, Cannibal, etc.) are automatically excluded since they already apply to your entire cult. Rolling them as individual traits would waste a slot. |

**Event Traits** (excluded by default, enable "Include Event Traits" to add them). Note: "Include Event Traits" only applies when "Use All Traits Pool" is enabled:

| Category | Traits |
|----------|--------|
| Marriage | MarriedHappily, MarriedUnhappily, MarriedJealous, MarriedMurderouslyJealous |
| Parenting | ProudParent, OverworkedParent |
| Widowing | HappilyWidowed, GrievingWidow, JiltedLover |
| Criminal | CriminalEvangelizing, CriminalHardened, CriminalReformed, CriminalScarred |
| Missionary | MissionaryExcited, MissionaryInspired, MissionaryTerrified |
| Special | ExCultLeader, ExistentialDread |
| DLC/Snowman | InfusibleSnowman, MasterfulSnowman, ShoddySnowman |
| DLC/Other | MutatedVisual, PureBlood, PureBlood_1/2/3, FreezeImmune |

### Notifications
Optional notifications when trait replacement adds or removes traits.

### Reset Settings
Reset all configuration options to their default values (vanilla game behavior) with a single click. Found in section "07. Reset Settings" in the Configuration Manager.

## Configuration Examples

### "I want all followers to have only positive traits"
| Setting | Value |
|---------|-------|
| Enable Trait Replacement | ON |
| Use Unlocked Traits Only | OFF (default: ON) |
| Prefer Exclusive Counterparts | ON (default) |

**Result:** All negative traits are replaced. Lazy becomes Industrious, Faithless becomes Faithful, etc. Set "Use Unlocked Traits Only" to OFF to access all positive traits regardless of progression.

### "I want to allow Immortal trait on new followers"
| Setting | Value |
|---------|-------|
| Use All Traits Pool | ON |
| Include Immortal | ON |

**Result:** Immortal can now appear when new followers join.

### "I want Faithful trait to appear more often"
| Setting | Value |
|---------|-------|
| Use All Traits Pool | ON |
| Enable Trait Weights | ON |
| Faithful (in Good Traits) | 5.0 |
| Other traits | 1.0 (default) |

**Result:** Faithful is 5x more likely to appear than other traits on new followers. Note: Faithful is only in the Rare pool by default, so "Use All Traits Pool" is needed for it to appear on regular new followers.

### "I want exclusive traits replaced with random positive traits"
| Setting | Value |
|---------|-------|
| Enable Trait Replacement | ON |
| Prefer Exclusive Counterparts | OFF (default: ON) |

**Result:** Lazy gets replaced with a random positive trait instead of Industrious.

### "I only want traits available at my game progression"
| Setting | Value |
|---------|-------|
| Enable Trait Replacement | ON |
| Use Unlocked Traits Only | ON (default) |

**Result:** Traits are filtered by game progression (e.g., Fashionable requires Tailor, Sin traits require Pleasure Shrine, some traits require Day 7+). This is enabled by default.

### "I want to pull from all traits, but not unique ones"
| Setting | Value |
|---------|-------|
| Use All Traits Pool | ON |
| Include Immortal | OFF |
| Include Disciple | OFF |
| Include Dont Starve | OFF |
| Include Blind | OFF |
| Include Born To The Rot | OFF |

**Result:** Trait selection uses all trait pools combined, but unique/crossover traits are excluded. Event traits are also excluded by default.

### "I want access to every single trait in the game"
| Setting | Value |
|---------|-------|
| Use All Traits Pool | ON |
| Include Immortal | ON |
| Include Disciple | ON |
| Include Dont Starve | ON |
| Include Blind | ON |
| Include Born To The Rot | ON |
| Include Event Traits | ON |

**Result:** Every trait in the game can appear on new followers, including special/crossover and event traits.

### "I never want Materialistic followers"
| Setting | Value |
|---------|-------|
| Enable Trait Weights | ON |
| Materialistic (in Bad Traits) | 0 |

**Result:** Materialistic trait will never appear on new followers. No more gift demands.

### "I want no lazy followers"
| Setting | Value |
|---------|-------|
| Enable Trait Replacement | ON |
| Prefer Exclusive Counterparts | ON |
| Enable Trait Weights | ON |
| Lazy (in Bad Traits) | 0 |

**Result:** Existing Lazy followers get their trait replaced. New followers will never have the Lazy trait.

### "I want followers who never die of old age"
| Setting | Value |
|---------|-------|
| Use All Traits Pool | ON |
| Include Immortal | ON |
| Enable Trait Weights | ON |
| Immortal (in Good Traits) | 50-100 |

**Result:** Immortal trait is available and heavily favored (~37-54% chance per follower).

### "I want a cult full of immortals (guaranteed)"
| Setting | Value |
|---------|-------|
| Include Immortal | ON |
| Guarantee Immortal | ON |
| Allow Multiple Unique Traits | ON |

**Result:** Every new follower will have the Immortal trait, guaranteed. No weights needed.

### "I want a cult full of immortals (weights method)"
| Setting | Value |
|---------|-------|
| Use All Traits Pool | ON |
| Include Immortal | ON |
| Allow Multiple Unique Traits | ON |
| Enable Trait Weights | ON |
| Immortal (in Good Traits) | 100 |
| Other traits | 0 |

**Result:** Every new follower will have the Immortal trait via weighted selection.

### "I want a faithful cult"
| Setting | Value |
|---------|-------|
| Use All Traits Pool | ON |
| Enable Trait Weights | ON |
| Faithful (in Good Traits) | 50 |
| Faithless (in Bad Traits) | 0 |

**Result:** New followers strongly favor the Faithful trait and will never be Faithless. Note: Faithful is in the Rare pool (20% chance for third trait slot in vanilla). "Use All Traits Pool" makes it equally likely as any other trait.

### "I want to keep negative traits but control which ones appear"
| Setting | Value |
|---------|-------|
| Enable Trait Replacement | OFF |
| Enable Trait Weights | ON |
| (Set unwanted bad traits to 0) | 0 |

**Result:** Negative traits still appear, but you control exactly which ones are allowed.

### "I want notifications when traits are replaced"
| Setting | Value |
|---------|-------|
| Enable Trait Replacement | ON |
| Show When Removing Traits | ON |
| Show When Adding Traits | ON |

**Result:** You'll see notifications whenever the mod removes a negative trait or adds a positive replacement.

### "I want followers with the Golden Poop trait (RoyalPooper)"
| Setting | Value |
|---------|-------|
| Use All Traits Pool | ON |
| Enable Trait Weights | ON |
| RoyalPooper (in Good Traits) | 100 |
| Other traits | 0 |

**Result:** New followers will have the RoyalPooper trait (produces gold instead of poop). Note: RoyalPooper is only in the Rare/Faithful pools, so "Use All Traits Pool" must be enabled for it to appear on regular new followers.

### "I want followers with lots of traits"
| Setting | Value |
|---------|-------|
| Minimum Traits | 5 |
| Maximum Traits | 8 |

**Result:** New followers will have 5-8 traits instead of the vanilla 2-3. (Maximum is 8 due to UI constraints.)

## Installation

* Install [BepInExPack CultOfTheLamb](https://thunderstore.io/c/cult-of-the-lamb/p/BepInEx/BepInExPack_CultOfTheLamb/)
* Install [BepInEx Configuration Manager](https://thunderstore.io/c/cult-of-the-lamb/p/p1xel8ted/BepInEx_Configuration_Manager/) (for in-game configuration)
* Place the plugin DLL into your "...\Cult of the Lamb\BepInEx\plugins" folder.

## Configuration

* Press F1 in game to open the configuration manager.
* The configuration file can be found in the "...\Cult of the Lamb\BepInEx\config" folder.

## Incompatibilities

This mod is incompatible with "Nothing Negative" or any other mod that modifies the trait system.

## My Other Mods

- [Cult of QoL](https://thunderstore.io/c/cult-of-the-lamb/p/p1xel8ted/Cult_of_QoL_Collection/) - Massive quality-of-life overhaul with 100+ features
- [Rebirth](https://thunderstore.io/c/cult-of-the-lamb/p/p1xel8ted/Rebirth/) - Followers can be reborn with new names and appearances
- [Namify](https://thunderstore.io/c/cult-of-the-lamb/p/p1xel8ted/Namify/) - Random name generation from 1000+ names plus custom names
- [Skip of the Lamb](https://thunderstore.io/c/cult-of-the-lamb/p/p1xel8ted/Skip_of_the_Lamb/) - Skip intros, splash screens, and videos
- [Skip of the Lamb Lite](https://thunderstore.io/c/cult-of-the-lamb/p/p1xel8ted/Skip_of_the_Lamb_Lite/) - Just skips splash screens, no config needed
- [Trait Control](https://thunderstore.io/c/cult-of-the-lamb/p/p1xel8ted/Trait_Control/) - Replace negative traits, control trait probabilities
- [Glyph Override](https://thunderstore.io/c/cult-of-the-lamb/p/p1xel8ted/Glyph_Override/) - Force specific controller button prompts

## Donate

If you enjoy the mod, please consider donating [here](https://ko-fi.com/p1xel8ted) or hit that thumbs up button!

[![KoFiLogo](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/p1xel8ted)

## Issues

Please report issues on the mod page or GitHub.
