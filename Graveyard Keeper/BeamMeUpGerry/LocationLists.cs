namespace BeamMeUpGerry;

public static class LocationLists
{
    internal readonly static List<Location> AllLocations =
    [
        new Location("zone_home", "house", "tp_house_b", new Vector2(29.5f, -68.4f), true, EnvironmentEngine.State.Inside), //tp_house_b
        new Location("zone_tavern", "tavern", "tp_tavern_b", new Vector2(52.5f, -87.5f), true, EnvironmentEngine.State.Inside), //tp_tavern_b
        new Location("@lighthouse", string.Empty, "tp_lighthouse_a", new Vector2(268.0f, -23.0f), true), //tp_lighthouse_a
        new Location("@quarry", "mining", "tp_mining_hut_b", new Vector2(49.0f, -122.9f), true, EnvironmentEngine.State.Inside), //tp_mining_hut_b
        new Location("@players_tavern", "players", "tp_players_tavern_front_b", new Vector2(203.5f, -97.5f), true, EnvironmentEngine.State.Inside), //tp_players_tavern_front_b
        new Location("@zone_nountain_fort", string.Empty, "tp_mountain_fort_point_a", new Vector2(203.0f, 39.0f), true), //tp_mountain_fort_point_a
        new Location("@zone_refugees_camp_tp", string.Empty, "tp_refugee_point_a", new Vector2(25.0f, 59.0f), true), //tp_refugee_point_a
        new Location("zone_witch_hut", string.Empty, string.Empty, new Vector2(-51.7f, -18.5f)),
        new Location("zone_cellar", "mortuary", string.Empty, new Vector2(112.9f, -96.3f), false, EnvironmentEngine.State.Inside),
        new Location("zone_alchemy", "mortuary", string.Empty, new Vector2(85.9f, -106.0f), false, EnvironmentEngine.State.Inside),
        new Location("zone_morgue", "mortuary", string.Empty, new Vector2(101.5f, -118.0f), false, EnvironmentEngine.State.Inside),
        new Location("zone_beegarden", string.Empty, string.Empty, new Vector2(33.7f, 18.9f)),
        new Location("zone_hill", string.Empty, string.Empty, new Vector2(86.4f, 14.5f)),
        new Location("zone_sacrifice", string.Empty, string.Empty, new Vector2(99.3f, -87.8f), false, EnvironmentEngine.State.Inside),
        new Location("zone_beatch", string.Empty, string.Empty, new Vector2(234.4f, 3.3f)),
        new Location("zone_vineyard", string.Empty, string.Empty, new Vector2(67.4f, 4.26f)),
        new Location("zone_camp", string.Empty, string.Empty, new Vector2(215.5f, 29.4f)),
        new Location("zone_souls", "mortuary", string.Empty, new Vector2(115.1f, -112.6f), false, EnvironmentEngine.State.Inside),
        new Location("zone_graveyard", string.Empty, string.Empty, new Vector2(17.0f, -15.7f)),
        new Location("zone_euric_room", "euric", string.Empty, new Vector2(209.5f, -120.8f), false, EnvironmentEngine.State.Inside),
        new Location("zone_church", "church", string.Empty, new Vector2(1.9f, -85.6f), false, EnvironmentEngine.State.Inside),
        new Location("zone_zombie_sawmill", string.Empty, string.Empty, new Vector2(23.0f, 35.5f)),
        new Location(strings.Coal, string.Empty, string.Empty, new Vector2(-5.3f, 63.5f)),
        new Location(strings.Clay, string.Empty, string.Empty, new Vector2(6.2f, -33.2f)),
        new Location(strings.Sand, string.Empty, string.Empty, new Vector2(3.5f, 9.1f)),
        new Location(strings.Mill, string.Empty, string.Empty, new Vector2(123.0f, -8.0f)),
        new Location(strings.Farmer, string.Empty, string.Empty, new Vector2(122.9f, -33.9f))
    ];

    private readonly static AnswerVisualData Page1Answer = new() {id = strings.Page_1};
    private readonly static AnswerVisualData Page2Answer = new() {id = strings.Page_2};
    private readonly static AnswerVisualData Page3Answer = new() {id = strings.Page_3};
    private readonly static AnswerVisualData Page4Answer = new() {id = strings.Page_4};
    private readonly static AnswerVisualData Page5Answer = new() {id = strings.Page_5};
    private readonly static AnswerVisualData Page6Answer = new() {id = strings.Page_6};
    private readonly static AnswerVisualData Page7Answer = new() {id = strings.Page_7};
    private readonly static AnswerVisualData Page8Answer = new() {id = strings.Page_8};
    private readonly static AnswerVisualData Page9Answer = new() {id = strings.Page_9};
    private readonly static AnswerVisualData Page10Answer = new() {id = strings.Page_10};
    private readonly static AnswerVisualData CancelAnswer = new() {id = Constants.Cancel};

    internal readonly static List<List<AnswerVisualData>> Locations = [];


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
        }
    }
    
    internal static void CreatePages()
    {
        LogData();
        
        Locations.Clear();

        var locations = AllLocations
            .Where(location => location.enabled)
            .ToList();

        if (Plugin.SortAlphabetically.Value)
        {
            locations = locations
                .OrderBy(location => Helpers.RemoveCharacters(location.zone))
                .ToList();
        }


        if (MainGame.me != null && MainGame.me.save != null)
        {
            locations = locations.Where(a => !Helpers.RemoveZone(a)).ToList();
        }

        var pageCount = Mathf.CeilToInt(locations.Count / (float) Plugin.LocationsPerPage.Value);

        for (var pageIndex = 0; pageIndex < pageCount; pageIndex++)
        {
            var page = locations.Skip(pageIndex * Plugin.LocationsPerPage.Value)
                .Take(Plugin.LocationsPerPage.Value)
                .Select(location => new AnswerVisualData {id = location.zone})
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
        var path = Location.GetSavePath();
        Directory.CreateDirectory(path); // CreateDirectory does nothing if the directory already exists

        var files = Directory.GetFiles(path, "*.json", SearchOption.AllDirectories);
        foreach (var file in files)
        {
            var location = Location.LoadFromJson(file);
            if (!string.IsNullOrWhiteSpace(location.zone))
            {
                var exists = AllLocations.Any(a => a.zone.Equals(location.zone, StringComparison.OrdinalIgnoreCase));
                if (exists)
                {
                    Plugin.Log.LogWarning($"Custom location '{location.zone}' already exists in the list, skipping...");
                    return;
                }
                AllLocations.Add(location);
            }
        }
    }
}