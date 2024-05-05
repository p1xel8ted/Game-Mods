using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Helper;
using PrayTheDayAway.lang;
using UnityEngine;

namespace PrayTheDayAway;

[HarmonyPatch]
public static partial class MainPatcher
{
    private static bool _lostPrayerItem;
    private static Item _prayerItem;
    private static bool _showPrayerItem;

    private static void ProcessCraftDoodad(ref Item selectedItem)
    {
        var playerInv = MainGame.me.player.GetMultiInventory(exceptions: null, force_world_zone: "",
            player_mi: MultiInventory.PlayerMultiInventory.IncludePlayer, include_toolbelt: true,
            include_bags: true, sortWGOS: true);
        var item = playerInv.GetItem(selectedItem.id);

        if (item == null)
        {
            Log($"Unable to find item {selectedItem.id} in player inventory");
            return;
        }

        _showPrayerItem = false;
        _prayerItem = null;
        
        if (_cfg.RandomlyUpgradeBasicPrayer && item.id == "b_empty")
        {
            UpgradePrayer(playerInv, item);
        }

        if (_cfg.SermonOverAndOver || _cfg.EverydayIsSermonDay)
        {
            MainGame.me.player.SetParam("prayed_this_week", 0f);

            if (_cfg.AlternateMode)
            {
                LowerCraftLevel(playerInv, item);
                return;
            }

            RemovePrayCraft(playerInv, item);
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(PrayCraftGUI), nameof(PrayCraftGUI.OnMiddlePrayBuffAnimation))]
    public static void PrayCraftGUI_OnFinishedPrayBuffAnimation()
    {
        if (_cfg.NotifyOnPrayerLoss && _lostPrayerItem)
        {
            List<string> lostAnother = new()
            {
                GetLocalizedString(strings.M1),
                GetLocalizedString(strings.M2),
                GetLocalizedString(strings.M3),
                GetLocalizedString(strings.M4),
                GetLocalizedString(strings.M5),
                GetLocalizedString(strings.M6),
                GetLocalizedString(strings.M7),
                GetLocalizedString(strings.M8),
                GetLocalizedString(strings.M9)
            };

            Tools.ShowMessage(lostAnother.RandomElement(), MainGame.me.player_pos, EffectBubblesManager.BubbleColor.Red, speechBubbleType: SpeechBubbleGUI.SpeechBubbleType.InfoBox, sayAsPlayer: true);
        }
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(PrayCraftGUI), nameof(PrayCraftGUI.OnMiddlePrayBuffAnimation))]
    public static void PrayCraftGUI_OnMiddlePrayBuffAnimation()
    {
        if (_prayerItem != null && _showPrayerItem)
        {
            Tools.ShowLootAddedIcon(_prayerItem);
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(EnvironmentEngine), nameof(EnvironmentEngine.OnEndOfDay))]
    public static void EnvironmentEngine_OnEndOfDay()
    {
        if (_cfg.EverydayIsSermonDay)
        {
            Log($"EnvironmentEngine_OnEndOfDay: EverydayIsSermonDay = true - New day, new sermon!");
            MainGame.me.player.SetParam("prayed_this_week", 0f);
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Stats), nameof(Stats.DesignEvent), typeof(string))]
    public static void Stats_DesignEvent(string event_name)
    {
        if (_cfg.EverydayIsSermonDay && event_name == "day")
        {
            Log($"Stats.DesignEvent: EverydayIsSermonDay = true - New day, new sermon!");
            MainGame.me.player.SetParam("prayed_this_week", 0f);
        }
    }

    private static void RemovePrayCraft(MultiInventory inventory, Item item)
    {
        if (item.id == "b_empty") return;
        _lostPrayerItem = false;
        Log($"RemovePrayCraft: {item.id}");
        float roll = Random.Range(0, 101);
        var remove = false;
        if (item.id.EndsWith(":3"))
        {
            Log($"Gold prayer item: 20% chance to lose it. Rolled {roll}/100.");
            remove = roll <= 20;
            //20% chance of loss
        }

        if (item.id.EndsWith(":2"))
        {
            Log($"Silver prayer item: 40% chance to lose it. Rolled {roll}/100.");
            remove = roll <= 40;
        }

        if (item.id.EndsWith(":1"))
        {
            Log($"Bronze prayer item: 60% chance to lose it. Rolled {roll}/100.");
            remove = roll <= 60;
        }

        if (remove)
        {
            _lostPrayerItem = true;
            Log($"Removed 1x {item.id}.");
            inventory.RemoveItem(item, 1);
        }
    }

    private static void UpgradePrayer(MultiInventory inventory, Item item)
    {
        if (item.id != "b_empty") return;
        Log($"PrayItem: {item.id}, ItemDef: {item.definition.id}, LinkedCraft: {item.definition.linked_craft.id}");
        var faithItems = GameBalance.me.items_data
            .Where(a => a.type == ItemDefinition.ItemType.Preach)
            .Where(b => MainGame.me.save.unlocked_crafts.Contains(b.id.Split(':')[0]))
            .Where(c => c.id != "b_empty").ToList();
        var newItem = faithItems.RandomElement().id;
        inventory.RemoveItem(item, 1);
        
        _prayerItem = new Item(newItem, 1);
        _showPrayerItem = true;
        inventory.AddItem(_prayerItem);

        Log($"UpgradePrayer: Removing 1x {item.id} and adding 1x {newItem}.");
    }

    private static void LowerCraftLevel(MultiInventory inventory, Item item)
    {
        if (item.id == "b_empty") return;
        var oldItemSplit = item.id.Split(':');
        var oldItemName = oldItemSplit[0];
        var oldItemLevel = oldItemSplit[1];
        var oldItemLevelInt = int.Parse(oldItemLevel);

        float roll = Random.Range(0, 101);
        bool downgrade = false;
        if (item.id.EndsWith(":3"))
        {
            Log($"Gold prayer item: 20% chance to downgrade it. Rolled {roll}/100.");
            downgrade = roll <= 20;
            //20% chance of downgrade
        }

        if (item.id.EndsWith(":2"))
        {
            Log($"Silver prayer item: 40% chance to downgrade it. Rolled {roll}/100.");
            downgrade = roll <= 40;
        }

        if (item.id.EndsWith(":1"))
        {
            Log($"Bronze prayer item: 60% chance to downgrade it. Rolled {roll}/100.");
            downgrade = roll <= 60;
        }

        if (!downgrade) return;
  
        switch (oldItemLevelInt)
        {
            case > 1:
                var newItemLevel = oldItemLevelInt - 1;
                var newItem = oldItemName + ":" + newItemLevel;
                Log($"LowerCraftLevel: Removing 1x {item.id} and adding 1x {newItem}.");
                inventory.RemoveItem(item, 1);
                _prayerItem = new Item(newItem, 1);
                _showPrayerItem = true;
                inventory.AddItem(_prayerItem);
                
                break;
            case 1:
                Log($"LowerCraftLevel: Removing 1x {item.id} and adding 1x b_empty.");
                inventory.RemoveItem(item, 1);
                _prayerItem = new Item("b_empty", 1);
                _showPrayerItem = true;
                inventory.AddItem(_prayerItem);
                break;
        }
    }


    [HarmonyPostfix]
    [HarmonyBefore("p1xel8ted.GraveyardKeeper.MiscBitsAndBobs")]
    [HarmonyPatch(typeof(PrayCraftGUI), nameof(PrayCraftGUI.OnPrayButtonPressed))]
    public static void PrayCraftGUI_OnPrayButtonPressed(ref PrayCraftGUI __instance)
    {
        if (__instance == null) return;
        Log($"Selected sermon item is {__instance._selected_item?.id}.");

        ProcessCraftDoodad(ref __instance._selected_item);
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.Interact))]
    public static bool WorldGameObject_Interact(ref WorldGameObject __instance)
    {
        if (!__instance.obj_id.Contains("church_pulpit"))
        {
            Log($"Not the pulpit, bailing!");
            return true;
        }

        if (!_cfg.EverydayIsSermonDay)
        {
            Log("EverydayIsSermonDay = false, bailing!");
            return true;
        }

        if (_cfg.EverydayIsSermonDay && !_cfg.SermonOverAndOver && MainGame.me.player.GetParam("prayed_this_week") >= 1)
        {
            Log("EverydayIsSermonDay = true, SermonOverAndOver = false, AlreadyPrayedThisWeek = true - bailing!");
            return true;
        }

        Log($"All good, running the sermon!");
        // __instance.components.craft.Interact(MainGame.me.player, -1);
        GUIElements.me.pray_craft.Open(__instance);
        return false;
    }

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
}