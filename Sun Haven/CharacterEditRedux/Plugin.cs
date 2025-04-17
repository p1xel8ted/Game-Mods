namespace CharacterEditRedux;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.sunhaven.charactereditredux";
    private const string PluginName = "Character Edit Redux";
    private const string PluginVersion = "0.1.0";
    internal static ManualLogSource Log { get; private set; }

    private void Awake()
    {
        Log = Logger;

        Utils.LoadTexture(Assembly.GetExecutingAssembly(), $"{GetType().Namespace}.assets.mouseover.png", ref Patches.MouseOverImage);
        Utils.LoadTexture(Assembly.GetExecutingAssembly(), $"{GetType().Namespace}.assets.default.png", ref Patches.DefaultImage);

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        Logger.LogInfo($"Plugin {PluginName} is loaded! Running game version {Application.version} on {PlatformHelper.Current}.");
    }
}