namespace RestInPatches.Patches;

[Harmony]
public static class CraftArrowFixPatches
{

    private const string MaxButtonGuid = "p1xel8ted.gyk.maxbuttonsredux";

    [HarmonyPostfix]
    [HarmonyPatch(typeof(CraftItemGUI), nameof(CraftItemGUI.Redraw))]
    public static void CraftItemGUI_Redraw_RestoreArrows(CraftItemGUI __instance)
    {
        RestoreArrows(__instance);
    }


    [HarmonyPostfix]
    [HarmonyBefore(MaxButtonGuid)]
    [HarmonyPatch(typeof(CraftGUI), nameof(CraftGUI.Open), typeof(WorldGameObject), typeof(CraftsInventory), typeof(string))]
    public static void CraftGUI_Open_RestoreArrows(CraftGUI __instance)
    {
        RestoreArrowsForAll(__instance);
    }

    [HarmonyPostfix]
    [HarmonyBefore(MaxButtonGuid)]
    [HarmonyPatch(typeof(CraftGUI), nameof(CraftGUI.SwitchTab))]
    public static void CraftGUI_SwitchTab_RestoreArrows(CraftGUI __instance)
    {
        RestoreArrowsForAll(__instance);
    }

    private static void RestoreArrowsForAll(CraftGUI craftGui)
    {
        if (craftGui == null)
        {
            return;
        }

        if (Plugin.ArrowLeftSprite == null && Plugin.ArrowUpSprite == null && Plugin.ArrowDownSprite == null)
        {
            return;
        }

        foreach (var item in craftGui.GetComponentsInChildren<CraftItemGUI>(true))
        {
            RestoreArrows(item);
        }
    }

    private static void RestoreArrows(CraftItemGUI item)
    {
        if (item == null)
        {
            return;
        }

        if (Plugin.ArrowLeftSprite != null)
        {
            AssignIfMissing(item.btn_amount_plus, "arrow spr", Plugin.ArrowLeftSprite);
            AssignIfMissing(item.btn_amount_minus, "arrow spr", Plugin.ArrowLeftSprite);
        }

        if (Plugin.ArrowUpSprite != null)
        {
            RestoreArrSprite(item.full_detailed_go, Plugin.ArrowUpSprite);
        }

        if (Plugin.ArrowDownSprite != null)
        {
            RestoreArrSprite(item.multi_quality_go, Plugin.ArrowDownSprite);
        }
    }

    private static void RestoreArrSprite(GameObject root, Sprite sprite)
    {
        if (root == null)
        {
            return;
        }

        foreach (var spr in root.GetComponentsInChildren<UI2DSprite>(true))
        {
            if (spr == null || spr.sprite2D != null) continue;
            if (!string.Equals(spr.name, "arr", StringComparison.Ordinal)) continue;
            spr.sprite2D = sprite;
            spr.MarkAsChanged();
        }
    }

    private static void AssignIfMissing(UIButton btn, string childName, Sprite sprite)
    {
        if (btn == null)
        {
            return;
        }

        var arrow = btn.transform.Find(childName);
        if (arrow == null)
        {
            return;
        }

        var spr = arrow.GetComponent<UI2DSprite>();
        if (spr == null || spr.sprite2D != null)
        {
            return;
        }

        spr.sprite2D = sprite;
        spr.MarkAsChanged();
    }
}
