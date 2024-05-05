using System.Diagnostics.CodeAnalysis;
using HarmonyLib;

namespace AnAlchemicalCollection;

[Harmony]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class SavePatches
{
    private static bool ReturningToMainMenu { get; set; }
    public static bool SkipAutoLoad { get; set; }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(MainMenu), nameof(MainMenu.ReturnToMainMenu))]
    public static void MainMenu_ReturnToMainMenu()
    {
        ReturningToMainMenu = true;
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(MainMenu), nameof(MainMenu.SetMainMenuLayout))]
    public static void MainMenu_SetMainMenuLayout(ref MainMenu __instance)
    {
        if (ReturningToMainMenu) return;

        if (Plugin.AutoLoadSave.Value && !SkipAutoLoad)
        {
            var slot = Plugin.AutoLoadSaveSlot.Value - 1;
            var saveObject = SaveSystemManager.LOAD(slot);
            if (saveObject.Result != null)
            {
                __instance.selectedSaveObject = saveObject.Result;
                __instance.selectedSlotIndex = 0;
                __instance.LoadGameScene();
            }
        }

        ReturningToMainMenu = false;
    }
    
}