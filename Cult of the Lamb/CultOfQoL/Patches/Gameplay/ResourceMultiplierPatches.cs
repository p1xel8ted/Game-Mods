namespace CultOfQoL.Patches.Gameplay;

[Harmony]
public static class ResourceMultiplierPatches
{
    [ThreadStatic]
    private static bool _insideMultiplier;

    private static readonly HashSet<InventoryItem.ITEM_TYPE> ExcludedTypes =
    [
        // Meta / empty
        InventoryItem.ITEM_TYPE.NONE,
        InventoryItem.ITEM_TYPE.GENERIC,
        InventoryItem.ITEM_TYPE.UNUSED,

        // Category meta-types (not actual items)
        InventoryItem.ITEM_TYPE.SEEDS,
        InventoryItem.ITEM_TYPE.MEALS,
        InventoryItem.ITEM_TYPE.INGREDIENTS,
        InventoryItem.ITEM_TYPE.FOLLOWERS,
        InventoryItem.ITEM_TYPE.ANIMALS,

        // Tarot / weapon / curse cards
        InventoryItem.ITEM_TYPE.TRINKET_CARD,
        InventoryItem.ITEM_TYPE.TRINKET_CARD_UNLOCKED,
        InventoryItem.ITEM_TYPE.WEAPON_CARD,
        InventoryItem.ITEM_TYPE.CURSE_CARD,

        // Relics, doctrine, progression
        InventoryItem.ITEM_TYPE.RELIC,
        InventoryItem.ITEM_TYPE.DOCTRINE_STONE,
        InventoryItem.ITEM_TYPE.CRYSTAL_DOCTRINE_FRAGMENT,
        InventoryItem.ITEM_TYPE.CRYSTAL_DOCTRINE_STONE,
        InventoryItem.ITEM_TYPE.TALISMAN,
        InventoryItem.ITEM_TYPE.GOD_TEAR,
        InventoryItem.ITEM_TYPE.GOD_TEAR_FRAGMENT,
        InventoryItem.ITEM_TYPE.LORE_STONE,
        InventoryItem.ITEM_TYPE.FLOCKADE_PIECE,

        // Keys
        InventoryItem.ITEM_TYPE.KEY_PIECE,
        InventoryItem.ITEM_TYPE.KEY,

        // Map
        InventoryItem.ITEM_TYPE.MAP,

        // Blueprints
        InventoryItem.ITEM_TYPE.BLUE_PRINT,

        // Found items (unique unlocks)
        InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION,
        InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION_ALT,
        InventoryItem.ITEM_TYPE.FOUND_ITEM_WEAPON,
        InventoryItem.ITEM_TYPE.FOUND_ITEM_CURSE,
        InventoryItem.ITEM_TYPE.FOUND_ITEM_FOLLOWERSKIN,
        InventoryItem.ITEM_TYPE.FOUND_ITEM_OUTFIT,

        // Hearts
        InventoryItem.ITEM_TYPE.RED_HEART,
        InventoryItem.ITEM_TYPE.HALF_HEART,
        InventoryItem.ITEM_TYPE.BLUE_HEART,
        InventoryItem.ITEM_TYPE.HALF_BLUE_HEART,
        InventoryItem.ITEM_TYPE.BLACK_HEART,
        InventoryItem.ITEM_TYPE.PERMANENT_HALF_HEART,
        InventoryItem.ITEM_TYPE.FIRE_HEART,
        InventoryItem.ITEM_TYPE.ICE_HEART,

        // Necklaces
        InventoryItem.ITEM_TYPE.Necklace_1,
        InventoryItem.ITEM_TYPE.Necklace_2,
        InventoryItem.ITEM_TYPE.Necklace_3,
        InventoryItem.ITEM_TYPE.Necklace_4,
        InventoryItem.ITEM_TYPE.Necklace_5,
        InventoryItem.ITEM_TYPE.Necklace_Loyalty,
        InventoryItem.ITEM_TYPE.Necklace_Demonic,
        InventoryItem.ITEM_TYPE.Necklace_Dark,
        InventoryItem.ITEM_TYPE.Necklace_Light,
        InventoryItem.ITEM_TYPE.Necklace_Missionary,
        InventoryItem.ITEM_TYPE.Necklace_Gold_Skull,
        InventoryItem.ITEM_TYPE.Necklace_Bell,
        InventoryItem.ITEM_TYPE.Necklace_Deaths_Door,
        InventoryItem.ITEM_TYPE.Necklace_Winter,
        InventoryItem.ITEM_TYPE.Necklace_Frozen,
        InventoryItem.ITEM_TYPE.Necklace_Weird,
        InventoryItem.ITEM_TYPE.Necklace_Targeted,
        InventoryItem.ITEM_TYPE.DLC_NECKLACE,

        // Special wool (fleece unlocks)
        InventoryItem.ITEM_TYPE.SPECIAL_WOOL_RANCHER,
        InventoryItem.ITEM_TYPE.SPECIAL_WOOL_LAMBWAR,
        InventoryItem.ITEM_TYPE.SPECIAL_WOOL_BLACKSMITH,
        InventoryItem.ITEM_TYPE.SPECIAL_WOOL_TAROT,
        InventoryItem.ITEM_TYPE.SPECIAL_WOOL_DECORATION,
        InventoryItem.ITEM_TYPE.SPECIAL_WOOL_6,
        InventoryItem.ITEM_TYPE.SPECIAL_WOOL_7,
        InventoryItem.ITEM_TYPE.SPECIAL_WOOL_8,
        InventoryItem.ITEM_TYPE.SPECIAL_WOOL_9,
        InventoryItem.ITEM_TYPE.SPECIAL_WOOL_10,
        InventoryItem.ITEM_TYPE.SPECIAL_WOOL_11,

        // Legendary weapons
        InventoryItem.ITEM_TYPE.BROKEN_WEAPON_HAMMER,
        InventoryItem.ITEM_TYPE.BROKEN_WEAPON_SWORD,
        InventoryItem.ITEM_TYPE.BROKEN_WEAPON_DAGGER,
        InventoryItem.ITEM_TYPE.BROKEN_WEAPON_GAUNTLETS,
        InventoryItem.ITEM_TYPE.BROKEN_WEAPON_AXE,
        InventoryItem.ITEM_TYPE.BROKEN_WEAPON_BLUNDERBUSS,
        InventoryItem.ITEM_TYPE.BROKEN_WEAPON_CHAIN,
        InventoryItem.ITEM_TYPE.REPAIRED_WEAPON_HAMMER,
        InventoryItem.ITEM_TYPE.REPAIRED_WEAPON_SWORD,
        InventoryItem.ITEM_TYPE.REPAIRED_WEAPON_DAGGER,
        InventoryItem.ITEM_TYPE.REPAIRED_WEAPON_GAUNTLETS,
        InventoryItem.ITEM_TYPE.REPAIRED_WEAPON_BLUNDERBUSS,
        InventoryItem.ITEM_TYPE.REPAIRED_WEAPON_CHAIN,
        InventoryItem.ITEM_TYPE.LEGENDARY_WEAPON_FRAGMENT,
        InventoryItem.ITEM_TYPE.RATAU_STAFF,

        // Animals (entities, not resources)
        InventoryItem.ITEM_TYPE.ANIMAL_GOAT,
        InventoryItem.ITEM_TYPE.ANIMAL_TURTLE,
        InventoryItem.ITEM_TYPE.ANIMAL_CRAB,
        InventoryItem.ITEM_TYPE.ANIMAL_SPIDER,
        InventoryItem.ITEM_TYPE.ANIMAL_SNAIL,
        InventoryItem.ITEM_TYPE.ANIMAL_COW,
        InventoryItem.ITEM_TYPE.ANIMAL_LLAMA,

        // Gifts (special interaction rewards)
        InventoryItem.ITEM_TYPE.GIFT_SMALL,
        InventoryItem.ITEM_TYPE.GIFT_MEDIUM,

        // Souls (own currency system)
        InventoryItem.ITEM_TYPE.SOUL,
        InventoryItem.ITEM_TYPE.SOUL_FRAGMENT,
        InventoryItem.ITEM_TYPE.BLACK_SOUL,

        // Special tokens / currencies
        InventoryItem.ITEM_TYPE.TIME_TOKEN,
        InventoryItem.ITEM_TYPE.DISCIPLE_POINTS,
        InventoryItem.ITEM_TYPE.PLEASURE_POINT,

        // Misc unique items
        InventoryItem.ITEM_TYPE.EGG_FOLLOWER,
        InventoryItem.ITEM_TYPE.WEBBER_SKULL,
        InventoryItem.ITEM_TYPE.FISHING_ROD,
        InventoryItem.ITEM_TYPE.YNGYA_GHOST,
        InventoryItem.ITEM_TYPE.BOP,
        InventoryItem.ITEM_TYPE.ILLEGIBLE_LETTER_SCYLLA,
        InventoryItem.ITEM_TYPE.ILLEGIBLE_LETTER_CHARYBDIS,
        InventoryItem.ITEM_TYPE.FORGE_FLAME
    ];

    // Prefix (not transpiler) because we need to split one Spawn call into two operations:
    // physical Spawn for capped quantity + Inventory.AddItem for overflow. The original method
    // is still called in full via the recursion guard — no logic is skipped.
    [HarmonyPrefix]
    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.Spawn), typeof(InventoryItem.ITEM_TYPE), typeof(int), typeof(Vector3), typeof(float), typeof(Action<PickUp>))]
    public static bool InventoryItem_Spawn(InventoryItem.ITEM_TYPE type, int quantity, Vector3 position, float StartSpeed, Action<PickUp> result)
    {
        if (_insideMultiplier) return true;

        var multiplier = Plugin.ResourceDropMultiplier.Value;
        if (!Helpers.IsMultiplierActive(multiplier)) return true;

        if (ExcludedTypes.Contains(type)) return true;

        var totalQuantity = Mathf.CeilToInt(quantity * multiplier);
        if (totalQuantity <= quantity) return true;

        var spawnCap = Plugin.ResourceDropSpawnCap.Value;
        var toSpawn = Math.Min(totalQuantity, spawnCap);
        var toInventory = Math.Max(0, totalQuantity - toSpawn);

        _insideMultiplier = true;
        try
        {
            InventoryItem.Spawn(type, toSpawn, position, StartSpeed, result);
        }
        finally
        {
            _insideMultiplier = false;
        }

        if (toInventory > 0)
        {
            Inventory.AddItem(type, toInventory);

            if (PlayerFarming.Instance != null)
            {
                var visualCount = Math.Min(toInventory, 5);
                for (var i = 0; i < visualCount; i++)
                {
                    ResourceCustomTarget.Create(PlayerFarming.Instance.gameObject, position, type, null);
                }
            }
        }

        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.SpawnBlackSoul))]
    public static void InventoryItem_SpawnBlackSoul(ref int quantity)
    {
        if (_insideMultiplier) return;
        if (!Plugin.ResourceDropMultiplierBlackSouls.Value) return;

        var multiplier = Plugin.ResourceDropMultiplier.Value;
        if (!Helpers.IsMultiplierActive(multiplier)) return;

        quantity = Mathf.CeilToInt(quantity * multiplier);
    }
}
