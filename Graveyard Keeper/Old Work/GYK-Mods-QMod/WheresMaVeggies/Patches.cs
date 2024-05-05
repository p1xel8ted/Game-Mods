using System;
using System.Linq;
using System.Threading;
using HarmonyLib;
using Helper;
using NodeCanvas.BehaviourTrees;
using UnityEngine;
using WheresMaVeggies.lang;

namespace WheresMaVeggies;

[HarmonyPatch]
public static partial class MainPatcher
{
    private static void MyDoPreZero(WorldGameObject wgo)
    {
        if (!UnlockedStageOne())
        {
            Log($"Haven't unlocked the necessary tech yet!");
            return;
        }

        Log("Do my pre zero HP activity on " + wgo.name);
        if (wgo.is_player || wgo.obj_def.type == ObjectDefinition.ObjType.Mob)
        {
            var character = wgo.components.character;
            if (character.enabled && character.attack.enabled && character.attack.performing_attack)
                character.InterruptAttack();
            if (wgo.is_player && character.movement_state != MovementComponent.MovementState.None)
                character.StopMovement();
        }

        // AccessTools.Field(typeof(WorldGameObject), "_already_dropped_drop").SetValue(wgo, false);
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
        if (!UnlockedStageOne())
        {
            Log($"Haven't unlocked the necessary tech yet!");
            return;
        }

        Log($"DDoPreZeroHPActivity: {__instance.obj_id}");
        if (__instance.obj_id.ToLowerInvariant().StartsWith("garden") && __instance.obj_id.ToLowerInvariant().EndsWith("ready"))
        {
            var objects = FindNearbyObjectsByVector(__instance);

            if (objects != null)
            {
                objects = objects.OrderBy(o => o.pos3.x).ToList();
                //  objects = objects.ToList().Sort(a=>b)
                var o = __instance;
                foreach (var obj in objects.Where(a => a != o))
                {
                    // Log($"Found items: {obj.obj_id} - X: {obj.pos3.x}, Y: {obj.pos3.y}, Z: {obj.pos3.z}");
                    MyDoPreZero(obj);
                }
            }
        }
    }

  // private static bool _printedTechs;

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

            if (__instance.tech_id.ToLowerInvariant().Contains(ControllerUnlockTechTooltip.ToLowerInvariant()))
            {
                component.AddData(new BubbleWidgetSeparatorData());
                component.AddData(new BubbleWidgetTextData(GetLocalizedString(strings.Stage1Header), UITextStyles.TextStyle.HintTitle));
                component.AddData(new BubbleWidgetTextData(GetLocalizedString(strings.Stage1Des), UITextStyles.TextStyle.TinyDescription, NGUIText.Alignment.Left));
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
           // Log($"MouseTooltip: {name}");
//p_collector

            if (name.ToLowerInvariant().Contains(MouseUnlockTechTooltip.ToLowerInvariant()))
            {
                tooltip.AddData(new BubbleWidgetBlankSeparatorData());
                tooltip.AddData(new BubbleWidgetTextData(Environment.NewLine + GetLocalizedString(strings.Stage1Header), UITextStyles.TextStyle.HintTitle, NGUIText.Alignment.Left));
                tooltip.AddData(new BubbleWidgetTextData(GetLocalizedString(strings.Stage1Des), UITextStyles.TextStyle.TinyDescription, NGUIText.Alignment.Left));
            }
        }
    }
}