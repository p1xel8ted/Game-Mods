namespace QueueEverything;

internal enum CraftCategory
{
    Misc,
    Alchemy,
    Cooking,
    Study,
    Metalwork,
    Morgue,
    Carpentry,
    Sermons,
    Printing,
    Winemaking,
    Pottery,
}

internal static class CraftCategories
{
    private static readonly Dictionary<CraftCategory, string[]> Members = new()
    {
        [CraftCategory.Alchemy] =
        [
            "mf_alchemy_craft_01", "mf_alchemy_craft_02", "mf_alchemy_craft_03",
            "mf_alchemy_survey", "mf_alchemy_mill", "mf_alchemy_stirrer_01"
        ],
        [CraftCategory.Cooking] =
        [
            "cooking_table", "cooking_table_2", "tavern_kitchen"
        ],
        [CraftCategory.Study] =
        [
            "desk", "desk_2", "table_book_constr"
        ],
        [CraftCategory.Metalwork] =
        [
            "mf_hammer_0", "mf_hammer_1",
            "mf_anvil_1", "mf_anvil_2", "mf_anvil_3",
            "mf_jewelry"
        ],
        [CraftCategory.Morgue] =
        [
            "mf_preparation_1", "mf_preparation_2", "autopsi_table", "packing_table"
        ],
        [CraftCategory.Carpentry] =
        [
            "mf_saw_1", "mf_chocks_1",
            "mf_workbench_1", "mf_workbench_2",
            "mf_beam_gantry_1"
        ],
        [CraftCategory.Sermons] =
        [
            "church_pulpit"
        ],
        [CraftCategory.Printing] =
        [
            "mf_printing_press", "mf_printing_press_2", "mf_paper_press"
        ],
        [CraftCategory.Winemaking] =
        [
            "mf_vine_press"
        ],
        [CraftCategory.Pottery] =
        [
            "mf_potter_wheel_1"
        ],
    };

    private static readonly Dictionary<string, CraftCategory> Reverse = BuildReverse();

    private static Dictionary<string, CraftCategory> BuildReverse()
    {
        var map = new Dictionary<string, CraftCategory>(StringComparer.Ordinal);
        foreach (var kv in Members)
        {
            foreach (var id in kv.Value)
            {
                map[id] = kv.Key;
            }
        }

        return map;
    }

    internal static CraftCategory Classify(List<string> craftIn)
    {
        if (craftIn == null || craftIn.Count == 0)
        {
            return CraftCategory.Misc;
        }

        foreach (var id in craftIn)
        {
            if (Reverse.TryGetValue(id, out var category))
            {
                return category;
            }
        }

        return CraftCategory.Misc;
    }

    internal static bool IsUncategorized(List<string> craftIn)
    {
        if (craftIn == null || craftIn.Count == 0)
        {
            return false;
        }

        foreach (var id in craftIn)
        {
            if (Reverse.ContainsKey(id))
            {
                return false;
            }
        }

        return true;
    }
}
