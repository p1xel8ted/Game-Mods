using Object = System.Object;

namespace CuisineerTweaks;

public static class Utils
{
    public static void WriteLog(string message, bool ignoreDebug = false)
    {
        // if (Plugin.DebugMode.Value || ignoreDebug)
        // {
        Plugin.Logger.LogInfo(message);
        //}
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
        var list = new List<T>();
        list.AddRange(Resources.FindObjectsOfTypeAll(Il2CppType.Of<T>()).Select(obj => obj.TryCast<T>()).Where(o => o != null));
        return list;
    }

    // public static void AttachToSceneOnLoaded(System.Action<Scene, LoadSceneMode> action)
    // {
    //     SceneManager.sceneLoaded += action;
    // }

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
}