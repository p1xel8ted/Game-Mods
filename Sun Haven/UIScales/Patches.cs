namespace UIScales;

[Harmony]
public static class Patches
{

    // [HarmonyPostfix]
    // [HarmonyPatch(typeof(MainMenuController), nameof(MainMenuController.Start))]
    // [HarmonyPatch(typeof(MainMenuController), nameof(MainMenuController.HomeMenu))]
    // public static void MainMenuController_Start(ref MainMenuController __instance)
    // {
    //     Utils.UpdateUiScale(true);
    //     Utils.UpdateZoomLevel();
    //     Utils.UpdateCanvasScaleFactors();
    //
    //     if (__instance.logo)
    //     {
    //         __instance.logo.transform.localScale = __instance.logo.transform.localScale with {x = Shared.Utils.PositiveScaleFactor, y = Shared.Utils.PositiveScaleFactor};
    //     }
    // }

    // [HarmonyPostfix]
    // [HarmonyPatch(typeof(UIHandler), nameof(UIHandler.ShowOvernightUI))]
    // [HarmonyPatch(typeof(UIHandler), nameof(UIHandler.ShowSleepUI))]
    // public static void UIHandler_ShowOvernightUI(ref UIHandler __instance)
    // {
    //     var overnight = GameObject.Find("Player(Clone)/UI/OvernightUI/Overnight");
    //     var eod = GameObject.Find("Player(Clone)/UI/OvernightUI/EndOfDayDisplay/Background Image");
    //
    //     if (Plugin.CorrectEndOfDayScreen.Value)
    //     {
    //         var sf = Shared.Utils.PositiveScaleFactor;
    //         if (overnight)
    //         {
    //             overnight.transform.localScale = overnight.transform.localScale with {x = sf, y = sf};
    //         }
    //         if (eod)
    //         {
    //             eod.transform.localScale = eod.transform.localScale with {x = sf, y = sf};
    //         }
    //     }
    //     else
    //     {
    //         if (overnight)
    //         {
    //             overnight.transform.localScale = overnight.transform.localScale with {x = 1, y = 1};
    //         }
    //         if (eod)
    //         {
    //             eod.transform.localScale = eod.transform.localScale with {x = 1, y = 1};
    //         }
    //     }
    // }

    // [HarmonyPostfix]
    // [HarmonyPatch(typeof(LoadCharacterMenu), nameof(LoadCharacterMenu.SetupSavePanels))]
    // public static void LoadCharacterMenu_SetupSavePanels(ref LoadCharacterMenu __instance)
    // {
    //     var backupButton = GameObject.Find("Canvas/[LoadCharacterMenu]/SwitchPanelButton").transform;
    //     var loadMenu = GameObject.Find("Canvas/[LoadCharacterMenu]/CurrentSaves").transform;
    //     if (!backupButton || !loadMenu)
    //     {
    //         return;
    //     }
    //
    //     backupButton.SetParent(loadMenu);
    //     var rectTransform = backupButton.GetComponent<RectTransform>();
    //     rectTransform.anchoredPosition = new Vector2(150f, -155f);
    //     rectTransform.anchoredPosition3D = new Vector3(150f, -155f, 1);
    //     rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
    //     rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
    // }

    private readonly static string[] SkipCanvasScalers = ["sinai", "UI_Loading", "UI_SplashScreen"];

    [HarmonyPostfix]
    [HarmonyPatch(typeof(CanvasScaler), nameof(CanvasScaler.OnEnable))]
    public static void CanvasScaler_OnEnable(ref CanvasScaler __instance)
    {
        if(__instance.gameObject.GetGameObjectPath().Equals("WorldInteraction(Clone)/Canvas")) return;
        UpdateScaler(__instance);
        if (Plugin.Debug.Value)
        {
            Plugin.LOG.LogInfo($"Updated CanvasScaler: {__instance.name} - {__instance.gameObject.GetGameObjectPath()}");
        }
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(AdaptiveUIScale), nameof(AdaptiveUIScale.Awake))]
    [HarmonyPatch(typeof(UIScaler), nameof(UIScaler.Awake))]
    [HarmonyPatch(typeof(CanvasScalerAdjustment), nameof(CanvasScalerAdjustment.Start))]
    public static void AdaptiveUIScale_Awake(ref MonoBehaviour __instance)
    {
        if (Plugin.Debug.Value)
        {
            Plugin.LOG.LogInfo($"Disabled {__instance.name} - {__instance.GetScriptClassName()}");   
        }
        __instance.enabled = false;
    }
    
    private static void UpdateScaler(CanvasScaler scaler)
    {
        if (scaler.name.Contains("sinai", StringComparison.OrdinalIgnoreCase)) return;
        if (SkipCanvasScalers.Contains(scaler.name)) return;

        scaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
        scaler.scaleFactor = Plugin.MainHudScale.Value;
        
        scaler.scaleFactor = scaler.name switch
        {
            "UI" when scaler.gameObject.GetGameObjectPath().Equals("Player(Clone)/UI") => Plugin.GenericDialogScale.Value,
            "UI" when scaler.gameObject.GetGameObjectPath().Equals("SharedManager/UI") => Plugin.TooltipScale.Value,
            "UI_VirtualKeyboard" => Plugin.VirtualKeyboardScale.Value,
            "UI_Toolbar" => Plugin.ToolbarScale.Value,
            "UI_Inventory" => Plugin.BackpackSettingsScale.Value,
            "UI_Quests" => Plugin.QuestsScale.Value,
            "UI_ItemIcons" => Plugin.ItemIconsScale.Value,
            "Quantum Console" => Plugin.ChatConsoleScale.Value,
            "ShopCanvas" => Plugin.ShopCanvasScale.Value,
            "QuestIconCanvas" => Plugin.QuestIconScale.Value,
            "ChestCanvas" => Plugin.ChestCanvasScale.Value,
            "SnaccoonCanvas" => Plugin.SnaccoonCanvasScale.Value,
            "DoubloonShopCanvas" => Plugin.DoubloonShopCanvasScale.Value,
            "CommunityTokenShopCanvas" => Plugin.CommunityTokenShopCanvasScale.Value,
            _ => Plugin.MainHudScale.Value
        };
    }

    internal static void UpdateAllScalers()
    {
        var canvasScalers = Resources.FindObjectsOfTypeAll<CanvasScaler>();
        foreach (var scaler in canvasScalers)
        {
            UpdateScaler(scaler);
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Player), nameof(Player.SetZoom))]
    public static void Player_SetZoom_Prefix(ref float zoomLevel, ref bool immediate)
    {
        zoomLevel = Plugin.ZoomLevel.Value;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Player), nameof(Player.ResetPlayerCamera), [])]
    public static void Player_ResetPlayerCamera()
    {
        Player.Instance.OverrideCameraZoomLevel = false;
        Player.Instance.SetZoom(Plugin.ZoomLevel.Value, true);
    }

    // [HarmonyPostfix]
    // [HarmonyPatch(typeof(DialogueController), nameof(DialogueController.LoadBust))]
    // public static void DialogueController_LoadBust(ref DialogueController __instance)
    // {
    //     Plugin.Bust = __instance._bust.gameObject.transform;
    //     __instance._bust.gameObject.transform.localScale = Vector3.one;
    //     Plugin.OriginalPortraitPosition.Value = Plugin.Bust.localPosition.x;
    //     Plugin.ScaleTransformWithBottomLeftPivot(Plugin.Bust, new Vector3(Plugin.PortraitScale.Value, Plugin.PortraitScale.Value, 1));
    //     Plugin.MovePortrait();
    // }
    //
    // [HarmonyPostfix]
    // [HarmonyPatch(typeof(DialogueController), nameof(DialogueController.LateUpdate))]
    // public static void DialogueController_LateUpdate(ref DialogueController __instance)
    // {
    //     Plugin.Bust = __instance._bust.gameObject.transform;
    //     if (__instance._usingBust)
    //     {
    //         __instance._bust.gameObject.transform.localPosition = __instance._bust.gameObject.transform.localPosition with {x = Plugin.OriginalPortraitPosition.Value + Plugin.PortraitHorizontalPosition.Value};
    //     }
    // }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Player), nameof(Player.InitializeAsOwner))]
    public static void Player_Awake(ref Player __instance)
    {
        Player.Instance.OverrideCameraZoomLevel = false;
        Player.Instance.SetZoom(Plugin.ZoomLevel.Value, true);
    }

    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(PlayerSettings), nameof(PlayerSettings.OnEnable))]
    public static void PlayerSettings_Start(ref PlayerSettings __instance)
    {
        __instance.uiScaleSlider.transform.parent.transform.parent.gameObject.SetActive(false);
        __instance.zoomSlider.transform.parent.transform.parent.gameObject.SetActive(false);
    }
}