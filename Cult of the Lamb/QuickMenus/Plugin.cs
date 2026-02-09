namespace QuickMenus;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInDependency("com.bepis.bepinex.configurationmanager", "18.4.1")]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.cotl.quickmenus";
    private const string PluginName = "Quick Menus";
    private const string PluginVer = "0.1.0";

    internal static ManualLogSource Log { get; private set; }

    private static float _savedTimeScale = 1f;
    private static UIMenuBase _activeMenu;
    private static string _activeMenuKey;

    // Hotkey configs
    internal static ConfigEntry<KeyboardShortcut> FollowerFormsKey { get; private set; }
    internal static ConfigEntry<KeyboardShortcut> BuildMenuKey { get; private set; }
    internal static ConfigEntry<KeyboardShortcut> TailorKey { get; private set; }
    internal static ConfigEntry<KeyboardShortcut> PlayerUpgradesKey { get; private set; }

    // Enable toggles
    internal static ConfigEntry<bool> EnableFollowerForms { get; private set; }
    internal static ConfigEntry<bool> EnableBuildMenu { get; private set; }
    internal static ConfigEntry<bool> EnableTailor { get; private set; }
    internal static ConfigEntry<bool> EnablePlayerUpgrades { get; private set; }

    private void Awake()
    {
        Log = Logger;

        FollowerFormsKey = Config.Bind(
            "01. Hotkeys", "Follower Forms",
            new KeyboardShortcut(KeyCode.F2),
            new ConfigDescription(Localization.DescFollowerForms, null,
                new ConfigurationManagerAttributes { Order = 8, DispName = Localization.NameFollowerForms }));

        BuildMenuKey = Config.Bind(
            "01. Hotkeys", "Build Menu",
            new KeyboardShortcut(KeyCode.F3),
            new ConfigDescription(Localization.DescBuildMenu, null,
                new ConfigurationManagerAttributes { Order = 7, DispName = Localization.NameBuildMenu }));

        TailorKey = Config.Bind(
            "01. Hotkeys", "Tailor / Clothing",
            new KeyboardShortcut(KeyCode.F4),
            new ConfigDescription(Localization.DescTailorClothing, null,
                new ConfigurationManagerAttributes { Order = 6, DispName = Localization.NameTailorClothing }));

        PlayerUpgradesKey = Config.Bind(
            "01. Hotkeys", "Player Upgrades / Fleeces",
            new KeyboardShortcut(KeyCode.F5),
            new ConfigDescription(Localization.DescPlayerUpgrades, null,
                new ConfigurationManagerAttributes { Order = 5, DispName = Localization.NamePlayerUpgrades }));

        EnableFollowerForms = Config.Bind(
            "02. Toggles", "Enable Follower Forms",
            true,
            new ConfigDescription(Localization.DescEnableFollowerForms, null,
                new ConfigurationManagerAttributes { Order = 4, DispName = Localization.NameEnableFollowerForms }));

        EnableBuildMenu = Config.Bind(
            "02. Toggles", "Enable Build Menu",
            true,
            new ConfigDescription(Localization.DescEnableBuildMenu, null,
                new ConfigurationManagerAttributes { Order = 3, DispName = Localization.NameEnableBuildMenu }));

        EnableTailor = Config.Bind(
            "02. Toggles", "Enable Tailor",
            true,
            new ConfigDescription(Localization.DescEnableTailor, null,
                new ConfigurationManagerAttributes { Order = 2, DispName = Localization.NameEnableTailor }));

        EnablePlayerUpgrades = Config.Bind(
            "02. Toggles", "Enable Player Upgrades",
            true,
            new ConfigDescription(Localization.DescEnablePlayerUpgrades, null,
                new ConfigurationManagerAttributes { Order = 1, DispName = Localization.NameEnablePlayerUpgrades }));

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        Helpers.PrintModLoaded(PluginName, Logger);
    }

    private void Update()
    {
        if (EnableFollowerForms.Value && FollowerFormsKey.Value.IsDown())
        {
            HandleKey("forms", OpenFollowerFormsMenu);
        }
        else if (EnableBuildMenu.Value && BuildMenuKey.Value.IsDown())
        {
            HandleKey("build", OpenBuildMenu);
        }
        else if (EnableTailor.Value && TailorKey.Value.IsDown())
        {
            HandleKey("tailor", OpenTailorMenu);
        }
        else if (EnablePlayerUpgrades.Value && PlayerUpgradesKey.Value.IsDown())
        {
            HandleKey("upgrades", OpenPlayerUpgradesMenu);
        }
    }

    private static void HandleKey(string key, Action open)
    {
        if (_activeMenu != null)
        {
            var isSameMenu = _activeMenuKey == key;
            _activeMenu.Hide(true);
            if (isSameMenu) return;
        }
        else if (!CanOpenMenu())
        {
            return;
        }

        _activeMenuKey = key;
        open();
    }

    private static bool CanOpenMenu()
    {
        if (MonoSingleton<UIManager>.Instance == null) return false;
        if (UIMenuBase.ActiveMenus is { Count: > 0 }) return false;
        if (MMTransition.IsPlaying) return false;
        if (GameManager.InMenu) return false;
        if (PlayerFarming.Instance == null) return false;

        var state = PlayerFarming.Instance.state;
        if (state == null) return false;

        return state.CURRENT_STATE is not (StateMachine.State.InActive
            or StateMachine.State.CustomAnimation
            or StateMachine.State.Dead);
    }

    private static void PauseGame()
    {
        _savedTimeScale = Time.timeScale;
        PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.InActive;
        HUD_Manager.Instance.Hide(false, 0);
        Time.timeScale = 0f;
    }

    private static void ResumeGame()
    {
        _activeMenu = null;
        _activeMenuKey = null;
        Time.timeScale = _savedTimeScale;
        PlayerFarming.SetStateForAllPlayers();
        HUD_Manager.Instance.Show(0);
    }

    private static void OpenFollowerFormsMenu()
    {
        var template = MonoSingleton<UIManager>.Instance.FollowerFormsMenuTemplate;
        if (template == null)
        {
            Log.LogWarning("FollowerFormsMenuTemplate is not loaded.");
            return;
        }

        PauseGame();

        var menu = template.Instantiate();
        _activeMenu = menu;
        menu.Show();
        menu.OnHidden += ResumeGame;
    }

    private static void OpenBuildMenu()
    {
        var template = MonoSingleton<UIManager>.Instance.BuildMenuTemplate;
        if (template == null)
        {
            Log.LogWarning("BuildMenuTemplate is not loaded.");
            return;
        }

        PauseGame();

        var menu = template.Instantiate();
        _activeMenu = menu;
        menu.Show();

        // Hide accept/confirm button prompt
        menu._controlPrompts.HideAcceptButton();

        // Destroy the original edit buildings text, then replace reference with
        // dummy so Update()'s per-frame SetActive() call doesn't NRE
        UnityEngine.Object.Destroy(menu._editBuildingsText);
        menu._editBuildingsText = new GameObject("Dummy");
        menu._editBuildingsText.SetActive(false);

        // Change title from "Build" to localized "Structures"
        foreach (var loc in menu.GetComponentsInChildren<Localize>(true))
        {
            if (loc._text == null || loc.gameObject.name != "Header Text") continue;
            loc.enabled = false;
            loc._text.text = Localization.Structures;
            break;
        }

        menu.OnHidden += ResumeGame;
    }

    private static void OpenTailorMenu()
    {
        // var tailors = StructureManager.GetAllStructuresOfType<Structures_Tailor>();
        // if (tailors == null || tailors.Count == 0)
        // {
        //     Log.LogInfo("No tailor building found. Build a tailor first.");
        //     return;
        // }

        var tailors = StructureManager.GetAllStructuresOfType<Structures_Tailor>();
        var tailor = tailors is { Count: > 0 } ? tailors[0] : null;

        var template = MonoSingleton<UIManager>.Instance.TailorMenuControllerTemplate;
        if (template == null)
        {
            Log.LogWarning("TailorMenuControllerTemplate is not loaded.");
            return;
        }

        PauseGame();

        var menu = template.Instantiate();
        _activeMenu = menu;

        // Force Customize tab as default before Show triggers ShowDefault()
        menu.tabNavigator.DefaultTabIndex = 1;

        menu.Show(tailor);

        // Hide all tab buttons and navigation arrows
        var tabs = menu.tabNavigator._tabs;
        tabs[0].gameObject.SetActive(false);
        tabs[1].gameObject.SetActive(false);
        tabs[2].gameObject.SetActive(false);
        menu.tabNavigator.SetNavigationVisibility(false);

        // Clone the "Header Text" GO from the build menu template as the title
        var buildTemplate = MonoSingleton<UIManager>.Instance.BuildMenuTemplate;
        if (buildTemplate != null)
        {
            foreach (var t in buildTemplate.GetComponentsInChildren<Transform>(true))
            {
                if (t.name != "Header Text") continue;
                var titleClone = UnityEngine.Object.Instantiate(t.gameObject, menu.tabNavigator.transform);
                titleClone.SetActive(true);
                var loc = titleClone.GetComponent<Localize>();
                if (loc != null)
                {
                    loc.enabled = false;
                }
                var tmp = titleClone.GetComponent<TextMeshProUGUI>();
                if (tmp != null)
                {
                    tmp.text = Localization.Clothing;
                }
                break;
            }
        }

        // Hide craft/queue elements
        menu._cookButton.gameObject.SetActive(false);
        menu._cookButtonRectTransform.gameObject.SetActive(false);
        menu._tailorQueue.gameObject.SetActive(false);
        menu.addToQueueButton.SetActive(false);
        menu.removeToQueueButton.SetActive(false);

        // Extend scroll view down into space freed by hidden craft queue
        var scrollRt = menu._scrollView.GetComponent<RectTransform>();
        var offsetMin = scrollRt.offsetMin;
        offsetMin.y = 0f;
        scrollRt.offsetMin = offsetMin;

        menu.OnHidden += ResumeGame;
    }

    private static void OpenPlayerUpgradesMenu()
    {
        var template = MonoSingleton<UIManager>.Instance.PlayerUpgradesMenuTemplate;
        if (template == null)
        {
            Log.LogWarning("PlayerUpgradesMenuTemplate is not loaded.");
            return;
        }

        PauseGame();

        var menu = template.Instantiate();
        _activeMenu = menu;
        menu.Show();

        // Hide doctrine/unlock buttons and ritual content container
        menu._ritualItem.gameObject.SetActive(false);
        menu._ritualItemAlert.SetActive(false);
        menu._crystalDoctrineItem.gameObject.SetActive(false);
        menu._crystalDoctrineItemAlert.SetActive(false);
        foreach (var t in menu.GetComponentsInChildren<Transform>(true))
        {
            if (t.name != "Ritual Content") continue;
            t.gameObject.SetActive(false);
            break;
        }

        // Hide crown abilities section — immediate hide for this frame,
        // then re-hide via OnShow callback because Start() re-enables them next frame
        menu._crownHeader.SetActive(false);
        menu._crownContainer.SetActive(false);
        menu._crownDLCContainer.SetActive(false);
        menu._crownAbilityCount.gameObject.SetActive(false);

        // Destroy the accept button container so game code (FocusCardFleece, FocusCard)
        // can't re-show it — ShowAcceptButton() null-checks before SetActive(true)
        UnityEngine.Object.Destroy(menu._controlPrompts._acceptPromptContainer);

        // Copy the fleece header's localization term to the menu title, then hide the section header
        var fleeceHeaderLoc = menu._fleeceHeader.GetComponentInChildren<Localize>();
        if (fleeceHeaderLoc != null)
        {
            foreach (var loc in menu.GetComponentsInChildren<Localize>(true))
            {
                if (loc.gameObject.name != "Header") continue;
                loc.SetTerm(fleeceHeaderLoc.mTerm);
                break;
            }
        }
        menu._fleeceHeader.SetActive(false);

        menu.OnShow += () =>
        {
            menu._crownHeader.SetActive(false);
            menu._crownContainer.SetActive(false);
            menu._crownDLCContainer.SetActive(false);
            menu._crownAbilityCount.gameObject.SetActive(false);
        };

        menu.OnHidden += ResumeGame;
    }
}

[HarmonyPatch(typeof(FleeceInfoCard), nameof(FleeceInfoCard.Update))]
internal static class FleeceInfoCardPatch
{
    // FleeceInfoCard.Update() uses Time.deltaTime for selectionDelay countdown,
    // which returns 0 when Time.timeScale is 0 (our paused menu state).
    // Prefix subtracts real frame delta so left/right input works while paused.
    [HarmonyPrefix]
    private static void Prefix(FleeceInfoCard __instance)
    {
        if (Time.timeScale == 0f)
        {
            __instance.selectionDelay -= Time.unscaledDeltaTime;
        }
    }
}
