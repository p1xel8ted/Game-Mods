using System.Reflection;

namespace LostToothUnlost;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.losttoothunlost";
    private const string PluginName = "Lost Tooth Unlost!";
    private const string PluginVer = "0.1.1";
    private static ManualLogSource LOG { get; set; }

    private void Awake()
    {
        LOG = Logger;
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameSave), nameof(GameSave.GlobalEventsCheck))]
    public static void OnGameStartedPlaying()
    {
        MainGame.me.player.DropItem(new Item("gerry_tooth"), Direction.ToPlayer);
    }
}
