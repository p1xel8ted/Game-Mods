### 2.4.1 - 07/02/2026

**New Features:**
* Added Elder Work Mode with three options: Disabled (vanilla), All Work, or Light Work Only. Light work allows elders to worship, cook, brew, and research while excluding physical labor like mining and body disposal.
* Added "Exclude Grass From Seed Deposit" option - when using "Deposit All Seeds" on a seed silo, grass will not be deposited.
* Added "Mass Notification Threshold" setting - when a mass action affects more than the configured number of followers (default 3), a single summary notification is shown instead of one per follower.
* Added "Rot Fertilizer Decay" option - rot fertilizer ground warming now expires after a configurable number of days (1-30, default 5) instead of lasting forever. Disabled by default.
* Added "Mass Action Costs" section with configurable costs for mass actions: gold, game time, and faith reduction for Bless/Inspire. Costs can be charged per mass action (flat fee) or per follower via a dropdown. Optional cost preview shows estimated cost in the command wheel when highlighting a mass action. All defaults are 0 (free). The original single interaction is never affected.

* Added "Disable Soul Camera Shake" option to stop the camera shaking when devotion orbs hit the shrine during worship.
* Added "Suppress Notifications On Load" option to prevent the flood of notifications when loading a save.
* Added "Refinery: Poop to Rot Fertilizer" option - refine Poop + Rotgrit into Rot Fertilizer at the refinery.
* Added "Furnace Heater Scaling" option - each proximity heater increases the furnace's fuel drain during winter, adding challenge to heating large bases.

**Changes:**
* Mass pet now only targets dogs, Poppy, and followers with the Pettable trait by default. Enable "Mass Pet All Followers" to pet everyone like before.

**Fixes:**
* Fixed "Collect All God Tears At Once" collecting double the tears when only one was available.
* Fixed mass level up not properly incrementing follower levels (followers were stuck needing a sermon to level up).
* Fixed rot fertilizer decay not stopping crop growth when warming expires — crops now properly freeze in place until re-fertilized.

### 2.4.0 - 04/02/2026

**Improvements:**
* Rewrote the mass action system from scratch for improved reliability and performance.
* Mass actions now skip locked and mutated followers to prevent edge case issues.
* Mass Intimidate now has a config option to control whether the Scared trait can be applied to all followers or just the original target.

**Fixes:**
* Fixed Base Damage Multiplier not applying to the second player (Goat) in co-op mode.

### 2.3.9 - 01/02/2026

**New Features:**
* Added Knucklebones Speed Control - speed up dice rolls and animations (1x to 10x multiplier).
* Added Rotburn as Shrine Fuel - use excess Rotburn to fuel shrine braziers, with shrines providing 20% warmth during winter.
* Added Tarot Luck Multiplier - adjustable slider (1x to 5x) replacing the old fixed 3x toggle.
* Added Animal Lifespan Controls - make farm animals immortal or adjust minimum age before death (default 15 days).
* Added Ritual Cost Multiplier - adjust ritual material costs up or down for custom difficulty.

**Improvements:**
* Renamed "Mass Pet Dog" to "Mass Pet Follower" - now works on all followers, not just dogs.
* Added dedicated Animals config section for better organization.
* Reorganized config sections with clearer formatting and alphabetical ordering.
* Clarified confusing setting names (Animal Old Age Death, Fishing, Ads, Notifications, Run Speed, Propaganda Speakers).

**Fixes:**
* Fixed Magnet Range slider not rounding to 0.25 increments like other sliders.
* Fixed vanilla crash when opening the Priest job board after claiming all fleece quest rewards.
* Fixed "Reverse Enrichment Nerf" giving far fewer coins than expected.
* Fixed "Collect Shrine Devotion Instantly" only collecting ~10% of stored devotion instead of all.

### 2.3.8 - 31/01/2026

* Fixed lore tablets not being awarded when handing in job board quests.
* Added "Auto Repair Missing Lore" option (in Fixes section) to restore lore tablets that weren't unlocked due to the above bug. Only repairs tablets for fleece quests where the reward was actually claimed at the job board.
* Fixed game graphics settings (Chromatic Aberration, Bloom, Vignette, Depth of Field) not applying in DLC zones.
* Added "Mass Open Scarecrows" option - opening one scarecrow trap opens all traps with caught birds.
* Added "Mass Fill Wolf Traps" option - filling one wolf trap with bait fills all empty traps with the same bait type.
* Added "Mass Feed Animals" option - feeding one animal feeds all hungry animals the same food (consumes one item per animal).
* Added "Mass Milk Animals" option - milking one animal milks all animals ready for milking.
* Added "Mass Shear Animals" option - shearing one animal shears all animals ready for shearing.
* Added "Collect Shrine Devotion Instantly" option - collect all devotion from the shrine with a single tap instead of holding.
* Removed redundant "Random Weather Change When Exiting Area" option (now covered by "Weather Change Trigger" setting).
* Reorganized some config settings for better grouping.
* Adjusted speed of mass water and mass fertilize for smoother animations.

### 2.3.7 - 29/01/2026

* Fixed game speed resetting to 1x when changing locations or taking damage.
* Fixed game speed wrapping from minimum (0.25x) to maximum (5x) when decreasing - now stays at minimum.
* Reverted a change that broke mass level up and potentially other mass actions.
* Mass level up now processes all followers simultaneously for faster completion.
* Added "Mass Level Up Instant Souls" option to instantly collect souls during mass level up instead of waiting for them to fly to you (enabled by default).
* Added configurable lightning rod range (basic and upgraded variants).
* Fixed structure range indicators (harvest totem, propaganda speaker, farm station, farm plot sign) unintentionally showing from further away during normal gameplay.

### 2.3.6 - 28/01/2026

* Fixed resource chest sound muting not working for lumberjack and mining station chests.

### 2.3.5 - 27/01/2026

* Fixed player getting stuck when using mass pet on farm animals.
* Fixed player bouncing around during mass level-up.
* Fixed mass romance affecting children followers.
* Fixed refinery auto-fill causing an error.
* Reduced log spam.
* Added option to mute resource chest deposit sounds (when followers deposit).
* Added option to mute resource chest collect sounds (when collecting from chests).

### 2.3.4 - 26/01/2026

* Hopefully fixed mass pet animals causing a crash and leaving the player unable to move.
* Potentially fixed mass water causing errors when UIManager is not fully initialized.
* Fixed interacting with stations while the pause menu is open.
* Improved logging for mass action features to show which feature is being patched.
* Added cooking fire mass fill - clicking a meal fills the queue to maximum.
* Added kitchen mass fill - clicking a meal fills the queue to maximum.
* Added pub mass fill - clicking a drink fills the queue to maximum.

### 2.3.3 - 26/01/2026

* Removed skip intro/video features (moved to dedicated Skip of the Lamb mod).
* Removed negative trait replacement feature (moved to dedicated Trait Control mod).
* Added refinery mass fill - clicking an item fills the queue to maximum.
* Added mass pet animals - petting one farm animal pets all of them.
* Added option to prioritize followers with active requests at the top of selection lists.
* Added "Disable All Notifications" option to suppress all in-game notifications.
* Added "Allow Critical Notifications" sub-option to still show critical notifications (deaths, weapon destruction, dissenters) when all notifications are disabled.
* Weather, scarecrow, fuel, and bed collapse notifications no longer appear when in dungeons or during cutscenes.
* Same notification won't appear again within 10 seconds.
* Reorganized Structures config section to group related settings together.
* Fixed mass collect from beds, shrines, outhouses, compost bins, and harvest totems sometimes stopping early or causing errors.
* Fixed relationship notifications sometimes causing errors when follower skin data is missing.
* Reduced log spam from the game.
* Updated Configuration Manager dependency to 18.4.1.

### 2.3.2 - 25/01/2026

* Added option to auto-select the two followers with the highest mating success chance when opening the Mating Tent.
* Added compatibility check for Skip of the Lamb mod. Skip intro/video features in CultOfQoL are automatically disabled when Skip of the Lamb is installed.
* Fixed crash/hang when starting a new game with "Only Unlocked Traits" enabled (no traits unlocked yet would cause an ArgumentOutOfRangeException).
* Renamed "Remove Level Limit" to "Uncap Level Benefits" - the base game removed the level cap in 1.5.0, but follower benefits (productivity, prayer devotion, sacrifice rewards) are still capped at level 10. This setting removes those caps.
* Removed force-enable of Uncap Level Benefits when enabling Reverse Enrichment Nerf (no longer required since base game allows leveling past 10).
* Added "WAITING" to the log spam filter.

### 2.3.1 - 24/01/2026

* Woolhaven update.
* Fixed crash spam after returning to the main menu.
* Re-introduced mass level-up (previously removed in 2.2.5).
* Fixed all mass actions (bless, bribe, bully, etc.) potentially crashing.
* Fixed bones from cooking spawning in the wrong location when leaving the base area.
* Fixed lumber mine chest giving infinite lumber when looting.
* Fixed farm station radius setting only changing visuals but not actual follower work range.
* Fixed rare tarot cards showing raw text keys instead of descriptions when using 3x luck.
* Mass action fixes now only activate when the corresponding mass action setting is enabled.
* Added option to hide healthy followers from the healing bay selection menu.

### 2.3.0 - 20/09/2025

* Configuration overhaul. Added Reset All Settings button. Default settings are now based on game defaults.
* Customise Harvest Totem radius
* Customise Propaganda Speaker radius
* Customise Farm Station radius
* Customise Farm Sign radius
* Customise Ritual Cooldowns
* Customise Life Expectancy range
* Customise Sin Unlock Boss amount
* Make wood silos produce spider webs (configurable)
* Make stone silos produce crystal shards (configurable)
* Make kitchen produce bones (configurable)
* Make Offering Shrines generate webs, silks, and crystals.

### 2.2.9 - 07/04/2025

* Added option to change the color of the various weather overlays that lighten or darken the screen during certain weather types.

### 2.2.8 - 10/01/2025

* Added option to set keybinds for modifiying game speed
* Pressing the reset game speed to default keybind will now not function if the feature is not enabled

### 2.2.7 - 25/08/2024

* Added requested feature, reversing the nerf to the Ritual of Enrichment. Enabling this will also remove follower level limit as it's required.
* Added a remove vignette option. Despite having it disabled in the game config, there is still vignetting present on the main camera.

### 2.2.6 - 24/08/2024

* Removal of Costura Fody as it's no longer required, and causes delays with updates on Thunderstore.
* Fix for chest loot issues raised in [Nexus Bug Report #7](https://www.nexusmods.com/cultofthelamb/mods/1?tab=bugs)
* If your settings allow, when telling an old follower to work from the menu, a random task will be chosen for them to work on (previously they would say no, too old)

### 2.2.5 - 23/08/2024

* Maintenance update for 1.4.4.592
* Make old followers work!
* Removed Mass Level Up. Might revisit it later.
* Reverted/removed UnityDebuggerAssistant requirement

### 2.2.4 - 23/08/2024

* Implemented hard dependency UnityDebuggerAssistant
* When using the healing bay, exhausted followers will now show the cost next to their name in the selection window.
* Added option to disable move speed increase when in dungeons and/or when in-combat.

### 2.2.3 - 23/08/2024

* Maintenance update for 1.4.4.590

### 2.2.2 - 22/08/2024

* Fix for black screen and resulting chaos on loading a save game [Nexus Bug Report #5](https://www.nexusmods.com/cultofthelamb/mods/1?tab=bugs)
* Adjustments to the lumber station

### 2.2.1 - 22/08/2024

* Fixed healing bay ignoring normal followers that werent exhausted. [Nexus Bug Report #1](https://www.nexusmods.com/cultofthelamb/mods/1?tab=bugs)
* Fixed lumber/mines/farmer station chest Auto Interact/Collect behaviour. [Nexus Bug Report #2](https://www.nexusmods.com/cultofthelamb/mods/1?tab=bugs)
* Potential fix for time stopped [Nexus Bug Report #4](https://www.nexusmods.com/cultofthelamb/mods/1?tab=bugs)

### 2.2.0 - 17/08/2024

* Maintenance update for Unholy Alliance
* There will be bugs.
* In-game settings menu via the API has been disabled. Use ConfigurationManager (default is F1)
* Added setting to turn off the main menu dark mode flash.

### 2.1.9 - 09/02/2024

* Fixed Romance MissingMethodException breaking everything else. This method didn't change in the new update, so no idea what caused this. Recompling against updated game files fixed it.
* Fixed the softlock after performing a sermon, and then choosing the Crown menu with Fast Sermons/Rituals enabled.
* Remove some patches for mass commands that re-enables the animations (lamb roaring, dancing etc); this was un-intended.

### 2.1.8 - 30/01/2024

* Fixed Exhausted followers not being able to be healed in the healing bays.
* Improved associated notifications for the healing bay.
* Speed increase now only applies to Sermons and Rituals.
* All logging (for QoL) has been disabled; if you wish to see it, enabled it in the config file (or via the F1 key)

### 2.1.7 - 28/01/2024

* Fixed lamb not roaring(?) when using Mass Intimidate.
* Removed some unnecessary logging.

### 2.1.6 - 27/01/2024

* Add Loot section; change the magnet distance and if loot magnets to the player or not.
* Added some extra checks to the Mass Follower features to prevent soft locks.
* Added ability to scale the size of the notification banners (independent of the UI scale).
* Added option to increase sermon/ritual/etc speeds. If you happen to get any exceptions that dont result in the player freezing, ignore them.
* Added option to make all tarot cards rare only.
* Added option to remove negative traits, and replace them with a random positive one (or it's direct opposite if available).
  * You need to remove the 'NothingNegative' mod first.
  * The followers current traits will be backed up to a file in the save folder.
* Re-implemented Double Silo Capacity option.
* Added option to make the silo capacities multiples of 32 (fits with the plots).
* Fixed the 'Heat' weather-type being excluded from weather change notifications.

### 2.1.5 - 25/01/2024

* Added Mass Fertilize, Water
* Added Mass Pet Dog, Bribe, Bully, Reassure, Re-educate. These last 3 haven't had extensive testing.
* If any of the Mass settings or the Level All Followers setting is enabled, the camera will not move to the followers who have leveled. This is to prevent the camera from moving all over the place. The lamb will also not hover when a follow levels. The command wheel will also close in most instances when finished. All of the above is intended, please don't raise bug reports about it.
* Fixed some of the settings not actually doing anything.
* Added an alert for when certain settings are toggle and require a game restart to take effect.
* Configuration Manager is now a hard requirement for the mod to launch.

### 2.1.4 - 20/01/2024

* Mass Extort, Bless, Inspire, and Intimidate have been re-implemented. Bribe is still a WIP as it breaks menus (it's disabled for now)
* Added offering shrines, outhouses and composts to the mass collect options.
* Added mass level up (triggered when interacting with a follower who can level).
* Added option to remove level cap.
* Added ability to interrupt the auto-load save by holding down Left Shift while the game is loading. Keybind is configurable.
* Fixed the auto-load save kicking in when you return to the menu from a game.
* Fixed the save to load selection not working as intended. i.e if you had it set to Slot 1, it would actually load Slot 2

- Disabled the API integration for now.

### 2.1.3 - 18/01/2024

* Mostly updated for Sins of the Flesh 
* Mass follower commands are currently disabled due to the large amount of changes made to the game code for them 
* In game settings via the API won't work until it's updated

### 2.1.2 - 02/01/2024

* Fixed menu items having cooked navigation when certain ones are disabled. This only occurred when using a controller/game-pad.
* Semi-future proofed Twitch authentication (Should no longer need a new version for subsequent Twitch drops as long as they stick to their current naming convention). Paid DLC are excluded on purpose.
* Misc code refactoring.

### 2.1.1 - 04/12/2023

* Corrected Damage Multiplier also affecting enemies attacks.

### 2.1.0 - 09/05/2023

* Removed bugfix patch that was causing some users grief.

### 2.0.9 - 07/05/2023

* Added option to auto-load into a specified save game on menu load. Default is slot 1 (slot 0 starts a new game).
* Cleaned up unnecessary logging.

### 2.0.8.1 - 04/05/2023

* Fixed the save and exit menu appearing when deleting a photo in photo mode.

### 2.0.8 - 04/05/2023

* Fixed the UI scale modifications defaulting to on and at such a small value you the UI appears gone.
* Added the missing option to disable save on exit.

### 2.0.7 - 02/05/2023

* Added - Remove New Game button when you have at least one save game.
* Added - Enable Save on exit when quitting. UI is modified to reflect this.
* Added - Quick save keybind to save whenever you want.
* Added - Few developer orientated changes to reduce junk log spam from the game.
* Added - Disable the new ads and developer/publisher logos on the main menu.
* Added - Customizable UI scale.
* Changed - Adjustments to the mass-bless/bribe etc. Still a little wonky, but better than before.
* Changed - Reworked weather mod to work with the new weather system in game.
* Changed - Support for modifying settings in game via the mod menu if have the API installed.

### 2.0.6 - 13/11/2022

* Fixed issue that was causing the New Game button to vanish...
* Added Mass Collect - at the moment it only works with the beds.

### 2.0.5 - 08/11/2022

* Fixed progress issue (inventory doesn't work etc) from skipping crown video.

### 2.0.4 - 07/11/2022

* Added option to skip the receive crown video.
* Fixed skins unlocks not working.

### 2.0.3 - 17/10/2022

* Apologies, fixed infinite lumber/mines being the exact opposite of infinite....

### 2.0.2 - 16/10/2022

* Added ability to modify base dodge, lunge and run speed using a multiplier.
* Added ability to modify base damage dealt using a multiplier.
* Added additional protection against being able to use negative numbers as multipliers.
* When cheesing the fishing mini game, the reel UI is now hidden (serves no purpose).

### 2.0.1 - 16/10/2022 - Config file changes, I suggest starting a fresh config.

* Added Bless, Bribe and Intimidate. Bless still has some issues, but they're minor.
* Added bed collapsing to notifications.
* Moved weather/phase change notifications to Notification config category.
* Added option to enable/disable the dynamic weather changes.
* Fixed lumber/mine aging priority. Previously nothing would happen if the top option was false. Works as intended now.
* Added ability to toggle and use custom values for most mods.

### 2.0.0 - 8/10/2022

* Weather is a little more dynamic. Don't get excited, there ain't any snow.
* Four config options for weather. Low range and high range for both rain and wind. Low range = light rain/wind, high range = heavy rain/wind. When the game changes weather, it basically rolls 0-100. Default config is:
- Low range rain = 0-15, meaning the roll has to fall between those numbers for it to rain lightly.
- High range rain = 85-100, meaning the roll has to fall between those numbers for it to rain heavily.
- Low range wind = 0-25, meaning the roll has to fall between those numbers for it to be light winds.
- High range wind = 75-100, meaning the roll has to fall between those numbers for it to be high winds.
* Notifications (on/off) for when the days phase changes, i.e. morning, noon, evening, night, etc.
* Notifications (on/off) for when the weather changes, i.e. light rain, heavy rain, wind, etc.
* Hopefully fixed a bug where buildings could vanish from the map when uing propaganda speaker mods.
* By default, the game changes weather when you exit a building or start a new day. You can now set it to change weather when the time of day changes, i.e. morning, noon, evening, night, etc.

### 1.9.1 - 2/10/2022

* Updated for game version 1.0.17.
* Unlocked Twitch skins will no longer be removed if you disable the option to unlock them.
* Another attempt at fixing propoganda speakers being turned off during the day for a whole phase.

### 1.9.0 - 19/09/2022

* Updated for game version 1.0.16. They removed the Debug button from the main menu, so I've removed the code that removes it.
* When the propaganda speakers turn off of at night, their fire animation will now also turn off.
* Implemented a potential fix for speakers not turning back on.
* Added ability to replaced follower necklaces. The one they're wearing will drop to the ground.
* Added ability to receive notifications when items that use fuel run out.
* Followers who can level after Inspire All should now level. Will be added to others when I implement them.

### 1.8.1 - 13/09/2022

* Fixed the follower menu breaking in some instances with the healing bay mod on.

### 1.8.0 - 11/09/2022 - Some options have moved categories, I suggest a clean config file.

* Lumberjack stations now have their loot speed delay lowered when collecting from the chest. Its not totally 0 (its 0.01), because it makes the animations look like junk.
* Added option for chests to enable them to automatically give you the loot when you're nearby. 
* Added option to double the distance required before the loot is sent your way.
* Added option to set a trigger limit before loot is automatically retrieved.
* Added the ability to "heal" exhausted followers in the healing bays.
* Added option to receive a notification when one of the scarecrows catches a bird.
* Fixed the passive and main shrines from not having their capacity doubled.

### 1.7.0 - 10/09/2022

* Added option to multiply your tarot card draw luck by 3. This doesnt mean every draw will be a rare+, you just have a higher chance of drawing one (goes from around 20% to 60%).
* Added option to disable the game over mechanic (user request).
* Added option to increase lifespan of lumber/mines by 50%, as 100% felt way too long.
* After being inspired with inspire all, any remaining glowy eyed followers should now level as well.

### 1.6.0 - 05/09/2022

* Added 0 to the speed choices, halts everything and becomes 2d photo mode effectively.
* Added ability to extend the day length.
* Added the lighthouse "shrine" and the lonely shack shrine to the fast collect structures.
* Added checks to ensure we're not inspiring/extorting when sleeping/dissenting/bathroom/prison.
* Inspire All "should" be fixed now.
* Added option to double lumber/mine stations age instead of only lasting forever.
* Removed anything outhouse related as they're being put into a separate outhouse specific mod.
* WIP - Option to double the storage capacity of shrines (not quite done, the main one ignores it for some reason).

### 1.5.0 - 02/09/2022

* Collection delays from the main shrine (credits to Matthew-X), the smaller shrines, the chest near the portal, the outhouses and the beds have been reduced to zero, otherwise shortened (otherwise animations look like junk).
* The seed and fertilizer silos can be expanded to 32 slots, which is the number of plots you can build around them. (credits to Matthew-X).
* Further adjustments to Inspire All - I have not found a consistent way to reproduce the issue. If it occurs (stand there dancing), save, exit to the menu and go back in.

### 1.4.0

* Made some adjustments to the outhouse stuff, see how it goes. Still a WIP.
* Added game speed manipulation. Left/Right arrows to increase/decrease in 0.25 increments. Up arrow resets it to default. Maximum (artifical cap) is 5, and good luck to you playing it at that speed.
* Improved the elderly extortion patch. Should now grey out when they've already been extorted, and only appear once you've unlocked that doctrine.

### 1.3.1

* Fixed the Twitch patches causing the game to lock you onto the portal platform, thingy.
* Modified golden fleece patch to be inline with changes from 1.0.13.

### 1.3

* Added ability to unlock Twitch items (will unlock on game load)
* Added being able to collect tithe from the oldies. Brutal.
* Added lumber/stone mine immortality.
* Added being able to extort the elderly. Brutal.

### 1.2

* Inspire all is somewhat fixed. Instead of being stuck sometimes, they'll move on to their next task.
* Added option to cure illness/exhaustion when a follower gains loyalty (the floaty white eyes thing).
* Fixed refinery config not being there.
* More

### 1.1

* Collect tithes/inspire all at once. Inspire has some quirks I'm working on.
* Reverse the 200% golden fleece cap.
* Double the rate the damage increases with the golden fleece.
* Halves (where possible) the cost of refining goods. Gets rounded up.

### 1.0 - Initial release

* Remove intros
* Cheese fishing min-game
* Remove button clutter (Discord, Bugs, Twitch etc..)