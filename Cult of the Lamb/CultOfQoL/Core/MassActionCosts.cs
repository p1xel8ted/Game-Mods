using System.Runtime.CompilerServices;

namespace CultOfQoL.Core;

internal static class MassActionCosts
{
    /// <summary>
    /// Checks gold affordability and deducts gold + advances game time.
    /// Costs are multiplied by the number of followers/targets affected.
    /// Returns false if the player can't afford the total gold cost (mass action should be skipped).
    /// </summary>
    internal static bool TryDeductCosts(int count, [CallerMemberName] string caller = "")
    {
        var goldPerTarget = Plugin.MassActionGoldCost.Value;
        if (goldPerTarget > 0f)
        {
            var totalGold = Mathf.CeilToInt(goldPerTarget * count);
            var currentGold = Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.BLACK_GOLD);
            if (currentGold < totalGold)
            {
                Plugin.WriteLog($"[MassActionCosts] {caller}: Not enough gold — have {currentGold}, need {totalGold} ({goldPerTarget:F2} x {count})");
                NotificationCentre.Instance?.PlayGenericNotification(
                    $"Not enough gold for mass action (need {totalGold})");
                return false;
            }

            Inventory.ChangeItemQuantity(InventoryItem.ITEM_TYPE.BLACK_GOLD, -totalGold);
            Plugin.WriteLog($"[MassActionCosts] {caller}: Deducted {totalGold} gold ({goldPerTarget:F2} x {count}), remaining: {currentGold - totalGold}");
        }

        var timePerTarget = Plugin.MassActionTimeCost.Value;
        if (timePerTarget > 0f)
        {
            var timeBefore = TimeManager.CurrentGameTime;
            TimeManager.CurrentGameTime += timePerTarget * count;
            Plugin.WriteLog($"[MassActionCosts] {caller}: Advanced time by {timePerTarget:F2} x {count} = {timePerTarget * count:F1} min (was {timeBefore:F1}, now {TimeManager.CurrentGameTime:F1})");
        }

        if (goldPerTarget <= 0f && timePerTarget <= 0f)
        {
            Plugin.WriteLog($"[MassActionCosts] {caller}: No costs configured, {count} targets approved");
        }

        return true;
    }

    /// <summary>
    /// Returns the faith multiplier for mass Bless/Inspire based on the MassFaithReduction config.
    /// 0% reduction = 1.0 (full faith), 50% = 0.5 (half faith), 100% = 0.0 (no faith).
    /// </summary>
    internal static float GetFaithMultiplier([CallerMemberName] string caller = "")
    {
        var reduction = Plugin.MassFaithReduction.Value;
        if (reduction <= 0)
        {
            return 1f;
        }

        var multiplier = 1f - (reduction / 100f);
        Plugin.WriteLog($"[MassActionCosts] {caller}: Faith reduction {reduction}% — multiplier {multiplier:F2}");
        return multiplier;
    }
}
