namespace WheresMaVeggies;

[Harmony]
public static class Patches
{
    private static void MyDoPreZero(WorldGameObject wgo)
    {
        if (!Helpers.IsReady())
        {
            if (Plugin.DebugEnabled)
            {
                Plugin.WriteLog($"[MyDoPreZero] skip '{wgo.obj_id}': Farmer perk required but not yet unlocked.");
            }
            return;
        }

        if (Plugin.DebugEnabled)
        {
            Plugin.WriteLog($"[MyDoPreZero] harvesting neighbour '{wgo.obj_id}' at ({wgo.pos3.x:F0}, {wgo.pos3.y:F0}).");
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
    public static void WorldGameObject_DoPreZeroHPActivity(WorldGameObject __instance)
    {
        if (!Helpers.IsReady())
        {
            if (Plugin.DebugEnabled)
            {
                Plugin.WriteLog($"[DoPreZeroHPActivity] skip '{__instance.obj_id}': Farmer perk required but not yet unlocked.");
            }
            return;
        }

        var lowerId = __instance.obj_id.ToLowerInvariant();
        if (!lowerId.StartsWith("garden") || !lowerId.EndsWith("ready"))
        {
            return;
        }

        if (Plugin.DebugEnabled)
        {
            Plugin.WriteLog($"[DoPreZeroHPActivity] seed '{__instance.obj_id}' harvested — scanning for same-crop neighbours.");
        }

        var objects = Helpers.FindNearbyObjectsByVector(__instance);
        if (objects == null || objects.Count == 0)
        {
            if (Plugin.DebugEnabled)
            {
                Plugin.WriteLog($"[DoPreZeroHPActivity] no neighbours matched '{__instance.obj_id}' within range.");
            }
            return;
        }

        objects.Sort((a, b) => a.pos3.x.CompareTo(b.pos3.x));

        var harvested = 0;
        foreach (var obj in objects.Where(obj => obj != __instance))
        {
            MyDoPreZero(obj);
            harvested++;
        }

        if (Plugin.DebugEnabled)
        {
            Plugin.WriteLog($"[DoPreZeroHPActivity] cascade finished — {harvested} neighbour(s) harvested alongside seed '{__instance.obj_id}'.");
        }
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(TechTreeGUIItem), nameof(TechTreeGUIItem.InitGamepadTooltip))]
    public static void TechTreeGUIItem_InitGamepadTooltip(TechTreeGUIItem __instance)
    {
        if (__instance == null) return;

        if (!LazyInput.gamepad_active) return;
        Lang.Reload();
        var component = __instance.GetComponent<Tooltip>();

        if (__instance.tech_id.ToLowerInvariant().Contains(Helpers.ControllerUnlockTechTooltip.ToLowerInvariant()))
        {
            component.AddData(new BubbleWidgetSeparatorData());
            component.AddData(new BubbleWidgetTextData(Lang.Get("Stage1Header"), UITextStyles.TextStyle.HintTitle));
            component.AddData(new BubbleWidgetTextData(Lang.Get("Stage1Des"), UITextStyles.TextStyle.TinyDescription, NGUIText.Alignment.Left));
            if (Plugin.DebugEnabled)
            {
                Plugin.WriteLog($"[Tooltip/Gamepad] appended mass-harvest blurb to tech '{__instance.tech_id}'.");
            }
        }
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(TechUnlock), nameof(TechUnlock.GetTooltip), typeof(Tooltip))]
    public static void TechUnlock_GetTooltip(TechUnlock __instance, Tooltip tooltip)
    {
        if (__instance == null) return;

        if (LazyInput.gamepad_active) return;
        Lang.Reload();
        var name = __instance.GetData().name;
        if (name.ToLowerInvariant().Contains(Helpers.MouseUnlockTechTooltip.ToLowerInvariant()))
        {
            tooltip.AddData(new BubbleWidgetBlankSeparatorData());
            tooltip.AddData(new BubbleWidgetTextData(Environment.NewLine + Lang.Get("Stage1Header"), UITextStyles.TextStyle.HintTitle, NGUIText.Alignment.Left));
            tooltip.AddData(new BubbleWidgetTextData(Lang.Get("Stage1Des"), UITextStyles.TextStyle.TinyDescription, NGUIText.Alignment.Left));
            if (Plugin.DebugEnabled)
            {
                Plugin.WriteLog($"[Tooltip/Mouse] appended mass-harvest blurb to tech '{name}'.");
            }
        }
    }
}