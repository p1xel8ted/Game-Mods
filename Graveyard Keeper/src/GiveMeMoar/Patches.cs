namespace GiveMeMoar;

[Harmony]
public static class Patches
{
    private static readonly List<string> DropList =
    [
        "fruit:berry", "fruit:apple_green_crop", "fruit:apple_red_crop", "honey", "beeswax", "ash", "shr_agaric",
        "shr_boletus", "bat_wing", "jelly_slug",
        "jelly_slug_blue", "jelly_slug_orange", "jelly_slug_black", "bee", "slime", "spider_web", "1h_ore_metal",
        "nails_bloody", "nugget_silver", "nugget_gold",
        "graphite", "sand_river", "stick", "stone_plate_1", "sulfur", "clay", "coal", "lifestone", "butterfly",
        "maggot",
        "moth", "flw_chamomile", "flw_dandelion", "flw_poppy", "wheat_seed", "cabbage_seed", "carrot_seed",
        "beet_seed", "onion_seed:1", "onion_seed:2",
        "onion_seed:3", "lentils_seed:1", "lentils_seed:2", "lentils_seed:3", "pumpkin_seed:1", "pumpkin_seed:2",
        "pumpkin_seed:3", "hop_seed:1", "hop_seed:2", "hop_seed:3",
        "hamp_seed:1", "hamp_seed:2", "hamp_seed:3", "grapes_seed:1", "grapes_seed:2", "grapes_seed:3"
    ];

    [HarmonyPrefix]
    [HarmonyPatch(typeof(PrayLogics), nameof(PrayLogics.SpreadFaithIncome))]
    private static void PrayLogics_SpreadFaithIncome(ref int faith)
    {
        var multi = Plugin.FaithMultiplier.Value;
        if (multi <= 0 || !MainGame.game_started) return;
        var newFaith = faith * multi;
        Helpers.Log($"Original Faith: {faith}, Multiplier: {multi}, NewFaith: {newFaith}");
        faith = (int) newFaith;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(SoulsHelper), nameof(SoulsHelper.CalculatePointsAfterSoulRelease))]
    private static void SoulsHelper_CalculatePointsAfterSoulRelease(ref float __result)
    {
        var multi = Plugin.GratitudeMultiplier.Value;
        if (multi <= 0 || !MainGame.game_started) return;
        var grat = __result * multi;
        Helpers.Log($"Original gratitude: {__result}, Multiplier: {multi}, New gratitude: {grat}");
        __result = Mathf.Round(grat);
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(DropResGameObject), nameof(DropResGameObject.DoDrop), typeof(Item), typeof(int), typeof(bool))]
    private static void DropResGameObject_Drop(DropResGameObject __instance, ref Item drop_item)
    {
        if (!MainGame.game_started) return;


        if (drop_item == null) return;


        Helpers.Log($"InstanceNull: {__instance == null}, ItemNull: {drop_item == null}");
        if (drop_item.definition.type == ItemDefinition.ItemType.BodyUniversalPart)
        {
            Helpers.Log($"Item {drop_item.id} is a body part, skipping!");
            return;
        }

        if (drop_item.id.Contains("stick") && Plugin.MultiplySticks.Value)
        {
            Helpers.Log($"Item: {drop_item.id}, Stick is disabled in config. Skipping.");
            return;
        }

        Helpers.Log($"Dropping {drop_item.id} of item type {drop_item.definition.type} with quantity {drop_item.value}.");

        if (DropList.Contains(drop_item.id) && Plugin.ResourceMultiplier.Value > 0)
        {
            var newValue = drop_item.value * Plugin.ResourceMultiplier.Value;
            Helpers.Log($"Item: {drop_item.id}, Original item amount: {drop_item.value}, Multiplier: {Plugin.ResourceMultiplier.Value}, New item amount: {newValue}");
            drop_item.value = Mathf.RoundToInt(newValue);
            return;
        }

        if (drop_item.id == "sin_shard" && Plugin.SinShardMultiplier.Value > 0)
        {
            var newValue = drop_item.value * Plugin.SinShardMultiplier.Value;
            Helpers.Log($"Original sin shard amount: {drop_item.value}, Multiplier: {Plugin.SinShardMultiplier.Value}, New sin shard amount: {newValue}");
            drop_item.value = Mathf.RoundToInt(newValue);
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(RefugeesCampEngine), nameof(RefugeesCampEngine.UpdateHappiness))]
    private static void RefugeesCampEngine_UpdateHappiness(ref float happiness_delta)
    {
        var multi = Plugin.HappinessMultiplier.Value;
        if (multi <= 0 || !MainGame.game_started) return;
        var newHappiness = happiness_delta * multi;
        Helpers.Log($"Original Happiness: {happiness_delta}, Multiplier: {multi}, NewHappiness: {newHappiness}");
        happiness_delta = newHappiness;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(PrayLogics), nameof(PrayLogics.SpreadMoneyIncome))]
    private static void PrayLogics_SpreadMoneyIncome(ref float money)
    {
        var multi = Plugin.DonationMultiplier.Value;
        if (multi <= 0 || !MainGame.game_started) return;
        var newMoney = Mathf.RoundToInt(money * multi);
        Helpers.Log($"Original Money: {money}, Multiplier: {multi}, NewMoney: {newMoney}");
        money = newMoney;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(TechPointsDrop), nameof(TechPointsDrop.Drop), typeof(Vector3), typeof(int), typeof(int), typeof(int))]
    private static void TechPointsDrop_Drop(ref int r, ref int g, ref int b)
    {
        var gameStarted = MainGame.game_started;
        var redMultiplier = Plugin.RedTechPointMultiplier.Value;
        var greenMultiplier = Plugin.GreenTechPointMultiplier.Value;
        var blueMultiplier = Plugin.BlueTechPointMultiplier.Value;

        if (!gameStarted) return;

        if (redMultiplier > 0)
        {
            var newRed = r * redMultiplier;
            Helpers.Log($"Original Red: {r}, Multiplier: {redMultiplier}, NewRed: {newRed}");
            r = Mathf.RoundToInt(newRed);
        }

        if (greenMultiplier > 0)
        {
            var newGreen = g * greenMultiplier;
            Helpers.Log($"Original Green: {g}, Multiplier: {greenMultiplier}, NewGreen: {newGreen}");
            g = Mathf.RoundToInt(newGreen);
        }

        if (blueMultiplier > 0)
        {
            var newBlue = b * blueMultiplier;
            Helpers.Log($"Original Blue: {b}, Multiplier: {blueMultiplier}, NewBlue: {newBlue}");
            b = Mathf.RoundToInt(newBlue);
        }
    }
}