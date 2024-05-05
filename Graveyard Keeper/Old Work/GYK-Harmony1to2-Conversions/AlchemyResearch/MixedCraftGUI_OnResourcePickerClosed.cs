using HarmonyLib;

namespace AlchemyResearch
{
    [HarmonyPatch(typeof(MixedCraftGUI), "OnResourcePickerClosed", typeof(Item))]
    public class MixedCraftGuiOnResourcePickerClosed
    {
        [HarmonyPostfix]
        public static void Patch(MixedCraftGUI __instance)
        {
            var objId = __instance.GetCrafteryWGO().obj_id;
            var crafteryTransform = MixedCraftGuiOpenAsAlchemy.GetCrafteryTransform(__instance.transform, objId);
            var resultPreview = __instance.transform.Find("ingredient container result");
            MixedCraftGuiOpenAsAlchemy.ResultPreviewDrawUnknown(resultPreview);
            if (!(bool)crafteryTransform)
                return;
            var transform1 = crafteryTransform.transform.Find("ingredients/ingredient container/Base Item Cell");
            var transform2 = crafteryTransform.transform.Find("ingredients/ingredient container (1)/Base Item Cell");
            var transform3 = crafteryTransform.transform.Find("ingredients/ingredient container (2)/Base Item Cell");
            if (!(bool)transform1 || !(bool)transform2)
                return;
            var component1 = transform1.GetComponent<BaseItemCellGUI>();
            var component2 = transform2.GetComponent<BaseItemCellGUI>();
            BaseItemCellGUI baseItemCellGui = null;
            if ((bool)transform3)
                baseItemCellGui = transform3.GetComponent<BaseItemCellGUI>();
            if (!(bool)component1 || !(bool)component2)
            {
                MixedCraftGuiOpenAsAlchemy.ResultPreviewDrawUnknown(resultPreview);
            }
            else
            {
                var id1 = component1.item.id;
                var id2 = component2.item.id;
                var ingredient3 = "empty";
                if ((bool)baseItemCellGui)
                    ingredient3 = baseItemCellGui?.item.id;
                if (id1 == "empty" || id2 == "empty" || ingredient3 == "empty" && objId == "mf_alchemy_craft_03")
                {
                    MixedCraftGuiOpenAsAlchemy.ResultPreviewDrawUnknown(resultPreview);
                }
                else
                {
                    var itemId = ResearchedAlchemyRecipes.IsRecipeKnown(id1, id2, ingredient3);
                    MixedCraftGuiOpenAsAlchemy.ResultPreviewDrawItem(resultPreview, itemId);
                }
            }
        }
    }
}