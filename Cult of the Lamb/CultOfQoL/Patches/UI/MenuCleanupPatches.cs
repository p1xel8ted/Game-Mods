namespace CultOfQoL.Patches.UI;

[Harmony]
public static class MenuCleanupPatches
{
    private static bool ReturningToMenu { get; set; }
    internal static bool SkipAutoLoad { get; set; }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(MainMenu), nameof(MainMenu.Start))]
    public static void MainMenu_Start(ref MainMenu __instance)
    {
        var buttons = __instance.GetComponentsInChildren<MMButton>();
        foreach (var button in buttons)
        {
            button.Selectable.navigation = button.Selectable.navigation with {mode = Navigation.Mode.Automatic};
        }


        __instance.SetActiveStateForMenu(__instance._creditsButton.gameObject, !Plugin.RemoveMenuClutter.Value);
        __instance.SetActiveStateForMenu(__instance._roadmapButton.gameObject, !Plugin.RemoveMenuClutter.Value);
        __instance._creditsButton.gameObject.SetActive(!Plugin.RemoveMenuClutter.Value);
        __instance._roadmapButton.gameObject.SetActive(!Plugin.RemoveMenuClutter.Value);


        if (Plugin.DirectLoadSave.Value && !ReturningToMenu && !SkipAutoLoad)
        {
            ReturningToMenu = true;
            if (SaveAndLoad.SaveExist(Plugin.SaveSlotToLoad.Value - 1))
            {
                if (Plugin.UIMainMenuController != null)
                {
                    Plugin.UIMainMenuController.LoadMenu.OnTryLoadSaveSlot(Plugin.SaveSlotToLoad.Value - 1);
                }
            }
            else
            {
                Plugin.PopupManager.ShowPopupDlg("The slot you selected doesn't contain a save game, so direct load was aborted.",false);
            }
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(UIPauseMenuController), nameof(UIPauseMenuController.LoadMainMenu))]
    public static void UIPauseMenuController_LoadMainMenu()
    {
        ReturningToMenu = true;
        SkipAutoLoad = false;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(UIMainMenuController), nameof(UIMainMenuController.Awake))]
    public static void UIMainMenuController_Awake(ref UIMainMenuController __instance)
    {
        Plugin.UIMainMenuController = __instance;
        __instance._discordButton.gameObject.SetActive(!Plugin.RemoveMenuClutter.Value);
        var stuff = GameObject.Find("Main Menu Controller/Main Menu/MainMenuContainer/Right/Transform/");
        if (stuff == null) return;
        __instance.SetActiveStateForMenu(stuff.gameObject, !Plugin.RemoveMenuClutter.Value);
        stuff.gameObject.SetActive(!Plugin.RemoveMenuClutter.Value);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(UIPauseMenuController), nameof(UIPauseMenuController.Awake))]
    public static void UIPauseMenuController_Awake(ref UIPauseMenuController __instance)
    {
        __instance.SetActiveStateForMenu(__instance._helpButton.gameObject, !Plugin.RemoveHelpButtonInPauseMenu.Value);
        __instance._helpButton.gameObject.SetActive(!Plugin.RemoveHelpButtonInPauseMenu.Value);


        GameObject gameObject;
        (gameObject = __instance._photoModeButton.gameObject).SetActive(!Plugin.RemovePhotoModeButtonInPauseMenu.Value);
        __instance.SetActiveStateForMenu(gameObject, !Plugin.RemovePhotoModeButtonInPauseMenu.Value);


        GameObject o;
        (o = __instance._twitchSettingsButton.gameObject).SetActive(!Plugin.RemoveTwitchButtonInPauseMenu.Value);
        __instance.SetActiveStateForMenu(o, !Plugin.RemoveTwitchButtonInPauseMenu.Value);
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(MenuAdController), nameof(MenuAdController.Start))]
    public static void MenuAdController_Start(ref MenuAdController __instance)
    {
        var one = GameObject.Find("Main Menu Controller/Main Menu/MainMenuContainer/Right/Ad/Outline");
        var two = GameObject.Find("Main Menu Controller/Main Menu/MainMenuContainer/Right/Ad/Transform");
        one.SetActive(!Plugin.RemoveMenuClutter.Value);
        two.SetActive(!Plugin.RemoveMenuClutter.Value);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(UITwitchExtensionButton), nameof(UITwitchExtensionButton.Awake))]
    public static void UITwitchExtensionButton_Awake(ref UITwitchExtensionButton __instance)
    {
        __instance.gameObject.SetActive(!Plugin.RemoveTwitchButton.Value);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(UITwitchButton), nameof(UITwitchButton.Awake))]
    public static void UITwitchButton_Awake(ref UITwitchButton __instance)
    {
        __instance.gameObject.SetActive(!Plugin.RemoveTwitchButton.Value);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(UIPauseMenuController), nameof(UIPauseMenuController.Start))]
    public static void UIPauseMenuController_Start(ref UIPauseMenuController __instance)
    {
        __instance.SetActiveStateForMenu(__instance._bugReportButton.gameObject, !Plugin.RemoveMenuClutter.Value);
        __instance.SetActiveStateForMenu(__instance._discordButton.gameObject, !Plugin.RemoveMenuClutter.Value);
        __instance._bugReportButton.gameObject.SetActive(!Plugin.RemoveMenuClutter.Value);
        __instance._discordButton.gameObject.SetActive(!Plugin.RemoveMenuClutter.Value);
    }
}