namespace Namify;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInDependency("com.bepis.bepinex.configurationmanager", "18.3")]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.cotl.namify";
    internal const string PluginName = "Namify";
    private const string PluginVer = "0.1.4";
    private const string NamesSection = "Names";
    private const string ApiSection = "API";

    public static ManualLogSource Log = null!;
    internal static ConfigEntry<string> PersonalApiKey = null!;
    internal static ConfigEntry<string> AddName = null!;

    private static bool _showGetNewConfirmationDialog;
    private static bool _showReloadConfirmationDialog;
    private static string _namifyNamesFilePath;
    private static string _userDataFilePath;
    private static string NamifyNamesFilePath => _namifyNamesFilePath ??= Path.Combine(Application.persistentDataPath, "saves", Data.NamifyDataPath);
    private static string UserNameFilePath => _userDataFilePath ??= Path.Combine(Application.persistentDataPath, "saves", Data.UserDataPath);

    private void Awake()
    {
        InitializeLogger();
        InitializeConfigurations();
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        Log.LogInfo($"Loaded {PluginName}!");
    }

    private void InitializeLogger()
    {
        Log = Logger;
    }
    private static PopupManager PopupManager = null!;
    private void InitializeConfigurations()
    {
        PopupManager = gameObject.AddComponent<PopupManager>();
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
            PopupManager.ShowPopup($"Names file ({NamifyNamesFilePath}) does not exist!");
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
            PopupManager.ShowPopup($"Names file ({UserNameFilePath}) does not exist!");
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
                _showGetNewConfirmationDialog = false;
            }

            if (GUILayout.Button("No", GUILayout.ExpandWidth(true)))
            {
                _showGetNewConfirmationDialog = false;
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
                    PopupManager.ShowPopup("Names reloaded from file!");
                }
                catch (Exception e)
                {
                    PopupManager.ShowPopup($"Error in reloading names. Check log for more details.");
                    Log.LogError($"Error in reloading names: {e.Message}");
                }
                _showReloadConfirmationDialog = false;
            }

            if (GUILayout.Button("No", GUILayout.ExpandWidth(true)))
            {
                _showReloadConfirmationDialog = false;
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
                PopupManager.ShowPopup("Error in generating new names!");
            }, () =>
            {
                PopupManager.ShowPopup("New names generated!");
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
                PopupManager.ShowPopup("You haven't entered a name to add?");
                return;
            }

            if (!Data.UserNames.Add(AddName.Value))
            {
                PopupManager.ShowPopup($"'{AddName.Value}' already exists!");
                return;
            }

            Data.SaveData();
            PopupManager.ShowPopup($"Added '{AddName.Value}' to available names!");
        }
    }

    private static void ReloadNames(ConfigEntryBase entry)
    {
        if (_showReloadConfirmationDialog)
        {
            DisplayReloadConfirmationDialog();
        }
        else
        {
            var button = GUILayout.Button("Reload Names From File", GUILayout.ExpandWidth(true));
            if (button)
            {
                _showReloadConfirmationDialog = true;
            }
        }
    }

    private static void GenerateNewNamesButton(ConfigEntryBase entry)
    {
        if (_showGetNewConfirmationDialog)
        {
            DisplayGetNewConfirmationDialog();
        }
        else
        {
            var button = GUILayout.Button("Generate New Names", GUILayout.ExpandWidth(true));
            if (button)
            {
                _showGetNewConfirmationDialog = true;
            }
        }
    }
}