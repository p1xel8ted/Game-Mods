namespace CultOfQoL;

public partial class Plugin
{
    private void Update()
    {
        if (DirectLoadSave.Value && DirectLoadSkipKey.Value.IsPressed() || DirectLoadSkipKey.Value.IsUp() || DirectLoadSkipKey.Value.IsDown())
        {
            if (!MenuCleanupPatches.SkipAutoLoad)
            {
                Log.LogWarning($"{DirectLoadSkipKey.Value.MainKey.ToString()} pressed; skipping auto-load.");
            }
            MenuCleanupPatches.SkipAutoLoad = true;
        }
        
        if (EnableQuickSaveShortcut.Value && SaveKeyboardShortcut.Value.IsUp())
        {
            SaveAndLoad.Save();
            NotificationCentre.Instance.PlayGenericNotification("Game Saved!");
        }

        if (DisableAds.Value && UIMainMenuController != null)
        {
            foreach (var comp in UIMainMenuController.ad.GetComponents<Component>())
            {
                comp.gameObject.SetActive(false);
            }
            UIMainMenuController.ad.gameObject.SetActive(false);
        }
    }
}