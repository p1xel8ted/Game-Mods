namespace WheresMaStorage;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.wheresmastorage";
    private const string PluginName = "Where's Ma' Storage!";
    private const string PluginVer = "2.1.14";

    // Section names. New scheme: ── Foo ── (alphabetical sort in CM).
    // Legacy section names get rewritten to these by MigrateRenamedSections() on first launch
    // of the new version, so existing user customisations are preserved.
    private const string AdvancedSection = "── Advanced ──";
    private const string InventorySection = "── Inventory ──";
    private const string ItemStackingSection = "── Item Stacking ──";
    private const string GameplaySection = "── Gameplay ──";
    private const string UISection = "── UI ──";

    private static readonly Dictionary<string, string> SectionRenames = new()
    {
        ["00. Advanced"] = AdvancedSection,
        ["3. Inventory"] = InventorySection,
        ["4. Item Stacking"] = ItemStackingSection,
        ["5. Gameplay"] = GameplaySection,
        ["6. UI"] = UISection,
    };

    internal static ManualLogSource Log { get; private set; }
    internal static bool DebugEnabled;
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
    internal static ConfigEntry<int> AdditionalPlayerInventorySpace { get; private set; }
    internal static ConfigEntry<int> AdditionalContainerInventorySpace { get; private set; }
    internal static ConfigEntry<int> StackSizeForStackables { get; private set; }
    internal static ConfigEntry<bool> HideStockpileWidgets { get; private set; }
    internal static ConfigEntry<bool> HideTavernWidgets { get; private set; }
    internal static ConfigEntry<bool> HideSoulWidgets { get; private set; }
    internal static ConfigEntry<bool> HideWarehouseShopWidgets { get; private set; }
    internal static ConfigEntry<bool> CollectDropsOnGameLoad { get; private set; }
    public static ConfigEntry<bool> AllowZombiesAccessToSharedInventory { get; set; }

    // Hidden migration entries.
    private static ConfigEntry<int> LegacyAdditionalInventorySpace { get; set; }
    private static ConfigEntry<bool> LegacySliderMigrated { get; set; }


    private void Awake()
    {
        Log = Logger;
        LogHelper.Log = Logger;
        MigrateRenamedSections();
        InitConfiguration();
        MigrateLegacySlider();
        Lang.Init(Assembly.GetExecutingAssembly(), Log);
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
    }

    // Rewrites old "[3. Inventory]" style headers to "[── Inventory ──]" in the .cfg file
    // so existing user values survive the section rename. Idempotent — re-running on an
    // already-migrated file is a no-op (no old headers left to find).
    private void MigrateRenamedSections()
    {
        var path = Config.ConfigFilePath;
        if (!File.Exists(path)) return;

        string content;
        try
        {
            content = File.ReadAllText(path);
        }
        catch (Exception ex)
        {
            Log.LogWarning($"[Migration] Could not read {path} for section rename: {ex.Message}");
            return;
        }

        var renamed = 0;
        foreach (var kv in SectionRenames)
        {
            var oldHeader = $"[{kv.Key}]";
            var newHeader = $"[{kv.Value}]";
            if (!content.Contains(oldHeader)) continue;
            content = content.Replace(oldHeader, newHeader);
            renamed++;
        }
        if (renamed == 0) return;

        try
        {
            File.WriteAllText(path, content);
        }
        catch (Exception ex)
        {
            Log.LogWarning($"[Migration] Could not write {path} after section rename: {ex.Message}");
            return;
        }

        Log.LogInfo($"[Migration] Renamed {renamed} legacy config section header(s) to the '── Name ──' style. Existing user values preserved.");
        Config.Reload();
    }

    // Copies the pre-split AdditionalInventorySpace value into the new player+container sliders
    // on first launch of the split version, so users don't lose their customisation.
    private static void MigrateLegacySlider()
    {
        if (LegacySliderMigrated.Value) return;

        if (LegacyAdditionalInventorySpace.Value != 20)
        {
            AdditionalPlayerInventorySpace.Value = LegacyAdditionalInventorySpace.Value;
            AdditionalContainerInventorySpace.Value = LegacyAdditionalInventorySpace.Value;
            Log.LogInfo($"[Migration] Migrated legacy 'Additional Inventory Space' value ({LegacyAdditionalInventorySpace.Value}) to both Player and Container sliders.");
        }
        LegacySliderMigrated.Value = true;
    }


    private void InitConfiguration()
    {
        // ── Advanced ──
        Debug = Config.Bind(AdvancedSection, "Debug Logging", false,
            new ConfigDescription("Enable or disable debug logging.", null,
                new ConfigurationManagerAttributes {Order = 90}));
        DebugEnabled = Debug.Value;
        Debug.SettingChanged += (_, _) => DebugEnabled = Debug.Value;

        LegacySliderMigrated = Config.Bind(AdvancedSection, "Legacy Slider Migrated", false,
            new ConfigDescription("Internal: marks whether the old single inventory-space slider has been split into Player/Container.", null,
                new ConfigurationManagerAttributes {Browsable = false, IsAdvanced = true, HideDefaultButton = true, ReadOnly = true}));

        // ── Inventory ──
        SharedInventory = Config.Bind(InventorySection, "Shared Inventory", true,
            new ConfigDescription("Enable or disable shared inventory when crafting.", null,
                new ConfigurationManagerAttributes {Order = 100}));

        AllowZombiesAccessToSharedInventory = Config.Bind(InventorySection, "Allow Zombies Access To Shared Inventory", false,
            new ConfigDescription("Enable or disable allowing zombies access to shared inventory.", null,
                new ConfigurationManagerAttributes {Order = 99, DispName = "    └ Allow Zombies Access"}));
        AllowZombiesAccessToSharedInventory.SettingChanged += (_, _) => Fields.InventoriesLoaded = false;

        ExcludeWellsFromSharedInventory = Config.Bind(InventorySection, "Exclude Wells From Shared Inventory", true,
            new ConfigDescription("Enable or disable excluding wells from shared inventory.", null,
                new ConfigurationManagerAttributes {Order = 98, DispName = "    └ Exclude Wells"}));
        ExcludeWellsFromSharedInventory.SettingChanged += (_, _) => Fields.InventoriesLoaded = false;

        ExcludeZombieMillFromSharedInventory = Config.Bind(InventorySection, "Exclude Zombie Mill From Shared Inventory", true,
            new ConfigDescription("Enable or disable excluding the zombie mill from shared inventory.", null,
                new ConfigurationManagerAttributes {Order = 97, DispName = "    └ Exclude Zombie Mill"}));
        ExcludeZombieMillFromSharedInventory.SettingChanged += (_, _) => Fields.InventoriesLoaded = false;

        ExcludeQuarryFromSharedInventory = Config.Bind(InventorySection, "Exclude Quarry From Shared Inventory", true,
            new ConfigDescription("Enable or disable excluding the quarry from shared inventory.", null,
                new ConfigurationManagerAttributes {Order = 96, DispName = "    └ Exclude Quarry"}));
        ExcludeQuarryFromSharedInventory.SettingChanged += (_, _) => Fields.InventoriesLoaded = false;

        ModifyInventorySize = Config.Bind(InventorySection, "Modify Inventory Size", true,
            new ConfigDescription("Enable or disable modifying the inventory size for the player and chests.", null,
                new ConfigurationManagerAttributes {Order = 90}));
        ModifyInventorySize.SettingChanged += (_, _) =>
        {
            Fields.InventorySizesDirty = true;
            Fields.InventoriesLoaded = false;
        };

        AdditionalPlayerInventorySpace = Config.Bind(InventorySection, "Additional Player Inventory Space", 20,
            new ConfigDescription("Number of additional inventory spaces granted to the player (0 means vanilla size). Requires Modify Inventory Size to be on.",
                new AcceptableValueRange<int>(0, 500),
                new ConfigurationManagerAttributes {Order = 89, DispName = "    └ Additional Player Inventory Space"}));
        AdditionalPlayerInventorySpace.SettingChanged += (_, _) =>
        {
            Fields.InventorySizesDirty = true;
            Fields.InventoriesLoaded = false;
        };

        AdditionalContainerInventorySpace = Config.Bind(InventorySection, "Additional Container Inventory Space", 20,
            new ConfigDescription("Number of additional inventory spaces granted to chests, racks, cabinets, and other shareable storage. Requires Modify Inventory Size to be on.",
                new AcceptableValueRange<int>(0, 500),
                new ConfigurationManagerAttributes {Order = 88, DispName = "    └ Additional Container Inventory Space"}));
        AdditionalContainerInventorySpace.SettingChanged += (_, _) =>
        {
            Fields.InventorySizesDirty = true;
            Fields.InventoriesLoaded = false;
        };

        // Legacy single slider — kept bound so it loads from existing .cfg files for migration.
        // Hidden from CM. Once MigrateLegacySlider has run, this value is unused.
        LegacyAdditionalInventorySpace = Config.Bind(InventorySection, "Additional Inventory Space", 20,
            new ConfigDescription("Legacy single slider — superseded by the Player and Container sliders above. Kept for one-time migration of existing user values.",
                new AcceptableValueRange<int>(0, 500),
                new ConfigurationManagerAttributes {Browsable = false, IsAdvanced = true, HideDefaultButton = true, ReadOnly = true}));

        ShowOnlyPersonalInventory = Config.Bind(InventorySection, "Show Only Personal Inventory", true,
            new ConfigDescription("Enable or disable showing only personal inventory.", null,
                new ConfigurationManagerAttributes {Order = 80}));
        DontShowEmptyRowsInInventory = Config.Bind(InventorySection, "Dont Show Empty Rows In Inventory", true,
            new ConfigDescription("Enable or disable displaying empty rows in the inventory.", null,
                new ConfigurationManagerAttributes {Order = 79}));
        ShowUsedSpaceInTitles = Config.Bind(InventorySection, "Show Used Space In Titles", true,
            new ConfigDescription("Enable or disable showing used space in inventory titles.", null,
                new ConfigurationManagerAttributes {Order = 78}));
        DisableInventoryDimming = Config.Bind(InventorySection, "Inventory Dimming", true,
            new ConfigDescription("Enable or disable inventory dimming.", null,
                new ConfigurationManagerAttributes {Order = 77}));
        ShowWorldZoneInTitles = Config.Bind(InventorySection, "Show World Zone In Titles", true,
            new ConfigDescription("Enable or disable showing world zone information in inventory titles.", null,
                new ConfigurationManagerAttributes {Order = 76}));
        HideInvalidSelections = Config.Bind(InventorySection, "Hide Invalid Selections", true,
            new ConfigDescription("Enable or disable hiding invalid item selections in the inventory.", null,
                new ConfigurationManagerAttributes {Order = 75}));
        RemoveGapsBetweenSections = Config.Bind(InventorySection, "Remove Gaps Between Sections", true,
            new ConfigDescription("Enable or disable removing gaps between inventory sections.", null,
                new ConfigurationManagerAttributes {Order = 74}));
        RemoveGapsBetweenSectionsVendor = Config.Bind(InventorySection, "Remove Gaps Between Sections Vendor", true,
            new ConfigDescription("Enable or disable removing gaps between sections in the vendor inventory.", null,
                new ConfigurationManagerAttributes {Order = 73}));

        // ── Item Stacking ──
        ModifyStackSize = Config.Bind(ItemStackingSection, "Modify Stack Size", true,
            new ConfigDescription("Enable or disable modifying the stack size of items.", null,
                new ConfigurationManagerAttributes {Order = 100}));
        ModifyStackSize.SettingChanged += (_, _) => Fields.StackSizesDirty = true;

        StackSizeForStackables = Config.Bind(ItemStackingSection, "Stack Size For Stackables", 999,
            new ConfigDescription("Set the maximum stack size for stackable items",
                new AcceptableValueRange<int>(1, 999),
                new ConfigurationManagerAttributes {Order = 99, DispName = "    └ Stack Size For Stackables"}));
        StackSizeForStackables.SettingChanged += (_, _) => Fields.StackSizesDirty = true;

        EnableGraveItemStacking = Config.Bind(ItemStackingSection, "Grave Item Stacking", false,
            new ConfigDescription("Allow grave items to stack", null,
                new ConfigurationManagerAttributes {Order = 98, DispName = "    └ Grave Item Stacking"}));
        EnableGraveItemStacking.SettingChanged += (_, _) => Fields.StackSizesDirty = true;

        EnablePenPaperInkStacking = Config.Bind(ItemStackingSection, "Pen Paper Ink Stacking", false,
            new ConfigDescription("Allow pen, paper, and ink items to stack", null,
                new ConfigurationManagerAttributes {Order = 97, DispName = "    └ Pen Paper Ink Stacking"}));
        EnablePenPaperInkStacking.SettingChanged += (_, _) => Fields.StackSizesDirty = true;

        EnableChiselStacking = Config.Bind(ItemStackingSection, "Chisel Stacking", false,
            new ConfigDescription("Allow chisel items to stack", null,
                new ConfigurationManagerAttributes {Order = 96, DispName = "    └ Chisel Stacking"}));
        EnableChiselStacking.SettingChanged += (_, _) => Fields.StackSizesDirty = true;

        EnableToolStacking = Config.Bind(ItemStackingSection, "Tool Stacking", true,
            new ConfigDescription("Allow tool items to stack", null,
                new ConfigurationManagerAttributes {Order = 95, DispName = "    └ Tool Stacking"}));
        EnableToolStacking.SettingChanged += (_, _) => Fields.StackSizesDirty = true;

        EnablePrayerStacking = Config.Bind(ItemStackingSection, "Prayer Stacking", true,
            new ConfigDescription("Allow prayer items to stack", null,
                new ConfigurationManagerAttributes {Order = 94, DispName = "    └ Prayer Stacking"}));
        EnablePrayerStacking.SettingChanged += (_, _) => Fields.StackSizesDirty = true;

        EnableWeaponStacking = Config.Bind(ItemStackingSection, "Weapon Stacking", true,
            new ConfigDescription("Allow weapon items to stack", null,
                new ConfigurationManagerAttributes {Order = 93, DispName = "    └ Weapon Stacking"}));
        EnableWeaponStacking.SettingChanged += (_, _) => Fields.StackSizesDirty = true;

        EnableEquipmentStacking = Config.Bind(ItemStackingSection, "Equipment Stacking", true,
            new ConfigDescription("Allow equipment items to stack", null,
                new ConfigurationManagerAttributes {Order = 92, DispName = "    └ Equipment Stacking"}));
        EnableEquipmentStacking.SettingChanged += (_, _) => Fields.StackSizesDirty = true;

        // ── Gameplay ──
        AllowHandToolDestroy = Config.Bind(GameplaySection, "Allow Hand Tool Destroy", true,
            new ConfigDescription("Enable or disable destroying objects with hand tools", null,
                new ConfigurationManagerAttributes {Order = 100}));
        AllowHandToolDestroy.SettingChanged += (_, _) => Fields.ToolDestroyDirty = true;

        CollectDropsOnGameLoad = Config.Bind(GameplaySection, "Collect Drops On Game Load", true,
            new ConfigDescription("Enable or disable collecting drops on game load", null,
                new ConfigurationManagerAttributes {Order = 99}));

        // ── UI ──
        HideStockpileWidgets = Config.Bind(UISection, "Hide Stockpile Widgets", true,
            new ConfigDescription("Enable or disable hiding stockpile widgets", null,
                new ConfigurationManagerAttributes {Order = 100}));
        HideTavernWidgets = Config.Bind(UISection, "Hide Tavern Widgets", true,
            new ConfigDescription("Enable or disable hiding tavern widgets", null,
                new ConfigurationManagerAttributes {Order = 99}));
        HideSoulWidgets = Config.Bind(UISection, "Hide Soul Widgets", true,
            new ConfigDescription("Enable or disable hiding soul widgets", null,
                new ConfigurationManagerAttributes {Order = 98}));
        HideWarehouseShopWidgets = Config.Bind(UISection, "Hide Warehouse Shop Widgets", true,
            new ConfigDescription("Enable or disable hiding warehouse shop widgets", null,
                new ConfigurationManagerAttributes {Order = 97}));

        Fields.GameBalanceAlreadyRun = false;
    }


    public void Update()
    {
        if (!MainGame.game_started) return;

        if (Fields.InventorySizesDirty && !Fields.ShrinkDialogOpen)
        {
            // Drain only when no shrink dialog is on screen — otherwise we'd queue up another
            // plan against the user's pending answer. The dialog's callbacks clear ShrinkDialogOpen.
            Fields.InventorySizesDirty = false;
            Helpers.UpdateInventorySizes();
            Helpers.HandleInventorySizesDirty();
        }

        if (Fields.StackSizesDirty)
        {
            Fields.StackSizesDirty = false;
            Helpers.UpdateStackSizes();
        }

        if (Fields.ToolDestroyDirty)
        {
            Fields.ToolDestroyDirty = false;
            Helpers.UpdateToolDestroy();
        }

        // Per-frame natural-shrink + legacy-recovery safety net for the player.
        // Cheap (one int compare and a branch) when nothing's changed.
        Helpers.ApplyPlayerInventorySize();

        if (Fields.InventoriesLoaded) return;
        Log.LogInfo("Refreshing inventories...");
        Helpers.RunWmsTasks();
    }
}
