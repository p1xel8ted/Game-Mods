namespace CultOfQoL.Patches;

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

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Interaction_CollectResourceChest), nameof(Interaction_CollectResourceChest.Update))]
    public static void Interaction_CollectResourceChest_Update(ref Interaction_CollectResourceChest __instance)
    {
        if (__instance.StructureInfo?.Inventory == null)
        {
            return;
        }

        var triggerExists = __instance.StructureInfo.Inventory.Exists(a => a.quantity >= Mathf.Abs(Plugin.TriggerAmount.Value));
        __instance.AutomaticallyInteract = false;
        if (Plugin.EnableAutoInteract.Value && (__instance.StructureInfo.Inventory.Count >= Mathf.Abs(Plugin.TriggerAmount.Value) || triggerExists))
        {
            __instance.Activating = true;
            if (Plugin.UseCustomRange.Value)
            {
                Plugin.OtherFastCollect.Value = __instance.DistanceToTriggerDeposits;


                __instance.DistanceToTriggerDeposits = Plugin.OtherFastCollect.Value * Mathf.Abs(Plugin.CustomRangeMulti.Value);
            }
            else
            {
                __instance.DistanceToTriggerDeposits = Plugin.IncreaseRange.Value ? 10f : 5f;
            }

            __instance.AutomaticallyInteract = true;
        }
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(LumberjackStation), nameof(LumberjackStation.Update))]
    public static void LumberjackStation_Update(ref LumberjackStation __instance)
    {
        if (__instance.StructureInfo?.Inventory == null)
        {
            return;
        }

        var triggerExists = __instance.StructureInfo.Inventory.Exists(a => a.quantity >= Mathf.Abs(Plugin.TriggerAmount.Value));
        __instance.AutomaticallyInteract = false;
        if (Plugin.EnableAutoInteract.Value && (__instance.StructureInfo.Inventory.Count >= Mathf.Abs(Plugin.TriggerAmount.Value) || triggerExists))
        {
            __instance.Activating = true;
            if (Plugin.UseCustomRange.Value)
            {
                Plugin.LumberFastCollect.Value = __instance.DistanceToTriggerDeposits;


                __instance.DistanceToTriggerDeposits = Plugin.LumberFastCollect.Value * Mathf.Abs(Plugin.CustomRangeMulti.Value);
            }
            else
            {
                __instance.DistanceToTriggerDeposits = Plugin.IncreaseRange.Value ? 10f : 5f;
            }

            __instance.AutomaticallyInteract = true;
        }
    }

    [HarmonyPrefix]
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
        foreach (var bed in beds.Where(a => a != null && a.StructureBrain?.SoulCount > 0))
        {
            if (bed == null || bed == bedInteraction) continue;
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
            if (shrine == null || shrine == __instance) continue;
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
            if (outhouse == null || outhouse == __instance) continue;
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
            if (cbd == null || cbd == __instance) continue;
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
            if (t == null || t == totem) continue;
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
            if (shrine == null || shrine == __instance) continue;
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

    [HarmonyPatch]
    public static class LootDelayTranspilers
    {
        //collection speed for Interaction_CollectResourceChest - default speed is 0.1f
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(Interaction_CollectResourceChest), nameof(Interaction_CollectResourceChest.Update))]
        [HarmonyPatch(typeof(LumberjackStation), nameof(LumberjackStation.Update))]
        public static IEnumerable<CodeInstruction> InteractionCollectResourceChestTranspiler(IEnumerable<CodeInstruction> instructions, MethodBase originalMethod)
        {
            if (!Plugin.FastCollecting.Value) return instructions;
            var delayField = AccessTools.Field(originalMethod.GetRealDeclaringType(), "Delay");
            var codes = new List<CodeInstruction>(instructions);
            for (var i = 0; i < codes.Count; i++)
            {
                if (codes[i].opcode == OpCodes.Ldc_R4 && codes[i + 1].StoresField(delayField))
                {
                    Plugin.L($"{originalMethod.GetRealDeclaringType().Name}: Found Delay at {i}");
                    codes[i].operand = originalMethod.GetRealDeclaringType().Name.Contains("Lumber") ? 0.025f : 0.01f;
                }
            }

            return codes.AsEnumerable();
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

}