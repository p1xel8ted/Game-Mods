using HarmonyLib;
using System;

namespace AlchemyResearch
{
    [HarmonyPatch(typeof(MixedCraftGUI), "OnCraftPressed", new Type[] { })]
    public class MixedCraftGuiOnCraftPressed
    {
        [HarmonyPrefix]
        public static void Patch(MixedCraftGUI __instance)
        {
            AlchemyRecipe.Initialize();
            if (!(bool)Reflection.MethodIsCraftAllowed.Invoke(__instance, Array.Empty<object>()))
                return;
            var mixedCraftPresetGui = (MixedCraftPresetGUI)Reflection.FieldCurrentPreset.GetValue(__instance);
            var craftDefinition = (CraftDefinition)Reflection.MethodGetCraftDefinition.Invoke(__instance, new object[2]
            {
          false,
          null
            }) ?? (CraftDefinition)Reflection.MethodGetCraftDefinition.Invoke(__instance, new object[2]
            {
          true,
          null
            });
            if (craftDefinition == null || !craftDefinition.id.StartsWith("mix:mf_alchemy"))
                return;
            var selectedItems = mixedCraftPresetGui.GetSelectedItems();
            if (selectedItems.Count < 2)
                return;
            for (var index = 0; index < selectedItems.Count; ++index)
            {
                switch (index)
                {
                    case 0:
                        AlchemyRecipe.Ingredient1 = selectedItems[index].id;
                        break;

                    case 1:
                        AlchemyRecipe.Ingredient2 = selectedItems[index].id;
                        break;

                    case 2:
                        AlchemyRecipe.Ingredient3 = selectedItems[index].id;
                        break;
                }
                AlchemyRecipe.Result = craftDefinition.GetFirstRealOutput().id;
                AlchemyRecipe.WorkstationUnityId = __instance.GetCrafteryWGO().GetInstanceID();
                AlchemyRecipe.WorkstationObjectId = __instance.GetCrafteryWGO().obj_id;
            }
            Logg.Log(
                $"Processed Recipe: {AlchemyRecipe.Ingredient1}|{AlchemyRecipe.Ingredient2}|{AlchemyRecipe.Ingredient3} => {AlchemyRecipe.Result} | WGO: {AlchemyRecipe.WorkstationUnityId} / {AlchemyRecipe.WorkstationObjectId}");
        }
    }
}