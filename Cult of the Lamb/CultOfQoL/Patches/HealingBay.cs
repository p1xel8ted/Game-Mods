namespace CultOfQoL.Patches;

[HarmonyPatch]
public static class HealingBay
{
    private const float duration = 3f;
    private const string InteractionHealingBay = "Interaction_HealingBay";
    private static FollowerBrainInfo _followerBrainInfo;
    private static float t;
    private static bool usingHealingBay;

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
    public static void UIFollowerSelectMenuController_Show_Prefix(UIFollowerSelectMenuController __instance, ref List<FollowerSelectEntry> followerSelectEntries)
    {
        if (!Plugin.AddExhaustedToHealingBay.Value) return;

        var isHealingBay = ReflectionHelper.GetCallingClassName(3)!.Equals(InteractionHealingBay, StringComparison.OrdinalIgnoreCase);
        if (!isHealingBay) return;

        followerSelectEntries.AddRange(from follower in Helpers.AllFollowers
            where IsExhausted(follower.Brain)
            select new FollowerSelectEntry(follower));
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(FollowerInformationBox), nameof(FollowerInformationBox.ConfigureImpl))]
    public static void FollowerInformationBox_ConfigureImpl(FollowerInformationBox __instance)
    {
        if (!Plugin.AddExhaustedToHealingBay.Value) return;
        if (!usingHealingBay) return;

        var hb = Resources.FindObjectsOfTypeAll<Interaction_HealingBay>().First();
        var costs = hb.GetCost(__instance.followBrain);

        if (__instance._unavailableContainer.activeSelf) return;

        __instance.ShowItemCostValue(costs);
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Interaction_HealingBay), nameof(Interaction_HealingBay.OnFollowerChosenForConversion))]
    [HarmonyPatch(typeof(Interaction_HealingBay), nameof(Interaction_HealingBay.OnHidden))]
    public static void Interaction_HealingBay_OnHidden(Interaction_HealingBay __instance)
    {
        usingHealingBay = false;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Interaction_HealingBay), nameof(Interaction_HealingBay.OnInteract))]
    public static void Interaction_HealingBay_OnInteract(Interaction_HealingBay __instance, StateMachine state)
    {
        usingHealingBay = true;
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(Interaction_HealingBay), nameof(Interaction_HealingBay.GetCost))]
    public static void Interaction_HealingBay_GetCost(ref List<InventoryItem> __result, FollowerBrain brain)
    {
        if (!Plugin.AddExhaustedToHealingBay.Value) return;

        _followerBrainInfo = brain.Info;

        if (IsExhausted(brain) || __result.Count <= 0)
        {
            __result = [new InventoryItem(InventoryItem.ITEM_TYPE.FLOWER_RED, 5)];
        }
    }

    private static bool IsExhausted(FollowerBrain brain)
    {
        return brain.Stats.Exhaustion > 0f;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(NotificationCentre), nameof(NotificationCentre.PlayFollowerNotification))]
    public static bool AddToNotificationHistory(NotificationCentre __instance, NotificationCentre.NotificationType type, FollowerBrainInfo info, NotificationFollower.Animation followerAnimation)
    {
        if (!Plugin.AddExhaustedToHealingBay.Value) return true;
        var isHealingBay = ReflectionHelper.GetCallingClassName(4)!.StartsWith(InteractionHealingBay, StringComparison.OrdinalIgnoreCase);
        if (isHealingBay && type == NotificationCentre.NotificationType.NoLongerIll && info.ID == _followerBrainInfo.ID)
        {
            return false;
        }
        return true;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Interaction_HealingBay), nameof(Interaction_HealingBay.HealingRoutine), MethodType.Enumerator)]
    private static void HealingRoutine(Interaction_HealingBay __instance)
    {
        if (!Plugin.AddExhaustedToHealingBay.Value) return;

        var follower = _followerBrainInfo;
        var exhaustion = follower._brain._directInfoAccess.Exhaustion;
        if (exhaustion <= 0) return;

        while (t < duration)
        {
            if (Time.deltaTime > 0f)
            {
                t += Time.deltaTime;
                var num = t / duration;
                follower._brain.Stats.Exhaustion = Mathf.Lerp(exhaustion, 0f, num);
            }
            break;
        }

        if (_followerBrainInfo._brain.Stats.Exhaustion != 0) return;

        FollowerBrainStats.OnExhaustionStateChanged.Invoke(_followerBrainInfo._brain.Info.ID, FollowerStatState.Off, FollowerStatState.On);
        NotificationCentre.Instance.PlayFaithNotification($"{_followerBrainInfo._brain.Info.Name} is no longer exhausted!", 0.001f, NotificationBase.Flair.None, _followerBrainInfo._brain.Info.ID, [_followerBrainInfo._brain.Info.Name]);
    }
}