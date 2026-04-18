namespace GiveMeMoar;

[Harmony]
public static class Patches
{
    // Item-ID category lists. Drawn from the fork at game_code/other_mods/GiveMeMoarFork so
    // the IDs match what the game actually spawns.
    private static readonly HashSet<string> Crops = new(StringComparer.Ordinal)
    {
        "fruit:berry", "fruit:apple_green_crop", "fruit:apple_red_crop",
        "beet_crop", "carrot_crop", "cabbage_crop", "wheat_crop",
        "fruit:grapes_crop:1", "fruit:grapes_crop:2", "fruit:grapes_crop:3",
        "hamp_crop:1",
        "hop_crop:1", "hop_crop:2", "hop_crop:3",
        "lentils_crop:1", "lentils_crop:2", "lentils_crop:3",
        "onion_crop:1", "onion_crop:2", "onion_crop:3",
        "pumpkin_crop:1", "pumpkin_crop:2", "pumpkin_crop:3",
        "crop_waste", "hiccup_grass",
        "shr_agaric", "shr_boletus",
        "flw_chamomile", "flw_dandelion", "flw_poppy"
    };

    private static readonly HashSet<string> Seeds = new(StringComparer.Ordinal)
    {
        "wheat_seed", "cabbage_seed", "carrot_seed", "beet_seed",
        "onion_seed:1", "onion_seed:2", "onion_seed:3",
        "lentils_seed:1", "lentils_seed:2", "lentils_seed:3",
        "pumpkin_seed:1", "pumpkin_seed:2", "pumpkin_seed:3",
        "hop_seed:1", "hop_seed:2", "hop_seed:3",
        "hamp_seed:1",
        "grapes_seed:1", "grapes_seed:2", "grapes_seed:3"
    };

    private static readonly HashSet<string> Bugs = new(StringComparer.Ordinal)
    {
        "bee", "butterfly", "maggot", "moth"
    };

    private static readonly HashSet<string> Ores = new(StringComparer.Ordinal)
    {
        "1h_ore_metal", "stone_plate_1", "marble_plate_1", "sulfur",
        "clay", "coal", "nugget_silver", "nugget_gold", "graphite",
        "sand_river", "faceted_diamond", "lifestone"
    };

    private static readonly HashSet<string> Misc = new(StringComparer.Ordinal)
    {
        "honey", "beeswax", "ash", "peat", "taste_booster:salt",
        "water", "drop_alcohol", "egg_chicken", "jug_milk",
        "detail_trash", "pail_water"
    };

    private static readonly HashSet<string> BodyParts = new(StringComparer.Ordinal)
    {
        "blood", "flesh", "fat", "skin", "bone", "skull"
    };

    private static readonly HashSet<string> EnemyDrops = new(StringComparer.Ordinal)
    {
        "bat_wing", "jelly_slug", "jelly_slug_blue",
        "jelly_slug_orange", "jelly_slug_black", "slime",
        "spider_web", "nails_bloody"
    };

    private static readonly HashSet<string> Logs = new(StringComparer.Ordinal)
    {
        "wood1", "wooden_plank", "wood_balk_1", "flitch"
        // sticks are handled separately via MultiplySticks so users can exclude them without
        // losing billets/planks/beams/flitches.
    };

    // Craft definitions whose IDs contain any of these substrings get automatically skipped
    // when the user leaves "Exclude Progression Crafts" on. Mirrors QueueEverything's
    // UnSafeCraftDefPartials so progression mechanics (tier upgrades, object placement,
    // repair, body quality, etc.) keep their vanilla quantities.
    private static readonly string[] ProgressionCraftPartials =
    [
        "0_to_1", "1_to_2", "2_to_3", "3_to_4", "4_to_5", "upgr_to", "_to_lantern_",
        "rem_grave", "soul_workbench_craft", "burgers_place", "beer_barrels_place",
        "remove", "refugee", "upgrade", "fountain", "blockage", "obstacle",
        "builddesk", "fix", "broken", "elevator",
        "repair_", "place_tent", "find_zombie"
    ];

    // Snapshot of each craft's pre-mutation output values, keyed by "craft.id|output-index".
    // Used to restore vanilla values before re-applying when the user changes a craft setting.
    private static readonly Dictionary<string, int> CraftOutputSnapshots = new(StringComparer.Ordinal);
    private static bool _craftOutputApplied;

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameBalance), nameof(GameBalance.LoadGameBalance))]
    public static void GameBalance_LoadGameBalance_Postfix()
    {
        CaptureCraftOutputSnapshots();
        ApplyCraftOutputMultiplier();
    }

    internal static void RequestCraftOutputReapply()
    {
        if (!_craftOutputApplied) return;
        // GameBalance already loaded once; restore everything then re-apply so settings
        // changes are reflected immediately in the live craft_data.
        RestoreCraftOutputSnapshots();
        ApplyCraftOutputMultiplier();
    }

    private static void CaptureCraftOutputSnapshots()
    {
        if (GameBalance.me == null) return;
        CraftOutputSnapshots.Clear();

        foreach (var craft in GameBalance.me.craft_data)
        {
            if (craft == null || string.IsNullOrEmpty(craft.id)) continue;
            for (var i = 0; i < craft.output.Count; i++)
            {
                var output = craft.output[i];
                if (output == null || string.IsNullOrEmpty(output.id)) continue;
                // r/g/b are research-point outputs — never touched by the craft multiplier.
                if (output.id is "r" or "g" or "b") continue;
                CraftOutputSnapshots[$"{craft.id}|{i}"] = output.value;
            }
        }

        if (Plugin.DebugEnabled)
        {
            Helpers.Log($"[CraftSnapshot] captured {CraftOutputSnapshots.Count} output values across {GameBalance.me.craft_data.Count} craft definitions");
        }
    }

    private static void RestoreCraftOutputSnapshots()
    {
        if (GameBalance.me == null) return;
        var restored = 0;
        foreach (var craft in GameBalance.me.craft_data)
        {
            if (craft == null || string.IsNullOrEmpty(craft.id)) continue;
            for (var i = 0; i < craft.output.Count; i++)
            {
                if (!CraftOutputSnapshots.TryGetValue($"{craft.id}|{i}", out var original)) continue;
                craft.output[i].value = original;
                restored++;
            }
        }

        if (Plugin.DebugEnabled)
        {
            Helpers.Log($"[CraftSnapshot] restored {restored} output values");
        }
    }

    private static void ApplyCraftOutputMultiplier()
    {
        if (GameBalance.me == null) return;

        var globalMulti = Plugin.CraftOutputMultiplier.Value;
        var excludeTools = Plugin.CraftExcludeToolsAndEquipment.Value;
        var excludeProgression = Plugin.CraftExcludeProgressionCrafts.Value;
        var stationOverrides = Plugin.CraftStationOverrideMap;
        var excludedIds = BuildExcludedCraftIdSet(Plugin.CraftExcludedIds.Value);

        // Fast exit: global is 1.0, no overrides, nothing to do.
        var anyOverride = stationOverrides.Count > 0;
        if (Mathf.Approximately(globalMulti, 1f) && !anyOverride)
        {
            _craftOutputApplied = true;
            if (Plugin.DebugEnabled)
            {
                Helpers.Log("[CraftApply] global=1.0 and no per-station overrides — nothing to multiply");
            }
            return;
        }

        var mutatedCrafts = 0;
        var mutatedOutputs = 0;
        var skippedProgression = 0;
        var skippedExcludedId = 0;
        var skippedToolLike = 0;

        foreach (var craft in GameBalance.me.craft_data)
        {
            if (craft == null || string.IsNullOrEmpty(craft.id)) continue;

            if (excludedIds.Contains(craft.id))
            {
                skippedExcludedId++;
                continue;
            }

            if (excludeProgression && IsProgressionCraft(craft.id))
            {
                skippedProgression++;
                continue;
            }

            var multi = ResolveCraftMultiplier(craft, globalMulti, stationOverrides);
            if (Mathf.Approximately(multi, 1f) || multi <= 0f) continue;

            var craftMutated = false;
            for (var i = 0; i < craft.output.Count; i++)
            {
                var output = craft.output[i];
                if (output == null || string.IsNullOrEmpty(output.id)) continue;
                if (output.id is "r" or "g" or "b") continue;

                if (excludeTools && IsToolLikeOutput(output.id))
                {
                    skippedToolLike++;
                    continue;
                }

                if (!CraftOutputSnapshots.TryGetValue($"{craft.id}|{i}", out var baseValue))
                {
                    // New output that appeared after snapshot (shouldn't happen with our
                    // LoadGameBalance postfix, but be defensive).
                    baseValue = output.value;
                }

                var scaled = Mathf.Max(1, Mathf.RoundToInt(baseValue * multi));
                if (scaled == output.value) continue;

                output.value = scaled;
                mutatedOutputs++;
                craftMutated = true;
            }

            if (craftMutated) mutatedCrafts++;
        }

        _craftOutputApplied = true;

        if (Plugin.DebugEnabled || mutatedCrafts > 0)
        {
            Helpers.Log($"[CraftApply] global={globalMulti}, overrides={stationOverrides.Count}, excludedIds={excludedIds.Count}, excludeTools={excludeTools}, excludeProgression={excludeProgression} → mutatedCrafts={mutatedCrafts}, mutatedOutputs={mutatedOutputs}, skipped(progression={skippedProgression}, excludedId={skippedExcludedId}, toolLike={skippedToolLike})");
        }
    }

    private static HashSet<string> BuildExcludedCraftIdSet(string raw)
    {
        var set = new HashSet<string>(StringComparer.Ordinal);
        if (string.IsNullOrWhiteSpace(raw)) return set;
        foreach (var part in raw.Split(';'))
        {
            var trimmed = part.Trim();
            if (trimmed.Length == 0) continue;
            set.Add(trimmed);
        }
        return set;
    }

    private static bool IsProgressionCraft(string craftId)
    {
        foreach (var needle in ProgressionCraftPartials)
        {
            if (craftId.Contains(needle)) return true;
        }
        return false;
    }

    private static bool IsToolLikeOutput(string outputId)
    {
        if (GameBalance.me == null) return false;
        var def = GameBalance.me.GetDataOrNull<ItemDefinition>(outputId);
        if (def == null) return false;

        // is_tool covers Axe/Pickaxe/Shovel/Sword/Hammer/FishingRod/Torch.
        if (def.is_tool) return true;

        switch (def.type)
        {
            case ItemDefinition.ItemType.HeadArmor:
            case ItemDefinition.ItemType.BodyArmor:
            case ItemDefinition.ItemType.Preach:
                return true;
            default:
                return false;
        }
    }

    private static float ResolveCraftMultiplier(CraftDefinition craft, float globalMulti, Dictionary<string, float> stationOverrides)
    {
        if (stationOverrides.Count == 0) return globalMulti;
        foreach (var station in craft.craft_in)
        {
            if (stationOverrides.TryGetValue(station, out var overrideValue))
            {
                if (Plugin.DebugEnabled)
                {
                    Helpers.Log($"[CraftApply] station override '{station}' → multiplier={overrideValue} (craft='{craft.id}')");
                }
                return overrideValue;
            }
        }
        return globalMulti;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(PrayLogics), nameof(PrayLogics.SpreadFaithIncome))]
    private static void PrayLogics_SpreadFaithIncome(ref int faith)
    {
        var multi = Plugin.FaithMultiplier.Value;
        if (multi <= 0 || !MainGame.game_started)
        {
            if (Plugin.DebugEnabled)
            {
                Helpers.Log($"[Faith] Skipped: multi={multi}, game_started={MainGame.game_started}");
            }
            return;
        }

        var original = faith;
        faith = (int)(faith * multi);
        if (Plugin.DebugEnabled)
        {
            Helpers.Log($"[Faith] Original={original}, Multi={multi}, New={faith}");
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(SoulsHelper), nameof(SoulsHelper.CalculatePointsAfterSoulRelease))]
    private static void SoulsHelper_CalculatePointsAfterSoulRelease(ref float __result)
    {
        var multi = Plugin.GratitudeMultiplier.Value;
        if (multi <= 0 || !MainGame.game_started)
        {
            if (Plugin.DebugEnabled)
            {
                Helpers.Log($"[Gratitude] Skipped: multi={multi}, game_started={MainGame.game_started}, result={__result}");
            }
            return;
        }

        var original = __result;
        __result = Mathf.Round(__result * multi);
        if (Plugin.DebugEnabled)
        {
            Helpers.Log($"[Gratitude] Original={original}, Multi={multi}, New={__result}");
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(DropResGameObject), nameof(DropResGameObject.DoDrop), typeof(Item), typeof(int), typeof(bool))]
    private static void DropResGameObject_Drop(DropResGameObject __instance, ref Item drop_item)
    {
        if (!MainGame.game_started || drop_item == null)
        {
            if (Plugin.DebugEnabled && drop_item == null)
            {
                Helpers.Log("[Drop] Skipped: drop_item is null");
            }
            return;
        }

        var id = drop_item.id;
        var isBodyPartType = drop_item.definition.type == ItemDefinition.ItemType.BodyUniversalPart;

        if (Plugin.DebugEnabled)
        {
            Helpers.Log($"[Drop] Incoming id='{id}', type={drop_item.definition.type}, qty={drop_item.value}");
        }

        // Body-part items are only multiplied when the user opted in AND the ID is one of the
        // six common ones (blood/flesh/fat/skin/bone/skull). Organs and other specialised
        // body-part-typed items are always left alone to avoid overpowering the morgue loop.
        if (isBodyPartType)
        {
            if (Plugin.MultiplyBodyParts.Value && BodyParts.Contains(id))
            {
                ApplyResourceMultiplier(drop_item, "BodyPart");
            }
            else if (Plugin.DebugEnabled)
            {
                Helpers.Log($"[Drop] Skipped body part '{id}' (toggle={Plugin.MultiplyBodyParts.Value}, commonList={BodyParts.Contains(id)})");
            }
            return;
        }

        // Sin shards keep their dedicated multiplier.
        if (id == "sin_shard")
        {
            var sinMulti = Plugin.SinShardMultiplier.Value;
            if (sinMulti > 0 && !Mathf.Approximately(sinMulti, 1f))
            {
                var original = drop_item.value;
                drop_item.value = Mathf.RoundToInt(drop_item.value * sinMulti);
                if (Plugin.DebugEnabled)
                {
                    Helpers.Log($"[Drop] SinShard {original}×{sinMulti} → {drop_item.value}");
                }
            }
            else if (Plugin.DebugEnabled)
            {
                Helpers.Log($"[Drop] SinShard multi={sinMulti} — no change");
            }
            return;
        }

        // Sticks have their own dedicated toggle — keeps the "exclude sticks so they don't
        // flood inventory" option independent from the broader Logs category.
        if (id.Contains("stick"))
        {
            if (Plugin.MultiplySticks.Value)
            {
                ApplyResourceMultiplier(drop_item, "Stick");
            }
            else if (Plugin.DebugEnabled)
            {
                Helpers.Log($"[Drop] Skipped '{id}': Multiply Sticks is OFF");
            }
            return;
        }

        if (Plugin.MultiplyCrops.Value      && Crops.Contains(id))      { ApplyResourceMultiplier(drop_item, "Crops");      return; }
        if (Plugin.MultiplySeeds.Value      && Seeds.Contains(id))      { ApplyResourceMultiplier(drop_item, "Seeds");      return; }
        if (Plugin.MultiplyLogs.Value       && Logs.Contains(id))       { ApplyResourceMultiplier(drop_item, "Logs");       return; }
        if (Plugin.MultiplyOres.Value       && Ores.Contains(id))       { ApplyResourceMultiplier(drop_item, "Ores");       return; }
        if (Plugin.MultiplyBugs.Value       && Bugs.Contains(id))       { ApplyResourceMultiplier(drop_item, "Bugs");       return; }
        if (Plugin.MultiplyEnemyDrops.Value && EnemyDrops.Contains(id)) { ApplyResourceMultiplier(drop_item, "EnemyDrops"); return; }
        if (Plugin.MultiplyMisc.Value       && Misc.Contains(id))       { ApplyResourceMultiplier(drop_item, "Misc");       return; }

        if (Plugin.DebugEnabled)
        {
            Helpers.Log($"[Drop] '{id}' did not match any enabled category — no multiplier applied");
        }
    }

    private static void ApplyResourceMultiplier(Item item, string tag)
    {
        var multi = Plugin.ResourceMultiplier.Value;
        if (multi <= 0 || Mathf.Approximately(multi, 1f))
        {
            if (Plugin.DebugEnabled)
            {
                Helpers.Log($"[Drop] {tag} '{item.id}' — Resource Multiplier={multi}, no change");
            }
            return;
        }

        var original = item.value;
        item.value = Mathf.RoundToInt(item.value * multi);
        if (Plugin.DebugEnabled)
        {
            Helpers.Log($"[Drop] {tag} '{item.id}' {original}×{multi} → {item.value}");
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(RefugeesCampEngine), nameof(RefugeesCampEngine.UpdateHappiness))]
    private static void RefugeesCampEngine_UpdateHappiness(ref float happiness_delta)
    {
        var multi = Plugin.HappinessMultiplier.Value;
        if (multi <= 0 || !MainGame.game_started)
        {
            if (Plugin.DebugEnabled)
            {
                Helpers.Log($"[Happiness] Skipped: multi={multi}, game_started={MainGame.game_started}, delta={happiness_delta}");
            }
            return;
        }

        var original = happiness_delta;
        happiness_delta *= multi;
        if (Plugin.DebugEnabled)
        {
            Helpers.Log($"[Happiness] Original={original}, Multi={multi}, New={happiness_delta}");
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(PrayLogics), nameof(PrayLogics.SpreadMoneyIncome))]
    private static void PrayLogics_SpreadMoneyIncome(ref float money)
    {
        var multi = Plugin.DonationMultiplier.Value;
        if (multi <= 0 || !MainGame.game_started)
        {
            if (Plugin.DebugEnabled)
            {
                Helpers.Log($"[Donation] Skipped: multi={multi}, game_started={MainGame.game_started}, money={money}");
            }
            return;
        }

        var original = money;
        money = Mathf.RoundToInt(money * multi);
        if (Plugin.DebugEnabled)
        {
            Helpers.Log($"[Donation] Original={original}, Multi={multi}, New={money}");
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(TechPointsDrop), nameof(TechPointsDrop.Drop), typeof(Vector3), typeof(int), typeof(int), typeof(int))]
    private static void TechPointsDrop_Drop(ref int r, ref int g, ref int b)
    {
        if (!MainGame.game_started)
        {
            if (Plugin.DebugEnabled)
            {
                Helpers.Log($"[TechPoints] Skipped: game not started (r={r}, g={g}, b={b})");
            }
            return;
        }

        var redMultiplier   = Plugin.RedTechPointMultiplier.Value;
        var greenMultiplier = Plugin.GreenTechPointMultiplier.Value;
        var blueMultiplier  = Plugin.BlueTechPointMultiplier.Value;

        if (Plugin.DebugEnabled)
        {
            Helpers.Log($"[TechPoints] Incoming r={r}, g={g}, b={b}; multipliers red={redMultiplier}, green={greenMultiplier}, blue={blueMultiplier}");
        }

        if (redMultiplier > 0)
        {
            var original = r;
            r = Mathf.RoundToInt(r * redMultiplier);
            if (Plugin.DebugEnabled)
            {
                Helpers.Log($"[TechPoints] Red {original}×{redMultiplier} → {r}");
            }
        }

        if (greenMultiplier > 0)
        {
            var original = g;
            g = Mathf.RoundToInt(g * greenMultiplier);
            if (Plugin.DebugEnabled)
            {
                Helpers.Log($"[TechPoints] Green {original}×{greenMultiplier} → {g}");
            }
        }

        if (blueMultiplier > 0)
        {
            var original = b;
            b = Mathf.RoundToInt(b * blueMultiplier);
            if (Plugin.DebugEnabled)
            {
                Helpers.Log($"[TechPoints] Blue {original}×{blueMultiplier} → {b}");
            }
        }
    }
}
