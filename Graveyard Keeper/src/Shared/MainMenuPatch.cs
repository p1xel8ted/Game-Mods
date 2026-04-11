using HarmonyLib;
using MonoMod.Utils;
using UnityEngine;

namespace Shared;

internal class VersionLabelPositioner : MonoBehaviour
{
    private void LateUpdate()
    {
        transform.localPosition = new Vector3(0f, -355f, 0f);
    }
}

[Harmony]
internal static class MainMenuPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(MainMenuGUI), nameof(MainMenuGUI.Open), typeof(bool))]
    public static void MainMenuGUI_Open_Postfix(MainMenuGUI __instance)
    {
        if (!__instance) return;

        var versionLabel = __instance.version_txt;
        if (!versionLabel)
        {
            return;
        }
        if(versionLabel.text.Contains("Modded"))
        {
            return;
        }

        versionLabel.text = $"[F7B000]BepInEx Modded[-]\nver. {LazyConsts.VERSION:0.000#} ({PlatformHelper.Current})".Replace(",", ".");
        versionLabel.overflowMethod = UILabel.Overflow.ResizeFreely;
        versionLabel.multiLine = true;
        versionLabel.MakePixelPerfect();

        if (!versionLabel.GetComponent<VersionLabelPositioner>())
        {
            versionLabel.gameObject.AddComponent<VersionLabelPositioner>();
        }
    }
}
