namespace UIScales;

[HarmonyPatch]
public static class Patches
{
   
    [HarmonyPostfix]
    [HarmonyPatch(typeof(MainMenuController), nameof(MainMenuController.Start))]
    [HarmonyPatch(typeof(MainMenuController), nameof(MainMenuController.HomeMenu))]
    public static void MainMenuController_Start(ref MainMenuController __instance)
    {
        Utils.UpdateUiScale(true);
        Utils.UpdateZoomLevel();
        Utils.UpdateCanvasScaleFactors();

        if (__instance.logo != null)
        {
            __instance.logo.transform.localScale = __instance.logo.transform.localScale with {x = Shared.Utils.PositiveScaleFactor, y = Shared.Utils.PositiveScaleFactor};
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(UIHandler), nameof(UIHandler.ShowOvernightUI))]
    [HarmonyPatch(typeof(UIHandler), nameof(UIHandler.ShowSleepUI))]
    public static void UIHandler_ShowOvernightUI(ref UIHandler __instance)
    {
        var overnight = GameObject.Find("Player(Clone)/UI/OvernightUI/Overnight");
        var eod = GameObject.Find("Player(Clone)/UI/OvernightUI/EndOfDayDisplay/Background Image");

        if (Plugin.CorrectEndOfDayScreen.Value)
        {
            var sf = Shared.Utils.PositiveScaleFactor;
            if (overnight != null)
            {
                overnight.transform.localScale = overnight.transform.localScale with {x = sf, y = sf};
            }
            if (eod != null)
            {
                eod.transform.localScale = eod.transform.localScale with {x = sf, y = sf};
            }
        }
        else
        {
            if (overnight != null)
            {
                overnight.transform.localScale = overnight.transform.localScale with {x = 1, y = 1};
            }
            if (eod != null)
            {
                eod.transform.localScale = eod.transform.localScale with {x = 1, y = 1};
            }
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(LoadCharacterMenu), nameof(LoadCharacterMenu.SetupSavePanels))]
    public static void LoadCharacterMenu_SetupSavePanels(ref LoadCharacterMenu __instance)
    {
        var backupButton = GameObject.Find("Canvas/[LoadCharacterMenu]/SwitchPanelButton").transform;
        var loadMenu = GameObject.Find("Canvas/[LoadCharacterMenu]/CurrentSaves").transform;
        if (backupButton == null || loadMenu == null)
        {
            return;
        }

        backupButton.SetParent(loadMenu);
        var rectTransform = backupButton.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(150f, -155f);
        rectTransform.anchoredPosition3D = new Vector3(150f, -155f, 1);
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(CanvasScaler), nameof(CanvasScaler.OnEnable))]
    public static void CanvasScaler_OnEnable(ref CanvasScaler __instance)
    {
        if (Plugin.UIOneCanvas is not null && Plugin.UITwoCanvas is not null && Plugin.QuantumCanvas is not null && Plugin.MainMenuCanvas is not null)
        {
            return;
        }

        var name = __instance.name;
        var path = __instance.gameObject.GetGameObjectPath();

        switch (name)
        {
            case "UI" when Plugin.UIOneCanvas == null && path.Equals("Manager/UI"):
                Plugin.LOG.LogInfo($"Found top left and right UI! Existing mode and scale: {__instance.uiScaleMode} {__instance.scaleFactor}");
                Plugin.UIOneCanvas = __instance;
                Shared.Utils.ConfigureCanvasScaler(Plugin.UIOneCanvas, CanvasScaler.ScaleMode.ConstantPixelSize, Plugin.InGameUiScale.Value);
                break;
            case "UI" when Plugin.UITwoCanvas == null && path.Equals("Player(Clone)/UI"):
            {
                Plugin.LOG.LogInfo($"Found action bars/quest log etc! Existing mode and scale: {__instance.uiScaleMode} {__instance.scaleFactor}");
                Plugin.UITwoCanvas = __instance;
                Shared.Utils.ConfigureCanvasScaler(Plugin.UITwoCanvas, CanvasScaler.ScaleMode.ConstantPixelSize, Plugin.InGameUiScale.Value);
                break;
            }
            case "Quantum Console" when path.Equals("SharedManager/Quantum Console"):
                Plugin.LOG.LogInfo($"Found cheat console! Existing mode and scale: {__instance.uiScaleMode} {__instance.scaleFactor}");
                Plugin.QuantumCanvas = __instance;
                Shared.Utils.ConfigureCanvasScaler(Plugin.QuantumCanvas, CanvasScaler.ScaleMode.ConstantPixelSize, Plugin.CheatConsoleScale.Value);
                break;
            case "Canvas" when path.Equals("Canvas") && SceneManager.GetActiveScene().name == "MainMenu":
                Plugin.LOG.LogInfo($"Found menu?? Existing mode and scale: {__instance.uiScaleMode} {__instance.scaleFactor}");
                Plugin.MainMenuCanvas = __instance;
                Shared.Utils.ConfigureCanvasScaler(Plugin.MainMenuCanvas, CanvasScaler.ScaleMode.ConstantPixelSize, Plugin.MainMenuUiScale.Value);
                break;
        }
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(Player), nameof(Player.SetZoom))]
    public static void Player_SetZoom_Prefix(ref float zoomLevel, ref bool immediate)
    {
        if (Plugin.Debug.Value)
        {
            Plugin.LOG.LogInfo("PlayerSetZoom running: overriding requested zoom level.");
        }

        zoomLevel = Plugin.ZoomLevel.Value;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Player), nameof(Player.ResetPlayerCamera), [])]
    public static void Player_ResetPlayerCamera()
    {
        Player.Instance.OverrideCameraZoomLevel = false;
        Player.Instance.SetZoom(Plugin.ZoomLevel.Value, true);
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(Player), nameof(Player.InitializeAsOwner))]
    public static void Player_Awake(ref Player __instance)
    {
        Player.Instance.OverrideCameraZoomLevel = false;
        Player.Instance.SetZoom(Plugin.ZoomLevel.Value, true);

        Plugin.PrepareDateTimeYearForCustomScales();
        
        if (Plugin.Debug.Value)
        {
            Plugin.LOG.LogInfo($"Player.InitializeAsOwner: Name:{__instance.name}");
        }
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(PlayerSettings), nameof(PlayerSettings.OnEnable))]
    public static void PlayerSettings_Start(ref PlayerSettings __instance)
    {
        __instance.zoomSlider.transform.parent.gameObject.SetActive(false);
       //GameObject.Find("Player(Clone)/UI/Inventory/Settings/SettingsScroll View_Video/Viewport/Content/Setting_ZoomLevel").SetActive(false);
    }
}