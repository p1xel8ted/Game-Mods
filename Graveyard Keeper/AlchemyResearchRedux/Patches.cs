namespace AlchemyResearchRedux;

[Harmony]
public static class Patches
{

    private const string AlchemyWorkbench1ObjID = "mf_alchemy_craft_02";
    private const string AlchemyWorkbench2ObjID = "mf_alchemy_craft_03";
    private const string IngredientContainerResult = "ingredient container result";
    private const string IngredientCell1 = "ingredients/ingredient container/Base Item Cell";
    private const string IngredientCell2 = "ingredients/ingredient container (1)/Base Item Cell";
    private const string IngredientCell3 = "ingredients/ingredient container (2)/Base Item Cell";


    private static string GetLocalResult()
    {
        var lang = GameSettings._cur_lng;
        return lang switch
        {
            "en" => "Result",
            "fr" => "Résultat",
            "de" => "Ergebnis",
            "zh_cn" => "结果",
            "zh-cn" => "结果",
            "es" => "Resultado",
            "pt-br" => "Resultado",
            "pt_br" => "Resultado",
            "ko" => "결과",
            "ja" => "結果",
            "ru" => "Результат",
            "it" => "Risultato",
            "pl" => "Wynik",
            _ => "Result"
        };
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(MixedCraftGUI), nameof(MixedCraftGUI.OnResourcePickerClosed))]
    public static void MixedCraftGUI_OnResourcePickerClosed(MixedCraftGUI __instance, Item item)
    {
        var objId = __instance.GetCrafteryWGO().obj_id;
        var crafteryTransform = GetCrafteryTransform(__instance.transform, objId);
        var resultTransform = __instance.transform.Find(IngredientContainerResult);

        if (!crafteryTransform)
        {
            ResultPreviewDrawUnknown(__instance.transform.Find(IngredientContainerResult));
            return;
        }

        var ingredient1 = crafteryTransform.Find(IngredientCell1)?.GetComponent<BaseItemCellGUI>();
        var ingredient2 = crafteryTransform.Find(IngredientCell2)?.GetComponent<BaseItemCellGUI>();
        var ingredient3 = crafteryTransform.Find(IngredientCell3)?.GetComponent<BaseItemCellGUI>();

        var ingred1 = ingredient1?.item?.id ?? string.Empty;
        var ingred2 = ingredient2?.item?.id ?? string.Empty;
        var ingred3 = ingredient3?.item?.id ?? string.Empty;

        var craftId = $"mix:{objId}:{ingred1}:{ingred2}:{ingred3}:";

        var resultId = AlchemyRecipe.GetRecipeResult(craftId)?.Result ?? string.Empty;

        if (resultId.IsNullOrWhiteSpace()) return;

        var itemDef = GameBalance.me.GetData<ItemDefinition>(resultId);

        if (itemDef == null)
        {
            Plugin.LOG.LogWarning($"No item definition found: {resultId}");
            return;
        }

        ResultPreviewDrawItem(resultTransform, itemDef.id);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(MixedCraftGUI), nameof(MixedCraftGUI.OpenAsAlchemy))]
    public static void PatchOpenAsAlchemy(MixedCraftGUI __instance, WorldGameObject craftery_wgo, string preset_name)
    {
        var crafteryTransform = GetCrafteryTransform(__instance.transform, craftery_wgo.obj_id);

        if (!crafteryTransform || __instance.transform.Find(IngredientContainerResult)) return;

        foreach (var craft in __instance.crafts)
        {
            var craftDef = GameBalance.me.GetData<CraftDefinition>(craft.id);
            var output = craftDef.GetFirstRealOutput();
            if (output.id.StartsWith("goo")) continue;

            var split = craft.id.Replace("_", "").Split(':');

            var ingredient1 = split[2];
            var ingredient2 = split[3];
            var ingredient3 = split[4];

            var recipe = new AlchemyRecipe
            {
                CraftString = craft.id,
                Ingredient1 = ingredient1,
                Ingredient2 = ingredient2,
                Ingredient3 = ingredient3,
                Result = output.id
            };

            AlchemyRecipe.AddRecipe(recipe);
        }


        var resultContainer = CreateResultContainer(crafteryTransform, __instance.transform, craftery_wgo.obj_id);

        ResultPreviewDrawUnknown(resultContainer);
        CreateResultLabel(resultContainer);
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(MixedCraftGUI), nameof(MixedCraftGUI.Hide))]
    public static void MixedCraftGUI_Hide(MixedCraftGUI __instance)
    {
        var resultTransform = __instance.transform.Find(IngredientContainerResult);
        if (resultTransform)
        {
            Object.Destroy(resultTransform.gameObject);
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(MixedCraftGUI), nameof(MixedCraftGUI.OnCraftPressed))]
    public static void MixedCraftGUI_OnCraftPressed(MixedCraftGUI __instance)
    {
        if (!__instance.IsCraftAllowed()) return;

        var craftDef = __instance.GetCraftDefinition(false, out _);

        if (craftDef == null || !craftDef.id.StartsWith("mix:mf_alchemy")) return;

        var preset = __instance._current_preset;
        var ingredients = preset.GetSelectedItems();

        var result = craftDef.GetFirstRealOutput();

        var recipe = new AlchemyRecipe
        {
            CraftString = craftDef.id,
            Ingredient1 = ingredients.Count > 0 ? ingredients[0].id : string.Empty,
            Ingredient2 = ingredients.Count > 1 ? ingredients[1].id : string.Empty,
            Ingredient3 = ingredients.Count > 2 ? ingredients[2].id : string.Empty,
            Result = result.id
        };

        AlchemyRecipe.AddRecipe(recipe);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(UIWidget), nameof(UIWidget.OnInit))]
    [HarmonyPatch(typeof(UIWidget), nameof(UIWidget.OnStart))]
    public static void UIWidget_Init(UIWidget __instance)
    {
        if (__instance.name.Contains("ingredient container") && !__instance.name.Contains("result"))
        {
            if (!__instance.transform.GetComponent<WidgetPos>())
            {
                __instance.transform.gameObject.AddComponent<WidgetPos>();
            }
        }
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(PlatformSpecific), nameof(PlatformSpecific.SaveGame))]
    public static void PlatformSpecific_SaveGame(SaveSlotData slot)
    {
        AlchemyRecipe.SaveRecipesToFile();
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(PlatformSpecific), nameof(PlatformSpecific.LoadGame))]
    public static void PatchLoadGame(SaveSlotData slot, PlatformSpecific.OnGameLoadedDelegate on_lodaded)
    {
        AlchemyRecipe.LoadRecipesFromFile();
    }


    private static Transform GetCrafteryTransform(Transform craftingStation, string crafteryWgoObjectId)
    {
        return crafteryWgoObjectId switch
        {
            AlchemyWorkbench1ObjID => craftingStation.Find("alchemy_craft_02"),
            AlchemyWorkbench2ObjID => craftingStation.Find("alchemy_craft_03"),
            _ => null
        };
    }


    private static Transform CreateResultContainer(Transform crafteryTransform, Transform parentTransform, string objId)
    {
        var container1 = crafteryTransform.Find("ingredients/ingredient container (1)");

        var resultContainer = Object.Instantiate(container1.gameObject, parentTransform);
        resultContainer.name = IngredientContainerResult;
        resultContainer.transform.localPosition = objId == AlchemyWorkbench2ObjID
            ? new Vector3(container1.localPosition.x, -20f, 0f)
            : new Vector3(0f, -20f, 0f);

        return resultContainer.transform;
    }

    private static void CreateResultLabel(Transform resultContainer)
    {
        var baseCell = resultContainer.Find("Base Item Cell/x2 container/counter");
        var resultLabel = Object.Instantiate(baseCell.gameObject, resultContainer);
        resultLabel.name = "label result";

        var labelComponent = resultLabel.GetComponent<UILabel>();
        labelComponent.text = GetLocalResult();
        labelComponent.pivot = UIWidget.Pivot.Center;
        labelComponent.color = new Color(0.937f, 0.87f, 0.733f);
        labelComponent.overflowWidth = 0;
        labelComponent.overflowMethod = 0;
        labelComponent.topAnchor.target = resultContainer;
        labelComponent.bottomAnchor.target = resultContainer;
        labelComponent.rightAnchor.target = resultContainer;
        labelComponent.leftAnchor.target = resultContainer;
        labelComponent.leftAnchor.relative = -10f;
        labelComponent.rightAnchor.relative = 10f;
        labelComponent.topAnchor.relative = -9f;
        labelComponent.bottomAnchor.relative = -10f;
    }

    private static void ResultPreviewDrawItem(Transform resultPreview, string itemId)
    {
        var baseItemCellGui = resultPreview.GetComponentInChildren<BaseItemCellGUI>();
        baseItemCellGui.DrawEmpty();
        baseItemCellGui.DrawItem(itemId, 1);
    }

    private static void ResultPreviewDrawUnknown(Transform resultPreview)
    {
        var baseItemCellGui = resultPreview?.GetComponentInChildren<BaseItemCellGUI>();
        baseItemCellGui?.DrawEmpty();
        baseItemCellGui?.DrawUnknown();
    }
}