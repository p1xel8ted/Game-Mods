using flanne.Pickups;
using HarmonyLib;

namespace LagLess;

[Harmony]
public class XPPickupPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(XPPickup), nameof(XPPickup.UsePickup))]
    private static void UsePickup(ref XPPickup __instance)
    {
        var component = __instance.gameObject.GetComponent<LLXPComponent>();
        for (var i = 0; i < component.extraExperienceCollected; i++)
        {
            __instance.PostNotification(XPPickup.XPPickupEvent, null);
        }
    }
}