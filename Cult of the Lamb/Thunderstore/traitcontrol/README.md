# Trait Control

Control your followers' traits with this quality-of-life mod.

*Originally part of [Cult of QoL](https://thunderstore.io/c/cult-of-the-lamb/p/p1xel8ted/Cult_of_QoL_Collection/), these trait features grew substantial enough to deserve their own dedicated mod.*

## Features

### Trait Replacement
- Automatically replaces negative traits with positive ones on all followers (existing and new)
- Backs up original traits and can restore them if feature is disabled
- Option to use only unlocked traits for replacements
- Option to pull from all trait pools instead of the game's separate pools
- Exclusive traits (like Lazy/Industrious) can be replaced with their counterpart or a random trait

### Unique Traits
Control whether special/crossover traits can appear in trait pools:
- **Immortal** - normally a special reward
- **Disciple** - normally a special reward
- **Dont Starve** - crossover trait (follower doesn't need to eat)
- **Blind** - crossover trait
- **Born To The Rot** - crossover trait

### Trait Weights
Control how likely each trait is to appear on new followers. Each trait has a weight slider from 0 to 10. Weights are relative to each other - a trait with weight 10 is 10x more likely to appear than a trait with weight 1.

- **Higher weight = more likely relative to other traits**
- **Lower weight = less likely relative to other traits**
- **Weight of 0 = disabled** - that trait will never appear
- All traits default to 1.0 (equal chance)

The trait list is dynamically generated from the game, so new traits added by the developers will automatically appear in the config.

### Notifications
Optional notifications when trait replacement adds or removes traits.

## Configuration Examples

### "I want all followers to have only positive traits"
| Setting | Value |
|---------|-------|
| Enable Trait Replacement | ON |
| Use Unlocked Traits Only | OFF |
| Prefer Exclusive Counterparts | ON |

**Result:** All negative traits are replaced. Lazy becomes Industrious, Faithless becomes Faithful, etc.

### "I want to allow Immortal trait on new followers"
| Setting | Value |
|---------|-------|
| Include Immortal | ON |

**Result:** Immortal can now appear when new followers join. Only one follower can have it at a time.

### "I want Faithful trait to appear more often"
| Setting | Value |
|---------|-------|
| Enable Trait Weights | ON |
| Faithful (in Good Traits) | 5.0 |
| Other traits | 1.0 (default) |

**Result:** Faithful is 5x more likely to appear than other traits on new followers.

### "I want exclusive traits replaced with random positive traits"
| Setting | Value |
|---------|-------|
| Enable Trait Replacement | ON |
| Prefer Exclusive Counterparts | OFF |

**Result:** Lazy gets replaced with a random positive trait instead of Industrious.

### "I only want traits I've unlocked via doctrines"
| Setting | Value |
|---------|-------|
| Enable Trait Replacement | ON |
| Use Unlocked Traits Only | ON |

**Result:** Only traits you've unlocked through doctrines will be used as replacements.

### "I want to pull from all traits, but not unique ones"
| Setting | Value |
|---------|-------|
| Use All Traits Pool | ON |
| Include Immortal | OFF |
| Include Disciple | OFF |
| Include Dont Starve | OFF |
| Include Blind | OFF |
| Include Born To The Rot | OFF |

**Result:** Trait selection uses all trait pools combined, but unique/crossover traits are excluded.

### "I never want Materialistic followers"
| Setting | Value |
|---------|-------|
| Enable Trait Weights | ON |
| Materialistic (in Bad Traits) | 0 |

**Result:** Materialistic trait will never appear on new followers. No more gift demands.

### "I want a cult of hard workers"
| Setting | Value |
|---------|-------|
| Enable Trait Replacement | ON |
| Prefer Exclusive Counterparts | ON |
| Enable Trait Weights | ON |
| Industrious (in Good Traits) | 10.0 |
| Lazy (in Bad Traits) | 0 |

**Result:** Existing Lazy followers become Industrious. New followers are much more likely to be Industrious and will never be Lazy.

### "I want followers who never die of old age"
| Setting | Value |
|---------|-------|
| Include Immortal | ON |
| Enable Trait Weights | ON |
| Immortal (in Good Traits) | 10.0 |

**Result:** Immortal trait is available and heavily favored. Note: Only one follower can have Immortal at a time.

### "I want a zealous cult"
| Setting | Value |
|---------|-------|
| Enable Trait Weights | ON |
| Faithful (in Good Traits) | 10.0 |
| Faithless (in Bad Traits) | 0 |
| SacrificeEnthusiast (in Good Traits) | 5.0 |

**Result:** New followers strongly favor Faithful and SacrificeEnthusiast traits.

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
| RoyalPooper (in Good Traits) | 10.0 |
| Other traits | 0 |

**Result:** New followers will have the RoyalPooper trait (produces gold instead of poop). Note: RoyalPooper is only in the Rare/Faithful pools, so "Use All Traits Pool" must be enabled for it to appear on regular new followers.

## Installation

* Install [BepInExPack CultOfTheLamb](https://thunderstore.io/c/cult-of-the-lamb/p/BepInEx/BepInExPack_CultOfTheLamb/)
* Install [Configuration Manager](https://thunderstore.io/c/cult-of-the-lamb/p/p1xel8ted/BepInEx_Configuration_Manager/)
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
