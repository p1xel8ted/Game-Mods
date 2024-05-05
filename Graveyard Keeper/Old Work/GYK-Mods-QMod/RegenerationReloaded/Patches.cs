using System;
using HarmonyLib;
using Helper;
using RegenerationReloaded.lang;
using UnityEngine;

namespace RegenerationReloaded;

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
            _delay = _cfg.regenDelay;

            if (Time.time - CrossModFields.ConfigReloadTime > CrossModFields.ConfigReloadTimeLength)
            {
                Tools.ShowMessage(GetLocalizedString(strings.ConfigMessage), Vector3.zero);
                CrossModFields.ConfigReloadTime = Time.time;
            }
        }
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(PlayerComponent), nameof(PlayerComponent.Update))]
    public static void PlayerComponent_Update()
    {
        var energyRegen = Math.Abs(_cfg.energyRegen);
        var lifeRegen = Math.Abs(_cfg.lifeRegen);
        var player = MainGame.me.player;
        var save = MainGame.me.save;

        if (_delay == 0.0f)
        {
            if (player.energy < save.max_energy)
            {
                player.energy += energyRegen;
                if (!player.energy.EqualsOrMore(save.max_energy) && _cfg.showRegenUpdates)
                {
                    EffectBubblesManager.ShowStackedEnergy(player, energyRegen);
                }
            }

            if (player.hp < save.max_hp)
            {
                player.hp += lifeRegen;
                if (!player.hp.EqualsOrMore(save.max_hp) && _cfg.showRegenUpdates)
                {
                    EffectBubblesManager.ShowStackedHP(player, lifeRegen);
                }
            }

            _delay = _cfg.regenDelay;
        }
        else
        {
            _delay = _delay <= 0.0 ? 0.0f : _delay - Time.deltaTime;
        }
    }
}