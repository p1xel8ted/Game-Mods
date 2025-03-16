namespace WorshippersOfCthulhu;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Plugin : BasePlugin
{
    private const string PluginGuid = "p1xel8ted.worshippersofcthulhu.tweaks";
    private const string PluginName = "Worshippers of Cthulhu Tweaks";
    private const string PluginVersion = "0.1.0";

    internal static ManualLogSource Logger { get; private set; }
   
    internal static ConfigEntry<KeyCode> KeybindReload { get; private set; }
    
    private void InitConfig()
    {
        KeybindReload = Config.Bind("00. General", "KeybindReload", KeyCode.R,
            new ConfigDescription("The keybind to reload the configuration file. List of valid entries here: https://docs.unity3d.com/ScriptReference/KeyCode.html"));
    }


    public override void Load()
    {
        Instance = this;
        Logger = Log;
        Config.ConfigReloaded += (_, _) =>
        {
            InitConfig();
            Logger.LogInfo("Reloaded configuration.");
        };
        
        InitConfig();
        
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
       
        AddComponent<UnityEvents>();
        
        Logger.LogInfo($"Plugin {PluginGuid} is loaded!");
    }

    public static Plugin Instance { get; private set; }
}