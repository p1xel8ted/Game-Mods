namespace ShowMeMoar;

[Harmony]
public static class Patches
{
    private static Vector3? _subPosition;
    internal static UIPanel HUD { get; private set; }
    internal static Transform ScreenSize { get; set; }
    private static bool IntroPlaying { get; set; }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameSave), nameof(GameSave.GlobalEventsCheck))]
    public static void GameSave_GlobalEventsCheck()
    {
        Plugin.OnGameStartedPlaying();
    }

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
    [HarmonyPatch(typeof(MainGame), nameof(MainGame.OnGameLoaded))]
    [HarmonyPatch(typeof(MainGame), nameof(MainGame.OnGameStartedPlaying))]
    [HarmonyPatch(typeof(MainGame), nameof(MainGame.Awake))]
    public static void MainGame_OnGameLoaded(MainGame __instance)
    {
        Plugin.UpdateCC();
        
        if (!Plugin.Ultrawide.Value) return;
        if (!__instance) return;
        var wind = __instance.transform.Find("Wind");
        if (wind)
        {
            wind.localScale = new Vector3(Plugin.ScaleFactor, Plugin.ScaleFactor, 1);
        }
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(UIPanel), nameof(UIPanel.Awake))]
    [HarmonyPatch(typeof(UIPanel), nameof(UIPanel.OnEnable))]
    [HarmonyPatch(typeof(UIPanel), nameof(UIPanel.OnStart))]
    [HarmonyPatch(typeof(UIPanel), nameof(UIPanel.OnInit))]
    public static void UIPanel_Patches(UIPanel __instance)
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

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Intro), nameof(Intro.Awake))]
    [HarmonyPatch(typeof(Intro), nameof(Intro.ShowIntro))]
    [HarmonyPatch(typeof(Intro), nameof(Intro.ShowSubtitleText))]
    public static void Intro_Awake(Intro __instance)
    {
        if (!Plugin.Ultrawide.Value) return;

        if (!__instance) return;

        _subPosition ??= __instance.subtitle_text.transform.localPosition -= new Vector3(0f, -12f, 0f);

        __instance.subtitle_text.transform.localPosition = _subPosition.Value;

        if (__instance.gameObject.GetComponent<AnimatorScaler>() == null)
        {
            __instance.gameObject.AddComponent<AnimatorScaler>();
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(CustomFlowScript), nameof(CustomFlowScript.Create), typeof(GameObject), typeof(FlowGraph), typeof(bool), typeof(CustomFlowScript.OnFinishedDelegate), typeof(string))]
    public static void CustomFlowScript_Create(ref FlowGraph g)
    {
        IntroPlaying = string.Equals(g.name, "red_eye_talk_1");
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
        if (IntroPlaying) return true;
        return !Plugin.RemoveFog.Value;
    }

    // Fog grid is 6 columns wide with wrapping at tile_pos.x > 5.0 (= 2880 screen pixels).
    // Screens wider than ~2880px see fog tiles wrapping prematurely, leaving gaps.
    // Fix: add extra columns and widen the wrapping boundary.

    private static int _fogColumns = 6;

    public static double GetFogColumns() => _fogColumns;

    [HarmonyPostfix]
    [HarmonyPatch(typeof(FogObject), nameof(FogObject.InitFog))]
    public static void FogObject_InitFog_Postfix(FogObject prefab)
    {
        if (Fog.me == null) return;

        _fogColumns = Math.Max(6, (int) Math.Ceiling(Display.main.systemWidth / 576f) + 2);
        if (_fogColumns <= 6) return;

        var wrapVector = new Vector3(_fogColumns * 6f, 0f, 0f);

        // Create extra fog columns beyond the original 6
        for (var col = 6; col < _fogColumns; col++)
        {
            for (var row = 0; row < 63; row++)
            {
                var fo = UnityEngine.Object.Instantiate(prefab);
                fo.transform.SetParent(FogObject._fog_parent, false);
                fo.round_and_sort = fo.GetComponent<RoundAndSortComponent>();
                fo.round_and_sort_set = fo.round_and_sort != null;
                Fog.me.OnNewFogObjectCreated(fo);
                fo.transform.localPosition = new Vector3(6f * col, 0.3f * row);
                fo.TILES_X_VECTOR = wrapVector;
            }
        }

        // Update TILES_X_VECTOR on the original fog objects too
        foreach (var fo in Fog.me._objs)
        {
            fo.TILES_X_VECTOR = wrapVector;
        }

        Plugin.Log.LogInfo($"Fog grid expanded from 6 to {_fogColumns} columns for {Display.main.systemWidth}px wide display.");
    }

    [HarmonyTranspiler]
    [HarmonyPatch(typeof(FogObject), nameof(FogObject.Update))]
    public static IEnumerable<CodeInstruction> FogObject_Update_Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var codes = instructions.ToList();
        var getColumns = AccessTools.Method(typeof(Patches), nameof(GetFogColumns));
        var replaced = false;

        for (var i = 0; i < codes.Count; i++)
        {
            // Replace ldc.r8 6.0 (the wrapping boundary for X) with call GetFogColumns()
            if (!replaced && codes[i].opcode == OpCodes.Ldc_R8 && codes[i].operand is double d && Math.Abs(d - 6.0) < 0.001)
            {
                codes[i].opcode = OpCodes.Call;
                codes[i].operand = getColumns;
                replaced = true;
                Plugin.Log.LogInfo("Fog wrapping boundary patched for ultrawide.");
            }
        }

        if (!replaced)
        {
            Plugin.Log.LogWarning("Failed to find fog wrapping boundary instruction (ldc.r8 6.0) in FogObject.Update.");
        }

        return codes;
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
}