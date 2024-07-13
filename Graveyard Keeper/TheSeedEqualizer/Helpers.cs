namespace TheSeedEqualizer;

public static class Helpers
{
    private static bool AlreadyRun { get; set; }

    private static void ModifyOutput(ObjectDefinition obj)
    {
        foreach (var output in obj.drop_items.Where(a => a.id.Contains("seed")))
        {
            Plugin.Log.LogInfo($"Initial craft def: {obj.id}");
            string craft;
            if (output.id.EndsWith(":3"))
            {
                craft = output.id.Replace("_seed:3", $"_planting_3");
            }
            else if (output.id.EndsWith(":2"))
            {
                craft = output.id.Replace("_seed:2", $"_planting_2");
            }
            else if (output.id.EndsWith(":1"))
            {
                craft = output.id.Replace("_seed:1", $"_planting_1");
            }
            else
            {
                craft = output.id.Replace("_seed", "_planting_1");
            }

            craft = craft.Replace("hamp", "cannabis");
            craft = $"garden_{craft}";

            Plugin.Log.LogInfo($"CraftDef for {output.id}: {craft}");


            var craftDef = GameBalance.me.GetDataOrNull<CraftDefinition>(craft);

            if (craftDef != null)
            {
                Plugin.Log.LogInfo($"Found corresponding craft, setting min_value of {output.id} to {craftDef.needs[0].value}");
                output.min_value = SmartExpression.ParseExpression(craftDef.needs[0].value.ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                Plugin.Log.LogInfo($"Did not find corresponding craft, setting min_value of {output.id} to 4.");
                output.min_value = SmartExpression.ParseExpression(4.ToString(CultureInfo.InvariantCulture));
            }

            var normalBoost = output.max_value.EvaluateFloat() + 2;
            var extraBoost = output.max_value.EvaluateFloat() + 4;
            var boost = Plugin.BoostPotentialSeedOutput.Value ? extraBoost : normalBoost;
            output.max_value = SmartExpression.ParseExpression(boost.ToString(CultureInfo.InvariantCulture));
        }
    }

    private static void ModifyOutput(CraftDefinition craft)
    {
        foreach (var output in craft.output.Where(a => a.id.Contains("seed")))
        {
            output.min_value = SmartExpression.ParseExpression(craft.needs[0].value.ToString(CultureInfo.InvariantCulture));
            var normalBoost = output.max_value.EvaluateFloat() + 2;
            var extraBoost = output.max_value.EvaluateFloat() + 4;
            var boost = Plugin.BoostPotentialSeedOutput.Value ? extraBoost : normalBoost;
            output.max_value = SmartExpression.ParseExpression(boost.ToString(CultureInfo.InvariantCulture));
        }
    }

    internal static void GameBalancePostfix()
    {
        if (AlreadyRun) return;
        AlreadyRun = true;
        Plugin.Log.LogInfo($"Running SeedEqualizer GameBalanceLoad as GameBalance has been loaded.");
        foreach (var craft in GameBalance.me.objs_data.Where(a => a.drop_items.Count > 0 && a.drop_items.Exists(b => b.id.Contains("seed"))))
        {
            if (Plugin.ModifyPlayerGardens.Value && craft.id.StartsWith("garden") && craft.id.EndsWith("ready"))
            {
                Plugin.Log.LogInfo($"Modifying Player Garden Seed Output in ObjData: {craft.id}");
                ModifyOutput(craft);
            }
        }

        foreach (var craft in GameBalance.me.craft_data.Where(a => a.needs.Count > 0 && a.needs.Exists(b => b.id.Contains("seed"))))
        {
            if (Tools.ZombieGardenCraft(craft.id) && Plugin.ModifyZombieGardens.Value)
            {
                Plugin.Log.LogInfo($"Modifying Zombie Garden Seed Output: {craft.id}");
                ModifyOutput(craft);
            }

            if (Tools.ZombieVineyardCraft(craft.id) && Plugin.ModifyZombieVineyards.Value)
            {
                Plugin.Log.LogInfo($"Modifying Zombie Vineyard Seed Output: {craft.id}");
                ModifyOutput(craft);
            }

            if (Tools.RefugeeGardenCraft(craft.id) && Plugin.ModifyRefugeeGardens.Value)
            {
                Plugin.Log.LogInfo($"Modifying Refugee Seed Output: {craft.id}");
                ModifyOutput(craft);
            }

            if (Tools.ZombieVineyardCraft(craft.id) && Plugin.AddWasteToZombieVineyards.Value && !craft.output.Exists(a => a.id == "crop_waste"))
            {
                Plugin.Log.LogInfo($"Adding Crop Waste To Zombie Vineyard Output: {craft.id}");
                var item = new Item("crop_waste", 3)
                {
                    min_value = SmartExpression.ParseExpression("3"),
                    max_value = SmartExpression.ParseExpression("5"),
                    self_chance = craft.needs[0].self_chance
                };
                craft.output.Add(item);
            }

            if (Tools.ZombieGardenCraft(craft.id) && Plugin.AddWasteToZombieGardens.Value && !craft.output.Exists(a => a.id == "crop_waste"))
            {
                Plugin.Log.LogInfo($"Adding Crop Waste To Zombie Garden Output: {craft.id}");
                var item = new Item("crop_waste", 3)
                {
                    min_value = SmartExpression.ParseExpression("3"),
                    max_value = SmartExpression.ParseExpression("5"),
                    self_chance = craft.needs[0].self_chance
                };
                craft.output.Add(item);
            }
        }
    }
}