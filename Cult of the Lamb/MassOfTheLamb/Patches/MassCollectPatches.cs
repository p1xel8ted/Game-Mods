using MassOfTheLamb.Core;

namespace MassOfTheLamb.Patches;

[Harmony]
public static class MassCollectPatches
{
    private static GameManager GI => GameManager.GetInstance();

    private static bool CollectBedsRunning { get; set; }
    private static bool CollectAllBuildingShrinesRunning { get; set; }
    private static bool CollectAllDiscipleShrinesRunning { get; set; }
    private static bool CollectAllShrinesRunning { get; set; }
    private static bool CollectAllOuthouseRunning { get; set; }
    private static bool CompostBinDeadBodyRunning { get; set; }
    private static bool CollectAllHarvestTotemsRunning { get; set; }
    private static bool CleanAllPoopRunning { get; set; }
    private static bool CleanAllVomitRunning { get; set; }

    #region Bed Speed-Up (for mass collection)

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Interaction_Bed), nameof(Interaction_Bed.GiveReward))]
    public static void Interaction_Bed_GiveReward(ref IEnumerator __result)
    {
        if (!Plugin.MassCollectFromBeds.Value) return;
        __result = SpeedUpBedReward(__result);
    }

    private static IEnumerator SpeedUpBedReward(IEnumerator original)
    {
        while (original.MoveNext())
        {
            yield return original.Current is WaitForSeconds ? new WaitForSeconds(0.05f) : original.Current;
        }
    }

    #endregion

    #region Mass Collect From Beds

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
        yield return new WaitForEndOfFrame();

        // Snapshot to avoid collection modified during enumeration
        foreach (var interaction in Interaction.interactions.ToList())
        {
            if (interaction is Interaction_Bed bed && bed && bed != bedInteraction && bed.StructureBrain?.SoulCount > 0)
            {
                bed.StartCoroutine(bed.GiveReward());
                yield return new WaitForSeconds(0.05f);
            }
        }

        CollectBedsRunning = false;
    }

#if DEBUG
    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameManager), nameof(GameManager.Update))]
    public static void DebugFillBedSouls()
    {
        if (!UnityEngine.Input.GetKeyDown(KeyCode.F10)) return;
        var count = 0;
        foreach (var interaction in Interaction.interactions)
        {
            if (interaction is Interaction_Bed bed && bed.StructureBrain != null)
            {
                bed.StructureBrain.SoulCount = bed.StructureBrain.SoulMax;
                bed.UpdateBar();
                count++;
            }
        }
        Plugin.WriteLog($"[DEBUG] Filled {count} bed(s) to max souls");
    }
#endif

    #endregion

    #region Mass Collect From Passive Shrines

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
                yield return new WaitForSeconds(0.05f);
            }
        }

        CollectAllBuildingShrinesRunning = false;
    }

    #endregion

    #region Mass Collect From Disciple Shrines

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Interaction_DiscipleCollectionShrine), nameof(Interaction_DiscipleCollectionShrine.OnInteract), typeof(StateMachine))]
    public static void DiscipleCollectionShrine_OnInteract(ref Interaction_DiscipleCollectionShrine __instance, ref StateMachine state)
    {
        if (!Plugin.MassCollectFromPassiveShrines.Value) return;
        if (!CollectAllDiscipleShrinesRunning)
        {
            GI.StartCoroutine(CollectAllDiscipleShrines(__instance, state));
        }
    }

    private static IEnumerator CollectAllDiscipleShrines(Interaction_DiscipleCollectionShrine __instance, StateMachine state)
    {
        CollectAllDiscipleShrinesRunning = true;
        yield return new WaitForEndOfFrame();

        foreach (var shrine in Interaction_DiscipleCollectionShrine.Shrines.ToList())
        {
            if (shrine && shrine != __instance && shrine.StructureBrain?.SoulCount > 0)
            {
                shrine.OnInteract(state);
                yield return new WaitForSeconds(0.05f);
            }
        }

        CollectAllDiscipleShrinesRunning = false;
    }

    #endregion

    #region Mass Collect From Outhouses

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
                yield return new WaitForSeconds(0.05f);
            }
        }

        CollectAllOuthouseRunning = false;
    }

    #endregion

    #region Mass Collect From Compost

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
                yield return new WaitForSeconds(0.05f);
            }
        }

        CompostBinDeadBodyRunning = false;
    }

    #endregion

    #region Mass Collect From Harvest Totems

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
        yield return new WaitForEndOfFrame();

        // Snapshot to avoid collection modified during enumeration
        foreach (var t in HarvestTotem.HarvestTotems.ToList())
        {
            if (t && t != totem && t.StructureBrain?.SoulCount > 0)
            {
                t.OnInteract(state);
                yield return new WaitForSeconds(0.05f);
            }
        }

        CollectAllHarvestTotemsRunning = false;
    }

    #endregion

    #region Mass Collect From Offering Shrines

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

        Plugin.WriteLog($"[MassCollectOfferingShrines] Collecting from {shrines.Count} additional shrines");

        foreach (var shrine in shrines)
        {
            // Re-check validity before interacting
            if (!shrine || shrine.StructureInfo?.Inventory?.Count <= 0) continue;

            yield return new WaitForSeconds(0.05f);

            // Try-catch must be outside yield
            try
            {
                shrine.OnInteract(state);
            }
            catch (Exception ex)
            {
                Plugin.WriteLog($"[MassCollectOfferingShrines] Error collecting from shrine: {ex.Message}", Plugin.LogType.Error);
            }
        }

        CollectAllShrinesRunning = false;
    }

    #endregion

    #region Shrine God Tears

    /// <summary>
    /// Before vanilla processes the shrine interaction, collect any extra god tears beyond the one
    /// vanilla will handle via GiveGodTearIE. This prefix runs before any other code can modify
    /// AbilityPoints (e.g. deferred SoulCustomTarget callbacks from instant devotion collection).
    /// Only applies when vanilla would give god tears directly (all upgrades unlocked, DeathCatBeaten).
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(BuildingShrine), nameof(BuildingShrine.OnInteract))]
    public static void BuildingShrine_OnInteract_GodTears_Prefix(BuildingShrine __instance)
    {
        if (!Plugin.CollectAllGodTearsAtOnce.Value) return;
        if (__instance.Activating) return;
        if (GameManager.HasUnlockAvailable()) return;
        if (!DataManager.Instance.DeathCatBeaten) return;

        var total = UpgradeSystem.AbilityPoints;
        Plugin.WriteLog($"[Collect All God Tears] AbilityPoints at interaction: {total}");
        if (total <= 1) return;

        var additional = total - 1;
        Inventory.AddItem((int)InventoryItem.ITEM_TYPE.GOD_TEAR, additional);
        UpgradeSystem.AbilityPoints = 1;

        Plugin.WriteLog($"[Collect All God Tears] Pre-collected {additional} additional god tears (was {total}, left 1 for vanilla)");
    }

    #endregion

    #region Mass Clean Poop/Vomit

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Interaction_Poop), nameof(Interaction_Poop.OnInteract), typeof(StateMachine))]
    public static void Interaction_Poop_OnInteract(ref Interaction_Poop __instance, ref StateMachine state)
    {
        if (!Plugin.MassCleanPoop.Value) return;
        if (!CleanAllPoopRunning)
        {
            GI.StartCoroutine(CleanAllPoop(__instance, state));
        }
    }

    private static IEnumerator CleanAllPoop(Interaction_Poop __instance, StateMachine state)
    {
        CleanAllPoopRunning = true;
        yield return new WaitForEndOfFrame();

        var cleanable = Interaction_Poop.Poops.Count(p => p && p != __instance && !p.Activating);
        if (cleanable == 0 || !MassActionCosts.TryDeductCosts(cleanable))
        {
            CleanAllPoopRunning = false;
            yield break;
        }

        foreach (var poop in Interaction_Poop.Poops.ToList())
        {
            if (poop && poop != __instance && !poop.Activating)
            {
                poop.OnInteract(state);
                yield return new WaitForSeconds(0.05f);
            }
        }

        CleanAllPoopRunning = false;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Vomit), nameof(Vomit.OnInteract), typeof(StateMachine))]
    public static void Vomit_OnInteract(ref Vomit __instance, ref StateMachine state)
    {
        if (!Plugin.MassCleanVomit.Value) return;
        if (!CleanAllVomitRunning)
        {
            GI.StartCoroutine(CleanAllVomit(__instance, state));
        }
    }

    private static IEnumerator CleanAllVomit(Vomit __instance, StateMachine state)
    {
        CleanAllVomitRunning = true;
        yield return new WaitForEndOfFrame();

        var cleanable = Vomit.Vomits.Count(v => v && v != __instance && !v.Activating);
        if (cleanable == 0 || !MassActionCosts.TryDeductCosts(cleanable))
        {
            CleanAllVomitRunning = false;
            yield break;
        }

        foreach (var v in Vomit.Vomits.ToList())
        {
            if (v && v != __instance && !v.Activating)
            {
                v.OnInteract(state);
                yield return new WaitForSeconds(0.05f);
            }
        }

        CleanAllVomitRunning = false;
    }

    #endregion

    #region Mass Sin Extraction

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Follower), nameof(Follower.GiveSinToPlayer), typeof(Action))]
    public static void Follower_GiveSinToPlayer_Postfix(Follower __instance)
    {
        if (!Plugin.MassSinExtract.Value) return;

        var eligible = Follower.Followers.Count(f => f && f != __instance && f.Brain != null &&
            f.Brain.CanGiveSin() && !f.InGiveSinSequence);
        if (eligible < 1 || !MassActionCosts.TryDeductCosts(eligible)) return;

        // Find all other eligible followers and extract sin from them
        foreach (var follower in Follower.Followers.ToList())
        {
            if (!follower || follower == __instance) continue;
            if (follower.Brain == null || !follower.Brain.CanGiveSin()) continue;
            if (follower.InGiveSinSequence) continue;

            // Start the floating animation for visual effect
            follower.SetBodyAnimation("Sin/sin-start", false);
            follower.AddBodyAnimation("Sin/sin-floating", true, 0.0f);

            // Give sin reward and reset pleasure
            GI.StartCoroutine(CollectSinFromFollower(follower));
        }
    }

    private static IEnumerator CollectSinFromFollower(Follower follower)
    {
        // Wait for the animation to get into the floating state
        yield return new WaitForSeconds(1.5f);

        // Play the collect animation
        follower.SetBodyAnimation("Sin/sin-collect", false);
        follower.AddBodyAnimation("idle", true, 0.0f);

        // Reset pleasure bar UI
        if (follower.PleasureUI?.BarController != null)
        {
            follower.PleasureUI.BarController.SetBarSize(0.0f, false, true);
        }

        // Give reward
        Inventory.AddItem(154, 1); // Sin item

        // Play sound
        AudioManager.Instance.PlayOneShot("event:/dialogue/followers/possessed/sinned_vom", follower.gameObject);

        yield return new WaitForSeconds(0.4f);

        // Reset pleasure
        follower.Brain.Info.Pleasure = 0;

        // Let the follower return to normal behavior after animation
        yield return new WaitForSeconds(1f);
        follower.Brain.CheckChangeState();
    }

    #endregion
}
