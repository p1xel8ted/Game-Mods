using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MaxButton
{
    public static class MaxButtonCrafting
    {
        private static FieldInfo _fieldAmount = typeof(CraftItemGUI).GetField("_amount", AccessTools.all);

        public static void AddMinAndMaxButtons(
          CraftItemGUI CraftItemGUI,
          string ParentButtonName,
          string MinMaxButtonName,
          bool isMaximum,
          WorldGameObject Craftery_wgo)
        {
            if (!CraftItemGUI.current_craft.CanCraftMultiple())
                return;
            var transform1 = CraftItemGUI.transform.Find("selection frame/amount buttons/" + ParentButtonName);
            transform1.transform.localPosition = new Vector3(transform1.transform.localPosition.x, -10f, transform1.transform.localPosition.z);
            transform1.GetComponent<UI2DSprite>().SetDimensions(26, 26);
            transform1.GetComponent<BoxCollider2D>().size = new Vector2(29.4f, 26f);
            var gameObject1 = Object.Instantiate(transform1.gameObject, transform1.parent);
            gameObject1.name = MinMaxButtonName;
            gameObject1.transform.localPosition = new Vector3(gameObject1.transform.localPosition.x, -31f, gameObject1.transform.localPosition.z);
            var component1 = transform1.GetComponent<UIButton>();
            var component2 = gameObject1.GetComponent<UIButton>();
            component2.normalSprite2D = component1.normalSprite2D;
            component2.hoverSprite2D = component1.hoverSprite2D;
            component2.pressedSprite2D = component1.pressedSprite2D;
            component2.onClick = new List<EventDelegate>();
            if (isMaximum)
                EventDelegate.Add(component2.onClick, () => SetMaximumAmount(ref CraftItemGUI, ref Craftery_wgo));
            else
                EventDelegate.Add(component2.onClick, () => SetMinimumAmount(ref CraftItemGUI));
            Object.Destroy(gameObject1.GetComponent<UIEventTrigger>());
            Object.Destroy(gameObject1.GetComponent<UIEventTrigger>());
            var transform2 = gameObject1.transform.Find("arrow spr");
            transform2.name = "arrow spr 1";
            transform2.transform.localPosition = new Vector3(transform2.transform.localPosition.x + 4f, transform2.transform.localPosition.y, transform2.transform.localPosition.z);
            var gameObject2 = Object.Instantiate(transform2.gameObject, transform2.parent);
            gameObject2.name = "arrow spr 2";
            gameObject2.transform.localPosition = new Vector3(gameObject2.transform.localPosition.x - 4f, gameObject2.transform.localPosition.y, gameObject2.transform.localPosition.z);
            var gameObject3 = Object.Instantiate(transform2.gameObject, transform2.parent);
            gameObject3.name = "arrow spr 3";
            gameObject3.transform.localPosition = new Vector3(gameObject3.transform.localPosition.x - 8f, gameObject3.transform.localPosition.y, gameObject3.transform.localPosition.z);
        }

        private static void SetMinimumAmount(ref CraftItemGUI CraftItemGUI) => SetAmount(ref CraftItemGUI, 1);

        private static void SetMaximumAmount(
          ref CraftItemGUI CraftItemGUI,
          ref WorldGameObject Craftery_wgo)
        {
            var val1 = 9999;
            var val21 = 9999;
            var multiInventory = !GlobalCraftControlGUI.is_global_control_active ? MainGame.me.player.GetMultiInventoryForInteraction() : GUIElements.me.craft.multi_inventory;
            foreach (var obj in CraftItemGUI.craft_definition.needs_from_wgo)
            {
                if (obj != null && Craftery_wgo != null && Craftery_wgo.data != null && obj.id == "fire" && obj.value > 0)
                    val21 = Craftery_wgo.data.GetTotalCount(obj.id) / obj.value;
                if (val21 > 1) continue;
                SetAmount(ref CraftItemGUI, 1);
                return;
            }
            foreach (var t in CraftItemGUI.current_craft.needs)
            {
                var num2 = 0;
                if (multiInventory != null)
                    num2 += multiInventory.GetTotalCount(t.id);
                if (num2 == 0 || num2 < t.value)
                {
                    SetAmount(ref CraftItemGUI, 1);
                    return;
                }
                var val22 = num2 / t.value;
                val1 = Math.Min(val1, val22);
            }
            var amount = Math.Max(Math.Min(val1, val21), 1);
            SetAmount(ref CraftItemGUI, amount);
        }

        private static void SetAmount(ref CraftItemGUI CraftItemGUI, int Amount)
        {
            if (_fieldAmount == null)
                _fieldAmount = typeof(CraftItemGUI).GetField("_amount", AccessTools.all);
            _fieldAmount?.SetValue(CraftItemGUI, Amount);
            CraftItemGUI.Redraw();
            CraftItemGUI.OnOver();
        }
    }
}