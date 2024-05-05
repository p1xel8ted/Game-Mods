using System;
using System.Linq;
using System.Threading;
using GYKHelper;
using HarmonyLib;
using NodeCanvas.BehaviourTrees;
using UnityEngine;
using WheresMaVeggies.lang;

namespace WheresMaVeggies;

[HarmonyPatch]
public static class Patches
{
    private static void MyDoPreZero(WorldGameObject wgo)
    {
        if (!Helpers.UnlockedStageOne())
        {
            Plugin.Log.LogMessage($"Haven't unlocked the necessary tech yet!");
            return;
        }
        
        if (wgo.is_player || wgo.obj_def.type == ObjectDefinition.ObjType.Mob)
        {
            var character = wgo.components.character;
            if (character.enabled && character.attack.enabled && character.attack.performing_attack)
                character.InterruptAttack();
            if (wgo.is_player && character.movement_state != MovementComponent.MovementState.None)
                character.StopMovement();
        }
        
        wgo._already_dropped_drop = false;
        if (!wgo.is_player && !wgo.is_dead && wgo.obj_def.type == ObjectDefinition.ObjType.Default)
        {
            wgo.DoZeroHPActivity();
        }
        else
        {
            var flag = true;
            if (wgo.components.animator != null)
            {
                foreach (var parameter in wgo.components.animator.parameters)
                {
                    if (parameter.type == AnimatorControllerParameterType.Trigger && parameter.name == "do_dying")
                        flag = false;
                }
            }

            if (flag)
            {
                wgo.DoZeroHPActivity();
            }
            else
            {
                wgo.components.animator.SetTrigger("do_dying");
                wgo.components.character.SetAnimationState(CharAnimState.Dying);
                wgo.is_dead = true;
                wgo.components.character.body.bodyType = (RigidbodyType2D) 2;
                if (wgo.obj_def.type == ObjectDefinition.ObjType.Mob)
                {
                    var component = wgo.wop.GetComponent<BehaviourTreeOwner>();
                    if (component != null)
                        component.enabled = false;
                }

                if (wgo.obj_def.do_drop_after_dying_anim_finished)
                    return;
                var items = ResModificator.ProcessItemsListBeforeDrop(wgo.obj_def.drop_items, wgo, MainGame.me.player);
                if (wgo.obj_def.drop_inventory_after_remove && string.IsNullOrEmpty(wgo.obj_def.after_hp_0.GetValue(wgo, MainGame.me.player)))
                {
                    items.AddRange(wgo.data.inventory.Where(obj => obj != null && !string.IsNullOrEmpty(obj.id) && obj.value >= 1));
                }

                if (!string.IsNullOrEmpty(wgo.obj_def.drop_sound))
                    Sounds.PlaySound(wgo.obj_def.drop_sound);
                wgo.DropItems(items);
                // AccessTools.Field(typeof(WorldGameObject), "_already_dropped_drop").SetValue(wgo, true);
                wgo._already_dropped_drop = true;
            }
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.DoPreZeroHPActivity))]
    public static void WorldGameObject_DoPreZeroHPActivity(ref WorldGameObject __instance)
    {
        if (!Helpers.UnlockedStageOne())
        {
            Plugin.Log.LogMessage($"Haven't unlocked the necessary tech yet!");
            return;
        }
        
        if (!__instance.obj_id.ToLowerInvariant().StartsWith("garden") || !__instance.obj_id.ToLowerInvariant().EndsWith("ready")) return;
        
        var objects = Helpers.FindNearbyObjectsByVector(__instance);

        if (objects == null)
        {
            return;
        }

        objects.Sort((a, b) => a.pos3.x.CompareTo(b.pos3.x));

        var o = __instance;
        foreach (var obj in objects.Where(obj => obj != o))
        {
            MyDoPreZero(obj);
        }
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(TechTreeGUIItem), nameof(TechTreeGUIItem.InitGamepadTooltip))]
    public static void TechTreeGUIItem_InitGamepadTooltip(ref TechTreeGUIItem __instance)
    {
        //
        if (__instance == null) return;
        {
            Thread.CurrentThread.CurrentUICulture = CrossModFields.Culture;
            if (!LazyInput.gamepad_active) return;
            var component = __instance.GetComponent<Tooltip>();

            if (__instance.tech_id.ToLowerInvariant().Contains(Helpers.ControllerUnlockTechTooltip.ToLowerInvariant()))
            {
                component.AddData(new BubbleWidgetSeparatorData());
                component.AddData(new BubbleWidgetTextData(Helpers.GetLocalizedString(strings.Stage1Header), UITextStyles.TextStyle.HintTitle));
                component.AddData(new BubbleWidgetTextData(Helpers.GetLocalizedString(strings.Stage1Des), UITextStyles.TextStyle.TinyDescription, NGUIText.Alignment.Left));
            }
        }
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(TechUnlock), nameof(TechUnlock.GetTooltip), typeof(Tooltip))]
    public static void TechUnlock_GetTooltip(ref TechUnlock __instance, ref Tooltip tooltip)
    {
        //
        if (__instance != null)
        {
            Thread.CurrentThread.CurrentUICulture = CrossModFields.Culture;
            if (LazyInput.gamepad_active) return;
            var name = __instance.GetData().name;
            if (name.ToLowerInvariant().Contains(Helpers.MouseUnlockTechTooltip.ToLowerInvariant()))
            {
                tooltip.AddData(new BubbleWidgetBlankSeparatorData());
                tooltip.AddData(new BubbleWidgetTextData(Environment.NewLine + Helpers.GetLocalizedString(strings.Stage1Header), UITextStyles.TextStyle.HintTitle, NGUIText.Alignment.Left));
                tooltip.AddData(new BubbleWidgetTextData(Helpers.GetLocalizedString(strings.Stage1Des), UITextStyles.TextStyle.TinyDescription, NGUIText.Alignment.Left));
            }
        }
    }
}