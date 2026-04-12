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
        if (Plugin.ArrowLeftSprite == null || craftGui == null)
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
        if (Plugin.ArrowLeftSprite == null || item == null)
        {
            return;
        }

        AssignIfMissing(item.btn_amount_plus);
        AssignIfMissing(item.btn_amount_minus);
    }

    private static void AssignIfMissing(UIButton btn)
    {
        if (btn == null)
        {
            return;
        }

        var arrow = btn.transform.Find("arrow spr");
        if (arrow == null)
        {
            return;
        }

        var spr = arrow.GetComponent<UI2DSprite>();
        if (spr == null || spr.sprite2D != null)
        {
            return;
        }

        spr.sprite2D = Plugin.ArrowLeftSprite;
        spr.MarkAsChanged();
    }
}
