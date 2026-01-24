using ReflectionHelper = CultOfQoL.Core.ReflectionHelper;

namespace CultOfQoL.Patches.Structures;

[Harmony]
public static class HealingBay
{
    private const float Duration = 3f;
    private const string InteractionHealingBay = "Interaction_HealingBay";
    private static FollowerBrainInfo _followerBrainInfo;
    private static bool _usingHealingBay;

    [HarmonyPostfix]
    [HarmonyPatch(typeof(NotificationFaith), nameof(NotificationFaith.Configure), typeof(string), typeof(float), typeof(FollowerInfo), typeof(bool), typeof(NotificationBase.Flair), typeof(bool), typeof(string[]))]
    public static void NotificationFaith_Configure(NotificationFaith __instance, float faithDelta)
    {
        if (!Mathf.Approximately(faithDelta, 0.001f)) return;
        __instance._faithIcon.gameObject.SetActive(false);
        __instance._faithDeltaText.gameObject.SetActive(false);
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(UIFollowerSelectMenuController), nameof(UIFollowerSelectMenuController.Show), typeof(List<FollowerSelectEntry>), typeof(bool), typeof(UpgradeSystem.Type), typeof(bool), typeof(bool), typeof(bool), typeof(bool), typeof(bool))]
    public static void UIFollowerSelectMenuController_Show_Prefix(UIFollowerSelectMenuController __instance, ref List<FollowerSelectEntry> followerSelectEntries)
    {
        var isHealingBay = ReflectionHelper.GetCallingClassName(3)!.Equals(InteractionHealingBay, StringComparison.OrdinalIgnoreCase);
        if (!isHealingBay) return;

        if (Plugin.AddExhaustedToHealingBay.Value)
        {
            foreach (var entry in followerSelectEntries)
            {
                if (entry.AvailabilityStatus != FollowerSelectEntry.Status.UnavailableDoesNotNeedHealing) continue;
                var brain = FollowerBrain.GetOrCreateBrain(entry.FollowerInfo);
                if (brain != null && IsExhausted(brain))
                {
                    entry.AvailabilityStatus = FollowerSelectEntry.Status.Available;
                }
            }
        }

        if (Plugin.HideHealthyFromHealingBay.Value)
        {
            followerSelectEntries.RemoveAll(entry => entry.AvailabilityStatus == FollowerSelectEntry.Status.UnavailableDoesNotNeedHealing);
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(FollowerInformationBox), nameof(FollowerInformationBox.ConfigureImpl))]
    public static void FollowerInformationBox_ConfigureImpl(FollowerInformationBox __instance)
    {
        if (!Plugin.AddExhaustedToHealingBay.Value) return;
        if (!_usingHealingBay) return;

        var hb = Resources.FindObjectsOfTypeAll<Interaction_HealingBay>().FirstOrDefault();
        if (hb == null) return;
        var isUpgraded = hb.structureBrain.Data.Type == StructureBrain.TYPES.HEALING_BAY_2;
        var costs = Interaction_HealingBay.GetCost(__instance.followBrain, isUpgraded);

        if (__instance._unavailableContainer.activeSelf) return;

        __instance.ShowItemCostValue(costs);
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Interaction_HealingBay), nameof(Interaction_HealingBay.OnFollowerChosenForConversion))]
    private static void Interaction_HealingBay_OnFollowerChosen(Interaction_HealingBay __instance, FollowerInfo followerInfo)
    {
        _usingHealingBay = false;

        if (!Plugin.AddExhaustedToHealingBay.Value) return;

        var brain = FollowerBrain.GetOrCreateBrain(followerInfo);
        if (brain?._directInfoAccess == null) return;
        if (brain._directInfoAccess.Exhaustion <= 0) return;

        __instance.StartCoroutine(HealExhaustionRoutine(brain.Info));
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Interaction_HealingBay), nameof(Interaction_HealingBay.OnHidden))]
    private static void Interaction_HealingBay_OnHidden()
    {
        _usingHealingBay = false;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Interaction_HealingBay), nameof(Interaction_HealingBay.OnInteract))]
    public static void Interaction_HealingBay_OnInteract(Interaction_HealingBay __instance, StateMachine state)
    {
        _usingHealingBay = true;
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(Interaction_HealingBay), nameof(Interaction_HealingBay.GetCost))]
    public static void Interaction_HealingBay_GetCost(ref List<InventoryItem> __result, FollowerBrain brain)
    {
        if (!Plugin.AddExhaustedToHealingBay.Value) return;

        _followerBrainInfo = brain.Info;

        if (IsExhausted(brain) || __result.Count <= 0)
        {
            var qty = 10;
            if (brain.Stats.Drunk > 0)
            {
                qty = 20;
            }
            __result = [new InventoryItem(InventoryItem.ITEM_TYPE.FLOWER_RED, qty)];
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

    private static IEnumerator HealExhaustionRoutine(FollowerBrainInfo follower)
    {
        var exhaustion = follower._brain._directInfoAccess.Exhaustion;
        if (exhaustion <= 0) yield break;

        var t = 0f;
        while (t < Duration)
        {
            if (Time.deltaTime > 0f)
            {
                t += Time.deltaTime;
                follower._brain.Stats.Exhaustion = Mathf.Lerp(exhaustion, 0f, t / Duration);
            }
            yield return null;
        }

        follower._brain.Stats.Exhaustion = 0f;
        FollowerBrainStats.OnExhaustionStateChanged?.Invoke(follower._brain.Info.ID, FollowerStatState.Off, FollowerStatState.On);
        NotificationCentre.Instance?.PlayFaithNotification($"{follower._brain.Info.Name} is no longer exhausted!", 0.001f, NotificationBase.Flair.None, follower._brain.Info.ID, follower._brain.Info.Name);
    }
}