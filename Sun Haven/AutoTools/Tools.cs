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

    // Legacy proximity-based detection state
    private static Rock Rock { get; set; }
    private static Tree Tree { get; set; }
    private static Crop Crop { get; set; }
    private static Wood Wood { get; set; }
    private static Plant Plant { get; set; }
    private static WaterSlime WaterSlime { get; set; }

    // Cursor-based detection state
    private static float _lastSwitchTime;
    private static Tool? _lastDetectedTool;
    private const float MaxAimDistance = 3f;

    #region Cursor-Based Detection

    internal static void DetectToolAtCursor()
    {
        if (!Plugin.EnableAutoTool.Value) return;
        if (Player.Instance == null || Player.Instance.InCombat) return;
        if (SceneSettingsManager.Instance == null || TileManager.Instance == null) return;
        if (SingletonBehaviour<GameManager>.Instance == null) return;

        if (!ShouldAllowSwitch()) return;

        if (Time.time - _lastSwitchTime < Plugin.SwitchCooldown.Value) return;

        var aimPos = Wish.Utilities.ExtendedMousePositionFloat();
        var playerPos = Player.Instance.ExactGraphicsPosition;

        // Don't switch if aiming too far from player (beyond tool reach)
        if ((aimPos - playerPos).magnitude > MaxAimDistance) return;

        // Don't switch on farm tiles unless enabled
        if (Utils.IsInFarmTile() && !Plugin.EnableAutoToolOnFarmTiles.Value) return;

        var detected = DetectToolForPosition(aimPos);

        if (detected == null) return;
        if (detected == _lastDetectedTool) return;

        SwitchToTool(detected.Value);
        _lastDetectedTool = detected;
        _lastSwitchTime = Time.time;
    }

    private static Tool? DetectToolForPosition(Vector2 aimPos)
    {
        var tilePos = new Vector2Int((int) aimPos.x, (int) aimPos.y);
        var gm = SingletonBehaviour<GameManager>.Instance;

        // Check decorations at aim position using subtile grid (small area around cursor)
        var centerSubX = (int) (aimPos.x * 6f);
        var centerSubY = (int) (aimPos.y * 6f);
        const int searchRadius = 4;

        Decoration closestPickaxeable = null;
        Decoration closestAxeable = null;
        Crop closestCrop = null;
        Plant closestPlant = null;
        WaterSlime closestWaterSlime = null;
        var closestPickDist = float.MaxValue;
        var closestAxeDist = float.MaxValue;
        var closestCropDist = float.MaxValue;
        var closestPlantDist = float.MaxValue;
        var closestSlimeDist = float.MaxValue;

        for (var dx = -searchRadius; dx <= searchRadius; dx++)
        {
            for (var dy = -searchRadius; dy <= searchRadius; dy++)
            {
                var subX = centerSubX + dx;
                var subY = centerSubY + dy;
                var worldPos = new Vector2(subX / 6f, subY / 6f);
                var dist = (worldPos - aimPos).sqrMagnitude;

                for (var z = 1; z >= -1; z--)
                {
                    var subTile = new Vector3Int(subX, subY, z);

                    if (gm.TryGetObjectSubTile(subTile, out Rock rock) && rock != null && rock.Pickaxeable)
                    {
                        if (dist < closestPickDist)
                        {
                            closestPickaxeable = rock;
                            closestPickDist = dist;
                        }
                    }

                    if (gm.TryGetObjectSubTile(subTile, out Tree tree) && tree != null)
                    {
                        if (tree.Axeable || (Utils.IsInFarmTile() && !tree.FullyGrown))
                        {
                            if (dist < closestAxeDist)
                            {
                                closestAxeable = tree;
                                closestAxeDist = dist;
                            }
                        }
                    }

                    if (gm.TryGetObjectSubTile(subTile, out Wood wood) && wood != null && wood.Axeable)
                    {
                        if (dist < closestAxeDist)
                        {
                            closestAxeable = wood;
                            closestAxeDist = dist;
                        }
                    }

                    if (gm.TryGetObjectSubTile(subTile, out Crop crop) && crop != null)
                    {
                        if (dist < closestCropDist)
                        {
                            closestCrop = crop;
                            closestCropDist = dist;
                        }
                    }

                    if (gm.TryGetObjectSubTile(subTile, out Plant plant) && plant != null)
                    {
                        if (dist < closestPlantDist)
                        {
                            closestPlant = plant;
                            closestPlantDist = dist;
                        }
                    }

                    if (gm.TryGetObjectSubTile(subTile, out WaterSlime waterSlime) && waterSlime != null)
                    {
                        if (dist < closestSlimeDist)
                        {
                            closestWaterSlime = waterSlime;
                            closestSlimeDist = dist;
                        }
                    }
                }
            }
        }

        // Priority-based tool selection

        // 1. Pickaxe for rocks
        if (closestPickaxeable != null && Plugin.EnableAutoPickaxe.Value)
        {
            Plugin.DebugLog("Cursor: Detected pickaxeable object.");
            return Tool.Pickaxe;
        }

        // 2. Axe for trees/wood
        if (closestAxeable != null && Plugin.EnableAutoAxe.Value)
        {
            Plugin.DebugLog("Cursor: Detected axeable object.");
            return Tool.Axe;
        }

        // 3. Scythe for fully grown crops or plants
        if (closestCrop is {FullyGrown: true} && Plugin.EnableAutoScythe.Value)
        {
            Plugin.DebugLog("Cursor: Detected fully grown crop.");
            return Tool.Scythe;
        }

        if (closestPlant != null && Plugin.EnableAutoScythe.Value)
        {
            Plugin.DebugLog("Cursor: Detected plant.");
            return Tool.Scythe;
        }

        // 4. Watering can for crops that need water, water slimes, or waterable tiles
        if (closestWaterSlime != null && Plugin.EnableAutoWateringCan.Value)
        {
            Plugin.DebugLog("Cursor: Detected water slime.");
            return Tool.WateringCan;
        }

        if (closestCrop is {_seedItem: not null} && (!closestCrop.data.watered || closestCrop.data.onFire) && Plugin.EnableAutoWateringCan.Value)
        {
            Plugin.DebugLog("Cursor: Detected crop needing water.");
            return Tool.WateringCan;
        }

        if (TileManager.Instance.IsWaterable(tilePos) && Plugin.EnableAutoWateringCan.Value)
        {
            Plugin.DebugLog("Cursor: Detected waterable tile.");
            return Tool.WateringCan;
        }

        // 5. Hoe for hoeable ground
        if (TileManager.Instance.IsHoeable(tilePos) && Plugin.EnableAutoHoe.Value)
        {
            Plugin.DebugLog("Cursor: Detected hoeable tile.");
            return Tool.Hoe;
        }

        // 6. Water detection — fishing rod or watering can refill
        if (gm.HasWater(tilePos))
        {
            FindBestTool(Tool.WateringCan);
            if (!WateringCanHasWater(true) && Plugin.EnableAutoWateringCan.Value)
            {
                Plugin.DebugLog("Cursor: Detected water — watering can needs refill.");
                return Tool.WateringCan;
            }

            if (Plugin.EnableAutoFishingRod.Value)
            {
                Plugin.DebugLog("Cursor: Detected water — fishing rod.");
                return Tool.FishingRod;
            }
        }

        // Nothing detected — clear last detection so we can re-detect when aiming at something new
        _lastDetectedTool = null;
        return null;
    }

    internal static void ResetCursorDetection()
    {
        _lastDetectedTool = null;
    }

    #endregion

    #region Shared

    private static bool ShouldAllowSwitch()
    {
        if (!Plugin.EnableEnemyDetection.Value) return true;

        if (Plugin.UseCombatRange.Value)
        {
            var distance = Patches.ClosestDistance;
            var limit = Plugin.CombatRange.Value;
            if (distance <= limit)
            {
                Plugin.DebugLog($"Closest distance ({distance}) is within combat range ({limit}). Blocking switch.");
                return false;
            }
        }
        else if (Patches.EnemyInArea)
        {
            Plugin.DebugLog("Enemy in area. Blocking switch.");
            return false;
        }

        return true;
    }

    private static void SwitchToTool(Tool tool)
    {
        var result = FindBestTool(tool);
        if (result.toolData == null) return;

        if (Utils.CanUse(result.toolData))
        {
            Utils.SetActionBar(result.index);
        }
    }

    #endregion

    #region Tool Lookup

    internal static int GetBestWateringCanId()
    {
        var toolDict = ReadOnlyDictionary(Tool.WateringCan);
        foreach (var toolEntry in toolDict.OrderByDescending(a => a.Key))
        {
            foreach (var item in Player.Instance.PlayerInventory._actionBarIcons.Where(a => a.ItemImage is not null))
            {
                var toolData = Utils.GetItemData(item.ItemImage.item.ID()) as ToolData;
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
        if (tool == Tool.WateringCan)
        {
            WateringCanIndex = -1;
        }

        var toolDict = ReadOnlyDictionary(tool);

        foreach (var toolEntry in toolDict.OrderByDescending(a => a.Key))
        {
            foreach (var item in Player.Instance.PlayerInventory._actionBarIcons.Where(a => a.ItemImage is not null))
            {
                var toolData = Utils.GetItemData(item.ItemImage.item.ID()) as ToolData;
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

        if (tool == Tool.WateringCan)
        {
            Plugin.DebugLog("No filled watering cans found. Searching for the best empty one.");
            foreach (var toolEntry in toolDict.OrderByDescending(a => a.Key))
            {
                foreach (var item in Player.Instance.PlayerInventory._actionBarIcons.Where(a => a.ItemImage is not null))
                {
                    var toolData = Utils.GetItemData(item.ItemImage.item.ID()) as ToolData;
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

    #endregion

    #region Legacy Proximity Detection

    private static void HandleToolInteraction(int toolIndex, int toolDataID, string errorMessage)
    {
        if (toolIndex != -1)
        {
            Utils.SetActionBar(toolIndex);
        }
        else
        {
            Utils.Notify(errorMessage, toolDataID, true);
        }
    }

    internal static void RunToolActions(Collider2D collider)
    {
        ToolAction(Tool.Pickaxe, Rock is not null && Rock.Pickaxeable, Plugin.EnableAutoPickaxe.Value, NoPickaxeOnActionBar);

        ToolAction(Tool.Axe, (Utils.IsInFarmTile() && Tree is not null && !Tree.FullyGrown) || (Tree is not null && Tree.Axeable) || (Wood is not null && Wood.Axeable), Plugin.EnableAutoAxe.Value, NoAxeOnActionBar);
        ToolAction(Tool.Scythe, Plant is not null || (collider.name.Contains(Foliage) && !collider.name.Contains(Prop)) || (Crop is not null && Crop.FullyGrown), Plugin.EnableAutoScythe.Value, NoScytheOnActionBar);
        ToolAction(Tool.FishingRod, TileHasWater(Plugin.FishingRodWaterDetectionDistance.Value), Plugin.EnableAutoFishingRod.Value, NoFishingRodOnActionBar);
        ToolAction(Tool.Hoe, TileManager.Instance.IsHoeable(new Vector2Int((int) Player.Instance.ExactGraphicsPosition.x, (int) Player.Instance.ExactGraphicsPosition.y)), Plugin.EnableAutoHoe.Value, NoHoeOnActionBar);

        FindBestTool(Tool.WateringCan);
        ToolAction(Tool.WateringCan, !WateringCanHasWater(true) && WateringCanOverWater(), Plugin.EnableAutoWateringCan.Value, NoWateringCanOnActionBar);
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

        if (!ShouldAllowSwitch()) return false;

        var isValid = __instance != null && collider != null && SceneSettingsManager.Instance != null && TileManager.Instance != null && !Player.Instance.InCombat;

        Plugin.DebugLog($"Validity check: {isValid}. Returning {isValid}.");
        return isValid;
    }

    private static bool TileHasWater(int magnitude = 10)
    {
        var vector = Wish.Utilities.ExtendedMousePositionFloat();
        return (vector - Player.Instance.ExactGraphicsPosition).magnitude < magnitude && SingletonBehaviour<GameManager>.Instance.HasWater(new Vector2Int((int) vector.x, (int) vector.y));
    }

    private static bool WateringCanOverWater()
    {
        if (WateringCan.OverWaterSource || TileHasWater())
        {
            return true;
        }
        return false;
    }

    private static bool WateringCanHasWater(bool refill = false)
    {
        if (WateringCanIndex == -1)
        {
            Plugin.DebugLog("No watering can on action bar!");
            return false;
        }

        var icons = Player.Instance.PlayerInventory._actionBarIcons;
        if (WateringCanIndex < 0 || WateringCanIndex >= icons.Count) return false;

        var icon = icons[WateringCanIndex];
        if (icon?.ItemImage?.item is not WateringCanItem wc) return false;

        var wcData = Utils.GetItemData(wc.id) as WateringCanData;
        if (wcData == null || wcData.waterCapacity <= 0) return false;

        var currentPercent = (float) wc.WaterAmount / wcData.waterCapacity * 100;
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
                Utils.Notify(failedConditionMessage, 0, true);
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
                Utils.Notify(ToolLevelTooLow(toolData.toolData), toolData.toolData.id, true);
            }
        }
        else
        {
            Utils.Notify(NoSuitableToolFoundOnActionBar, 0, true);
        }
    }

    private static string ToolLevelTooLow(ToolData toolData)
    {
        Plugin.DebugLog($"Your {toolData.profession} level is too low to use {toolData.name}!");
        return $"Your {toolData.profession} level is too low to use {toolData.name}!";
    }

    #endregion

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
