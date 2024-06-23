namespace MoreScythesRedux;

[Harmony]
public static class Patches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Database), nameof(Database.GetCacheCapacity))]
    public static void DatabaseGetCacheCapacity(ref int __result)
    {
        __result = 999999;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(UseItem), nameof(UseItem.SetPlayer))]
    public static void UseItem_SetPlayer(ref UseItem __instance)
    {
        if (!__instance.gameObject.activeSelf)
        {
            __instance.gameObject.SetActive(true);
        }
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(CraftingTable), nameof(CraftingTable.Awake))]
    public static void CraftingTable_Awake(ref CraftingTable __instance)
    {
        foreach (var item in ItemHandler.CustomScythes)
        {
            if (!__instance.name.Contains(item.craftingStation, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            if (__instance.recipeList.craftingRecipes.Any(r => r.output2.id == item.id))
            {
                return;
            }

            var recipe = ScriptableObject.CreateInstance<Recipe>();
            recipe.worldProgressTokens = [];
            recipe.characterProgressTokens = [];
            recipe.questProgressTokens = [];
            recipe.hoursToCraft = item.craftingHours;

            recipe.output = new ItemInfo {item = item.data, amount = 1};
            recipe.output2 = new SerializedItemDataNamedAmount {id = item.id, name = item.data.name, amount = 1};

            recipe.input = item.inputs;
            recipe.input2 = [];

            foreach (var itemInfo in item.inputs)
            {
                recipe.input2.Add(new SerializedItemDataNamedAmount {id = itemInfo.item.id, name = itemInfo.item.name, amount = itemInfo.amount});
            }

            __instance.recipeList.craftingRecipes.Add(recipe);
            Plugin.LOG.LogInfo($"Added recipe for item {item.id} to crafting station {__instance.name}");
        }
    }
}