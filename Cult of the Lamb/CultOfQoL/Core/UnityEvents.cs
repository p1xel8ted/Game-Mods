using CultOfQoL.Patches.UI;

namespace CultOfQoL.Core;

public partial class Plugin
{
    private void Update()
    {
        if (CultOfQoL.Plugin.DirectLoadSave.Value && CultOfQoL.Plugin.DirectLoadSkipKey.Value.IsPressed() || CultOfQoL.Plugin.DirectLoadSkipKey.Value.IsUp() || CultOfQoL.Plugin.DirectLoadSkipKey.Value.IsDown())
        {
            if (!MenuCleanupPatches.SkipAutoLoad)
            {
                CultOfQoL.Plugin.Log.LogWarning($"{CultOfQoL.Plugin.DirectLoadSkipKey.Value.MainKey.ToString()} pressed; skipping auto-load.");
            }
            MenuCleanupPatches.SkipAutoLoad = true;
        }
        
        if (CultOfQoL.Plugin.EnableQuickSaveShortcut.Value && CultOfQoL.Plugin.SaveKeyboardShortcut.Value.IsUp())
        {
            SaveAndLoad.Save();
            NotificationCentre.Instance.PlayGenericNotification("Game Saved!");
        }

        if (CultOfQoL.Plugin.DisableAds.Value && CultOfQoL.Plugin.UIMainMenuController)
        {
            foreach (var comp in CultOfQoL.Plugin.UIMainMenuController.ad.GetComponents<Component>())
            {
                comp.gameObject.SetActive(false);
            }
            CultOfQoL.Plugin.UIMainMenuController.ad.gameObject.SetActive(false);
        }
    }
}