using Shared;

namespace Namify;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInDependency("com.bepis.bepinex.configurationmanager", BepInDependency.DependencyFlags.SoftDependency)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.cotl.namify";
    internal const string PluginName = "Namify";
    private const string PluginVer = "0.2.3";

    // Section constants - match CultOfQoL format
    private const string NamesSection = "── Names ──";
    private const string ApiSection = "── API ──";

    public static ManualLogSource Log { get; private set; }
    internal static ConfigEntry<string> PersonalApiKey { get; private set; }
    internal static ConfigEntry<string> AddName { get; private set; }
    internal static ConfigEntry<bool> AsterixNames { get; private set; }
    private static bool ShowGetNewConfirmationDialog { get; set; }
    private static bool ShowReloadConfirmationDialog { get; set; }

    private static string NamifyNamesFilePath => Data.NamifyNamesFilePath;
    private static string UserNameFilePath => Data.UserNamesFilePath;
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

    private static IEnumerator PreloadNamesCoroutine()
    {
        // Wait a few frames for GameManager to initialize
        for (int i = 0; i < 10; i++)
        {
            yield return null;
        }
        
        // Try to load existing names from file first (check both .json and old .mp files)
        var namifyJsonExists = File.Exists(NamifyNamesFilePath);
        var namifyMpExists = File.Exists(Path.ChangeExtension(NamifyNamesFilePath, ".mp"));
        var userJsonExists = File.Exists(UserNameFilePath);
        var userMpExists = File.Exists(Path.ChangeExtension(UserNameFilePath, ".mp"));

        if (namifyJsonExists || namifyMpExists || userJsonExists || userMpExists)
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

        // ── Names ──
        AsterixNames = Config.Bind(
            NamesSection,
            "Asterisk Names", false,
            new ConfigDescription(
                Localization.DescAsteriskNames,
                null,
                new ConfigurationManagerAttributes { Order = 7 }
            )
        );

        AddName = Config.Bind(
            NamesSection,
            "Add Name", "",
            new ConfigDescription(
                Localization.DescAddName,
                null,
                new ConfigurationManagerAttributes { Order = 6 }
            )
        );

        Config.Bind(
            NamesSection,
            "Add Name Button", true,
            new ConfigDescription(
                Localization.DescAddNameButton,
                null,
                new ConfigurationManagerAttributes { Order = 5, DispName = string.Empty, HideDefaultButton = true, CustomDrawer = AddNameButton }
            )
        );

        Config.Bind(
            NamesSection,
            "Open Namify Names File", true,
            new ConfigDescription(
                Localization.DescOpenNamifyFile,
                null,
                new ConfigurationManagerAttributes { Order = 4, DispName = string.Empty, HideDefaultButton = true, CustomDrawer = OpenNamifyNamesFile }
            )
        );

        Config.Bind(
            NamesSection,
            "Open User Names File", true,
            new ConfigDescription(
                Localization.DescOpenUserFile,
                null,
                new ConfigurationManagerAttributes { Order = 3, DispName = string.Empty, HideDefaultButton = true, CustomDrawer = OpenUserGeneratedNamesFile }
            )
        );

        Config.Bind(
            NamesSection,
            "Generate New Names", true,
            new ConfigDescription(
                Localization.DescGenerateNew,
                null,
                new ConfigurationManagerAttributes { Order = 2, DispName = string.Empty, HideDefaultButton = true, CustomDrawer = GenerateNewNamesButton }
            )
        );

        Config.Bind(
            NamesSection,
            "Reload Names", true,
            new ConfigDescription(
                Localization.DescReloadNames,
                null,
                new ConfigurationManagerAttributes { Order = 1, DispName = string.Empty, HideDefaultButton = true, CustomDrawer = ReloadNames }
            )
        );

        // ── API ──
        PersonalApiKey = Config.Bind(
            ApiSection,
            "Personal API Key", "ee5f806e1c1d458b99c934c0eb3de5b8",
            new ConfigDescription(
                Localization.DescApiKey,
                null,
                new ConfigurationManagerAttributes { Order = 1 }
            )
        );
    }

    private static void OpenNamifyNamesFile(ConfigEntryBase entry)
    {
        if (GUILayout.Button(Localization.OpenNamifyList, GUILayout.ExpandWidth(true)))
        {
            TryOpenNamifyNamesFile();
        }
    }

    private static void OpenUserGeneratedNamesFile(ConfigEntryBase entry)
    {
        if (GUILayout.Button(Localization.OpenUserList, GUILayout.ExpandWidth(true)))
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
            PopupManagerInstance.ShowPopup(string.Format(Localization.FileNotFound, NamifyNamesFilePath));
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
            PopupManagerInstance.ShowPopup(string.Format(Localization.FileNotFound, UserNameFilePath));
        }
    }

    private static void DisplayGetNewConfirmationDialog()
    {
        GUILayout.Label(Localization.ConfirmGenerateNew);

        GUILayout.BeginHorizontal();
        {
            if (GUILayout.Button(Localization.Yes, GUILayout.ExpandWidth(true)))
            {
                GenerateNewNamesAction();
                ShowGetNewConfirmationDialog = false;
            }

            if (GUILayout.Button(Localization.No, GUILayout.ExpandWidth(true)))
            {
                ShowGetNewConfirmationDialog = false;
            }
        }
        GUILayout.EndHorizontal();
    }

    private static void DisplayReloadConfirmationDialog()
    {
        GUILayout.Label(Localization.ConfirmReload);

        GUILayout.BeginHorizontal();
        {
            if (GUILayout.Button(Localization.Yes, GUILayout.ExpandWidth(true)))
            {
                try
                {
                    Data.LoadData();
                    PopupManagerInstance.ShowPopup(Localization.NamesReloaded);
                }
                catch (Exception e)
                {
                    PopupManagerInstance.ShowPopup(Localization.ErrorReloading);
                    Log.LogError($"Error in reloading names: {e.Message}");
                }
                ShowReloadConfirmationDialog = false;
            }

            if (GUILayout.Button(Localization.No, GUILayout.ExpandWidth(true)))
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
                PopupManagerInstance.ShowPopup(Localization.ErrorGenerating);
            }, () =>
            {
                PopupManagerInstance.ShowPopup(Localization.NewNamesGenerated);
            });
        }
        catch (Exception ex)
        {
            Log.LogError($"Error in generating new names: {ex.Message}");
        }
    }

    private static void AddNameButton(ConfigEntryBase entry)
    {
        if (GUILayout.Button(Localization.AddNameButton, GUILayout.ExpandWidth(true)))
        {
            if (string.IsNullOrWhiteSpace(AddName.Value))
            {
                PopupManagerInstance.ShowPopup(Localization.NoNameEntered);
                return;
            }

            if (!Data.UserNames.Add(AddName.Value))
            {
                PopupManagerInstance.ShowPopup(string.Format(Localization.NameExists, AddName.Value));
                return;
            }

            Data.SaveData();
            PopupManagerInstance.ShowPopup(string.Format(Localization.NameAdded, AddName.Value));
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
            if (GUILayout.Button(Localization.ReloadNamesFromFile, GUILayout.ExpandWidth(true)))
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
            if (GUILayout.Button(Localization.GenerateNewNames, GUILayout.ExpandWidth(true)))
            {
                ShowGetNewConfirmationDialog = true;
            }
        }
    }
}