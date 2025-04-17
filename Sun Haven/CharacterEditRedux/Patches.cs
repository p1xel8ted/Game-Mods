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
    

    [HarmonyPostfix]
    [HarmonyPatch(typeof(SavePanel), nameof(SavePanel.SetPlayerImage))]
    private static void SavePanel_SetPlayerImage(SavePanel __instance, CharacterData character)
    {
        MainMenuController.StartCoroutine(FixButtons(__instance, character));
    }


    private static IEnumerator FixButtons(SavePanel savePanel, CharacterData character)
    {
        var index = GameSave.Saves.FindIndex(s => s.characterData == character);
        
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
            GameSave.LoadCharacter(index);
            MainMenuController.EnableMenu(MainMenuController.newCharacterMenu);
            SetupCharacter();
            MainMenuController.backCharacterButton.onClick.AddListener(SetupButtons);
            MainMenuController.confirmCharacterButton.onClick.RemoveAllListeners();
            MainMenuController.confirmCharacterButton.onClick.AddListener(delegate
            {
                var currentClothingDictionary = NewCharacterCreator.currentClothingDictionary;
                foreach (var clothingLayer in (ClothingLayer[])Enum.GetValues(typeof(ClothingLayer)))
                {
                    var style = CharacterClothingStyles.GetStyle(clothingLayer, CurrentCharacterData.styleData[(byte)clothingLayer]);
                    if (style)
                    {
                        currentClothingDictionary[clothingLayer] = style;
                        NewCharacterCreator.SetClothingLayerData(style, clothingLayer, false);
                    }
                }

                foreach (var keyValuePair in currentClothingDictionary.ToList())
                {
                    if (!keyValuePair.Value.armorData) continue;

                    var key = keyValuePair.Key;
                    if (key <= ClothingLayer.Back)
                    {
                        if (key != ClothingLayer.FrontGloves && key != ClothingLayer.Back)
                        {
                            continue;
                        }
                    }
                    else if (key != ClothingLayer.Hat && key != ClothingLayer.Chest && key != ClothingLayer.Pants)
                    {
                        continue;
                    }

                    var vanityIndexByArmorType = PlayerInventory.GetVanityIndexByArmorType(keyValuePair.Value.armorData.armorType);
                    CurrentCharacterData.Items[(short)vanityIndexByArmorType] = new InventoryItemData
                    {
                        Amount = 1,
                        Item = keyValuePair.Value.armorData.GenerateArmorItem()
                    };
                    var clothingLayer = keyValuePair.Value.clothingLayers[0];
                    NewCharacterCreator.SetClothingLayerData(NewCharacterCreator.defaultLayers[clothingLayer], clothingLayer, false);
                }


                GameSave.WriteCharacterToFile();
                
                if (CurrentCharacterData.characterName != _lastName)
                {
                    var path = Path.Combine(Application.persistentDataPath, "Saves", _lastName + ".save");
                    if (File.Exists(path))
                        File.Delete(path);

                    GameSave.LoadAllCharacters();
                }

                MainMenuController.EnableMenu(MainMenuController.homeMenu);
                MainMenuController.SetupButtons();
            });
        });
        
        yield break;
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
            NewCharacterCreator.SetFemale();
        else
            NewCharacterCreator.SetMale();

        var currentClothingDictionary = NewCharacterCreator.currentClothingDictionary;

        foreach (var clothingLayer in (ClothingLayer[])Enum.GetValues(typeof(ClothingLayer)))
        {
            var style = CharacterClothingStyles.GetStyle(clothingLayer, CurrentSaveCharacterData.styleData[(byte)clothingLayer]);
            if (style)
            {
                currentClothingDictionary[clothingLayer] = style;
                NewCharacterCreator.SetClothingLayerData(style, clothingLayer, false);
            }
        }

        foreach (var armorType in (ArmorType[])Enum.GetValues(typeof(ArmorType)))
        {
            var vanityIndexByArmorType = PlayerInventory.GetVanityIndexByArmorType(armorType);

            if (CurrentCharacterData.Items[(short)vanityIndexByArmorType]?.Item is ArmorItem)
            {
                var itemData = Utils.GetItemData<ArmorData>(CurrentCharacterData.Items[(short)vanityIndexByArmorType].Item.ID());

                if (!itemData || itemData.clothingLayerData is null)
                    continue;
                foreach (var clothingLayer in itemData.clothingLayerData.clothingLayers)
                {
                    NewCharacterCreator.SetClothingLayerData(itemData.clothingLayerData, clothingLayer, false);
                }
            }
        }

        NewCharacterCreator.UpdateStartingItems();
    }
}