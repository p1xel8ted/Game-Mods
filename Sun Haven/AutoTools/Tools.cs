

// ReSharper disable SuggestBaseTypeForParameter

namespace AutoTools;

[Harmony]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class Tools
{
    private const string NoSuitableToolFoundOnActionBar = "No suitable tool found on action bar!";
    private const string NoPickaxeOnActionBar = "No pickaxe on action bar!";
    private const string NoAxeOnActionBar = "No axe on action bar!";
    private const string Prop = "prop";
    private const string Foliage = "foliage";
    private const string NoWateringCanOnActionBar = "No watering can on action bar!";
    private const string NoScytheOnActionBar = "No scythe on action bar!";
    private const string NoHoeOnActionBar = "No hoe on action bar!";
    private const string NoFishingRodOnActionBar = "No fishing rod on action bar!";
    internal const string YourWateringCanIsEmpty = "Your watering can is empty!";
    private static int WateringCanIndex { get; set; } = -1;
    private static Rock Rock { get; set; }
    private static Tree Tree { get; set; }
    private static Crop Crop { get; set; }
    private static Wood Wood { get; set; }
    private static Plant Plant { get; set; }
    private static WaterSlime WaterSlime { get; set; }

    internal static int GetBestWateringCanId()
    {
        var toolDict = ReadOnlyDictionary(Tool.WateringCan);
        foreach (var toolEntry in toolDict.OrderByDescending(a => a.Key))
        {
            foreach (var item in Player.Instance.PlayerInventory._actionBarIcons.Where(a => a.ItemImage is not null))
            {
                var toolData = ItemDatabase.GetItemData(item.ItemImage.item) as ToolData;
                if (toolData == null || toolData.id != toolEntry.Key || !Utils.CanUse(toolData)) continue;

                if (item.ItemImage.item is WateringCanItem wc)
                {
                    return wc.id;
                }
            }
        }

        return 0;
    }

    internal static (int index, ToolData toolData) FindBestTool(Tool tool)
    {
        var toolDict = ReadOnlyDictionary(tool);

        // Loop through each tool entry starting from the highest
        foreach (var toolEntry in toolDict.OrderByDescending(a => a.Key))
        {
            foreach (var item in Player.Instance.PlayerInventory._actionBarIcons.Where(a => a.ItemImage is not null))
            {
                var toolData = ItemDatabase.GetItemData(item.ItemImage.item) as ToolData;
                if (toolData == null || toolData.id != toolEntry.Key || !Utils.CanUse(toolData))
                    continue;

                if (tool == Tool.WateringCan)
                {
                    if (item.ItemImage.item is WateringCanItem {WaterAmount: <= 0})
                    {
                        Plugin.DebugLog($"Found a {toolEntry.Value} but it's empty!");
                        continue;
                    }

                    WateringCanIndex = item.ItemImage.slotIndex;
                    Plugin.DebugLog($"Found a {toolEntry.Value} with water. Setting as best tool.");
                    return (item.ItemImage.slotIndex, toolData);
                }

                Plugin.DebugLog($"Found a suitable {toolEntry.Value}. Setting as best tool.");
                return (item.ItemImage.slotIndex, toolData);
            }
        }

        // Specific check for watering can if we didn't find any with water above
        if (tool == Tool.WateringCan)
        {
            Plugin.DebugLog("No filled watering cans found. Searching for the best empty one.");
            foreach (var toolEntry in toolDict.OrderByDescending(a => a.Key))
            {
                foreach (var item in Player.Instance.PlayerInventory._actionBarIcons.Where(a => a.ItemImage is not null))
                {
                    var toolData = ItemDatabase.GetItemData(item.ItemImage.item) as ToolData;
                    if (toolData == null || toolData.id != toolEntry.Key || !Utils.CanUse(toolData))
                        continue;

                    WateringCanIndex = item.ItemImage.slotIndex;
                    return (item.ItemImage.slotIndex, toolData);
                }
            }
        }

        Plugin.DebugLog("No suitable tool found on action bar!");
        return (-1, null);
    }

    private static ReadOnlyDictionary<int, string> ReadOnlyDictionary(Tool tool)
    {
        var toolDict = tool switch
        {
            Tool.Pickaxe => ToolDictionaries.PickAxes,
            Tool.Axe => ToolDictionaries.Axes,
            Tool.Scythe => ToolDictionaries.Scythes,
            Tool.FishingRod => ToolDictionaries.FishingRods,
            Tool.WateringCan => ToolDictionaries.WateringCans,
            Tool.Hoe => ToolDictionaries.Hoes,
            _ => throw new ArgumentOutOfRangeException(nameof(tool), tool, null)
        };
        return toolDict;
    }


    private static void HandleToolInteraction(int toolIndex, int toolDataID, string errorMessage)
    {
        if (toolIndex != -1)
        {
            Utilities.SetActionBar(toolIndex);
        }
        else
        {
            Utilities.Notify(errorMessage, toolDataID, true);
        }
    }


    internal static void RunToolActions(Collider2D collider)
    {
        ToolAction(Tool.Pickaxe, Rock is not null && Rock.Pickaxeable, Plugin.EnableAutoPickaxe.Value, NoPickaxeOnActionBar);

        ToolAction(Tool.Axe, (Utilities.IsInFarmTile() && Tree is not null && !Tree.FullyGrown) || (Tree is not null && Tree.Axeable) || (Wood is not null && Wood.Axeable), Plugin.EnableAutoAxe.Value, NoAxeOnActionBar);
        ToolAction(Tool.Scythe, Plant is not null || (collider.name.Contains(Foliage) && !collider.name.Contains(Prop)) || (Crop is not null && Crop.FullyGrown), Plugin.EnableAutoScythe.Value, NoScytheOnActionBar);
        ToolAction(Tool.FishingRod, Vector2.Distance(Player.Instance.ExactGraphicsPosition, Wish.Utilities.MousePositionFloat()) < Plugin.FishingRodWaterDetectionDistance.Value && SingletonBehaviour<GameManager>.Instance.HasWater(new Vector2Int((int) Wish.Utilities.MousePositionFloat().x, (int) Wish.Utilities.MousePositionFloat().y)), Plugin.EnableAutoFishingRod.Value, NoFishingRodOnActionBar);
        ToolAction(Tool.Hoe, TileManager.Instance.IsHoeable(new Vector2Int((int) Player.Instance.ExactGraphicsPosition.x, (int) Player.Instance.ExactGraphicsPosition.y)), Plugin.EnableAutoHoe.Value, NoHoeOnActionBar);

        FindBestTool(Tool.WateringCan); //this is here to update the watering can index prior to running the conditions below
        ToolAction(Tool.WateringCan, !WateringCanHasWater(true) && (WateringCan.OverWaterSource || Vector2.Distance(Player.Instance.ExactGraphicsPosition, Wish.Utilities.MousePositionFloat()) < 10 && SingletonBehaviour<GameManager>.Instance.HasWater(new Vector2Int((int) Wish.Utilities.MousePositionFloat().x, (int) Wish.Utilities.MousePositionFloat().y))), Plugin.EnableAutoWateringCan.Value, NoWateringCanOnActionBar);
        ToolAction(Tool.WateringCan, WaterSlime != null || Crop is not null && Crop._seedItem != null && (!Crop.data.watered || Crop.data.onFire), Plugin.EnableAutoWateringCan.Value, NoWateringCanOnActionBar, () => WateringCanHasWater());
    }


    internal static void UpdateColliders(Collider2D collider)
    {
        Rock = collider.GetComponent<Rock>();
        Tree = collider.GetComponent<Tree>();
        Crop = collider.GetComponent<Crop>();
        Wood = collider.GetComponent<Wood>();
        Plant = collider.GetComponent<Plant>();
        WaterSlime = collider.GetComponent<WaterSlime>();
    }

    internal static bool EnableToolSwaps(PlayerInteractions __instance, Collider2D collider)
    {
        Plugin.DebugLog("EnableToolSwaps called.");
        
        if (Plugin.EnableEnemyDetection.Value)
        {
            Plugin.DebugLog("Enemy detection is enabled.");
            
            if (Plugin.UseCombatRange.Value)
            {
                var distance = Patches.ClosestDistance;
                var limit = Plugin.CombatRange.Value;
                if (distance <= limit)
                {
                    Plugin.DebugLog($"Closest distance ({distance}) is within combat range ({limit}). Returning false.");
                    
                    return false;
                }
            }
            else
            {
                Plugin.DebugLog("Not using combat range.");
                
                if (Patches.EnemyInArea)
                {
                    Plugin.DebugLog("Enemy in area. Returning false.");
                    
                    return false;
                }
            }
        }
        
        var isValid = __instance != null && collider != null && SceneSettingsManager.Instance != null && TileManager.Instance != null && !Player.Instance.InCombat;
        
        Plugin.DebugLog($"Validity check: {isValid}. Returning {isValid}.");
        return isValid;
    }


    private static bool WateringCanHasWater(bool refill = false)
    {
        if (WateringCanIndex == -1)
        {
            Plugin.DebugLog("No watering can on action bar!");
            return false;
        }

        var item = Player.Instance.PlayerInventory._actionBarIcons[WateringCanIndex].ItemImage.item;
        if (item is not WateringCanItem wc)
        {
            return false;
        }

        var maxWater = ItemDatabase.GetItemData<WateringCanData>(wc).waterCapacity;
        var currentPercent = (float) wc.WaterAmount / maxWater * 100;
        //Plugin.LOG.LogWarning($"Watering can has {wc.WaterAmount}/{maxWater} water ({currentPercent}%). Threshold is {Plugin.WateringCanFillThreshold.Value}%.");
        if (refill && currentPercent <= Plugin.WateringCanFillThreshold.Value)
        {
            return false;
        }

        return wc.WaterAmount > 0;
    }

    private static void ToolAction(Tool tool, bool condition, bool pluginValue, string errorMessage, Func<bool> additionalCondition = null, string failedConditionMessage = null)
    {
        if (!condition || !pluginValue) return;

        if (additionalCondition != null && !additionalCondition())
        {
            if (!string.IsNullOrEmpty(failedConditionMessage))
            {
                Utilities.Notify(failedConditionMessage, 0, true);
            }

            return;
        }


        var toolData = FindBestTool(tool);
        if (toolData.toolData != null)
        {
            if (Utils.CanUse(toolData.toolData))
            {
                HandleToolInteraction(toolData.index, toolData.toolData.id, errorMessage);
            }
            else
            {
                Utilities.Notify(ToolLevelTooLow(toolData.toolData), toolData.toolData.id, true);
            }
        }
        else
        {
            Utilities.Notify(NoSuitableToolFoundOnActionBar, 0, true);
        }
    }

    private static string ToolLevelTooLow(ToolData toolData)
    {
        Plugin.DebugLog($"Your {toolData.profession} level is too low to use {toolData.name}!");
        return $"Your {toolData.profession} level is too low to use {toolData.name}!";
    }


    internal enum Tool
    {
        Pickaxe,
        Axe,
        Scythe,
        FishingRod,
        WateringCan,
        Hoe,
    }
}