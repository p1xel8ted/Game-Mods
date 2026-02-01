using Shared;

namespace Namify;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInDependency("com.bepis.bepinex.configurationmanager", "18.4.1")]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.cotl.namify";
    internal const string PluginName = "Namify";
    private const string PluginVer = "0.2.2";
    private const string NamesSection = "Names";
    private const string ApiSection = "API";

    public static ManualLogSource Log { get; private set; }
    internal static ConfigEntry<string> PersonalApiKey { get; private set; }
    internal static ConfigEntry<string> AddName { get; private set; }
    internal static ConfigEntry<bool> AsterixNames { get; private set; }
    private static bool ShowGetNewConfirmationDialog { get; set; }
    private static bool ShowReloadConfirmationDialog { get; set; }

    // private static string _namifyNamesFilePath { get; set; }
    // private static string _userDataFilePath { get; set; }
    private static string NamifyNamesFilePath => Path.Combine(Application.persistentDataPath, "saves", Data.NamifyDataPath);
    private static string UserNameFilePath => Path.Combine(Application.persistentDataPath, "saves", Data.UserDataPath);
    private static PopupManager PopupManagerInstance { get; set; }

    private void Awake()
    {
        InitializeLogger();
        InitializeConfigurations();
        
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        Helpers.PrintModLoaded(PluginName, Logger);
    }

    private void Start()
    {
        // Pre-load names to avoid async issues during first follower generation
        StartCoroutine(PreloadNamesCoroutine());
    }

    private IEnumerator PreloadNamesCoroutine()
    {
        // Wait a few frames for GameManager to initialize
        for (int i = 0; i < 10; i++)
        {
            yield return null;
        }
        
        // Try to load existing names from file first
        if (File.Exists(NamifyNamesFilePath) || File.Exists(UserNameFilePath))
        {
            Log.LogInfo("Loading existing names from file...");
            Data.LoadData();
        }
        
        // If still no names loaded, fetch from API
        if (Data.NamifyNames.Count == 0 && Data.UserNames.Count == 0)
        {
            Log.LogInfo("No existing names found, fetching from API...");
            Data.GetNamifyNames(
                onFail: () => Log.LogInfo("Failed to fetch names during initialization"),
                onComplete: () => Log.LogInfo("Successfully pre-loaded names from API")
            );
        }
        else
        {
            Log.LogInfo($"Pre-loaded {Data.NamifyNames.Count} Namify names and {Data.UserNames.Count} user names.");
        }
    }

    private void InitializeLogger()
    {
        Log = Logger;
    }

    private void InitializeConfigurations()
    {
        PopupManagerInstance = gameObject.AddComponent<PopupManager>();
        AsterixNames = Config.Bind(NamesSection, "Asterisks Names", false, new ConfigDescription("Namified names will have an asterisk next to them in the indoctrination UI.", null, new ConfigurationManagerAttributes {Order = 12}));
        
        PersonalApiKey = Config.Bind(ApiSection, "Personal API Key", "ee5f806e1c1d458b99c934c0eb3de5b8", "The default API Key is mine, limited to 1000 requests per day. You can get your own at https://randommer.io/");
        AddName = Config.Bind(NamesSection, "Add Name", "", new ConfigDescription("Adds a name to the list of names.", null, new ConfigurationManagerAttributes {Order = 10}));
       
        Config.Bind(NamesSection, "Add Name Button", true, new ConfigDescription("Add the name entered to the list.", null, new ConfigurationManagerAttributes {Order = 9, DispName = string.Empty, HideDefaultButton = true, CustomDrawer = AddNameButton}));
        Config.Bind(NamesSection, "Open Namify Names File", true, new ConfigDescription("Opens the Namify generated names file for viewing/editing.", null, new ConfigurationManagerAttributes {Order = 8, DispName = string.Empty, HideDefaultButton = true, CustomDrawer = OpenNamifyNamesFile}));
        Config.Bind(NamesSection, "Open User Names File", true, new ConfigDescription("Opens the user-generated names file for viewing/editing.", null, new ConfigurationManagerAttributes {Order = 7, DispName = string.Empty, HideDefaultButton = true, CustomDrawer = OpenUserGeneratedNamesFile}));
        Config.Bind(NamesSection, "Generate New Names", true, new ConfigDescription("Generates new Namify games. User-generated names are not changed.", null, new ConfigurationManagerAttributes {Order = 6, DispName = string.Empty, HideDefaultButton = true, CustomDrawer = GenerateNewNamesButton}));
        Config.Bind(NamesSection, "Reload Names", true, new ConfigDescription("Reloads names from file.", null, new ConfigurationManagerAttributes {Order = 5, DispName = string.Empty, HideDefaultButton = true, CustomDrawer = ReloadNames}));
    }

    private static void OpenNamifyNamesFile(ConfigEntryBase entry)
    {
        if (GUILayout.Button("Open Namify List", GUILayout.ExpandWidth(true)))
        {
            TryOpenNamifyNamesFile();
        }
    }
    
    private static void OpenUserGeneratedNamesFile(ConfigEntryBase entry)
    {
        if (GUILayout.Button("Open User List", GUILayout.ExpandWidth(true)))
        {
            TryOpenUserNamesFile();
        }
    }

    private static void TryOpenNamifyNamesFile()
    {
        if (File.Exists(NamifyNamesFilePath))
        {
            Application.OpenURL(NamifyNamesFilePath);
        }
        else
        {
            PopupManagerInstance.ShowPopup($"Names file ({NamifyNamesFilePath}) does not exist!");
        }
    }

    private static void TryOpenUserNamesFile()
    {
        if (File.Exists(UserNameFilePath))
        {
            Application.OpenURL(UserNameFilePath);
        }
        else
        {
            PopupManagerInstance.ShowPopup($"Names file ({UserNameFilePath}) does not exist!");
        }
    }

    private static void DisplayGetNewConfirmationDialog()
    {
        GUILayout.Label("Are you sure you want to generate new names?");

        GUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Yes", GUILayout.ExpandWidth(true)))
            {
                GenerateNewNamesAction();
                ShowGetNewConfirmationDialog = false;
            }

            if (GUILayout.Button("No", GUILayout.ExpandWidth(true)))
            {
                ShowGetNewConfirmationDialog = false;
            }
        }
        GUILayout.EndHorizontal();
    }

    private static void DisplayReloadConfirmationDialog()
    {
        GUILayout.Label("Are you sure you want to reload names from file?");

        GUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Yes", GUILayout.ExpandWidth(true)))
            {
                try
                {
                    Data.LoadData();
                    PopupManagerInstance.ShowPopup("Names reloaded from file!");
                }
                catch (Exception e)
                {
                    PopupManagerInstance.ShowPopup("Error in reloading names. Check log for more details.");
                    Log.LogError($"Error in reloading names: {e.Message}");
                }
                ShowReloadConfirmationDialog = false;
            }

            if (GUILayout.Button("No", GUILayout.ExpandWidth(true)))
            {
                ShowReloadConfirmationDialog = false;
            }
        }
        GUILayout.EndHorizontal();
    }

    private static void GenerateNewNamesAction()
    {
        try
        {
            File.Delete(NamifyNamesFilePath);
            Data.NamifyNames.Clear();
            Data.GetNamifyNames(() =>
            {
                PopupManagerInstance.ShowPopup("Error in generating new names!");
            }, () =>
            {
                PopupManagerInstance.ShowPopup("New names generated!");
            });
        }
        catch (Exception ex)
        {
            Log.LogError($"Error in generating new names: {ex.Message}");
        }
    }

    private static void AddNameButton(ConfigEntryBase entry)
    {
        if (GUILayout.Button("Add Name", GUILayout.ExpandWidth(true)))
        {
            if (string.IsNullOrWhiteSpace(AddName.Value))
            {
                PopupManagerInstance.ShowPopup("You haven't entered a name to add?");
                return;
            }

            if (!Data.UserNames.Add(AddName.Value))
            {
                PopupManagerInstance.ShowPopup($"'{AddName.Value}' already exists!");
                return;
            }

            Data.SaveData();
            PopupManagerInstance.ShowPopup($"Added '{AddName.Value}' to available names!");
        }
    }

    private static void ReloadNames(ConfigEntryBase entry)
    {
        if (ShowReloadConfirmationDialog)
        {
            DisplayReloadConfirmationDialog();
        }
        else
        {
            var button = GUILayout.Button("Reload Names From File", GUILayout.ExpandWidth(true));
            if (button)
            {
                ShowReloadConfirmationDialog = true;
            }
        }
    }

    private static void GenerateNewNamesButton(ConfigEntryBase entry)
    {
        if (ShowGetNewConfirmationDialog)
        {
            DisplayGetNewConfirmationDialog();
        }
        else
        {
            var button = GUILayout.Button("Generate New Names", GUILayout.ExpandWidth(true));
            if (button)
            {
                ShowGetNewConfirmationDialog = true;
            }
        }
    }
}