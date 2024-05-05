namespace Tweaks;

[Harmony]
public static class Patches
{
    [HarmonyPostfix]
    [HarmonyWrapSafe]
    [HarmonyPatch(typeof(ItemDetailsDisplay), nameof(ItemDetailsDisplay.ShowDetails))]
    private static void ItemDetailsDisplay_ShowDetails(ItemDetailsDisplay __instance, int detailCount)
    {
        try
        {
            var rowToGet = detailCount + 1;
            var row = __instance.GetRow(rowToGet);
            if (row is null) return;
            if (__instance.itemDisplay?.RefItem is null) return;
            var refItem = __instance.itemDisplay.RefItem;
            row.SetInfo("ItemID: ", refItem.ItemID.ToString());
            row.gameObject.SetActive(true);
            row.Show(true);
            if (row.m_lblDataValue.text.Contains("super long name", StringComparison.OrdinalIgnoreCase))
            {
                row.gameObject.SetActive(false);
                row.Show(false);
            }
        }
        catch (Exception)
        {
            // fail silently
        }
    }

    internal static CharacterUI CharacterUI = null!;

    [HarmonyPrefix]
    [HarmonyPatch(typeof(StartupVideo), nameof(StartupVideo.Awake))]
    [HarmonyPatch(typeof(StartupVideo), nameof(StartupVideo.Start))]
    public static void StartupVideo_Awake(ref StartupVideo __instance)
    {
        StartupVideo.HasPlayedOnce = true;
        __instance.m_done = true;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Debug), nameof(Debug.LogWarning), [typeof(object)])]
    [HarmonyPatch(typeof(Debug), nameof(Debug.LogWarning), [typeof(object), typeof(UnityEngine.Object)])]
    public static bool Debug_LogWarning(ref object message)
    {
        if (message is string s)
        {
            if (s.Contains("DontDestroyOnLoad", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
        }
        return true;
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(CharacterUI), nameof(CharacterUI.Awake))]
    [HarmonyPatch(typeof(CharacterUI), nameof(CharacterUI.Start))]
    private static void CharacterUI_Set(ref CharacterUI __instance)
    {
        CharacterUI = __instance;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Currency), nameof(Currency.GetGoldWeight))]
    [HarmonyPatch(typeof(Currency), nameof(Currency.GetSilverWeight))]
    [HarmonyPatch(typeof(Currency), nameof(Currency.Weight), MethodType.Getter)]
    [HarmonyPatch(typeof(Currency), nameof(Currency.CoinStackWeight), MethodType.Getter)]
    private static void Currency_GetGoldWeight(ref float __result)
    {
        __result = 0;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Item), nameof(Item.Weight), MethodType.Getter)]
    [HarmonyPatch(typeof(Item), nameof(Item.RawWeight), MethodType.Getter)]
    [HarmonyPatch(typeof(Item), nameof(Item.DefaultRawWeight), MethodType.Getter)]
    private static void Item_GetWeight(ref Item __instance, ref float __result)
    {
        if (__instance.Name.Contains("Key", StringComparison.OrdinalIgnoreCase) || __instance.Name.Contains("Arrow", StringComparison.OrdinalIgnoreCase))
        {
            __result = 0;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(EnvironmentItemDisplay), nameof(EnvironmentItemDisplay.RefreshShowTakeAll))]
    private static void EnvironmentItemDisplay_Show(ref EnvironmentItemDisplay __instance)
    {
        if (__instance.m_btnTakeAll == null || !__instance.m_btnTakeAll.isActiveAndEnabled)
        {
            return;
        }

        foreach (var containerDisplay in __instance.m_containerDisplayList.Where(containerDisplay => containerDisplay != null && containerDisplay.IsDisplayed))
        {
            var items = containerDisplay.ReferencedContainer.GetContainedItems();
            var itemDictionary = new Dictionary<int, int>();

            foreach (var item in items)
            {
                if (itemDictionary.TryGetValue(item.ItemID, out var count))
                {
                    itemDictionary[item.ItemID] = count + 1;
                }
                else
                {
                    itemDictionary.Add(item.ItemID, 1);
                }
            }

            containerDisplay.LocalCharacter.Inventory.NotifyItemTake(itemDictionary.Keys.ToArray(), itemDictionary.Values.ToArray());

            ContainerDisplay.TakeAllItems(containerDisplay);
        }
    }


}