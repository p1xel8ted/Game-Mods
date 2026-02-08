namespace BeamMeUpGerry;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class Helpers
{
    internal static bool MakingChoice { get; set; }
    internal static bool IsInDungeon { get; set; }

    private static WorldGameObject Gerry { get; set; }
    private static bool GerryRunning { get; set; }
    private static bool _cinematicPlaying;

    private static CultureInfo GameCulture => CultureInfo.GetCultureInfo(
        GameSettings.me.language.Replace('_', '-').ToLower(CultureInfo.InvariantCulture).Trim());

    private static readonly string[] TutorialQuests =
    [
        "start",
        "get_out_from_house_tech",
        "get_out_from_house",
        "dig_graved_skull",
        "go_to_talk_with_donkey_first_time",
        "go_to_mortuary_after_skull_tech",
        "go_to_mortuary_after_skull",
        "on_skull_talk_autopsi",
        "go_to_graveyard_and_talk_with_skull",
        "ghost_come_after_1st_burial",
        "skull_talk_after_burial",
        "tools_from_grave_chest_taken_tech",
        "take_tools_from_grave_chest",
        "goto_tavern",
        "goto_tavern_tech",
        "goto_tavern_2",
        "player_repairs_sword_before",
        "player_repairs_sword"
    ];

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
            Language.GetTranslation(Language.Terms.M1), Language.GetTranslation(Language.Terms.M2), Language.GetTranslation(Language.Terms.M3),
            Language.GetTranslation(Language.Terms.M4), Language.GetTranslation(Language.Terms.M5), Language.GetTranslation(Language.Terms.M6),
            Language.GetTranslation(Language.Terms.M7), Language.GetTranslation(Language.Terms.M8), Language.GetTranslation(Language.Terms.M9),
            Language.GetTranslation(Language.Terms.M10)
        };
        return messageList.RandomElement();
    }

    private static float GenerateFee()
    {
        const float minimumFee = 0.01f;
        const float maximumFee = 5f;
        var playerMoney = MainGame.me.player.data.money;

        // Calculate dynamic fee based on player's money
        var dynamicFee = (float)Math.Max(minimumFee, Math.Min(maximumFee, Math.Round(0.1f * playerMoney / 100f, 2)));

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
        var removeTheseCharacters = new[] { '\n', '\t', '\\', '\'', '[', ']' };
        var cleanedString = removeTheseCharacters.Aggregate(translation, (current, c) => current.Replace(c.ToString(), ""));
        var textInfo = CultureInfo.CurrentCulture.TextInfo;
        return textInfo.ToTitleCase(cleanedString.ToLower());
    }


    private static string GetMessage()
    {
        var messageList = new List<string>
        {
            Language.GetTranslation(Language.Terms.M4), Language.GetTranslation(Language.Terms.M7),
            Language.GetTranslation(Language.Terms.M8), Language.GetTranslation(Language.Terms.M9)
        };
        return messageList.RandomElement();
    }

    internal static void SpawnGerry(string message, Vector3 customPosition, bool money = false)
    {
        if (GerryRunning) return;

        Thread.CurrentThread.CurrentUICulture = GameCulture;

        var location = customPosition != Vector3.zero ? customPosition : MainGame.me.player_pos;
        location.x -= 75f;

        if (Gerry == null)
        {
            InitializeGerry(location);
        }

        GJTimer.AddTimer(0.5f, () => GerryDialogueHandler(message, money));
    }

    private static void NameSpawnedGerry(WorldGameObject gerry)
    {
        if (gerry == null) return;
        gerry.tag = "mod_gerry";
        gerry.custom_tag = "mod_gerry";
    }

    private static void ShowCinematic(Transform transform)
    {
        if (_cinematicPlaying) return;
        _cinematicPlaying = true;
        GS.AddCameraTarget(transform);
        GS.SetPlayerEnable(false, true);
    }

    private static void HideCinematic()
    {
        if (!_cinematicPlaying) return;
        _cinematicPlaying = false;
        GS.AddCameraTarget(MainGame.me.player.transform);
        GS.SetPlayerEnable(true, true);
    }

    private static void InitializeGerry(Vector3 location)
    {
        Gerry = WorldMap.SpawnWGO(MainGame.me.world_root.transform, Constants.GerryTalkingSkullID, location);
        NameSpawnedGerry(Gerry);
        if (Plugin.CinematicEffect.Value)
        {
            ShowCinematic(Gerry.transform);
        }

        Gerry.ReplaceWithObject(Constants.GerryTalkingSkullID, true);
        NameSpawnedGerry(Gerry);
        GerryRunning = true;
    }

    private static void GerryDialogueHandler(string message, bool money)
    {
        if (Gerry == null)
        {
            HideCinematic();
            return;
        }

        var newMessage = message != string.Empty ? message : money ? GetMoneyMessage() : GetMessage();

        Gerry.Say(newMessage, () => GJTimer.AddTimer(0.25f, () => FinalizeGerry(money)), null, SpeechBubbleGUI.SpeechBubbleType.Talk, SmartSpeechEngine.VoiceID.Skull);
    }

    private static void FinalizeGerry(bool money)
    {
        if (Gerry == null)
        {
            HideCinematic();
            return;
        }

        Gerry.ReplaceWithObject(Constants.GerryTalkingSkullID, true);
        NameSpawnedGerry(Gerry);
        HideCinematic();
        Gerry.DestroyMe();
        Gerry = null;
        GerryRunning = false;

        if (money)
        {
            TakeMoney(MainGame.me.player_pos);
        }
    }

    private static readonly Dictionary<string, string> UnusualMaps = new(StringComparer.OrdinalIgnoreCase)
    {
        { Constants.ZoneLSand, Constants.SandMoundZone },
        { Constants.ZoneLClay, Constants.ClayPitZone },
        { Constants.ZoneLLighthouse, "sealight" },
        { Constants.ZoneLQuarry, "stone_workyard" },
        { Constants.ZoneLFellingsite, "zombie_sawmill" },
        { Constants.ZoneLCoal, Constants.NorthCoalZone },
        { "@lighthouse", "sealight" },
        { "@quarry", "stone_workyard" },
        { "@players_tavern", "players_tavern" },
        { "@zone_nountain_fort", "camp" }, //no match, this is close enough
        { "@zone_refugees_camp_tp", "refugees_camp" }
    };

    internal static bool RemoveZone(Location location)
    {
        if (!Plugin.RestrictToFoundLocations.Value)
        {
            Plugin.Log.LogInfo($"[RemoveZone] {location.zone} - RestrictToFoundLocations is false. Not removing.");
            return false;
        }

        if (location.customZone)
        {
            Plugin.Log.LogInfo($"[RemoveZone] {location.zone} is a custom location. Not removing.");
            return false;
        }

        var wheatExists = MainGame.me.save.known_world_zones.Exists(a => a.Equals(Constants.ZoneWheatLand));

        if (location.zone.Contains("farmer"))
        {
            var knowsFarmer = MainGame.me.save.known_npcs.npcs.Exists(a => a.npc_id.Contains(Constants.Farmer));
            bool remove;

            if (wheatExists)
            {
                remove = !knowsFarmer;
            }
            else
            {
                remove = false;
            }

            Plugin.Log.LogInfo($"[RemoveZone-Farmer] - {remove} - {location.zone} - Seen Wheat Land?: {wheatExists}, Talked to Farmer?: {knowsFarmer}");
            return remove;
        }

        if (location.zone.Contains("mill") && !location.zone.Contains("zombie"))
        {
            var knowsMiller = MainGame.me.save.known_npcs.npcs.Exists(a => a.npc_id.Contains(Constants.Miller));
            bool remove;

            if (wheatExists)
            {
                remove = !knowsMiller;
            }
            else
            {
                remove = false;
            }

            Plugin.Log.LogInfo($"[RemoveZone-Miller] - {remove} - {location.zone} - Seen Wheat Land?: {wheatExists}, Talked to Miller?: {knowsMiller}");
            return remove;
        }

        if (UnusualMaps.TryGetValue(location.zone, out var zone1))
        {
            var removeUnusualZone = !MainGame.me.save.known_world_zones.Exists(a => a.Equals(zone1));
            Plugin.Log.LogInfo($"[RemoveZone-UnusualMap] - {removeUnusualZone} - {location.zone} -> {zone1}");
            return removeUnusualZone;
        }

        var zone = location.zone.Replace(Constants.ZonePartial, string.Empty);
        var removeZone = !MainGame.me.save.known_world_zones.Exists(a => a.Equals(zone));
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
        GS.SetPlayerEnable(true, Plugin.CinematicEffect.Value);
    }

    internal static void DisablePlayerControl()
    {
        MakingChoice = true;
        GS.SetPlayerEnable(false, Plugin.CinematicEffect.Value);
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

        SpawnGerry(Language.GetTranslation(Language.Terms.MoreCoin), MessagePositioning());
        return false;
    }


    public static bool IsUpdateConditionsMet()
    {
        return MultiAnswerGUI._current == null && !MakingChoice && MainGame.game_started && GS.IsPlayerEnable() && !MainGame.me.player.is_dead && !MainGame.me.player.IsDisabled() && !MainGame.paused && BaseGUI.all_guis_closed;
    }

    internal static bool PlayerDisabled()
    {
        return MainGame.me.player.is_dead || !GS.IsPlayerEnable();
    }

    private static bool TutorialDone()
    {
        if (!MainGame.game_started) return false;
        var completed = false;
        foreach (var q in TutorialQuests)
        {
            completed = MainGame.me.save.quests.IsQuestSucced(q);
            if (!completed) break;
        }

        return !MainGame.me.save.IsInTutorial() && completed;
    }

    public static bool InTutorial()
    {
        return !TutorialDone() || MainGame.me.save.IsInTutorial();
    }

    internal static IEnumerator LogPosition(Action onComplete = null)
    {
        if (!MainGame.game_started || MainGame.me.player == null || EnvironmentEngine.me == null) yield break;
        var dateTimeNow = DateTime.Now.ToString("HH_mm_ss_dd_MM_yyyy"); //convert to filesystem safe format
        var myZone = MainGame.me.player.GetMyWorldZoneId();
        var zone = myZone.IsNullOrWhiteSpace() ? string.Empty : $"zone_{MainGame.me.player.GetMyWorldZoneId()}";
        var preset = EnvironmentEngine.cur_preset;
        var teleportPoints = WorldMap.gd_points.Where(a => a.IsTPPoint()).OrderBy(a => Vector3.Distance(a.pos, MainGame.me.player.pos3)).ToList();
        var coords = MainGame.me.player.grid_pos;
        var state = EnvironmentEngine.me.data.state;
        var location = new Location($"custom_{zone}_rename_me_{dateTimeNow}", preset == null ? "" : preset.name, teleportPoints[0].gd_tag, coords, false, state)
        {
            customZone = true
        };
        location.SaveJson();

        if (onComplete == null) yield break;

        Plugin.Log.LogWarning("Invoking onComplete");
        onComplete.Invoke();
    }

    internal static bool TryAdd<T>(this List<T> list, T item)
    {
        if (list.Contains(item)) return false;
        list.Add(item);
        return true;
    }

    internal static T TryAddComponent<T>(this GameObject gameObject) where T : Component
    {
        return gameObject.GetComponent<T>() ?? gameObject.AddComponent<T>();
    }
}
