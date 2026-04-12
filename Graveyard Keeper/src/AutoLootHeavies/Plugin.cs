namespace AutoLootHeavies;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.autolootheavies";
    private const string PluginName = "Auto-Loot Heavies!";
    private const string PluginVer = "3.5.3";

    private const float EnergyRequirement = 3f;

    internal static ManualLogSource Log { get; private set; }

    internal static List<Stockpile> SortedStockpiles { get; } = [];
    internal static float LastBubbleTime { get; set; }
    internal static List<WorldGameObject> Objects { get; set; }
    internal static ConfigEntry<bool> TeleportToDumpSiteWhenAllStockPilesFull { get; private set; }
    internal static ConfigEntry<Vector3> DesignatedTimberLocation { get; private set; }
    internal static ConfigEntry<Vector3> DesignatedOreLocation { get; private set; }
    internal static ConfigEntry<Vector3> DesignatedStoneLocation { get; private set; }
    internal static ConfigEntry<bool> ImmersionMode { get; private set; }
    internal static ConfigEntry<bool> Debug { get; private set; }
    internal static bool DebugEnabled;
    internal static bool DebugDialogShown;
    internal static ConfigEntry<KeyboardShortcut> SetTimberLocationKeybind { get; private set; }
    internal static ConfigEntry<KeyboardShortcut> SetOreLocationKeybind { get; private set; }
    internal static ConfigEntry<KeyboardShortcut> SetStoneLocationKeybind { get; private set; }
    internal static ConfigEntry<KeyboardShortcut> TeleportToggleKeybind { get; private set; }

    internal static bool InitialFullUpdate { get; set; }

    private void Awake()
    {
        Log = Logger;
        LogHelper.Log = Logger;
        InitConfiguration();
        Lang.Init(Assembly.GetExecutingAssembly(), Log);
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
    }

    private void InitConfiguration()
    {
        TeleportToDumpSiteWhenAllStockPilesFull = Config.Bind("1. Features", "Teleport To Dump Site When Full", true, new ConfigDescription("Teleport resources to a designated dump site when all stockpiles are full", null, new ConfigurationManagerAttributes {Order = 9}));
        ImmersionMode = Config.Bind("1. Features", "Immersive Mode", true, new ConfigDescription("Disable immersive mode to remove energy requirements for teleportation", null, new ConfigurationManagerAttributes {Order = 8}));

        DesignatedTimberLocation = Config.Bind("2. Locations", "Designated Timber Location", new Vector3(-3712.003f, 6144f, 1294.643f), new ConfigDescription("Set the designated location for dumping excess timber", null, new ConfigurationManagerAttributes {Order = 7}));
        DesignatedOreLocation = Config.Bind("2. Locations", "Designated Ore Location", new Vector3(-3712.003f, 6144f, 1294.643f), new ConfigDescription("Set the designated location for dumping excess ore", null, new ConfigurationManagerAttributes {Order = 6}));
        DesignatedStoneLocation = Config.Bind("2. Locations", "Designated Stone Location", new Vector3(-3712.003f, 6144f, 1294.643f), new ConfigDescription("Set the designated location for dumping excess stone and marble", null, new ConfigurationManagerAttributes {Order = 5}));

        SetTimberLocationKeybind = Config.Bind("3. Keybinds", "Set Timber Location Keybind", new KeyboardShortcut(KeyCode.Alpha7), new ConfigDescription("Define the keybind for setting the Timber Location", null, new ConfigurationManagerAttributes {Order = 4}));
        SetOreLocationKeybind = Config.Bind("3. Keybinds", "Set Ore Location Keybind", new KeyboardShortcut(KeyCode.Alpha8), new ConfigDescription("Define the keybind for setting the Ore Location", null, new ConfigurationManagerAttributes {Order = 3}));
        SetStoneLocationKeybind = Config.Bind("3. Keybinds", "Set Stone Location Keybind", new KeyboardShortcut(KeyCode.Alpha9), new ConfigDescription("Define the keybind for setting the Stone Location", null, new ConfigurationManagerAttributes {Order = 2}));
        TeleportToggleKeybind = Config.Bind("3. Keybinds", "Toggle Teleport Keybind", new KeyboardShortcut(KeyCode.Alpha6), new ConfigDescription("Define the keybind for toggling teleport to dump site when all stockpiles are full", null, new ConfigurationManagerAttributes {Order = 1}));

        Debug = Config.Bind("4. Advanced", "Debug Logging", false, new ConfigDescription("Toggle debug logging on or off", null, new ConfigurationManagerAttributes {IsAdvanced = true, Order = 1}));
        DebugEnabled = Debug.Value;
        Debug.SettingChanged += (_, _) => DebugEnabled = Debug.Value;
    }

    internal static void ShowDebugWarningOnce()
    {
        if (!DebugEnabled || DebugDialogShown) return;
        DebugDialogShown = true;
        Lang.Reload();
        GUIElements.me.dialog.OpenOK(PluginName, null, Lang.Get("DebugWarning"), true, string.Empty);
    }

    internal static void WriteLog(string message, bool error = false)
    {
        if (error)
        {
            LogHelper.Error(message);
        }
        else
        {
            LogHelper.Info(message);
        }
    }

    internal static void DropObjectAndNull(BaseCharacterComponent __instance, Item item)
    {
        DropResGameObject.Drop(__instance.tf.position, item,
            __instance.tf.parent,
            __instance.anim_direction,
            3f,
            Random.Range(0, 2), false);

        __instance.SetOverheadItem(null);
    }

    private static (int, int) GetGridLocation()
    {
        const int horizontal = 30;
        const int vertical = 5;
        var tupleList = new List<(int, int)>(horizontal * vertical);

        for (var x = 0; x < vertical; ++x)
        {
            for (var y = 0; y < horizontal; ++y)
            {
                tupleList.Add((x, y));
            }
        }

        var spot = tupleList.RandomElement();
        return spot;
    }

    internal static void ShowLootAddedIcon(Item item)
    {
        var originalSize = item.definition.item_size;
        item.definition.item_size = 1;
        DropCollectGUI.OnDropCollected(item);
        item.definition.item_size = originalSize;
        Sounds.PlaySound("pickup", null, true);
    }

    internal static void TeleportItem(BaseCharacterComponent __instance, Item item)
    {
        var pwo = MainGame.me.player;
        var needEnergy = !ImmersionMode.Value || pwo.IsPlayerInvulnerable() ? 0f : EnergyRequirement;

        if (pwo.energy >= needEnergy)
        {
            pwo.energy -= needEnergy;
            EffectBubblesManager.ShowStackedEnergy(pwo, -needEnergy);

            var loc = GetGridLocation();
            float xAdjustment = loc.Item1 * 75;

            var timber = DesignatedTimberLocation.Value + new Vector3(xAdjustment, 0, 0);
            var ore = DesignatedOreLocation.Value + new Vector3(xAdjustment, 0, 0);
            var stone = DesignatedStoneLocation.Value + new Vector3(xAdjustment, 0, 0);

            var location = item.id switch
            {
                Constants.ItemDefinitionId.Wood => timber,
                Constants.ItemDefinitionId.Ore => ore,
                Constants.ItemDefinitionId.Stone => stone,
                Constants.ItemDefinitionId.Marble => stone,
                _ => MainGame.me.player_pos
            };

            MainGame.me.player.DropItem(item, Direction.IgnoreDirection, location, 0f, false);
            if (Plugin.DebugEnabled) WriteLog($"Teleporting {item.id} to dump site.");
            __instance.SetOverheadItem(null);
        }
        else
        {
            DropObjectAndNull(__instance, item);

            if (Time.time - LastBubbleTime >= 0.5f)
            {
                LastBubbleTime = Time.time;

                EffectBubblesManager.ShowImmediately(pwo.bubble_pos,
                    GJL.L("not_enough_something", $"(en)"),
                    EffectBubblesManager.BubbleColor.Energy, true, 1f);
                if (Plugin.DebugEnabled) WriteLog($"Not enough energy to teleport. Dropping.");
            }
        }
    }

    internal static bool TryPutToInventoryAndNull(BaseCharacterComponent __instance, WorldGameObject wgo, List<Item> itemsToInsert)
    {
        var pwo = MainGame.me.player;
        var needEnergy = !ImmersionMode.Value || pwo.IsPlayerInvulnerable() ? 0f : EnergyRequirement;

        if (pwo.energy >= needEnergy)
        {
            wgo.TryPutToInventory(itemsToInsert, out var failed);
            if (failed.Count == 0)
            {
                pwo.energy -= needEnergy;
                EffectBubblesManager.ShowStackedEnergy(pwo, -needEnergy);
                __instance.SetOverheadItem(null);
                return true;
            }
        }
        else if (Time.time - LastBubbleTime > 0.5f)
        {
            LastBubbleTime = Time.time;
            EffectBubblesManager.ShowImmediately(pwo.bubble_pos,
                GJL.L("not_enough_something", $"(en)"),
                EffectBubblesManager.BubbleColor.Energy, true, 1f);
        }

        return false;
    }

    internal struct Constants
    {
        public struct ItemDefinitionId
        {
            public const string Marble = "marble";
            public const string Ore = "ore_metal";
            public const string Stone = "stone";
            public const string Wood = "wood";
        }

        public struct ItemObjectId
        {
            public const string Ore = "mf_ore_1";
            public const string Stone = "mf_stones_1";
            public const string Timber = "mf_timber_1";
        }
    }

    internal static void RemoveStockpile(WorldGameObject wgo)
    {
        var stockpile = SortedStockpiles.Find(a => a.Wgo == wgo);
        if (stockpile != null)
        {
            if (Plugin.DebugEnabled) WriteLog($"Removed stockpile: location: {stockpile.Location}, type: {stockpile.Type}, distance: {stockpile.DistanceFromPlayer}");
            SortedStockpiles.Remove(stockpile);
        }
        else
        {
            WriteLog($"Error removing stockpile (null??).", error: true);
        }
    }

    private static Vector3 GetLocation(WorldGameObject wgo)
    {
        return new Vector3((float) Math.Ceiling(wgo.pos3.x), (float) Math.Ceiling(wgo.pos3.y),
            (float) Math.Ceiling(wgo.pos3.z));
    }

    private static float GetDistance(WorldGameObject wgo)
    {
        return Vector3.Distance(MainGame.me.player_pos, wgo.pos3);
    }

    private static Stockpile.StockpileType GetStockpileType(WorldGameObject wgo)
    {
        return wgo.obj_id switch
        {
            { } id when id.Contains(Constants.ItemObjectId.Ore) => Stockpile.StockpileType.Ore,
            { } id when id.Contains(Constants.ItemObjectId.Stone) => Stockpile.StockpileType.Stone,
            { } id when id.Contains(Constants.ItemObjectId.Timber) => Stockpile.StockpileType.Timber,
            _ => Stockpile.StockpileType.Unknown,
        };
    }

    internal static bool AddStockpile(WorldGameObject stockpile)
    {
        var exists = SortedStockpiles.Find(a => a.Wgo == stockpile);
        if (exists != null)
        {
            exists.DistanceFromPlayer = GetDistance(stockpile);
            return false;
        }

        var newStockpile = new Stockpile
        (
            GetLocation(stockpile),
            GetStockpileType(stockpile),
            GetDistance(stockpile),
            stockpile
        );

        SortedStockpiles.Add(newStockpile);
        return true;
    }

    internal static bool OverheadItemIsHeavy(Item item)
    {
        return item.id is Constants.ItemDefinitionId.Wood or
            Constants.ItemDefinitionId.Ore or
            Constants.ItemDefinitionId.Stone or
            Constants.ItemDefinitionId.Marble;
    }

    internal static IEnumerator RunFullUpdate()
    {
        if (!MainGame.game_started) yield break;

        var sw = new Stopwatch();
        sw.Start();

        SortedStockpiles.Clear();

        var allObjects = WorldMap._objs;
        Objects = allObjects!
            .Where(x => x.obj_id.Contains(Constants.ItemObjectId.Timber) || x.obj_id.Contains(Constants.ItemObjectId.Ore) ||
                        x.obj_id.Contains(Constants.ItemObjectId.Stone))
            .Where(x => x.data.inventory_size > 0)
            .ToList();

        if (Plugin.DebugEnabled) WriteLog($"[ALH]: Scanning world for stockpiles.");
        Objects.RemoveAll(a => a.obj_id.Contains("decor"));

        foreach (var obj in Objects)
        {
            if (Plugin.DebugEnabled) WriteLog($"Found stockpile: location: {GetLocation(obj)}, type: {GetStockpileType(obj)}, distance: {GetDistance(obj)}");
        }

        if (Plugin.DebugEnabled) WriteLog($"[ALH]: Updating stockpiles distance, type etc and sorting by distance to player.");

        foreach (var stockpile in Objects.Where(a => a != null))
        {
            if (Plugin.DebugEnabled) WriteLog(AddStockpile(stockpile) ? $"Added stockpile: location: {GetLocation(stockpile)}, type: {GetStockpileType(stockpile)}, distance: {GetDistance(stockpile)}" : $"Stockpile already exists in list - updating distance from player.");
        }

        SortedStockpiles.Sort((x, y) => x.DistanceFromPlayer.CompareTo(y.DistanceFromPlayer));

        sw.Stop();
        if (Plugin.DebugEnabled) WriteLog($"Scanning, updating, and sorting stockpiles took {sw.ElapsedMilliseconds}ms");
    }

    internal static void ShowMessage(string msg, Vector3 pos)
    {
        if (GJL.IsEastern())
        {
            MainGame.me.player.Say(msg, null, false, SpeechBubbleGUI.SpeechBubbleType.Think,
                SmartSpeechEngine.VoiceID.None, true);
        }
        else
        {
            var newPos = pos;
            if (newPos == Vector3.zero)
            {
                newPos = MainGame.me.player.pos3;
                newPos.y += 125f;
            }

            EffectBubblesManager.ShowImmediately(newPos, msg,
                EffectBubblesManager.BubbleColor.Relation,
                true, 3f);
        }
    }

    internal static bool StockpileIsValid(WorldGameObject wgo)
    {
        return wgo.obj_id.Contains(Constants.ItemObjectId.Timber) ||
               wgo.obj_id.Contains(Constants.ItemObjectId.Ore) ||
               wgo.obj_id.Contains(Constants.ItemObjectId.Stone);
    }

    internal static void WorldGameObjectInteract(WorldGameObject obj)
    {
        if (StockpileIsValid(obj))
        {
            AddStockpile(obj);
        }
    }

    internal static void CheckKeybinds()
    {
        if (SetTimberLocationKeybind.Value.IsUp())
        {
            DesignatedTimberLocation.Value = MainGame.me.player_pos;
            ShowMessage(Lang.Get("DumpTimber"), DesignatedTimberLocation.Value);
        }

        if (SetOreLocationKeybind.Value.IsUp())
        {
            DesignatedOreLocation.Value = MainGame.me.player_pos;
            ShowMessage(Lang.Get("DumpOre"), DesignatedOreLocation.Value);
        }

        if (SetStoneLocationKeybind.Value.IsUp())
        {
            DesignatedStoneLocation.Value = MainGame.me.player_pos;
            ShowMessage(Lang.Get("DumpStone"), DesignatedStoneLocation.Value);
        }

        if (TeleportToggleKeybind.Value.IsUp())
        {
            TeleportToDumpSiteWhenAllStockPilesFull.Value = !TeleportToDumpSiteWhenAllStockPilesFull.Value;
            var state = TeleportToDumpSiteWhenAllStockPilesFull.Value ? "enabled" : "disabled";
            ShowMessage($"Teleport to dump site: {state}", MainGame.me.player.pos3);
        }
    }
}
