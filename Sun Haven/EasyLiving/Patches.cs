namespace EasyLiving;

[HarmonyPatch]
[HarmonyAfter("devopsdinosaur.sunhaven.continue_button")]
public static class Patches
{
    private const float BaseMoveSpeed = 4.5f;
    private const string SkippingLoadOfLastModifiedSave = "Skipping load of last modified save.";
    private const float RegenerationInterval = 3f; // Example value for regeneration interval
    private const string PlayerFarmScene = "2playerfarm";

    internal const string DevOpsContinue = "devopsdinosaur.sunhaven.continue_button";
    internal static GameObject QuitToDesktopButton { get; set; }

    private static readonly WriteOnce<Vector2> OriginalSize = new();

    private static float _regenerationTimer;

    private static int _installedDlcCount;
    private static int _totalDlcCount;
    private static bool PlayerReturnedToMenu { get; set; }

    internal static bool SkipAutoLoad { get; set; }


    private static GameObject ContinueButton { get; set; }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(UIHandler), nameof(UIHandler.ExitGame))]
    [HarmonyPatch(typeof(UIHandler), nameof(UIHandler.ExitGameImmediate))]
    public static void UIHandler_ExitGame()
    {
        PlayerReturnedToMenu = true;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(MainMenuController), nameof(MainMenuController.PlayGame), [])]
    [HarmonyPatch(typeof(MainMenuController), nameof(MainMenuController.PlayGame), typeof(int))]
    public static void MainMenuController_PlayGame()
    {
        PlayerReturnedToMenu = false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Player), nameof(Player.FixedUpdateOLD))]
    public static void Player_FixedUpdate(ref Player __instance)
    {
        if (__instance == null || Player.Instance == null) return;
        ApplyMoveSpeed(__instance);
        RegenerateHealthAndMana(__instance);
    }

    private static void ApplyMoveSpeed(Player player)
    {
        if (player == null || Player.Instance == null) return;
        if (!Plugin.ApplyMoveSpeedMultiplier.Value) return;
        var newSpeed = BaseMoveSpeed * Plugin.MoveSpeedMultiplier.Value;
        if (!Mathf.Approximately(player.moveSpeed, newSpeed))
        {
            player.moveSpeed = newSpeed;
        }
    }

    private static void RegenerateHealthAndMana(Player player)
    {
        if (player == null || Player.Instance == null) return;
        _regenerationTimer += Time.deltaTime;
        if (!(_regenerationTimer >= RegenerationInterval)) return;
        _regenerationTimer = 0f;
        player.Health = RegenerateStat(player.Health, player.MaxHealth, 1f);
        player.Mana = RegenerateStat(player.Mana, player.MaxMana, 1f);
    }

    private static float RegenerateStat(float currentStat, float maxStat, float regenerationAmount)
    {
        return currentStat < maxStat ? currentStat + regenerationAmount : currentStat;
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(Player), nameof(Player.MoveSpeed), MethodType.Getter)]
    public static void Player_MoveSpeed(ref Player __instance, ref float __result)
    {
        if (__instance == null || Player.Instance == null) return;
        if (!Plugin.ApplyMoveSpeedMultiplier.Value) return;
        __result = BaseMoveSpeed * Plugin.MoveSpeedMultiplier.Value;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(CraftingTable), nameof(CraftingTable.Interact))]
    public static void CraftingPanel_Interact(ref CraftingTable __instance)
    {
        if (!Plugin.MaterialsOnlyDefault.Value) return;
        __instance.craftingUI.sortHasMats.isOn = true;
        __instance.hasMats = true;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(MainMenuController), nameof(MainMenuController.Start))]
    public static void GameSave_LoadAllCharacters(ref MainMenuController __instance)
    {
        if (!Plugin.AutoLoadMostRecentSave.Value) return;
        if (PlayerReturnedToMenu) return;
        if (SkipAutoLoad)
        {
            Plugin.LOG.LogWarning(SkippingLoadOfLastModifiedSave);
            return;
        }

        var saves = SingletonBehaviour<GameSave>.Instance.Saves.OrderByDescending(save => save.worldData.saveTime).ToList();
        var lastModifiedSave = saves.FirstOrDefault();
        if (lastModifiedSave != null)
        {
            __instance.PlayGame(lastModifiedSave.characterData.characterIndex);
        }
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(ScrollRect), nameof(ScrollRect.OnEnable))]
    [HarmonyPatch(typeof(ScrollRect), nameof(ScrollRect.LateUpdate))]
    public static void ScrollRect_Initialize(ref ScrollRect __instance)
    {
        // Plugin.LOG.LogWarning($"ScrollRect: {__instance.name}, {GetGameObjectPath(__instance.gameObject)}");

        if (__instance.gameObject.GetGameObjectPath() != "Player(Clone)/UI_Quests/QuestTracker/Scroll View") return;


        if (!Plugin.EnableAdjustQuestTrackerHeightView.Value)
        {
            if (OriginalSize.Value != Vector2.zero)
            {
                __instance.viewport.GetComponent<RectTransform>().sizeDelta = OriginalSize.Value;
            }

            return;
        }

        if (OriginalSize.Value == Vector2.zero)
        {
            OriginalSize.Value = __instance.viewport.GetComponent<RectTransform>().sizeDelta;
        }

        var viewport = __instance.viewport.GetComponent<RectTransform>();
        var viewportHeight = Mathf.RoundToInt(Plugin.AdjustQuestTrackerHeightView.Value);
        viewport.sizeDelta = new Vector2(viewport.sizeDelta.x, viewportHeight);
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(DialogueController), nameof(DialogueController.PushDialogue), typeof(DialogueNode), typeof(UnityAction), typeof(bool), typeof(bool))]
    public static bool DialogueController_PushDialogue(ref DialogueNode dialogue, ref UnityAction onComplete)
    {
        if (!Plugin.SkipMuseumMissingItemsDialogue.Value) return true;

        if (dialogue.dialogueText.Any(str => str.Contains("museum bundle") && str.Contains("missing items")))
        {
            onComplete?.Invoke();
            return false;
        }

        return true;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(ScenePortalManager), nameof(ScenePortalManager.StartGame))]
    public static void ScenePortalManager_StartGame(ref Vector2 location, ref string sceneToLoad)
    {
        if (!Plugin.PlayerStartsAwayFromMailbox.Value) return;
        if (sceneToLoad.Equals(PlayerFarmScene))
        {
            location = new Vector2(357.3f, 124.2f);
        }
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(PlayerInventory), nameof(PlayerInventory.OpenMajorPanel))]
    [HarmonyPatch(typeof(PlayerInventory), nameof(PlayerInventory.LoadPlayerInventory))]
    [HarmonyPatch(typeof(PlayerSettings), nameof(PlayerSettings.OnEnable))]
    public static void PlayerSettings_OnEnable()
    {
        var moneyInfo = Player.Instance.PlayerInventory._panels.Find(a => a.name == "Items").Find("Money");
        if (moneyInfo)
        {
            moneyInfo.gameObject.SetActive(true);
            //Plugin.LOG.LogInfo("Money info is now visible.");
        }


        var characterBorder = Player.Instance.PlayerInventory.equipmentPanel.transform.Find("Border");
        if (characterBorder)
        {
            characterBorder.gameObject.SetActive(true);
            //Plugin.LOG.LogInfo("Character border is now visible.");
        }


        if (!Plugin.AddQuitToDesktopButton.Value)
        {
            if (QuitToDesktopButton)
            {
                Object.Destroy(QuitToDesktopButton);
            }

            return;
        }

        var exitButton = Player.Instance.PlayerInventory._panels.Find(a => a.name == "Items").parent.Find("ExitButton");
    
        if (!exitButton || QuitToDesktopButton) return;

        QuitToDesktopButton = Object.Instantiate(exitButton.gameObject, exitButton.transform.parent);
        QuitToDesktopButton.name = "ExitToDesktopButton";

        var pop = QuitToDesktopButton.AddComponent<Popup>();
        pop.name = "ExitToDesktopPop";
        pop.description = "Save progress and exit directly to the desktop.";
        pop.text = "Exit to Desktop";

        QuitToDesktopButton.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
        QuitToDesktopButton.GetComponent<Button>().onClick.RemoveAllListeners();
        QuitToDesktopButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            NotificationStack.Instance.SendNotification("Game Saved! Exiting...");
            SingletonBehaviour<GameSave>.Instance.SaveGame();
            GC.Collect();
            Application.Quit();
        });

        var canvasGroup = QuitToDesktopButton.GetComponent<CanvasGroup>();
        if (canvasGroup)
        {
            canvasGroup.enabled = false;
        }

        var rectTransform = QuitToDesktopButton.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(265.5f, -147.5f);
        rectTransform.anchoredPosition3D = new Vector3(265.5f, -147.5f, 1);
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
    }

    private static void UpdateDlcData()
    {
        _installedDlcCount = 0;
        _totalDlcCount = 0;

        var scroller = MainMenuController.Instance.PCHomeMenu.transform.FindFirstChildByName("DLC_Scroller");
        // var scroller = GameObject.Find("Canvas_Home/[HomeMenu]/[PCHomeMenu]/DLCBox/DLC_Scroller");
        if (!scroller) return;

        var sc = scroller.GetComponent<Scroller>();
        if (!sc) return;

        foreach (var data in sc.scrollerDatas)
        {
            if (data is not DLCScrollerData dlc) continue;

            _totalDlcCount += dlc.steamPackIDs.Count;
            _installedDlcCount += dlc.steamPackIDs.Count(id => SteamApps.BIsDlcInstalled(new AppId_t { m_AppId = id }));
        }

        Canvas.ForceUpdateCanvases();
    }

    internal static void UpdateMainMenu()
    {
        UpdateDlcData();

        UpdateLogo();

        UpdateSocialMedia();

        UpdateDlcBox();

        UpdatePatchNotes();
    }

    private static void UpdatePatchNotes()
    {
        var patchNotesBox = MainMenuController.Instance.PCHomeMenu.transform.FindFirstChildByName("PatchNotesBox");
        // var patchNotesBox = GameObject.Find("Canvas_Home/[HomeMenu]/[PCHomeMenu]/PatchNotesBox");
        if (patchNotesBox)
        {
            patchNotesBox.gameObject.SetActive(!Plugin.RemovePatchNotes.Value);
        }

        Canvas.ForceUpdateCanvases();
    }

    private static void UpdateDlcBox()
    {
        var dlcBox = MainMenuController.Instance.PCHomeMenu.transform.FindFirstChildByName("DLCBox");
        //var dlcBox = GameObject.Find("Canvas_Home/[HomeMenu]/[PCHomeMenu]/DLCBox");
        if (!dlcBox) return;

        UpdateDlcData();

        if (_installedDlcCount >= _totalDlcCount)
        {
            dlcBox.gameObject.SetActive(false);
        }
        else
        {
            dlcBox.gameObject.SetActive(!Plugin.RemoveDlcAds.Value);
        }

        Canvas.ForceUpdateCanvases();
    }

    private static void UpdateSocialMedia()
    {
        var socialMedia = MainMenuController.Instance.PCHomeMenu.transform.FindFirstChildByName("SocialMediaButtons");
        // var socialMedia = GameObject.Find("Canvas_Home/[HomeMenu]/[PCHomeMenu]/PlayButtons/SocialMediaButtons");
        if (socialMedia)
        {
            socialMedia.gameObject.SetActive(!Plugin.RemoveSocialMediaButtons.Value);
        }

        Canvas.ForceUpdateCanvases();
    }

    private static void UpdateLogo()
    {
        var ppStudios = MainMenuController.Instance.PCHomeMenu.transform.Find("Image");
        // var ppStudios = GameObject.Find("Canvas_Home/[HomeMenu]/[PCHomeMenu]/Image");
        if (ppStudios)
        {
            ppStudios.gameObject.SetActive(!Plugin.RemovePixelSproutStudiosLogo.Value);
        }

        Canvas.ForceUpdateCanvases();
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(DLCScrollerData), nameof(DLCScrollerData.Setup))]
    public static void DLCScrollerData_Setup(ref DLCScrollerData __instance, ref Scroller scroller)
    {
        for (var i = 0; i < __instance.steamPackIDs.Count; i++)
        {
            var steamID = __instance.steamPackIDs[i];
            var component = scroller.gameObjects[i];
            var images = component.GetComponentsInChildren<Image>();
            var text = component.GetComponentInChildren<TextMeshProUGUI>();
            var button = component.GetComponent<Button>();

            var isInstalled = SteamApps.BIsDlcInstalled(new AppId_t { m_AppId = steamID });

            text.alpha = isInstalled ? 0.50f : 1f;
            foreach (var image in images)
            {
                image.color = image.color with { a = isInstalled ? 0.25f : 1f };
            }

            button.interactable = !isInstalled;
        }
    }

    private static MenuUpdater MenuUpdaterInstance { get; set; }

    [HarmonyPostfix]
    [HarmonyAfter(DevOpsContinue)]
    [HarmonyPatch(typeof(MainMenuController), nameof(MainMenuController.HomeMenu))]
    [HarmonyPatch(typeof(MainMenuController), nameof(MainMenuController.Start))]
    [HarmonyPatch(typeof(MainMenuController), nameof(MainMenuController.SetBackgroundBlur))]
    [HarmonyPatch(typeof(MainMenuController), nameof(MainMenuController.SetupButtons))]
    public static void MainMenuController_HomeMenuPatches(MainMenuController __instance)
    {
        UpdateMainMenu();
        MenuUpdaterInstance = __instance.homeMenu.TryAddComponent<MenuUpdater>();
    }


    [HarmonyPostfix]
    [HarmonyAfter(DevOpsContinue)]
    [HarmonyPatch(typeof(MainMenuController), nameof(MainMenuController.HomeMenu))]
    public static void MainMenuController_HomeMenu(MainMenuController __instance)
    {
        if (!Plugin.QuickContinueButton.Value) return;

        if (Utils.DevOpsContinueExists(DevOpsContinue))
        {
            Plugin.LOG.LogWarning($"Detected DevOpsDinosaurs Continue mod, aborting creating continue button!");
        }
        else
        {
            CreateContinueButton(__instance);
        }
    }

    private static string GetLocalizedContinue()
    {
        var language = LocalizationManager.CurrentLanguageCode;
        return language switch
        {
            "da" => "Fortsæt",
            "de" => "Fortsetzen",
            "en" => "Continue",
            "es" => "Continuar",
            "fr" => "Continuer",
            "it" => "Continua",
            "ja" => "続行",
            "ko" => "계속",
            "nl" => "Doorgaan",
            "pt" => "Continuar",
            "pt-BR" => "Continuar",
            "ru" => "Продолжить",
            "sv" => "Fortsätt",
            "uk" => "Продовжити",
            "zh-CN" => "继续",
            "zh-TW" => "繼續",
            _ => "Continue"
        };
    }

    // NOTE: MainMenuLoader class was removed in a game update - Quick Boot feature disabled
    // [HarmonyPostfix]
    // [HarmonyPatch(typeof(MainMenuLoader), nameof(MainMenuLoader.MainLoadingRoutine))]
    // private static void MaineMenuLoader_MainLoadingRoutine(ref IEnumerator __result)
    // {
    //     if (!Plugin.QuickBoot.Value) return;
    //     __result = Utils.FilterByMethodName(__result, "RunLogoAnimationRoutine");
    // }

    internal static void CreateContinueButton(MainMenuController mmc)
    {
        if (Utils.DevOpsContinueExists(DevOpsContinue)) return;

        if (!Plugin.QuickContinueButton.Value)
        {
            if (!ContinueButton) return;

            Object.Destroy(ContinueButton);
            if (MenuUpdaterInstance)
            {
                MenuUpdaterInstance.SetContinueNull();
            }

            return;
        }

        if (ContinueButton) return;

        var savesDirectory = Path.Combine(Application.persistentDataPath, "Saves");

        var playButtonParent = mmc.PCHomeMenu.transform.Find("PlayButtons");

        if (playButtonParent)
        {
            var playButton = playButtonParent.Find("PlayButton");

            var lastModifiedSave = SingletonBehaviour<GameSave>.Instance.Saves
                .OrderByDescending(save => File.GetLastWriteTime(Path.Combine(savesDirectory, save.fileName)))
                .FirstOrDefault();

            if (lastModifiedSave != null && playButton)
            {
                var characterIndex = SingletonBehaviour<GameSave>.Instance.Saves.IndexOf(lastModifiedSave);
                var menuRect = playButtonParent.GetComponent<RectTransform>();
                menuRect.sizeDelta = new Vector2(menuRect.sizeDelta.x, 310);

                ContinueButton = Object.Instantiate(playButton.gameObject, playButtonParent);
                ContinueButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = GetLocalizedContinue();
                ContinueButton.transform.SetAsFirstSibling();
                ContinueButton.name = "ContinueButton";

                Object.Destroy(ContinueButton.transform.GetChild(0)?.gameObject.GetComponent<Localize>());

                ContinueButton.GetComponent<Button>().onClick.AddListener(delegate { mmc.PlayGame(characterIndex); });
            }
        }
    }
}