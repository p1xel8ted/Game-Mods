namespace CultOfQoL.Core;

internal static class MassActionCosts
{
    /// <summary>
    /// Checks gold affordability and deducts gold + advances game time.
    /// Costs are multiplied by the number of followers/targets affected.
    /// Returns false if the player can't afford the total gold cost (mass action should be skipped).
    /// </summary>
    internal static bool TryDeductCosts(int count)
    {
        var goldPerTarget = Plugin.MassActionGoldCost.Value;
        if (goldPerTarget > 0)
        {
            var totalGold = goldPerTarget * count;
            if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.BLACK_GOLD) < totalGold)
            {
                NotificationCentre.Instance?.PlayGenericNotification(
                    $"Not enough gold for mass action (need {totalGold})");
                return false;
            }

            Inventory.ChangeItemQuantity(InventoryItem.ITEM_TYPE.BLACK_GOLD, -totalGold);
        }

        var timePerTarget = Plugin.MassActionTimeCost.Value;
        if (timePerTarget > 0)
        {
            TimeManager.CurrentGameTime += timePerTarget * count;
        }

        return true;
    }

    /// <summary>
    /// Returns the faith multiplier for mass Bless/Inspire based on the MassFaithReduction config.
    /// 0% reduction = 1.0 (full faith), 50% = 0.5 (half faith), 100% = 0.0 (no faith).
    /// </summary>
    internal static float GetFaithMultiplier()
    {
        var reduction = Plugin.MassFaithReduction.Value;
        if (reduction <= 0)
        {
            return 1f;
        }

        return 1f - (reduction / 100f);
    }
}
