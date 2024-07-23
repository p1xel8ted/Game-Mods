using BepInEx;
using BepInEx.Logging;
using GYKHelper;

namespace LostToothUnlost;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.losttoothunlost";
    private const string PluginName = "Lost Tooth Unlost!";
    private const string PluginVer = "0.1.0";
    private static ManualLogSource LOG { get; set; }

    private void Awake()
    {
        LOG = Logger;
        Actions.GameStartedPlaying += OnGameStartedPlaying;
        LOG.LogInfo($"Plugin {PluginName} is loaded!");
    }
    private static void OnGameStartedPlaying()
    {
        MainGame.me.player.DropItem(new Item("gerry_tooth"), Direction.ToPlayer);
    }

}