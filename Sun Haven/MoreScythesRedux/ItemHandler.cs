using PSS;
using UnityEngine.ResourceManagement.Util;

namespace MoreScythesRedux;

public static class ItemHandler
{
    private const int OriginalScytheId = 3000;
    private const int AdamantScytheId = 30000;
    private const int MithrilScytheId = 30020;
    private const int SuniteScytheId = 30030;
    private const int GloriteScytheId = 30090;
    private const string RecipeListAnvil = "RecipeList_Anvil";
    private const string RecipeListMonsterAnvil = "RecipeList_Monster Anvil";

    private static void CreateAndConfigureItem(int id, int speed, int damage)
    {
        var original = Utils.GetItemData(id);

        var item = ScriptableObject.CreateInstance<ItemData>();
        JsonUtility.FromJsonOverwrite(
            FileLoader.LoadFile(Assembly.GetExecutingAssembly(), $"data.{id}.json"),
            item);

        item.icon = SpriteUtil.CreateSprite(
            FileLoader.LoadFileBytes(Assembly.GetExecutingAssembly(), $"img.{id}.png"),
            $"Modded item icon {id}");

        var useItem = Object.Instantiate(original.useItem);
        if (!useItem)
        {
            Plugin.LOG.LogError("Original scythe has no useItem");
            return;
        }

        item.useItem = useItem;
        useItem.gameObject.GetComponent<Weapon>()._frameRate = speed;
        useItem.gameObject.GetComponent<DamageSource>()._damageRange.Set(damage - 8, damage);
        useItem.gameObject.SetActive(false);

        Object.DontDestroyOnLoad(useItem);

        Database.Instance.validIDs.Add(item.id);
        Database.Instance.ids.TryAdd(item.name.RemoveWhitespace().ToLower(), item.id);
        Database.Instance.types.TryAdd(item.id, typeof(ToolItem));
        var node = Database.Instance.lruList.AddFirst(new Database.CacheItem(item.id, item));
        Database.Instance.cache[typeof(ToolItem)][item.id] = node;


        Plugin.LOG.LogInfo($"Created item {item.id} with name {item.name}");
    }

    private static void ConfigureRecipe(int itemId, string recipeListName, List<ItemInfo> inputs, float craftingHours)
    {
        foreach (var rl in Resources.FindObjectsOfTypeAll<RecipeList>())
        {
            if (!rl.name.Equals(recipeListName)) continue;

            if (rl.craftingRecipes.Exists(r => r.output.item.id == itemId))
            {
                continue;
            }

            var recipe = ScriptableObject.CreateInstance<Recipe>();
            recipe.output = new ItemInfo {item = Utils.GetItemData(itemId), amount = 1};
            recipe.input = inputs;
            recipe.worldProgressTokens = [];
            recipe.characterProgressTokens = [];
            recipe.questProgressTokens = [];
            recipe.hoursToCraft = craftingHours;

            rl.craftingRecipes.Add(recipe);
            Plugin.LOG.LogInfo($"Added item {itemId} to {recipeListName}");
        }
    }

    public static void CreateScytheItems()
    {
        var scytheDefinitions = new List<(int id, int speed, int damage, string recipeList, List<ItemInfo> inputs, float craftingHours)>
        {
            (AdamantScytheId, 13, 14, RecipeListAnvil, [new ItemInfo {item =  Utils.GetItemData(ItemID.AdamantBar), amount = 10}], 6f),
            (MithrilScytheId, 14, 18, RecipeListAnvil, [new ItemInfo {item =  Utils.GetItemData(ItemID.MithrilBar), amount = 10}], 12f),
            (SuniteScytheId, 15, 22, RecipeListAnvil, [new ItemInfo {item =  Utils.GetItemData(ItemID.SuniteBar), amount = 10}], 24f),
            (GloriteScytheId, 16, 26, RecipeListMonsterAnvil, [new ItemInfo {item =  Utils.GetItemData(ItemID.GloriteBar), amount = 10}], 48f),
        };

        foreach (var (id, speed, damage, recipeList, inputs, craftingHours) in scytheDefinitions)
        {
            CreateAndConfigureItem(id, speed, damage);
            ConfigureRecipe(id, recipeList, inputs, craftingHours);
        }
    }
}