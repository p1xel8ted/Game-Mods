namespace CuisineerTweaks;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Plugin : BasePlugin
{
    private const string PluginGuid = "p1xel8ted.cuisineer.cuisineertweaks";
    private const string PluginName = "Cuisineer Tweaks (IL2CPP)";
    internal const string PluginVersion = "0.2.1";

    internal static ManualLogSource Logger { get; private set; }
    internal static ConfigEntry<bool> CorrectMainMenuAspect { get; private set; }
    internal static ConfigEntry<bool> CorrectFixedUpdateRate { get; private set; }
    internal static ConfigEntry<bool> UseRefreshRateForFixedUpdateRate { get; private set; }
    internal static ConfigEntry<bool> IncreaseStackSize { get; private set; }
    internal static ConfigEntry<int> IncreaseStackSizeValue { get; private set; }
    internal static ConfigEntry<bool> InstantText { get; private set; }
    internal static ConfigEntry<bool> EnableAutoSave { get; private set; }
    internal static ConfigEntry<bool> LoadToSaveMenu { get; private set; }
    internal static ConfigEntry<bool> AutoLoadSpecifiedSave { get; private set; }
    internal static ConfigEntry<int> AutoLoadSpecifiedSaveSlot { get; private set; }
    internal static ConfigEntry<bool> PauseTimeWhenViewingInventories { get; private set; }
    internal static ConfigEntry<bool> UnlockBothModSlots { get; private set; }
    internal static ConfigEntry<bool> InstantRestaurantUpgrades { get; private set; }
    internal static ConfigEntry<bool> InstantBrew { get; private set; }
    internal static ConfigEntry<bool> InstantWeaponUpgrades { get; private set; }
    internal static ConfigEntry<bool> ItemDropMultiplier { get; private set; }
    internal static ConfigEntry<int> ItemDropMultiplierValue { get; private set; }
    internal static ConfigEntry<bool> ModifyPlayerMaxHp { get; private set; }
    internal static ConfigEntry<float> ModifyPlayerMaxHpMultiplier { get; private set; }
    internal static ConfigEntry<bool> RegenPlayerHp { get; private set; }
    internal static ConfigEntry<int> RegenPlayerHpAmount { get; private set; }
    internal static ConfigEntry<float> RegenPlayerHpTick { get; private set; }
    internal static ConfigEntry<bool> RegenPlayerHpShowFloatingText { get; private set; }
    internal static ConfigEntry<bool> AutomaticPayment { get; private set; }
    internal static ConfigEntry<bool> IncreaseCustomerMoveSpeed { get; private set; }
    internal static ConfigEntry<bool> IncreaseCustomerMoveSpeedAnimation { get; private set; }
    internal static ConfigEntry<float> CustomerMoveSpeedValue { get; private set; }
    internal static ConfigEntry<bool> IncreasePlayerMoveSpeed { get; private set; }
    internal static ConfigEntry<float> PlayerMoveSpeedValue { get; private set; }
    internal static ConfigEntry<int> AutoSaveFrequency { get; private set; }
    internal static ConfigEntry<bool> RemoveChainAttackDelay { get; private set; }
    internal static ConfigEntry<bool> AutoCook { get; private set; }
    internal static ConfigEntry<bool> FreeCook { get; private set; }
    internal static ConfigEntry<bool> InstantKill { get; private set; }
    internal static ConfigEntry<bool> DineAndDash { get; private set; }
    internal static ConfigEntry<bool> ModifyWeaponSpecialCooldown { get; private set; }
    internal static ConfigEntry<float> WeaponSpecialCooldownValue { get; private set; }
    internal static ConfigEntry<bool> DebugMode { get; private set; }

    internal static ConfigEntry<bool> AllCustomersSelfServe { get; private set; }
    internal static Plugin Instance { get; private set; }
    internal static ConfigEntry<bool> OneHitDestructible { get; private set; }
    internal static ConfigEntry<bool> AutoReloadWeapons { get; private set; }
    internal static ConfigEntry<bool> AdjustableZoomLevel { get; private set; }
    internal static ConfigEntry<float> RelativeZoomAdjustment { get; private set; }

    internal static ConfigEntry<bool> UseStaticZoomLevel { get; private set; }
    internal static ConfigEntry<float> StaticZoomAdjustment { get; private set; }

    internal static ConfigEntry<bool> DisplayMessages { get; private set; }


    internal static ConfigEntry<KeyboardShortcut> KeybindReload { get; private set; }
    internal static ConfigEntry<KeyboardShortcut> KeybindSaveGame { get; private set; }
    internal static ConfigEntry<KeyboardShortcut> KeybindZoomIn{ get; private set; }
    internal static ConfigEntry<KeyboardShortcut> KeybindZoomOut { get; private set; }
    

    private void InitConfig()
    {
        // Group 0: General Settings
        DebugMode = Config.Bind("00. General", "DebugMode", false,
            new ConfigDescription("Enables debug mode, which enables more verbose logging. This is not recommended for normal use."));
        DisplayMessages = Config.Bind("00. General", "DisplayMessages", true,
            new ConfigDescription("Enables the display of messages in-game. Disable this if you don't want to see messages."));
        KeybindReload = Config.Bind("00. General", "KeybindReload", new KeyboardShortcut(KeyCode.R),
            new ConfigDescription("The keybind to reload the configuration file. List of valid entries here: https://docs.unity3d.com/ScriptReference/KeyCode.html"));
        KeybindSaveGame = Config.Bind("00. General", "KeybindSaveGame", new KeyboardShortcut(KeyCode.K),
            new ConfigDescription("The keybind to save the current game. List of valid entries here: https://docs.unity3d.com/ScriptReference/KeyCode.html"));
        KeybindZoomIn = Config.Bind("00. General", "KeybindZoomIn", new KeyboardShortcut(KeyCode.O),
            new ConfigDescription("The keybind to zoom in. List of valid entries here: https://docs.unity3d.com/ScriptReference/KeyCode.html"));
        KeybindZoomOut = Config.Bind("00. General", "KeybindZoomOut", new KeyboardShortcut(KeyCode.L),
            new ConfigDescription("The keybind to zoom out. List of valid entries here: https://docs.unity3d.com/ScriptReference/KeyCode.html"));
        
        
        // Group 1: Performance Settings
        CorrectFixedUpdateRate = Config.Bind("01. Performance", "CorrectFixedUpdateRate", true,
            new ConfigDescription("Adjusts the fixed update rate to minimum amount to reduce camera judder based on your refresh rate."));
        UseRefreshRateForFixedUpdateRate = Config.Bind("01. Performance", "UseRefreshRateForFixedUpdateRate", false,
            new ConfigDescription("Sets the fixed update rate based on the monitor's refresh rate for smoother gameplay. If you're playing on a potato, this may have performance impacts."));
        CorrectMainMenuAspect = Config.Bind("01. Performance", "CorrectMainMenuAspect", true,
            new ConfigDescription("Adjusts the main menu images to fit the screen. Will only be applied on aspect ratios wider than 16:9."));

        // Group 2: User Interface Enhancements
        InstantText = Config.Bind("02. User Interface", "InstantDialogueText", true,
            new ConfigDescription("Dialogue text is instantly displayed, skipping the typewriter effect."));

        // Group 3: Inventory Management
        IncreaseStackSize = Config.Bind("03. Inventory", "IncreaseStackSize", true,
            new ConfigDescription("Enables increasing the item stack size, allowing for more efficient inventory management."));
        IncreaseStackSizeValue = Config.Bind("03. Inventory", "IncreaseStackSizeValue", 999,
            new ConfigDescription("Determines the maximum number of items in a single stack.", new AcceptableValueRange<int>(1, 999)));

        // Group 4: Save System Customization
        EnableAutoSave = Config.Bind("04. Save System", "EnableAutoSave", true,
            new ConfigDescription("Activates the auto-save feature, automatically saving game progress at set intervals."));
        AutoSaveFrequency = Config.Bind("04. Save System", "AutoSaveFrequency", 300,
            new ConfigDescription("Sets the frequency of auto-saves in seconds.", new AcceptableValueRange<int>(30, 600)));

        LoadToSaveMenu = Config.Bind("04. Save System", "LoadToSaveMenu", true,
            new ConfigDescription("Changes the initial game load screen to the save menu, streamlining the game start."));
        AutoLoadSpecifiedSave = Config.Bind("04. Save System", "AutoLoadSpecifiedSave", false,
            new ConfigDescription("Automatically loads a pre-selected save slot when starting the game."));
        AutoLoadSpecifiedSaveSlot = Config.Bind("04. Save System", "AutoLoadSpecifiedSaveSlot", 1,
            new ConfigDescription("Determines which save slot to auto-load.", new AcceptableValueRange<int>(1, 5)));

        // Group 5: Gameplay Enhancements
        PauseTimeWhenViewingInventories = Config.Bind("05. Gameplay", "PauseTimeWhenViewingInventories", true,
            new ConfigDescription("Pauses the game when accessing inventory screens, excluding cooking interfaces."));
        UnlockBothModSlots = Config.Bind("05. Gameplay", "UnlockBothModSlots", false,
            new ConfigDescription("Both mod slots on gears/weapons remain unlocked."));
        InstantRestaurantUpgrades = Config.Bind("05. Gameplay", "InstantRestaurantUpgrades", false,
            new ConfigDescription("Allows for immediate upgrades to the restaurant, bypassing build times."));
        InstantBrew = Config.Bind("05. Gameplay", "InstantBrew", false,
            new ConfigDescription("Enables instant brewing processes, eliminating the usual brewing duration."));
        InstantWeaponUpgrades = Config.Bind("05. Gameplay", "InstantWeaponUpgrades", false,
            new ConfigDescription("Enables instant weapon upgrades, eliminating the usual upgrade duration."));
        ItemDropMultiplier = Config.Bind("05. Gameplay", "ItemDropMultiplier", false,
            new ConfigDescription("Enables the modification of item drop rates."));
        ItemDropMultiplierValue = Config.Bind("05. Gameplay", "ItemDropMultiplierValue", 2,
            new ConfigDescription("Sets the multiplier for item drop rates. At the moment, basically sets stack size on drop.", new AcceptableValueList<int>(2, 3, 4, 5, 6)));

        // Group 6: Player Health Customization
        ModifyPlayerMaxHp = Config.Bind("06. Player Health", "ModifyPlayerMaxHp", false,
            new ConfigDescription("Enables the modification of the player's maximum health."));
        ModifyPlayerMaxHpMultiplier = Config.Bind("06. Player Health", "ModifyPlayerMaxHpMultiplier", 1.25f,
            new ConfigDescription("Sets the multiplier for the player's maximum health. 1.25 would be 25% more health.", new AcceptableValueList<float>(0.5f, 1.25f, 1.5f, 1.75f, 2f)));
        RegenPlayerHp = Config.Bind("06. Player Health", "RegenPlayerHp", true,
            new ConfigDescription("Activates health regeneration for the player, gradually restoring health over time."));
        RegenPlayerHpAmount = Config.Bind("06. Player Health", "RegenPlayerHpAmount", 1,
            new ConfigDescription("Specifies the amount of health regenerated per tick.", new AcceptableValueList<int>(1, 2, 3, 4, 5)));
        RegenPlayerHpTick = Config.Bind("06. Player Health", "RegenPlayerHpTick", 3f,
            new ConfigDescription("Determines the time interval in seconds for each health regeneration tick.", new AcceptableValueList<float>(1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f, 9f, 10f)));
        RegenPlayerHpShowFloatingText = Config.Bind("06. Player Health", "RegenPlayerHpShowFloatingText", true,
            new ConfigDescription("Displays a floating text notification during health regeneration."));

        //Group 7: Player Movement
        IncreasePlayerMoveSpeed = Config.Bind("07. Player Movement", "IncreasePlayerMoveSpeed", false,
            new ConfigDescription("Increases the speed of the player."));
        PlayerMoveSpeedValue = Config.Bind("07. Player Movement", "PlayerMoveSpeedValue", 1.25f,
            new ConfigDescription("Determines the speed of the player. Good luck playing at 5.", new AcceptableValueRange<float>(1.10f, 5f)));

        // Group 8: Restaurant Management
        AutomaticPayment = Config.Bind("08. Restaurant", "AutomaticPayment", false,
            new ConfigDescription("Automatically accept payment for customer."));
        IncreaseCustomerMoveSpeed = Config.Bind("08. Restaurant", "IncreaseCustomerMoveSpeed", false,
            new ConfigDescription("Increases the speed of customers."));
        IncreaseCustomerMoveSpeedAnimation = Config.Bind("08. Restaurant", "IncreaseCustomerMoveSpeedAnimation", false,
            new ConfigDescription("Increases the speed of customers move animation. Test it out, see how you go."));
        CustomerMoveSpeedValue = Config.Bind("08. Restaurant", "CustomerMoveSpeedValue", 1.25f,
            new ConfigDescription("Determines the speed of customers. Setting too high will cause momentum issues.", new AcceptableValueRange<float>(1.10f, 5f)));
        DineAndDash = Config.Bind("08. Restaurant", "DineAndDash", false,
            new ConfigDescription("Toggle the Dine & Dash mechanic."));
        AllCustomersSelfServe = Config.Bind("08. Restaurant", "AllCustomersSelfServe", false,
            new ConfigDescription("All customers are able to collect their own dishes."));

        //Group 9: Attacks/Damage
        RemoveChainAttackDelay = Config.Bind("09. Attacks/Damage", "RemoveChainAttackDelay", false,
            new ConfigDescription("Removes the delay for chain attacks. So instead of [swing..swing.....big swing] it becomes, [swing..swing..big swing]. Or that's the idea. It's very subtle, as the delay is only half a second anyway."));
        ModifyWeaponSpecialCooldown = Config.Bind("09. Attacks/Damage", "ModifyWeaponSpecialCooldown", false,
            new ConfigDescription("Modify the cooldown for weapon special attacks."));
        WeaponSpecialCooldownValue = Config.Bind("09. Attacks/Damage", "WeaponSpecialCooldownValue", 25f,
            new ConfigDescription("Set the cooldown for weapon special attacks. 25 is 25% shorter. 12sec would become 9sec.", new AcceptableValueRange<float>(10f, 90f)));
        OneHitDestructible = Config.Bind("09. Attacks/Damage", "OneHitDestructible", false,
            new ConfigDescription("One hit destructible objects. Like the rocks/trees etc."));
        AutoReloadWeapons = Config.Bind("09. Attacks/Damage", "AutoReloadWeapons", false,
            new ConfigDescription("Automatically reload weapons when they run out of ammo."));


        //Group 10: Cheats
        AutoCook = Config.Bind("10. Cheats", "AutoCook", false,
            new ConfigDescription("Automatically cook food. !!WARNING!! - THIS WILL CAUSE ALL(?) RECIPES TO UNLOCK AND POP THE APPROPRIATE ACHIEVEMENTS."));
        FreeCook = Config.Bind("10. Cheats", "FreeCook", false,
            new ConfigDescription("No ingredients required."));
        InstantKill = Config.Bind("10. Cheats", "InstantKill", false,
            new ConfigDescription("One hit kill enemies."));

        //Group 11: Zoom
        AdjustableZoomLevel = Config.Bind("11. Zoom", "AdjustableZoomLevel", false,
            new ConfigDescription("Adjust the zoom level of the camera. This needs to be true for any of the zoom settings to function."));
        RelativeZoomAdjustment = Config.Bind("11. Zoom", "RelativeZoomAdjustment", 0f,
            new ConfigDescription("Adjust the relative zoom level of the camera. Each area has a different base zoom, this adds/removes to the base zoom. Recommended.", new AcceptableValueRange<float>(-100f, 100f)));
        UseStaticZoomLevel = Config.Bind("11. Zoom", "UseStaticZoomLevel", false,
            new ConfigDescription("Use a static zoom level instead of relative zoom. Zoom will be the same everywhere. Relative is ignored if this is true."));
        StaticZoomAdjustment = Config.Bind("11. Zoom", "StaticZoomAdjustment", 5f,
            new ConfigDescription("Static zoom value to use. For reference, inside the bedroom, the default game value is 2.9. Outside is 5, and a dungeon is 4.5.", new AcceptableValueRange<float>(1f, 100f)));
    }


    public override void Load()
    {
        Instance = this;
        Logger = Log;
        Config.ConfigReloaded += (_, _) =>
        {
            InitConfig();
            Utils.WriteLog("Reloaded configuration.", true);
            Fixes.RunFixes(string.Empty, true);
            Utils.ShowScreenMessage(Lang.GetReloadedConfigMessage(), 2);
        };
        InitConfig();

        Utils.AttachToSceneOnLoaded((scene, _) =>
        {
            Fixes.RunFixes(scene.name);
        });
        
        
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        AddComponent<UnityEvents>();
        Utils.WriteLog($"Plugin {PluginGuid} is loaded!", true);
    }


}