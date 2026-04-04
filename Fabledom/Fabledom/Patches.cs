namespace Fabledom;

/// <summary>
/// Harmony patches for dynamically adjusting canvas scaler screen match modes based on aspect ratio.
/// Switches to Expand mode for ultra-wide displays to prevent UI stretching.
/// </summary>
[Harmony]
public static class Patches
{

    /// <summary>
    /// Stores the original screen match mode for each canvas scaler by instance ID.
    /// Allows restoration to original mode when returning to standard aspect ratios.
    /// </summary>
    private static readonly Dictionary<int, CanvasScaler.ScreenMatchMode> OriginalScreenMatchModes = new();

    /// <summary>
    /// List of all canvas scalers found in the scene.
    /// Tracked to allow updating when aspect ratio changes.
    /// </summary>
    private static readonly List<CanvasScaler> CanvasScalers = [];

    /// <summary>
    /// Postfix patch that registers canvas scalers and applies appropriate screen match mode.
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(CanvasScaler), nameof(CanvasScaler.OnEnable))]
    public static void CanvasScaler_OnEnable(CanvasScaler __instance)
    {
        if (__instance.name.Contains("sinai")) return;

        CanvasScalers.Add(__instance);
        UpdateScalers(Plugin.CurrentAspect);
    }

    /// <summary>
    /// Adds a "Continue" button to the main menu that loads the most recently saved game.
    /// Clones the existing Load button for consistent styling, positioned above it.
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(MainMenuSceneManager), nameof(MainMenuSceneManager.Start))]
    public static void MainMenuSceneManager_Start(MainMenuSceneManager __instance)
    {
        var usedSlots = SaveMaster.GetUsedSlots();
        if (usedSlots.Length == 0) return;

        var mostRecentSlot = -1;
        var mostRecentTime = System.DateTime.MinValue;

        foreach (var slot in usedSlots)
        {
            SaveMaster.GetMetaData("lastsavedtime", out string timeStr, slot);
            if (string.IsNullOrEmpty(timeStr)) continue;

            if (System.DateTime.TryParseExact(timeStr, "yy-MM-dd HH:mm:ss",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out var time) && time > mostRecentTime)
            {
                mostRecentTime = time;
                mostRecentSlot = slot;
            }
        }

        if (mostRecentSlot == -1) return;

        var loadButtonGo = __instance.loadButton.gameObject;
        var parent = loadButtonGo.transform.parent;

        // Destroy existing clone if returning to menu
        var existing = parent.Find("ContinueButton");
        if (existing) Object.Destroy(existing.gameObject);

        var continueGo = Object.Instantiate(loadButtonGo, parent);
        continueGo.name = "ContinueButton";
        continueGo.transform.SetSiblingIndex(loadButtonGo.transform.GetSiblingIndex());

        var tmpText = continueGo.GetComponentInChildren<TMP_Text>();
        if (tmpText)
        {
            // Destroy localization component that would overwrite our text
            foreach (var comp in tmpText.GetComponents<Component>())
            {
                if (comp.GetType().Name.Contains("Localize"))
                {
                    Object.Destroy(comp);
                }
            }
            tmpText.text = "Continue";
        }

        var button = continueGo.GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        var slotToLoad = mostRecentSlot;
        button.onClick.AddListener(() =>
        {
            DataManager.Instance.isLoaded = true;
            MainMenuSceneManager.Instance.LoadGame(slotToLoad);
        });

        Plugin.Log.LogInfo($"Continue button added (slot {mostRecentSlot}, saved {mostRecentTime:yy-MM-dd HH:mm:ss})");
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(AstarAgent), nameof(AstarAgent.UpdateSpeedMultiplier))]
    public static void AstarAgent_UpdateSpeedMultiplier(AstarAgent __instance)
    {
        __instance.agent.maxSpeed *= Plugin.WalkSpeedMultiplier.Value;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(AstarAgent), nameof(AstarAgent.RotateInMovementDirection))]
    public static bool AstarAgent_RotateInMovementDirection(AstarAgent __instance)
    {
        var forward = __instance.transform.position - __instance.lastPosition;
        if (forward == Vector3.zero) return false;
        var turnSpeed = __instance.movementConfig.turnSpeed * Plugin.WalkSpeedMultiplier.Value;
        __instance.transform.rotation = Quaternion.Euler(0f,
            Quaternion.Slerp(__instance.transform.rotation, Quaternion.LookRotation(forward),
                Time.deltaTime * turnSpeed).eulerAngles.y, 0f);
        return false;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(DateTimeManager), nameof(DateTimeManager.GetDaysInSeason))]
    public static void DateTimeManager_GetDaysInSeason(ref int __result)
    {
        __result = Mathf.Max(1, Mathf.RoundToInt(__result * Plugin.SeasonLengthMultiplier.Value));
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(DateTimeManager), nameof(DateTimeManager.Start))]
    public static void DateTimeManager_Start(DateTimeManager __instance)
    {
        Plugin.BaseDeltaTimeInDay = __instance.deltaTimeInDay;
        __instance.deltaTimeInDay = Plugin.BaseDeltaTimeInDay * Plugin.DayLengthMultiplier.Value;

        __instance.OnSeasonChanged += () =>
        {
            var mode = Plugin.WorkerOptimizeMode.Value;
            if (mode is WorkerOptimizeTrigger.SeasonChange or WorkerOptimizeTrigger.All)
            {
                GameManager.Instance.StartCoroutine(WorkerOptimizer.OptimizeCoroutine());
            }
        };

        __instance.OnDayIncreased += () =>
        {
            if (Plugin.BottleneckAlertEnabled.Value) RunBottleneckCheck();
        };
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(KingdomManager), nameof(KingdomManager.Immigration))]
    public static bool KingdomManager_Immigration() => Plugin.ImmigrationEnabled.Value;

    [HarmonyPostfix]
    [HarmonyPatch(typeof(KingdomManager), nameof(KingdomManager.Start))]
    public static void KingdomManager_Start(KingdomManager __instance)
    {
        __instance.OnImmigrationComplete += AutoAssignUnemployed;
    }

    private static void AutoAssignUnemployed()
    {
        if (!Plugin.AutoAssignImmigrants.Value) return;

        var assigned = 0;
        foreach (var worker in Nielsen.Worker.Instances.ToArray())
        {
            if (worker.fablingClass == FablingClass.FAIRY) continue;
            if (worker.HasWorkPlace()) continue;
            if (worker.resident == null || !worker.resident.HasHousing()) continue;

            Nielsen.WorkPlace bestWp = null;
            var bestDist = float.MaxValue;

            foreach (var wp in Nielsen.WorkPlace.Instances)
            {
                if (!wp.HasCapacity()) continue;
                if (!wp.gridObject.wo.data.workPlaceFablingClasses.Contains(worker.fablingClass)) continue;

                var dist = worker.GetDistanceBetweenHomeAndWork(wp);
                if (dist < bestDist)
                {
                    bestDist = dist;
                    bestWp = wp;
                }
            }

            if (bestWp == null) continue;

            var name = worker.unit.GetLocalizedBirthName();
            var wpName = bestWp.worldObject.data.title.GetLocalizedString();
            Plugin.Log.LogInfo($"Auto-assigned {name} ({worker.fablingClass}) -> {wpName} (dist: {bestDist:F0})");
            worker.StartProfession(bestWp);
            assigned++;
        }

        if (assigned > 0)
        {
            Plugin.Log.LogInfo($"Auto-assigned {assigned} immigrant(s) to jobs");
        }
    }

    internal static void RunBottleneckCheck()
    {
        var issueNames = new List<string>();
        foreach (var wp in Nielsen.WorkPlace.Instances)
        {
            var pp = wp.GetComponent<Nielsen.ProductionPlace>();
            if (!pp) continue;

            var name = wp.worldObject.data.title.GetLocalizedString();

            if (!wp.HasWorker())
            {
                Plugin.Log.LogWarning($"[Bottleneck] UNDERSTAFFED: {name} has 0/{wp.GetTotalCapacity()} workers");
                issueNames.Add($"{name} (understaffed)");
            }
            else if (!pp.HasEnoughInput())
            {
                Plugin.Log.LogWarning($"[Bottleneck] NO INPUT: {name} missing input resources");
                issueNames.Add($"{name} (no input)");
            }
            else if (!pp.HasRoomForOutput())
            {
                Plugin.Log.LogWarning($"[Bottleneck] FULL OUTPUT: {name} output storage full");
                issueNames.Add($"{name} (full output)");
            }
            else if (!pp.IsProductionAllowed())
            {
                Plugin.Log.LogWarning($"[Bottleneck] CAPPED: {name} at production limit");
                issueNames.Add($"{name} (capped)");
            }
        }

        if (issueNames.Count > 0)
        {
            Plugin.Log.LogWarning($"[Bottleneck] {issueNames.Count} production issue(s) detected");
            WorkerOptimizer.ShowModNotification($"Production issues: {string.Join(", ", issueNames)}");
        }
        else
        {
            Plugin.Log.LogInfo("[Bottleneck] No production issues found");
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Nielsen.Constructable), nameof(Nielsen.Constructable.Start))]
    public static void Constructable_Start(Nielsen.Constructable __instance)
    {
        __instance.OnConstructionComplete += () =>
        {
            if (__instance.worldObject.data.workerCapacity <= 0) return;

            var mode = Plugin.WorkerOptimizeMode.Value;
            if (mode is WorkerOptimizeTrigger.NewBuilding or WorkerOptimizeTrigger.All)
            {
                GameManager.Instance.StartCoroutine(WorkerOptimizer.OptimizeCoroutine());
            }
        };
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(UnitBottomBar), nameof(UnitBottomBar.HandleStuckButtonClick))]
    public static void UnitBottomBar_HandleStuckButtonClick(UnitBottomBar __instance)
    {
        if (!__instance.resident) return;

        var worker = __instance.resident.worker;
        var ai = __instance.resident.unit.ai;

        // Clear stale pathfinding state
        ai.isStuck = false;
        ai.isPreStuck = false;
        ai.timeStuck = 0f;

        // Clear stale resource reservations
        if (worker != null)
        {
            worker.ClearResourceReservations(true);
            // Reset behavior tree to get a fresh task
            worker.unit.btc.current.SendEvent("professionChanged");
            Plugin.Log.LogInfo($"Unstuck + reset: {worker.unit.GetLocalizedBirthName()}");
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GatherableBottomBar), nameof(GatherableBottomBar.OnEnable))]
    public static void GatherableBottomBar_OnEnable(GatherableBottomBar __instance)
    {
        if (!__instance.selection) return;

        var destroyBtnGo = __instance.destroyBtn.gameObject;
        var parent = destroyBtnGo.transform.parent;

        var existing = parent.Find("RelocateButton");
        if (existing) Object.Destroy(existing.gameObject);

        var relocateGo = Object.Instantiate(destroyBtnGo, parent);
        relocateGo.name = "RelocateButton";
        relocateGo.transform.SetSiblingIndex(destroyBtnGo.transform.GetSiblingIndex() + 1);
        __instance.confirmationPanel.SetActive(false);

        // Strip ALL children except the one containing the label text
        for (var i = relocateGo.transform.childCount - 1; i >= 0; i--)
        {
            var child = relocateGo.transform.GetChild(i);
            if (!child.GetComponentInChildren<TMP_Text>())
            {
                Object.Destroy(child.gameObject);
            }
        }

        var iconText = relocateGo.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        if (iconText) iconText.text = "2";

        var relocateCost = KingdomManager.Instance.kingdomConfig.resourceDestroyCost * 2;
        var canAfford = GameManager.Instance.IsSandboxMode(false) || Utils_Resources.HasInGlobalStorage("coin", relocateCost);
        var costText = $"{Utils_Text.FontSprite("_coin")}{Utils_Text.HighlightOrangeBold(relocateCost.ToString())}";

        var tooltip = relocateGo.GetComponent<ShowTooltip>();
        if (tooltip)
        {
            if (canAfford)
            {
                tooltip.SetTextFromScript("Relocate", $"Move deposit to a new location. Cost: {costText}");
            }
            else
            {
                tooltip.SetTextFromScript("Relocate", $"{Utils_Text.GetLocalizedString("NOT_ENOUGH_COIN")} ({costText})");
            }
        }

        var button = relocateGo.GetComponent<Button>();
        button.interactable = canAfford;
        button.onClick.RemoveAllListeners();
        var selection = __instance.selection;
        button.onClick.AddListener(() =>
        {
            var wo = selection.GetComponent<Nielsen.WorldObject>();
            if (!wo) return;

            Plugin.Log.LogInfo($"Relocate: {wo.data.title.GetLocalizedString()} — left-click to place, right-click to cancel");
            Plugin.PendingRelocatePath = wo.data.GetFullPath();
            Plugin.PendingRelocateOriginal = selection.gameObject;

            UIManager.Instance.HideActionWindow();
            TooltipManager.Instance.HideTooltip();

            // Hide renderers instead of deactivating to avoid coroutine errors
            foreach (var renderer in selection.gameObject.GetComponentsInChildren<Renderer>())
            {
                renderer.enabled = false;
            }
        });
    }

    /// <summary>
    /// Updates all registered canvas scalers to use Expand mode for ultra-wide displays or original mode for standard displays.
    /// Cleans up destroyed objects to prevent memory leaks.
    /// </summary>
    /// <param name="aspect">Current display aspect ratio.</param>
    internal static void UpdateScalers(float aspect)
    {
        CanvasScalers.RemoveAll(scaler => scaler == null);

        foreach (var key in OriginalScreenMatchModes.Keys.ToArray())
        {
            if (!CanvasScalers.Any(scaler => scaler != null && scaler.GetInstanceID() == key))
            {
                OriginalScreenMatchModes.Remove(key);
            }
        }

        foreach (var scaler in CanvasScalers)
        {
            if (!OriginalScreenMatchModes.TryGetValue(scaler.GetInstanceID(), out var originalMode))
            {
                OriginalScreenMatchModes.Add(scaler.GetInstanceID(), scaler.screenMatchMode);
                originalMode = scaler.screenMatchMode;
            }

            scaler.screenMatchMode = aspect > Plugin.NativeAspect
                ? CanvasScaler.ScreenMatchMode.Expand
                : originalMode;
        }
    }

}
