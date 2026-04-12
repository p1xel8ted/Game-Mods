namespace MaxButtonsRedux;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.maxbuttonsredux";
    private const string PluginName = "Max Buttons Redux";
    private const string PluginVer = "1.3.11";

    internal static ManualLogSource Log { get; private set; }

    internal const string VendorGui = "VendorGUI";
    internal const string InventoryGui = "InventoryGUI";
    internal const string ChestGui = "ChestGUI";

    internal static bool _craftGuiOpen;
    internal static bool _itemCountGuiOpen;
    internal static CraftItemGUI _craftItemGui;
    internal static WorldGameObject _crafteryWgo;
    internal static SmartSlider _slider;
    internal static bool _unsafeInteraction;

    internal static readonly string[] UnSafeCraftObjects =
    [
        "mf_crematorium_corp", "garden_builddesk", "tree_garden_builddesk", "mf_crematorium", "grave_ground",
        "tile_church_semicircle_2floors", "mf_grindstone_1", "zombie_garden_desk_1", "zombie_garden_desk_2",
        "zombie_garden_desk_3",
        "zombie_vineyard_desk_1", "zombie_vineyard_desk_2", "zombie_vineyard_desk_3", "graveyard_builddesk",
        "blockage_H_low", "blockage_V_low",
        "blockage_H_high", "blockage_V_high", "wood_obstacle_v", "refugee_camp_garden_bed", "refugee_camp_garden_bed_1",
        "refugee_camp_garden_bed_2",
        "refugee_camp_garden_bed_3"
    ];

    internal static readonly string[] UnSafeCraftZones =
    [
        "church"
    ];

    internal static readonly string[] UnSafePartials =
    [
        "blockage", "obstacle", "builddesk", "fix", "broken"
    ];

    private void Awake()
    {
        Log = Logger;
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
    }

    internal static bool IsUpdateConditionsMet()
    {
        return MainGame.game_started && !MainGame.me.player.is_dead && !MainGame.me.player.IsDisabled() && FloatingWorldGameObject.cur_floating == null;
    }

    internal static void HandleGamepadInput()
    {
        if (!LazyInput.gamepad_active) return;

        var player = ReInput.players.GetPlayer(0);
        if (_itemCountGuiOpen)
        {
            HandleItemCountGuiInput(player);
        }
        else if (_craftGuiOpen && !_unsafeInteraction)
        {
            HandleCraftGuiInput(player);
        }
    }

    private static void HandleItemCountGuiInput(Player player)
    {
        if (player.GetButtonDown(19))
        {
            _slider.SetValue(_slider._max);
        }

        if (player.GetButtonDown(20))
        {
            _slider.SetValue(_slider._min);
        }
    }

    private static void HandleCraftGuiInput(Player player)
    {
        if (IsCraftGuiInputValid()) return;

        if (player.GetButtonDown(19))
        {
            MaxButtonCrafting.SetMaximumAmount(_craftItemGui, _crafteryWgo);
        }

        if (player.GetButtonDown(20))
        {
            MaxButtonCrafting.SetMinimumAmount(_craftItemGui);
        }
    }

    private static bool IsCraftGuiInputValid()
    {
        return _craftItemGui.current_craft.needs.Any(need => need.is_multiquality) || _craftItemGui.current_craft.one_time_craft;
    }
}
