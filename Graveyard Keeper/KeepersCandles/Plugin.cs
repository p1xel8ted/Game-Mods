namespace KeepersCandles;

[Harmony]
[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInDependency("p1xel8ted.gyk.gykhelper", "3.1.0")]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.keeperscandles";
    private const string PluginName = "Keeper's Candles!";
    private const string PluginVer = "0.1.4";
    private const string Souls = "souls";
    private const string Candelabrum = "candelabrum";
    private const string Column = "column";
    private const string Church = "CHURCH";
    
    private readonly static List<GameObject> ChurchColumnsList = [];
    private static ManualLogSource LOG { get; set; }
    private static ConfigEntry<float> ExtinguishDistance { get; set; }
    private static ConfigEntry<KeyboardShortcut> ExtinguishKeyBind { get; set; }
    private static ConfigEntry<string> ExtinguishControllerButton { get; set; }
    private static ConfigEntry<bool> DirectionalArrow { get; set; }
    private static ConfigEntry<bool> ChurchColumns { get; set; }
    private static Vector2 PlayerPosition => MainGame.me.player.grid_pos;

    private void Awake()
    {
        LOG = Logger;
        SceneManager.sceneLoaded += (_, _) => OnGameBalanceLoaded();
        Actions.GameBalanceLoad += OnGameBalanceLoaded;
        Actions.GameStartedPlaying += OnGameBalanceLoaded;

        ExtinguishDistance = Config.Bind("01. Distance", "Extinguish Distance", 1f, new ConfigDescription("Distance in units to extinguish a candle.", new AcceptableValueRange<float>(1, 5), new ConfigurationManagerAttributes {Order = 19}));
        ExtinguishDistance.SettingChanged += (_, _) =>
        {
            //clamp to 0.25 increments
            ExtinguishDistance.Value = Mathf.Round(ExtinguishDistance.Value * 4) / 4;
        };

        DirectionalArrow = Config.Bind("01. Distance", "Directional Arrow", true, new ConfigDescription("Display an arrow that will point to the nearest candle you can extinguish.", null, new ConfigurationManagerAttributes {Order = 20}));
        DirectionalArrow.SettingChanged += (_, _) => ResetArrow();

        ChurchColumns = Config.Bind("02. Features", "Church Columns", true, new ConfigDescription("Toggle church column visibility.", null, new ConfigurationManagerAttributes {Order = 16}));
        ChurchColumns.SettingChanged += (_, _) => ChurchColumnsToggle();

        ExtinguishKeyBind = Config.Bind("03. Keybinds", "Extinguish Keybind", new KeyboardShortcut(KeyCode.C), new ConfigDescription("Keybind to extinguish a candle.", null, new ConfigurationManagerAttributes {Order = 18}));
        ExtinguishControllerButton = Config.Bind("03. Keybinds", "Extinguish Controller Button", Enum.GetName(typeof(GamePadButton), GamePadButton.DUp), new ConfigDescription("Select the controller button used to extinguish a candle.", new AcceptableValueList<string>(Enum.GetNames(typeof(GamePadButton))), new ConfigurationManagerAttributes {Order = 17}));

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        LOG.LogInfo($"Plugin {PluginName} is loaded!");
    }

    private void Update()
    {
        if (!CanFindCandles()) return;

        if ((!LazyInput.gamepad_active || !ReInput.players.GetPlayer(0).GetButtonDown(ExtinguishControllerButton.Value)) && !ExtinguishKeyBind.Value.IsUp()) return;

        WorldGameObject closestCandle = null;
        var closestDistance = float.MaxValue;

        foreach (var candle in GetCandles())
        {
            var distance = Vector3.Distance(candle.grid_pos, PlayerPosition);

            if (!(distance < closestDistance)) continue;

            closestDistance = distance;
            closestCandle = candle;
        }

        if (closestCandle)
        {
            if (closestDistance <= ExtinguishDistance.Value)
            {
                var unlitCandle = GetUnlitCandle(closestCandle.components.craft.last_craft_id);
                if (unlitCandle.IsNullOrWhiteSpace())
                {
                    LOG.LogError($"Could not find unlit candle for {closestCandle.obj_id}. Last craft ID: {closestCandle.components.craft.last_craft_id}. Please report this!");
                    ResetArrow();
                    return;
                }
                ResetArrow();
                ReplaceAndDrop(closestCandle, unlitCandle);
            }
            else
            {
                SetArrow(closestCandle);
                Tools.ShowMessage(LangDicts.GetMessage(LangDicts.Messages.TooFar), PlayerPosition, sayAsPlayer: true, time: 1.5f);
            }
        }
        else
        {
            Tools.ShowMessage(LangDicts.GetMessage(LangDicts.Messages.NoneFound), PlayerPosition, sayAsPlayer: true, time: 1.5f);
        }
    }

    private static void ChurchColumnsToggle()
    {
        foreach (var column in ChurchColumnsList.ToList().Where(column => column))
        {
            column.gameObject.SetActive(ChurchColumns.Value);
        }
    }

    private static void ResetArrow()
    {
        GUIElements.me.tutorial_arrow.SetActive(false);
        GUIElements.me.tutorial_arrow._attached_wgo = null;
        GUIElements.me.tutorial_arrow._visible = false;
    }

    private static void SetArrow(WorldGameObject wgo)
    {
        if (!DirectionalArrow.Value)
        {
            ResetArrow();
            return;
        }

        GUIElements.me.tutorial_arrow.Init();
        GUIElements.me.tutorial_arrow.AttachToWGO(wgo);
        GUIElements.me.tutorial_arrow._visible = true;
    }

    private static void OnGameBalanceLoaded()
    {
        try
        {
            foreach (var obj in GameBalance._instance.craft_data.Where(obj => ShouldProcess(obj.id)))
            {
                obj.dur_needs_item = 0f;
                obj.dur_parameter = 0f;
                obj.can_craft_always = true;
            }

            foreach (var obj in GameBalance._instance.objs_data.Where(obj => ShouldProcess(obj.id)))
            {
                obj.durability_modificator = 0f;
                obj.always_active = true;
            }

            foreach (var wgo in WorldMap._objs.Where(wgo => ShouldProcess(wgo.obj_id) || ShouldProcess(wgo.obj_def.id)))
            {
                wgo.obj_def.durability_modificator = 0f;
                wgo.obj_def.always_active = true;
            }

            FixCandles();
            ChurchColumnsToggle();
        }
        catch (Exception)
        {
            //
        }
    }

    private static string GetUnlitCandle(string input)
    {
        if (!input.Contains("_to_")) return string.Empty;

        var parts = input.Split(["_to_"], StringSplitOptions.None);
        return parts[0];
    }

    private static List<WorldGameObject> GetCandles()
    {
        var zone = MainGame.me.player.GetMyWorldZone();

        var allCandles = zone ? zone.GetZoneWGOs().Where(wgo => ShouldProcess(wgo.obj_id) || ShouldProcess(wgo.obj_def.id)).ToList() : WorldMap._objs.Where(wgo => ShouldProcess(wgo.obj_id) || ShouldProcess(wgo.obj_def.id)).ToList();

        return allCandles.Where(wgo => wgo.components.craft.is_crafting).ToList();
    }

    private static void ReplaceAndDrop(WorldGameObject wgo, string unlitCandle)
    {
        wgo.ReplaceWithObject(unlitCandle, true);
        wgo.components.craft.is_crafting = false;

        var candleItem = wgo.components.craft._cur_craft_items_used.FirstOrDefault();
        if (candleItem != null)
        {
            DropResGameObject.Drop(wgo.tf.position, candleItem, wgo.tf.parent, Direction.ToPlayer, 3f, Random.Range(0, 2), force_stacked_drop: true);
        }
        else
        {
            LOG.LogError($"Could not find candle item used for {wgo.obj_id}. Please report this!");
        }
    }

    private static bool CanFindCandles()
    {
        return MainGame.game_started &&
               !MainGame.me.player.is_dead &&
               !MainGame.me.player.IsDisabled() &&
               !MainGame.paused &&
               BaseGUI.all_guis_closed;
    }
    private static bool ShouldProcess(string id)
    {
        return !id.Contains(Souls) && id.Contains(Candelabrum);
    }

    private static void FixCandles()
    {
        foreach (var wgo in WorldMap._objs.Where(wgo => ShouldProcess(wgo.obj_id) || ShouldProcess(wgo.obj_def.id)))
        {
            var split = wgo.obj_id.Split([Candelabrum], StringSplitOptions.None);
            var postfix = split.Last();
            var underscoreCount = postfix.Count(c => c == '_');
            if (underscoreCount < 2) continue;
            LOG.LogInfo($"Correcting '{wgo.obj_id}' crafting status.");
            wgo.components.craft.is_crafting = true;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(ChunkedGameObject), nameof(ChunkedGameObject.Init))]
    public static void ChunkedGameObject_Init(ChunkedGameObject __instance)
    {
        var name = __instance.name;
        var path = GetPath(__instance.transform);
        if (name.Contains(Column) && path.Contains(Church))
        {
            ChurchColumnsList.Add(__instance.gameObject);
        }
        
        ChurchColumnsToggle();
    }

    private static string GetPath(Transform transform)
    {
        var path = transform.name;
        while (transform.parent)
        {
            transform = transform.parent;
            path = $"{transform.name}/{path}";
        }
        return path;
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(ChunkManager), nameof(ChunkManager.RescanAllObjects))]
    public static void ChunkManager_RescanAllObjects()
    {
        OnGameBalanceLoaded();
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameSave), nameof(GameSave.LateSaveFixer))]
    public static void GameSave_LateSaveFixer()
    {
        OnGameBalanceLoaded();
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(CraftComponent), nameof(CraftComponent.ReallyUpdateComponent))]
    public static bool CraftComponent_ReallyUpdateComponent(CraftComponent __instance)
    {
        return !__instance.wgo.obj_id.Contains(Candelabrum);
    }
}