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
        if (followerCommands == null || followerCommands.Length == 0)
        {
            return true;
        }

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
