using CultOfQoL.Core.Routines;

namespace CultOfQoL.Patches.Followers;

[Harmony]
public static class FollowerPatches
{
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


    [HarmonyPostfix]
    [HarmonyPatch(typeof(FollowerBrain), nameof(FollowerBrain.GetPersonalTask))]
    public static void FollowerBrain_GetTask(FollowerBrain __instance, FollowerLocation location, ref FollowerTask __result)
    {
        if (!Plugin.MakeOldFollowersWork.Value) return;

        if (__result is not FollowerTask_OldAge) return;

        var scheduledActivity = TimeManager.GetScheduledActivity(location);
        var found = false;
        if (scheduledActivity == ScheduledActivity.Work)
        {
            var task = FollowerBrain.GetDesiredTask_Work(location);
            task.RemoveAll(a => a is FollowerTask_OldAge);

            if (task.Count > 0) // Ensure the list has elements
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
                Plugin.L($"No available work tasks for elderly follower {__instance.Info.Name}.");
            }
        }

        if (found)
        {
            Plugin.L($"Elderly follower {__instance.Info.Name} set to work on {__result}.");
        }
    }


    private static IEnumerator ExtortMoneyRoutine(interaction_FollowerInteraction interaction)
    {
        yield return new WaitForEndOfFrame();
        interaction.follower.FacePosition(PlayerFarming.Instance.transform.position);
        yield return new WaitForSeconds(0.25f);
        int num;
        for (var i = 0; i < Random.Range(3, 7); i = num + 1)
        {
            ResourceCustomTarget.Create(PlayerFarming.Instance.gameObject, interaction.follower.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, delegate { Inventory.AddItem(20, 1); });
            yield return new WaitForSeconds(0.1f);
            num = i;
        }

        yield return new WaitForSeconds(0.25f);
    }

    private static IEnumerator RunEnumerator(bool run, IEnumerator enumerator, Action onComplete = null)
    {
        if (!run) yield break;
        yield return enumerator;
        yield return new WaitForSeconds(1f);
        onComplete?.Invoke();
    }

    private static bool ShouldMassBribe(FollowerCommands followerCommands)
    {
        if (!Plugin.MassBribe.Value) return false;
        if (followerCommands != FollowerCommands.Bribe) return false;
        var notBribed = Follower.Followers.Count(follower => FollowerCommandItems.Bribe().IsAvailable(follower));
        return notBribed > 1;
    }

    private static bool ShouldMassPetDog(FollowerCommands followerCommands)
    {
        if (!Plugin.MassPetDog.Value) return false;
        if (followerCommands != FollowerCommands.PetDog) return false;
        var notPetted = Follower.Followers.Count(follower => FollowerCommandItems.PetDog().IsAvailable(follower) && IsFollowerADog(follower.Brain));
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
        var notKissedCount = Follower.Followers.Count(follower => FollowerCommandItems.Kiss().IsAvailable(follower));
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

    private static bool IsFollowerADog(FollowerBrain brain)
    {
        if (brain.Info.SkinName.Contains("Dog", StringComparison.OrdinalIgnoreCase)) return true;
        Plugin.L($"Skipping {brain.Info.Name} because they are not a dog!");
        return false;
    }

    private static bool IsFollowerDissenting(FollowerBrain brain)
    {
        if (brain.Info.CursedState is not Thought.Dissenter) return false;
        Plugin.L($"Skipping {brain.Info.Name} because they are dissenting!");
        return true;
    }

    private static bool IsFollowerImprisoned(FollowerBrain brain)
    {
        if (!brain.Info.CursedState.ToString().Contains("Imprison", StringComparison.OrdinalIgnoreCase)) return false;
        Plugin.L($"Skipping {brain.Info.Name} because they are in prison!");
        return true;
    }

    private static bool IsFollowerAvailable(FollowerBrain brain)
    {
        if (brain.CurrentTaskType is not (FollowerTaskType.Sleep or FollowerTaskType.SleepBedRest or FollowerTaskType.Mating)) return true;
        Plugin.L($"Skipping {brain.Info.Name} because they are busy with task: {brain.CurrentTaskType.ToString()}");
        return false;
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.OnFollowerCommandFinalized), typeof(FollowerCommands[]))]
    public static void interaction_FollowerInteraction_OnFollowerCommandFinalized_Postfix(ref interaction_FollowerInteraction __instance, params FollowerCommands[] followerCommands)
    {
        if (followerCommands[0] is not (FollowerCommands.Reassure or FollowerCommands.Reeducate or FollowerCommands.Bully or FollowerCommands.Romance or FollowerCommands.PetDog or FollowerCommands.ExtortMoney or FollowerCommands.Dance or FollowerCommands.Intimidate or FollowerCommands.Bless or FollowerCommands.Bribe))
        {
            Plugin.L($"Skipping mass command because {followerCommands[0]} is not a mass command!");
            return;
        }

        var cmd = followerCommands[0];
        var originalFollower = __instance.follower;

        var followers = Helpers.AllFollowers.Where(f => f != originalFollower).ToList();

        foreach (var f in followers.Where(f => f.Interaction_FollowerInteraction))
        {
            f.Interaction_FollowerInteraction.playerFarming ??= PlayerFarming.Instance;
        }

        if (cmd == FollowerCommands.Reassure && ShouldMassReassure(followerCommands[0]))
        {
            foreach (var interaction in followers.Select(follower => follower.Interaction_FollowerInteraction))
            {
                var run = FollowerCommandItems.Reassure().IsAvailable(interaction.follower) && IsFollowerAvailable(interaction.follower.Brain) && !IsFollowerImprisoned(interaction.follower.Brain) && !IsFollowerDissenting(interaction.follower.Brain);
                interaction.StartCoroutine(RunEnumerator(run, interaction.ReassureRoutine(), delegate
                {
                    interaction.follower.Brain.Stats.ScaredTraitInteracted = true;
                    Plugin.L($"Reassured {interaction.follower.name}!");
                }));
            }
        }

        if (cmd == FollowerCommands.Reeducate && ShouldMassReeducate(followerCommands[0]))
        {
            foreach (var interaction in followers.Select(follower => follower.Interaction_FollowerInteraction))
            {
                var run = FollowerCommandItems.Reeducate().IsAvailable(interaction.follower) && IsFollowerAvailable(interaction.follower.Brain) && (IsFollowerImprisoned(interaction.follower.Brain) || IsFollowerDissenting(interaction.follower.Brain));
                interaction.StartCoroutine(RunEnumerator(run, interaction.ReeducateRoutine(), delegate
                {
                    interaction.follower.Brain.Stats.ReeducatedAction = true;
                    Plugin.L($"Re-educated {interaction.follower.name}!");
                }));
            }
        }

        if (cmd == FollowerCommands.Bully && ShouldMassBully(followerCommands[0]))
        {
            foreach (var interaction in followers.Select(follower => follower.Interaction_FollowerInteraction))
            {
                var run = FollowerCommandItems.Bully().IsAvailable(interaction.follower) && IsFollowerAvailable(interaction.follower.Brain) && !IsFollowerImprisoned(interaction.follower.Brain) && !IsFollowerDissenting(interaction.follower.Brain);
                interaction.StartCoroutine(RunEnumerator(run, interaction.BullyRoutine(), delegate
                {
                    interaction.follower.Brain.Stats.ScaredTraitInteracted = true;
                    Plugin.L($"Scared straight {interaction.follower.name}!");
                }));
            }
        }

        if (cmd == FollowerCommands.Romance && ShouldMassRomance(followerCommands[0]))
        {
            foreach (var interaction in followers.Select(follower => follower.Interaction_FollowerInteraction))
            {
                var run = FollowerCommandItems.Kiss().IsAvailable(interaction.follower) && IsFollowerAvailable(interaction.follower.Brain) && !IsFollowerImprisoned(interaction.follower.Brain) && !IsFollowerDissenting(interaction.follower.Brain);
                interaction.StartCoroutine(RunEnumerator(run, interaction.RomanceRoutine(), delegate
                {
                    interaction.follower.Brain.Stats.KissedAction = true;
                    Plugin.L($"Romanced {interaction.follower.name}!");
                }));
            }
        }

        if (cmd == FollowerCommands.PetDog && ShouldMassPetDog(followerCommands[0]))
        {
            foreach (var interaction in followers.Select(follower => follower.Interaction_FollowerInteraction))
            {
                var isDog = IsFollowerADog(interaction.follower.Brain);
                Plugin.L($"Is {interaction.follower.name} a dog? {isDog}");
                var run = FollowerCommandItems.PetDog().IsAvailable(interaction.follower) && isDog && IsFollowerAvailable(interaction.follower.Brain) && !IsFollowerImprisoned(interaction.follower.Brain) && !IsFollowerDissenting(interaction.follower.Brain);
                interaction.StartCoroutine(RunEnumerator(run, interaction.PetDogRoutine(), delegate
                {
                    interaction.follower.Brain.Stats.PetDog = true;
                    Plugin.L($"Petted {interaction.follower.name}!");
                }));
            }
        }

        if (cmd == FollowerCommands.ExtortMoney && ShouldMassExtort(followerCommands[0]))
        {
            foreach (var interaction in followers.Select(follower => follower.Interaction_FollowerInteraction))
            {
                var run = FollowerCommandItems.Extort().IsAvailable(interaction.follower) && IsFollowerAvailable(interaction.follower.Brain) && !IsFollowerImprisoned(interaction.follower.Brain) && !IsFollowerDissenting(interaction.follower.Brain);
                interaction.StartCoroutine(RunEnumerator(run, ExtortMoneyRoutine(interaction), delegate
                {
                    interaction.follower.Brain.Stats.PaidTithes = true;
                    Plugin.L($"Extorted {interaction.follower.name}!");
                }));
            }
        }

        if (cmd == FollowerCommands.Dance && ShouldMassInspire(followerCommands[0]))
        {
            foreach (var interaction in followers.Select(follower => follower.Interaction_FollowerInteraction))
            {
                var run = FollowerCommandItems.Dance().IsAvailable(interaction.follower) && IsFollowerAvailable(interaction.follower.Brain) && !IsFollowerImprisoned(interaction.follower.Brain) && !IsFollowerDissenting(interaction.follower.Brain);
                interaction.StartCoroutine(RunEnumerator(run, interaction.DanceRoutine(false), delegate
                {
                    interaction.follower.Brain.Stats.Inspired = true;
                    Plugin.L($"Inspired {interaction.follower.name}!");
                }));
            }
        }

        if (cmd == FollowerCommands.Intimidate && ShouldMassIntimidate(followerCommands[0]))
        {
            foreach (var interaction in followers.Select(follower => follower.Interaction_FollowerInteraction))
            {
                var run = FollowerCommandItems.Intimidate().IsAvailable(interaction.follower) && IsFollowerAvailable(interaction.follower.Brain) && !IsFollowerImprisoned(interaction.follower.Brain) && !IsFollowerDissenting(interaction.follower.Brain);
                interaction.StartCoroutine(RunEnumerator(run, interaction.IntimidateRoutine(false, PlayerFarming.Instance), delegate
                {
                    interaction.follower.Brain.Stats.Intimidated = true;
                    Plugin.L($"Intimidated {interaction.follower.name}!");
                }));
            }
        }


        if (cmd == FollowerCommands.Bless && ShouldMassBless(followerCommands[0]))
        {
            foreach (var interaction in followers.Select(follower => follower.Interaction_FollowerInteraction))
            {
                var run = FollowerCommandItems.Bless().IsAvailable(interaction.follower) && IsFollowerAvailable(interaction.follower.Brain) && !IsFollowerImprisoned(interaction.follower.Brain) && !IsFollowerDissenting(interaction.follower.Brain);
                interaction.StartCoroutine(RunEnumerator(run, interaction.BlessRoutine(false, PlayerFarming.Instance), delegate
                {
                    interaction.follower.Brain.Stats.ReceivedBlessing = true;
                    interaction.follower.Brain.Stats.LastBlessing = DataManager.Instance.CurrentDayIndex;
                    Plugin.L($"Blessed {interaction.follower.name}!");
                }));
            }
        }


        if (cmd == FollowerCommands.Bribe && ShouldMassBribe(followerCommands[0]))
        {
            foreach (var interaction in followers.Select(follower => follower.Interaction_FollowerInteraction))
            {
                var run = FollowerCommandItems.Bribe().IsAvailable(interaction.follower) && IsFollowerAvailable(interaction.follower.Brain) && !IsFollowerImprisoned(interaction.follower.Brain) && !IsFollowerDissenting(interaction.follower.Brain);
                interaction.StartCoroutine(RunEnumerator(run, interaction.BribeRoutine(), delegate
                {
                    interaction.follower.Brain.Stats.Bribed = true;
                    Plugin.L($"Bribed {interaction.follower.name}!");
                }));
            }
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
            Plugin.L($"Resetting follower {__instance.follower.name} from exhaustion!");
        }

        if (__instance.follower.Brain.Stats.Illness > 0)
        {
            __instance.follower.Brain._directInfoAccess.Illness = 0f;
            __instance.follower.Brain.Stats.Illness = 0f;
            var onIllnessStateChanged = FollowerBrainStats.OnIllnessStateChanged;
            onIllnessStateChanged?.Invoke(__instance.follower.Brain._directInfoAccess.ID, FollowerStatState.Off, FollowerStatState.On);
            Plugin.L($"Resetting follower {__instance.follower.name} from illness!");
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

        GameManager.GetInstance().StartCoroutine(MassLevelUpAll(originalFollower));
    }

    private static IEnumerator MassLevelUpAll(Follower original)
    {
        MassLevelUpRunning = true;
        yield return new WaitForSeconds(0.5f);

        foreach (var follower in Helpers.AllFollowers)
        {
            if (follower == original) continue;
            if (!follower.Brain.CanLevelUp()) continue;

            var interaction = follower.Interaction_FollowerInteraction;
            if (!interaction) continue;

            interaction.playerFarming = PlayerFarming.Instance;
            interaction.StartCoroutine(interaction.LevelUpRoutine(follower.Brain.CurrentTaskType, null, false, false, false));
            yield return new WaitForSeconds(0.5f);
        }

        MassLevelUpRunning = false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.Close), typeof(bool), typeof(bool), typeof(bool))]
    public static void interaction_FollowerInteraction_Close(ref bool DoResetFollower, ref bool unpause, ref bool reshowMenu)
    {
        if (!RoutinesTranspilers.AnyMassActionsEnabled) return;

        DoResetFollower = true;
        unpause = true;
        reshowMenu = false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.LevelUpRoutine))]
    public static void interaction_FollowerInteraction_LevelUpRoutine(ref Action Callback, ref bool GoToAndStop, ref bool onFinishClose)
    {
        if (!RoutinesTranspilers.AnyMassActionsEnabled) return;

        Callback = null;
        GoToAndStop = false;
        onFinishClose = true;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(FollowerCommandItems), nameof(FollowerCommandItems.GiveWorkerCommand))]
    public static void interaction_FollowerInteraction_OnFollowerCommandFinalized(Follower follower, ref CommandItem __result)
    {
        if (Plugin.MakeOldFollowersWork.Value && follower.Brain.Info.CursedState == Thought.OldAge)
        {
            __result.SubCommands = FollowerCommandGroups.GiveWorkerCommands(follower);
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.OnFollowerCommandFinalized))]
    public static bool interaction_FollowerInteraction_OnFollowerCommandFinalized(ref interaction_FollowerInteraction __instance, params FollowerCommands[] followerCommands)
    {
        var makeOldPeopleWork = Plugin.MakeOldFollowersWork.Value;
        var command = followerCommands[0] == FollowerCommands.GiveWorkerCommand_2 || followerCommands[0] == FollowerCommands.MakeDemand;
        var old = __instance.follower.Brain.Info.CursedState == Thought.OldAge;
        if (makeOldPeopleWork && command && old)
        {
            __instance.follower.Brain.CompleteCurrentTask();
            var task = FollowerBrain.GetDesiredTask_Work(__instance.follower.Brain.Location).Random();
            __instance.follower.Brain.HardSwapToTask(task);
            NotificationCentre.Instance.PlayGenericNotification($"{__instance.follower.Brain.Info.Name} sent to work on random task!", NotificationBase.Flair.Positive);
            Plugin.L($"Old follower {__instance.follower.name} made to work on {task}");
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

        if (Plugin.MakeOldFollowersWork.Value)
        {
            __result.Add(FollowerCommandItems.GiveWorkerCommand(follower));
        }
    }
}