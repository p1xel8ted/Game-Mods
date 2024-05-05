using System.Linq;
using System.Threading;
using Exhaustless.lang;
using HarmonyLib;
using Helper;
using UnityEngine;
using Tools = Helper.Tools;

namespace Exhaustless;

[HarmonyPatch]
public static partial class MainPatcher
{
    //hooks into the time of day update and saves if the K key was pressed
    [HarmonyPrefix]
    [HarmonyPatch(typeof(TimeOfDay), nameof(TimeOfDay.Update))]
    public static void TimeOfDay_Update()
    {
        if (Input.GetKeyUp(_cfg.ReloadConfigKeyBind))
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
    [HarmonyPatch(typeof(BuffsLogics), nameof(BuffsLogics.AddBuff))]
    public static void BuffsLogics_AddBuff(ref string buff_id)
    {
        if (!_cfg.YawnMessage) return;
        if (buff_id.Equals("buff_tired"))
            Thread.CurrentThread.CurrentUICulture = CrossModFields.Culture;
        MainGame.me.player.Say(strings.Yawn, null, null,
            SpeechBubbleGUI.SpeechBubbleType.Think, SmartSpeechEngine.VoiceID.None, true);
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(CraftComponent), nameof(CraftComponent.TrySpendPlayerGratitudePoints))]
    public static void CraftComponent_TrySpendPlayerGratitudePoints(ref float value)
    {
        if (_cfg.SpendHalfGratitude) value /= 2f;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameBalance), nameof(GameBalance.LoadGameBalance))]
    public static void GameBalance_LoadGameBalance()
    {
        if (!_cfg.MakeToolsLastLonger) return;
        foreach (var itemDef in GameBalance.me.items_data.Where(a => ToolItems.Contains(a.type)))
        {
            if (itemDef.durability_decrease_on_use)
            {
                itemDef.durability_decrease_on_use_speed = 0.005f;
            }
        }
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(MainGame), nameof(MainGame.OnEquippedToolBroken))]
    public static void MainGame_OnEquippedToolBroken()
    {
        if (!_cfg.AutoEquipNewTool) return;
        var equippedTool = MainGame.me.player.GetEquippedTool();
        var save = MainGame.me.save;
        var playerInv = save.GetSavedPlayerInventory();

        foreach (var item in playerInv.inventory.Where(item =>
                     item.definition.type == equippedTool.definition.type))
        {
            if (item.durability_state is not (Item.DurabilityState.Full or Item.DurabilityState.Used))
                continue;
            MainGame.me.player.EquipItem(item, -1, playerInv.is_bag ? playerInv : null);
            MainGame.me.player.Say(
                $"{strings.LuckyHadAnotherPartOne} {item.definition.GetItemName()} {strings.LuckyHadAnotherPartTwo}",
                null, false,
                SpeechBubbleGUI.SpeechBubbleType.Think,
                SmartSpeechEngine.VoiceID.None, true);
        }
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(PlayerComponent), nameof(PlayerComponent.SpendSanity))]
    public static void PlayerComponent_SpendSanity(ref float need_sanity)
    {
        if (_cfg.SpendHalfSanity) need_sanity /= 2f;
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(PlayerComponent), nameof(PlayerComponent.TrySpendEnergy))]
    public static void PlayerComponent_TrySpendEnergy(ref float need_energy)
    {
        if (_cfg.SpendHalfEnergy) need_energy /= 2f;
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(SleepGUI), nameof(SleepGUI.Update))]
    public static void SleepGUI_Update()
    {
        if (!_cfg.SpeedUpSleep) return;
        MainGame.me.player.energy += 0.25f;
        MainGame.me.player.hp += 0.25f;
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(WaitingGUI), nameof(WaitingGUI.Update))]
    public static void WaitingGUI_Update_Postfix(WaitingGUI __instance)
    {
        if (!_cfg.AutoWakeFromMeditation) return;
        var save = MainGame.me.save;
        if (MainGame.me.player.energy.EqualsOrMore(save.max_hp) &&
            MainGame.me.player.hp.EqualsOrMore(save.max_energy))
            __instance.StopWaiting();
            // typeof(WaitingGUI).GetMethod("StopWaiting", AccessTools.all)
            //     ?.Invoke(__instance, null);
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(WaitingGUI), nameof(WaitingGUI.Update))]
    public static void WaitingGUI_Update_Prefix()
    {
        if (!_cfg.SpeedUpMeditation) return;
        MainGame.me.player.energy += 0.25f;
        MainGame.me.player.hp += 0.25f;
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.EquipItem))]
    public static void WorldGameObject_EquipItem(ref Item item)
    {
        if (!_cfg.MakeToolsLastLonger) return;
        if (! ToolItems.Contains(item.definition.type)) return;
        if (item.definition.durability_decrease_on_use)
        {
            item.definition.durability_decrease_on_use_speed = 0.005f;
        }
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.GetParam))]
    private static void WorldGameObject_GetParam(ref WorldGameObject __instance, ref string param_name,
        ref float __result)
    {
        if (!param_name.Contains("tiredness")) return;
        var tiredness = __instance._data.GetParam("tiredness");

        var newTirednessLimit = (float) _cfg.EnergySpendBeforeSleepDebuff;
        __result = tiredness < newTirednessLimit ? 250 : 350;
    }
}