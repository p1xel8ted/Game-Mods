using System.Linq;
using EconomyReloaded.lang;
using HarmonyLib;
using Helper;
using UnityEngine;

namespace EconomyReloaded;

public static partial class MainPatcher
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Trading),nameof(Trading.GetSingleItemCostInTraderInventory), typeof(Item), typeof(int))]
    public static void Trading_GetSingleItemCostInTraderInventory(ref float __result, Item item)
    {
        if (!_cfg.oldSchoolMode) return;
        if (!_cfg.disableInflation) return;
        if (__result != 0.0)
        {
            __result = item.definition.base_price;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Trading),nameof(Trading.GetSingleItemCostInPlayerInventory), typeof(Item), typeof(int))]
    public static void Trading_GetSingleItemCostInPlayerInventory(ref float __result, Item item)
    {
        if (!_cfg.oldSchoolMode) return;
        if (!_cfg.disableDeflation) return;
        if (__result != 0.0)
        {
            __result = item.definition.base_price;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameBalance), nameof(GameBalance.LoadGameBalance))]
    public static void GameBalance_LoadGameBalance()
    {
        if (_cfg.oldSchoolMode) return;
        if (_gameBalanceAlreadyRun) return;
        _gameBalanceAlreadyRun = true;

        foreach (var itemDef in GameBalance.me.items_data.Where(itemDef => itemDef.base_price > 0))
        {
            itemDef.is_static_cost = true;
        }
    }

    //hooks into the time of day update and saves if the K key was pressed
    [HarmonyPrefix]
    [HarmonyPatch(typeof(TimeOfDay), nameof(TimeOfDay.Update))]
    public static void TimeOfDay_Update()
    {
        if (Input.GetKeyUp(_cfg.reloadConfigKeyBind))
        {
            _cfg = Config.GetOptions();

            if (Time.time - CrossModFields.ConfigReloadTime > CrossModFields.ConfigReloadTimeLength)
            {
                Tools.ShowMessage(GetLocalizedString(strings.ConfigMessage), Vector3.zero);
                CrossModFields.ConfigReloadTime = Time.time;
            }
        }
    }
}