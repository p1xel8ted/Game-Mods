namespace CheatEnabler;

[CommandPrefix("/")]
public static class CheatEnablerCommands
{
    private readonly static Dictionary<int, string> questStarts = new()
    {
        {1, "TheSunDragonsProtection1Quest"},
        {2, "TheSunDragonsProtection2Quest"},
        {3, "TheSunDragonsProtection3Quest"},
        {4, "TheSunDragonsProtection4Quest"},
        {5, "TheSunDragonsProtection5Quest"},
        {6, "TheSunDragonsProtection6Quest"},
        {7, "TheSunDragonsProtection7Quest"},
        {8, "TheSunDragonsProtection8Quest"}
    };

    private readonly static Dictionary<int, string[]> progressFlags = new()
    {
        {2, ["TheSunDragonsProtection1Quest", "JourneyToDragonsMeetCutscene1"]},
        {3, ["TheSunDragonsProtection2Quest", "JourneyToDragonsMeetCutscene2"]},
        {4, ["TheSunDragonsProtection3Quest", "TheSunDragonsProtectionCutscene1"]},
        {5, ["TheSunDragonsProtection4Quest"]},
        {6, ["TheSunDragonsProtection5Quest", "TheSunDragonsProtectionCutscene2"]},
        {7, ["TheSunDragonsProtection6Quest", "TheSunDragonsProtectionCutscene3", "TheSunDragonsProtectionCutscene4", "CollectedGloriteCrystal"]},
        {8, ["TheSunDragonsProtection7Quest", "TheSunDragonsProtectionCutscene5"]}
    };

    private readonly static Dictionary<int, string> questStartsNelvari = new()
    {
        {0, "TheSunDragonsProtection1Quest"},
        {1, "TheSunDragonsProtection2Quest"},
        {2, "TheSunDragonsProtection3Quest"},
        {3, "TheSunDragonsProtection4Quest"},
        {5, "TheSunDragonsProtection8Quest"},
        {6, "ClearingTheRoad1Quest"},
        {7, "TheMysteryOfNelvari2Quest"},
        {8, "TheMysteryOfNelvari3Quest"}
    };

    private readonly static Dictionary<int, string[]> progressFlagsNelvari = new()
    {
        {1, ["TheSunDragonsProtection1Quest", "JourneyToDragonsMeetCutscene1"]},
        {2, ["TheSunDragonsProtection2Quest", "JourneyToDragonsMeetCutscene2"]},
        {3, ["TheSunDragonsProtection3Quest", "TheSunDragonsProtectionCutscene1"]},
        {4, ["TheSunDragonsProtection4Quest", "TheSunDragonsProtectionCutscene2"]},
        {5, ["TheSunDragonsProtection3Quest", "TheSunDragonsProtectionCutscene3", "TheSunDragonsProtectionCutscene4", "TheSunDragonsProtectionCutscene5"]},
        {6, ["TheSunDragonsProtection8Quest", "TimeOfNeedCutscene1", "TimeOfNeedCutscene2"]},
        {7, ["TheMysteryOfNelvari1Quest"]},
        {8, ["TheMysteryOfNelvari2Quest"]}
    };

    private readonly static Dictionary<int, string> questStartsWithergate = new()
    {
        {1, "TheSunDragonsProtection8Quest"},
        {6, "ClearingTheRoad1Quest"},
        {7, "ClearingTheRoad2Quest"},
        {8, "JourneyToWithergate1Quest"},
        {9, "JourneyToWithergate2Quest"},
        {10, "JourneyToWithergate3Quest"},
        {11, "JourneyToWithergate4Quest"},
        {12, "JourneyToWithergate5Quest"},
        {13, "JourneyToWithergate6Quest"},
        {14, "ConfrontingDynus1Quest"},
        {15, "ConfrontingDynus2Quest"},
        {16, "ConfrontingDynus3Quest"},
        {17, "ConfrontingDynus4Quest"},
        {18, "ConfrontingDynus5Quest"},
        {19, "ConfrontingDynus6Quest"}
    };

    private readonly static Dictionary<int, string[]> progressFlagsWithergate = new()
    {
        {6, ["TheSunDragonsProtection8Quest", "TimeOfNeedCutscene1", "TimeOfNeedCutscene2", "NelvariTree", "CompleteNelvariTree0", "CompleteNelvariTree1", "CompleteNelvariTree2", "CompleteNelvariTree3"]},
        {7, ["NorthTownMonster", "ClearingTheRoad1Quest", "ClearingTheRoadCutscene1"]},
        {8, ["ClearingTheRoad2Quest", "ClearingTheRoadCutscene2"]},
        {9, ["JourneyToWithergate1Quest", "JourneyToWithergateCutscene1"]},
        {10, ["JourneyToWithergate2Quest", "JourneyToWithergateCutscene2"]},
        {11, ["JourneyToWithergate3Quest", "JourneyToWithergateCutscene3"]},
        {12, ["JourneyToWithergate4Quest", "JourneyToWithergateCutscene4"]},
        {13, ["Apartment", "JourneyToWithergate5Quest", "JourneyToWithergateCutscene5"]},
        {14, ["JourneyToWithergate6Quest"]},
        {15, ["ConfrontingDynus1Quest", "ConfrontingDynusCutscene1"]},
        {16, ["ConfrontingDynus2Quest", "ConfrontingDynusCutscene2"]},
        {17, ["ConfrontingDynus3Quest", "ConfrontingDynusCutscene3", "ConfrontingDynusCutscene4", "ConfrontingDynusCutscene5"]},
        {18, ["ConfrontingDynus4Quest", "DynusAltarCutscene"]},
        {19, ["ConfrontingDynus5Quest", "DynusIntroCutscene"]}
    };

    [Command]
    public static void getworldquestskipwithergatebreakpoints(int breakpoint)
    {
        var affectedQuests = new List<string>();

        // // Start base world quest for Withergate
        // affectedQuests.Add("StartWorldQuest: skiptoworldquest(9)");
        getworldquestskipbreakpoints(9);

        // Handle quest starts
        if (questStartsWithergate.TryGetValue(breakpoint, out var value1))
        {
            affectedQuests.Add($"StartQuest: {value1}");
        }

        // Handle progress flags
        for (var i = 1; i <= breakpoint; i++)
        {
            if (progressFlagsWithergate.TryGetValue(i, out var value))
            {
                affectedQuests.AddRange(value.Select(flag => $"SetProgress: {flag}"));
            }
        }

        foreach (var quest in affectedQuests)
        {
            Utils.LogToPlayer($"{quest}");
        }
    }
    [Command]
    public static void getworldquestskipepiloguebreakpoints(int breakpoint)
    {
        List<string> affectedQuests = [];

        switch (breakpoint)
        {
            case 0:
                affectedQuests.Add("StartQuest: Daybreak1AQuest");
                break;
            case 1:
                affectedQuests.Add("StartQuest: Daybreak1BQuest");
                break;
            case 2:
                affectedQuests.Add("SetProgress: PeaceWithWithergate8Cutscene");
                break;
            case 3:
                affectedQuests.Add("SetProgress: PeaceWithWithergate8Cutscene");
                affectedQuests.Add("SetProgress: FriendsToNelvariSummonsCutscene");
                affectedQuests.Add("StartQuest: FriendsToNelvari1Quest");
                break;
            case 4:
                affectedQuests.Add("SetProgress: EpilogueSpawnTonya");
                affectedQuests.Add("StartQuest: AHerosHarvest1AQuest");
                break;
        }

        foreach (var quest in affectedQuests)
        {
            Utils.LogToPlayer($"{quest}");
        }
    }

    [Command]
    public static void getworldquestskipnelvaribreakpoints(int breakpoint)
    {
        var affectedQuests = new List<string>();

        // Handle quest starts
        if (questStartsNelvari.TryGetValue(breakpoint, out var value1))
        {
            affectedQuests.Add($"StartQuest: {value1}");
        }

        // Handle progress flags
        for (var i = 1; i <= breakpoint; i++)
        {
            if (progressFlagsNelvari.TryGetValue(i, out var value))
            {
                affectedQuests.AddRange(value.Select(flag => $"SetProgress: {flag}"));
            }
        }

        // Handle specific quests starting at breakpoint 6
        if (breakpoint == 6)
        {
            affectedQuests.Add("StartQuest: ClearingTheRoad1Quest");
            affectedQuests.Add("StartQuest: TheMysteryOfNelvari1Quest");
            affectedQuests.Add("StartQuest: SunDragonsApprentice1Quest");
        }

        // Handle Nelvari Tree progress
        if (breakpoint <= 8)
        {
            for (var i = 0; i <= 3; i++)
            {
                affectedQuests.Add($"SetProgress: CompleteNelvariTree{i}");
            }
            affectedQuests.Add("SetProgress: NelvariTree");
        }

        foreach (var quest in affectedQuests)
        {
            Utils.LogToPlayer($"{quest}");
        }
    }

    [Command]
    public static void getworldquestskipbreakpoints(int breakpoint)
    {
        List<string> affectedQuests = [];

        if (questStarts.TryGetValue(breakpoint, out var start))
        {
            affectedQuests.Add($"StartQuest: {start}");
        }

        for (var i = 2; i <= breakpoint; i++)
        {
            if (progressFlags.TryGetValue(i, out var progressFlag))
            {
                affectedQuests.AddRange(progressFlag.Select(flag => $"SetProgress: {flag}"));
            }
        }

        foreach (var quest in affectedQuests)
        {
            Utils.LogToPlayer($"{quest}");
        }
    }

    [CommandDescription("Generates a user manual for any given command, including built in ones. To use the man command, simply put the desired command name in front of it. For example, 'man my-command' will generate the manual for 'my-command'")]
    [Command("help")]
    [Command("manual")]
    [Command]
    public static void man(string commandName)
    {
        //remove all / from commandName, and add just one to the beginning
        commandName = commandName.Replace("/", "");
        commandName = "/" + commandName;

        var manual = QuantumConsoleProcessor.GenerateCommandManual(commandName);
        if (string.IsNullOrWhiteSpace(manual))
        {
            Utils.SendNotification($"Manual for command '{commandName}' not found!");
        }
        else
        {
            throw new ArgumentException(manual);
        }
    }

    [Command]
    public static void printallscenes()
    {
        foreach (var scene in SingletonBehaviour<SceneSettingsManager>.Instance.sceneNameDictionary.Keys)
        {
            Utils.LogToPlayer($"{scene}");
        }
    }

    [Command]
    public static void printallseasons()
    {
        foreach (var season in Enum.GetNames(typeof(Season)))
        {
            Utils.LogToPlayer($"{season}");
        }
    }

    [Command]
    public static void printallprofessions()
    {
        foreach (var prof in Enum.GetNames(typeof(ProfessionType)))
        {
            Utils.LogToPlayer($"{prof}");
        }
    }

    [Command]
    public static void printallstats()
    {
        foreach (var stat in Enum.GetNames(typeof(StatType)))
        {
            Utils.LogToPlayer($"{stat}");
        }
    }

    [Command]
    public static void printallmail()
    {
        var allMail = SingletonBehaviour<MailManager>.Instance.mailDictionary;
        foreach (var mail in allMail)
        {
            Utils.LogToPlayer($"{mail.Key}");
        }
    }


    [Command]
    public static void printallbbquests()
    {
        var allQuests = SingletonBehaviour<QuestManager>.Instance.questDictionary;
        foreach (var quest in allQuests)
        {
            Utils.LogToPlayer($"{quest.Key} - {quest.Value.bulletinBoardPS}");
        }
    }


    [Command]
    public static void printromancenpcs()
    {
        var allNpcs = SingletonBehaviour<NPCManager>.Instance._npcs;
        foreach (var npc in allNpcs.Where(a => a.Value.Romanceable))
        {
            Utils.LogToPlayer($"{npc.Key} - {npc.Value.LocalizedActualNPCName}");
        }
    }

    [Command]
    public static void printallnpcs()
    {
        var allNpcs = SingletonBehaviour<NPCManager>.Instance._npcs;
        foreach (var npc in allNpcs)
        {
            Utils.LogToPlayer($"{npc.Key} - {npc.Value.LocalizedActualNPCName}");
        }
    }

    [Command]
    public static void printcompletedquests()
    {
        var completedQuests = SingletonBehaviour<GameSave>.Instance.CurrentSave.characterData.QuestData;
        foreach (var quest in completedQuests)
        {
            Utils.LogToPlayer($"{quest.Key}");
        }
    }

    [Command]
    public static void savegame()
    {
        SingletonBehaviour<GameSave>.Instance.SaveGame(true);
        SingletonBehaviour<NotificationStack>.Instance.SendNotification("Game Saved!");
    }

    [Command]
    public static void additembyid(int itemId, int amount = 1)
    {
        var item = Utils.GetNameByID(itemId);
        var qcm = Object.FindObjectOfType<Wish.QuantumConsoleManager>();
        try
        {
            Database.GetData(itemId, delegate(ItemData data)
            {
                if (data.isDLCItem)
                {
                   Utils.LogToPlayer($"Unfortunately, {data.FormattedName} is a DLC item and cannot be added via Cheat Enabler.");
                }
                else
                {
                    qcm.additem(item, amount);  
                }
            }, () => throw new Exception());
        }
        catch (Exception)
        {
            Utils.LogToPlayer($"Failed to add item {item} to inventory. Try /finditemid to get the correct item id.");
        }
    }

    [Command]
    public static void finditemid(string itemName)
    {
        List<KeyValuePair<string, int>> Exists = [];

        foreach (var word in itemName.Split(' '))
        {
            Exists.AddRange(Database.Instance.ids.Where(a => a.Key.ToLower().Contains(word.ToLower())));
        }

        if (Exists.Count == 0)
        {
            Utils.LogToPlayer($"No item found for '{itemName}'");
            return;
        }

        foreach (var item in Exists)
        {
            var itemKeyK = $"{item.Key}.Name";
            Utils.LogToPlayer($"{item.Value} - {item.Key} - {LocalizeText.TranslateText(itemKeyK, item.Key).Trim()}");
        }
    }

    [Command]
    public static void saveallitems()
    {
        var path = Path.Combine(Paths.GameRootPath, "items.txt");
        var sb = new StringBuilder();


        var itemsWithLocalizedNames = ItemInfoDatabase.Instance.allItemSellInfos
            .Select(item => new
            {
                item.Key,
                Name = Utils.GetNameByID(item.Key),
                LocalizedName = LocalizeText.TranslateText(item.Value.keyName, item.Value.name).Trim()
            })
            .ToList();


        var sortedItems = itemsWithLocalizedNames.OrderBy(item => item.LocalizedName, StringComparer.OrdinalIgnoreCase).ToList();


        foreach (var item in sortedItems)
        {
            sb.AppendLine($"{item.Key} - {item.Name} - {item.LocalizedName}");
        }


        File.WriteAllText(path, sb.ToString());

        Utils.LogToPlayer($"{sortedItems.Count} items saved to {path}");

        var processStartInfo = new ProcessStartInfo
        {
            FileName = path,
            UseShellExecute = true
        };

        Process.Start(processStartInfo);
    }
}