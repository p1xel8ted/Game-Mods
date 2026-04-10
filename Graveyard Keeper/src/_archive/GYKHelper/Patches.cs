using MonoMod.Utils;

namespace GYKHelper;

[HarmonyPatch]
public static class Patches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(MainMenuGUI), nameof(MainMenuGUI.Open))]
    public static void MainMenuGUI_Open_Postfix(MainMenuGUI __instance)
    {
        if (!__instance) return;

        //disables opaque border around the menu buttons
        var menuButtons = __instance.transform.Find("dark back (1)");
        if (menuButtons) menuButtons.gameObject.SetActive(false);

        //disables ads
        var pc1 = __instance.transform.Find("PC2PreorderBanner");
        if (pc1) pc1.gameObject.SetActive(false);

        var pc2 = __instance.transform.Find("PC2AvailableBanner");
        if (pc2) pc2.gameObject.SetActive(false);

        //updates version text
        var version = __instance.transform.Find("ver txt");
        if (version)
        {
            var versionLabel = version.GetComponent<UILabel>();
            versionLabel.text =
                $"[F7B000] BepInEx Modded[-] [F7B000]GYKHelper v{Assembly.GetExecutingAssembly().GetName().Version.Major}.{Assembly.GetExecutingAssembly().GetName().Version.Minor}.{Assembly.GetExecutingAssembly().GetName().Version.Build}[-]";
            versionLabel.text += $"\n({LazyConsts.VERSION}-{PlatformHelper.Current})";
            versionLabel.overflowMethod = UILabel.Overflow.ResizeFreely;
#pragma warning disable CS0618 // Type or member is obsolete
            versionLabel.lineHeight = 32;
#pragma warning restore CS0618 // Type or member is obsolete
            versionLabel.multiLine = true;
            versionLabel.MakePixelPerfect();
        }
        version.localPosition = new Vector3(0f, -355f, 0f);
    }
}
