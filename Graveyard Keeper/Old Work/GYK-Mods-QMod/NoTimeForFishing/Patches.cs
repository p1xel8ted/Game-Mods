using Fishing;
using HarmonyLib;
using UnityEngine;

namespace NoTimeForFishing;

[HarmonyPatch]
public static partial class MainPatcher
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(FishLogic), nameof(FishLogic.CalculateFishPos))]
    public static void FishLogic_CalculateFishPos(ref float pos, ref float rod_zone_size)
    {
        pos = 0f;
        rod_zone_size = 100f;
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(FishingGUI), nameof(FishingGUI.UpdateWaitingForBite), null)]
    public static void FishingGUI_UpdateWaitingForBite_Prefix(ref float ____waiting_for_bite_delay)
    {
        ____waiting_for_bite_delay = 0f;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(FishingGUI), nameof(FishingGUI.UpdateWaitingForBite), null)]
    private static void FishingGUI_UpdateWaitingForBite_Postfix(FishingGUI __instance)
    {
        
        var fishy = __instance.GetRandomFish(out __instance._waiting_for_bite_delay);

        // var fishy = AccessTools.Method(typeof(FishingGUI), "GetRandomFish").Invoke(__instance, new object[]
        // {
        //     ____waiting_for_bite_delay
        // }) as FishDefinition;

        // var fishy = (FishDefinition) typeof(FishingGUI)
        //     .GetMethod("GetRandomFish", AccessTools.all)
        //     ?.Invoke(__instance, new object[]
        //     {
        //         ____waiting_for_bite_delay
        //     });

        __instance._fish_def = fishy;
        __instance._fish = new Item(__instance._fish_def.item_id, 1);
        __instance._fish_preset = Resources.Load<FishPreset>("MiniGames/Fishing/" + __instance._fish_def.fish_preset);

        __instance.ChangeState(FishingGUI.FishingState.WaitingForPulling);
        // AccessTools.Method(typeof(FishingGUI), "ChangeState").Invoke(__instance, new object[]
        // {
        //     FishingGUI.FishingState.WaitingForPulling
        // });

        // typeof(FishingGUI).GetMethod("ChangeState", AccessTools.all)
        //     ?.Invoke(__instance, new object[]
        //     {
        //         FishingGUI.FishingState.WaitingForPulling
        //     });
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(FishingGUI), nameof(FishingGUI.UpdateWaitingForPulling), null)]
    private static void FishingGUI_UpdateWaitingForPulling(FishingGUI __instance)
    {
       
        __instance.is_success_fishing = true;
        __instance.ChangeState(     FishingGUI.FishingState.Pulling);
        // AccessTools.Method(typeof(FishingGUI), "ChangeState").Invoke(__instance, new object[]
        // {
        //
        // });
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(FishingGUI), nameof(FishingGUI.UpdatePulling), null)]
    private static void FishingGUI_UpdatePulling(FishingGUI __instance)
    {
        __instance.ChangeState(FishingGUI.FishingState.TakingOut);
        // AccessTools.Method(typeof(FishingGUI), "ChangeState").Invoke(__instance, new object[]
        // {
        //     FishingGUI.FishingState.TakingOut
        // });
        // // typeof(FishingGUI).GetMethod("ChangeState", AccessTools.all)
        // //     ?.Invoke(__instance, new object[]
        // //     {
        // //         FishingGUI.FishingState.TakingOut
        // //     });
    }
}