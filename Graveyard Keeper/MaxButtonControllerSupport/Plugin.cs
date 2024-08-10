namespace MaxButtonControllerSupport;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInDependency("p1xel8ted.gyk.gykhelper", "3.1.0")]
public partial class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.maxbuttoncontrollersupport";
    private const string PluginName = "Max Button Controller Support";
    private const string PluginVer = "1.3.7";

    private static ManualLogSource Log { get; set; }


    private void Awake()
    {
        Log = Logger;
        Actions.WorldGameObjectInteractPrefix += WorldGameObject_Interact;
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        Log.LogInfo($"Plugin {PluginName} is loaded!");
    }

    private void Update()
    {
        if (!IsUpdateConditionsMet()) return;

        HandleGamepadInput();
    }

    private static bool IsUpdateConditionsMet()
    {
        return MainGame.game_started && !MainGame.me.player.is_dead && !MainGame.me.player.IsDisabled() && FloatingWorldGameObject.cur_floating == null;
    }

    private static void HandleGamepadInput()
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
            //InvokeMaxButtonVendorMethod("SetMaxPrice", new object[] {_slider});
            _slider.SetValue(_slider._max);
        }

        if (player.GetButtonDown(20))
        {
           // InvokeMaxButtonVendorMethod("SetSliderValue", new object[] {_slider, 1});
            _slider.SetValue(_slider._min);
        }
    }

    private static void HandleCraftGuiInput(Player player)
    {
        if (IsCraftGuiInputValid()) return;

        if (player.GetButtonDown(19))
        {
            MaxButtonCrafting.SetMaximumAmount(ref _craftItemGui, ref _crafteryWgo);
        }

        if (player.GetButtonDown(20))
        {
            MaxButtonCrafting.SetMinimumAmount(ref _craftItemGui);
        }
    }

    private static bool IsCraftGuiInputValid()
    {
        return _craftItemGui.current_craft.needs.Any(need => need.is_multiquality) || _craftItemGui.current_craft.one_time_craft;
    }
}