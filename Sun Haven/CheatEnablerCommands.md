# Cheat Enabler Commands User Manual

## Tip

Generate a manual of sorts using the `man` command. For example, to get information on the `additem` command you would enter `/man additem`

### `man`
**Description:** Display the manual for a specified command<br>
**Usage:** `/man command`

## Custom Commands
These are commands I've added to be used in conjunction with the built-in commands.

### `additembyid`
**Description:** Add a specified item to the player's inventory by item ID. Use `/finditemid` to find an items ID and name. i.e. `/finditemid albino` will return all items with the word albino in them.<br>
**Usage:** `/additembyid itemID [amount]`

### `savegame`
**Description:** Save the game.<br>
**Usage:** `/savegame`

### `getworldquestskipwithergatebreakpoints`<br>
**Description:** Get the breakpoints for the `skiptoworldquestwithergate` command.<br>
**Usage:** `/getworldquestskipwithergatebreakpoints`<br>

### `getworldquestskipnelvaribreakpoints`<br>
**Description:** Get the breakpoints for the `skiptoworldquestnelvari` command.<br>
**Usage:** `/getworldquestskipnelvaribreakpoints`<br>

### `getworldquestskipbreakpoints`<br>
**Description:** Get the breakpoints for the `skiptoworldquest` command.<br>
**Usage:** `/getworldquestskipbreakpoints`<br>

### `getworldquestskipepiloguebreakpoints`<br>
**Description:** Get the breakpoints for the `skiptoworldepilogue` command.<br>
**Usage:** `/getworldquestskipepiloguebreakpoints`<br>

### `printallbbquests`<br>
**Description:** Print all bulletin board quests.<br>
**Usage:** `/printallbbquests`<br>

### `printallmail`<br>
**Description:** Print all mail.<br>
**Usage:** `/printallmail`<br>

### `printallnpcs`<br>
**Description:** Print all NPCs.<br>
**Usage:** `/printallnpcs`<br>

### `printallprofessions`<br>
**Description:** Print all professions.<br>
**Usage:** `/printallprofessions`<br>

### `printallquests`<br>
**Description:** Print all quests.<br>
**Usage:** `/printallquests`<br>

### `printallscenes`<br>
**Description:** Print all scenes.<br>
**Usage:** `/printallscenes`<br>

### `printallseasons`<br>
**Description:** Print all seasons.<br>
**Usage:** `/printallseasons`<br>

### `printallstats`<br>
**Description:** Print all stats.<br>
**Usage:** `/printallstats`<br>

### `printcompletedquests`<br>
**Description:** Print all completed quests.<br>
**Usage:** `/printcompletedquests`

### `printromancenpcs`<br>
**Description:** Print all romance NPCs.<br>
**Usage:** `/printromancenpcs`<br>

## Built-in Commands

### `abandonquest`<br>
**Description:** Abandon a specified quest.This command will abandon the quest and remove it from the player's quest log. Use `/printcompletedquests` to see a list of completed quests.<br>
**Usage:** `/abandonquest questName`<br>

### `adddevitems`<br>
**Description:** Adds a full set of golden tools to the player's inventory.  <br>
**Usage:** `/adddevitems`<br>

### `addexp`
**Description:** Add experience points to a specified profession.<br>
**Valid Professions:** Use `/printallprofessions` to see a list of valid professions.<br>
**Usage:** `/addexp professionName amount`<br>

### `additem`<br>
**Description:** Add a specified item to the player's inventory by item name. Use `/finditemid` to find an items ID and name. i.e. `/finditemid albino` will return all items with the word albino in them.<br>
**Usage:** `/additem itemName [amount]`<br>

### `addmoney`<br>
**Description:** Add money to the player's inventory. The source of the money is recorded internally as `Exploration` <br>
**Usage:** `/addmoney amount`<br>

### `addpermenentstatbonus`<br>
**Description:** Add a permanent bonus to a player's stat.  <br>
**Valid Stats:** Use `/printallstats` to see a list of valid stats.<br>
**Usage:** `/addpermenentstatbonus statName amount`<br>

### `addrangeofitems`<br>
**Description:** Add a range of items to the player's inventory starting at the specified item name. For example if the item code of the entered item is `15324` and the range is 5, the player will receive items `15324`, `15325`, `15326`, `15327`, `15328`.<br>
**Usage:** `/addrangeofitems itemName range [amount]`<br>

### `addstat`<br>
**Description:** Add a specified amount to a player's stat temporarily.  <br>
**Valid Stats:** Use `/printallstats` to see a list of valid stats.<br>
**Usage:** `/addstat statName amount`<br>

### `addtime`<br>
**Description:** Add a specified amount of time to the current day.  <br>
**Usage:** `/addtime hours`<br>

### `brinestonedeepssaved`<br>
**Description:** Set the 'Brinestone Deeps Saved' status.  <br>
**Usage:** `/brinestonedeepssaved true|false`<br>

### `despawnpet`<br>
**Description:** Despawn the current pet.  <br>
**Usage:** `/despawnpet`<br>

### `divorceNPC`<br>
**Description:** Divorce the currently married NPC.  <br>
**Notes:* The `npcName` parameter is currently not used by the code that executes this command.<br>
**Usage:** `/divorceNPC npcName`<br>

### `enabledaycycle`<br>
**Description:** Enable or disable the day cycle.  <br>
**Usage:** `/enabledaycycle true|false`<br>

### `FixPets`<br>
**Description:** Fix the pets. This command will bring all pets to the player's location.<br>
**Usage:** `/FixPets`<br>

### `getrelationships`<br>
**Description:** Get all relationships in the game.  <br>
**Usage:** `/getrelationships`<br>

### `getstat`
**Description:** Get the current value of a specified stat.  <br>
**Valid Stats:** Use `/printallstats` to see a list of valid stats.<br>
**Usage:** `/getstat statName`<br>

### `godmode`<br>
**Description:** Enable or disable god mode. Sets noClip (move through walls etc.) to true, and increases move speed and jump height.<br>
**Usage:** `/godmode true|false`<br>

### `hidePlayer`<br>
**Description:** Hide the player.  <br>
**Usage:** `/hidePlayer`<br>

### `lockmines`<br>
**Description:** Lock all mines in the game.  <br>
**Usage:** `/lockmines`<br>

### `marryNPC`
**Description:** Marry a specified NPC.  <br>
**Valid NPCs:** Use `/printromancenpcs` to see a list of valid NPCs.<br>
**Usage:** `/marryNPC npcName`<br>

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
**Usage:** `/unpausetime`<br>
