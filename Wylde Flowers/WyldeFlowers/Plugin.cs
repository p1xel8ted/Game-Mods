using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

namespace WyldeFlowers;

[Harmony]
[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Plugin : BasePlugin
{
    private const string PluginGuid = "p1xel8ted.wyldeflowers.tweaks";
    private const string PluginName = "Wylde Flowers Tweaks";
    private const string PluginVersion = "0.1.0";
    internal static PlayerState PlayerStateInstance { get; set; }
    private static ManualLogSource Logger { get; set; }
    private static Harmony Harmony { get; set; }
    internal static ConfigEntry<bool> SkipLogos { get; private set; }
    private static ConfigEntry<int> StaminaInterval { get; set; }
    private static ConfigEntry<int> StaminaAmount { get; set; }
    internal static ConfigEntry<float> RunSpeedPercentIncrease { get; private set; }
    internal static ConfigEntry<bool> AlsoAdjustRunAnimationSpeed { get; private set; }
    private static ConfigEntry<bool> PauseOnFocusLost { get; set; }
    internal static ConfigEntry<bool> DropStraightToInventory { get; private set; }
    private UpdateEvent UpdateEventInstance { get; set; }
    private static float Time { get; set; }

    [DllImport("user32.dll")]
    private static extern IntPtr SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    private static void ForceFocus()
    {
        var currentProcess = Process.GetCurrentProcess();
        var hWnd = currentProcess.MainWindowHandle;
        if (hWnd == IntPtr.Zero) return;
        SetForegroundWindow(hWnd);
        ShowWindow(hWnd, 3);
        Logger.LogInfo("Forced focus to prevent stuttering.");
    }

    public override void Load()
    {
        SceneManager.sceneLoaded += (UnityAction<Scene, LoadSceneMode>) SceneManagerOnSceneLoaded;
        SkipLogos = Config.Bind("01. General", "Skip Logos", true, new ConfigDescription("Skip the intro logos.", null, new ConfigurationManagerAttributes {Order = 50}));
        PauseOnFocusLost = Config.Bind("01. General", "Pause On Focus Lost", false, new ConfigDescription("Pause the game when the window loses focus.", null, new ConfigurationManagerAttributes {Order = 51}));
        StaminaInterval = Config.Bind("02. Stamina", "Interval", 5, new ConfigDescription("Time in seconds between each stamina tick.", null, new ConfigurationManagerAttributes {Order = 49}));
        StaminaAmount = Config.Bind("02. Stamina", "Amount", 10, new ConfigDescription("Amount of stamina to add each tick.", null, new ConfigurationManagerAttributes {Order = 48}));
        RunSpeedPercentIncrease = Config.Bind("03. Movement", "Run Speed Percentage Increase", 25f, new ConfigDescription("Run speed multiplier. Default is a 25% speed increase.", new AcceptableValueRange<float>(0f, 500f), new ConfigurationManagerAttributes {ShowRangeAsPercent = true, Order = 47}));
        AlsoAdjustRunAnimationSpeed = Config.Bind("03. Movement", "Also Adjust Run Animation Speed", true, new ConfigDescription("Also adjust the run animation speed. This will make the run animation look more natural(?). Test both and see for yourself.", null, new ConfigurationManagerAttributes {Order = 46}));
        DropStraightToInventory = Config.Bind("04. Inventory", "Drop Straight To Inventory", true, new ConfigDescription("Drop items straight to inventory instead of dropping them on the ground.", null, new ConfigurationManagerAttributes {Order = 45}));
        Logger = Log;
        Harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        UpdateEventInstance = AddComponent<UpdateEvent>();
        Application.runInBackground = !PauseOnFocusLost.Value;
    }

    public override bool Unload()
    {
        SceneManager.sceneLoaded -= (UnityAction<Scene, LoadSceneMode>) SceneManagerOnSceneLoaded;
        UpdateEventInstance.Destroy();
        Harmony.UnpatchSelf();
        return base.Unload();
    }

    private static void SceneManagerOnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        var maxRefreshRate = Screen.resolutions.Max(a => a.refreshRate);
        Screen.SetResolution(Display._mainDisplay.systemWidth, Display._mainDisplay.systemHeight, FullScreenMode.FullScreenWindow, maxRefreshRate);
        UnityEngine.Time.fixedDeltaTime = 1f / maxRefreshRate;
        Logger.LogInfo($"Set resolution to {Screen.currentResolution} and fixedDeltaTime to {UnityEngine.Time.fixedDeltaTime}.");
        ForceFocus();
    }

    public class UpdateEvent : MonoBehaviour
    {
        public void Update()
        {
            if (PlayerStateInstance == null) return;


            if (Input.GetKeyUp(KeyCode.Plus) || Input.GetKeyUp(KeyCode.KeypadPlus))
            {
                RunSpeedPercentIncrease.Value += 5f;
                RunSpeedPercentIncrease.Value = Mathf.Clamp(RunSpeedPercentIncrease.Value, 0f, 500f);
            }
            else if (Input.GetKeyUp(KeyCode.Minus) || Input.GetKeyUp(KeyCode.KeypadMinus))
            {
                RunSpeedPercentIncrease.Value -= 5f;
                RunSpeedPercentIncrease.Value = Mathf.Clamp(RunSpeedPercentIncrease.Value, 0f, 500f);
            }

            if (UnityEngine.Time.time > Time + StaminaInterval.Value)
            {
                if (PlayerStateInstance.stamina < PlayerStateInstance.maxStamina)
                {
                    PlayerStateInstance.stamina += StaminaAmount.Value;
                    Logger.LogInfo("Stamina: " + PlayerStateInstance.stamina);
                }

                Time = UnityEngine.Time.time;
            }
        }
    }
}