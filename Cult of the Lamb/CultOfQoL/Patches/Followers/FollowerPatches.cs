// using CultOfQoL.Core.Routines;
using CultOfQoL.Core;

namespace CultOfQoL.Patches.Followers;

[Harmony]
public static class FollowerPatches
{
    /// <summary>
    /// Reference to the original follower interaction that triggered the mass action.
    ///
    /// NOTE: With the direct effects approach, this tracking may no longer be necessary.
    /// The original follower runs the vanilla routine which calls Close() normally,
    /// and other followers get instant effects (no callbacks, no Close() calls).
    /// Kept as a safety measure - can be removed if testing confirms it's not needed.
    /// </summary>
    private static interaction_FollowerInteraction OriginalMassActionInteraction { get; set; }

    /// <summary>
    /// Suppresses notifications if the affected count exceeds the configured threshold.
    /// Must be paired with <see cref="NotifySuppressEnd"/> after the mass action loop.
    /// </summary>
    private static void NotifySuppressBegin(int affectedCount)
    {
        if (affectedCount > Plugin.MassNotificationThreshold.Value)
        {
            NotificationCentre.NotificationsEnabled = false;
        }
    }

    /// <summary>
    /// Re-enables notifications and shows a single summary if suppression was active.
    /// </summary>
    private static void NotifySuppressEnd(string actionName, int affectedCount)
    {
        if (affectedCount > Plugin.MassNotificationThreshold.Value)
        {
            NotificationCentre.NotificationsEnabled = true;
            NotificationCentre.Instance?.PlayGenericNotification(
                $"{actionName} {affectedCount} followers",
                NotificationBase.Flair.Positive);
        }
    }

    public static int GetMinLifeExpectancy()
    {
        return Plugin.MinRangeLifeExpectancy.Value;
    }

    public static int GetMaxLifeExpectancy()
    {
        return Plugin.MaxRangeLifeExpectancy.Value;
    }

    [HarmonyTranspiler]
    [HarmonyPatch(typeof(FollowerInfo), nameof(FollowerInfo.NewCharacter))]
    public static IEnumerable<CodeInstruction> FollowerInfo_NewCharacter_Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var original = instructions.ToList();
        try
        {
            var codes = new List<CodeInstruction>(original);
            var getMinMethod = AccessTools.Method(typeof(FollowerPatches), nameof(GetMinLifeExpectancy));
            var getMaxMethod = AccessTools.Method(typeof(FollowerPatches), nameof(GetMaxLifeExpectancy));
            var randomRangeMethod = AccessTools.Method(typeof(Random), nameof(Random.Range), [typeof(int), typeof(int)]);
            var found = false;

            for (var i = 0; i < codes.Count - 2; i++)
            {
                if (IsLoadInt(codes[i], 40) && IsLoadInt(codes[i + 1], 55) && codes[i + 2].Calls(randomRangeMethod))
                {
                    codes[i] = new CodeInstruction(OpCodes.Call, getMinMethod).WithLabels(codes[i].labels);
                    codes[i + 1] = new CodeInstruction(OpCodes.Call, getMaxMethod).WithLabels(codes[i + 1].labels);
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                Plugin.Log.LogWarning("[Transpiler] FollowerInfo.NewCharacter: Failed to find Random.Range(40, 55) call.");
                return original;
            }

            Plugin.Log.LogInfo($"[Transpiler] FollowerInfo.NewCharacter: Replaced life expectancy range with ({GetMinLifeExpectancy()}, {GetMaxLifeExpectancy()}).");
            return codes;

            bool IsLoadInt(CodeInstruction instr, int value)
            {
                if (instr.opcode == OpCodes.Ldc_I4_S && instr.operand is sbyte sb) return sb == (sbyte)value;
                if (instr.opcode == OpCodes.Ldc_I4 && instr.operand is int iv) return iv == value;
                return false;
            }
        }
        catch (Exception ex)
        {
            Plugin.Log.LogWarning($"[Transpiler] FollowerInfo.NewCharacter: {ex.Message}");
            return original;
        }
    }


    /// <summary>
    /// Task types considered too physically demanding for elderly followers.
    /// Used when ElderWorkMode is set to LightWorkOnly.
    /// </summary>
    private static readonly HashSet<FollowerTaskType> HeavyWorkTasks =
    [
        FollowerTaskType.Build,
        FollowerTaskType.Farm,
        FollowerTaskType.BuryBody,
        FollowerTaskType.ClearRubble,
        FollowerTaskType.ClearRubbleBig,
        FollowerTaskType.ClearWeeds,
        FollowerTaskType.Lumberjack,
        FollowerTaskType.ChopTrees,
        FollowerTaskType.MineBloodStone,
        FollowerTaskType.MineRotstone,
        FollowerTaskType.ResourceStation,
        FollowerTaskType.Blacksmith,
        FollowerTaskType.Refinery,
        FollowerTaskType.Janitor,
        FollowerTaskType.CleanWaste,
        FollowerTaskType.Undertaker,
        FollowerTaskType.Handyman,
        FollowerTaskType.Forage
    ];

    [HarmonyPostfix]
    [HarmonyPatch(typeof(FollowerBrain), nameof(FollowerBrain.GetPersonalTask))]
    public static void FollowerBrain_GetTask(FollowerBrain __instance, FollowerLocation location, ref FollowerTask __result)
    {
        if (Plugin.ElderWorkMode.Value == ElderWorkMode.Disabled) return;

        if (__result is not FollowerTask_OldAge) return;

        var scheduledActivity = TimeManager.GetScheduledActivity(location);
        var found = false;
        if (scheduledActivity == ScheduledActivity.Work)
        {
            var task = FollowerBrain.GetDesiredTask_Work(location);
            task.RemoveAll(a => a is FollowerTask_OldAge);

            // Filter out heavy work when Light Work Only mode is active
            if (Plugin.ElderWorkMode.Value == ElderWorkMode.LightWorkOnly)
            {
                task.RemoveAll(a => HeavyWorkTasks.Contains(a.Type));
            }

            if (task.Count > 0)
            {
                var workTask = task.Random();
                if (workTask != null)
                {
                    __result = workTask;
                    found = true;
                }
            }
            else
            {
                Plugin.WriteLog($"[ElderWork] No available work tasks for elderly follower {__instance.Info.Name}.");
            }
        }

        if (found)
        {
            Plugin.WriteLog($"[ElderWork] Elderly follower {__instance.Info.Name} set to work on {__result}.");
        }
    }


    private static bool ShouldMassBribe(FollowerCommands followerCommands)
    {
        if (!Plugin.MassBribe.Value) return false;
        if (followerCommands != FollowerCommands.Bribe) return false;
        var notBribed = Follower.Followers.Count(follower => FollowerCommandItems.Bribe().IsAvailable(follower));
        return notBribed > 1;
    }

    /// <summary>
    /// Returns true if a follower qualifies for mass petting.
    /// Checks MassPetAllFollowers config or Dog/Poppy skin or Pettable trait.
    /// </summary>
    internal static bool CanBePetted(Follower f)
    {
        if (Plugin.MassPetAllFollowers.Value) return true;
        var skinName = f.Brain.Info.SkinName;
        return skinName.Contains("Dog") || skinName.Contains("Poppy")
            || f.Brain.HasTrait(FollowerTrait.TraitType.Pettable);
    }

    private static bool ShouldMassPetFollower(FollowerCommands followerCommands)
    {
        if (!Plugin.MassPetFollower.Value) return false;
        if (followerCommands is not (FollowerCommands.PetDog or FollowerCommands.PetFollower)) return false;
        var notPetted = Follower.Followers.Count(follower =>
            FollowerCommandItems.PetDog().IsAvailable(follower) && CanBePetted(follower));
        return notPetted > 1;
    }

    private static bool ShouldMassExtort(FollowerCommands followerCommands)
    {
        if (!Plugin.MassExtort.Value) return false;
        if (followerCommands != FollowerCommands.ExtortMoney) return false;
        var notPaidTithesCount = Follower.Followers.Count(follower => FollowerCommandItems.Extort().IsAvailable(follower));
        return notPaidTithesCount > 1;
    }

    private static bool ShouldMassInspire(FollowerCommands followerCommands)
    {
        if (!Plugin.MassInspire.Value) return false;
        if (followerCommands != FollowerCommands.Dance) return false;
        var notInspiredCount = Follower.Followers.Count(follower => FollowerCommandItems.Dance().IsAvailable(follower));
        return notInspiredCount > 1;
    }

    private static bool ShouldMassIntimidate(FollowerCommands followerCommands)
    {
        if (!Plugin.MassIntimidate.Value) return false;
        if (followerCommands != FollowerCommands.Intimidate) return false;
        var notIntimidatedCount = Follower.Followers.Count(follower => FollowerCommandItems.Intimidate().IsAvailable(follower));
        return notIntimidatedCount > 1;
    }

    private static bool ShouldMassBless(FollowerCommands followerCommands)
    {
        if (!Plugin.MassBless.Value) return false;
        if (followerCommands != FollowerCommands.Bless) return false;
        var notBlessedCount = Follower.Followers.Count(follower => FollowerCommandItems.Bless().IsAvailable(follower));
        return notBlessedCount > 1;
    }

    private static bool ShouldMassRomance(FollowerCommands followerCommands)
    {
        if (!Plugin.MassRomance.Value) return false;
        if (followerCommands != FollowerCommands.Romance) return false;
        var notKissedCount = Follower.Followers.Count(follower => FollowerCommandItems.Kiss().IsAvailable(follower) && !FollowerManager.IsChild(follower.Brain.Info.ID));
        return notKissedCount > 1;
    }

    private static bool ShouldMassBully(FollowerCommands followerCommands)
    {
        if (!Plugin.MassBully.Value) return false;
        if (followerCommands != FollowerCommands.Bully) return false;
        var notBulliedCount = Follower.Followers.Count(follower => FollowerCommandItems.Bully().IsAvailable(follower));
        return notBulliedCount > 1;
    }

    private static bool ShouldMassReassure(FollowerCommands followerCommands)
    {
        if (!Plugin.MassReassure.Value) return false;
        if (followerCommands != FollowerCommands.Reassure) return false;
        var notReassuredCount = Follower.Followers.Count(follower => FollowerCommandItems.Reassure().IsAvailable(follower));
        return notReassuredCount > 1;
    }

    private static bool ShouldMassReeducate(FollowerCommands followerCommands)
    {
        if (!Plugin.MassReeducate.Value) return false;
        if (followerCommands != FollowerCommands.Reeducate) return false;
        var notReeducatedCount = Follower.Followers.Count(follower => FollowerCommandItems.Reeducate().IsAvailable(follower));
        return notReeducatedCount > 1;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.OnFollowerCommandFinalized), typeof(FollowerCommands[]))]
    public static void interaction_FollowerInteraction_OnFollowerCommandFinalized_Postfix(ref interaction_FollowerInteraction __instance, params FollowerCommands[] followerCommands)
    {
        if (followerCommands[0] is not (FollowerCommands.Reassure or FollowerCommands.Reeducate or FollowerCommands.Bully or FollowerCommands.Romance or FollowerCommands.PetDog or FollowerCommands.PetFollower or FollowerCommands.ExtortMoney or FollowerCommands.Dance or FollowerCommands.Intimidate or FollowerCommands.Bless or FollowerCommands.Bribe))
        {
            Plugin.WriteLog($"[MassAction] Skipping mass command because {followerCommands[0]} is not a mass command!");
            return;
        }

        var cmd = followerCommands[0];
        var originalFollower = __instance.follower;

        var followers = Helpers.AllFollowers.Where(f => f != originalFollower).ToList();
        var validFollowers = followers.Where(f => f.Interaction_FollowerInteraction != null).ToList();

        foreach (var f in validFollowers)
        {
            f.Interaction_FollowerInteraction.playerFarming ??= PlayerFarming.Instance ??= Object.FindObjectOfType<PlayerFarming>();
        }

        if (cmd == FollowerCommands.Reassure && ShouldMassReassure(followerCommands[0]))
        {
            if (validFollowers.Count == 0) return;

            OriginalMassActionInteraction = __instance;
            var reassureEligible = validFollowers.Where(f =>
                FollowerCommandItems.Reassure().IsAvailable(f) &&
                MassActionEffects.IsAvailable(f.Brain) &&
                !MassActionEffects.IsImprisoned(f.Brain) &&
                !MassActionEffects.IsDissenting(f.Brain)).ToList();
            if (reassureEligible.Count == 0 || !MassActionCosts.TryDeductCosts(reassureEligible.Count)) return;
            NotifySuppressBegin(reassureEligible.Count);
            foreach (var follower in reassureEligible)
            {
                MassActionEffects.ApplyReassure(follower);
            }
            NotifySuppressEnd("Reassured", reassureEligible.Count);
        }

        if (cmd == FollowerCommands.Reeducate && ShouldMassReeducate(followerCommands[0]))
        {
            if (validFollowers.Count == 0) return;

            OriginalMassActionInteraction = __instance;
            var reeducateEligible = validFollowers.Where(f =>
                FollowerCommandItems.Reeducate().IsAvailable(f) &&
                MassActionEffects.IsAvailable(f.Brain) &&
                (MassActionEffects.IsImprisoned(f.Brain) || MassActionEffects.IsDissenting(f.Brain))).ToList();
            if (reeducateEligible.Count == 0 || !MassActionCosts.TryDeductCosts(reeducateEligible.Count)) return;
            NotifySuppressBegin(reeducateEligible.Count);
            foreach (var follower in reeducateEligible)
            {
                MassActionEffects.ApplyReeducate(follower);
            }
            NotifySuppressEnd("Reeducated", reeducateEligible.Count);
        }

        if (cmd == FollowerCommands.Bully && ShouldMassBully(followerCommands[0]))
        {
            if (validFollowers.Count == 0) return;

            OriginalMassActionInteraction = __instance;
            var bullyEligible = validFollowers.Where(f =>
                FollowerCommandItems.Bully().IsAvailable(f) &&
                MassActionEffects.IsAvailable(f.Brain) &&
                !MassActionEffects.IsImprisoned(f.Brain) &&
                !MassActionEffects.IsDissenting(f.Brain)).ToList();
            if (bullyEligible.Count == 0 || !MassActionCosts.TryDeductCosts(bullyEligible.Count)) return;
            NotifySuppressBegin(bullyEligible.Count);
            foreach (var follower in bullyEligible)
            {
                MassActionEffects.ApplyBully(follower);
            }
            NotifySuppressEnd("Bullied", bullyEligible.Count);
        }

        if (cmd == FollowerCommands.Romance && ShouldMassRomance(followerCommands[0]))
        {
            if (validFollowers.Count == 0) return;

            OriginalMassActionInteraction = __instance;
            var allowZombieReaction = !originalFollower.Brain.Info.HasTrait(FollowerTrait.TraitType.Zombie);
            var zombieReactionPlayed = false;
            var romanceEligible = validFollowers.Where(f =>
                FollowerCommandItems.Kiss().IsAvailable(f) &&
                !FollowerManager.IsChild(f.Brain.Info.ID) &&
                MassActionEffects.IsAvailable(f.Brain) &&
                !MassActionEffects.IsImprisoned(f.Brain) &&
                !MassActionEffects.IsDissenting(f.Brain)).ToList();
            if (romanceEligible.Count == 0 || !MassActionCosts.TryDeductCosts(romanceEligible.Count)) return;
            NotifySuppressBegin(romanceEligible.Count);
            foreach (var follower in romanceEligible)
            {
                var triggered = MassActionEffects.ApplyRomance(follower, allowZombieReaction && !zombieReactionPlayed);
                if (triggered)
                {
                    zombieReactionPlayed = true;
                }
            }
            NotifySuppressEnd("Romanced", romanceEligible.Count);
        }

        if (cmd is (FollowerCommands.PetDog or FollowerCommands.PetFollower) && ShouldMassPetFollower(followerCommands[0]))
        {
            if (validFollowers.Count == 0) return;

            OriginalMassActionInteraction = __instance;
            var petEligible = validFollowers.Where(f =>
                FollowerCommandItems.PetDog().IsAvailable(f) &&
                CanBePetted(f) &&
                MassActionEffects.IsAvailable(f.Brain) &&
                !MassActionEffects.IsImprisoned(f.Brain) &&
                !MassActionEffects.IsDissenting(f.Brain)).ToList();
            if (petEligible.Count == 0 || !MassActionCosts.TryDeductCosts(petEligible.Count)) return;
            NotifySuppressBegin(petEligible.Count);
            foreach (var follower in petEligible)
            {
                MassActionEffects.ApplyPet(follower);
            }
            NotifySuppressEnd("Petted", petEligible.Count);
        }

        if (cmd == FollowerCommands.ExtortMoney && ShouldMassExtort(followerCommands[0]))
        {
            if (validFollowers.Count == 0) return;

            OriginalMassActionInteraction = __instance;
            var extortEligible = validFollowers.Where(f =>
                FollowerCommandItems.Extort().IsAvailable(f) &&
                MassActionEffects.IsAvailable(f.Brain) &&
                !MassActionEffects.IsImprisoned(f.Brain) &&
                !MassActionEffects.IsDissenting(f.Brain)).ToList();
            if (extortEligible.Count == 0 || !MassActionCosts.TryDeductCosts(extortEligible.Count)) return;
            NotifySuppressBegin(extortEligible.Count);
            foreach (var follower in extortEligible)
            {
                MassActionEffects.ApplyExtort(follower);
            }
            NotifySuppressEnd("Extorted", extortEligible.Count);
        }

        if (cmd == FollowerCommands.Dance && ShouldMassInspire(followerCommands[0]))
        {
            if (validFollowers.Count == 0) return;

            OriginalMassActionInteraction = __instance;
            var inspireEligible = validFollowers.Where(f =>
                FollowerCommandItems.Dance().IsAvailable(f) &&
                MassActionEffects.IsAvailable(f.Brain) &&
                f.Brain.CurrentTaskType != FollowerTaskType.GetPlayerAttention &&
                !FollowerManager.FollowerLocked(f.Brain.Info.ID) &&
                !f.Brain.HasTrait(FollowerTrait.TraitType.Mutated) &&
                !MassActionEffects.IsImprisoned(f.Brain) &&
                !MassActionEffects.IsDissenting(f.Brain)).ToList();
            if (inspireEligible.Count == 0 || !MassActionCosts.TryDeductCosts(inspireEligible.Count)) return;
            NotifySuppressBegin(inspireEligible.Count);
            foreach (var follower in inspireEligible)
            {
                MassActionEffects.ApplyInspire(follower);
            }
            NotifySuppressEnd("Inspired", inspireEligible.Count);
        }

        if (cmd == FollowerCommands.Intimidate && ShouldMassIntimidate(followerCommands[0]))
        {
            if (validFollowers.Count == 0) return;

            OriginalMassActionInteraction = __instance;
            var intimidateEligible = validFollowers.Where(f =>
                FollowerCommandItems.Intimidate().IsAvailable(f) &&
                MassActionEffects.IsAvailable(f.Brain) &&
                !FollowerManager.FollowerLocked(f.Brain.Info.ID) &&
                !f.Brain.HasTrait(FollowerTrait.TraitType.Mutated) &&
                !MassActionEffects.IsImprisoned(f.Brain) &&
                !MassActionEffects.IsDissenting(f.Brain)).ToList();
            if (intimidateEligible.Count == 0 || !MassActionCosts.TryDeductCosts(intimidateEligible.Count)) return;
            NotifySuppressBegin(intimidateEligible.Count);
            foreach (var follower in intimidateEligible)
            {
                MassActionEffects.ApplyIntimidate(follower, allowScaredTrait: Plugin.MassIntimidateScareAll.Value);
            }
            NotifySuppressEnd("Intimidated", intimidateEligible.Count);
        }


        if (cmd == FollowerCommands.Bless && ShouldMassBless(followerCommands[0]))
        {
            if (validFollowers.Count == 0) return;

            OriginalMassActionInteraction = __instance;
            var blessEligible = validFollowers.Where(f =>
                FollowerCommandItems.Bless().IsAvailable(f) &&
                MassActionEffects.IsAvailable(f.Brain) &&
                !FollowerManager.FollowerLocked(f.Brain.Info.ID) &&
                !f.Brain.HasTrait(FollowerTrait.TraitType.Mutated) &&
                !MassActionEffects.IsImprisoned(f.Brain) &&
                !MassActionEffects.IsDissenting(f.Brain)).ToList();
            if (blessEligible.Count == 0 || !MassActionCosts.TryDeductCosts(blessEligible.Count)) return;
            NotifySuppressBegin(blessEligible.Count);
            foreach (var follower in blessEligible)
            {
                MassActionEffects.ApplyBless(follower);
            }
            NotifySuppressEnd("Blessed", blessEligible.Count);
        }


        if (cmd == FollowerCommands.Bribe && ShouldMassBribe(followerCommands[0]))
        {
            if (validFollowers.Count == 0) return;

            OriginalMassActionInteraction = __instance;
            var bribeEligible = validFollowers.Where(f =>
                FollowerCommandItems.Bribe().IsAvailable(f) &&
                MassActionEffects.IsAvailable(f.Brain) &&
                !MassActionEffects.IsImprisoned(f.Brain) &&
                !MassActionEffects.IsDissenting(f.Brain)).ToList();
            if (bribeEligible.Count == 0 || !MassActionCosts.TryDeductCosts(bribeEligible.Count)) return;
            NotifySuppressBegin(bribeEligible.Count);
            foreach (var follower in bribeEligible)
            {
                MassActionEffects.ApplyBribe(follower);
            }
            NotifySuppressEnd("Bribed", bribeEligible.Count);
        }
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.LevelUpRoutine))]
    public static void interaction_FollowerInteraction_GiveDiscipleRewardRoutine(ref interaction_FollowerInteraction __instance)
    {
        if (!Plugin.CleanseIllnessAndExhaustionOnLevelUp.Value) return;
        if (__instance.follower.Brain.Stats.Exhaustion > 0)
        {
            __instance.follower.Brain._directInfoAccess.Exhaustion = 0f;
            __instance.follower.Brain.Stats.Exhaustion = 0f;
            var onExhaustionStateChanged = FollowerBrainStats.OnExhaustionStateChanged;
            onExhaustionStateChanged?.Invoke(__instance.follower.Brain._directInfoAccess.ID, FollowerStatState.Off, FollowerStatState.On);
            Plugin.WriteLog($"[LevelUp] Resetting follower {__instance.follower.name} from exhaustion!");
        }

        if (__instance.follower.Brain.Stats.Illness > 0)
        {
            __instance.follower.Brain._directInfoAccess.Illness = 0f;
            __instance.follower.Brain.Stats.Illness = 0f;
            var onIllnessStateChanged = FollowerBrainStats.OnIllnessStateChanged;
            onIllnessStateChanged?.Invoke(__instance.follower.Brain._directInfoAccess.ID, FollowerStatState.Off, FollowerStatState.On);
            Plugin.WriteLog($"[LevelUp] Resetting follower {__instance.follower.name} from illness!");
        }
    }

    private static bool MassLevelUpRunning { get; set; }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.LevelUpRoutine))]
    public static void MassLevelUp_Postfix(ref interaction_FollowerInteraction __instance)
    {
        if (!Plugin.MassLevelUp.Value) return;
        if (MassLevelUpRunning) return;

        var originalFollower = __instance.follower;
        var eligibleCount = Helpers.AllFollowers.Count(f => f != originalFollower && f.Brain.CanLevelUp());
        if (eligibleCount < 1) return;

        Plugin.WriteLog($"[MassLevelUp] Triggered by {originalFollower.Brain.Info.Name}, {eligibleCount} other followers eligible.");
        GameManager.GetInstance().StartCoroutine(MassLevelUpAll(originalFollower));
    }


    private static readonly HashSet<SoulCustomTarget> LevelUpSouls = [];
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(SoulCustomTarget), nameof(SoulCustomTarget.Create),
        typeof(GameObject), typeof(Vector3), typeof(Color), typeof(Action),
        typeof(float), typeof(float), typeof(bool), typeof(bool), typeof(bool),
        typeof(string), typeof(string), typeof(bool))]
    public static void SoulCustomTarget_Create(GameObject __result)
    {
        if (!Plugin.MassLevelUpInstantSouls.Value) return;
        if (!Helpers.IsCalledFrom("LevelUpRoutine")) return;

        var soul = __result.GetComponent<SoulCustomTarget>();
        if (soul)
        {
            LevelUpSouls.Add(soul);
        }
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(SoulCustomTarget), nameof(SoulCustomTarget.FixedUpdate))]
    public static void SoulCustomTarget_FixedUpdate(SoulCustomTarget __instance)
    {
        if (!Plugin.MassLevelUpInstantSouls.Value) return;
        if (!LevelUpSouls.Remove(__instance)) return;

        if (!__instance.isCollected)
        {
            __instance.CollectMe();
        }
    }

    private static IEnumerator MassLevelUpAll(Follower original)
    {
        MassLevelUpRunning = true;
        yield return new WaitForSeconds(0.5f);

        var eligible = Helpers.AllFollowers
            .Where(f => f != original && f.Brain.CanLevelUp())
            .ToList();

        Plugin.WriteLog($"[MassLevelUp] Starting level up for {eligible.Count} followers.");

        NotifySuppressBegin(eligible.Count);
        foreach (var follower in eligible)
        {
            var previousTask = follower.Brain.CurrentTaskType;
            Plugin.WriteLog($"[MassLevelUp] Leveling up {follower.Brain.Info.Name}.");
            MassActionEffects.ApplyLevelUp(follower, previousTask, follower.Interaction_FollowerInteraction);
        }
        NotifySuppressEnd("Leveled up", eligible.Count);

        Plugin.WriteLog("[MassLevelUp] All level ups applied.");
        MassLevelUpRunning = false;
    }

    /// <summary>
    /// Prevents the interaction wheel from reopening after mass actions.
    ///
    /// NOTE: With the direct effects approach, this prefix may no longer be necessary.
    /// Only the original follower runs a routine with callbacks - other followers get
    /// instant effects. The vanilla Close() behavior should work fine for the original.
    /// Kept as a safety measure until testing confirms it can be removed.
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.Close), typeof(bool), typeof(bool), typeof(bool))]
    public static void interaction_FollowerInteraction_Close(interaction_FollowerInteraction __instance, ref bool DoResetFollower, ref bool unpause, ref bool reshowMenu)
    {
        // Safety check: force reshowMenu=false for the original mass action interaction
        if (__instance == OriginalMassActionInteraction)
        {
            DoResetFollower = true;
            unpause = true;
            reshowMenu = false;
        }

        // NOTE: Legacy code below commented out - MassActionCurrentlyRunning is never set with the
        // direct effects approach (MassActionEffects). Only the original follower runs a routine;
        // other followers get instant effects without callbacks.

        // if (!RoutinesTranspilers.MassActionCurrentlyRunning) return;
        //
        // DoResetFollower = true;
        // unpause = true;
        // reshowMenu = false;
    }

    // NOTE: Legacy prefix commented out - MassActionCurrentlyRunning is never set with the direct
    // effects approach. This prefix was intended to modify callback behavior when mass routines
    // were running, but now only the original follower runs a routine.

    // [HarmonyPrefix]
    // [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.LevelUpRoutine))]
    // public static void interaction_FollowerInteraction_LevelUpRoutine(ref Action Callback, ref bool GoToAndStop, ref bool onFinishClose)
    // {
    //     // Only modify behavior when a mass action is actually running, not just when config is enabled
    //     if (!RoutinesTranspilers.MassActionCurrentlyRunning) return;
    //
    //     Callback = null;
    //     GoToAndStop = false;
    //     onFinishClose = true;
    // }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(FollowerCommandItems), nameof(FollowerCommandItems.GiveWorkerCommand))]
    public static void interaction_FollowerInteraction_OnFollowerCommandFinalized(Follower follower, ref CommandItem __result)
    {
        if (Plugin.ElderWorkMode.Value != ElderWorkMode.Disabled && follower.Brain.Info.CursedState == Thought.OldAge)
        {
            __result.SubCommands = FollowerCommandGroups.GiveWorkerCommands(follower);
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.OnFollowerCommandFinalized))]
    public static bool interaction_FollowerInteraction_OnFollowerCommandFinalized(ref interaction_FollowerInteraction __instance, params FollowerCommands[] followerCommands)
    {
        var elderWorkEnabled = Plugin.ElderWorkMode.Value != ElderWorkMode.Disabled;
        var command = followerCommands[0] == FollowerCommands.GiveWorkerCommand_2 || followerCommands[0] == FollowerCommands.MakeDemand;
        var old = __instance.follower.Brain.Info.CursedState == Thought.OldAge;
        if (elderWorkEnabled && command && old)
        {
            __instance.follower.Brain.CompleteCurrentTask();

            // Get available tasks, filtering for light work if needed
            var tasks = FollowerBrain.GetDesiredTask_Work(__instance.follower.Brain.Location);
            if (Plugin.ElderWorkMode.Value == ElderWorkMode.LightWorkOnly)
            {
                tasks.RemoveAll(t => HeavyWorkTasks.Contains(t.Type));
            }

            if (tasks.Count == 0)
            {
                NotificationCentre.Instance.PlayGenericNotification($"No suitable work available for {__instance.follower.Brain.Info.Name}.", NotificationBase.Flair.Negative);
                return true;
            }

            var task = tasks.Random();
            __instance.follower.Brain.HardSwapToTask(task);
            NotificationCentre.Instance.PlayGenericNotification($"{__instance.follower.Brain.Info.Name} sent to work on {task.Type}!", NotificationBase.Flair.Positive);
            Plugin.WriteLog($"[ElderWork] Old follower {__instance.follower.name} made to work on {task}");
            __instance.Close(false, true, false);
            return false;
        }

        return true;
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(FollowerCommandGroups), nameof(FollowerCommandGroups.OldAgeCommands), typeof(Follower))]
    public static void FollowerCommandGroups_OldAgeCommands(ref List<CommandItem> __result, Follower follower)
    {
        if (Plugin.CollectTitheFromOldFollowers.Value)
        {
            if (DoctrineUpgradeSystem.GetUnlocked(DoctrineUpgradeSystem.DoctrineType.Possessions_ExtortTithes))
            {
                __result.Add(FollowerCommandItems.Extort());
            }
        }

        if (Plugin.IntimidateOldFollowers.Value)
        {
            if (DoctrineUpgradeSystem.GetUnlocked(DoctrineUpgradeSystem.DoctrineType.WorkWorship_Intimidate))
            {
                __result.Add(FollowerCommandItems.Intimidate());
            }
        }

        if (Plugin.ElderWorkMode.Value != ElderWorkMode.Disabled)
        {
            __result.Add(FollowerCommandItems.GiveWorkerCommand(follower));
        }
    }
}

