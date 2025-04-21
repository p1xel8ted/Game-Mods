using System.Globalization;
using TMPro;

namespace UIScales;

[Harmony]
public static class Patches
{
    private static readonly string[] SkipCanvasScalers = ["sinai", "UI_Loading", "UI_SplashScreen"];

    [HarmonyPostfix]
    [HarmonyPatch(typeof(CanvasScaler), nameof(CanvasScaler.OnEnable))]
    public static void CanvasScaler_OnEnable(ref CanvasScaler __instance)
    {
        if (__instance.gameObject.GetGameObjectPath().Equals("WorldInteraction(Clone)/Canvas")) return;
        UpdateScaler(__instance);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(AdaptiveUIScale), nameof(AdaptiveUIScale.Awake))]
    [HarmonyPatch(typeof(UIScaler), nameof(UIScaler.Awake))]
    [HarmonyPatch(typeof(CanvasScalerAdjustment), nameof(CanvasScalerAdjustment.Start))]
    public static void AdaptiveUIScale_Awake(ref MonoBehaviour __instance)
    {
        __instance.enabled = false;
    }

    private static void UpdateCanvasScalerRefScale(CanvasScaler scaler)
    {
        var targetResolution = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
        if (scaler.uiScaleMode != CanvasScaler.ScaleMode.ScaleWithScreenSize) return;

        // Get reference resolution and scale factor
        var referenceResolution = scaler.referenceResolution;

        // Calculate new scale factor
        var scaleFactor = Mathf.Min(targetResolution.x / referenceResolution.x, targetResolution.y / referenceResolution.y);

        // Set to ConstantPixelSize and apply the new scale factor
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
        scaler.scaleFactor = scaleFactor;
    }

    private static void UpdateScaler(CanvasScaler scaler)
    {
        UpdateCanvasScalerRefScale(scaler);

        if (scaler.name.Contains("sinai", StringComparison.OrdinalIgnoreCase)) return;
        if (SkipCanvasScalers.Contains(scaler.name)) return;

        scaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
        scaler.scaleFactor = Plugin.EverythingElseScale.Value;

        scaler.scaleFactor = scaler.name switch
        {
            "UI" when scaler.gameObject.GetGameObjectPath().Equals("Player(Clone)/UI") => Plugin.GenericDialogScale.Value,
            "UI" when scaler.gameObject.GetGameObjectPath().Equals("SharedManager/UI") => Plugin.TooltipScale.Value,
            "UI" when scaler.gameObject.GetGameObjectPath().Equals("Manager/UI") => Plugin.MainHudScale.Value,
            "Canvas" when SceneManager.GetActiveScene().name.Equals("MainMenu") => Plugin.OptionsMenuScale.Value,
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
            _ => Plugin.EverythingElseScale.Value
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

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Player), nameof(Player.InitializeAsOwner))]
    public static void Player_Awake(ref Player __instance)
    {
        Player.Instance.OverrideCameraZoomLevel = false;
        Player.Instance.SetZoom(Plugin.ZoomLevel.Value, true);
    }

    private static Slider SliderInstance { get; set; }
    private static TextMeshProUGUI TextInstance { get; set; }

    internal static void ExtSetZoomSlider(float value)
    {
        if (SliderInstance)
        {
            SliderInstance.Set(value, false);
        }

        if (TextInstance)
        {
            TextInstance.SetText($"{value.ToString(CultureInfo.InvariantCulture)}x");
        }

        if (Player.Instance)
        {
            Player.Instance.OverrideCameraZoomLevel = false;
            Player.Instance.SetZoom(value, true);
        }
    }

    private static void SetupZoomSlider(Slider slider, TextMeshProUGUI text)
    {
        SliderInstance = slider;
        TextInstance = text;

        slider.onValueChanged = new Slider.SliderEvent();
        slider.wholeNumbers = false;
        slider.minValue = 0.5f;
        slider.maxValue = 10f;

        slider.onValueChanged.RemoveAllListeners();
        slider.onValueChanged.AddListener(value =>
        {
            value = Mathf.Round(value * 4) / 4;
            Plugin.ZoomLevel.Value = value;
            slider.Set(value, false);
            text.SetText($"{value.ToString(CultureInfo.InvariantCulture)}x");
            Player.Instance.OverrideCameraZoomLevel = false;
            Player.Instance.SetZoom(value, true);
        });

        slider.value = Plugin.ZoomLevel.Value;
        slider.Set(Plugin.ZoomLevel.Value, false);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(MainMenuController), nameof(MainMenuController.PopupPanel))]
    public static void MainMenuController_PopupPanel(MainMenuController __instance)
    {
        var slider = __instance.settings.zoomSlider;
        var text = __instance.settings.zoomTMP;
        SetupZoomSlider(slider, text);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Slider), nameof(Slider.ClampValue))]
    public static void Slider_ClampValue(Slider __instance, float input, ref float __result)
    {
        if (__instance.name == "ZoomSlider")
        {
            __result = Mathf.Round(input * 4) / 4;
        }
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(PlayerSettings), nameof(PlayerSettings.SetupUI))]
    public static void PlayerSettings_Start(PlayerSettings __instance)
    {
        var slider = __instance.zoomSlider;
        var text = __instance.zoomTMP;
        SetupZoomSlider(slider, text);

        __instance.uiScaleSlider.transform.parent.transform.parent.gameObject.SetActive(false);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(DialogueController), nameof(DialogueController.LoadBust))]
    public static void DialogueController_LoadBust(ref DialogueController __instance)
    {
        Plugin.Bust = __instance._bust.gameObject.transform;
        if (!Plugin.ScalePortraitAdjustments.Value) return;
        __instance._bust.gameObject.transform.localScale = Vector3.one;
        Plugin.OriginalPortraitPosition.Value = Plugin.Bust.localPosition.x;
        Plugin.ScaleTransformWithBottomLeftPivot(Plugin.Bust, new Vector3(Plugin.PortraitScale.Value, Plugin.PortraitScale.Value, 1));
        Plugin.MovePortrait();
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(DialogueController), nameof(DialogueController.LateUpdate))]
    public static void DialogueController_LateUpdate(ref DialogueController __instance)
    {
        Plugin.Bust = __instance._bust.gameObject.transform;
        if (!Plugin.ScalePortraitAdjustments.Value) return;
        if (__instance._usingBust)
        {
            __instance._bust.gameObject.transform.localPosition = __instance._bust.gameObject.transform.localPosition with { x = Plugin.OriginalPortraitPosition.Value + Plugin.PortraitHorizontalPosition.Value };
        }
    }
}