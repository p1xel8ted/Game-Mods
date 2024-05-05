using HarmonyLib;
using System;

namespace MaxButton
{
    [HarmonyPatch(typeof(ItemCountGUI), "Open", typeof(string), typeof(int), typeof(int), typeof(Action<int>), typeof(int), typeof(ItemCountGUI.PriceCalculateDelegate))]
    public class ItemCountGuiOpen
    {
        [HarmonyPostfix]
        public static void Postfix(
          ItemCountGUI __instance,
          string item_id,
          int min,
          int max,
          Action<int> on_confirm,
          int slider_step_for_keyboard = 1,
          ItemCountGUI.PriceCalculateDelegate price_calculate_delegate = null)
        {
            MaxButtonVendor.AddMaxButton(ref __instance, price_calculate_delegate);
        }
    }
}