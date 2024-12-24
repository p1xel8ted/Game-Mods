namespace Mirthwood;

[Harmony]
public static class Patches
{
    private const float NativeAspect = 16f / 9f; // 1.777777777777778

    private static readonly Dictionary<int, float> OriginalFoVs = new();
    private static WriteOnce<Vector3> OriginalTopLeft { get; } = new();
    private static WriteOnce<Vector3> OriginalTopRight { get; } = new();
    private static WriteOnce<Vector3> OriginalBottomLeft { get; } = new();
    private static WriteOnce<Vector3> OriginalBottomRight { get; } = new();
    private static WriteOnce<Vector2> OriginalMinorTextMoveLocalStart { get; } = new();
    private static WriteOnce<Vector2> OriginalMinorTextMoveLocalEnd { get; } = new();
    private static Transform TopLeft { get; set; }
    private static Transform TopRight { get; set; }
    private static Transform BottomLeft { get; set; }
    private static Transform BottomRight { get; set; }
    private static int NativeWidth => Mathf.RoundToInt(Screen.currentResolution.height * NativeAspect); //1440 * 1.777777777777778 = 2560
    private static int CurrentWidth => Screen.currentResolution.width; // 3440
    private static float Difference => CurrentWidth - NativeWidth; // 3440 - 2560 = 880
    private static float BlackBarSize => Difference / 2f; // 880 / 2 = 440


    [HarmonyPrefix]
    [HarmonyPatch(typeof(VideoPlayer), nameof(VideoPlayer.Prepare))]
    [HarmonyPatch(typeof(VideoPlayer), nameof(VideoPlayer.Play))]
    [HarmonyPatch(typeof(VideoPlayer), nameof(VideoPlayer.StepForward))]
    public static void VideoPlayer_Play(VideoPlayer __instance)
    {
        if (!Plugin.IsUltraWide) return;

        __instance.aspectRatio = VideoAspectRatio.FitVertically;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(CanvasScaler), nameof(CanvasScaler.OnEnable))]
    public static void CanvasScaler_OnEnable(CanvasScaler __instance)
    {
        if (!Plugin.IsUltraWide) return;

        if (__instance.name.ToLower().Contains("sinai")) return;
        if (__instance.uiScaleMode == CanvasScaler.ScaleMode.ScaleWithScreenSize && __instance.screenMatchMode != CanvasScaler.ScreenMatchMode.Expand)
        {
            __instance.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
        }
    }

    internal static void UpdateHUD()
    {
        if (!Plugin.IsUltraWide) return;

        try
        {
            if (Plugin.ExpandHUD.Value)
            {
                TopLeft.position = TopLeft.position with { x = 0 };
                TopRight.position = TopRight.position with { x = Screen.currentResolution.width };
                BottomLeft.position = BottomLeft.position with { x = 0 };
                BottomRight.position = BottomRight.position with { x = Screen.currentResolution.width };


                var startX = OriginalMinorTextMoveLocalStart.Value.x - BlackBarSize;
                var endX = OriginalMinorTextMoveLocalEnd.Value.x - (BlackBarSize - 100);

                HUDMenuController.Instance.minorTextMoveLocalStart = OriginalMinorTextMoveLocalStart.Value with { x = startX };
                HUDMenuController.Instance.minorTextMoveLocalEnd = OriginalMinorTextMoveLocalEnd.Value with { x = endX };
            }
            else
            {
                TopLeft.position = OriginalTopLeft.Value;
                TopRight.position = OriginalTopRight.Value;
                BottomLeft.position = OriginalBottomLeft.Value;
                BottomRight.position = OriginalBottomRight.Value;


                HUDMenuController.Instance.minorTextMoveLocalStart = OriginalMinorTextMoveLocalStart.Value;
                HUDMenuController.Instance.minorTextMoveLocalEnd = OriginalMinorTextMoveLocalEnd.Value;
            }
        }
        catch (Exception)
        {
            //ignored
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(TypewriterCore), nameof(TypewriterCore.ShowTextRoutine))]
    [HarmonyPatch(typeof(TypewriterCore), nameof(TypewriterCore.StartShowingText))]
    public static void TypewriterCore_Start(TypewriterCore __instance)
    {
        __instance.internalSpeed = Plugin.DialogueSpeed.Value;
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(TypewriterCore), nameof(TypewriterCore.GetDeltaTime))]
    public static void TypewriterCore_GetDeltaTime(TypewriterCore __instance)
    {
        __instance.internalSpeed = Plugin.DialogueSpeed.Value;
    }

    internal static void UpdateCameras()
    {
        var constrainedCameras = Resources.FindObjectsOfTypeAll<ConstrainedCamera>();
        foreach (var constrainedCamera in constrainedCameras)
        {
            UpdateCameraProperties(constrainedCamera);
        }
    }

    private static void UpdateCameraProperties(ConstrainedCamera camera)
    {
        if (camera.cam)
        {
            if (camera.cam.name == "UI Camera") return;

            camera.cam.useOcclusionCulling = Plugin.OcclusionCulling.Value;
            camera.cam.nearClipPlane = Plugin.NearClipPlane.Value;
            camera.cam.farClipPlane = Plugin.FarClipPlane.Value;

            var hash = camera.GetHashCode();
            if (!OriginalFoVs.TryGetValue(hash, out var originalFOV))
            {
                OriginalFoVs.Add(hash, camera.fieldOfView);
                originalFOV = camera.fieldOfView;
            }

            var pct = Plugin.FieldOfView.Value / 100f;

            camera.fieldOfView = originalFOV - originalFOV * pct;

            OptimizersManager.Instance.DOTS_CheckScreenAndFOVChange();
            OptimizersManager.DOTS_RefreshCamera();
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(ConstrainedCamera), nameof(ConstrainedCamera.Awake))]
    [HarmonyPatch(typeof(ConstrainedCamera), nameof(ConstrainedCamera.Start))]
    [HarmonyPatch(typeof(ConstrainedCamera), nameof(ConstrainedCamera.SetProjection))]
    public static void ConstrainedCamera_Start(ConstrainedCamera __instance)
    {
        UpdateCameraProperties(__instance);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(HUDMenuController), nameof(HUDMenuController.OnEnable))]
    [HarmonyPatch(typeof(HUDMenuController), nameof(HUDMenuController.Start))]
    public static void HUDMenuController_Start(HUDMenuController __instance)
    {
        if (!Plugin.IsUltraWide) return;

        var bottomRight = __instance.transform.Find("Bottom Right");
        if (bottomRight)
        {
            OriginalBottomRight.Value = bottomRight.position;
            BottomRight = bottomRight;
        }

        var topRight = __instance.transform.Find("Top Right");
        if (topRight)
        {
            OriginalTopRight.Value = topRight.position;
            TopRight = topRight;
        }

        var topLeft = __instance.transform.Find("Top Left");
        if (topLeft)
        {
            OriginalTopLeft.Value = topLeft.position;
            TopLeft = topLeft;
        }

        var bottomLeft = __instance.transform.Find("Bottom Left");
        if (bottomLeft)
        {
            OriginalBottomLeft.Value = bottomLeft.position;
            BottomLeft = bottomLeft;
        }

        OriginalMinorTextMoveLocalStart.Value = __instance.minorTextMoveLocalStart;
        OriginalMinorTextMoveLocalEnd.Value = __instance.minorTextMoveLocalEnd;

        UpdateHUD();
    }
}