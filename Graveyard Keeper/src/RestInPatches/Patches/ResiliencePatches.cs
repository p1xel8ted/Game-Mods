namespace RestInPatches.Patches;

[Harmony]
public static class ResiliencePatches
{
    [HarmonyFinalizer]
    [HarmonyPatch(typeof(HUD), nameof(HUD.Update))]
    public static Exception HUD_Update_Finalizer(Exception __exception)
    {
        return __exception is IndexOutOfRangeException ? null : __exception;
    }

    [HarmonyFinalizer]
    [HarmonyPatch(typeof(InventoryPanelGUI), nameof(InventoryPanelGUI.OnItemOver), typeof(InventoryWidget), typeof(BaseItemCellGUI))]
    public static Exception InventoryPanelGUI_OnItemOver_Finalizer()
    {
        return null;
    }

    [HarmonyFinalizer]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.Interact))]
    public static Exception WorldGameObject_Interact_Finalizer()
    {
        return null;
    }

    [HarmonyFinalizer]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.SmartInstantiate))]
    public static Exception WorldGameObject_SmartInstantiate_Finalizer()
    {
        return null;
    }
}
