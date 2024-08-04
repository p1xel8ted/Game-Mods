namespace EasyLiving;

[HarmonyPatch]
[HarmonyAfter("devopsdinosaur.sunhaven.continue_button")]
public static class Patches
{
    private const float BaseMoveSpeed = 4.5f;
    private const string SkippingLoadOfLastModifiedSave = "Skipping load of last modified save.";
    private const float RegenerationInterval = 3f; // Example value for regeneration interval
    private const string PlayerFarmScene = "2playerfarm";
    private static GameObject _newButton;
    private readonly static WriteOnce<Vector2> OriginalSize = new();

    private static float _regenerationTimer;

    private static int InstalledDlcCount;
    private static int TotalDlcCount;
    private static bool PlayerReturnedToMenu { get; set; }

    internal static bool SkipAutoLoad { get; set; }

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

    private static string GetGameObjectPath(GameObject obj)
    {
        var path = obj.name;
        var parent = obj.transform.parent;
        while (parent != null)
        {
            path = parent.name + "/" + path;
            parent = parent.parent;
        }

        return path;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(ScrollRect), nameof(ScrollRect.OnEnable))]
    [HarmonyPatch(typeof(ScrollRect), nameof(ScrollRect.LateUpdate))]
    public static void ScrollRect_Initialize(ref ScrollRect __instance)
    {
        if (GetGameObjectPath(__instance.gameObject) != "Player(Clone)/UI_Quests/QuestTracker/Scroll View/") return;

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
        var moneyInfo = GameObject.Find("Player(Clone)/UI_Inventory/Inventory/Items/Money");
        if (moneyInfo)
        {
            moneyInfo.SetActive(true);
            Plugin.LOG.LogInfo("Money info is now visible.");
        }

        var characterBorder = GameObject.Find("Player(Clone)/UI_Inventory/Inventory/Items/Slots/CharacterPanel/Border");
        if (characterBorder)
        {
            characterBorder.SetActive(true);
            Plugin.LOG.LogInfo("Character border is now visible.");
        }


        if (!Plugin.AddQuitToDesktopButton.Value)
        {
            if (_newButton)
            {
                Object.Destroy(_newButton);
            }

            return;
        }

        var exitButton = GameObject.Find("Player(Clone)/UI_Inventory/Inventory/ExitButton");
        if (!exitButton || _newButton) return;

        _newButton = Object.Instantiate(exitButton, exitButton.transform.parent);
        _newButton.name = "ExitToDesktopButton";
        var pop = _newButton.AddComponent<Popup>();
        pop.name = "ExitToDesktopPop";
        pop.description = "Save progress and exit directly to the desktop.";
        pop.text = "Exit to Desktop";

        _newButton.GetComponent<Button>().onClick.RemoveAllListeners();
        _newButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            NotificationStack.Instance.SendNotification("Game Saved! Exiting...");
            SingletonBehaviour<GameSave>.Instance.SaveGame();
            GC.Collect();
            Application.Quit();
        });


        var rectTransform = _newButton.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(265.5f, -147.5f);
        rectTransform.anchoredPosition3D = new Vector3(265.5f, -147.5f, 1);
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
    }

    private static void UpdateDlcData()
    {
        InstalledDlcCount = 0;
        TotalDlcCount = 0;

        var scroller = GameObject.Find("Canvas_Home/[HomeMenu]/DLCBox/DLC_Scroller");
        if (!scroller) return;

        var sc = scroller.GetComponent<Scroller>();
        if (!sc) return;

        foreach (var data in sc.scrollerDatas)
        {
            if (data is not DLCScrollerData dlc) continue;

            TotalDlcCount += dlc.steamPackIDs.Count;
            InstalledDlcCount += dlc.steamPackIDs.Count(id => SteamApps.BIsDlcInstalled(new AppId_t {m_AppId = id}));
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
        var patchNotesBox = GameObject.Find("Canvas_Home/[HomeMenu]/PatchNotesBox");
        if (patchNotesBox)
        {
            patchNotesBox.SetActive(!Plugin.RemovePatchNotes.Value);
        }

        Canvas.ForceUpdateCanvases();
    }
    private static void UpdateDlcBox()
    {
        var dlcBox = GameObject.Find("Canvas_Home/[HomeMenu]/DLCBox");
        if (!dlcBox) return;

        UpdateDlcData();

        if (InstalledDlcCount >= TotalDlcCount)
        {
            dlcBox.SetActive(false);
        }
        else
        {
            dlcBox.SetActive(!Plugin.RemoveDlcAds.Value);
        }

        Canvas.ForceUpdateCanvases();
    }
    private static void UpdateSocialMedia()
    {
        var socialMedia = GameObject.Find("Canvas_Home/[HomeMenu]/PlayButtons/SocialMediaButtons");
        if (socialMedia)
        {
            socialMedia.SetActive(!Plugin.RemoveSocialMediaButtons.Value);
        }

        Canvas.ForceUpdateCanvases();
    }
    private static void UpdateLogo()
    {
        var ppStudios = GameObject.Find("Canvas_Home/[HomeMenu]/Image");
        if (ppStudios)
        {
            ppStudios.SetActive(!Plugin.RemovePixelSproutStudiosLogo.Value);
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

            var isInstalled = SteamApps.BIsDlcInstalled(new AppId_t {m_AppId = steamID});

            text.alpha = isInstalled ? 0.50f : 1f;
            foreach (var image in images)
            {
                image.color = image.color with {a = isInstalled ? 0.25f : 1f};
            }
            button.interactable = !isInstalled;
        }
    }

    [HarmonyPostfix]
    [HarmonyAfter("devopsdinosaur.sunhaven.continue_button")]
    [HarmonyPatch(typeof(MainMenuController), nameof(MainMenuController.HomeMenu))]
    [HarmonyPatch(typeof(MainMenuController), nameof(MainMenuController.Start))]
    [HarmonyPatch(typeof(MainMenuController), nameof(MainMenuController.SetBackgroundBlur))]
    [HarmonyPatch(typeof(MainMenuController), nameof(MainMenuController.SetupButtons))]
    public static void MainMenuController_HomeMenu(MainMenuController __instance)
    {
        UpdateMainMenu();
        
        if (__instance.homeMenu.GetComponent<MenuUpdater>()) return;
        __instance.homeMenu.AddComponent<MenuUpdater>();
    }
}