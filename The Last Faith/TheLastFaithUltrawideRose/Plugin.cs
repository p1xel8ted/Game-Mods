using System;
using System.Linq;
using System.Runtime.CompilerServices;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using Cinemachine;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TheLastFaithUltrawideRose;

[Harmony]
[BepInPlugin("thelastfaith.ultrawide.rose", "The Last Faith Ultrawide by Rose", "9.9.9")]
public class Plugin : BasePlugin
{

    private const float BaseAR = 16f / 9f;
    private readonly static float CurrentAR = SystemWidth / (float) SystemHeight;
    private readonly static float ARDifference = CurrentAR / BaseAR;
    private readonly static int MaxRefresh = Screen.resolutions.Max(a => a.refreshRate);
    private static int SystemWidth => Display.displays[0].systemWidth;
    private static int SystemHeight => Display.displays[0].systemHeight;
    private static ManualLogSource Logger { get; set; }
    private static WindowPositioner WindowPositioner { get; set; }

    public override void Load()
    {
        WindowPositioner = AddComponent<WindowPositioner>();
        Logger = Log;
        Harmony.CreateAndPatchAll(typeof(Patches));
        SceneManager.sceneLoaded += (UnityAction<Scene, LoadSceneMode>) SceneManager_SceneLoaded;
        Logger.LogInfo("The Last Faith Ultrawide by Rose loaded!");
        Display.displays[0].Activate(SystemWidth, SystemHeight, MaxRefresh);
    }

    private static void SceneManager_SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        WindowPositioner.Start();
        Application.targetFrameRate = MaxRefresh;
        Screen.SetResolution(SystemWidth, SystemHeight, FullScreenMode.FullScreenWindow, MaxRefresh);
        Time.fixedDeltaTime = 1f / MaxRefresh;
    }

    [Harmony]
    private class Patches
    {
        private static void CamFix(Camera Cam)
        {
            if (Cam.targetTexture != null)
            {
                var targetTexture = Cam.targetTexture;
                var descriptor = targetTexture.descriptor;
                if (descriptor.width != Display.main.systemWidth)
                {
                    descriptor.width = Display.main.systemWidth;
                    descriptor.height = Display.main.systemHeight;
                    var renderTexture = new RenderTexture(descriptor)
                    {
                        filterMode = targetTexture.filterMode,
                        name = targetTexture.name
                    };
                    Cam.targetTexture = renderTexture;
                    if (Cam.targetTexture == renderTexture)
                    {
                        targetTexture.Release();
                    }
                }
            }
            // var component = Cam.GetComponent<PixelPerfectCamera>();
            // if (component == null) return;
            // component.gridSnapping = PixelPerfectCamera.GridSnapping.PixelSnapping;
            // if (SystemHeight == 3840 && SystemHeight == 1024)
            // {
            //     component.enabled = false;
            // }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Screen), nameof(Screen.SetResolution), typeof(int), typeof(int), typeof(bool), typeof(int))]
        [HarmonyPatch(typeof(Screen), nameof(Screen.SetResolution), typeof(int), typeof(int), typeof(FullScreenMode))]
        [HarmonyPatch(typeof(Screen), nameof(Screen.SetResolution), typeof(int), typeof(int), typeof(bool))]
        [HarmonyPatch(typeof(Screen), nameof(Screen.SetResolution), typeof(int), typeof(int), typeof(FullScreenMode), typeof(int))]
        public static void Screen_SetResolution(ref int width, ref int height)
        {
            width = SystemWidth;
            height = SystemHeight;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(LoadableScene), nameof(LoadableScene.Awake))]
        private static void LoadableScene_Awake()
        {
            var gameObject = GameObject.FindWithTag("MainCamera");
            if (gameObject == null) return;
            var component = gameObject.GetComponent<Camera>();
            if (component == null) return;
            CamFix(component);
            var componentInChildren = gameObject.GetComponentInChildren<MeshRenderer>();
            if (componentInChildren == null) return;
            var transform = componentInChildren.transform;
            var localScale = transform.localScale;
            localScale = new Vector3(localScale.y * CurrentAR, localScale.y, localScale.z);
            transform.localScale = localScale;
            componentInChildren.material.mainTexture = component.targetTexture;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(CinemachineVirtualCamera), nameof(CinemachineVirtualCamera.OnEnable))]
        private static void CinemachineVirtualCameraBounds_OnEnable(CinemachineVirtualCamera __instance)
        {
            var gameObject = __instance.transform.gameObject;
            if (gameObject == null) return;
            var componentInChildren = gameObject.GetComponentInChildren<CinemachineConfiner>();
            if (componentInChildren == null) return;
            componentInChildren.enabled = false;
            if (gameObject.GetComponentInChildren<CinemachineConfiner2D>() != null || !(CurrentAR < 2.8f)) return;
            var cinemachineConfiner2D = gameObject.AddComponent<CinemachineConfiner2D>();
            var component = GameObject.Find("CONFINE CAMERA").GetComponent<PolygonCollider2D>();
            if (component == null) return;
            cinemachineConfiner2D.m_BoundingShape2D = component;
            cinemachineConfiner2D.m_MaxWindowSize = __instance.m_Lens.OrthographicSize;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(CanvasScaler), nameof(CanvasScaler.Handle))]
        private static void UI(CanvasScaler __instance)
        {
            if (__instance != null)
            {
                __instance.m_ScreenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
            }
            var gameObject = GameObject.Find("Cinematic Black Bars");
            if (gameObject != null && Mathf.Approximately(gameObject.transform.localScale.x, 1f))
            {
                var localScale = gameObject.transform.localScale;
                localScale.x *= ARDifference;
                gameObject.transform.localScale = localScale;
            }
        }
    }
}