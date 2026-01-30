namespace CultOfQoL.Patches.Systems;

[Harmony]
public static class FastCollectingPatches
{
    private static GameManager GI => GameManager.GetInstance();
    private static bool CollectBedsRunning { get; set; }
    private static bool CollectAllBuildingShrinesRunning { get; set; }
    private static bool CollectAllShrinesRunning { get; set; }
    private static bool CollectAllOuthouseRunning { get; set; }
    private static bool CompostBinDeadBodyRunning { get; set; }

    private static bool CollectAllHarvestTotemsRunning { get; set; }

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
    [HarmonyPatch(typeof(Interaction_Bed), nameof(Interaction_Bed.OnSecondaryInteract))]
    public static void Interaction_Bed_OnSecondaryInteract(ref Interaction_Bed __instance)
    {
        if (!Plugin.MassCollectFromBeds.Value) return;
        if (!CollectBedsRunning)
        {
            GI.StartCoroutine(CollectBeds(__instance));
        }
    }

    private static IEnumerator CollectBeds(Interaction_Bed bedInteraction)
    {
        CollectBedsRunning = true;
        
        // Snapshot to avoid collection modified during enumeration
        foreach (var interaction in Interaction.interactions.ToList())
        {
            if (interaction is Interaction_Bed bed && bed && bed != bedInteraction && bed.StructureBrain?.SoulCount > 0)
            {
                bed.StartCoroutine(bed.GiveReward());
            }
        }

        yield return new WaitForSeconds(0.10f);
        CollectBedsRunning = false;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(BuildingShrinePassive), nameof(BuildingShrinePassive.OnInteract), typeof(StateMachine))]
    public static void BuildingShrinePassive_OnInteract(ref BuildingShrinePassive __instance, ref StateMachine state)
    {
        if (!Plugin.MassCollectFromPassiveShrines.Value) return;
        if (!CollectAllBuildingShrinesRunning)
        {
            GI.StartCoroutine(CollectAllBuildingShrines(__instance, state));
        }
    }

    private static IEnumerator CollectAllBuildingShrines(BuildingShrinePassive __instance, StateMachine state)
    {
        CollectAllBuildingShrinesRunning = true;
        yield return new WaitForEndOfFrame();
        
        // Snapshot to avoid collection modified during enumeration
        foreach (var interaction in Interaction.interactions.ToList())
        {
            if (interaction is BuildingShrinePassive shrine && shrine && shrine != __instance && shrine.StructureBrain?.SoulCount > 0)
            {
                shrine.OnInteract(state);
                yield return new WaitForSeconds(0.10f);
            }
        }

        CollectAllBuildingShrinesRunning = false;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Interaction_Outhouse), nameof(Interaction_Outhouse.OnInteract), typeof(StateMachine))]
    public static void Interaction_Outhouse_OnInteract(ref Interaction_Outhouse __instance, ref StateMachine state)
    {
        if (!Plugin.MassCollectFromOuthouses.Value) return;
        if (!CollectAllOuthouseRunning)
        {
            GI.StartCoroutine(CollectAllOuthouse(__instance, state));
        }
    }

    private static IEnumerator CollectAllOuthouse(Interaction_Outhouse __instance, StateMachine state)
    {
        CollectAllOuthouseRunning = true;
        yield return new WaitForEndOfFrame();
        
        // Snapshot to avoid collection modified during enumeration
        foreach (var interaction in Interaction.interactions.ToList())
        {
            if (interaction is Interaction_Outhouse outhouse && outhouse && outhouse != __instance && outhouse.StructureBrain?.GetPoopCount() > 0)
            {
                outhouse.OnInteract(state);
                yield return new WaitForSeconds(0.10f);
            }
        }

        CollectAllOuthouseRunning = false;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Interaction_CompostBinDeadBody), nameof(Interaction_CompostBinDeadBody.OnInteract), typeof(StateMachine))]
    public static void Interaction_CompostBinDeadBody_OnInteract(ref Interaction_CompostBinDeadBody __instance, ref StateMachine state)
    {
        if (!Plugin.MassCollectFromCompost.Value) return;
        if (!CompostBinDeadBodyRunning)
        {
            GI.StartCoroutine(CollectAllCompostBinDeadBody(__instance, state));
        }
    }

    private static IEnumerator CollectAllCompostBinDeadBody(Interaction_CompostBinDeadBody __instance, StateMachine state)
    {
        CompostBinDeadBodyRunning = true;
        yield return new WaitForEndOfFrame();
        
        // Snapshot to avoid collection modified during enumeration
        foreach (var interaction in Interaction.interactions.ToList())
        {
            if (interaction is Interaction_CompostBinDeadBody cbd && cbd && cbd != __instance && cbd.StructureBrain?.PoopCount > 0)
            {
                cbd.OnInteract(state);
                yield return new WaitForSeconds(0.10f);
            }
        }

        CompostBinDeadBodyRunning = false;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(HarvestTotem), nameof(HarvestTotem.OnInteract), typeof(StateMachine))]
    public static void HarvestTotem_OnInteract(ref HarvestTotem __instance, ref StateMachine state)
    {
        if (!Plugin.MassCollectFromHarvestTotems.Value) return;
        if (!CollectAllHarvestTotemsRunning)
        {
            GI.StartCoroutine(CollectAllHarvestTotems(__instance, state));
        }
    }

    private static IEnumerator CollectAllHarvestTotems(HarvestTotem totem, StateMachine state)
    {
        CollectAllHarvestTotemsRunning = true;
        
        // Snapshot to avoid collection modified during enumeration
        foreach (var t in HarvestTotem.HarvestTotems.ToList())
        {
            if (t && t != totem && t.StructureBrain?.SoulCount > 0)
            {
                t.OnInteract(state);
            }
        }

        yield return new WaitForSeconds(0.10f);
        CollectAllHarvestTotemsRunning = false;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Interaction_OfferingShrine), nameof(Interaction_OfferingShrine.OnInteract), typeof(StateMachine))]
    public static void Interaction_OfferingShrine_OnInteract(ref Interaction_OfferingShrine __instance, ref StateMachine state)
    {
        if (!Plugin.MassCollectFromOfferingShrines.Value) return;
        if (!CollectAllShrinesRunning)
        {
            GI.StartCoroutine(CollectAllShrines(__instance, state));
        }
    }

    private static IEnumerator CollectAllShrines(Interaction_OfferingShrine __instance, StateMachine state)
    {
        CollectAllShrinesRunning = true;
        yield return new WaitForEndOfFrame();

        // Snapshot the list to avoid modification during iteration
        var shrines = Interaction.interactions.ToList()
            .OfType<Interaction_OfferingShrine>()
            .Where(s => s && s != __instance && s.StructureInfo?.Inventory?.Count > 0)
            .ToList();

        Plugin.L($"[MassCollectOfferingShrines] Collecting from {shrines.Count} additional shrines");

        foreach (var shrine in shrines)
        {
            // Re-check validity before interacting
            if (!shrine || shrine.StructureInfo?.Inventory?.Count <= 0) continue;

            yield return new WaitForSeconds(0.10f);

            // Try-catch must be outside yield
            try
            {
                shrine.OnInteract(state);
            }
            catch (Exception ex)
            {
                Plugin.LE($"[MassCollectOfferingShrines] Error collecting from shrine: {ex.Message}");
            }
        }

        CollectAllShrinesRunning = false;
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

        if (!__instance.playerFarming)
        {
            return;
        }

        if (!InputManager.Gameplay.GetInteractButtonHeld())
        {
            if (__instance.StructureBrain is Structures_FarmerStation && !Plugin.AutoCollectFromFarmStationChests.Value)
            {
                return;
            }

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


            if (!triggerHit || emptyInventory || tooFarAway)
            {
                return true;
            }

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

        // __instance.DistanceToTriggerDeposits = 5f;
        //
        // if (__instance.Activating && (__instance.StructureInfo.Inventory.Count <= 0 || InputManager.Gameplay.GetInteractButtonUp() || Vector3.Distance(__instance.transform.position, __instance.Player.transform.position) > __instance.DistanceToTriggerDeposits))
        // {
        //     __instance.Activating = false;
        // }
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

    #region Mass Scarecrow Traps

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Scarecrow), nameof(Scarecrow.OnInteract))]
    public static void Scarecrow_OnInteract(Scarecrow __instance)
    {
        if (!Plugin.MassOpenScarecrows.Value)
        {
            return;
        }

        foreach (var scarecrow in Scarecrow.Scarecrows.ToList())
        {
            if (scarecrow == null || scarecrow == __instance || !scarecrow.Brain.HasBird)
            {
                continue;
            }

            scarecrow.OpenTrap();
            scarecrow.Brain.EmptyTrap();
            InventoryItem.Spawn(InventoryItem.ITEM_TYPE.MEAT, Random.Range(2, 5), scarecrow.transform.position);
        }
    }

    #endregion

    #region Mass Bear Traps

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Interaction_WolfTrap), nameof(Interaction_WolfTrap.OnInteract))]
    public static void Interaction_WolfTrap_OnInteract(Interaction_WolfTrap __instance)
    {
        if (!Plugin.MassFillBearTraps.Value)
        {
            return;
        }

        // Only trigger on fill action (not harvest - harvest already gives meat from the triggered trap)
        if (__instance.structure.Brain.Data.HasBird)
        {
            return;
        }

        // Hook into the item selector that's about to show
        GI.StartCoroutine(HookBaitSelection(__instance));
    }

    private static IEnumerator HookBaitSelection(Interaction_WolfTrap triggered)
    {
        yield return new WaitForEndOfFrame();

        // Find the UIItemSelectorOverlayController that was just opened
        var selector = MonoSingleton<UIManager>.Instance.GetComponentInChildren<UIItemSelectorOverlayController>();
        if (selector == null)
        {
            yield break;
        }

        // Store original callback
        var originalCallback = selector.OnItemChosen;

        selector.OnItemChosen = chosenItem =>
        {
            // Call original first (for the triggered trap)
            originalCallback?.Invoke(chosenItem);

            // Then fill all other empty traps
            FillAllBearTraps(triggered, chosenItem);
        };
    }

    private static void FillAllBearTraps(Interaction_WolfTrap triggered, InventoryItem.ITEM_TYPE baitType)
    {
        var emptyTraps = Interaction_WolfTrap.Traps
            .Where(t => t != null && t != triggered &&
                        !t.structure.Brain.Data.HasBird &&
                        t.structure.Brain.Data.Inventory.Count == 0)
            .ToList();

        foreach (var trap in emptyTraps)
        {
            // Check player has bait
            if (Inventory.GetItemQuantity(baitType) <= 0)
            {
                break;
            }

            // Deposit bait
            Inventory.ChangeItemQuantity((int)baitType, -1);
            trap.structure.Brain.Data.Inventory.Add(new InventoryItem(baitType, 1));
            trap.UpdateVisuals();
        }
    }

    #endregion
}