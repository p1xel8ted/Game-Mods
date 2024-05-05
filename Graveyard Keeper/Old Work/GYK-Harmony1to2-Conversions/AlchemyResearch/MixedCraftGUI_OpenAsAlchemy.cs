using HarmonyLib;
using UnityEngine;

namespace AlchemyResearch
{
    [HarmonyPatch(typeof(MixedCraftGUI), "OpenAsAlchemy", typeof(WorldGameObject), typeof(string))]
    public static class MixedCraftGuiOpenAsAlchemy
    {
        public static Transform GetCrafteryTransform(
        Transform CraftingStation,
        string CrafteryWGOObjectID)
        {
            if (CrafteryWGOObjectID == "mf_alchemy_craft_02")
                return CraftingStation.Find("alchemy_craft_02");
            return CrafteryWGOObjectID == "mf_alchemy_craft_03" ? CraftingStation.Find("alchemy_craft_03") : null;
        }

        [HarmonyPostfix]
        public static void Patch(
      MixedCraftGUI __instance,
      WorldGameObject craftery_wgo,
      string preset_name)
        {
            AlchemyRecipe.Initialize();
            var crafteryTransform = GetCrafteryTransform(__instance.transform, craftery_wgo.obj_id);
            if (__instance.transform.Find("ingredient container result") != null)
                return;
            var transform1 = crafteryTransform.transform.Find("ingredients/ingredient container (1)");
            var gameObject1 = Object.Instantiate(transform1.gameObject);
            gameObject1.name = "ingredient container result";
            var transform2 = gameObject1.transform;
            transform2.transform.SetParent(__instance.transform, false);
            transform2.transform.position = Vector3.zero;
            transform2.transform.localPosition = new Vector3(0.0f, -40f, 0.0f);
            if (craftery_wgo.obj_id == "mf_alchemy_craft_03")
                transform2.transform.localPosition = new Vector3(transform1.localPosition.x, -40f, 0.0f);
            ResultPreviewDrawUnknown(gameObject1.transform);
            var gameObject2 = Object.Instantiate(crafteryTransform.transform.Find("ingredients/ingredient container/Base Item Cell/x2 container/counter").gameObject);
            gameObject2.name = "label result";
            gameObject2.transform.SetParent(gameObject1.transform, false);
            var component = gameObject2.GetComponent<UILabel>();
            component.text = MainPatcher.ResultPreviewText;
            component.pivot = (UIWidget.Pivot)4;
            component.color = new Color(0.937f, 0.87f, 0.733f);
            component.overflowWidth = 0;
            component.overflowMethod = 0;
            component.topAnchor.target = gameObject1.transform;
            component.bottomAnchor.target = gameObject1.transform;
            component.rightAnchor.target = gameObject1.transform;
            component.leftAnchor.target = gameObject1.transform;
            component.leftAnchor.relative = -10f;
            component.rightAnchor.relative = 10f;
            component.topAnchor.relative = -9f;
            component.bottomAnchor.relative = -10f;
        }

        public static void ResultPreviewDrawItem(Transform ResultPreview, string ItemID)
        {
            var componentInChildren = ResultPreview.GetComponentInChildren<BaseItemCellGUI>();
            componentInChildren.DrawEmpty();
            componentInChildren.DrawItem(ItemID, 1);
        }

        public static void ResultPreviewDrawUnknown(Transform ResultPreview)
        {
            if (!(bool)ResultPreview)
                return;
            var componentInChildren = ResultPreview.GetComponentInChildren<BaseItemCellGUI>();
            if (!(bool)componentInChildren)
                return;
            componentInChildren.DrawEmpty();
            componentInChildren.DrawUnknown();
        }
    }
}