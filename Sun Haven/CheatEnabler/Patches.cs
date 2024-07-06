namespace CheatEnabler;

[Harmony]
public class Patches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(PlayerSettings), nameof(PlayerSettings.SetCheatsEnabled))]
    public static void PlayerSettings_SetCheatsEnabled(ref bool enable)
    {
        if (Plugin.Debug.Value)
        {
            Plugin.LOG.LogInfo("Override PlayerSettings.SetCheatsEnabled() to TRUE");
        }
        enable = true;
    }



    [HarmonyPostfix]
    [HarmonyPatch(typeof(Player), nameof(Player.InitializeAsOwner))]
    [HarmonyPatch(typeof(Player), nameof(Player.InitializeAsClient))]
    public static void Player_InitializeAsOwner()
    {
        if (PlayerPrefs.GetInt("CheatEnabler") == 1) return;

        var lang = LocalizationManager.CurrentLanguageCode;
        var title = Lang.DlcMessageTitleTranslations.TryGetValue(lang, out var titleTranslation) ? titleTranslation : "Cheat Enabler";
        var message = Lang.DlcMessageTranslations.TryGetValue(lang, out var messageTranslation) ? messageTranslation : "Cheat Enabler will not allow you to add/spawn items that are marked as DLC items regardless if you own the DLC or not. Please do not raise bug reports regarding this.";
        var response = Lang.DlcOkTranslations.TryGetValue(lang, out var responseTranslation) ? responseTranslation : "I Understand";
        DialogueController.Instance.SetDialogueBustVisualsOptimized("Lynn", small: true, isSwimsuitBust: true);
        DialogueController.Instance.PushDialogue(new DialogueNode
        {
            dialogueText = [message],
            responses = new Dictionary<int, Response>()
            {
                [0] = new()
                {
                    responseText = () => response
                }
            }
        }, delegate
        {
            PlayerPrefs.SetInt("CheatEnabler", 1);
        });
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(QuantumConsoleManager), nameof(QuantumConsoleManager.abandonquest))]
    public static bool QuantumConsoleManager_abandonquest(string questname)
    {
        var questDictionary = SingletonBehaviour<QuestManager>.Instance.questDictionary;
        var caseSensitiveKey = questDictionary.GetCaseSensitiveKey(questname);
        if (caseSensitiveKey.IsNullOrWhiteSpace())
        {
            Utils.LogToPlayer($"The quest '{questname}' does not exist. Please use /printquestlog to get valid quest names.");
            return false; //don't let the original method run
        }

        return true; //let the original method run
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(QuantumConsoleManager), nameof(QuantumConsoleManager.addpermenentstatbonus))]
    public static bool QuantumConsoleManager_addpermenentstatbonus(string stat)
    {
        var validEnum = Enum.TryParse(stat, true, out StatType _);
        if (validEnum) return true; //let the original method run

        Utils.LogToPlayer($"The stat '{stat}' does not exist. Use /printallstats to see a list of valid stats.");
        return false; //don't let the original method run
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(QuantumConsoleManager), nameof(QuantumConsoleManager.addexp))]
    public static bool QuantumConsoleManager_addexp(string profession)
    {
        var validEnum = Enum.TryParse(profession, true, out ProfessionType _);
        if (validEnum) return true; //let the original method run

        Utils.LogToPlayer($"The profession '{profession}' does not exist. Use /printallprofessions to see a list of valid professions.");

        return false; //don't let the original method run
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(QuantumConsoleManager), nameof(QuantumConsoleManager.addrangeofitems))]
    public static bool QuantumConsoleManager_addrangeofitems(string item, int range, int amount = 1)
    {
        var id = Database.GetID(item);
        if (id <= 0)
        {
            Utils.LogToPlayer($"Item '{item}' not found. Please use /finditemid to get valid item names.");
            return false; //don't let the original method run
        }

        var dlcItemFound = false;
        for (var i = 0; i < range; i++)
        {
            var innerId = Database.GetID(item);
            if (innerId <= 0)
            {
                Utils.LogToPlayer($"Item with ID '{i}' not found. Please use /finditemid to get valid item names.");
                return false; //don't let the original method run
            }

            Database.GetData(innerId, delegate(ItemData data)
            {
                if (!data.isDLCItem) return;
                dlcItemFound = true;
                Utils.LogToPlayer($"Unfortunately, {data.FormattedName} ({data.ID}) is a DLC item and cannot be added via Cheat Enabler.");
            });
            if (dlcItemFound) return false; //don't let the original method run
        }
        return true; //let the original method run
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(QuantumConsoleManager), nameof(QuantumConsoleManager.setzoom))]
    public static bool QuantumConsoleManager_setzoom(int zoomLevel)
    {
        if (zoomLevel is 1 or 2 or 3 or 4) return true; //let the original method run

        Utils.LogToPlayer($"The zoom level '{zoomLevel}' is not a valid zoom level. Please use 1, 2, 3, or 4.");
        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(QuantumConsoleManager), nameof(QuantumConsoleManager.skiptoworldquest))]
    public static bool QuantumConsoleManager_skiptoworldquest(int breakpoint)
    {
        if (breakpoint is 1 or 2 or 3 or 4 or 5 or 6 or 7 or 8) return true; //let the original method run

        Utils.LogToPlayer($"The breakpoint '{breakpoint}' is not a valid breakpoint. Please use 1, 2, 3, 4, 5, 6, 7, 8. Use /getworldquestskipbreakpoints to see a list of skipped quests for each breakpoint.");
        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(QuantumConsoleManager), nameof(QuantumConsoleManager.skiptoworldquestnelvari))]
    public static bool QuantumConsoleManager_skiptoworldquestnelvari(int breakpoint)
    {
        if (breakpoint is 0 or 1 or 2 or 3 or 4 or 5 or 6 or 7 or 8) return true; //let the original method run

        Utils.LogToPlayer($"The breakpoint '{breakpoint}' is not a valid breakpoint. Please use 0, 1, 2, 3, 4, 5, 6, 7, 8. Use /getworldquestskipnelvaribreakpoints to see a list of skipped quests for each breakpoint.");
        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(QuantumConsoleManager), nameof(QuantumConsoleManager.skiptoworldquestwithergate))]
    public static bool QuantumConsoleManager_skiptoworldquestwithergate(int breakpoint)
    {
        if (breakpoint is 1 or 6 or 7 or 8 or 9 or 10 or 11 or 12 or 13 or 14 or 15 or 16 or 17 or 18 or 19) return true; //let the original method run

        Utils.LogToPlayer($"The breakpoint '{breakpoint}' is not a valid breakpoint. Please use 1, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19. Use /getworldquestskipwithergatebreakpoints to see a list of skipped quests for each breakpoint.");
        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(QuantumConsoleManager), nameof(QuantumConsoleManager.teleport))]
    public static bool QuantumConsoleManager_teleport(ref string sceneName)
    {
        var sceneDict = SingletonBehaviour<SceneSettingsManager>.Instance.sceneNameDictionary;
        var caseSensitiveKey = sceneDict.GetCaseSensitiveKey(sceneName);

        if (caseSensitiveKey.IsNullOrWhiteSpace())
        {
            Utils.LogToPlayer($"Scene '{sceneName}' not found. Please use /printallscenes to get valid scene names.");
            return false; //don't let the original method run
        }

        sceneName = caseSensitiveKey;
        return true; //let the original method run
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(QuantumConsoleManager), nameof(QuantumConsoleManager.startquest))]
    public static bool QuantumConsoleManager_startquest(ref string questname)
    {
        var questDictionary = SingletonBehaviour<QuestManager>.Instance.questDictionary;
        var caseSensitiveKey = questDictionary.GetCaseSensitiveKey(questname);
        if (caseSensitiveKey.IsNullOrWhiteSpace())
        {
            Utils.LogToPlayer($"The quest '{questname}' does not exist. Please use /printallbbquests to get valid quest names.");
            return false; //don't let the original method run
        }

        questname = caseSensitiveKey;
        return true; //let the original method run
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(QuantumConsoleManager), nameof(QuantumConsoleManager.spawnpet))]
    public static bool QuantumConsoleManager_spawnpet(ref string petname)
    {
        var petID = Database.GetID(petname);
        if (petID <= 0)
        {
            Utils.LogToPlayer($"Pet '{petname}' not found. Please use /finditemid to get valid pet names.");
            return false;
        }

        Database.GetData(petID, delegate(PetData data)
        {
            if (!data.isDLCItem)
            {
                SingletonBehaviour<PetManager>.Instance.SpawnPet(Player.Instance, new PetItem(data.id));
                return;
            }
            Utils.LogToPlayer($"Unfortunately, {data.FormattedName} ({data.ID}) is a DLC item and cannot be spawned via Cheat Enabler.");
        });
        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(QuantumConsoleManager), nameof(QuantumConsoleManager.setstat))]
    public static bool QuantumConsoleManager_setstat(string stat)
    {
        var validEnum = Enum.TryParse(stat, true, out StatType _);
        if (validEnum) return true; //let the original method run

        Utils.LogToPlayer($"The stat '{stat}' does not exist. Use /printallstats to see a list of valid stats.");
        return false; //don't let the original method run
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(QuantumConsoleManager), nameof(QuantumConsoleManager.setseason))]
    public static bool QuantumConsoleManager_setseason(ref string season)
    {
        var validEnum = Enum.TryParse(season, true, out Season _);
        if (validEnum) return true; //let the original method run

        Utils.LogToPlayer($"The season '{season}' does not exist. Use /printallseasons to see a list of valid seasons.");
        return false; //don't let the original method run
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(QuantumConsoleManager), nameof(QuantumConsoleManager.setnpcquest))]
    public static bool QuantumConsoleManager_setnpcquest(ref string npcName)
    {
        var npcs = SingletonBehaviour<NPCManager>.Instance._npcs;
        //check if the npc exists in the case-insensitive dictionary
        var caseSensitiveKey = npcs.GetCaseSensitiveKey(npcName);
        if (caseSensitiveKey.IsNullOrWhiteSpace())
        {
            Utils.LogToPlayer($"NPC '{npcName}' not found. Please use /printallnpcs to get valid NPC names.");
            return false; //don't let the original method run
        }

        npcName = caseSensitiveKey;

        return true; //let the original method run
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(QuantumConsoleManager), nameof(QuantumConsoleManager.skiptonpccycle))]
    [HarmonyPatch(typeof(QuantumConsoleManager), nameof(QuantumConsoleManager.divorceNPC))]
    [HarmonyPatch(typeof(QuantumConsoleManager), nameof(QuantumConsoleManager.marryNPC))]
    [HarmonyPatch(typeof(QuantumConsoleManager), nameof(QuantumConsoleManager.setrelationship))]
    public static bool QuantumConsoleManager_npcStuff(ref string npc)
    {
        var npcs = SingletonBehaviour<NPCManager>.Instance._npcs;

        var caseSensitiveKey = npcs.GetCaseSensitiveKey(npc);

        if (caseSensitiveKey.IsNullOrWhiteSpace())
        {
            Utils.LogToPlayer($"NPC '{npc}' not found. Please use /printromancenpcs or /printallnpcs to get valid NPC names.");
            return false; //don't let the original method run
        }

        npc = caseSensitiveKey;

        return true; //let the original method run
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(QuantumConsoleManager), nameof(QuantumConsoleManager.sendmail))]
    public static bool QuantumConsoleManager_sendmail(ref string mailName)
    {
        if (!Application.isEditor && mailName is "CuteMountDLCMail" or "CutePetDLCMail" or "SpookyMountDLCMail" or "SpookyPetDLCMail" or "WithergateMaskMail1" or "SparklingSundressandArmorMail" or "BabyDragonMail" or "BabyTigerMail" or "GoldenRecordAndFiveCustomRecordsMail" or "OceanShoreDLCMail" or "MushyDLCMail" or "SeasideDLCMail" or "TrickOrTreatDLCMail" or "SpiritPetalDLCMail" or "RockNRollDLCMail" or "CyberpopDLCMail" or "FunkyMonkeyDLCMail" or "DreamyRamDLCMail" or "PopSensationDLCMail" or "StarlightDLCMail" or "ToyDLCMail" or "SnowDayDLCMail" or "TisTheSeasonDLCMail" or "SugarRushDLCMail")
        {
            Utils.LogToPlayer($"The mail '{mailName}' is related to DLC content and cannot be added via Cheat Enabler.");
            return false;
        }

        var mail = SingletonBehaviour<MailManager>.Instance.mailDictionary;

        var caseSensitiveKey = mail.GetCaseSensitiveKey(mailName);
        if (caseSensitiveKey.IsNullOrWhiteSpace())
        {
            Utils.LogToPlayer($"Mail '{mailName}' not found. Please use /printallmail to get valid mail names.");
            return false; //don't let the original method run
        }

        mailName = caseSensitiveKey;
        return true;
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(QuantumConsoleManager), nameof(QuantumConsoleManager.getstat))]
    public static bool QuantumConsoleManager_getstat(string stat)
    {
        var validEnum = Enum.TryParse(stat, true, out StatType _);
        if (validEnum) return true; //let the original method run

        Utils.LogToPlayer($"The stat '{stat}' does not exist. Use /printallstats to see a list of valid stats.");
        return false; //don't let the original method run
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(QuantumConsoleManager), nameof(QuantumConsoleManager.setarenaboss))]
    public static bool QuantumConsoleManager_setarenaboss(ref string bossName)
    {
        var bossDictionary = SingletonBehaviour<BossManager>.Instance.bossDictionary;
        var caseSensitiveKey = bossDictionary.GetCaseSensitiveKey(bossName);
        if (caseSensitiveKey.IsNullOrWhiteSpace())
        {
            Utils.LogToPlayer($"The boss '{bossName}' does not exist. Please use /printallarenabosses to get valid boss names.");
            return false; //don't let the original method run
        }

        bossName = caseSensitiveKey;
        return true; //let the original method run
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(QuantumConsoleManager), nameof(QuantumConsoleManager.setbirthday))]
    public static bool QuantumConsoleManager_setbirthday(ref string season)
    {
        var validEnum = Enum.TryParse(season, true, out Season _);
        if (validEnum) return true; //let the original method run

        Utils.LogToPlayer($"The season '{season}' does not exist. Use /printallseasons to see a list of valid seasons.");
        return false; //don't let the original method run
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(QuantumConsoleManager), nameof(QuantumConsoleManager.getrelationships))]
    public static bool QuantumConsoleManager_getrelationships()
    {
        foreach (var kvp in SingletonBehaviour<GameSave>.Instance.CurrentSave.characterData.relationships)
        {
            Utils.LogToPlayer($"{kvp.Key}: {kvp.Value}");
        }
        return false; //don't let the original method run
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(QuantumConsoleManager), nameof(QuantumConsoleManager.setbulletinboardquest))]
    public static bool QuantumConsoleManager_setbulletinboardquest(ref string questname1, ref string questname2)
    {
        var questDictionary = SingletonBehaviour<QuestManager>.Instance.questDictionary;
        var caseSensitiveKey1 = questDictionary.GetCaseSensitiveKey(questname1);
        if (caseSensitiveKey1.IsNullOrWhiteSpace())
        {
            Utils.LogToPlayer($"The quest '{questname1}' does not exist. Please use /printallbbquests to get valid quest names.");
            return false; //don't let the original method run
        }

        questname1 = caseSensitiveKey1;

        var caseSensitiveKey2 = questDictionary.GetCaseSensitiveKey(questname2);
        if (caseSensitiveKey2.IsNullOrWhiteSpace())
        {
            Utils.LogToPlayer($"The quest '{questname2}' does not exist. Please use /printallbbquests to get valid quest names.");
            return false; //don't let the original method run
        }

        questname2 = caseSensitiveKey2;
        return true; //let the original method run
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(QuantumConsoleManager), nameof(QuantumConsoleManager.setdayspeed))]
    public static bool QuantumConsoleManager_setdayspeed(int speed)
    {
        if (speed is 0 or 1 or 10 or 100 or 1000) return true; //let the original method run
        Utils.LogToPlayer($"The speed '{speed}' is not a valid speed. Please use 0, 1, 10, 100, or 1000.");
        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(QuantumConsoleManager), nameof(QuantumConsoleManager.setexp))]
    public static bool QuantumConsoleManager_setexp(string profession)
    {
        var validEnum = Enum.TryParse(profession, true, out ProfessionType _);
        if (validEnum) return true; //let the original method run

        Utils.LogToPlayer($"The profession '{profession}' does not exist. Use /printallprofessions to see a list of valid professions.");
        return false; //don't let the original method run
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(QuantumConsoleManager), nameof(QuantumConsoleManager.additem))]
    public static bool QuantumConsoleManager_additem(string item)
    {
        var id = Database.GetID(item);
        if (id <= 0)
        {
            Utils.LogToPlayer($"Item '{item}' not found. Please use /finditemid to get valid item names.");
            return false; //don't let the original method run
        }
        Database.GetData(id, delegate(ItemData data)
        {
            if (!data.isDLCItem)
            {
                return;
            }
            Utils.LogToPlayer($"Unfortunately, {data.FormattedName} ({data.ID}) is a DLC item and cannot be added via Cheat Enabler.");
        });
        return true; //let the original method run
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(QuantumConsole), nameof(QuantumConsole.Initialize))]
    public static void QuantumConsole_Initialize()
    {
        if (Plugin.Debug.Value)
        {
            Plugin.LOG.LogInfo("Forcing load of custom commands, and built-in commands.");
        }
        QuantumConsoleProcessor.LoadCommandsFromType(typeof(CheatEnablerCommands));
        QuantumConsoleProcessor.LoadCommandsFromType(typeof(QuantumConsoleManager));
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(SetLanguageDropdown), nameof(SetLanguageDropdown.OnValueChanged))]
    public static void SetLanguageDropdown_SetLanguageAndCode()
    {
        PlayerPrefs.SetInt("CheatEnabler", 0);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(LocalizationManager), nameof(LocalizationManager.SetLanguageAndCode))]
    public static void LocalizationManager_SetLanguageAndCode(string LanguageName, string LanguageCode)
    {
        if (Plugin.Debug.Value)
        {
            Plugin.LOG.LogInfo($"Language set to {LanguageName} ({LanguageCode})");
        }

        var lang = LocalizationManager.CurrentLanguageCode;
        var descDict = CommandDescriptionsHelper.GetLanguageDictionary(lang);
        if (descDict == null)
        {
            Plugin.LOG.LogError($"No descriptions dictionary found for '{lang}'");
            return;
        }
        foreach (var command in QuantumConsoleProcessor._commandTable)
        {
            var commandData = command.Value;

            if (descDict.TryGetValue(commandData.CommandSignature, out var description))
            {
                commandData.CommandDescription = description;
            }
            else
            {
                if (!SkipCommands.Contains(commandData.CommandName))
                {
                    Plugin.LOG.LogInfo($"Command '{commandData.CommandName}' - '{commandData.CommandSignature}' has no description. Language: '{lang}'");
                }
            }
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(QuantumConsoleProcessor), nameof(QuantumConsoleProcessor.TryAddCommand))]
    public static void QuantumConsoleProcessor_TryAddCommand(CommandData command, bool __result)
    {
        if (!__result) return;

        var lang = LocalizationManager.CurrentLanguageCode;
        var descDict = CommandDescriptionsHelper.GetLanguageDictionary(lang);
        if (descDict != null && descDict.TryGetValue(command.CommandSignature, out var description))
        {
            command.CommandDescription = description;
        }
        else
        {
            if (descDict == null && Plugin.Debug.Value)
            {
                Plugin.LOG.LogInfo($"No descriptions dictionary found for '{lang}'");
            }
            if (!SkipCommands.Contains(command.CommandName))
            {
                Plugin.LOG.LogInfo($"Command '{command.CommandName}' - '{command.CommandSignature}' has no description. Language: '{lang}'");
            }
        }

        if (Plugin.Debug.Value)
        {
            Plugin.LOG.LogInfo($"Added Command: '{command.CommandSignature}' from '{command.MethodData.DeclaringType.GetDisplayName()}'");
        }
    }

    private readonly static string[] SkipCommands =
    [
        "/man",
        "/help",
        "/manual"
    ];

}