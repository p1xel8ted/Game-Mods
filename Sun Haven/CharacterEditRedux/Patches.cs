namespace CharacterEditRedux;

[Harmony]
public static class Patches
{
    private static string _lastName;
    private static string _lastFileName;
    internal static Sprite MouseOverImage;
    internal static Sprite DefaultImage;
    private static MainMenuController MainMenuController => MainMenuController.Instance;
    private static GameSave GameSave => SingletonBehaviour<GameSave>.Instance;
    private static CharacterClothingStyles CharacterClothingStyles => SingletonBehaviour<CharacterClothingStyles>.Instance;
    private static NewCharacterCreator NewCharacterCreator => MainMenuController.newCharacterCreator;
    private static CharacterData CurrentSaveCharacterData => GameSave.CurrentSave.characterData;
    private static CharacterData CurrentCharacterData => NewCharacterCreator.CurrentCharacter;


    private static void BackUpData(CharacterData character)
    {
        var saves = GameSave.Saves;
        var saveData = saves.FirstOrDefault(a => a.characterData == character);
        if (saveData == null)
        {
            Plugin.Log.LogWarning($"No save data found for character {character.characterName}");
            return;
        }

        var path = Path.Combine(Application.persistentDataPath, "Saves", saveData.fileName);
        if (!File.Exists(path))
        {
            Plugin.Log.LogWarning($"Save file not found at path: {path}");
            return;
        }

        var culture = CultureInfo.CurrentCulture;
        var dtf = culture.DateTimeFormat;
        var safeDatePattern = dtf.ShortDatePattern.Replace("/", "-").Replace(".", "-");
        var safeTimePattern = dtf.LongTimePattern.Replace(":", "-");
        var fileTimestampFormat = $"{safeDatePattern} ({safeTimePattern})";
        var timestamp = DateTime.Now.ToString(fileTimestampFormat, culture);
        var backupDir = Path.Combine(Application.persistentDataPath, "Saves", "CE_Backups");
        if (!Directory.Exists(backupDir))
        {
            Directory.CreateDirectory(backupDir);
        }

        var fileNameOnly = Path.GetFileName(path);
        var newName = Path.Combine(backupDir, $"{fileNameOnly}-{timestamp}.bak");
        Plugin.Log.LogInfo($"Backing up save: {saveData.characterData.characterName} -> {newName}");
        File.Copy(path, newName, true);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(SavePanel), nameof(SavePanel.SetPlayerImage))]
    private static void SavePanel_SetPlayerImage(SavePanel __instance, CharacterData character)
    {
        AddEditButton(__instance, character);
    }

    private static void AddEditButton(SavePanel savePanel, CharacterData character)
    {
        var saves = GameSave.Saves;
        var index = saves.FindIndex(s => s.characterData == character);
        if (index < 0) return;

        _lastFileName = saves[index].fileName;

        var buttonObject = Object.Instantiate(savePanel.deleteButton.gameObject, savePanel.deleteButton.transform.parent);
        buttonObject.name = "CharacterEditRedux";

        var rect = buttonObject.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(20, -65);

        var wishButton = buttonObject.GetComponent<UIButton>();
        wishButton.defaultImage = DefaultImage;
        wishButton.hoverOverImage = MouseOverImage;
        wishButton.pressedImage = MouseOverImage;

        var button = buttonObject.GetComponent<Button>();
        button.image.sprite = DefaultImage;

        button.onClick = new Button.ButtonClickedEvent();
        button.onClick.AddListener(delegate
        {
            BackUpData(character);
            GameSave.LoadCharacter(index);
            NewCharacterCreator.CurrentCharacter = GameSave.CurrentSave.characterData;
            MainMenuController.EnableMenu(MainMenuController.newCharacterMenu);
            SetupCharacter();
            MainMenuController.backCharacterButton.onClick.AddListener(SetupButtons);
            MainMenuController.confirmCharacterButton.onClick.RemoveAllListeners();
            MainMenuController.confirmCharacterButton.onClick.AddListener(ConfirmCharacterEdit);
        });
    }

    private static void ConfirmCharacterEdit()
    {
        var currentClothingDictionary = NewCharacterCreator.currentClothingDictionary;

        foreach (var clothingLayer in (ClothingLayer[]) Enum.GetValues(typeof(ClothingLayer)))
        {
            var style = CharacterClothingStyles.GetStyle(clothingLayer, CurrentCharacterData.styleData[(byte) clothingLayer]);
            if (style)
            {
                currentClothingDictionary[clothingLayer] = style;
                NewCharacterCreator.SetClothingLayerData(style, clothingLayer);
            }
        }

        foreach (var keyValuePair in currentClothingDictionary.ToList())
        {
            if (!keyValuePair.Value.armorData)
                continue;

            var key = keyValuePair.Key;
            if (key <= ClothingLayer.Back)
            {
                if (key != ClothingLayer.FrontGloves && key != ClothingLayer.Back)
                    continue;
            }
            else if (key != ClothingLayer.Hat && key != ClothingLayer.Chest && key != ClothingLayer.Pants)
            {
                continue;
            }

            var vanityIndex = PlayerInventory.GetVanityIndexByArmorType(keyValuePair.Value.armorData.armorType);
            CurrentCharacterData.Items[(short) vanityIndex] = new InventoryItemData
            {
                Amount = 1,
                Item = keyValuePair.Value.armorData.GenerateArmorItem()
            };
            var clothingLayerIdx = keyValuePair.Value.clothingLayers[0];
            NewCharacterCreator.SetClothingLayerData(NewCharacterCreator.defaultLayers[clothingLayerIdx], clothingLayerIdx);
        }

        GameSave.WriteCharacterToFile();

        if (CurrentCharacterData.characterName != _lastName && !string.IsNullOrEmpty(_lastFileName))
        {
            var oldPath = Path.Combine(Application.persistentDataPath, "Saves", _lastFileName);
            if (File.Exists(oldPath))
            {
                Plugin.Log.LogInfo($"Character renamed, deleting old save file: {_lastFileName}");
                File.Delete(oldPath);
            }

            GameSave.LoadAllCharacters();
        }

        MainMenuController.EnableMenu(MainMenuController.homeMenu);
        MainMenuController.SetupButtons();
    }

    private static void SetupButtons()
    {
        MainMenuController.SetupButtons();
        MainMenuController.backCharacterButton.onClick.RemoveListener(SetupButtons);
    }

    private static void SetupCharacter()
    {
        _lastName = CurrentCharacterData.characterName;
        NewCharacterCreator.nameInputField.text = _lastName;

        if (!CurrentCharacterData.male)
        {
            NewCharacterCreator.SetFemale();
        }
        else
        {
            NewCharacterCreator.SetMale();
        }

        var currentClothingDictionary = NewCharacterCreator.currentClothingDictionary;

        foreach (var layer in (ClothingLayer[]) Enum.GetValues(typeof(ClothingLayer)))
        {
            var style = CharacterClothingStyles.GetStyle(layer, CurrentSaveCharacterData.styleData[(byte) layer]);

            if (!style) continue;

            currentClothingDictionary[layer] = style;
            NewCharacterCreator.SetClothingLayerData(style, layer);
        }

        foreach (var armorType in (ArmorType[]) Enum.GetValues(typeof(ArmorType)))
        {
            var vanityIndex = PlayerInventory.GetVanityIndexByArmorType(armorType);
            var item = CurrentCharacterData.Items[(short) vanityIndex]?.Item;
            if (item is ArmorItem)
            {
                var itemData = Utils.GetItemData<ArmorData>(item.ID());
                if (!itemData || !itemData.clothingLayerData) continue;
                foreach (var clothingLayer in itemData.clothingLayerData.clothingLayers)
                {
                    NewCharacterCreator.SetClothingLayerData(itemData.clothingLayerData, clothingLayer);
                }
            }
        }

        NewCharacterCreator.UpdateStartingItems();
    }
}
