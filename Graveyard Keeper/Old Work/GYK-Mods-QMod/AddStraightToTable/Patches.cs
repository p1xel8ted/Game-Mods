using System.Linq;
using HarmonyLib;
using Helper;

namespace AddStraightToTable;

[HarmonyPatch]
public static partial class MainPatcher
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(AutopsyGUI),nameof(AutopsyGUI.OnBodyItemPress), typeof(BaseItemCellGUI))]
    public static bool AutopsyGUI_OnBodyItemPress_Prefix()
    {
        _wms = Tools.ModLoaded(WheresMaStorageId, WheresMaStorageFileName, WheresMaStorageName) || Harmony.HasAnyPatches("p1xel8ted.GraveyardKeeper.WheresMaStorage");
        if (_wms)
        {
            _cfg = Config.GetOptions();
        }

        return false;
    }
    

    [HarmonyPostfix]
    [HarmonyPatch(typeof(AutopsyGUI),nameof(AutopsyGUI.OnBodyItemPress), typeof(BaseItemCellGUI))]
    public static void AutopsyGUI_OnBodyItemPress_Postfix(ref AutopsyGUI __instance, BaseItemCellGUI item_gui)
    {
        if (item_gui.item.id == "insertion_button_pseudoitem")
        {
            var obj = MainGame.me.player;
            if (GlobalCraftControlGUI.is_global_control_active && __instance._autopti_obj != null) obj = __instance._autopti_obj;
    
            var inventory = __instance._parts_inventory;
            var instance = __instance;
            GUIElements.me.resource_picker.Open(obj, delegate(Item item, InventoryWidget _)
                {
                    if (item == null || item.IsEmpty())
                    {
                        if (_wms && _cfg.HideInvalidSelections)
                        {
                            return InventoryWidget.ItemFilterResult.Inactive;
                        }
    
                        return InventoryWidget.ItemFilterResult.Hide;
                    }
    
                    if (item.definition.type != ItemDefinition.ItemType.BodyUniversalPart)
                        return InventoryWidget.ItemFilterResult.Inactive;
    
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
            return;
        }
    
        // var craftDefinition = (CraftDefinition) typeof(AutopsyGUI)
        //     .GetMethod("GetExtractCraftDefinition", AccessTools.all)
        //     ?.Invoke(__instance, new object[]
        //     {
        //         item_gui.item
        //     });
        
        var craftDefinition = __instance.GetExtractCraftDefinition(item_gui.item);
    
        if (craftDefinition == null) return;
    
        AutopsyGUI.RemoveBodyPartFromBody(__instance._body, item_gui.item);
        // typeof(AutopsyGUI).GetMethod("RemoveBodyPartFromBody", AccessTools.all)
        //     ?.Invoke(__instance, new object[]
        //     {
        //         ____body,
        //         item_gui.item
        //     });
    
        __instance._autopti_obj.components.craft.CraftAsPlayer(craftDefinition, item_gui.item);
        {
            __instance.Hide();
        }
    }
}