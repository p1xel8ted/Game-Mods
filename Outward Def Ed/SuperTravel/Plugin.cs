using System;
using System.Net.Mime;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace SuperTravel;

[BepInPlugin(GUID, NAME, VERSION)]
public class Plugin : BaseUnityPlugin
{
    private static ManualLogSource Log = null!;

    internal static ConfigEntry<float> SpeedIncrease { get; private set; }
        
    private const string GUID = "boctimus.supertravel";
    private const string NAME = "Super Travel";
    private const string VERSION = "1.0.1";

    private void Awake()
    {
        Log = Logger;
        SpeedIncrease = Config.Bind("General", "Speed Increase", 0.5f, new ConfigDescription("The speed increase when out of combat", new AcceptableValueRange<float>(0f, 2f)));
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), GUID);
        Log.LogInfo($"Loaded {NAME}!");
    }
    
}