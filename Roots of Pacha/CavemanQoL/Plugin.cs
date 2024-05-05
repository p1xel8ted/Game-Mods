namespace CavemanQoL;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.rootsofpacha.cavemanqol";
    private const string PluginName = "Caveman QoL";
    private const string PluginVersion = "0.0.1";
    private static ManualLogSource Log { get; set; }
    

    private void Awake()
    {
        Log = Logger;

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        Log.LogInfo($"Plugin {PluginName} is loaded!");
    }

    private void Update()
    {
        if (Session.Instance == null) return;
        if (Input.GetKeyUp(KeyCode.F5))
        {
            SavedGames.Save(Session.Instance.CurrentDay.Value, Session.Instance.ShippingBin.NextDayContributions, Session.Instance.ShippingBin.NextDayProsperity);
        }
    }
      
}