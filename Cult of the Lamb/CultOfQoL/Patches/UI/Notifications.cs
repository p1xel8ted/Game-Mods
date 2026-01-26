namespace CultOfQoL.Patches.UI;

[Harmony]
public static class Notifications
{
    private static readonly HashSet<int> StructuresWithNoFuel = [];
    private static readonly Dictionary<string, float> RecentNotifications = new();
    private const float NotificationCooldown = 10f;

    /// <summary>
    /// Notification types considered critical (deaths, equipment destruction, dissenters).
    /// These are still shown when "Allow Critical Notifications" is enabled.
    /// </summary>
    private static readonly HashSet<NotificationCentre.NotificationType> CriticalNotifications =
    [
        // Deaths
        NotificationCentre.NotificationType.Died,
        NotificationCentre.NotificationType.DiedFromStarvation,
        NotificationCentre.NotificationType.DiedFromIllness,
        NotificationCentre.NotificationType.DiedFromOldAge,
        NotificationCentre.NotificationType.KilledInAFightPit,
        NotificationCentre.NotificationType.KilledByBlizzardMonster,
        NotificationCentre.NotificationType.SacrificedAwayFromCult,
        NotificationCentre.NotificationType.MurderedByYou,
        NotificationCentre.NotificationType.ZombieKilledFollower,
        // Equipment destruction
        NotificationCentre.NotificationType.WeaponDestroyed,
        NotificationCentre.NotificationType.CurseDestroyed,
        // Faith crisis
        NotificationCentre.NotificationType.BecomeDissenter,
        NotificationCentre.NotificationType.LeaveCult
    ];

    /// <summary>
    /// Returns true if the notification with the given type should be allowed.
    /// </summary>
    private static bool ShouldAllowNotification(NotificationCentre.NotificationType type)
    {
        if (!Plugin.DisableAllNotifications.Value)
        {
            return true;
        }

        if (Plugin.AllowCriticalNotifications.Value && CriticalNotifications.Contains(type))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Returns true if generic notifications (without type) should be allowed.
    /// </summary>
    private static bool ShouldAllowGenericNotification()
    {
        return !Plugin.DisableAllNotifications.Value;
    }

    /// <summary>
    /// Returns true if notifications should be suppressed (in dungeon or during cutscene/transition).
    /// </summary>
    internal static bool ShouldSuppressNotification()
    {
        if (LocationManager.LocationIsDungeon(PlayerFarming.Location))
            return true;

        if (MMTransition.IsPlaying || LetterBox.IsPlaying || MMConversation.isPlaying)
            return true;

        return false;
    }

    /// <summary>
    /// Returns true if this notification message was recently shown (within cooldown period).
    /// </summary>
    internal static bool IsDuplicateNotification(string message)
    {
        var now = Time.time;

        // Clean expired entries
        var expiredKeys = RecentNotifications
            .Where(kvp => now - kvp.Value > NotificationCooldown)
            .Select(kvp => kvp.Key)
            .ToList();
        foreach (var key in expiredKeys)
        {
            RecentNotifications.Remove(key);
        }

        // Check if duplicate
        if (RecentNotifications.ContainsKey(message))
            return true;

        RecentNotifications[message] = now;
        return false;
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Scarecrow), nameof(Scarecrow.ShutTrap))]
    public static void Scarecrow_ShutTrap(ref Scarecrow __instance)
    {
        if (!Plugin.NotifyOfScarecrowTraps.Value) return;
        if (ShouldSuppressNotification()) return;

        var name = __instance.Structure.Structure_Info.GetLocalizedName();
        if (NotificationCentre.Instance.notificationsThisFrame.Contains(name)) return;

        // Clean up bird name (remove "(Clone)" suffix if present)
        var birdName = __instance.Bird.name.Replace("(Clone)", "").Trim();
        var message = $"{name} has caught a {birdName}!";
        if (IsDuplicateNotification(message)) return;

        NotificationCentre.Instance.PlayGenericNotificationNonLocalizedParams(message);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Structures_Bed), nameof(Structures_Bed.Collapse))]
    public static void Structures_Bed_Collapse(ref Structures_Bed __instance)
    {
        if (!Plugin.NotifyOfBedCollapse.Value) return;
        if (ShouldSuppressNotification()) return;

        var name = __instance.Data.GetLocalizedName();
        if (NotificationCentre.Instance.notificationsThisFrame.Contains(name)) return;

        var message = $"{name} has collapsed!";
        if (IsDuplicateNotification(message)) return;

        NotificationCentre.Instance.PlayGenericNotificationNonLocalizedParams(message);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Interaction_AddFuel), nameof(Interaction_AddFuel.Update))]
    public static void Interaction_AddFuel_Update(ref Interaction_AddFuel __instance)
    {
        if (!Plugin.NotifyOfNoFuel.Value) return;

        var structure = __instance.Structure?.Structure_Info;
        if (structure == null) return;

        var structureId = structure.ID;
        var hasFuel = structure.Fuel > 0;
        var wasNotified = StructuresWithNoFuel.Contains(structureId);

        if (!hasFuel && !wasNotified)
        {
            // Structure just ran out of fuel - notify (but not in dungeons/cutscenes)
            if (ShouldSuppressNotification()) return;

            var name = structure.GetLocalizedName();
            if (NotificationCentre.Instance.notificationsThisFrame.Contains(name)) return;

            var message = $"{name} has no fuel!";
            if (IsDuplicateNotification(message)) return;

            NotificationCentre.Instance.PlayGenericNotificationNonLocalizedParams(message);
            StructuresWithNoFuel.Add(structureId);
        }
        else if (hasFuel && wasNotified)
        {
            // Structure was refueled - clear notification state
            StructuresWithNoFuel.Remove(structureId);
        }
    }

    /// <summary>
    /// Vanilla game bug fix: Prevents NullReferenceException when FollowerInfo has invalid/empty SkinName.
    /// This can happen during relationship change notifications if follower data is corrupted.
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(SpineExtensions), nameof(SpineExtensions.ConfigureFollowerSkin),
        typeof(Spine.Unity.SkeletonGraphic), typeof(FollowerInfo))]
    public static bool SpineExtensions_ConfigureFollowerSkin(FollowerInfo followerInfo)
    {
        if (followerInfo == null || string.IsNullOrEmpty(followerInfo.SkinName))
        {
            return false;
        }

        var skinData = WorshipperData.Instance.GetColourData(followerInfo.SkinName);
        if (skinData?.Skin == null || skinData.Skin.Count == 0)
        {
            return false;
        }

        return true;
    }

    #region Disable All Notifications

    [HarmonyPrefix]
    [HarmonyPatch(typeof(NotificationCentre), nameof(NotificationCentre.PlayFollowerNotification))]
    [HarmonyPatch(typeof(NotificationCentre), nameof(NotificationCentre.PlayGenericNotification), typeof(NotificationCentre.NotificationType))]
    [HarmonyPatch(typeof(NotificationCentre), nameof(NotificationCentre.PlayRelationshipNotification))]
    public static bool NotificationCentre_PlayTypedNotification(NotificationCentre.NotificationType type)
    {
        return ShouldAllowNotification(type);
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(NotificationCentre), nameof(NotificationCentre.PlayGenericNotification), typeof(string), typeof(NotificationBase.Flair))]
    [HarmonyPatch(typeof(NotificationCentre), nameof(NotificationCentre.PlayGenericNotificationNonLocalizedParams))]
    [HarmonyPatch(typeof(NotificationCentre), nameof(NotificationCentre.PlayGenericNotificationLocalizedParams))]
    [HarmonyPatch(typeof(NotificationCentre), nameof(NotificationCentre.PlayItemNotification))]
    [HarmonyPatch(typeof(NotificationCentre), nameof(NotificationCentre.PlayFaithNotification))]
    [HarmonyPatch(typeof(NotificationCentre), nameof(NotificationCentre.PlaySinNotification))]
    [HarmonyPatch(typeof(NotificationCentre), nameof(NotificationCentre.PlayWarmthNotification))]
    [HarmonyPatch(typeof(NotificationCentre), nameof(NotificationCentre.PlayTwitchNotification))]
    [HarmonyPatch(typeof(NotificationCentre), nameof(NotificationCentre.PlayHelpHinderNotification))]
    public static bool NotificationCentre_PlayGenericNotification()
    {
        return ShouldAllowGenericNotification();
    }

    #endregion
}