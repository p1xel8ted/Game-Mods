namespace ShowMeMoar;

[Harmony]
public static class Patches
{
    internal static UIPanel HUD { get; private set; }
    internal static Transform ScreenSize { get; set; }
    
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
            Plugin.Log.LogWarning($"Screen size: {__instance.name}");
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
    
        Plugin.Log.LogInfo($"ResolutionConfig_GetResolutionConfigOrNull: Width: {width}, Height: {height}, Pixel Size: {res.pixel_size}");
        __result = res;
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(FogObject), nameof(FogObject.InitFog))]
    public static bool FogObject_InitFog(FogObject prefab)
    {
        if (!Plugin.Ultrawide.Value) return true;
        if (prefab == null)
        {
            Debug.LogError("Fog object not found on a scene");
            return true;
        }

        FogObject._fog_parent = Fog.SpawnNewFog();
        var chunkedGameObject = prefab.GetComponent<ChunkedGameObject>();
        if (chunkedGameObject == null)
        {
            chunkedGameObject = prefab.gameObject.AddComponent<ChunkedGameObject>();
        }

        chunkedGameObject.always_active = true;

        var screenAspect = Screen.currentResolution.width / (float) Screen.currentResolution.height;
        var fogFieldWidthInObjects = Mathf.CeilToInt(6 * screenAspect / (16f / 9f));

        for (var i = 0; i < fogFieldWidthInObjects; i++)
        {
            for (var j = 0; j < 63; j++)
            {
                var fogObject = Object.Instantiate(prefab, FogObject._fog_parent, false);
                fogObject.round_and_sort = fogObject.GetComponent<RoundAndSortComponent>();
                fogObject.round_and_sort_set = fogObject.round_and_sort != null;
                Fog.me.OnNewFogObjectCreated(fogObject);
                fogObject.transform.localPosition = new Vector3(6f * i, 0.3f * j);
            }
        }

        return false;
    }
}