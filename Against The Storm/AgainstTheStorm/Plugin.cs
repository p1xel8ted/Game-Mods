using System;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace AgainstTheStorm;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
[BepInDependency("com.bepis.bepinex.configurationmanager", "18.2")]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.againstthestorm.qol";
    internal const string PluginName = "QoL";
    private const string PluginVersion = "0.1.0";

    public static ManualLogSource Log { get; private set; }

    private void Awake()
    {
       
        InitializeLogger();
        //InitializeConfigurations();
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        Log.LogInfo($"Loaded {PluginName}!");
    }
    
    private void InitializeLogger()
    {
        Log = Logger;
    }
   
    // private void InitializeConfigurations()
    // {
    //     
    // }
    
}