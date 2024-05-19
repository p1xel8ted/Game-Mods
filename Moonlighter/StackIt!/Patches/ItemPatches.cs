namespace StackIt
{
    [HarmonyPatch]
    public static class ItemPatches
    {
        private readonly static Dictionary<string, int> ItemBackup = new();

        private static void ApplyModifications(IEnumerable<ItemMaster> items)
        {
            var stackIt = Plugin.StackIt.Value;
            var customStackSizes = Plugin.CustomStackSizes.Value;

            foreach (var item in items)
            {
                if (!ItemBackup.TryGetValue(item.nameKey, out var original))
                {
                    ItemBackup[item.nameKey] = item.maxStack;
                    if (Plugin.Debug.Value)
                    {
                        Plugin.LOG.LogInfo($"BACKUP: '{GetName(item)}': {item.maxStack}");
                    }
                }
                else
                {
                    if (!Modify(item)) continue;

                    item.maxStack = original;

                    if (stackIt)
                    {
                        DoubleStacks(item, original);
                    }
                    else if (customStackSizes)
                    {
                        CustomStackSizes(item, original);
                    }
                    if (Plugin.Debug.Value)
                    {
                        Plugin.LOG.LogInfo($"MODIFY: '{GetName(item)}': {original} => {item.maxStack}");
                    }
                }
            }
        }

        internal static void ReProcessAll()
        {
            var allItems = ItemDatabase.Instance.itemCollections
                .SelectMany(c => c.items.Concat(c.createdItems));
            ApplyModifications(allItems);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(ItemDatabase), nameof(ItemDatabase.LoadDLCItems), [])]
        [HarmonyPatch(typeof(ItemDatabase), nameof(ItemDatabase.LoadDLCItems), [typeof(string)])]
        [HarmonyPatch(typeof(ItemDatabase), nameof(ItemDatabase.ReadFromDLC))]
        [HarmonyPatch(typeof(ItemDatabase), nameof(ItemDatabase.ReadDefaultFile))]
        private static void ItemDatabase_LoadOrReadMethods()
        {
            ReProcessAll();
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(ItemStack), nameof(ItemStack.Init))]
        private static void ItemStack_Init(ItemStack __instance)
        {
            if (!Plugin.Debug.Value) return;

            Plugin.LOG.LogInfo(__instance.effect ? $"INIT: '{GetName(__instance)}' has a FixedStack: {__instance.MaxStack}" : $"INIT: '{GetName(__instance)}' has a MaxStack: {__instance.MaxStack}");
        }
 
        [HarmonyPostfix]
        [HarmonyPatch(typeof(ItemDatabase), nameof(ItemDatabase.GetItems))]
        [HarmonyPatch(typeof(ItemDatabase), nameof(ItemDatabase.GetGeneratedItems))]
        [HarmonyPatch(typeof(ItemDatabase), nameof(ItemDatabase.GetDLCItems))]
        private static void ItemDatabase_GetMethods(ref List<ItemMaster> __result)
        {
            ApplyModifications(__result);
        }

        private static void LogRejection(ItemMaster item, string reason)
        {
            if (!Plugin.Debug.Value) return;
            Plugin.LOG.LogInfo($"REJECTION: '{GetName(item)}' was rejected because {reason}");
        }

        private static string GetName(ItemMaster item)
        {
            var displayName = DigitalSunGames.Languages.I2.Localization.Get(item.nameKey);
            return string.IsNullOrWhiteSpace(displayName) ? item.nameKey : $"{displayName} ({item.nameKey})";
        }
        
        private static string GetName(ItemStack item)
        {
            var displayName = DigitalSunGames.Languages.I2.Localization.Get(item.master.nameKey);
            return string.IsNullOrWhiteSpace(displayName) ? item.master.nameKey : $"{displayName} ({item.master.nameKey})";
        }


        private static bool Modify(ItemMaster item)
        {
            if (!Plugin.CustomStackSizes.Value && !Plugin.StackIt.Value)
            {
                LogRejection(item, "Both StackIt and CustomStackSizes are disabled!");
                return false;
            }

            if (item.name.Equals("Coin", StringComparison.OrdinalIgnoreCase))
            {
                LogRejection(item, "item is a coin");
                return false;
            }

            var total = item.minChestStack + item.maxChestStack + item.maxStack + item.fixedChestStack;
            var result = !(total <= 4) || Plugin.IncludeUniqueItems.Value;

            if (!result)
            {
                LogRejection(item, "item is unique/quest item/etc.");
            }

            return result;
        }

        private static void DoubleStacks(ItemMaster item, int original)
        {
            if (original > 0)
            {
                item.maxStack = original * 2;
            }
        }

        private static void CustomStackSizes(ItemMaster item, int original)
        {
            if (original > 0 && item.maxStack < Plugin.MaxStackSize.Value)
            {
                item.maxStack = Plugin.MaxStackSize.Value;
            }
        }
    }
}