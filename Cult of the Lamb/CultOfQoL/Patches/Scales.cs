namespace CultOfQoL.Patches;

[HarmonyPatch]
public static class Scales
{
    private static float _originalScaleFactor;
    private static float _originalReferenceResolutionX;
    private static float _originalReferenceResolutionY;
    private static CanvasScaler.ScaleMode _originalScaleMode;

    private static float _dOriginalScaleFactor;
    private static float _dOriginalReferenceResolutionX;
    private static float _dOriginalReferenceResolutionY;
    private static CanvasScaler.ScaleMode _dOriginalScaleMode;


    [HarmonyPostfix]
    [HarmonyPatch(typeof(CanvasScaler), nameof(CanvasScaler.OnEnable))]
    public static void CanvasScaler_OnEnable(ref CanvasScaler __instance)
    {
        if (__instance.name.Equals("Canvas") && Plugin.GameCanvasScaler == null)
        {
            _originalScaleFactor = __instance.scaleFactor;
            _originalReferenceResolutionX = __instance.referenceResolution.x;
            _originalReferenceResolutionY = __instance.referenceResolution.y;
            _originalScaleMode = __instance.uiScaleMode;
            Plugin.GameCanvasScaler = __instance;
        }

        if (Helpers.GetGameObjectPath(__instance.gameObject).Equals("Game Prefab/Canvas") && Plugin.DungeonCanvasScaler == null)
        {
            _dOriginalScaleFactor = __instance.scaleFactor;
            _dOriginalReferenceResolutionX = __instance.referenceResolution.x;
            _dOriginalReferenceResolutionY = __instance.referenceResolution.y;
            _dOriginalScaleMode = __instance.uiScaleMode;
            
            Plugin.DungeonCanvasScaler = __instance;
        }
        
        UpdateScale();
    }

    public static void RestoreScale()
    {
        if (Plugin.EnableCustomUiScale.Value) return;
        if (Plugin.GameCanvasScaler != null)
        {
            Plugin.GameCanvasScaler.referenceResolution =
                new Vector2(_originalReferenceResolutionX, _originalReferenceResolutionY);
            Plugin.GameCanvasScaler.uiScaleMode = _originalScaleMode;
            Plugin.GameCanvasScaler.scaleFactor = _originalScaleFactor;
        }

        if (Plugin.DungeonCanvasScaler != null)
        {
            Plugin.DungeonCanvasScaler.referenceResolution =
                new Vector2(_dOriginalReferenceResolutionX, _dOriginalReferenceResolutionY);
            Plugin.DungeonCanvasScaler.uiScaleMode = _dOriginalScaleMode;
            Plugin.DungeonCanvasScaler.scaleFactor = _dOriginalScaleFactor;
        }
    }

    public static void UpdateScale()
    {
        if (!Plugin.EnableCustomUiScale.Value) return;

        var newScale = (float) Plugin.CustomUiScale.Value / 100;
        var newRef = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
        const CanvasScaler.ScaleMode mode = CanvasScaler.ScaleMode.ConstantPixelSize;
        
        if (Plugin.GameCanvasScaler != null)
        {
            Plugin.GameCanvasScaler.referenceResolution = newRef;
            Plugin.GameCanvasScaler.uiScaleMode = mode;
            Plugin.GameCanvasScaler.scaleFactor = newScale;
        }

        if (Plugin.DungeonCanvasScaler != null)
        {
            Plugin.DungeonCanvasScaler.referenceResolution = newRef;
            Plugin.DungeonCanvasScaler.uiScaleMode = mode;
            Plugin.DungeonCanvasScaler.scaleFactor = newScale;
        }
    }
}