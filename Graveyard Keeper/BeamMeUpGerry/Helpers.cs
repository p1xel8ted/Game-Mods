namespace BeamMeUpGerry;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class Helpers
{
    private static bool MakingChoice { get; set; }

    private static WorldGameObject Gerry { get; set; }
    private static bool GerryRunning { get; set; }


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

        var newMessage = message != string.Empty ? message : (money ? GetMoneyMessage() : GetMessage());

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


    internal static bool RemoveZone(Location location)
    {
        //
        //     home
        //     mf_wood
        //     morgue_outside
        //     morgue
        //     graveyard
        //     wheat_land
        //     vilage
        //     tavern
        //     garden
        //     cellar
        //     tree_garden
        //     cremation
        //     hill
        //     burned_house
        //     vineyard
        //     sealight
        //     beatch
        //     church
        //     alchemy
        //     refugees_camp
        //     alarich_tent_inside
        //     flat_under_waterflow_2
        //     zombie_sawmill
        //     beegarden
        //     cellar_storage
        //     souls
        //     swamp
        //     witch_hut
        //     flat_under_waterflow
        //     marble_deposit
        //     stone_workyard
        //     euric_room
        //     storage
        //     players_tavern
        //     player_tavern_cellar
        //     sacrifice
        //     camp
        //     cliff
        
        if (Plugin.DebugEnabled.Value && MainGame.me != null && MainGame.me.save != null)
        {
            Plugin.Log.LogInfo("Players Seen Zones - Start");
            foreach (var z in MainGame.me.save.known_world_zones)
            {
                Plugin.Log.LogInfo(z);
            }
            Plugin.Log.LogInfo("Players Seen Zones - End");
        }

        var wheatExists = Tools.PlayerHasSeenZone(Constants.ZoneWheatLand);
        var coalExists = Tools.PlayerHasSeenZone(Constants.ZoneFlatUnderWaterflow);

        // Group similar conditions to reduce string operations
        if (location.zone.ContainsByLanguage(strings.Farmer))
        {
            return wheatExists && Tools.PlayerKnowsNpcPartial(Constants.Farmer);
        }

        if (location.zone.ContainsByLanguage(strings.Mill))
        {
            return wheatExists && Tools.PlayerKnowsNpcPartial(Constants.Miller);
        }

        if (location.zone.ContainsByLanguage(strings.Coal))
        {
            return coalExists;
        }

        //   @lighthouse -> sealight
        //      @quarry -> stone_workyard
        // @players_tavern -> players_tavern
        //   @zone_nountain_fort -> @nountain_fort
        //   @zone_refugees_camp_tp -> refugees_camp
        var UnusualMaps = new Dictionary<string, string>()
        {
            {"@lighthouse", "sealight"},
            {"@quarry", "stone_workyard"},
            {"@players_tavern", "players_tavern"},
            {"@zone_nountain_fort", "camp"}, //no match, this is close enough
            {"@zone_refugees_camp_tp", "refugees_camp"}
        };

        string[] locationsToReturnFalse =
        [
            Constants.Mystery, strings.Clay, strings.Sand, strings.Page_1, strings.Page_2, strings.Page_3, strings.Page_4, strings.Page_5, strings.Page_6, strings.Page_7, strings.Page_8, strings.Page_9, strings.Page_10, strings.Custom_Locations, Constants.Cancel
        ];

        // Check for multiple conditions in a single if statement
        if (Array.IndexOf(locationsToReturnFalse, location.zone) != -1)
        {
            Log($"[RemoveZone] {location.zone} is a special case. Returning false.");
            return false;
        }

        if (UnusualMaps.TryGetValue(location.zone, out var zone1))
        {
            var removeUnusualZone = !MainGame.me.save.known_world_zones.Exists(a => a.EqualsByLanguage(zone1));
            Log($"[RemoveUnusualZone] - {removeUnusualZone} - {location.zone} -> {zone1}");
            return removeUnusualZone;
        }

        // Perform the replace operation only if necessary
        var zone = location.zone.Replace(Constants.ZonePartial, string.Empty);
        var removeZone = !MainGame.me.save.known_world_zones.Exists(a => a.EqualsByLanguage(zone));
        Log($"[RemoveZone] - {removeZone} - {location.zone} -> {zone}");
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
        var distance = Vector3.Distance(teleportPoints[0].pos, MainGame.me.player.pos3);
        var coords = MainGame.me.player.grid_pos;
        var state = EnvironmentEngine.me.data.state;

        Plugin.Log.LogMessage(preset == null ? $"Zone: {zone}, Preset: {preset}, Closest TeleportPoint: {teleportPoints[0]} (Distance: {distance}), Coords: {coords}, State: {state.ToString()}" : $"Zone: {zone}, Preset: {preset.name}, Closest TeleportPoint: {teleportPoints[0]} (Distance: {distance}), Coords: {coords}, State: {state.ToString()}");

        var location = new Location(zone, preset == null ? "" : preset.name, teleportPoints[0].gd_tag, coords, false, state);
        location.SaveJson();
        onComplete?.Invoke();
    }
}