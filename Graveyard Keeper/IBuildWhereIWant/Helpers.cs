namespace IBuildWhereIWant;

public partial class Plugin
{
    private static WorldGameObject BuildDesk { get; set; }
    private static WorldGameObject BuildDeskClone { get; set; }

    private static CraftsInventory CraftsInventory { get; set; }

    private static Dictionary<string, string> CraftDictionary { get; set; } = new();
    private const string Zone = "mf_wood";

    private const string BuildDeskConst = "buildanywhere_desk";

    private static int UnlockedCraftListCount { get; set; }

    private static string GetLocalizedString(string content)
    {
        Thread.CurrentThread.CurrentUICulture = CrossModFields.Culture;
        return content;
    }

    private static void OpenCraftAnywhere()
    {
        if (MainGame.me.player.GetMyWorldZoneId().Contains("refugee")) return;
        if (MainGame.me.player.GetParamInt("in_tutorial") == 1 &&
            MainGame.me.player.GetParamInt("tut_shown_tut_1") == 0)
        {
            MainGame.me.player.Say("cant_do_it_now");
            return;
        }

        CraftsInventory ??= new CraftsInventory();

        CraftDictionary ??= new Dictionary<string, string>();

        if (BuildDesk == null)
        {
            BuildDesk = FindObjectsOfType<WorldGameObject>(true)
                .FirstOrDefault(x => string.Equals(x.obj_id, "mf_wood_builddesk"));
        }

        WriteLog(
            BuildDesk != null
                ? $"Found Build Desk: {BuildDesk}, Zone: {BuildDesk.GetMyWorldZone()}"
                : "Unable to locate a build desk.", BuildDesk == null);

        if (BuildDeskClone != null)
        {
            Destroy(BuildDeskClone);
        }

        BuildDeskClone = Instantiate(BuildDesk);

        BuildDeskClone.name = BuildDeskConst;

        var needsRefresh = false;
        if (MainGame.me.save.unlocked_crafts.Count > UnlockedCraftListCount)
        {
            UnlockedCraftListCount = MainGame.me.save.unlocked_crafts.Count;
            needsRefresh = true;
        }

        if (needsRefresh)
        {
            foreach (var objectCraftDefinition in GameBalance.me.craft_obj_data.Where(x =>
                             x.build_type == ObjectCraftDefinition.BuildType.Put)
                         .Where(a => a.icon.Length > 0)
                         .Where(b => !b.id.Contains("refugee"))
                         .Where(d => MainGame.me.save.IsCraftVisible(d))
                         .Where(e => !CraftDictionary.TryGetValue(GJL.L(e.GetNameNonLocalized()), out _)))

            {
                var itemName = GJL.L(objectCraftDefinition.GetNameNonLocalized());
                CraftDictionary.Add(itemName, objectCraftDefinition.id);
            }


            var craftList = CraftDictionary.ToList();
            craftList.Sort((pair1, pair2) => string.CompareOrdinal(pair1.Key, pair2.Key));

            craftList.ForEach(craft => { CraftsInventory.AddCraft(craft.Value); });
        }

        CrossModFields.CraftAnywhere = true;

        BuildModeLogics.last_build_desk = BuildDeskClone;

        MainGame.me.build_mode_logics.SetCurrentBuildZone(BuildDeskClone.obj_def.zone_id, "");
        GUIElements.me.craft.OpenAsBuild(BuildDeskClone, CraftsInventory);
        MainGame.paused = false;
    }

    private static void WriteLog(string message, bool error = false)
    {
        if (error)
        {
            Log.LogError($"{message}");
        }
        else
        {
            if (Debug.Value)
            {
                Log.LogInfo($"{message}");
            }
        }
    }
}