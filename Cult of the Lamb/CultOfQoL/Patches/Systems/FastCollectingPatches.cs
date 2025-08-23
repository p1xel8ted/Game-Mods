namespace CultOfQoL.Patches.Systems;

[HarmonyPatch]
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
    public static void BuildingShrine_Update(ref float ___ReduceDelay, ref float ___Delay)
    {
        if (!Plugin.FastCollecting.Value) return;
        ___ReduceDelay = 0.0f;
        ___Delay = 0.0f;
    }

    [HarmonyPrefix]
    [HarmonyPostfix]
    [HarmonyPatch(typeof(BuildingShrinePassive), nameof(BuildingShrinePassive.Update))]
    public static void BuildingShrinePassive_Update(ref float ___Delay)
    {
        if (!Plugin.FastCollecting.Value) return;
        ___Delay = 0.0f;
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
        var beds = Interaction.interactions.OfType<Interaction_Bed>().ToList();
        foreach (var bed in beds.Where(a => a && a.StructureBrain?.SoulCount > 0))
        {
            if (!bed || bed == bedInteraction) continue;
            bed.StartCoroutine(bed.GiveReward());
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
        var shrines = Interaction.interactions.OfType<BuildingShrinePassive>().ToList();
        foreach (var shrine in shrines.Where(a => a.StructureBrain?.SoulCount > 0))
        {
            if (!shrine || shrine == __instance) continue;
            shrine.OnInteract(state);
            yield return new WaitForSeconds(0.10f);
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
        var outhouses = Interaction.interactions.OfType<Interaction_Outhouse>().ToList();
        foreach (var outhouse in outhouses.Where(a => a.StructureBrain?.GetPoopCount() > 0))
        {
            if (!outhouse || outhouse == __instance) continue;
            outhouse.OnInteract(state);
            yield return new WaitForSeconds(0.10f);
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
        var composts = Interaction.interactions.OfType<Interaction_CompostBinDeadBody>().ToList();
        foreach (var cbd in composts.Where(a => a.StructureBrain?.PoopCount > 0))
        {
            if (!cbd || cbd == __instance) continue;
            cbd.OnInteract(state);
            yield return new WaitForSeconds(0.10f);
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
        foreach (var t in HarvestTotem.HarvestTotems.Where(a => a != null && a.StructureBrain?.SoulCount > 0))
        {
            if (!t || t == totem) continue;
            t.OnInteract(state);
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
        var shrines = Interaction.interactions.OfType<Interaction_OfferingShrine>().ToList();
        foreach (var shrine in shrines.Where(a => a.StructureInfo?.Inventory?.Count > 0))
        {
            if (!shrine || shrine == __instance) continue;
            yield return new WaitForSeconds(0.10f);
            shrine.OnInteract(state);
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

        if (Plugin.EnableAutoCollect.Value && !InputManager.Gameplay.GetInteractButtonHeld())
        {
            if (__instance.StructureBrain is Structures_FarmerStation && !Plugin.AutoCollectFromFarmStationChests.Value)
            {
                return;
            }

            var range = 5f;

            if (Plugin.IncreaseAutoCollectRange.Value)
            {
                range = 10f;
            }

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

            if (Plugin.IncreaseAutoCollectRange.Value)
            {
                range = 10f;
            }

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
        if (!Plugin.FastCollecting.Value) return instructions;
        var delayField = AccessTools.Field(originalMethod.GetRealDeclaringType(), "Delay");

        var codes = new List<CodeInstruction>(instructions);
        for (var i = 0; i < codes.Count; i++)
        {
            if (codes[i].opcode == OpCodes.Ldc_R4 && codes[i + 1].StoresField(delayField))
            {
                Plugin.L($"{originalMethod.GetRealDeclaringType().Name}: Found Delay at {i}");
                codes[i].operand = originalMethod.GetRealDeclaringType().Name.Contains("Outhouse") ? 0.025f : 0f;
            }
        }

        return codes.AsEnumerable();
    }
}