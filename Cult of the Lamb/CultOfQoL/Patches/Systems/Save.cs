namespace CultOfQoL.Patches.Systems;

[Harmony]
public static class Save
{
    private const string SaveAndQuitTitle = "Save & Quit";
    private const string QuitToMenuMessage = "Are you sure you want to quit to menu? Your progress will be saved.";
    private const string QuitToDesktopMessage = "Are you sure you want to quit to desktop? Your progress will be saved.";
    
    static Save()
    {
        Application.quitting += GameSpeedManipulationPatches.ResetTime;
        SaveAndLoad.OnLoadComplete += GameSpeedManipulationPatches.ResetTime;
    }
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(SaveAndLoad), nameof(SaveAndLoad.Save), [])]
    public static void SaveAndLoad_Save()
    {
        GameSpeedManipulationPatches.ResetTime();
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(LoadMenu), nameof(LoadMenu.Start))]
    [HarmonyPatch(typeof(LoadMenu), nameof(LoadMenu.OnShowStarted))]
    public static void LoadMenu_Show(ref LoadMenu __instance)
    {
        if (!Plugin.HideNewGameButtons.Value) return;
        
        var hasAnySaves = __instance._saveSlots.Any(slot => slot.Occupied);
        if (!hasAnySaves) return;

        // Hide all empty save slots when there's at least one save
        foreach (var emptySlot in __instance._saveSlots.Where(slot => !slot.Occupied))
        {
            emptySlot.gameObject.SetActive(false);
        }
    }
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(UIPauseMenuController), nameof(UIPauseMenuController.OnMainMenuButtonPressed))]
    private static bool UIPauseMenuController_OnMainMenuButtonPressed(ref UIPauseMenuController __instance)
    {
        if (!Plugin.SaveOnQuitToMenu.Value) return true;
        
        var instance = __instance;
        ShowSaveAndQuitDialog(__instance, QuitToMenuMessage, () =>
        {
            PerformSaveAndResetTime();
            instance.LoadMainMenu();
        });
        
        return false;
    }
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(UIPauseMenuController), nameof(UIPauseMenuController.OnQuitButtonPressed))]
    private static bool UIPauseMenuController_OnQuitButtonPressed(ref UIPauseMenuController __instance)
    {
        if (!Plugin.SaveOnQuitToDesktop.Value) return true;
        
        ShowSaveAndQuitDialog(__instance, QuitToDesktopMessage, () =>
        {
            PerformSaveAndResetTime();
            Application.Quit();
        });
        
        return false;
    }
    
    private static void ShowSaveAndQuitDialog(UIPauseMenuController controller, string message, Action onConfirm)
    {
        var confirmationWindow = controller.Push(MonoSingleton<UIManager>.Instance.ConfirmationWindowTemplate);
        confirmationWindow.Configure(SaveAndQuitTitle, message);
        confirmationWindow.OnConfirm += onConfirm;
    }
    
    private static void PerformSaveAndResetTime()
    {
        GameSpeedManipulationPatches.ResetTime();
        SaveAndLoad.Save();
    }
}