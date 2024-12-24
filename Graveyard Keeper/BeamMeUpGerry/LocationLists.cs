namespace BeamMeUpGerry;

public static class LocationLists
{
    // internal static readonly List<Location> CustomLocations = [];
    internal static readonly List<Location> AllLocations =
    [
        new("zone_home", "house", "tp_house_b", new Vector2(29.5f, -68.4f), true, EnvironmentEngine.State.Inside), //tp_house_b
        new("zone_tavern", "tavern", "tp_tavern_b", new Vector2(52.5f, -87.5f), true, EnvironmentEngine.State.Inside), //tp_tavern_b
        new(Constants.ZoneLLighthouse, string.Empty, "tp_lighthouse_a", new Vector2(268.0f, -23.0f), true), //tp_lighthouse_a
        new(Constants.ZoneLQuarry, "mining", "tp_mining_hut_b", new Vector2(49.0f, -122.9f), true, EnvironmentEngine.State.Inside), //tp_mining_hut_b
        new("@players_tavern", "players", "tp_players_tavern_front_b", new Vector2(203.5f, -97.5f), true, EnvironmentEngine.State.Inside), //tp_players_tavern_front_b
        new("@zone_nountain_fort", string.Empty, "tp_mountain_fort_point_a", new Vector2(203.0f, 39.0f), true), //tp_mountain_fort_point_a
        new("@zone_refugees_camp_tp", string.Empty, "tp_refugee_point_a", new Vector2(25.0f, 59.0f), true), //tp_refugee_point_a
        new("zone_witch_hut", string.Empty, string.Empty, new Vector2(-51.7f, -18.5f)),
        new("zone_cellar", "mortuary", string.Empty, new Vector2(112.9f, -96.3f), false, EnvironmentEngine.State.Inside),
        new("zone_alchemy", "mortuary", string.Empty, new Vector2(85.9f, -106.0f), false, EnvironmentEngine.State.Inside),
        new("zone_morgue", "mortuary", string.Empty, new Vector2(101.5f, -118.0f), false, EnvironmentEngine.State.Inside),
        new("zone_beegarden", string.Empty, string.Empty, new Vector2(33.7f, 18.9f)),
        new("zone_hill", string.Empty, string.Empty, new Vector2(86.4f, 14.5f)),
        new("zone_sacrifice", string.Empty, string.Empty, new Vector2(99.3f, -87.8f), false, EnvironmentEngine.State.Inside),
        new("zone_beatch", string.Empty, string.Empty, new Vector2(234.4f, 3.3f)),
        new("zone_vineyard", string.Empty, string.Empty, new Vector2(67.4f, 4.26f)),
        new("zone_camp", string.Empty, string.Empty, new Vector2(215.5f, 29.4f)),
        new("zone_souls", "mortuary", string.Empty, new Vector2(115.1f, -112.6f), false, EnvironmentEngine.State.Inside),
        new("zone_graveyard", string.Empty, string.Empty, new Vector2(17.0f, -15.7f)),
        new("zone_euric_room", "euric", string.Empty, new Vector2(209.5f, -120.8f), false, EnvironmentEngine.State.Inside),
        new("zone_church", "church", string.Empty, new Vector2(1.9f, -85.6f), false, EnvironmentEngine.State.Inside),
        new(Constants.ZoneLFellingsite, string.Empty, string.Empty, new Vector2(23.0f, 35.5f)),
        new(Constants.ZoneLCoal, string.Empty, string.Empty, new Vector2(-5.3f, 63.5f)),
        new(Constants.ZoneLClay, string.Empty, string.Empty, new Vector2(6.2f, -33.2f)),
        new(Constants.ZoneLSand, string.Empty, string.Empty, new Vector2(3.5f, 9.1f)),
        new(Constants.ZoneLMill, string.Empty, string.Empty, new Vector2(123.0f, -8.0f)),
        new(Constants.ZoneLFarmer, string.Empty, string.Empty, new Vector2(122.9f, -33.9f))
    ];

    private static AnswerVisualData Page1Answer => new() { id = Language.GetTranslation(Language.Terms.Page1) };
    private static AnswerVisualData Page2Answer => new() { id = Language.GetTranslation(Language.Terms.Page2) };
    private static AnswerVisualData Page3Answer => new() { id = Language.GetTranslation(Language.Terms.Page3) };
    private static AnswerVisualData Page4Answer => new() { id = Language.GetTranslation(Language.Terms.Page4) };
    private static AnswerVisualData Page5Answer => new() { id = Language.GetTranslation(Language.Terms.Page5) };
    private static AnswerVisualData Page6Answer => new() { id = Language.GetTranslation(Language.Terms.Page6) };
    private static AnswerVisualData Page7Answer => new() { id = Language.GetTranslation(Language.Terms.Page7) };
    private static AnswerVisualData Page8Answer => new() { id = Language.GetTranslation(Language.Terms.Page8) };
    private static AnswerVisualData Page9Answer => new() { id = Language.GetTranslation(Language.Terms.Page9) };
    private static AnswerVisualData Page10Answer => new() { id = Language.GetTranslation(Language.Terms.Page10) };
    private static AnswerVisualData CancelAnswer => new() { id = Constants.Cancel };

    internal static readonly List<List<AnswerVisualData>> Locations = [];


    // internal static Dictionary<string, string> LocationTranslationMap => new(StringComparer.OrdinalIgnoreCase)
    // {
    //     {Constants.ZoneLLighthouse, strings.Lighthouse},
    //     {Constants.ZoneLQuarry, strings.Quarry},
    //     {Constants.ZoneLFellingsite, strings.FellingSite},
    //     {Constants.ZoneLCoal, strings.Coal},
    //     {Constants.ZoneLClay, strings.Clay},
    //     {Constants.ZoneLSand, strings.Sand},
    //     {Constants.ZoneLMill, strings.Mill},
    //     {Constants.ZoneLFarmer, strings.Farmer}
    //     
    // };


    internal static void LogData()
    {
        if (Plugin.DebugEnabled.Value && MainGame.me && MainGame.me.save != null)
        {
            Plugin.Log.LogInfo("|---------- Players Known NPC:Start ----------|");
            foreach (var z in MainGame.me.save.known_npcs.npcs)
            {
                Plugin.Log.LogInfo(z.npc_id);
            }

            Plugin.Log.LogInfo("|---------- Players Known NPC:End ----------|");

            Plugin.Log.LogInfo("|---------- Players Seen Zones:Start ----------|");
            foreach (var z in MainGame.me.save.known_world_zones)
            {
                Plugin.Log.LogInfo(z);
            }

            Plugin.Log.LogInfo("|---------- Players Seen Zones:End ----------|");

            Plugin.Log.LogInfo("|---------- One Time Crafts:Start ----------|");
            foreach (var blockage in MainGame.me.save.completed_one_time_crafts)
            {
                Plugin.Log.LogInfo($"[Completed One Time Crafts] - {blockage}");
            }

            Plugin.Log.LogInfo("|---------- One Time Crafts:End ----------|");

            Plugin.Log.LogInfo("|---------- Unlocked Phrases:Start ----------|");
            foreach (var phrase in MainGame.me.save.unlocked_phrases)
            {
                Plugin.Log.LogInfo($"[Unlocked Phrase] - {phrase}");
            }

            Plugin.Log.LogInfo("|---------- Unlocked Phrases:End ----------|");

            Plugin.Log.LogInfo("|---------- Blacklist Phrases:Start ----------|");
            foreach (var phrase in MainGame.me.save.black_list_of_phrases)
            {
                Plugin.Log.LogInfo($"[Blacklist Phrase] - {phrase}");
            }

            Plugin.Log.LogInfo("|---------- Blacklist Phrases:End ----------|");
        }
    }

    internal static void CreatePages()
    {
        LogData();

        Locations.Clear();

        var locationsList = AllLocations;

        foreach (var loc in locationsList)
        {
            Plugin.LocationSettings.TryGetValue(loc.zone, out var settings);
            loc.enabled = settings == null || settings.Value;
        }

        var locations = locationsList
            .Where(location => location.enabled)
            .ToList();


        if (Plugin.SortAlphabetically.Value)
        {
            locations = locations
                .OrderBy(location => Helpers.RemoveCharacters(location.zone))
                .ToList();
        }


        if (MainGame.me && MainGame.me.save != null)
        {
            locations = locations.Where(a => !Helpers.RemoveZone(a)).ToList();
        }

        var pageCount = Mathf.CeilToInt(locations.Count / (float)Plugin.LocationsPerPage.Value);

        for (var pageIndex = 0; pageIndex < pageCount; pageIndex++)
        {
            var page = locations.Skip(pageIndex * Plugin.LocationsPerPage.Value)
                .Take(Plugin.LocationsPerPage.Value)
                .Select(location => new AnswerVisualData { id = location.zone })
                .ToList();

            AddNavigationAnswers(page, pageIndex, pageCount);
            Locations.Add(page);
        }
    }

    private static void AddNavigationAnswers(IList<AnswerVisualData> page, int pageIndex, int pageCount)
    {
        if (Plugin.EnablePreviousPageChoices.Value && pageIndex > 0)
        {
            if (Plugin.PreviousPageChoiceAtTop.Value)
            {
                page.Insert(0, GetPageAnswer(pageIndex - 1));
            }
            else
            {
                page.Add(GetPageAnswer(pageIndex - 1));
            }
        }

        if (pageIndex < pageCount - 1)
        {
            page.Add(GetPageAnswer(pageIndex + 1));
        }

        page.Add(CancelAnswer);
    }


    private static AnswerVisualData GetPageAnswer(int pageIndex)
    {
        return pageIndex switch
        {
            0 => Page1Answer,
            1 => Page2Answer,
            2 => Page3Answer,
            3 => Page4Answer,
            4 => Page5Answer,
            5 => Page6Answer,
            6 => Page7Answer,
            7 => Page8Answer,
            8 => Page9Answer,
            9 => Page10Answer,
            _ => throw new ArgumentOutOfRangeException(nameof(pageIndex), @"Invalid page index")
        };
    }


    internal static void LoadCustomZones()
    {
        try
        {
            var path = Location.GetSavePath();
            Plugin.Log.LogInfo($"Custom zones save path: {path}");

            Directory.CreateDirectory(path); // CreateDirectory does nothing if the directory already exists
            Plugin.Log.LogInfo("Checked or created the directory.");

            var files = Directory.GetFiles(path, "*.json", SearchOption.AllDirectories);
            Plugin.Log.LogInfo($"Found {files.Length} JSON files in path: {path}");

            foreach (var file in files)
            {
                Plugin.Log.LogInfo($"Processing file: {file}");
                try
                {
                    var location = Location.LoadFromJson(file);
                    if (location == null)
                    {
                        Plugin.Log.LogWarning($"Failed to load location from file: {file}");
                        continue;
                    }

                    Plugin.Log.LogInfo($"Loaded location from file: {file}, Zone: {location.zone}");

                    if (!string.IsNullOrWhiteSpace(location.zone))
                    {
                        var exists = AllLocations.Any(a => a.zone.Equals(location.zone, StringComparison.OrdinalIgnoreCase));
                        if (exists)
                        {
                            Plugin.Log.LogWarning($"Custom location '{location.zone}' already exists in the list, skipping...");
                            continue;
                        }

                        AllLocations.Add(location);
                        Plugin.Log.LogInfo($"Added location '{location.zone}' to the list.");
                    }
                    else
                    {
                        Plugin.Log.LogWarning($"Location zone is null or empty in file: {file}");
                    }
                }
                catch (Exception ex)
                {
                    Plugin.Log.LogError($"Error processing file: {file}. Exception: {ex}");
                }
            }
        }
        catch (Exception ex)
        {
            Plugin.Log.LogError($"Error in LoadCustomZones: {ex}");
        }
    }
}