namespace RestInPatches.Patches;

[Harmony]
public static class CursorPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(MouseCursorAutoHide), nameof(MouseCursorAutoHide.Update))]
    public static void MouseCursorAutoHide_Update(MouseCursorAutoHide __instance)
    {
        if (__instance._mouse_shown && !Cursor.visible)
        {
            Cursor.visible = true;
        }
    }
}
