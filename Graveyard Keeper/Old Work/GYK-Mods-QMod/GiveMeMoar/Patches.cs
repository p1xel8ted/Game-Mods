using DLCRefugees;
using GiveMeMoar.lang;
using HarmonyLib;
using Helper;
using UnityEngine;
using Tools = Helper.Tools;

namespace GiveMeMoar;

[HarmonyPatch]
public static partial class MainPatcher
{
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

    [HarmonyPrefix]
    [HarmonyPatch(typeof(PrayLogics), nameof(PrayLogics.SpreadFaithIncome))]
    private static void PrayLogics_SpreadFaithIncome(ref int faith)
    {

        if (!MainGame.game_started) return;
        if (_cfg.faithMultiplier > 0)
        {
            var newFaith = faith * _cfg.faithMultiplier;
            Log($"Original Faith: {faith}, Multiplier: {_cfg.faithMultiplier}, NewFaith: {newFaith}");
            faith = Mathf.RoundToInt(newFaith);
        }
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(SoulsHelper), nameof(SoulsHelper.CalculatePointsAfterSoulRelease))]
    private static void SoulsHelper_CalculatePointsAfterSoulRelease(ref float __result)
    {

        if (!MainGame.game_started) return;
        if (_cfg.gratitudeMultiplier > 0)
        {
            var grat = __result * _cfg.gratitudeMultiplier;
            Log($"Original gratitude: {__result}, Multiplier: {_cfg.gratitudeMultiplier}, New gratitude: {grat}");
            __result = Mathf.RoundToInt(grat);
        }
    }


    // [HarmonyPrefix]
    // [HarmonyPatch(typeof(DropResGameObject), "Drop", typeof(Vector3), typeof(Item), typeof(Transform),
    //     typeof(Direction), typeof(float), typeof(int), typeof(bool), typeof(bool))]
    [HarmonyPrefix]
    [HarmonyPatch(typeof(DropResGameObject), nameof(DropResGameObject.DoDrop), typeof(Item),typeof(int), typeof(bool))]
    private static void DropResGameObject_Drop(ref DropResGameObject __instance, ref Item drop_item)
    {
        if (!MainGame.game_started) return;
        


        if (drop_item == null) return;


        Log($"InstanceNull: {__instance == null}, ItemNull: {drop_item == null}");
        if (drop_item.definition.type == ItemDefinition.ItemType.BodyUniversalPart)
        {
            Log($"Item {drop_item.id} is a body part, skipping!");
            return;
        }

        if (drop_item.id.Contains("stick") & _cfg.disableSticks)
        {
            Log($"Item: {drop_item.id}, Stick is disabled in config. Skipping.");
            return;
        }

        Log($"Dropping {drop_item.id} of item type {drop_item.definition.type} with quantity {drop_item.value}.");

        if (DropList.Contains(drop_item.id) && _cfg.resourceMultiplier > 0)
        {
            var newValue = drop_item.value * _cfg.resourceMultiplier;
            Log($"Item: {drop_item.id}, Original item amount: {drop_item.value}, Multiplier: {_cfg.resourceMultiplier}, New item amount: {newValue}");
            drop_item.value = Mathf.RoundToInt(newValue);
            return;
        }

        if (drop_item.id == "sin_shard" && _cfg.sinShardMultiplier > 0)
        {
            var newValue = drop_item.value * _cfg.sinShardMultiplier;
            Log($"Original sin shard amount: {drop_item.value}, Multiplier: {_cfg.sinShardMultiplier}, New sin shard amount: {newValue}");
            drop_item.value = Mathf.RoundToInt(newValue);
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(RefugeesCampEngine), nameof(RefugeesCampEngine.UpdateHappiness))]
    private static void RefugeesCampEngine_UpdateHappiness(ref float happiness_delta)
    {

        if (!MainGame.game_started) return;
        if (_cfg.happinessMultiplier > 0)
        {
            var newHappiness = happiness_delta * _cfg.happinessMultiplier;
            Log($"Original Happiness: {happiness_delta}, Multiplier: {_cfg.happinessMultiplier}, NewHappiness: {newHappiness}");
            happiness_delta = newHappiness;
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(PrayLogics), nameof(PrayLogics.SpreadMoneyIncome))]
    private static void PrayLogics_SpreadMoneyIncome(ref float money)
    {
   

        if (!MainGame.game_started) return;
        if (_cfg.donationMultiplier > 0)
        {
            var newMoney = money * _cfg.donationMultiplier;
            Log($"Original Money: {money}, Multiplier: {_cfg.donationMultiplier}, NewMoney: {newMoney}");
            money = Mathf.RoundToInt(newMoney);
        }
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(TechPointsDrop), nameof(TechPointsDrop.Drop), typeof(Vector3), typeof(int), typeof(int), typeof(int))]
    private static void TechPointsDrop_Drop(ref int r, ref int g, ref int b)
    {
    

        if (!MainGame.game_started) return;
        if (_cfg.redTechPointMultiplier > 0)
        {
            var newRed = r * _cfg.redTechPointMultiplier;
            Log($"Original Red: {r}, Multiplier: {_cfg.redTechPointMultiplier}, NewRed: {newRed}");
            r = Mathf.RoundToInt(newRed);
        }

        if (_cfg.greenTechPointMultiplier > 0)
        {
            var newGreen = g * _cfg.greenTechPointMultiplier;
            Log($"Original Green: {g}, Multiplier: {_cfg.greenTechPointMultiplier}, NewGreen: {newGreen}");
            g = Mathf.RoundToInt(newGreen);
        }

        if (_cfg.blueTechPointMultiplier > 0)
        {
            var newBlue = b * _cfg.blueTechPointMultiplier;
            Log($"Original Blue: {b}, Multiplier: {_cfg.blueTechPointMultiplier}, NewBlue: {newBlue}");
            b = Mathf.RoundToInt(newBlue);
        }
    }
}