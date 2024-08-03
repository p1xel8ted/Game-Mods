namespace BeamMeUpGerry;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class Helpers
{
    internal static bool MakingChoice { get; set; }

    private static WorldGameObject Gerry { get; set; }
    private static bool GerryRunning { get; set; }


    private static bool RemovedQuarryBlockage()
    {
        return MainGame.me.save.completed_one_time_crafts.Any(craft => craft.StartsWith("steep_yellow_blockage"));
    }
    
    
    internal static void Log(string message)
    {
        if (Plugin.DebugEnabled.Value)
        {
            Plugin.Log.LogInfo(message);
        }
    }

    internal static Item GetHearthstone()
    {
        return MainGame.me.player.data.GetItemWithID(Constants.Hearthstone);
    }


    private static string GetMoneyMessage()
    {
        var messageList = new List<string>
        {
            strings.M1, strings.M2, strings.M3, strings.M4, strings.M5,
            strings.M6, strings.M7, strings.M8, strings.M9, strings.M10
        };
        return messageList.RandomElement();
    }

    private static float GenerateFee()
    {
        const float minimumFee = 0.01f;
        const float maximumFee = 5f;
        var playerMoney = MainGame.me.player.data.money;

        // Calculate dynamic fee based on player's money
        var dynamicFee = (float) Math.Max(minimumFee, Math.Min(maximumFee, Math.Round(0.1f * playerMoney / 100f, 2)));

        // Logging for debugging or information purposes
        Log($"[Fee]: {Trading.FormatMoney(dynamicFee, true)}\nMoney: {Trading.FormatMoney(playerMoney, true)}, Minimum: {Trading.FormatMoney(minimumFee, true)}");

        return dynamicFee;
    }


    internal static void TakeMoney(Vector3 vector)
    {
        if (!Plugin.GerryCharges.Value) return;

        // Adjust the vector's y-coordinate
        vector.y += 125f;

        // Calculate the fee to be paid
        var feeToPay = GenerateFee();

        // Deduct the fee from the player's money
        MainGame.me.player.data.money -= feeToPay;

        // Play the coin sound effect at the modified vector position
        Sounds.PlaySound("coins_sound", vector, true);

        // Show the effect bubble for the fee deduction
        var formattedFee = Trading.FormatMoney(feeToPay, true);
        EffectBubblesManager.ShowImmediately(vector, $"-{formattedFee}", EffectBubblesManager.BubbleColor.Red, true, 3f);
    }

    public static string RemoveCharacters(string locationZone)
    {
        var translation = SpeechBubbleGUI.SpeechText(locationZone);
        var removeTheseCharacters = new[] {'\n', '\t', '\\', '\'', '[', ']'};
        var cleanedString = removeTheseCharacters.Aggregate(translation, (current, c) => current.Replace(c.ToString(), ""));
        var textInfo = CultureInfo.CurrentCulture.TextInfo;
        return textInfo.ToTitleCase(cleanedString.ToLower());
    }


    private static string GetMessage()
    {
        var messageList = new List<string>
        {
            strings.M4, strings.M7,
            strings.M8, strings.M9
        };
        return messageList.RandomElement();
    }

    internal static void SpawnGerry(string message, Vector3 customPosition, bool money = false)
    {
        if (GerryRunning) return;

        Thread.CurrentThread.CurrentUICulture = CrossModFields.Culture;

        var location = customPosition != Vector3.zero ? customPosition : MainGame.me.player_pos;
        location.x -= 75f;

        if (Gerry == null)
        {
            InitializeGerry(location);
        }

        GJTimer.AddTimer(0.5f, () => GerryDialogueHandler(message, money));
    }

    private static void InitializeGerry(Vector3 location)
    {
        Gerry = WorldMap.SpawnWGO(MainGame.me.world_root.transform, Constants.GerryTalkingSkullID, location);
        Tools.NameSpawnedGerry(Gerry);
        Tools.ShowCinematic(Gerry.transform);
        Gerry.ReplaceWithObject(Constants.GerryTalkingSkullID, true);
        Tools.NameSpawnedGerry(Gerry);
        GerryRunning = true;
    }

    private static void GerryDialogueHandler(string message, bool money)
    {
        if (Gerry == null)
        {
            Tools.HideCinematic();
            return;
        }

        var newMessage = message != string.Empty ? message : money ? GetMoneyMessage() : GetMessage();

        Gerry.Say(newMessage, () => GJTimer.AddTimer(0.25f, () => FinalizeGerry(money)), null, SpeechBubbleGUI.SpeechBubbleType.Talk, SmartSpeechEngine.VoiceID.Skull);
    }

    private static void FinalizeGerry(bool money)
    {
        if (Gerry == null)
        {
            Tools.HideCinematic();
            return;
        }

        Gerry.ReplaceWithObject(Constants.GerryTalkingSkullID, true);
        Tools.NameSpawnedGerry(Gerry);
        Tools.HideCinematic();
        Gerry.DestroyMe();
        Gerry = null;
        GerryRunning = false;

        if (money)
        {
            TakeMoney(MainGame.me.player_pos);
        }
    }

    private readonly static Dictionary<string,string> UnusualMaps = new()
    {
        {"@lighthouse", "sealight"},
        {"@quarry", "stone_workyard"},
        {"@players_tavern", "players_tavern"},
        {"@zone_nountain_fort", "camp"}, //no match, this is close enough
        {"@zone_refugees_camp_tp", "refugees_camp"}
    };

    private static string[] LocationsToReturnFalse =>
    [
        "home",
        "mf_wood",
        "morgue_outside",
        "morgue",
        "garden",
        "beegarden",
        "graveyard",
        "wheat_land",
        "zombie_sawmill",
        "flat_under_waterflow_3",
        "vilage",
        "tavern",
        "flat_under_waterflow_2",
        "flat_under_waterflow",
        "marble_deposit",
        "stone_workyard",
        "@lighthouse",
        "sealight",
        "@quarry",
        "tree_garden",
        "stone_workyard",
        "@players_tavern",
        "players_tavern",
        "@zone_nountain_fort",
        "camp",
        "@zone_refugees_camp_tp",
        "refugees_camp",
        Constants.Mystery,
        strings.Clay,
        strings.Sand,
        strings.Page_1,
        strings.Page_2,
        strings.Page_3,
        strings.Page_4,
        strings.Page_5,
        strings.Page_6,
        strings.Page_7,
        strings.Page_8,
        strings.Page_9,
        strings.Page_10,
        strings.Custom_Locations,
        Constants.Cancel
    ];
    
    internal static string[] BlockageLocations =>
    [
        "@quarry",
        "stone_workyard",
        "@zone_refugees_camp_tp",
        "refugees_camp",
        "marble_deposit",
        "zombie_sawmill",
        strings.Coal,
        Constants.ZoneFlatUnderWaterflow
    ];
    
    internal static bool RemoveZone(Location location)
    {
        if (location.customZone)
        {
            Plugin.Log.LogInfo($"[RemoveZone] {location.zone} is a custom location. Not removing.");
            return false;
        }

        var removedBlockage = RemovedQuarryBlockage();
        var wheatExists = Tools.PlayerHasSeenZone(Constants.ZoneWheatLand);
        
        if (!removedBlockage)
        {
            if (Array.IndexOf(BlockageLocations, location.zone) != -1)
            {
                Plugin.Log.LogInfo($"[RemoveZone-Blockage] {location.zone} is blocked. Removing.");
                return true;
            }
        }
        
        if (location.zone.ContainsByLanguage(strings.Farmer))
        {
            var farmer = wheatExists && Tools.PlayerKnowsNpcPartial(Constants.Farmer);
            Plugin.Log.LogInfo($"[RemoveZone-Farmer] - {farmer} - {location.zone}");
            return farmer;
        }
        
        if (location.zone.ContainsByLanguage(strings.Mill) && !location.zone.ContainsByLanguage("zombie"))
        {
            var mill= wheatExists && Tools.PlayerKnowsNpcPartial(Constants.Miller);
            Plugin.Log.LogInfo($"[RemoveZone-WheatMill] - {mill} - {location.zone}");
            return mill;
        }
        
        if (Array.IndexOf(LocationsToReturnFalse, location.zone) != -1)
        {
            Plugin.Log.LogInfo($"[RemoveZone-Special] {location.zone} is a special case (or default location). Not removing.");
            return false;
        }

        if (UnusualMaps.TryGetValue(location.zone, out var zone1))
        {
            var removeUnusualZone = !MainGame.me.save.known_world_zones.Exists(a => a.EqualsByLanguage(zone1));
            Plugin.Log.LogInfo($"[RemoveZone-UnusualMap] - {removeUnusualZone} - {location.zone} -> {zone1}");
            return removeUnusualZone;
        }
        
        var zone = location.zone.Replace(Constants.ZonePartial, string.Empty);
        var removeZone = !MainGame.me.save.known_world_zones.Exists(a => a.EqualsByLanguage(zone));
        Plugin.Log.LogInfo($"[RemoveZone-KnownZones] - {removeZone} - {location.zone} -> {zone}");
        return removeZone;
    }

    internal static void UpdateEnvironmentPreset(Location location)
    {
        EnvironmentEngine.me.SetEngineGlobalState(location.state);
        Log($"[ApplyCurrentEnvironmentPreset, id] = {location.preset}");
        var environmentPreset = EnvironmentPreset.Load(location.preset);
        EnvironmentEngine.me.ApplyEnvironmentPreset(environmentPreset);
    }

    internal static void EnablePlayerControl()
    {
        MakingChoice = false;
        GS.SetPlayerEnable(true, true);
    }

    internal static void DisablePlayerControl()
    {
        MakingChoice = true;
        GS.SetPlayerEnable(false, true);
    }

    internal static Vector3 MessagePositioning()
    {
        var location = MainGame.me.player_pos;
        location.x += 125f;
        location.y += 125f;
        return location;
    }

    internal static bool HasTheMoney()
    {
        if (!Plugin.GerryCharges.Value) return true;

        if (MainGame.me.player.data.money >= GenerateFee()) return true;

        SpawnGerry(strings.MoreCoin, MessagePositioning());
        return false;
    }


    public static bool IsUpdateConditionsMet()
    {
        return MultiAnswerGUI._current == null && !MakingChoice && MainGame.game_started && GS.IsPlayerEnable() && !MainGame.me.player.is_dead && !MainGame.me.player.IsDisabled() && !MainGame.paused && BaseGUI.all_guis_closed;
    }

    public static bool InTutorial()
    {
        return !Tools.TutorialDone() || MainGame.me.save.IsInTutorial();
    }

    internal static IEnumerator LogPosition(Action onComplete = null)
    {
        if (!MainGame.game_started || MainGame.me.player == null || EnvironmentEngine.me == null) yield break;
        var myZone = MainGame.me.player.GetMyWorldZoneId();
        var zone = myZone.IsNullOrWhiteSpace() ? string.Empty : $"zone_{MainGame.me.player.GetMyWorldZoneId()}";
        var preset = EnvironmentEngine.cur_preset;
        var teleportPoints = WorldMap.gd_points.Where(a => a.IsTPPoint()).OrderBy(a => Vector3.Distance(a.pos, MainGame.me.player.pos3)).ToList();
        var coords = MainGame.me.player.grid_pos;
        // Plugin.Log.LogWarning($"Local: {MainGame.me.player.transform.localPosition}, Global: {MainGame.me.player.transform.position} ");
        var state = EnvironmentEngine.me.data.state;
        var location = new Location($"custom_{zone}_rename_me", preset == null ? "" : preset.name, teleportPoints[0].gd_tag, coords, false, state)
        {
            customZone = true
        };
        location.SaveJson();
        onComplete?.Invoke();
    }
}