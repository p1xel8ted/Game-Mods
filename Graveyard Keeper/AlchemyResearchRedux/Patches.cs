using System.Linq;
using HarmonyLib;

namespace AlchemyResearchRedux;

[Harmony]
public static class Patches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(AutopsyGUI), nameof(AutopsyGUI.OnBodyItemPress), typeof(BaseItemCellGUI))]
    public static bool AutopsyGUI_OnBodyItemPress_Postfix(ref AutopsyGUI __instance, BaseItemCellGUI item_gui)
    {
        if (item_gui.item.id == "insertion_button_pseudoitem")
        {
            var obj = MainGame.me.player;
            if (GlobalCraftControlGUI.is_global_control_active && __instance._autopti_obj != null) obj = __instance._autopti_obj;

            var inventory = __instance._parts_inventory;
            var instance = __instance;
            GUIElements.me.resource_picker.Open(obj, delegate(Item item, InventoryWidget _)
                {
                    if (item != null && item.IsEmpty())
                    {
                        return InventoryWidget.ItemFilterResult.Hide;
                    }

                    if (item != null && item.definition.type != ItemDefinition.ItemType.BodyUniversalPart)
                        return InventoryWidget.ItemFilterResult.Inactive;

                    if (item == null)
                        return instance.GetInsertCraftDefinition(item) == null
                            ? InventoryWidget.ItemFilterResult.Inactive
                            : InventoryWidget.ItemFilterResult.Active;
                    var text = item.id;
                    if (text.Contains(":")) text = text.Split(':')[0];

                    text = text.Replace("_dark", "");
                    if (inventory.data.inventory.Any(item2 =>
                            item2 != null && !item2.IsEmpty() && item2.id.StartsWith(text)))
                        return InventoryWidget.ItemFilterResult.Inactive;

                    return instance.GetInsertCraftDefinition(item) == null
                        ? InventoryWidget.ItemFilterResult.Inactive
                        : InventoryWidget.ItemFilterResult.Active;
                },
                __instance.OnItemForInsertionPicked);
            return true;
        }

        var craftDefinition = __instance.GetExtractCraftDefinition(item_gui.item);

        if (craftDefinition == null) return true;

        AutopsyGUI.RemoveBodyPartFromBody(__instance._body, item_gui.item);

        __instance._autopti_obj.components.craft.CraftAsPlayer(craftDefinition, item_gui.item);

        __instance.Hide();
        return false;
    }
}