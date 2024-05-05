using HarmonyLib;

namespace MaxButton
{
    [HarmonyPatch(typeof(CraftGUI), "Open", typeof(WorldGameObject), typeof(CraftsInventory), typeof(string))]
    public class CraftGuiOpen
    {
        [HarmonyPostfix]
        public static void Postfix(CraftGUI __instance, WorldGameObject craftery_wgo)
        {
            if (LazyInput.gamepad_active)
                return;
            var componentsInChildren = __instance.GetComponentsInChildren<CraftItemGUI>();
            foreach (var t in componentsInChildren)
            {
                MaxButtonCrafting.AddMinAndMaxButtons(t, "amount btn R", "amount btn max", true, craftery_wgo);
                MaxButtonCrafting.AddMinAndMaxButtons(t, "amount btn L", "amount btn min", false, craftery_wgo);
            }
        }
    }
}