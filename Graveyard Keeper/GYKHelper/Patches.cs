using MonoMod.Utils;

namespace GYKHelper;

[HarmonyPriority(1)]
public static class Patches
{
    [HarmonyPostfix]
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(BaseGUI), nameof(BaseGUI.Hide), typeof(bool))]
    public static void BaseGuiHidePostfix()
    {
        if (BaseGUI.all_guis_closed)
        {
            Tools.SetAllInteractionsFalse();
        }
    }


    [HarmonyPrefix]
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(SaveSlotsMenuGUI), nameof(SaveSlotsMenuGUI.Open))]
    public static void SaveSlotsMenuGUI_Open()
    {
        MainGame.game_started = false;

        if (!Plugin.DisplayDuplicateHarmonyPatches.Value) return;

        var originalMethods = GetAllPatchedMethodsManually();
        var groupedMethods = originalMethods.GroupBy(method => new {method.DeclaringType, method.Name});

        var sortedMethods = groupedMethods.Where(group => group.Count() > 1)
            .SelectMany(group => group)
            .OrderBy(method => method.DeclaringType!.ToString())
            .ThenBy(method => method.Name);

        foreach (var method in sortedMethods)
        {
            Plugin.Log.LogWarning($"Type: {method.DeclaringType}, Method: {method.Name}");
        }
        return;

        static IEnumerable<MethodInfo> GetAllPatchedMethodsManually()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();

                foreach (var type in types)
                {
                    var methods = type.GetMethods(AccessTools.all);
                    foreach (var method in methods)
                    {
                        var harmonyPatchAttributes = method.GetCustomAttributes<HarmonyPatch>(false);
                        foreach (var patchAttribute in harmonyPatchAttributes)
                        {
                            var targetType = patchAttribute.info.declaringType;
                            var targetMethodName = patchAttribute.info.methodName;
                            var targetMethodArguments = patchAttribute.info.argumentTypes;

                            if (targetType != null && targetMethodName != null)
                            {
                                MethodInfo targetMethod = null;
                                try
                                {
                                    targetMethod = targetMethodArguments == null
                                        ? targetType.GetMethod(targetMethodName, AccessTools.all)
                                        : targetType.GetMethod(targetMethodName, AccessTools.all, null,
                                            targetMethodArguments, null);
                                }
                                catch (AmbiguousMatchException)
                                {
                                    Plugin.Log.LogWarning(
                                        $"AmbiguousMatchException for {targetType}.{targetMethodName}");
                                }

                                if (targetMethod != null)
                                {
                                    yield return targetMethod;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    [HarmonyPostfix]
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(GameSettings), nameof(GameSettings.ApplyLanguageChange))]
    [HarmonyPatch(typeof(GameSettings), nameof(GameSettings.Init))]
    public static void GameSettingsApplyLanguageChangePostfix()
    {
        CrossModFields.Lang = GameSettings.me.language.Replace('_', '-').ToLower(CultureInfo.InvariantCulture).Trim();
        CrossModFields.Culture = CultureInfo.GetCultureInfo(CrossModFields.Lang);
        Thread.CurrentThread.CurrentUICulture = CrossModFields.Culture;
    }

    [HarmonyPrefix]
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(SmartAudioEngine), nameof(SmartAudioEngine.OnEndNPCInteraction))]
    public static void OnEndNPCInteractionPrefix()
    {
        CrossModFields.TalkingToNpc(false);
    }

    [HarmonyPrefix]
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(MovementComponent), nameof(MovementComponent.UpdateMovement), typeof(Vector2),
        typeof(float))]
    public static void Prefix(ref MovementComponent __instance)
    {
        if (__instance.wgo.is_player)
        {
            CrossModFields.PlayerIsDead = __instance.wgo.is_dead;
            CrossModFields.PlayerIsControlled = __instance.player_controlled_by_script;
            CrossModFields.PlayerFollowingTarget = __instance.is_following_target;
        }
    }


    [HarmonyPostfix]
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(MainMenuGUI), nameof(MainMenuGUI.Open))]
    public static void MainMenuGUI_Open_Postfix(ref MainMenuGUI __instance)
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

        //updates version text for GYK Helper
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


    [HarmonyPrefix]
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(VendorGUI), nameof(VendorGUI.Open), typeof(WorldGameObject),
        typeof(GJCommons.VoidDelegate))]
    public static void VendorGUI_Open()
    {
        if (!MainGame.game_started) return;
        CrossModFields.TalkingToNpc(true);
    }

    [HarmonyPrefix]
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(VendorGUI), nameof(VendorGUI.Hide), typeof(bool))]
    public static void VendorGUI_Hide()
    {
        if (!MainGame.game_started) return;
        CrossModFields.TalkingToNpc(false);
    }

    [HarmonyPrefix]
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(VendorGUI), nameof(VendorGUI.OnClosePressed))]
    public static void VendorGUI_OnClosePressed()
    {
        if (!MainGame.game_started) return;
        CrossModFields.TalkingToNpc(false);
    }
}