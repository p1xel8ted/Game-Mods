namespace WheresMaStorage;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInDependency("p1xel8ted.gyk.gykhelper", "3.0.9")]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.wheresmastorage";
    private const string PluginName = "Where's Ma' Storage!";
    private const string PluginVer = "2.1.7";

    internal static ManualLogSource Log { get; private set; }
    internal static ConfigEntry<bool> ModifyInventorySize { get; private set; }
    internal static ConfigEntry<bool> EnableGraveItemStacking { get; private set; }
    internal static ConfigEntry<bool> EnablePenPaperInkStacking { get; private set; }
    internal static ConfigEntry<bool> EnableChiselStacking { get; private set; }
    internal static ConfigEntry<bool> EnableToolStacking { get; private set; }
    internal static ConfigEntry<bool> EnablePrayerStacking { get; private set; }
    internal static ConfigEntry<bool> EnableWeaponStacking { get; private set; }
    internal static ConfigEntry<bool> EnableEquipmentStacking { get; private set; }
    internal static ConfigEntry<bool> AllowHandToolDestroy { get; private set; }
    internal static ConfigEntry<bool> ModifyStackSize { get; private set; }
    internal static ConfigEntry<bool> Debug { get; private set; }
    internal static ConfigEntry<bool> SharedInventory { get; private set; }
    internal static ConfigEntry<bool> ExcludeWellsFromSharedInventory { get; private set; }
    internal static ConfigEntry<bool> ExcludeZombieMillFromSharedInventory { get; private set; }
    internal static ConfigEntry<bool> ExcludeQuarryFromSharedInventory { get; private set; }
    internal static ConfigEntry<bool> DontShowEmptyRowsInInventory { get; private set; }
    internal static ConfigEntry<bool> ShowUsedSpaceInTitles { get; private set; }
    internal static ConfigEntry<bool> DisableInventoryDimming { get; private set; }
    internal static ConfigEntry<bool> ShowWorldZoneInTitles { get; private set; }
    internal static ConfigEntry<bool> HideInvalidSelections { get; private set; }
    internal static ConfigEntry<bool> RemoveGapsBetweenSections { get; private set; }
    internal static ConfigEntry<bool> RemoveGapsBetweenSectionsVendor { get; private set; }
    internal static ConfigEntry<bool> ShowOnlyPersonalInventory { get; private set; }
    internal static ConfigEntry<int> AdditionalInventorySpace { get; private set; }
    internal static ConfigEntry<int> StackSizeForStackables { get; private set; }
    internal static ConfigEntry<bool> HideStockpileWidgets { get; private set; }
    internal static ConfigEntry<bool> HideTavernWidgets { get; private set; }
    internal static ConfigEntry<bool> HideSoulWidgets { get; private set; }
    internal static ConfigEntry<bool> HideWarehouseShopWidgets { get; private set; }
    internal static ConfigEntry<bool> CollectDropsOnGameLoad { get; private set; }
    public static ConfigEntry<bool> AllowZombiesAccessToSharedInventory { get; set; }
    

    private void Awake()
    {
        Log = Logger;
        InitConfiguration();
        Actions.GameStartedPlaying += Helpers.RunWmsTasks;
        Actions.GameBalanceLoad += Helpers.GameBalanceLoad;
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        Log.LogInfo($"Plugin {PluginName} is loaded!");
    }


    private void InitConfiguration()
    {
        Debug = Config.Bind("00. Advanced", "Debug Logging", false, new ConfigDescription("Enable or disable debug logging.", null, new ConfigurationManagerAttributes {IsAdvanced = true, Order = 49}));

        SharedInventory = Config.Bind("3. Inventory", "Shared Inventory", true, new ConfigDescription("Enable or disable shared inventory when crafting.", null, new ConfigurationManagerAttributes {Order = 48}));
        AllowZombiesAccessToSharedInventory = Config.Bind("3. Inventory", "Allow Zombies Access To Shared Inventory", false, new ConfigDescription("Enable or disable allowing zombies access to shared inventory.", null, new ConfigurationManagerAttributes {Order = 47}));
        AllowZombiesAccessToSharedInventory.SettingChanged += (_, _) => Fields.InventoriesLoaded = false;
        
        ExcludeWellsFromSharedInventory = Config.Bind("3. Inventory", "Exclude Wells From Shared Inventory", true, new ConfigDescription("Enable or disable excluding wells from shared inventory.", null, new ConfigurationManagerAttributes {Order = 47}));
        ExcludeWellsFromSharedInventory.SettingChanged += (_, _) => Fields.InventoriesLoaded = false;

        ExcludeZombieMillFromSharedInventory = Config.Bind("3. Inventory", "Exclude Zombie Mill From Shared Inventory", true, new ConfigDescription("Enable or disable excluding the zombie mill from shared inventory.", null, new ConfigurationManagerAttributes {Order = 46}));
        ExcludeZombieMillFromSharedInventory.SettingChanged += (_, _) => Fields.InventoriesLoaded = false;

        ExcludeQuarryFromSharedInventory = Config.Bind("3. Inventory", "Exclude Quarry From Shared Inventory", true, new ConfigDescription("Enable or disable excluding the quarry from shared inventory.", null, new ConfigurationManagerAttributes {Order = 45}));
        ExcludeQuarryFromSharedInventory.SettingChanged += (_, _) => Fields.InventoriesLoaded = false;
        
        ModifyInventorySize = Config.Bind("3. Inventory", "Modify Inventory Size", true, new ConfigDescription("Enable or disable modifying the inventory size.", null, new ConfigurationManagerAttributes {Order = 47}));
        ModifyInventorySize.SettingChanged += (_, _) => Helpers.UpdateInventorySizes();

        AdditionalInventorySpace = Config.Bind("3. Inventory", "Additional Inventory Space", 20, new ConfigDescription("Set the number of additional inventory spaces.", new AcceptableValueRange<int>(1, 500), new ConfigurationManagerAttributes {Order = 46}));
        AdditionalInventorySpace.SettingChanged += (_, _) => Helpers.UpdateInventorySizes();

        ModifyStackSize = Config.Bind("4. Item Stacking", "Modify Stack Size", true, new ConfigDescription("Enable or disable modifying the stack size of items.", null, new ConfigurationManagerAttributes {Order = 45}));
        ModifyStackSize.SettingChanged += (_, _) => Helpers.UpdateStackSizes();

        StackSizeForStackables = Config.Bind("4. Item Stacking", "Stack Size For Stackables", 999, new ConfigDescription("Set the maximum stack size for stackable items", new AcceptableValueRange<int>(1, 999), new ConfigurationManagerAttributes {Order = 44}));
        StackSizeForStackables.SettingChanged += (_, _) => Helpers.UpdateStackSizes();

        EnableGraveItemStacking = Config.Bind("4. Item Stacking", "Grave Item Stacking", false, new ConfigDescription("Allow grave items to stack", null, new ConfigurationManagerAttributes {Order = 43}));
        EnableGraveItemStacking.SettingChanged += (_, _) => Helpers.UpdateStackSizes();

        EnablePenPaperInkStacking = Config.Bind("4. Item Stacking", "Pen Paper Ink Stacking", false, new ConfigDescription("Allow pen, paper, and ink items to stack", null, new ConfigurationManagerAttributes {Order = 42}));
        EnablePenPaperInkStacking.SettingChanged += (_, _) => Helpers.UpdateStackSizes();

        EnableChiselStacking = Config.Bind("4. Item Stacking", "Chisel Stacking", false, new ConfigDescription("Allow chisel items to stack", null, new ConfigurationManagerAttributes {Order = 41}));
        EnableChiselStacking.SettingChanged += (_, _) => Helpers.UpdateStackSizes();

        EnableToolStacking = Config.Bind("4. Item Stacking", "Tool Stacking", true, new ConfigDescription("Allow tool items to stack", null, new ConfigurationManagerAttributes {Order = 39}));
        EnableToolStacking.SettingChanged += (_, _) => Helpers.UpdateStackSizes();

        EnablePrayerStacking = Config.Bind("4. Item Stacking", "Prayer Stacking", true, new ConfigDescription("Allow prayer items to stack", null, new ConfigurationManagerAttributes {Order = 38}));
        EnablePrayerStacking.SettingChanged += (_, _) => Helpers.UpdateStackSizes();

        EnableWeaponStacking = Config.Bind("4. Item Stacking", "Weapon Stacking", true, new ConfigDescription("Allow weapon items to stack", null, new ConfigurationManagerAttributes {Order = 37}));
        EnableWeaponStacking.SettingChanged += (_, _) => Helpers.UpdateStackSizes();

        EnableEquipmentStacking = Config.Bind("4. Item Stacking", "Equipment Stacking", true, new ConfigDescription("Allow equipment items to stack", null, new ConfigurationManagerAttributes {Order = 36}));
        EnableEquipmentStacking.SettingChanged += (_, _) => Helpers.UpdateStackSizes();

        DontShowEmptyRowsInInventory = Config.Bind("3. Inventory", "Dont Show Empty Rows In Inventory", true, new ConfigDescription("Enable or disable displaying empty rows in the inventory.", null, new ConfigurationManagerAttributes {Order = 39}));
        ShowUsedSpaceInTitles = Config.Bind("3. Inventory", "Show Used Space In Titles", true, new ConfigDescription("Enable or disable showing used space in inventory titles.", null, new ConfigurationManagerAttributes {Order = 38}));
        DisableInventoryDimming = Config.Bind("3. Inventory", "Inventory Dimming", true, new ConfigDescription("Enable or disable inventory dimming.", null, new ConfigurationManagerAttributes {Order = 37}));
        ShowWorldZoneInTitles = Config.Bind("3. Inventory", "Show World Zone In Titles", true, new ConfigDescription("Enable or disable showing world zone information in inventory titles.", null, new ConfigurationManagerAttributes {Order = 36}));
        HideInvalidSelections = Config.Bind("3. Inventory", "Hide Invalid Selections", true, new ConfigDescription("Enable or disable hiding invalid item selections in the inventory.", null, new ConfigurationManagerAttributes {Order = 35}));
        RemoveGapsBetweenSections = Config.Bind("3. Inventory", "Remove Gaps Between Sections", true, new ConfigDescription("Enable or disable removing gaps between inventory sections.", null, new ConfigurationManagerAttributes {Order = 34}));
        RemoveGapsBetweenSectionsVendor = Config.Bind("3. Inventory", "Remove Gaps Between Sections Vendor", true, new ConfigDescription("Enable or disable removing gaps between sections in the vendor inventory.", null, new ConfigurationManagerAttributes {Order = 33}));
        ShowOnlyPersonalInventory = Config.Bind("3. Inventory", "Show Only Personal Inventory", true, new ConfigDescription("Enable or disable showing only personal inventory.", null, new ConfigurationManagerAttributes {Order = 32}));

        AllowHandToolDestroy = Config.Bind("5. Gameplay", "Allow Hand Tool Destroy", true, new ConfigDescription("Enable or disable destroying objects with hand tools", null, new ConfigurationManagerAttributes {Order = 31}));
        AllowHandToolDestroy.SettingChanged += (_, _) => Helpers.UpdateToolDestroy();

        CollectDropsOnGameLoad = Config.Bind("5. Gameplay", "Collect Drops On Game Load", true, new ConfigDescription("Enable or disable collecting drops on game load", null, new ConfigurationManagerAttributes {Order = 30}));
        HideStockpileWidgets = Config.Bind("6. UI", "Hide Stockpile Widgets", true, new ConfigDescription("Enable or disable hiding stockpile widgets", null, new ConfigurationManagerAttributes {Order = 29}));
        HideTavernWidgets = Config.Bind("6. UI", "Hide Tavern Widgets", true, new ConfigDescription("Enable or disable hiding tavern widgets", null, new ConfigurationManagerAttributes {Order = 28}));
        HideSoulWidgets = Config.Bind("6. UI", "Hide Soul Widgets", true, new ConfigDescription("Enable or disable hiding soul widgets", null, new ConfigurationManagerAttributes {Order = 27}));
        HideWarehouseShopWidgets = Config.Bind("6. UI", "Hide Warehouse Shop Widgets", true, new ConfigDescription("Enable or disable hiding warehouse shop widgets", null, new ConfigurationManagerAttributes {Order = 26}));
        Fields.GameBalanceAlreadyRun = false;
    }


    public void Update()
    {
        if (!MainGame.game_started) return;
        if (Fields.InventoriesLoaded) return;
        Log.LogInfo("Refreshing inventories...");
        Helpers.RunWmsTasks();
    }
}