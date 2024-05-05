using System;
using HarmonyLib;

namespace QoL.Patches;

[Harmony]
public static class KillQuestFix
{
    private static ActiveQuest Current { get; set; }    
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(HeroMerchant), nameof(HeroMerchant.CompleteQuest))]
    public static void HeroMerchant_CompleteQuest_Prefix(ref ActiveQuest activeQuest)
    {
        Current = activeQuest;
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(HeroMerchant), nameof(HeroMerchant.CompleteQuest))]
    public static void HeroMerchant_CompleteQuest_Postfix()
    {
        Current = null;
    }
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(ShopManager), nameof(ShopManager.RemoveItems))]
    public static bool ShopManager_RemoveItems(ItemMaster item, ref int quantity)
    {
        var run = Current != null && item.name != Current.quest.target && HeroMerchant.Instance.activeQuests.Exists(x => x.quest.killQuestTarget == item.name && x != Current);
        if (!run) return true;
        
        var failed = Current.failed;
        if (failed)
        {
            return false;
        }
        
        quantity = Current.quest.quantity;
        return true;
    }
}