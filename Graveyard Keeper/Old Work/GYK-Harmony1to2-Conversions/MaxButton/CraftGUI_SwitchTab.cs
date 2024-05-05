using HarmonyLib;

namespace MaxButton
{
    [HarmonyPatch(typeof(CraftGUI), "SwitchTab", typeof(string))]
    public class CraftGuiSwitchTab
    {
        [HarmonyPostfix]
        public static void Postfix(CraftGUI __instance)
        {
            var componentsInChildren = __instance.GetComponentsInChildren<CraftItemGUI>();
            foreach (var t in componentsInChildren)
            {
                MaxButtonCrafting.AddMinAndMaxButtons(t, "amount btn R", "amount btn max", true, __instance.GetCrafteryWGO());
                MaxButtonCrafting.AddMinAndMaxButtons(t, "amount btn L", "amount btn min", false, __instance.GetCrafteryWGO());
            }
        }
    }
}