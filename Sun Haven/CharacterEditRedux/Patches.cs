namespace CharacterEditRedux;

[Harmony]
public static class Patches
{
    private static string _lastName;
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
        Plugin.Log.LogInfo($"[Debug] Entering SavePanel_SetPlayerImage for character: {character.characterName}");
        var saves = GameSave.Saves;
        Plugin.Log.LogInfo($"[Debug] Total saves count: {saves.Count}");
        foreach (var s in saves)
        {
            Plugin.Log.LogInfo($"[Debug] Save entry: Name={s.characterData.characterName}, FileName={s.fileName}");
        }

        var saveData = saves.FirstOrDefault(a => a.characterData == character);
        if (saveData == null)
        {
            Plugin.Log.LogWarning($"[Debug] No saveData found for character {character.characterName}");
        }
        else
        {
            var path = Path.Combine(Application.persistentDataPath, "Saves", saveData.fileName);
            Plugin.Log.LogInfo($"[Debug] Found save file path: {path}");
            if (File.Exists(path))
            {
                Plugin.Log.LogInfo($"[Debug] File exists, preparing backup for: {saveData.fileName}");
                var culture = CultureInfo.CurrentCulture;
                var dtf = culture.DateTimeFormat;
                var safeDatePattern = dtf.ShortDatePattern.Replace("/", "-").Replace(".", "-");
                var safeTimePattern = dtf.LongTimePattern.Replace(":", "-");
                var fileTimestampFormat = $"{safeDatePattern} ({safeTimePattern})";
                var timestamp = DateTime.Now.ToString(fileTimestampFormat, culture);
                var backupDir = Path.Combine(Application.persistentDataPath, "Saves", "CE_Backups");
                if (!Directory.Exists(backupDir))
                {
                    Plugin.Log.LogInfo($"[Debug] Creating backup directory at: {backupDir}");
                    Directory.CreateDirectory(backupDir);
                }

                var fileNameOnly = Path.GetFileName(path);
                var newName = Path.Combine(backupDir, $"{fileNameOnly}-{timestamp}.bak");
                Plugin.Log.LogInfo($"Backing Up Save to Backups folder: {saveData.characterData.characterName} - {saveData.fileName} -> {newName}");
                File.Copy(path, newName, true);
            }
            else
            {
                Plugin.Log.LogWarning($"[Debug] Save file not found at path: {path}");
            }
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(SavePanel), nameof(SavePanel.SetPlayerImage))]
    private static void SavePanel_SetPlayerImage(SavePanel __instance, CharacterData character)
    {
        Plugin.Log.LogInfo("[Debug] Starting FixButtons coroutine");
        MainMenuController.StartCoroutine(FixButtons(__instance, character));
    }

    private static IEnumerator FixButtons(SavePanel savePanel, CharacterData character)
    {
        Plugin.Log.LogInfo($"[Debug] Entering FixButtons for character: {character.characterName}");

        var saves = GameSave.Saves;
        Plugin.Log.LogInfo($"[Debug] Total saves count in FixButtons: {saves.Count}");
        for (var i = 0; i < saves.Count; i++)
        {
            Plugin.Log.LogInfo($"[Debug] Save[{i}]: Name={saves[i].characterData.characterName}, FileName={saves[i].fileName}");
        }

        var index = saves.FindIndex(s => s.characterData == character);
        Plugin.Log.LogInfo($"[Debug] Character index in saves list: {index}");

        Plugin.Log.LogInfo("[Debug] Instantiating deleteButton clone");
        var buttonObject = Object.Instantiate(savePanel.deleteButton.gameObject, savePanel.deleteButton.transform.parent);
        buttonObject.name = "CharacterEditRedux";

        var rect = buttonObject.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(20, -65);

        Plugin.Log.LogInfo("[Debug] Configuring UISprites on new button");
        var wishButton = buttonObject.GetComponent<UIButton>();
        wishButton.defaultImage = DefaultImage;
        wishButton.hoverOverImage = MouseOverImage;
        wishButton.pressedImage = MouseOverImage;

        var button = buttonObject.GetComponent<Button>();
        button.image.sprite = DefaultImage;

        Plugin.Log.LogInfo("[Debug] Setting up onClick listener for custom button");
        button.onClick = new Button.ButtonClickedEvent();
        button.onClick.AddListener(delegate
        {
            BackUpData(character);
            Plugin.Log.LogInfo($"[Debug] Custom button clicked, loading character at index {index}");
            GameSave.LoadCharacter(index);
            MainMenuController.EnableMenu(MainMenuController.newCharacterMenu);
            Plugin.Log.LogInfo("[Debug] newCharacterMenu enabled");
            SetupCharacter();
            MainMenuController.backCharacterButton.onClick.AddListener(SetupButtons);
            MainMenuController.confirmCharacterButton.onClick.RemoveAllListeners();
            MainMenuController.confirmCharacterButton.onClick.AddListener(delegate
            {
                Plugin.Log.LogInfo("[Debug] ConfirmCharacterButton clicked, applying clothing and saving");

                var currentClothingDictionary = NewCharacterCreator.currentClothingDictionary;
                Plugin.Log.LogInfo($"[Debug] Clothing dictionary count before applying: {currentClothingDictionary.Count}");
                foreach (var kv in currentClothingDictionary)
                {
                    Plugin.Log.LogInfo($"[Debug] Pre-Apply Clothing: Layer={kv.Key}, Style={kv.Value?.name ?? "null"} ");
                }

                foreach (var clothingLayer in (ClothingLayer[])Enum.GetValues(typeof(ClothingLayer)))
                {
                    var style = CharacterClothingStyles.GetStyle(clothingLayer, CurrentCharacterData.styleData[(byte)clothingLayer]);
                    Plugin.Log.LogInfo($"[Debug] Applying style for layer {clothingLayer}: {style?.name}");
                    if (style)
                    {
                        currentClothingDictionary[clothingLayer] = style;
                        NewCharacterCreator.SetClothingLayerData(style, clothingLayer);
                    }
                }

                Plugin.Log.LogInfo($"[Debug] Clothing dictionary count after applying: {currentClothingDictionary.Count}");
                foreach (var kv in currentClothingDictionary)
                {
                    Plugin.Log.LogInfo($"[Debug] Post-Apply Clothing: Layer={kv.Key}, Style={kv.Value?.name ?? "null"} ");
                }

                foreach (var keyValuePair in currentClothingDictionary.ToList())
                {
                    if (!keyValuePair.Value.armorData)
                        continue;

                    var key = keyValuePair.Key;
                    Plugin.Log.LogInfo($"[Debug] Processing armor layer {key}");
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
                    Plugin.Log.LogInfo($"[Debug] Vanity index for armor type {keyValuePair.Value.armorData.armorType}: {vanityIndex}");
                    CurrentCharacterData.Items[(short)vanityIndex] = new InventoryItemData
                    {
                        Amount = 1,
                        Item = keyValuePair.Value.armorData.GenerateArmorItem()
                    };
                    var clothingLayerIdx = keyValuePair.Value.clothingLayers[0];
                    NewCharacterCreator.SetClothingLayerData(NewCharacterCreator.defaultLayers[clothingLayerIdx], clothingLayerIdx);
                }

                Plugin.Log.LogInfo("[Debug] Writing character data to file");
                GameSave.WriteCharacterToFile();

                Plugin.Log.LogInfo($"[Debug] Total items in CurrentCharacterData.Items: {CurrentCharacterData.Items.Count}");
                foreach (var kv in CurrentCharacterData.Items.Where(kv => kv.Value?.Item != null))
                {
                    Plugin.Log.LogInfo($"[Debug] Item slot {kv.Key}: ID={kv.Value.Item.ID()}, Amount={kv.Value.Amount}");
                }


                Plugin.Log.LogInfo($"[Debug] Comparing names: CurrentCharacterData.characterName='{CurrentCharacterData.characterName}' vs _lastName='{_lastName}'");
                if (CurrentCharacterData.characterName != _lastName)
                {
                    Plugin.Log.LogInfo("[Debug] Name mismatch detected, entering rename cleanup block");
                    var oldPath = Path.Combine(Application.persistentDataPath, "Saves", _lastName + ".save");
                    Plugin.Log.LogInfo($"[Debug] Old save path to delete: {oldPath}");
                    if (File.Exists(oldPath))
                    {
                        Plugin.Log.LogInfo($"[Debug] Old save file exists, deleting now");
                        File.Delete(oldPath);
                        Plugin.Log.LogInfo("[Debug] Deleted old save file");
                    }
                    else
                    {
                        Plugin.Log.LogWarning($"[Debug] Old save file not found at {oldPath}, nothing to delete");
                    }

                    Plugin.Log.LogInfo("[Debug] Reloading all character saves");
                    GameSave.LoadAllCharacters();
                    Plugin.Log.LogInfo("[Debug] Reloaded all character saves");
                }

                MainMenuController.EnableMenu(MainMenuController.homeMenu);
                Plugin.Log.LogInfo("[Debug] homeMenu enabled");
                MainMenuController.SetupButtons();
            });
        });

        Plugin.Log.LogInfo("[Debug] Exiting FixButtons coroutine");
        yield break;
    }

    private static void SetupButtons()
    {
        Plugin.Log.LogInfo("[Debug] Entering SetupButtons");
        MainMenuController.SetupButtons();
        MainMenuController.backCharacterButton.onClick.RemoveListener(SetupButtons);
        Plugin.Log.LogInfo("[Debug] Exiting SetupButtons");
    }

    private static void SetupCharacter()
    {
        Plugin.Log.LogInfo("[Debug] Entering SetupCharacter");
        _lastName = CurrentCharacterData.characterName;
        Plugin.Log.LogInfo($"[Debug] _lastName set to {_lastName}");
        NewCharacterCreator.nameInputField.text = _lastName;

        if (!CurrentCharacterData.male)
        {
            Plugin.Log.LogInfo("[Debug] Setting character gender to Female");
            NewCharacterCreator.SetFemale();
        }
        else
        {
            Plugin.Log.LogInfo("[Debug] Setting character gender to Male");
            NewCharacterCreator.SetMale();
        }

        var currentClothingDictionary = NewCharacterCreator.currentClothingDictionary;
        Plugin.Log.LogInfo($"[Debug] Clothing dictionary count at start of SetupCharacter: {currentClothingDictionary.Count}");
        foreach (var kv in currentClothingDictionary)
        {
            Plugin.Log.LogInfo($"[Debug] Initial Clothing: Layer={kv.Key}, Style={kv.Value?.name}");
        }

        foreach (var layer in (ClothingLayer[])Enum.GetValues(typeof(ClothingLayer)))
        {
            var style = CharacterClothingStyles.GetStyle(layer, CurrentSaveCharacterData.styleData[(byte)layer]);
            Plugin.Log.LogInfo($"[Debug] Load style for saved character layer {layer}: {style?.name}");
           
            if (!style) continue;
            
            currentClothingDictionary[layer] = style;
            NewCharacterCreator.SetClothingLayerData(style, layer);
        }

        foreach (var armorType in (ArmorType[])Enum.GetValues(typeof(ArmorType)))
        {
            var vanityIndex = PlayerInventory.GetVanityIndexByArmorType(armorType);
            var item = CurrentCharacterData.Items[(short)vanityIndex]?.Item;
            Plugin.Log.LogInfo($"[Debug] Checking item at vanity index {vanityIndex}: {item?.ID()}");
            if (item is ArmorItem)
            {
                var itemData = Utils.GetItemData<ArmorData>(item.ID());
                if (!itemData || !itemData.clothingLayerData) continue;
                foreach (var clothingLayer in itemData.clothingLayerData.clothingLayers)
                {
                    Plugin.Log.LogInfo($"[Debug] Applying default layer data for armor: {clothingLayer}");
                    NewCharacterCreator.SetClothingLayerData(itemData.clothingLayerData, clothingLayer);
                }
            }
        }

        Plugin.Log.LogInfo("[Debug] Updating starting items");
        NewCharacterCreator.UpdateStartingItems();
        Plugin.Log.LogInfo("[Debug] Exiting SetupCharacter");
    }
}