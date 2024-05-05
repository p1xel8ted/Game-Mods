namespace CultOfQoL.Patches;

[HarmonyPatch]
public static class HealingBay
{

    private static IEnumerator StartHeal(FollowerBrainInfo followerBrain)
    {
        if (!Plugin.AddExhaustedToHealingBay.Value) yield break;
        followerBrain._brain.Stats.Exhaustion = 0f;
        FollowerBrainStats.OnExhaustionStateChanged?.Invoke(followerBrain._info.ID, FollowerStatState.Off, FollowerStatState.On);
        yield return new WaitForSeconds(6);
        NotificationCentre.Instance.PlayFaithNotification($"{followerBrain._info.Name} is no longer exhausted!", 0.001f, NotificationBase.Flair.None, followerBrain.ID, [followerBrain.Name]);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(NotificationFaith), nameof(NotificationFaith.Configure), typeof(string), typeof(float), typeof(FollowerInfo), typeof(bool), typeof(NotificationBase.Flair), typeof(string[]))]
    public static void NotificationFaith_Configure(NotificationFaith __instance, float faithDelta)
    {
        if (!Mathf.Approximately(faithDelta, 0.001f)) return;
        __instance._faithIcon.gameObject.SetActive(false);
        __instance._faithDeltaText.gameObject.SetActive(false);
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(UIFollowerSelectMenuController), nameof(UIFollowerSelectMenuController.Show), typeof(List<FollowerSelectEntry>), typeof(bool), typeof(UpgradeSystem.Type), typeof(bool), typeof(bool), typeof(bool), typeof(bool))]
    public static void UIFollowerSelectMenuController_Show(ref List<FollowerSelectEntry> followerSelectEntries)
    {
        var isHealingBay = ReflectionHelper.GetCallingClassName(3)!.Equals("Interaction_HealingBay", StringComparison.OrdinalIgnoreCase);
        if (!isHealingBay) return;
        Plugin.L($"Is Healing Bay: TRUE");

     
        followerSelectEntries.Clear();
        followerSelectEntries.AddRange(from follower in Helpers.AllFollowers
            where follower.Brain.Info.CursedState is Thought.FeelingUnwell or Thought.Ill or Thought.Injured or Thought.TiredFromMissionary or Thought.TiredFromMissionaryHappy or Thought.TiredFromMissionaryScared ||
                  follower.Brain.Stats.Exhaustion > 0f ||
                  follower.Brain.Stats.Illness > 0f ||
                  follower.Brain.Stats.Injured > 0f
            select new FollowerSelectEntry(follower));
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Interaction_HealingBay), nameof(Interaction_HealingBay.HealingRoutine))]
    public static void Interaction_HealingBay_HealingRoutine(ref Follower follower)
    {
        if (follower.Brain.Info.CursedState is Thought.FeelingUnwell or Thought.TiredFromMissionary or Thought.TiredFromMissionaryHappy or Thought.TiredFromMissionaryScared ||
            follower.Brain.Stats.Exhaustion > 0f)
        {
            GameManager.GetInstance().StartCoroutine(StartHeal(follower.Brain.Info));
        }
    }
}