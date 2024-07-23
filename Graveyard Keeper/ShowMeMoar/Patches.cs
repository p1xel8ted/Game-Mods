using FlowCanvas;

namespace ShowMeMoar;

[Harmony]
public static class Patches
{
    internal static UIPanel HUD { get; private set; }
    internal static Transform ScreenSize { get; set; }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(TitleScreen), nameof(TitleScreen.Awake))]
    [HarmonyPatch(typeof(TitleScreen), nameof(TitleScreen.Show))]
    public static void TitleScreen_Awake(TitleScreen __instance)
    {
        if (!Plugin.Ultrawide.Value) return;
        if (!__instance) return;
        var gfx = __instance.transform.Find("GFX");
        gfx.localScale = new Vector3(Plugin.ScaleFactor, Plugin.ScaleFactor, 1);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(UIPanel), nameof(UIPanel.Awake))]
    [HarmonyPatch(typeof(UIPanel), nameof(UIPanel.OnEnable))]
    [HarmonyPatch(typeof(UIPanel), nameof(UIPanel.OnStart))]
    [HarmonyPatch(typeof(UIPanel), nameof(UIPanel.OnInit))]
    public static void UIPanel_Patches(ref UIPanel __instance)
    {
        if (!MainGame.game_started) return;
        if (__instance.name.Equals("HUD"))
        {
            HUD = __instance;
            __instance.transform.localScale = new Vector3(Plugin.HudScale.Value, Plugin.HudScale.Value, 1);
        }

        if (__instance.name.StartsWith("Screen size"))
        {
            // Plugin.Log.LogWarning($"Screen size: {__instance.name}");
            var transform = __instance.transform;
            ScreenSize = transform;
            transform.localScale = new Vector3(Plugin.HorizontalHudPosition.Value, Plugin.VerticalHudPosition.Value, 1);
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(ResolutionConfig), nameof(ResolutionConfig.GetResolutionConfigOrNull))]
    public static void ResolutionConfig_GetResolutionConfigOrNull(int width, int height, ref ResolutionConfig __result)
    {
        if (!Plugin.Ultrawide.Value) return;
        var res = new ResolutionConfig(width, height);
        if (height < 900 || width < 1280)
        {
            res.large_gui_scale = height / 900f;
        }
        __result = res;
    }

    // [HarmonyPostfix]
    // [HarmonyPatch(typeof(TitleScreenCamera), nameof(TitleScreenCamera.Update))]
    // public static void TitleScreenCamera_Update(TitleScreenCamera __instance)
    // {
    //     if (__instance.name.Equals("Camera 2 (black frame)"))
    //     {
    //         Plugin.Log.LogWarning($"Camera 2: {__instance.name}");
    //         __instance.gameObject.SetActive(false);
    //     }
    // }

    private readonly static WriteOnce<Vector3> SubPosition = new();

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Intro), nameof(Intro.Awake))]
    [HarmonyPatch(typeof(Intro), nameof(Intro.ShowIntro))]
    [HarmonyPatch(typeof(Intro), nameof(Intro.ShowSubtitleText))]
    public static void Intro_Awake(Intro __instance)
    {
        if (!Plugin.Ultrawide.Value) return;

        if (!__instance) return;

        SubPosition.Value = __instance.subtitle_text.transform.localPosition -= new Vector3(0f, -12f, 0f);

        __instance.subtitle_text.transform.localPosition = SubPosition.Value;

        __instance.gameObject.TryAddComponent<AnimatorScaler>();
    }

    private class AnimatorScaler : MonoBehaviour
    {
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            _animator.transform.localScale = new Vector3(0.75f + Plugin.ScaleFactor, 0.75f + Plugin.ScaleFactor, 1);
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(CustomFlowScript), nameof(CustomFlowScript.Create), typeof(GameObject), typeof(FlowGraph), typeof(bool), typeof(CustomFlowScript.OnFinishedDelegate), typeof(string))]
    public static void CustomFlowScript_Create(ref FlowGraph g)
    {
        IntroPlaying = string.Equals(g.name, "red_eye_talk_1");
    }

    private static bool IntroPlaying { get; set; }
    [HarmonyPostfix]
    [HarmonyPatch(typeof(CameraBounds), nameof(CameraBounds.Awake))]
    [HarmonyPatch(typeof(CameraBounds), nameof(CameraBounds.Update))]
    public static void CameraBounds_Awake(CameraBounds __instance)
    {
        var bounds = __instance.GetComponent<BoxCollider2D>();
        if (bounds == null) return;
        Plugin.Log.LogWarning($"CameraBounds: {bounds.size}");
    }

    private static CameraFilterPack_Atmosphere_Fog FogFilter { get; set; }

    // [HarmonyPostfix]
    // [HarmonyPatch(typeof(MainGame), nameof(MainGame.Update))]
    // public static void MainGame_OnGameLoaded()
    // {
    //     FogFilter ??= MainGame.me.gameObject.TryAddComponent<CameraFilterPack_Atmosphere_Fog>();
    //     FogFilter._Far = 2.5f;
    //     FogFilter.FogColor = Color.white;
    //     FogFilter.FogColor.a = 0.5f;
    // }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(SmartWeatherState), nameof(SmartWeatherState.Update))]
    public static void SmartWeatherState_Update(SmartWeatherState __instance)
    {
        if (!MainGame.game_started || __instance == null || IntroPlaying) return;
        FogFilter ??= MainGame.me.gameObject.TryAddComponent<CameraFilterPack_Atmosphere_Fog>();
        if (__instance.type == SmartWeatherState.WeatherType.Fog && __instance._enabled)
        {
            FogFilter.enabled = true;
            FogFilter._Far = 2.5f;
            FogFilter.FogColor = Color.white;
            FogFilter.FogColor.a = 0.5f;
        }
        else
        {
            FogFilter.enabled = false;
        }
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(Fog), nameof(Fog.SpawnNewFog))]
    [HarmonyPatch(typeof(Fog), nameof(Fog.ApplyCurrentAmount))]
    [HarmonyPatch(typeof(Fog), nameof(Fog.OnNewFogObjectCreated))]
    [HarmonyPatch(typeof(Fog), nameof(Fog.Update))]
    [HarmonyPatch(typeof(Fog), nameof(Fog.Set))]
    [HarmonyPatch(typeof(FogObject), nameof(FogObject.InitFog))]
    [HarmonyPatch(typeof(FogObject), nameof(FogObject.Update))]
    public static bool Fog_SpawnNewFog()
    {
        return false;
    }

}