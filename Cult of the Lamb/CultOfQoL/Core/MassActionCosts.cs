using System.Runtime.CompilerServices;
using CultOfQoL.Patches.Followers;
using CultOfQoL.Patches.Systems;

namespace CultOfQoL.Core;

internal static class MassActionCosts
{
    /// <summary>
    /// Checks gold affordability and deducts gold + advances game time.
    /// Cost calculation depends on <see cref="MassActionCostMode"/>:
    /// <list type="bullet">
    ///   <item>PerMassAction — flat fee regardless of count</item>
    ///   <item>PerObject — cost multiplied by count</item>
    /// </list>
    /// Returns false if the player can't afford the total gold cost (mass action should be skipped).
    /// </summary>
    internal static bool TryDeductCosts(int count, [CallerMemberName] string caller = "")
    {
        var mode = Plugin.MassActionCostModeEntry.Value;
        var goldCost = Plugin.MassActionGoldCost.Value;
        var timeCost = Plugin.MassActionTimeCost.Value;

        var totalGold = mode == MassActionCostMode.PerMassAction
            ? goldCost
            : goldCost * count;

        var totalTime = mode == MassActionCostMode.PerMassAction
            ? timeCost
            : timeCost * count;

        // Gold affordability check
        if (totalGold > 0f)
        {
            var totalGoldInt = Mathf.CeilToInt(totalGold);
            var currentGold = Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.BLACK_GOLD);
            if (currentGold < totalGoldInt)
            {
                Plugin.WriteLog($"[MassActionCosts] {caller}: Not enough gold — have {currentGold}, need {totalGoldInt} ({mode}: {goldCost:F2} x {count})");
                NotificationCentre.Instance?.PlayGenericNotification(
                    $"Not enough gold for mass action (need {totalGoldInt})");
                return false;
            }

            Inventory.ChangeItemQuantity(InventoryItem.ITEM_TYPE.BLACK_GOLD, -totalGoldInt);
            Plugin.WriteLog($"[MassActionCosts] {caller}: Deducted {totalGoldInt} gold ({mode}: {goldCost:F2}, count {count}), remaining: {currentGold - totalGoldInt}");
        }

        // Time advancement
        if (totalTime > 0f)
        {
            var timeBefore = TimeManager.CurrentGameTime;
            TimeManager.CurrentGameTime += totalTime;
            Plugin.WriteLog($"[MassActionCosts] {caller}: Advanced time by {totalTime:F1} min ({mode}: {timeCost:F2}, count {count}), was {timeBefore:F1}, now {TimeManager.CurrentGameTime:F1}");
        }

        if (totalGold <= 0f && totalTime <= 0f)
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

    /// <summary>
    /// Returns the number of eligible targets for a given mass action command.
    /// Used by the wheel cost preview to calculate estimated costs before execution.
    /// </summary>
    internal static int GetMassActionTargetCount(FollowerCommands cmd)
    {
        return cmd switch
        {
            FollowerCommands.Bribe when Plugin.MassBribe.Value =>
                Follower.Followers.Count(f => FollowerCommandItems.Bribe().IsAvailable(f)),

            FollowerCommands.Bless when Plugin.MassBless.Value =>
                Follower.Followers.Count(f => FollowerCommandItems.Bless().IsAvailable(f)),

            FollowerCommands.Dance when Plugin.MassInspire.Value =>
                Follower.Followers.Count(f => FollowerCommandItems.Dance().IsAvailable(f)),

            FollowerCommands.Intimidate when Plugin.MassIntimidate.Value =>
                Follower.Followers.Count(f => FollowerCommandItems.Intimidate().IsAvailable(f)),

            FollowerCommands.ExtortMoney when Plugin.MassExtort.Value =>
                Follower.Followers.Count(f => FollowerCommandItems.Extort().IsAvailable(f)),

            FollowerCommands.Romance when Plugin.MassRomance.Value =>
                Follower.Followers.Count(f => FollowerCommandItems.Kiss().IsAvailable(f) && !FollowerManager.IsChild(f.Brain.Info.ID)),

            FollowerCommands.PetDog or FollowerCommands.PetFollower when Plugin.MassPetFollower.Value =>
                Follower.Followers.Count(f => FollowerCommandItems.PetDog().IsAvailable(f) && FollowerPatches.CanBePetted(f)),

            FollowerCommands.Bully when Plugin.MassBully.Value =>
                Follower.Followers.Count(f => FollowerCommandItems.Bully().IsAvailable(f)),

            FollowerCommands.Reassure when Plugin.MassReassure.Value =>
                Follower.Followers.Count(f => FollowerCommandItems.Reassure().IsAvailable(f)),

            FollowerCommands.Reeducate when Plugin.MassReeducate.Value =>
                Follower.Followers.Count(f => FollowerCommandItems.Reeducate().IsAvailable(f)),

            // Animal commands
            FollowerCommands.PetAnimal when Plugin.MassPetAnimals.Value =>
                Interaction_Ranchable.Ranchables.Count(a => a && !a.animal.PetToday),

            FollowerCommands.Clean when Plugin.MassCleanAnimals.Value =>
                Interaction_Ranchable.Ranchables.Count(a => a && a.Animal.Ailment == Interaction_Ranchable.Ailment.Stinky),

            FollowerCommands.MilkAnimal when Plugin.MassMilkAnimals.Value =>
                Interaction_Ranchable.Ranchables.Count(a => a && !a.Animal.MilkedToday && a.Animal.MilkedReady),

            FollowerCommands.Harvest when Plugin.MassShearAnimals.Value =>
                Interaction_Ranchable.Ranchables.Count(a => a && !a.Animal.WorkedToday && a.Animal.WorkedReady),

            _ when FastCollectingPatches.FeedCommands.Contains(cmd) && Plugin.MassFeedAnimals.Value =>
                Interaction_Ranchable.Ranchables.Count(a => a && !a.Animal.EatenToday),

            _ => 0
        };
    }

    /// <summary>
    /// Returns a cost preview string for display in the command wheel description,
    /// or null if the command isn't a mass action or preview is disabled.
    /// Called every frame for the highlighted wheel item via GetDescription() postfix.
    /// </summary>
    internal static string GetCostPreviewText(FollowerCommands cmd)
    {
        if (!Plugin.ShowMassActionCostPreview.Value) return null;

        var count = GetMassActionTargetCount(cmd);
        return GetCostPreviewTextForCount(count);
    }

    /// <summary>
    /// Returns a cost preview string for a given target count (used by non-wheel actions like farm plots).
    /// Returns null if preview is disabled, count is 0, or no costs are configured.
    /// </summary>
    internal static string GetCostPreviewTextForCount(int count)
    {
        if (!Plugin.ShowMassActionCostPreview.Value) return null;
        if (count <= 0) return null;
        if (Plugin.MassActionCostModeEntry.Value != MassActionCostMode.PerObject) return null;

        var mode = Plugin.MassActionCostModeEntry.Value;
        var goldCost = Plugin.MassActionGoldCost.Value;
        var timeCost = Plugin.MassActionTimeCost.Value;

        var totalGold = mode == MassActionCostMode.PerMassAction ? goldCost : goldCost * count;
        var totalTime = mode == MassActionCostMode.PerMassAction ? timeCost : timeCost * count;

        if (totalGold <= 0f && totalTime <= 0f) return null;

        var parts = new List<string>();
        if (totalGold > 0f)
        {
            parts.Add($"{Mathf.CeilToInt(totalGold)} gold");
        }
        if (totalTime > 0f)
        {
            parts.Add($"{totalTime:F1} min");
        }

        var modeLabel = mode == MassActionCostMode.PerMassAction ? "flat" : $"x{count}";
        return $"Mass cost: {string.Join(", ", parts)} ({modeLabel})";
    }
}
