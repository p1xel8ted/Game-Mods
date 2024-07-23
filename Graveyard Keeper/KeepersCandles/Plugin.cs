using System;
using UnityEngine.SceneManagement;

namespace KeepersCandles;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInDependency("p1xel8ted.gyk.gykhelper", "3.0.8")]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.keeperscandles";
    private const string PluginName = "Keeper's Candles!";
    private const string PluginVer = "0.1.0";
    internal const string Candle = "candle";
    internal static ConfigEntry<bool> AlwaysActive { get; private set; }
    internal static ConfigEntry<float> DurabilityModificator { get; private set; }
    internal static ManualLogSource LOG { get; set; }

    private void Awake()
    {
        LOG = Logger;
        AlwaysActive = Config.Bind("01. General", "Always Active", false, "Keeps them active all the time. I think.");
        AlwaysActive.SettingChanged += (_, _) => OnGameBalanceLoaded();
        DurabilityModificator = Config.Bind("01. General", "Durability Modificator", 1f, "This is how fast/slow the candle burns. Set to 0 to disable.");
        DurabilityModificator.SettingChanged += (_, _) => OnGameBalanceLoaded();
        SceneManager.sceneLoaded += (_, _) => OnGameBalanceLoaded();
        Actions.GameBalanceLoad += OnGameBalanceLoaded;
        Actions.GameStartedPlaying += OnGameBalanceLoaded;
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        LOG.LogInfo($"Plugin {PluginName} is loaded!");
    }
    internal static void OnGameBalanceLoaded()
    {
        try
        {
            foreach (var obj in GameBalance._instance.objs_data.Where(obj => obj.id.ContainsByLanguage(Candle)))
            {
                obj.always_active = AlwaysActive.Value;
                obj.durability_modificator = DurabilityModificator.Value;
                LOG.LogInfo($"Candle {obj.id} is always active: {obj.always_active}, durability modificator: {obj.durability_modificator}");
            }

            foreach (var wgo in WorldMap._objs.Where(wgo => wgo.obj_def.id.ContainsByLanguage(Candle)))
            {
                wgo.obj_def.always_active = AlwaysActive.Value;
                wgo.obj_def.durability_modificator = DurabilityModificator.Value;
                LOG.LogInfo($"Candle {wgo.obj_def.id} is always active: {wgo.obj_def.always_active}, durability modificator: {wgo.obj_def.durability_modificator}");
            }
        }
        catch (Exception)
        {
            // ignored
        }
    }


}