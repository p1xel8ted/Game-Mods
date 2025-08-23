namespace CultOfQoL.Patches.Systems;

[HarmonyPatch]
public static class Save
{
    
    static Save()
    {
        Application.quitting += GameSpeedManipulationPatches.ResetTime;
        SaveAndLoad.OnLoadComplete += GameSpeedManipulationPatches.ResetTime;
    }
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(SaveAndLoad), nameof(SaveAndLoad.Save))]
    public static void SaveAndLoad_Save()
    {
        GameSpeedManipulationPatches.ResetTime();
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(LoadMenu), nameof(LoadMenu.Start))]
    [HarmonyPatch(typeof(LoadMenu), nameof(LoadMenu.OnShowStarted))]
    public static void LoadMenu_Show(ref LoadMenu __instance)
    {
        if(!Plugin.HideNewGameButtons.Value) return;
        
        var count = __instance._saveSlots.Count(a => a.Occupied);
        if (count == 0) return;

        foreach (var slot in __instance._saveSlots.Where(a => !a.Occupied))
        {
            slot.gameObject.SetActive(false);
        }

    }
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(UIPauseMenuController), nameof(UIPauseMenuController.OnMainMenuButtonPressed))]
    private static bool UIPauseMenuController_OnMainMenuButtonPressed(ref UIPauseMenuController __instance)
    {
        if (!Plugin.SaveOnQuitToMenu.Value) return true;
        
        var saveAndQuit = __instance.Push(MonoSingleton<UIManager>.Instance.ConfirmationWindowTemplate);
        saveAndQuit.Configure("Save & Quit", "Are you sure you want to quit to menu? Your progress will be saved.");
        var instance = __instance;
        saveAndQuit.OnConfirm += delegate
        {
            GameSpeedManipulationPatches.ResetTime();
            SaveAndLoad.Save();
            instance.LoadMainMenu();
        };
        return false;
    }
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(UIPauseMenuController), nameof(UIPauseMenuController.OnQuitButtonPressed))]
    private static bool UIPauseMenuController_OnQuitButtonPressed(ref UIPauseMenuController __instance)
    {
        if (!Plugin.SaveOnQuitToDesktop.Value) return true;
     
        var saveAndQuit = __instance.Push(MonoSingleton<UIManager>.Instance.ConfirmationWindowTemplate);
        saveAndQuit.Configure("Save & Quit", "Are you sure you want to quit to desktop? Your progress will be saved.");
        saveAndQuit.OnConfirm += delegate
        {
            GameSpeedManipulationPatches.ResetTime();
            SaveAndLoad.Save();
            Application.Quit();
        };
        return false;
    }
}