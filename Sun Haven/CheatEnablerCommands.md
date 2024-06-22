# Cheat Enabler Commands User Manual

## Commands

### `abandonquest`
**Description:** Abandon a specified quest.This command will abandon the quest and remove it from the player's quest log. Use `/printcompletedquests` to see a list of completed quests.
**Usage:** `/abandonquest questName`

### `adddevitems`
**Description:** Adds a full set of golden tools to the player's inventory.  
**Usage:** `/adddevitems`

### `addexp`
**Description:** Add experience points to a specified profession.
**Valid Professions:** Use `/printallprofessions` to see a list of valid professions.
**Usage:** `/addexp professionName amount`

### `additem`
**Description:** Add a specified item to the player's inventory by item name. Use `/finditemid` to find an items ID and name. i.e. `/finditemid albino` will return all items with the word albino in them.
**Usage:** `/additem itemName [amount]`

### `addmoney`
**Description:** Add money to the player's inventory. The source of the money is recorded internally as `Exploration` 
**Usage:** `/addmoney amount`

### `addpermenentstatbonus`
**Description:** Add a permanent bonus to a player's stat.  
**Valid Stats:** Use `/printallstats` to see a list of valid stats.
**Usage:** `/addpermenentstatbonus statName amount`

### `addrangeofitems`
**Description:** Add a range of items to the player's inventory starting at the specified item name. For example if the item code of the entered item is `15324` and the range is 5, the player will receive items `15324`, `15325`, `15326`, `15327`, `15328`.
**Usage:** `/addrangeofitems itemName range [amount]`

### `addstat`
**Description:** Add a specified amount to a player's stat temporarily.  
**Valid Stats:** Use `/printallstats` to see a list of valid stats.
**Usage:** `/addstat statName amount`

### `addtime`
**Description:** Add a specified amount of time to the current day.  
**Usage:** `/addtime hours`

### `brinestonedeepssaved`
**Description:** Set the 'Brinestone Deeps Saved' status.  
**Usage:** `/brinestonedeepssaved true|false`

### `despawnpet`
**Description:** Despawn the current pet.  
**Usage:** `/despawnpet`

### `divorceNPC`
**Description:** Divorce the currently married NPC.  
**Notes:* The `npcName` parameter is currently not used by the code that executes this command.
**Usage:** `/divorceNPC npcName`

### `enabledaycycle`
**Description:** Enable or disable the day cycle.  
**Usage:** `/enabledaycycle true|false`

### `FixPets`
**Description:** Fix the pets. This command will bring all pets to the player's location.
**Usage:** `/FixPets`

### `getrelationships`
**Description:** Get all relationships in the game.  
**Usage:** `/getrelationships`

### `getstat`
**Description:** Get the current value of a specified stat.  
**Valid Stats:** Use `/printallstats` to see a list of valid stats.
**Usage:** `/getstat statName`

### `godmode`
**Description:** Enable or disable god mode. Sets noClip (move through walls etc.) to true, and increases move speed and jump height.
**Usage:** `/godmode true|false`

### `hidePlayer`
**Description:** Hide the player.  
**Usage:** `/hidePlayer`

### `lockmines`
**Description:** Lock all mines in the game.  
**Usage:** `/lockmines`

### `marryNPC`
**Description:** Marry a specified NPC.  
**Valid NPCs:** Use `/printromancenpcs` to see a list of valid NPCs.
**Usage:** `/marryNPC npcName`

### `noclip`
**Description:** Enable or disable noClip (move through walls etc.) mode.  
**Usage:** `/noclip true|false`

### `pausetime`
**Description:** Pause the progression of time in the game.  
**Usage:** `/pausetime`

### `resetalldecoration`
**Description:** Reset all decorations in the game.  
**Usage:** `/resetalldecoration`

### `resetanimals`
**Description:** Reset all animals.  
**Usage:** `/resetanimals`

### `resetcharacterprogress`
**Description:** Reset the progress for a specified character.  
**Usage:** `/resetcharacterprogress progressName`

### `resetfoodstats`
**Description:** Reset the player's food stats. Removes all stats gained from food.
**Usage:** `/resetfoodstats`

### `resetfarminginfo`
**Description:** Reset all farming information. This will destroy all farm tiles in the active scene.
**Usage:** `/resetfarminginfo`

### `resethelpnotifications`
**Description:** Reset all help notifications.
**Note:** The `mailName` parameter is currently not used by the code that executes this command.
**Usage:** `/resethelpnotifications mailName`

### `resetinventory`
**Description:** Reset the player's inventory.  
**Usage:** `/resetinventory`

### `resetmail`
**Description:** Reset all mails.  
**Usage:** `/resetmail`

### `resetmoney`
**Description:** Reset the player's money to zero.  
**Usage:** `/resetmoney`

### `resetpermanentstatbonuses`
**Description:** Reset all permanent stat bonuses for the current character.  
**Usage:** `/resetpermanentstatbonuses`

### `resetprogress`
**Description:** Reset the game's progress.  
**Usage:** `/resetprogress`

### `resetquests`
**Description:** Reset **all** quests.  
**Usage:** `/resetquests`

### `resetrelationships`
**Description:** Reset **all** relationships in the game.  
**Usage:** `/resetrelationships`

### `resetskills`
**Description:** Reset **all** skills.  
**Usage:** `/resetskills`

### `resetworldprogress`
**Description:** Reset the progress for the game world.  
**Usage:** `/resetworldprogress progressName`

### `sendmail`
**Description:** Send a mail to the player.  
**Valid Mail:** Use `/printallmail` to see a list of valid mail ID's.
**Usage:** `/sendmail mailID`

### `setarenaboss`
**Description:** Set the arena boss by name.  
**Usage:** `/setarenaboss bossName`

### `setbirthday`
**Description:** Set the player's birthday.  
**Usage:** `/setbirthday season day`

### `setbulletinboardquest`
**Description:** Set a specified bulletin board quest. 
**Valid Quests:** Use `/printallbbquests` to see a list of valid quests.
**Usage:** `/setbulletinboardquest questName`

### `setcharacterprogress`
**Description:** Set the progress for a specified character.  
**Usage:** `/setcharacterprogress progressName`

### `setclothinggloves`
**Description:** Set the player's clothing gloves.  
**Usage:** `/setclothinggloves glovesID`

### `setday`
**Description:** Set the current day in the game.  
**Usage:** `/setday dayNumber`

### `setdaya`
**Description:** Set the current day to type A.  
**Usage:** `/setdaya`

### `setdayb`
**Description:** Set the current day to type B.  
**Usage:** `/setdayb`

### `setdayfoggy`
**Description:** Set the current day to foggy.  
**Usage:** `/setdayfoggy`

### `setdaygloomyrain`
**Description:** Set the current day to gloomy rain.  
**Usage:** `/setdaygloomyrain`

### `setdaylightsnow`
**Description:** Set the current day to light snow.  
**Usage:** `/setdaylightsnow`

### `setdayspeed`
**Description:** Set the speed of the day cycle.  
**Valid Speeds:** `0`, `1`, `10`, `100`, `1000`.
**Usage:** `/setdayspeed speed`

### `setdayrain`
**Description:** Set the current day to rain.  
**Usage:** `/setdayrain`

### `setdayseasonalparticle`
**Description:** Set the current day to seasonal particle effect.  
**Usage:** `/setdayseasonalparticle`

### `setdaywindy`
**Description:** Set the current day to windy.  
**Usage:** `/setdaywindy`

### `setexp`
**Description:** Set the experience points for a specified profession.  
**Valid Professions:** Use `/printallprofessions` to see a list of valid professions.
**Usage:** `/setexp professionName amount`

### `sethealth`
**Description:** Set the player's health.  
**Usage:** `/sethealth amount`

### `setmana`
**Description:** Set the player's mana.  
**Usage:** `/setmana amount`

### `setmaxfoodstats`
**Description:** Set the maximum food stats for the player.  
**Usage:** `/setmaxfoodstats amount`

### `setnpcquest`
**Description:** Set a specified NPC quest. The quest is generated randomly. 
**Valid NPCs:** Use `/printallnpcs` to see a list of valid npcs.
**Usage:** `/setnpcquest questName`

### `setpreplaceddecorations`
**Description:** Set pre-placed decorations.  
**Usage:** `/setpreplaceddecorations`

### `setrelationship`
**Description:** Set the relationship level with a specified NPC.  
**Valid NPCs:** Use `/printallnpcs` to see a list of valid npcs.
**Usage:** `/setrelationship npcName amount`

### `setseason`
**Description:** Set the current season in the game.  
**Valid Seasons:** Use `/printallseasons` to see a list of valid seasons.
**Usage:** `/setseason seasonName`

### `setstat`
**Description:** Set a player's stat to a specific amount. 
**Valid Stats:** Use `/printallstats` to see a list of valid stats.
**Usage:** `/setstat statName amount`

### `settime`
**Description:** Set the current time to a specific hour.  
**Usage:** `/settime hour`

### `setuiactive`
**Description:** Set the UI active or inactive.  
**Usage:** `/setuiactive true|false`

### `setuiactivebutactionbar`
**Description:** Set the UI active or inactive except for the action bar.  
**Usage:** `/setuiactivebutactionbar true|false`

### `setworldprogress`
**Description:** Set the progress for the game world.  
**Usage:** `/setworldprogress progressName`

### `setzoom`
**Description:** Set the camera zoom level. 
**Valid Levels:** `1`, `2`, `3`, `4`.
**Usage:** `/setzoom zoomLevel`

### `skipday`
**Description:** Skip one day in the game.  
**Usage:** `/skipday`

### `skipday`
**Description:** Skip a specified number of days in the game.  
**Usage:** `/skipday days`

### `skipintro`
**Description:** Skip the game's intro sequence (not the splash screens)  
**Usage:** `/skipintro`

### `skiptonpccycle`
**Description:** Skip to a specified NPC cycle.  
**Valid NPCs:** Use `/printallnpcs` to see a list of valid npcs.
**Usage:** `/skiptonpccycle npcName cycleNumber`

### `skiptoworldepilogue`
**Description:** Skip to a specified world epilogue. Will trigger the specified cutscenes and quests.
**Valid Breakpoints:** `0`,`1`,`2`,`3`,`4`
**Affected Quests/Progress:** Use `/getworldepilogueskipbreakpoints` i.e. `/getworldepilogueskipbreakpoints 3`
**Usage:** `/skiptoworldepilogue breakpoint`

### `skiptoworldquest`
**Description:** Skip to a specified world quest.  
**Valid Breakpoints:** `1`, `2`, `3`, `4`, `5`, `6`, `7`, `8`
**Affected Quests/Progress:** Use `/getworldquestskipbreakpoints` i.e. `/getworldquestskipbreakpoints 3`
**Usage:** `/skiptoworldquest breakpoint`

### `skiptoworldquestnelvari`
**Description:** Skip to a specified world quest in Nelvari.  
**Valid Breakpoints:** `0`, `1`, `2`, `3`, `4`, `5`, `6`, `7`, `8`
**Affected Quests/Progress:** Use `/getworldquestskipnelvaribreakpoints` i.e. `/getworldquestskipnelvaribreakpoints 6`
**Usage:** `/skiptoworldquestnelvari breakpoint`

### `skiptoworldquestwithergate`
**Description:** Skip to a specified world quest in Withergate.  
**Valid Breakpoints:** `1`, `6`, `7`, `8`, `9`, `10`, `11`, `12`, `13`, `14`, `15`, `16`, `17`, `18`, `19`
**Affected Quests/Progress:** Use `/getworldquestskipwithergatebreakpoints` i.e. `/getworldquestskipwithergatebreakpoints 7`
**Usage:** `/skiptoworldquestwithergate breakpoint`

### `spawnpet`
**Description:** Spawn a pet.  
**Usage:** `/spawnpet petName`

### `startquest`
**Description:** Start a specified quest.  
**Valid Quests:** Use `/printallquests` to see a list of valid quests.
**Usage:** `/startquest questName`

### `teleport`
**Description:** Teleport the player to a specified scene.
**Valid Scenes:** Use `/printallscenes` to see a list of valid scenes.
**Usage:** `/teleport sceneName`

### `TestPlaceAllDecorations`
**Description:** Test place all decorations.  
**Usage:** `/TestPlaceAllDecorations`

### `unlockmines`
**Description:** Unlock all mines in the game.  
**Usage:** `/unlockmines`

### `unpausetime`
**Description:** Resume the progression of time in the game.  
**Usage:** `/unpausetime`