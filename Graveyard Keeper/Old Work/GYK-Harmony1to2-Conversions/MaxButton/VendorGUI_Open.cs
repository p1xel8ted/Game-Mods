using HarmonyLib;
using System.Reflection;

namespace MaxButton
{
    [HarmonyPatch(typeof(VendorGUI), "OpenItemCountWidnow", typeof(string), typeof(int), typeof(bool), typeof(bool))]
    public class VendorGUI_Open
    {
        private static FieldInfo _fieldItem;

        [HarmonyPrefix]
        public static void Patch(
          VendorGUI __instance,
          string item_id,
          int can_move,
          bool from_inventory,
          bool from_player)
        {
            if (_fieldItem == null)
                _fieldItem = typeof(VendorGUI).GetField("_selected_item", AccessTools.all);
            var obj = (Item)_fieldItem?.GetValue(__instance);
            MaxButtonVendor.SetCaller("VendorGUI", from_player, can_move, obj, __instance.trading);
        }
    }
}