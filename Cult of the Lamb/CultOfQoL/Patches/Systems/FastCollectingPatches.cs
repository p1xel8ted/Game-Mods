namespace CultOfQoL.Patches.Systems;

[Harmony]
public static class FastCollectingPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(BuildingShrine), nameof(BuildingShrine.Update))]
    public static void BuildingShrine_Update(BuildingShrine __instance)
    {
        if (!Plugin.FastCollecting.Value) return;
        __instance.ReduceDelay = 0.0f;
        __instance.Delay = 0.0f;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(BuildingShrinePassive), nameof(BuildingShrinePassive.Update))]
    public static void BuildingShrinePassive_Update(BuildingShrinePassive __instance)
    {
        if (!Plugin.FastCollecting.Value) return;
        __instance.Delay = 0.0f;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Interaction_DiscipleCollectionShrine), nameof(Interaction_DiscipleCollectionShrine.Update))]
    public static void DiscipleCollectionShrine_Update(Interaction_DiscipleCollectionShrine __instance)
    {
        if (!Plugin.FastCollecting.Value) return;
        __instance.Delay = 0.0f;
        __instance.AccelerateCollection = 0.09f;
    }

    [HarmonyPostfix]
    [HarmonyPriority(Priority.High)]
    [HarmonyPatch(typeof(Interaction_DiscipleCollectionShrine), nameof(Interaction_DiscipleCollectionShrine.OnInteract), typeof(StateMachine))]
    public static void DiscipleCollectionShrine_OnInteract_Instant(Interaction_DiscipleCollectionShrine __instance)
    {
        if (!Plugin.CollectShrineDevotionInstantly.Value) return;
        if (!__instance.Activating) return;
        if (__instance.StructureBrain?.SoulCount <= 0) return;

        var totalSouls = __instance.StructureBrain.SoulCount;
        __instance.StructureBrain.SoulCount = 0;

        var hasUnlockAvailable = GameManager.HasUnlockAvailable() || DataManager.Instance.DeathCatBeaten;

        if (hasUnlockAvailable)
        {
            var visualSouls = Math.Min(totalSouls, 10);
            var directSouls = totalSouls - visualSouls;

            for (var i = 0; i < visualSouls; i++)
            {
                SoulCustomTarget.Create(
                    PlayerFarming.Instance.gameObject,
                    __instance.ReceiveSoulPosition.transform.position,
                    Color.white,
                    () => PlayerFarming.Instance?.GetSoul(1)
                );
            }

            for (var i = 0; i < directSouls; i++)
            {
                PlayerFarming.Instance?.GetSoul(1);
            }
        }
        else
        {
            var visualItems = Math.Min(totalSouls, 10);
            var directItems = totalSouls - visualItems;

            for (var i = 0; i < visualItems; i++)
            {
                InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1, __instance.transform.position + Vector3.back, 0.0f)
                    .SetInitialSpeedAndDiraction(8f + Random.Range(-0.5f, 1f), 270 + Random.Range(-90, 90));
            }

            if (directItems > 0)
            {
                Inventory.ChangeItemQuantity(InventoryItem.ITEM_TYPE.BLACK_GOLD, directItems);
            }
        }

        __instance.Activating = false;
        __instance.UpdateBar();
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Interaction_Bed), nameof(Interaction_Bed.GiveReward))]
    [HarmonyPatch(typeof(Interaction_CollectedResources), nameof(Interaction_CollectedResources.GiveResourcesRoutine))]
    private static void Interaction_Filter(ref IEnumerator __result)
    {
        if (!Plugin.FastCollecting.Value) return;
        __result = Helpers.FilterEnumerator(__result, [typeof(WaitForSeconds)]);
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Interaction_CollectResourceChest), nameof(Interaction_CollectResourceChest.Update))]
    private static void Interaction_CollectResourceChest_Update(ref Interaction_CollectResourceChest __instance)
    {
        if (!Plugin.EnableAutoCollect.Value) return;

        if (!__instance.playerFarming) return;

        if (!InputManager.Gameplay.GetInteractButtonHeld())
        {
            if (__instance.StructureBrain is Structures_FarmerStation && !Plugin.AutoCollectFromFarmStationChests.Value) return;

            var range = 5f;

            if (!Mathf.Approximately(Plugin.AutoInteractRangeMulti.Value, 1.0f))
            {
                range *= Plugin.AutoInteractRangeMulti.Value;
            }

            __instance.DistanceToTriggerDeposits = range;
            __instance.Activating = true;

            var inventoryQty = __instance.StructureInfo.Inventory.Sum(item => item.quantity);
            var emptyInventory = inventoryQty <= 0;
            var triggerHit = inventoryQty >= Plugin.TriggerAmount.Value;
            var distance = Vector3.Distance(__instance.transform.position, __instance.playerFarming.transform.position);
            var tooFarAway = distance > __instance.DistanceToTriggerDeposits;

            if (!triggerHit || emptyInventory || tooFarAway)
            {
                __instance.Activating = false;
            }

            return;
        }

        __instance.DistanceToTriggerDeposits = 5f;

        if (__instance.Activating && (__instance.StructureInfo.Inventory.Count <= 0 || InputManager.Gameplay.GetInteractButtonUp() || Vector3.Distance(__instance.transform.position, __instance.playerFarming.transform.position) > __instance.DistanceToTriggerDeposits))
        {
            __instance.Activating = false;
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(LumberjackStation), nameof(LumberjackStation.Update))]
    private static bool LumberjackStation_Update(ref LumberjackStation __instance)
    {
        if (!Plugin.EnableAutoCollect.Value) return true;

        if (!__instance.Player)
        {
            if (!__instance.playerFarming) return true;

            __instance.Player = PlayerFarming.Instance.gameObject;
        }

        if (Plugin.EnableAutoCollect.Value && !(InputManager.Gameplay.GetInteractButtonHeld() || InputManager.Gameplay.GetInteractButtonUp()))
        {
            var range = 5f;

            if (!Mathf.Approximately(Plugin.AutoInteractRangeMulti.Value, 1.0f))
            {
                range *= Plugin.AutoInteractRangeMulti.Value;
            }

            __instance.DistanceToTriggerDeposits = range;

            var inventoryQty = __instance.StructureInfo.Inventory.Sum(item => item.quantity);
            var emptyInventory = inventoryQty <= 0;
            var triggerHit = inventoryQty >= Plugin.TriggerAmount.Value;
            var distance = Vector3.Distance(__instance.transform.position, __instance.Player.transform.position);
            var tooFarAway = distance > __instance.DistanceToTriggerDeposits;

            if (!triggerHit || emptyInventory || tooFarAway) return true;

            __instance.Delay -= Time.deltaTime;
            if (__instance.Delay > 0f) return false;

            var station = __instance;
            foreach (var itemType in __instance.StructureInfo.Inventory.Select(item => (InventoryItem.ITEM_TYPE)item.type))
            {
                ResourceCustomTarget.Create(__instance.Player.gameObject, __instance.transform.position, itemType, delegate { station.GiveItem(itemType); });
            }

            __instance.StructureInfo.Inventory.Clear();

            AudioManager.Instance.PlayOneShot("event:/chests/chest_item_spawn", __instance.transform.position);
            __instance.ChestPosition.transform.DOKill();
            __instance.ChestPosition.transform.localScale = Vector3.one;
            __instance.ChestPosition.transform.DOPunchScale(__instance.PunchScale, 1f);
            __instance.UpdateChest();
            __instance.Delay = 0.1f;
            __instance.ExhaustedCheck();

            return false;
        }

        return true;
    }

    //collection speed for Interaction_EntranceShrine (dungeon shrines) - default speed is 0.1f
    [HarmonyTranspiler]
    [HarmonyPatch(typeof(Interaction_RatauShrine), nameof(Interaction_RatauShrine.Update))]
    [HarmonyPatch(typeof(Interaction_EntranceShrine), nameof(Interaction_EntranceShrine.Update))]
    [HarmonyPatch(typeof(Interaction_Outhouse), nameof(Interaction_Outhouse.Update))]
    public static IEnumerable<CodeInstruction> InteractionEntranceShrineTranspiler(IEnumerable<CodeInstruction> instructions, MethodBase originalMethod)
    {
        var original = instructions.ToList();
        if (!Plugin.FastCollecting.Value) return original;

        try
        {
            var codes = new List<CodeInstruction>(original);
            var typeName = originalMethod.GetRealDeclaringType().Name;
            var delayField = AccessTools.Field(originalMethod.GetRealDeclaringType(), "Delay");
            var found = false;

            for (var i = 0; i < codes.Count - 1; i++)
            {
                if (codes[i].opcode == OpCodes.Ldc_R4 && codes[i + 1].StoresField(delayField))
                {
                    codes[i].operand = typeName.Contains("Outhouse") ? 0.025f : 0f;
                    found = true;
                }
            }

            if (!found)
            {
                Plugin.Log.LogWarning($"[Transpiler] {typeName}.Update: Failed to find Delay field store.");
                return original;
            }

            Plugin.Log.LogInfo($"[Transpiler] {typeName}.Update: Reduced collection delay.");
            return codes;
        }
        catch (Exception ex)
        {
            Plugin.Log.LogWarning($"[Transpiler] {originalMethod.GetRealDeclaringType().Name}.Update: {ex.Message}");
            return original;
        }
    }

    /// <summary>
    /// When collecting devotion from the shrine, collect all instantly on a single tap instead of holding.
    /// Hook OnInteract - when Activating is set to true, collect all souls immediately.
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(BuildingShrine), nameof(BuildingShrine.OnInteract))]
    public static void BuildingShrine_OnInteract_Postfix(BuildingShrine __instance)
    {
        if (!Plugin.CollectShrineDevotionInstantly.Value) return;
        if (!__instance.Activating) return;
        if (__instance.StructureBrain?.SoulCount <= 0) return;

        if (__instance.StructureBrain != null)
        {
            var totalSouls = __instance.StructureBrain.SoulCount;
            __instance.StructureBrain.SoulCount = 0;

            // Check if player has unlocks available (determines soul vs black gold)
            var hasUnlockAvailable = GameManager.HasUnlockAvailable() || DataManager.Instance.DeathCatBeaten;

            if (hasUnlockAvailable)
            {
                // Spawn visual soul effects (cap at 10 for performance, rest go directly)
                var visualSouls = Math.Min(totalSouls, 10);
                var directSouls = totalSouls - visualSouls;

                for (var i = 0; i < visualSouls; i++)
                {
                    SoulCustomTarget.Create(
                        PlayerFarming.Instance.gameObject,
                        __instance.ReceiveSoulPosition.transform.position,
                        Color.white,
                        () => PlayerFarming.Instance?.GetSoul(1)
                    );
                }

                // Give remaining souls directly - must loop because GetSoul only adds 1 XP per call
                for (var i = 0; i < directSouls; i++)
                {
                    PlayerFarming.Instance?.GetSoul(1);
                }
            }
            else
            {
                // Spawn visual black gold (cap at 10 for performance)
                var visualItems = Math.Min(totalSouls, 10);
                var directItems = totalSouls - visualItems;

                for (var i = 0; i < visualItems; i++)
                {
                    InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1, __instance.transform.position + Vector3.back, 0.0f)
                        .SetInitialSpeedAndDiraction(8f + Random.Range(-0.5f, 1f), 270 + Random.Range(-90, 90));
                }

                // Give remaining black gold directly
                if (directItems > 0)
                {
                    Inventory.ChangeItemQuantity(InventoryItem.ITEM_TYPE.BLACK_GOLD, directItems);
                }
            }
        }

        // Stop the hold-to-collect behavior since we already collected everything
        __instance.Activating = false;

        // Update the UI bar
        __instance.UpdateBar();
    }

    #region Soul Camera Shake

    /// <summary>
    /// Conditionally suppresses camera shake from SoulCustomTarget.CollectMe() based on config.
    /// Called by the transpiler in place of CameraManager.ShakeCameraForDuration.
    /// </summary>
    private static void ConditionalSoulShake(CameraManager instance, float min, float max, float duration, bool stackShakes)
    {
        if (!Plugin.DisableSoulCameraShake.Value)
        {
            instance.ShakeCameraForDuration(min, max, duration, stackShakes);
        }
    }

    [HarmonyTranspiler]
    [HarmonyPatch(typeof(SoulCustomTarget), nameof(SoulCustomTarget.CollectMe))]
    private static IEnumerable<CodeInstruction> SoulCustomTarget_CollectMe(IEnumerable<CodeInstruction> instructions)
    {
        var original = instructions.ToList();
        try
        {
            var codes = new List<CodeInstruction>(original);
            var shakeMethod = AccessTools.Method(typeof(CameraManager), nameof(CameraManager.ShakeCameraForDuration),
                [typeof(float), typeof(float), typeof(float), typeof(bool)]);
            var conditionalMethod = AccessTools.Method(typeof(FastCollectingPatches), nameof(ConditionalSoulShake));
            var found = false;

            for (var i = 0; i < codes.Count; i++)
            {
                if (!codes[i].Calls(shakeMethod)) continue;
                codes[i].operand = conditionalMethod;
                found = true;
                break;
            }

            if (!found)
            {
                Plugin.Log.LogWarning("[Transpiler] SoulCustomTarget.CollectMe: Failed to find ShakeCameraForDuration call.");
                return original;
            }

            Plugin.Log.LogInfo("[Transpiler] SoulCustomTarget.CollectMe: Redirected ShakeCameraForDuration to ConditionalSoulShake.");
            return codes;
        }
        catch (Exception ex)
        {
            Plugin.Log.LogWarning($"[Transpiler] SoulCustomTarget.CollectMe: {ex.Message}");
            return original;
        }
    }

    #endregion
}
