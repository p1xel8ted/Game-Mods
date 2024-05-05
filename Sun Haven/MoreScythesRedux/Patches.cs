namespace MoreScythesRedux;

[Harmony]
public static class Patches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(MainMenuController), nameof(MainMenuController.Awake))]
    public static void MainMenuControllerAwake()
    {
        ItemHandler.CreateScytheItems();
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(UseItem), nameof(UseItem.SetPlayer))]
    public static void UseItemSetPlayer(ref UseItem __instance)
    {
        if (!__instance.gameObject.activeSelf)
        {
            __instance.gameObject.SetActive(true);
        }
    }
}