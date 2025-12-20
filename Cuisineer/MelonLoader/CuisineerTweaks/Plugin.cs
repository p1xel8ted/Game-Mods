[assembly: MelonInfo(typeof(CuisineerTweaks.Plugin), "Cuisineer Tweaks", "0.3.0", "p1xel8ted")]
[assembly: MelonGame("BattleBrewProductions", "Cuisineer")]

namespace CuisineerTweaks;

public class Plugin : MelonMod
{
    // Preference Categories
    private static MelonPreferences_Category _generalCategory;
    private static MelonPreferences_Category _performanceCategory;
    private static MelonPreferences_Category _uiCategory;
    private static MelonPreferences_Category _inventoryCategory;
    private static MelonPreferences_Category _saveSystemCategory;
    private static MelonPreferences_Category _gameplayCategory;
    private static MelonPreferences_Category _playerHealthCategory;
    private static MelonPreferences_Category _playerMovementCategory;
    private static MelonPreferences_Category _restaurantCategory;
    private static MelonPreferences_Category _attacksDamageCategory;
    private static MelonPreferences_Category _cheatsCategory;
    private static MelonPreferences_Category _zoomCategory;

    // Preference Entries - General
    internal static MelonPreferences_Entry<bool> DebugMode { get; private set; }
    internal static MelonPreferences_Entry<bool> DisplayMessages { get; private set; }
    internal static MelonPreferences_Entry<KeyCode> KeybindReload { get; private set; }
    internal static MelonPreferences_Entry<KeyCode> KeybindSaveGame { get; private set; }
    internal static MelonPreferences_Entry<KeyCode> KeybindZoomIn { get; private set; }
    internal static MelonPreferences_Entry<KeyCode> KeybindZoomOut { get; private set; }

    // Performance
    internal static MelonPreferences_Entry<bool> CorrectMainMenuAspect { get; private set; }
    internal static MelonPreferences_Entry<bool> CorrectFixedUpdateRate { get; private set; }
    internal static MelonPreferences_Entry<bool> UseRefreshRateForFixedUpdateRate { get; private set; }

    // User Interface
    internal static MelonPreferences_Entry<bool> InstantText { get; private set; }

    // Inventory
    internal static MelonPreferences_Entry<bool> IncreaseStackSize { get; private set; }
    internal static MelonPreferences_Entry<int> IncreaseStackSizeValue { get; private set; }

    // Save System
    internal static MelonPreferences_Entry<bool> EnableAutoSave { get; private set; }
    internal static MelonPreferences_Entry<int> AutoSaveFrequency { get; private set; }
    internal static MelonPreferences_Entry<bool> LoadToSaveMenu { get; private set; }
    internal static MelonPreferences_Entry<bool> AutoLoadSpecifiedSave { get; private set; }
    internal static MelonPreferences_Entry<int> AutoLoadSpecifiedSaveSlot { get; private set; }

    // Gameplay
    internal static MelonPreferences_Entry<bool> PauseTimeWhenViewingInventories { get; private set; }
    internal static MelonPreferences_Entry<bool> UnlockBothModSlots { get; private set; }
    internal static MelonPreferences_Entry<bool> InstantRestaurantUpgrades { get; private set; }
    internal static MelonPreferences_Entry<bool> InstantBrew { get; private set; }
    internal static MelonPreferences_Entry<bool> InstantWeaponUpgrades { get; private set; }
    internal static MelonPreferences_Entry<bool> ItemDropMultiplier { get; private set; }
    internal static MelonPreferences_Entry<int> ItemDropMultiplierValue { get; private set; }

    // Player Health
    internal static MelonPreferences_Entry<bool> ModifyPlayerMaxHp { get; private set; }
    internal static MelonPreferences_Entry<float> ModifyPlayerMaxHpMultiplier { get; private set; }
    internal static MelonPreferences_Entry<bool> RegenPlayerHp { get; private set; }
    internal static MelonPreferences_Entry<int> RegenPlayerHpAmount { get; private set; }
    internal static MelonPreferences_Entry<float> RegenPlayerHpTick { get; private set; }
    internal static MelonPreferences_Entry<bool> RegenPlayerHpShowFloatingText { get; private set; }

    // Player Movement
    internal static MelonPreferences_Entry<bool> IncreasePlayerMoveSpeed { get; private set; }
    internal static MelonPreferences_Entry<float> PlayerMoveSpeedValue { get; private set; }

    // Restaurant
    internal static MelonPreferences_Entry<bool> AutomaticPayment { get; private set; }
    internal static MelonPreferences_Entry<bool> IncreaseCustomerMoveSpeed { get; private set; }
    internal static MelonPreferences_Entry<bool> IncreaseCustomerMoveSpeedAnimation { get; private set; }
    internal static MelonPreferences_Entry<float> CustomerMoveSpeedValue { get; private set; }
    internal static MelonPreferences_Entry<bool> DineAndDash { get; private set; }
    internal static MelonPreferences_Entry<bool> AllCustomersSelfServe { get; private set; }

    // Attacks/Damage
    internal static MelonPreferences_Entry<bool> RemoveChainAttackDelay { get; private set; }
    internal static MelonPreferences_Entry<bool> ModifyWeaponSpecialCooldown { get; private set; }
    internal static MelonPreferences_Entry<float> WeaponSpecialCooldownValue { get; private set; }
    internal static MelonPreferences_Entry<bool> OneHitDestructible { get; private set; }
    internal static MelonPreferences_Entry<bool> AutoReloadWeapons { get; private set; }

    // Cheats
    internal static MelonPreferences_Entry<bool> AutoCook { get; private set; }
    internal static MelonPreferences_Entry<bool> FreeCook { get; private set; }
    internal static MelonPreferences_Entry<bool> InstantKill { get; private set; }

    // Zoom
    internal static MelonPreferences_Entry<bool> AdjustableZoomLevel { get; private set; }
    internal static MelonPreferences_Entry<float> RelativeZoomAdjustment { get; private set; }
    internal static MelonPreferences_Entry<bool> UseStaticZoomLevel { get; private set; }
    internal static MelonPreferences_Entry<float> StaticZoomAdjustment { get; private set; }

    internal static Plugin Instance { get; private set; }

    public override void OnInitializeMelon()
    {
        Debug.unityLogger.logEnabled = true;
        Instance = this;

        InitPreferences();

        HarmonyInstance.PatchAll(Assembly.GetExecutingAssembly());

        LoggerInstance.Msg($"Plugin loaded successfully!");
    }

    private void InitPreferences()
    {
        // Category: General Settings
        _generalCategory = MelonPreferences.CreateCategory("00. General");
        DebugMode = _generalCategory.CreateEntry("DebugMode", false,
            "Enables debug mode, which enables more verbose logging. This is not recommended for normal use.");
        DisplayMessages = _generalCategory.CreateEntry("DisplayMessages", true,
            "Enables the display of messages in-game. Disable this if you don't want to see messages.");
        KeybindReload = _generalCategory.CreateEntry("KeybindReload", KeyCode.R,
            "The keybind to reload the configuration file. List of valid entries here: https://docs.unity3d.com/ScriptReference/KeyCode.html");
        KeybindSaveGame = _generalCategory.CreateEntry("KeybindSaveGame", KeyCode.K,
            "The keybind to save the current game. List of valid entries here: https://docs.unity3d.com/ScriptReference/KeyCode.html");
        KeybindZoomIn = _generalCategory.CreateEntry("KeybindZoomIn", KeyCode.O,
            "The keybind to zoom in. List of valid entries here: https://docs.unity3d.com/ScriptReference/KeyCode.html");
        KeybindZoomOut = _generalCategory.CreateEntry("KeybindZoomOut", KeyCode.L,
            "The keybind to zoom out. List of valid entries here: https://docs.unity3d.com/ScriptReference/KeyCode.html");

        // Category: Performance Settings
        _performanceCategory = MelonPreferences.CreateCategory("01. Performance");
        CorrectFixedUpdateRate = _performanceCategory.CreateEntry("CorrectFixedUpdateRate", true,
            "Adjusts the fixed update rate to minimum amount to reduce camera judder based on your refresh rate.");
        UseRefreshRateForFixedUpdateRate = _performanceCategory.CreateEntry("UseRefreshRateForFixedUpdateRate", false,
            "Sets the fixed update rate based on the monitor's refresh rate for smoother gameplay. If you're playing on a potato, this may have performance impacts.");
        CorrectMainMenuAspect = _performanceCategory.CreateEntry("CorrectMainMenuAspect", true,
            "Adjusts the main menu images to fit the screen. Will only be applied on aspect ratios wider than 16:9.");

        // Category: User Interface Enhancements
        _uiCategory = MelonPreferences.CreateCategory("02. User Interface");
        InstantText = _uiCategory.CreateEntry("InstantDialogueText", true,
            "Dialogue text is instantly displayed, skipping the typewriter effect.");

        // Category: Inventory Management
        _inventoryCategory = MelonPreferences.CreateCategory("03. Inventory");
        IncreaseStackSize = _inventoryCategory.CreateEntry("IncreaseStackSize", true,
            "Enables increasing the item stack size, allowing for more efficient inventory management.");
        IncreaseStackSizeValue = _inventoryCategory.CreateEntry("IncreaseStackSizeValue", 999,
            "Determines the maximum number of items in a single stack. Recommended range: 1-999.");

        // Category: Save System Customization
        _saveSystemCategory = MelonPreferences.CreateCategory("04. Save System");
        EnableAutoSave = _saveSystemCategory.CreateEntry("EnableAutoSave", true,
            "Activates the auto-save feature, automatically saving game progress at set intervals.");
        AutoSaveFrequency = _saveSystemCategory.CreateEntry("AutoSaveFrequency", 300,
            "Sets the frequency of auto-saves in seconds. Recommended range: 30-600 (30 seconds to 10 minutes).");
        LoadToSaveMenu = _saveSystemCategory.CreateEntry("LoadToSaveMenu", true,
            "Changes the initial game load screen to the save menu, streamlining the game start.");
        AutoLoadSpecifiedSave = _saveSystemCategory.CreateEntry("AutoLoadSpecifiedSave", false,
            "Automatically loads a pre-selected save slot when starting the game.");
        AutoLoadSpecifiedSaveSlot = _saveSystemCategory.CreateEntry("AutoLoadSpecifiedSaveSlot", 1,
            "Determines which save slot to auto-load. Valid range: 1-5.");

        // Category: Gameplay Enhancements
        _gameplayCategory = MelonPreferences.CreateCategory("05. Gameplay");
        PauseTimeWhenViewingInventories = _gameplayCategory.CreateEntry("PauseTimeWhenViewingInventories", true,
            "Pauses the game when accessing inventory screens, excluding cooking interfaces.");
        UnlockBothModSlots = _gameplayCategory.CreateEntry("UnlockBothModSlots", false,
            "Both mod slots on gears/weapons remain unlocked.");
        InstantRestaurantUpgrades = _gameplayCategory.CreateEntry("InstantRestaurantUpgrades", false,
            "Allows for immediate upgrades to the restaurant, bypassing build times.");
        InstantBrew = _gameplayCategory.CreateEntry("InstantBrew", false,
            "Enables instant brewing processes, eliminating the usual brewing duration.");
        InstantWeaponUpgrades = _gameplayCategory.CreateEntry("InstantWeaponUpgrades", false,
            "Enables instant weapon upgrades, eliminating the usual upgrade duration.");
        ItemDropMultiplier = _gameplayCategory.CreateEntry("ItemDropMultiplier", false,
            "Enables the modification of item drop rates.");
        ItemDropMultiplierValue = _gameplayCategory.CreateEntry("ItemDropMultiplierValue", 2,
            "Sets the multiplier for item drop rates. At the moment, basically sets stack size on drop. Recommended range: 2-6.");

        // Category: Player Health Customization
        _playerHealthCategory = MelonPreferences.CreateCategory("06. Player Health");
        ModifyPlayerMaxHp = _playerHealthCategory.CreateEntry("ModifyPlayerMaxHp", false,
            "Enables the modification of the player's maximum health.");
        ModifyPlayerMaxHpMultiplier = _playerHealthCategory.CreateEntry("ModifyPlayerMaxHpMultiplier", 1.25f,
            "Sets the multiplier for the player's maximum health. 1.25 would be 25% more health. Recommended range: 0.5-2.0.");
        RegenPlayerHp = _playerHealthCategory.CreateEntry("RegenPlayerHp", true,
            "Activates health regeneration for the player, gradually restoring health over time.");
        RegenPlayerHpAmount = _playerHealthCategory.CreateEntry("RegenPlayerHpAmount", 1,
            "Specifies the amount of health regenerated per tick. Recommended range: 1-5.");
        RegenPlayerHpTick = _playerHealthCategory.CreateEntry("RegenPlayerHpTick", 3f,
            "Determines the time interval in seconds for each health regeneration tick. Recommended range: 1-10.");
        RegenPlayerHpShowFloatingText = _playerHealthCategory.CreateEntry("RegenPlayerHpShowFloatingText", true,
            "Displays a floating text notification during health regeneration.");

        // Category: Player Movement
        _playerMovementCategory = MelonPreferences.CreateCategory("07. Player Movement");
        IncreasePlayerMoveSpeed = _playerMovementCategory.CreateEntry("IncreasePlayerMoveSpeed", false,
            "Increases the speed of the player.");
        PlayerMoveSpeedValue = _playerMovementCategory.CreateEntry("PlayerMoveSpeedValue", 1.25f,
            "Determines the speed of the player. Good luck playing at 5. Recommended range: 1.10-5.0.");

        // Category: Restaurant Management
        _restaurantCategory = MelonPreferences.CreateCategory("08. Restaurant");
        AutomaticPayment = _restaurantCategory.CreateEntry("AutomaticPayment", false,
            "Automatically accept payment for customer.");
        IncreaseCustomerMoveSpeed = _restaurantCategory.CreateEntry("IncreaseCustomerMoveSpeed", false,
            "Increases the speed of customers.");
        IncreaseCustomerMoveSpeedAnimation = _restaurantCategory.CreateEntry("IncreaseCustomerMoveSpeedAnimation", false,
            "Increases the speed of customers move animation. Test it out, see how you go.");
        CustomerMoveSpeedValue = _restaurantCategory.CreateEntry("CustomerMoveSpeedValue", 1.25f,
            "Determines the speed of customers. Setting too high will cause momentum issues. Recommended range: 1.10-5.0.");
        DineAndDash = _restaurantCategory.CreateEntry("DineAndDash", false,
            "Toggle the Dine & Dash mechanic.");
        AllCustomersSelfServe = _restaurantCategory.CreateEntry("AllCustomersSelfServe", false,
            "All customers are able to collect their own dishes.");

        // Category: Attacks/Damage
        _attacksDamageCategory = MelonPreferences.CreateCategory("09. Attacks/Damage");
        RemoveChainAttackDelay = _attacksDamageCategory.CreateEntry("RemoveChainAttackDelay", false,
            "Removes the delay for chain attacks. So instead of [swing..swing.....big swing] it becomes, [swing..swing..big swing]. Or that's the idea. It's very subtle, as the delay is only half a second anyway.");
        ModifyWeaponSpecialCooldown = _attacksDamageCategory.CreateEntry("ModifyWeaponSpecialCooldown", false,
            "Modify the cooldown for weapon special attacks.");
        WeaponSpecialCooldownValue = _attacksDamageCategory.CreateEntry("WeaponSpecialCooldownValue", 25f,
            "Set the cooldown for weapon special attacks. 25 is 25% shorter. 12sec would become 9sec. Recommended range: 10-90.");
        OneHitDestructible = _attacksDamageCategory.CreateEntry("OneHitDestructible", false,
            "One hit destructible objects. Like the rocks/trees etc.");
        AutoReloadWeapons = _attacksDamageCategory.CreateEntry("AutoReloadWeapons", false,
            "Automatically reload weapons when they run out of ammo.");

        // Category: Cheats
        _cheatsCategory = MelonPreferences.CreateCategory("10. Cheats");
        AutoCook = _cheatsCategory.CreateEntry("AutoCook", false,
            "Automatically cook food. !!WARNING!! - THIS WILL CAUSE ALL(?) RECIPES TO UNLOCK AND POP THE APPROPRIATE ACHIEVEMENTS.");
        FreeCook = _cheatsCategory.CreateEntry("FreeCook", false,
            "No ingredients required.");
        InstantKill = _cheatsCategory.CreateEntry("InstantKill", false,
            "One hit kill enemies.");

        // Category: Zoom
        _zoomCategory = MelonPreferences.CreateCategory("11. Zoom");
        AdjustableZoomLevel = _zoomCategory.CreateEntry("AdjustableZoomLevel", false,
            "Adjust the zoom level of the camera. This needs to be true for any of the zoom settings to function.");
        RelativeZoomAdjustment = _zoomCategory.CreateEntry("RelativeZoomAdjustment", 0f,
            "Adjust the relative zoom level of the camera. Each area has a different base zoom, this adds/removes to the base zoom. Recommended. Recommended range: -10 to 10.");
        UseStaticZoomLevel = _zoomCategory.CreateEntry("UseStaticZoomLevel", false,
            "Use a static zoom level instead of relative zoom. Zoom will be the same everywhere. Relative is ignored if this is true.");
        StaticZoomAdjustment = _zoomCategory.CreateEntry("StaticZoomAdjustment", 5f,
            "Static zoom value to use. For reference, inside the bedroom, the default game value is 2.9. Outside is 5, and a dungeon is 4.5. Recommended range: 1-10.");
    }
}
