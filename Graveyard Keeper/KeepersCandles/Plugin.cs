namespace KeepersCandles;

[Harmony]
[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInDependency("p1xel8ted.gyk.gykhelper", "3.1.0")]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.keeperscandles";
    private const string PluginName = "Keeper's Candles!";
    private const string PluginVer = "0.1.1";
    private const string Remove = "remove";
    private const string Souls = "souls";
    private readonly static string[] Wicks = ["candelabrum"];

    private static ManualLogSource LOG { get; set; }
    private static ConfigEntry<int> ExtinguishDistance { get; set; }
    private static ConfigEntry<KeyboardShortcut> ExtinguishKeyBind { get; set; }
    private static ConfigEntry<string> ExtinguishControllerButton { get; set; }

    private static Vector2 PlayerPosition => MainGame.me.player.grid_pos;

    private void Awake()
    {
        LOG = Logger;
        SceneManager.sceneLoaded += (_, _) => OnGameBalanceLoaded();
        Actions.GameBalanceLoad += OnGameBalanceLoaded;
        Actions.GameStartedPlaying += OnGameBalanceLoaded;

        ExtinguishDistance = Config.Bind("01. Distance", "Extinguish Distance", 1, new ConfigDescription("Distance in units to extinguish a candle.", null, new ConfigurationManagerAttributes {Order = 19}));
        ExtinguishKeyBind = Config.Bind("02. Keybinds", "Extinguish Keybind", new KeyboardShortcut(KeyCode.C), new ConfigDescription("Keybind to extinguish a candle.", null, new ConfigurationManagerAttributes {Order = 18}));
        ExtinguishControllerButton = Config.Bind("02. Keybinds", "Extinguish Controller Button", Enum.GetName(typeof(GamePadButton), GamePadButton.DUp), new ConfigDescription("Select the controller button used to extinguish a candle.", new AcceptableValueList<string>(Enum.GetNames(typeof(GamePadButton))), new ConfigurationManagerAttributes {Order = 17}));

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
                    return;
                }

                ReplaceAndDrop(closestCandle, unlitCandle);
            }
            else
            {
                Tools.ShowMessage(LangDicts.GetMessage(LangDicts.Messages.TooFar), PlayerPosition, sayAsPlayer: true);
            }
        }
        else
        {
            Tools.ShowMessage(LangDicts.GetMessage(LangDicts.Messages.NoneFound), PlayerPosition, sayAsPlayer: true);
        }
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
        return !id.Contains(Souls) && Wicks.Any(id.Contains);
    }

    private static void FixCandles()
    {
        foreach (var wgo in WorldMap._objs.Where(wgo => ShouldProcess(wgo.obj_id) || ShouldProcess(wgo.obj_def.id)))
        {
            var split = wgo.obj_id.Split(["candelabrum"], StringSplitOptions.None);
            var postfix = split.Last();
            var underscoreCount = postfix.Count(c => c == '_');
            if (underscoreCount < 2) continue;
            LOG.LogInfo($"Correcting '{wgo.obj_id}' crafting status.");
            wgo.components.craft.is_crafting = true;
            var remove = $"{wgo.obj_id}_{Remove}";
            var craftDef = GameBalance.me.GetData<CraftDefinition>(remove);
            if(craftDef == null)
            {
                LOG.LogError($"Could not find craft definition for '{remove}' - Please report this.");
                continue;
            }
            wgo.components.craft.current_craft = craftDef;
        }
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
        if (!__instance.other_obj || !__instance.other_obj.is_player || __instance.current_craft == null) return true;
        return !(ShouldProcess(__instance.wgo.obj_id) || ShouldProcess(__instance.current_craft.id));
    }
}