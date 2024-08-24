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