using MassOfTheLamb.Core;

namespace MassOfTheLamb.Patches;

[Harmony]
public static class MassFollowerPatches
{
    /// <summary>
    /// Reference to the original follower interaction that triggered the mass action.
    /// Used to suppress the interaction wheel from reopening after mass actions.
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

    private static bool ShouldMassBribe(FollowerCommands followerCommands)
    {
        if (!Plugin.MassBribe.Value) return false;
        if (followerCommands != FollowerCommands.Bribe) return false;
        var notBribed = Follower.Followers.Count(follower => FollowerCommandItems.Bribe().IsAvailable(follower));
        return notBribed > 1;
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
        if (followerCommands == null || followerCommands.Length == 0)
        {
            return;
        }

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
        if (!MassActionCosts.TryDeductCosts(eligibleCount)) return;

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
    }
}
