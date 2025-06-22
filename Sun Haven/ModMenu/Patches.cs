namespace ModMenu;

[HarmonyPatch]
public static class Patches
{

    private const string ConfigurationManager = "Configuration Manager";
    private const string UnityExplorer = "UnityExplorer";
    private const string SeparatorImage = "SeparatorImage";
    private static Transform _modMenuView, _audioView, _audioButton;
    private static readonly Color TitleColor = new(0.660f, 0.830f, 0f, 1f);
    private static readonly Color SectionColor = new(1, 0.8f, 0, 1);

    private static Transform ModHeaderTemplate { get; set; }
    private static Transform ButtonTemplate { get; set; }
    private static Transform DropDownTemplate { get; set; }
    private static Transform SliderTemplate { get; set; }
    private static Transform ToggleTemplate { get; set; }
    private static Transform InputBoxTemplate { get; set; }
    private static Transform ModViewContent { get; set; }

    private static List<TextMeshProUGUI> DescriptionTexts { get; } = [];


    private static TextMeshProUGUI DescriptionTextTemplate { get; set; }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(SettingsUI), nameof(SettingsUI.Start))]
    public static void SettingsUI_Start(SettingsUI __instance)
    {
        _audioButton = __instance.transform.Find("AudioButton");
        CreateModMenuButton(__instance);
        UpdateExistingUIElements(__instance);
    }

    private static void CreateModMenuButton(SettingsUI instance)
    {
        ModMenuButton = Object.Instantiate(_audioButton, _audioButton.parent);
        ConfigureButton(ModMenuButton, instance);
        AddButtonToUI(instance, ModMenuButton);
    }

    
    private static void ConfigureButton(Component newButton, SettingsUI instance)
    {
        newButton.name = "ModsButton";
        newButton.transform.SetSiblingIndex(_audioButton.GetSiblingIndex() + 1);
        var buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = "Mod Menu";

        var buttonComponent = newButton.GetComponent<Button>();
        buttonComponent.onClick.RemoveAllListeners();
        buttonComponent.onClick.AddListener(() => OpenModMenu(instance));

        // var audioButtonNav = _audioButton.GetComponent<NavigationElement>();
        // var modButtonName = newButton.GetComponent<NavigationElement>();
        // audioButtonNav.right = modButtonName;
        // modButtonName.left = audioButtonNav;
    }

    private static Transform ModMenuButton { get; set; }

    private static void OpenModMenu(SettingsUI instance)
    {
        instance.EnablePanel(_modMenuView);
        UpdateWordWrapping();
        
    }


    private static void UpdateExistingUIElements(SettingsUI instance)
    {
        var newButtonImage = instance.buttons.Last().GetComponent<Image>();
        instance.buttons = instance.buttons.Append(newButtonImage).ToArray();
        instance.panels = instance.panels.Append(_modMenuView.GetComponent<RectTransform>()).ToArray();
        LocateTemplates(instance.panels[0].transform);
    }

    private static Transform CreateModTitle(PluginInfo mod, Transform parentTransform, Color titleColor)
    {
        var newTitle = Object.Instantiate(ModHeaderTemplate, parentTransform);
        Object.Destroy(newTitle.GetChild(1).gameObject); //don't remove

        var modName = mod.Metadata.Name;
        var modVersion = mod.Metadata.Version;
        newTitle.name = $"{modName} Title";


        ConfigureModTitleText(newTitle, $"{modName} v{modVersion}", titleColor);


        ConfigureModTitleVerticalLayout(newTitle);


        RemoveAllPopups(newTitle);

        return newTitle;
    }

    private static void ConfigureModTitleText(Transform titleTransform, string titleText, Color titleColor)
    {
        if (!titleTransform.TryGetComponentInChildren<TextMeshProUGUI>(out var text)) return;

        text.alignment = TextAlignmentOptions.Center;
        text.text = titleText;
        text.color = titleColor;
        text.fontSize = 20;
        text.fontStyle = FontStyles.Normal;
        text.enableWordWrapping = false;
        text.autoSizeTextContainer = true;
        text.margin = new Vector4(0, 0, 0, 0);
    }

    private static void ConfigureModTitleVerticalLayout(Transform titleTransform)
    {
        var vlg = titleTransform.gameObject.AddComponent<VerticalLayoutGroup>();
        vlg.spacing = 0;
        vlg.padding = new RectOffset(0, 0, 0, 5);
    }

    private static void RemoveAllPopups(Component transform)
    {
        var popups = transform.GetComponentsInChildren<Popup>();
        foreach (var popup in popups)
        {
            Object.Destroy(popup);
        }
    }


    // Adds content size fitters to a GameObject for better UI layout management
    private static (VerticalLayoutGroup VLG, ContentSizeFitter CSF) AddContentSizeFitters(GameObject newModEntry)
    {
        var fitter = newModEntry.AddComponent<ContentSizeFitter>();
        fitter.verticalFit = ContentSizeFitter.FitMode.MinSize;
        fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;

        return (newModEntry.AddComponent<VerticalLayoutGroup>(), fitter);
    }

    // Creates a UI entry for a mod in the mod view
    private static void CreateModEntry(PluginInfo mod)
    {
        var newModEntry = new GameObject($"{mod.Metadata.Name} Settings");
        newModEntry.transform.SetParent(ModViewContent, false);

        var fitters = AddContentSizeFitters(newModEntry);
        ConfigureModEntryVerticalLayoutGroup(fitters.VLG);

        var title = CreateModTitle(mod, ModViewContent, TitleColor);
        title.SetParent(newModEntry.transform, true);

        var sections = mod.Instance.Config.Select(a => a.Key.Section).Distinct().OrderBy(section => section);
        foreach (var sectionName in sections)
        {
            CreateSection(mod.Instance.Config, newModEntry, sectionName);
        }

        newModEntry.transform.SetAsLastSibling();
    }


    private static void CreateSection(ConfigFile modConfig, GameObject newModEntry, string section)
    {
       
        var newSectionTransform = Object.Instantiate(ModHeaderTemplate, newModEntry.transform).transform;
        CleanUpSection(newSectionTransform);
        newSectionTransform.name = $"{section} Section";

     
        if (newSectionTransform.TryGetComponentInChildren<TextMeshProUGUI>(out var text))
        {
            ConfigureSectionTitleText(text, section);
        }


        if (newSectionTransform.TryGetComponentInChildren<Popup>(out var popup))
        {
            Object.Destroy(popup);
        }

       
        var fitters = AddContentSizeFitters(newSectionTransform.gameObject);
        fitters.VLG.spacing = 5;

  
        var descriptions = new HashSet<string>();
        foreach (var configEntry in modConfig)
        {
            if (configEntry.Key.Section == section && descriptions.Add(configEntry.Value.Description.Description))
            {
                CreateConfigEntry(configEntry, newSectionTransform);
            }
        }
    }


    private static Transform CreateInputBox(KeyValuePair<ConfigDefinition, ConfigEntryBase> configEntry, Transform newSection)
    {
        var newInputBox = Object.Instantiate(InputBoxTemplate, newSection);

        newInputBox.name = configEntry.Key.Key;

        var newParent = new GameObject($"{configEntry.Key.Key} Input");

        var newDescriptionText = Object.Instantiate(SliderTemplate.Find("DescriptionTMP"), newParent.transform).GetComponentInChildren<TextMeshProUGUI>();
        newDescriptionText.name = $"{configEntry.Key.Key} Description";
        var borderRect = newParent.AddComponent<RectTransform>();

        borderRect.SetParent(newInputBox.transform.parent, true);
        var inputFieldRect = newInputBox.GetComponent<RectTransform>();
        borderRect.sizeDelta = new Vector2(96, 18);
        borderRect.position = inputFieldRect.position;

        inputFieldRect.SetParent(borderRect, true);
        inputFieldRect.sizeDelta = new Vector2(92, 14);
        inputFieldRect.localPosition = Vector3.zero;

        newInputBox.localScale = Vector3.one;
        newParent.transform.localScale = Vector3.one;

        var inputBox = newInputBox.GetComponentInChildren<TMP_InputField>();
        inputBox.name = $"{configEntry.Key.Key} InputBox";
        inputBox.lineType = TMP_InputField.LineType.SingleLine;
        inputBox.verticalScrollbar = null; // Disable the vertical scrollbar
        inputBox.scrollSensitivity = 0; // Set scroll sensitivity to 0
        inputBox.text = configEntry.Value.GetSerializedValue();
        inputBox.onValueChanged.RemoveAllListeners();
        inputBox.onValueChanged.AddListener(value => configEntry.Value.SetSerializedValue(value));

        configEntry.Value.ConfigFile.SettingChanged += (_, _) =>
        {
            inputBox.text = configEntry.Value.GetSerializedValue();
        };

        ConfigureInputBoxDescriptionText(newDescriptionText, configEntry.Key.Key);

        Object.Destroy(newInputBox.FindFirstChildByName("Icon").gameObject);
        Object.Destroy(newInputBox.FindFirstChildByName("Placeholder").gameObject);

        newInputBox.GetComponent<RectTransform>().anchoredPosition = new Vector2(48, 0);
        newParent.GetComponent<RectTransform>().anchoredPosition = new Vector2(245, 0);
        ConfigureTooltip(newParent.transform, configEntry.Key.Key, configEntry.Value.Description.Description, true);
        return newParent.transform;
    }


    private static Transform CreateDropDown(KeyValuePair<ConfigDefinition, ConfigEntryBase> configEntry, Transform newSection, IReadOnlyList<object> acceptableValuesAv)
    {
        var acceptableValuesAvList = acceptableValuesAv.Select(obj => obj.ToString()).ToList();
        var serializedValue = configEntry.Value.GetSerializedValue();
        var newDropDown = Object.Instantiate(DropDownTemplate, newSection);
        Object.Destroy(newDropDown.GetChild(2).gameObject);
        ConfigureTooltip(newDropDown, configEntry.Key.Key, configEntry.Value.Description.Description, true);
        newDropDown.name = configEntry.Key.Key;
        var dropDown = newDropDown.GetComponentInChildren<TMP_Dropdown>();
        dropDown.ClearOptions();
        dropDown.AddOptions(acceptableValuesAvList);
        dropDown.value = acceptableValuesAvList.IndexOf(serializedValue);
        dropDown.onValueChanged.RemoveAllListeners();
        dropDown.onValueChanged.AddListener(value =>
        {
            configEntry.Value.SetSerializedValue(acceptableValuesAv[value].ToString());
        });
        var dropDownText = newDropDown.GetComponentInChildren<TextMeshProUGUI>();
        ConfigureDropDownText(dropDownText, configEntry.Key.Key);
        newDropDown.GetComponent<RectTransform>().anchoredPosition = new Vector2(245, 0);

        configEntry.Value.ConfigFile.SettingChanged += (sender, args) =>
        {
            dropDown.value = acceptableValuesAvList.IndexOf(configEntry.Value.GetSerializedValue());
        };

        return newDropDown;
    }


    private static List<Transform> ConfigEntries { get; set; } = [];

    private static void CreateConfigEntry(KeyValuePair<ConfigDefinition, ConfigEntryBase> configEntry, Transform newSection)
    {
        Plugin.Log.LogWarning($"Creating config entry for {configEntry.Key.Key} - Tags?: {configEntry.Value.Description.Tags?.Length}");
        if (configEntry.Value.Description.Tags?.Length > 0)
        {
            foreach (var tag in configEntry.Value.Description.Tags)
            {
                if (tag is ConfigurationManagerAttributes a)
                {
                    Plugin.Log.LogWarning($"Att {a}");
                }
            }
        }
        if (configEntry.Value.Description.AcceptableValues is null)
        {
            if (configEntry.Value.SettingType == typeof(bool))
            {
                ConfigEntries.Add(CreateToggle(configEntry, newSection));
            }
            else
            {
                if (configEntry.Value.SettingType.IsEnum)
                {
                    var list = Enum.GetValues(configEntry.Value.SettingType);
                    ConfigEntries.Add(CreateDropDown(configEntry, newSection, list.Cast<object>().ToList())); 
                }
                else
                {
                    ConfigEntries.Add(CreateInputBox(configEntry, newSection));
                }
            }
            return;
        }
        var acceptableValues = GetAcceptableValues(configEntry.Value.Description.AcceptableValues);
        if (acceptableValues.AV.Length > 0)
        {
            ConfigEntries.Add(CreateDropDown(configEntry, newSection, acceptableValues.AV));
        }
        else if (acceptableValues.AVR.Key != null)
        {
            ConfigEntries.Add(CreateSlider(configEntry, newSection, acceptableValues.AVR, acceptableValues.ShowRangeAsPercent));
        }
        else
        {
            ConfigEntries.Add(CreateToggle(configEntry, newSection));
        }
    }

    //thanks Marco
    private static (object[] AV, KeyValuePair<object, object> AVR, bool ShowRangeAsPercent) GetAcceptableValues(AcceptableValueBase values)
    {
        var av = new object[] { };
        var avr = new KeyValuePair<object, object>();
        var showRangeAsPercent = false;
        var t = values.GetType();
        var listProp = t.GetProperty(nameof(AcceptableValueList<bool>.AcceptableValues), AccessTools.all);
        if (listProp != null)
        {
            av = ((IEnumerable) listProp.GetValue(values, null)).Cast<object>().ToArray();
        }
        else
        {
            var minProp = t.GetProperty(nameof(AcceptableValueRange<bool>.MinValue), AccessTools.all);
            if (minProp == null) return (av, avr, false);
            var maxProp = t.GetProperty(nameof(AcceptableValueRange<bool>.MaxValue), AccessTools.all);
            if (maxProp == null) throw new ArgumentNullException(nameof(maxProp));
            avr = new KeyValuePair<object, object>(minProp.GetValue(values, null), maxProp.GetValue(values, null));
            showRangeAsPercent = (avr.Key.Equals(0) || avr.Key.Equals(1)) && avr.Value.Equals(100) ||
                                 avr.Key.Equals(0f) && avr.Value.Equals(1f);
        }
        return (av, avr, showRangeAsPercent);
    }

    private static Transform CreateSlider(KeyValuePair<ConfigDefinition, ConfigEntryBase> configEntry, Transform newSection, KeyValuePair<object, object> acceptableValues, bool ShowRangeAsPercent)
    {
        var newSlider = Object.Instantiate(SliderTemplate, newSection);

        Object.Destroy(newSlider.GetChild(2).gameObject);
        ConfigureTooltip(newSlider, configEntry.Key.Key, configEntry.Value.Description.Description, true);
        newSlider.name = configEntry.Key.Key;
        var slider = newSlider.GetComponentInChildren<Slider>();
        slider.transform.name = $"{configEntry.Key.Key} Slider";

        slider.minValue = acceptableValues.Key is int key ? key : (float) Convert.ToDouble(acceptableValues.Key, CultureInfo.InvariantCulture);
        slider.maxValue = acceptableValues.Value is int value ? value : (float) Convert.ToDouble(acceptableValues.Value, CultureInfo.InvariantCulture);


        slider.value = Convert.ToSingle(configEntry.Value.GetSerializedValue());
        var valueText = slider.transform.GetComponentInChildren<TextMeshProUGUI>();
        slider.onValueChanged.RemoveAllListeners();
        slider.onValueChanged.AddListener(newValue =>
        {
            if (configEntry.Value.SettingType == typeof(int))
            {
                newValue = Mathf.RoundToInt(newValue);
                if (ShowRangeAsPercent)
                {
                    var valuePercent = Mathf.Round(100 * Mathf.Abs(newValue - slider.minValue) / Mathf.Abs(slider.minValue - slider.maxValue)) + "%";
                    valueText.text = valuePercent;
                }
                else
                {
                    valueText.text = newValue.ToString(CultureInfo.InvariantCulture);
                }

                configEntry.Value.SetSerializedValue(newValue.ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                newValue = Mathf.Round(newValue * 100f) / 100f;
                if (ShowRangeAsPercent)
                {
                    var valuePercent = Mathf.Round(100 * Mathf.Abs(newValue - slider.minValue) / Mathf.Abs(slider.minValue - slider.maxValue)) + "%";
                    valueText.text = valuePercent;
                }
                else
                {
                    valueText.text = newValue.ToString(CultureInfo.InvariantCulture);
                }
                configEntry.Value.SetSerializedValue(newValue.ToString(CultureInfo.InvariantCulture));
            }
        });

        configEntry.Value.ConfigFile.SettingChanged += (_, _) =>
        {
            slider.value = Convert.ToSingle(configEntry.Value.GetSerializedValue());
        };

        ConfigureText(valueText, configEntry.Value.GetSerializedValue());
        DescriptionTextTemplate = newSlider.GetComponentInChildren<TextMeshProUGUI>();
        ConfigureText(DescriptionTextTemplate, configEntry.Key.Key, true);
        DescriptionTextTemplate.enableWordWrapping = true;
        var sliderLeft = newSlider.FindFirstChildByName("SliderLeft");
        Object.Destroy(sliderLeft.gameObject);
        var sliderRight = newSlider.FindFirstChildByName("SliderRight");
        Object.Destroy(sliderRight.gameObject);
        return newSlider;
    }

    private static void ConfigureText(TextMeshProUGUI text, string keyKey, bool description = false)
    {
        text.alignment = description ? TextAlignmentOptions.MidlineRight : TextAlignmentOptions.MidlineLeft;
        text.text = $"{keyKey}";
        text.fontSize = 17;
        text.fontStyle = FontStyles.Normal;
        text.enableWordWrapping = description;
        text.autoSizeTextContainer = true;
        text.margin = new Vector4(0, 0, 0, 0);
        if (description)
        {
            DescriptionTexts.Add(text);
        }
    }

    private static void ConfigureTooltip(Component item, string title, string content, bool searchChildren = false)
    {
        var popup = searchChildren ? item.GetComponentInChildren<Popup>() : item.GetComponent<Popup>();
        if (popup == null)
        {
            Plugin.Log.LogError($"Could not find popup component on {item.name}");
            return;
        }
        popup.text = title;
        popup.description = content;
    }

    private static Transform CreateToggle(KeyValuePair<ConfigDefinition, ConfigEntryBase> configEntry, Transform newSection)
    {
        var newToggle = Object.Instantiate(ToggleTemplate, newSection);
        Object.Destroy(newToggle.GetChild(2).gameObject);

        ConfigureTooltip(newToggle, configEntry.Key.Key, configEntry.Value.Description.Description, true);

        newToggle.name = configEntry.Key.Key;
        var toggle = newToggle.GetComponentInChildren<Toggle>();
        toggle.isOn = Convert.ToBoolean(configEntry.Value.GetSerializedValue());
        toggle.onValueChanged.RemoveAllListeners();
        toggle.onValueChanged.AddListener(value => configEntry.Value.SetSerializedValue(value.ToString()));
        var toggleText = newToggle.GetComponentInChildren<TextMeshProUGUI>();
        ConfigureText(toggleText, configEntry.Key.Key, true);

        configEntry.Value.ConfigFile.SettingChanged += (_, _) =>
        {
            toggle.isOn = Convert.ToBoolean(configEntry.Value.GetSerializedValue());
        };

        return newToggle;
    }


    private static void CleanUpSection(Transform section)
    {
        Object.Destroy(section.GetChild(1).gameObject);
        Object.Destroy(section.GetChild(2).gameObject);
    }

    private static void ConfigureDropDownText(TextMeshProUGUI dropDownText, string keyKey)
    {
        dropDownText.alignment = TextAlignmentOptions.MidlineRight;
        dropDownText.text = keyKey;
        dropDownText.fontSize = 17;
        dropDownText.fontStyle = FontStyles.Normal;
        dropDownText.enableWordWrapping = true;
        dropDownText.autoSizeTextContainer = true;
        dropDownText.margin = new Vector4(0, 0, 0, 0);
        DescriptionTexts.Add(dropDownText);
    }

    private static void ConfigureInputBoxDescriptionText(TextMeshProUGUI inputBoxText, string keyKey)
    {
        inputBoxText.alignment = TextAlignmentOptions.MidlineRight;
        inputBoxText.text = keyKey;
        inputBoxText.fontSize = 17;
        inputBoxText.lineSpacing = -80;
        inputBoxText.m_maxWidth = 200;
        inputBoxText.fontStyle = FontStyles.Normal;
        inputBoxText.enableWordWrapping = true;
        inputBoxText.autoSizeTextContainer = true;
        inputBoxText.margin = new Vector4(0, 0, 0, 0);
        DescriptionTexts.Add(inputBoxText);
    }

    private static void UpdateWordWrapping()
    {
        foreach (var text in DescriptionTexts)
        {
            text.rectTransform.sizeDelta = new Vector2(150, text.rectTransform.sizeDelta.y);
            text.enableWordWrapping = true;
            text.lineSpacing = -80;
        }
    }


    private static void ConfigureSectionTitleText(TMP_Text text, string section)
    {
        text.alignment = TextAlignmentOptions.Center;
        text.text = section;
        text.color = SectionColor;
        text.fontSize = 17;
        text.fontStyle = FontStyles.Italic;
        text.enableWordWrapping = false;
        text.autoSizeTextContainer = true;
        text.margin = new Vector4(0, 0, 0, 0);
    }

    private static void LocateTemplates(Transform panel)
    {
        Plugin.Log.LogWarning($"Locating templates and constructing mod menu....");
        ModHeaderTemplate = panel.Find("Viewport/Content/Setting_Keybinds");
        ButtonTemplate = panel.Find("Viewport/Content/Setting_Keybinds");
        DropDownTemplate = panel.Find("Viewport/Content/Setting_Language");
        SliderTemplate = panel.Find("Viewport/Content/Setting_DaySpeed");
        ToggleTemplate = panel.Find("Viewport/Content/Setting_Helptips");
        InputBoxTemplate = Resources.FindObjectsOfTypeAll<TMP_InputField>().FirstOrDefault(a => a.name.Equals("SearchInput"))?.transform.parent.transform;


        var loadedMods = BepInEx.Bootstrap.Chainloader.PluginInfos.Select(mod => mod.Value).OrderBy(a => a.Metadata.Name).ToList();
        foreach (var mod in loadedMods.Where(a => a.Instance.Config.Count > 0))
        {
            if (mod.Metadata.Name.Equals(ConfigurationManager) || mod.Metadata.Name.Equals(UnityExplorer)) continue;
            CreateModEntry(mod);
        }

        // foreach (var tt in _modMenuView.GetComponentsInChildren<TranslatedText>())
        // {
        //     Object.Destroy(tt);
        // }

        foreach (var contentFitter in _modMenuView.GetComponentsInChildren<ContentSizeFitter>())
        {
            contentFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            contentFitter.verticalFit = ContentSizeFitter.FitMode.MinSize;
        }

        foreach (var lg in _modMenuView.GetComponentsInChildren<VerticalLayoutGroup>())
        {
            lg.childAlignment = TextAnchor.MiddleCenter;
            lg.childControlHeight = false;
            lg.childControlWidth = true;
            lg.childForceExpandHeight = true;
            lg.childForceExpandWidth = true;
            lg.childScaleHeight = false;
            lg.childScaleWidth = false;
        }

        foreach (var text in _modMenuView.GetComponentsInChildren<TextMeshProUGUI>())
        {
            if (text.enableWordWrapping)
            {
                text.lineSpacing = -75;
            }
        }

        foreach (var sep in _modMenuView.GetComponentsInChildren<Image>())
        {
            if (!sep.name.Equals(SeparatorImage)) continue;
            sep.transform.localScale = new Vector3(0.50f, 1, 1);
        }

    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(PlayerSettings), nameof(PlayerSettings.Start))]
    public static void PlayerSettings_SetupUI(PlayerSettings __instance)
    {
        _audioView = __instance.transform.Find("SettingsScroll View_Audio");
        _modMenuView = Object.Instantiate(_audioView, _audioView.parent);
        _modMenuView.gameObject.SetActive(false);

        ModViewContent = _modMenuView.Find("Viewport/Content");

        Utils.DestroyChildren(ModViewContent);
        _modMenuView.name = "SettingsScroll View_Mods";
        _modMenuView.transform.SetSiblingIndex(_audioView.GetSiblingIndex() + 1);
        _modMenuView.gameObject.SetActive(false);
    }


    private static void ConfigureModEntryVerticalLayoutGroup(HorizontalOrVerticalLayoutGroup layoutGroup)
    {
        layoutGroup.spacing = 10;
        layoutGroup.padding = new RectOffset(0, 0, 0, 5);
    }


    private static void AddButtonToUI(SettingsUI instance, Component newButton)
    {
        var newButtonImage = newButton.GetComponent<Image>();
        var existingButtonImages = instance.buttons.ToList();
        existingButtonImages.Add(newButtonImage);
        instance.buttons = existingButtonImages.ToArray();

        var existingPanels = instance.panels.ToList();
        existingPanels.Add(_modMenuView.GetComponent<RectTransform>());
        instance.panels = existingPanels.ToArray();
    }
}