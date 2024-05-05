using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MaxButtonControllerSupport;

public static class MaxButtonCrafting
{
    public static void AddMinAndMaxButtons(CraftItemGUI craftItemGUI, string parentButtonName, string minMaxButtonName, bool isMaximum, WorldGameObject crafteryWgo)
    {
        if (!craftItemGUI.current_craft.CanCraftMultiple())
        {
            return;
        }
        
        var parentButtonTransform = craftItemGUI.transform.Find($"selection frame/amount buttons/{parentButtonName}");
        var parentButtonSprite = parentButtonTransform.GetComponent<UI2DSprite>();
        var parentButtonCollider = parentButtonTransform.GetComponent<BoxCollider2D>();
        var parentButtonUI = parentButtonTransform.GetComponent<UIButton>();

        parentButtonTransform.localPosition = new Vector3(parentButtonTransform.localPosition.x, -10f, parentButtonTransform.localPosition.z);
        parentButtonSprite.SetDimensions(26, 26);
        parentButtonCollider.size = new Vector2(29.4f, 26f);

        var minMaxButton = Object.Instantiate(parentButtonTransform.gameObject, parentButtonTransform.parent);
        minMaxButton.name = minMaxButtonName;
        minMaxButton.transform.localPosition = new Vector3(minMaxButton.transform.localPosition.x, -31f, minMaxButton.transform.localPosition.z);

        var minMaxButtonUI = minMaxButton.GetComponent<UIButton>();
        minMaxButtonUI.normalSprite2D = parentButtonUI.normalSprite2D;
        minMaxButtonUI.hoverSprite2D = parentButtonUI.hoverSprite2D;
        minMaxButtonUI.pressedSprite2D = parentButtonUI.pressedSprite2D;
        minMaxButtonUI.onClick = new List<EventDelegate>();

        if (isMaximum)
        {
            EventDelegate.Add(minMaxButtonUI.onClick, delegate { SetMaximumAmount(ref craftItemGUI, ref crafteryWgo); });
        }
        else
        {
            EventDelegate.Add(minMaxButtonUI.onClick, delegate { SetMinimumAmount(ref craftItemGUI); });
        }

        // var uiEventTriggers = minMaxButton.GetComponents<UIEventTrigger>();
        // foreach (var trigger in uiEventTriggers)
        // {
        //     Object.Destroy(trigger);
        // }

        var arrowSpriteTransform = minMaxButton.transform.Find("arrow spr");
        arrowSpriteTransform.name = "arrow spr 1";
        arrowSpriteTransform.localPosition += new Vector3(4f, 0f, 0f);

        CloneAndPositionSprite(arrowSpriteTransform, "arrow spr 2", -4f);
        CloneAndPositionSprite(arrowSpriteTransform, "arrow spr 3", -8f);
  
    }
    
    private static void CloneAndPositionSprite(Transform spriteTransform, string spriteName, float xOffset)
    {
        var clonedSprite = Object.Instantiate(spriteTransform.gameObject, spriteTransform.parent);
        clonedSprite.name = spriteName;
        clonedSprite.transform.localPosition += new Vector3(xOffset, 0f, 0f);
    }


    internal static void SetMinimumAmount(ref CraftItemGUI craftItemGUI)
    {
        SetAmount(ref craftItemGUI, 1);
    }


    internal static void SetMaximumAmount(ref CraftItemGUI craftItemGUI, ref WorldGameObject crafteryWgo)
    {
        int maxCraftableFromWgo = 9999;
        int maxCraftableFromInventory = 9999;
        var multiInventory = GlobalCraftControlGUI.is_global_control_active ? GUIElements.me.craft.multi_inventory : MainGame.me.player.GetMultiInventoryForInteraction(null);

        foreach (var neededItemFromWgo in craftItemGUI.craft_definition.needs_from_wgo)
        {
            if (neededItemFromWgo != null && crafteryWgo != null && crafteryWgo.data != null && neededItemFromWgo.id == "fire" && neededItemFromWgo.value > 0)
            {
                maxCraftableFromWgo = crafteryWgo.data.GetTotalCount(neededItemFromWgo.id, true) / neededItemFromWgo.value;
            }

            if (maxCraftableFromWgo > 1) continue;
            SetAmount(ref craftItemGUI, 1);
            return;
        }

        foreach (var neededItemFromCraft in craftItemGUI.current_craft.needs)
        {
            var totalCountNeededItem = 0;
            if (multiInventory != null)
            {
                totalCountNeededItem += multiInventory.GetTotalCount(neededItemFromCraft.id);
            }

            if (totalCountNeededItem == 0 || totalCountNeededItem < neededItemFromCraft.value)
            {
                SetAmount(ref craftItemGUI, 1);
                return;
            }

            var maxCraftableFromCurrentItem = totalCountNeededItem / neededItemFromCraft.value;
            maxCraftableFromInventory = Math.Min(maxCraftableFromInventory, maxCraftableFromCurrentItem);
        }

        int finalMaxCraftable = Math.Min(maxCraftableFromInventory, maxCraftableFromWgo);
        finalMaxCraftable = Math.Max(finalMaxCraftable, 1);
        SetAmount(ref craftItemGUI, finalMaxCraftable);
    }


    private static void SetAmount(ref CraftItemGUI craftItemGUI, int amount)
    {
        craftItemGUI._amount = amount;
        craftItemGUI.Redraw();
        craftItemGUI.OnOver();
    }
}