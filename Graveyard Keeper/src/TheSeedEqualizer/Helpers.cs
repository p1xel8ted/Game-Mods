namespace TheSeedEqualizer;

public static class Helpers
{
    internal static void Log(string message, bool error = false)
    {
        if (error)
        {
            LogHelper.Error(message);
        }
        else
        {
            LogHelper.Info(message);
        }
    }

    private static void ModifyOutput(ObjectDefinition obj)
    {
        var seedOutputs = obj.drop_items.Where(a => a.id.Contains("seed")).ToList();
        if (Plugin.DebugEnabled)
        {
            Log($"[ModifyOutput/Obj] obj='{obj.id}' seedDrops={seedOutputs.Count}");
        }

        foreach (var output in seedOutputs)
        {
            string craft;
            if (output.id.EndsWith(":3"))
            {
                craft = output.id.Replace("_seed:3", "_planting_3");
            }
            else if (output.id.EndsWith(":2"))
            {
                craft = output.id.Replace("_seed:2", "_planting_2");
            }
            else if (output.id.EndsWith(":1"))
            {
                craft = output.id.Replace("_seed:1", "_planting_1");
            }
            else
            {
                craft = output.id.Replace("_seed", "_planting_1");
            }

            craft = craft.Replace("hamp", "cannabis");
            craft = $"garden_{craft}";

            if (Plugin.DebugEnabled)
            {
                Log($"[ModifyOutput/Obj] resolved seedOut='{output.id}' → craftDef='{craft}'");
            }

            var craftDef = GameBalance.me.GetDataOrNull<CraftDefinition>(craft);

            float minValue;
            if (craftDef != null)
            {
                minValue = craftDef.needs[0].value;
                if (Plugin.DebugEnabled)
                {
                    Log($"[ModifyOutput/Obj] '{output.id}' min_value ← {minValue} (from craftDef '{craft}'.needs[0])");
                }
            }
            else
            {
                minValue = 4f;
                if (Plugin.DebugEnabled)
                {
                    Log($"[ModifyOutput/Obj] '{output.id}' min_value ← 4 (no craftDef '{craft}' found, default)");
                }
            }
            output.min_value = SmartExpression.ParseExpression(minValue.ToString(CultureInfo.InvariantCulture));

            var maxBefore = output.max_value.EvaluateFloat();
            var normalBoost = maxBefore + 2;
            var extraBoost = maxBefore + 4;
            var boost = Plugin.BoostPotentialSeedOutput.Value ? extraBoost : normalBoost;
            output.max_value = SmartExpression.ParseExpression(boost.ToString(CultureInfo.InvariantCulture));

            if (Plugin.DebugEnabled)
            {
                Log($"[ModifyOutput/Obj] '{output.id}' max_value {maxBefore} → {boost} (boostPotential={Plugin.BoostPotentialSeedOutput.Value})");
            }
        }
    }

    private static void ModifyOutput(CraftDefinition craft)
    {
        var seedOutputs = craft.output.Where(a => a.id.Contains("seed")).ToList();
        if (Plugin.DebugEnabled)
        {
            Log($"[ModifyOutput/Craft] craft='{craft.id}' seedOutputs={seedOutputs.Count} need[0]={craft.needs[0].id}:{craft.needs[0].value}");
        }

        foreach (var output in seedOutputs)
        {
            var minValue = craft.needs[0].value;
            output.min_value = SmartExpression.ParseExpression(minValue.ToString(CultureInfo.InvariantCulture));

            var maxBefore = output.max_value.EvaluateFloat();
            var normalBoost = maxBefore + 2;
            var extraBoost = maxBefore + 4;
            var boost = Plugin.BoostPotentialSeedOutput.Value ? extraBoost : normalBoost;
            output.max_value = SmartExpression.ParseExpression(boost.ToString(CultureInfo.InvariantCulture));

            if (Plugin.DebugEnabled)
            {
                Log($"[ModifyOutput/Craft] '{output.id}' min ← {minValue}, max {maxBefore} → {boost} (boostPotential={Plugin.BoostPotentialSeedOutput.Value})");
            }
        }
    }

    internal static void GameBalancePostfix()
    {
        Plugin.Log.LogInfo("Running SeedEqualizer GameBalanceLoad as GameBalance has been loaded.");

        if (Plugin.DebugEnabled)
        {
            Log($"[GameBalancePostfix] config snapshot — playerGardens={Plugin.ModifyPlayerGardens.Value}, zombieGardens={Plugin.ModifyZombieGardens.Value}, zombieVineyards={Plugin.ModifyZombieVineyards.Value}, refugeeGardens={Plugin.ModifyRefugeeGardens.Value}, wasteToZGardens={Plugin.AddWasteToZombieGardens.Value}, wasteToZVineyards={Plugin.AddWasteToZombieVineyards.Value}, boostSeed={Plugin.BoostPotentialSeedOutput.Value}, rainGrowth={Plugin.BoostGrowSpeedWhenRaining.Value}");
        }

        var playerGardenCandidates = GameBalance.me.objs_data
            .Where(a => a.drop_items.Count > 0 && a.drop_items.Exists(b => b.id.Contains("seed")))
            .ToList();

        if (Plugin.DebugEnabled)
        {
            Log($"[GameBalancePostfix/PlayerGardens] scanning {playerGardenCandidates.Count} object definitions with seed drops");
        }

        var playerModified = 0;
        foreach (var craft in playerGardenCandidates)
        {
            if (!Plugin.ModifyPlayerGardens.Value)
            {
                continue;
            }
            if (!(craft.id.StartsWith("garden") && craft.id.EndsWith("ready")))
            {
                if (Plugin.DebugEnabled)
                {
                    Log($"[GameBalancePostfix/PlayerGardens] skip '{craft.id}': not a garden_*_ready definition");
                }
                continue;
            }

            if (Plugin.DebugEnabled)
            {
                Log($"[GameBalancePostfix/PlayerGardens] modifying '{craft.id}'");
            }
            ModifyOutput(craft);
            playerModified++;
        }

        if (Plugin.DebugEnabled)
        {
            Log($"[GameBalancePostfix/PlayerGardens] modified {playerModified} player garden definitions (enabled={Plugin.ModifyPlayerGardens.Value})");
        }

        var craftCandidates = GameBalance.me.craft_data
            .Where(a => a.needs.Count > 0 && a.needs.Exists(b => b.id.Contains("seed")))
            .ToList();

        if (Plugin.DebugEnabled)
        {
            Log($"[GameBalancePostfix/Crafts] scanning {craftCandidates.Count} craft definitions with seed inputs");
        }

        var zombieModified = 0;
        var vineyardModified = 0;
        var refugeeModified = 0;
        var wasteVineyardAdded = 0;
        var wasteGardenAdded = 0;

        foreach (var craft in craftCandidates)
        {
            if (craft.id.Contains("grow_desk_planting") && Plugin.ModifyZombieGardens.Value)
            {
                if (Plugin.DebugEnabled)
                {
                    Log($"[GameBalancePostfix/ZombieGardens] modifying '{craft.id}'");
                }
                ModifyOutput(craft);
                zombieModified++;
            }

            if (craft.id.Contains("grow_vineyard_planting") && Plugin.ModifyZombieVineyards.Value)
            {
                if (Plugin.DebugEnabled)
                {
                    Log($"[GameBalancePostfix/ZombieVineyards] modifying '{craft.id}'");
                }
                ModifyOutput(craft);
                vineyardModified++;
            }

            if (craft.id.StartsWith("refugee_garden") && Plugin.ModifyRefugeeGardens.Value)
            {
                if (Plugin.DebugEnabled)
                {
                    Log($"[GameBalancePostfix/RefugeeGardens] modifying '{craft.id}'");
                }
                ModifyOutput(craft);
                refugeeModified++;
            }

            if (craft.id.Contains("grow_vineyard_planting") && Plugin.AddWasteToZombieVineyards.Value && !craft.output.Exists(a => a.id == "crop_waste"))
            {
                if (Plugin.DebugEnabled)
                {
                    Log($"[GameBalancePostfix/Waste] adding crop_waste 3-5 to vineyard craft '{craft.id}'");
                }
                var item = new Item("crop_waste", 3)
                {
                    min_value = SmartExpression.ParseExpression("3"),
                    max_value = SmartExpression.ParseExpression("5"),
                    self_chance = craft.needs[0].self_chance
                };
                craft.output.Add(item);
                wasteVineyardAdded++;
            }
            else if (craft.id.Contains("grow_vineyard_planting") && Plugin.AddWasteToZombieVineyards.Value)
            {
                if (Plugin.DebugEnabled)
                {
                    Log($"[GameBalancePostfix/Waste] vineyard craft '{craft.id}' already drops crop_waste — skipping add");
                }
            }

            if (craft.id.Contains("grow_desk_planting") && Plugin.AddWasteToZombieGardens.Value && !craft.output.Exists(a => a.id == "crop_waste"))
            {
                if (Plugin.DebugEnabled)
                {
                    Log($"[GameBalancePostfix/Waste] adding crop_waste 3-5 to garden craft '{craft.id}'");
                }
                var item = new Item("crop_waste", 3)
                {
                    min_value = SmartExpression.ParseExpression("3"),
                    max_value = SmartExpression.ParseExpression("5"),
                    self_chance = craft.needs[0].self_chance
                };
                craft.output.Add(item);
                wasteGardenAdded++;
            }
            else if (craft.id.Contains("grow_desk_planting") && Plugin.AddWasteToZombieGardens.Value)
            {
                if (Plugin.DebugEnabled)
                {
                    Log($"[GameBalancePostfix/Waste] garden craft '{craft.id}' already drops crop_waste — skipping add");
                }
            }
        }

        if (Plugin.DebugEnabled)
        {
            Log($"[GameBalancePostfix] done — zombieGardens={zombieModified}, zombieVineyards={vineyardModified}, refugeeGardens={refugeeModified}, wasteAdded(vineyards)={wasteVineyardAdded}, wasteAdded(gardens)={wasteGardenAdded}");
        }
    }
}
