using System.IO;
using System.Text;

namespace CuisineerTweaks;

public static class Utils
{
    public static void WriteLog(string message, bool ignoreDebug = false)
    {
        if (Plugin.DebugMode.Value || ignoreDebug)
        {
            Plugin.Logger.LogInfo(message);
        }
    }

    public static float FindLowestFrameRateMultipleAboveFifty(float originalRate)
    {
        for (var rate = originalRate / 2; rate > 50; rate--)
        {
            if (originalRate % rate == 0)
            {
                return rate;
            }
        }

        return originalRate;
    }

    public static List<T> FindIl2CppType<T>() where T : UnityEngine.Object
    {
        return Resources.FindObjectsOfTypeAll(Il2CppType.Of<T>())
            .Select(obj => obj.TryCast<T>())
            .Where(o => o != null)
            .ToList();
    }

    internal static void FastForwardBrewCraft(UI_BrewArea.StateData stateData)
    {
        var currDate = GameInstances.CalendarManagerInstance.CurrDate;
        stateData.m_BrewDate = currDate - 2;
    }


    internal static void ShowScreenMessage(string message, int displayFor = 3)
    {
        if (!Plugin.DisplayMessages.Value) return;
        var tpm = GameInstances.TextPopupManagerInstance;
        if (tpm == null)
        {
            WriteLog("TextPopupManager is null!");
            return;
        }

        tpm.m_VisibleDuration = displayFor;
        tpm.ShowText(message);
    }


    internal static void UpdateResolutionData(UI_GameplayOptions __instance, bool changeRes = false)
    {
        if (__instance == null)
        {
            return;
        }

        GameInstances.GameplayOptionsInstance = __instance;
        var resData = UI_GameplayOptions.ResolutionDatas[__instance.m_ResolutionSelection.DropDown.value];
        Fixes.ResolutionWidth = resData.m_Width;
        Fixes.ResolutionHeight = resData.m_Height;
        var fsData = UI_GameplayOptions.FullscreenDatas[__instance.m_FullscreenSelection.DropDown.value];
        Fixes.FullScreenMode = fsData.m_FullScreenMode;
        Fixes.MaxRefreshRate = UI_GameplayOptions.FramerateDatas[__instance.m_FramerateSelection.DropDown.value].m_FPS;


        WriteLog($"Chosen Display Settings: {Fixes.ResolutionWidth}x{Fixes.ResolutionHeight}@{Fixes.MaxRefreshRate}Hz in {Fixes.FullScreenMode} mode");

        Fixes.UpdateFixedDeltaTime();
        if (!changeRes) return;
        Fixes.UpdateResolutionFrameRate();
    }

    internal static void ScaleElement(string path, bool maskCheck, float scaleFactor = 1f)
    {
        var element = GameObject.Find(path);
        if (element == null) return;
        if (maskCheck)
        {
            var maskComponent = element.GetComponent<Mask>();
            if (maskComponent != null)
            {
                maskComponent.enabled = false;
            }
        }

        element.transform.localScale = element.transform.localScale with { x = scaleFactor };
    }

    private static string BreadcrumbPath => Path.GetFullPath(Path.Combine(Application.dataPath, "..", "CuisineerTweaks_Breadcrumbs.log"));

    internal static void Breadcrumb(string message)
    {
        try
        {
            File.AppendAllText(BreadcrumbPath,
                $"[{System.DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] {message}\n");
        }
        catch
        {
            // ignored
        }
    }

    internal static void LogCrash(string patchName, System.Exception ex)
    {
        Plugin.Logger.LogError($"[{patchName}] {ex}");
        try
        {
            var crashPath = Path.Combine(Application.dataPath, "..", "CuisineerTweaks_Crash.log");
            File.AppendAllText(Path.GetFullPath(crashPath),
                $"[{System.DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{patchName}] {ex}\n\n");
        }
        catch
        {
            // If we can't write to disk, at least the LogError above may survive
        }
    }

    internal static Dictionary<string, string> SnapshotConfig(ConfigFile config)
    {
        var snapshot = new Dictionary<string, string>();
        foreach (var entry in config)
        {
            var key = $"{entry.Key.Section}/{entry.Key.Key}";
            snapshot[key] = entry.Value.BoxedValue?.ToString() ?? "null";
        }
        return snapshot;
    }

    internal static void WriteConfigDiff(Dictionary<string, string> oldValues, Dictionary<string, string> newValues)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Config reload at {System.DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        sb.AppendLine(new string('-', 60));

        var changed = false;
        foreach (var key in oldValues.Keys.Union(newValues.Keys).OrderBy(k => k))
        {
            oldValues.TryGetValue(key, out var oldVal);
            newValues.TryGetValue(key, out var newVal);

            if (oldVal == newVal) continue;

            changed = true;
            if (oldVal == null)
            {
                sb.AppendLine($"[ADDED]   {key} = {newVal}");
            }
            else if (newVal == null)
            {
                sb.AppendLine($"[REMOVED] {key} = {oldVal}");
            }
            else
            {
                sb.AppendLine($"[CHANGED] {key}: {oldVal} -> {newVal}");
            }
        }

        if (!changed)
        {
            sb.AppendLine("No changes detected.");
        }

        sb.AppendLine();

        var diffPath = Path.Combine(Application.dataPath, "..", "CuisineerTweaks_ConfigDiff.log");
        diffPath = Path.GetFullPath(diffPath);
        File.AppendAllText(diffPath, sb.ToString());
        Plugin.Logger.LogInfo($"Config diff written to {diffPath}");
    }
}