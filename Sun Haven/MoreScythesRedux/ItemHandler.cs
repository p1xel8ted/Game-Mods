namespace MoreScythesRedux;

public static class ItemHandler
{
    internal const int OriginalScytheId = 3000;
    internal const int AdamantScytheId = 21810;
    internal const int MithrilScytheId = 21811;
    internal const int SuniteScytheId = 21812;
    internal const int GloriteScytheId = 21813;
    private const string RecipeListAnvil = "new_Anvil";
    private const string RecipeListMonsterAnvil = "monster_anvil";
    
    internal static readonly List<(int id, int speed, int damage, string craftingStation, List<ItemInfo> inputs, float craftingHours, ItemData data)> CustomScythes = [];

    private static void CreateAndConfigureItem((int id, int speed, int damage, string craftingStation, List<ItemInfo> inputs, float craftingHours) scythe)
    {
        try
        {
            if (Database.Instance.ids.ContainsValue(scythe.id)) return;

            Database.GetData(OriginalScytheId, delegate(ItemData data)
            {
                if (!data)
                {
                    Plugin.LOG.LogError($"Original item with ID {OriginalScytheId} not found");
                    return;
                }

                var item = ScriptableObject.CreateInstance<ItemData>();
                var jsonData = FileLoader.LoadFile(Assembly.GetExecutingAssembly(), $"data.{scythe.id}.json");
                if (string.IsNullOrEmpty(jsonData))
                {
                    Plugin.LOG.LogError($"Failed to load JSON data for item {scythe.id}");
                    return;
                }

                JsonUtility.FromJsonOverwrite(jsonData, item);

                var iconData = FileLoader.LoadFileBytes(Assembly.GetExecutingAssembly(), $"img.{scythe.id}.png");
                if (iconData == null)
                {
                    Plugin.LOG.LogError($"Failed to load icon data for item {scythe.id}");
                    return;
                }

                item.icon = SpriteUtil.CreateSprite(iconData, $"Modded item icon {scythe.id}");

                var useItem = Object.Instantiate(data.useItem);
                if (!useItem)
                {
                    Plugin.LOG.LogError("Original scythe has no useItem");
                    return;
                }
                
                // Object.DontDestroyOnLoad(useItem);
                // Object.DontDestroyOnLoad(item);

                item.useItem = useItem;
                var weaponComponent = useItem.gameObject.GetComponent<Weapon>();
                var damageSourceComponent = useItem.gameObject.GetComponent<DamageSource>();

                if (!weaponComponent || !damageSourceComponent)
                {
                    Plugin.LOG.LogError("Required components not found on useItem");
                    return;
                }

                weaponComponent._frameRate = scythe.speed;
                damageSourceComponent._damageRange.Set(scythe.damage - 8, scythe.damage);
                useItem.gameObject.SetActive(false);

             

                Database.Instance.ids[item.name.RemoveWhitespace().ToLower()] = item.id;
                Database.Instance.types[item.id] = typeof(ItemData);
                Database.Instance.validIDs.Add(item.id);

                var node = Database.Instance.lruList.AddFirst(new Database.CacheItem(item.id, item));
                if (!Database.Instance.cache.ContainsKey(item.GetType()))
                {
                    Database.Instance.cache[item.GetType()] = new Dictionary<object, LinkedListNode<Database.CacheItem>>();
                }
                Database.Instance.cache[item.GetType()][item.id] = node;

                var originalSellInfo = Utils.GetItemSellInfo(data.id);
                var itemSellInfo = new ItemSellInfo
                {
                    name = item.name,
                    keyName = item.name,
                    sellPrice = item.sellPrice,
                    orbSellPrice = item.orbsSellPrice,
                    ticketSellPrice = item.ticketSellPrice,
                    stackSize = item.stackSize,
                    isMeal = originalSellInfo.isMeal,
                    isFruit = originalSellInfo.isFruit,
                    isArtisanryItem = originalSellInfo.isArtisanryItem,
                    isForageable = item.isForageable,
                    isPotion = originalSellInfo.isPotion,
                    isGem = item.isGem,
                    isAnimalProduct = item.isAnimalProduct,
                    itemType = originalSellInfo.itemType,
                    rarity = item.rarity,
                    decorationType = item.decorationType,
                };

                SingletonBehaviour<ItemInfoDatabase>.Instance.allItemSellInfos.Add(item.id, itemSellInfo);
                var newScythe = (scythe.id, scythe.speed, scythe.damage, scythe.craftingStation, scythe.inputs, scythe.craftingHours, item);

                if (CustomScythes.Exists(a => a.id == newScythe.id)) return;
                
                CustomScythes.Add(newScythe);  
                Plugin.LOG.LogInfo($"Created item {item.id} with name {item.name}");

            });
        }
        catch (Exception ex)
        {
            Plugin.LOG.LogError($"Exception while creating item {scythe.id}: {ex.Message}\n{ex.StackTrace}");
        }
    }

    public static void CreateScytheItems()
    {
        Database.GetData(ItemID.AdamantBar, delegate(ItemData data)
        {
            var adamantScythe = (AdamantScytheId, 13, 14, RecipeListAnvil, (List<ItemInfo>) [new ItemInfo {item = data, amount = 10}], 6f);
            CreateAndConfigureItem(adamantScythe);
        });

        Database.GetData(ItemID.MithrilBar, delegate(ItemData data)
        {
            var mithrilScythe = (MithrilScytheId, 14, 18, RecipeListAnvil, (List<ItemInfo>) [new ItemInfo {item = data, amount = 10}], 12f);
            CreateAndConfigureItem(mithrilScythe);
        });

        Database.GetData(ItemID.SuniteBar, delegate(ItemData data)
        {
            var suniteScythe = (SuniteScytheId, 15, 22, RecipeListAnvil, (List<ItemInfo>) [new ItemInfo {item = data, amount = 10}], 24f);
            CreateAndConfigureItem(suniteScythe);
        });

        Database.GetData(ItemID.GloriteBar, delegate(ItemData data)
        {
            var gloriteScythe = (GloriteScytheId, 16, 26, RecipeListMonsterAnvil, (List<ItemInfo>) [new ItemInfo {item = data, amount = 10}], 48f);
            CreateAndConfigureItem(gloriteScythe);
        });
    }
}