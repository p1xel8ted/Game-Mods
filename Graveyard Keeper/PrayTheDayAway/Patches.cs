using System.Collections.Generic;
using System.Linq;
using GYKHelper;
using HarmonyLib;
using PrayTheDayAway.lang;
using UnityEngine;

namespace PrayTheDayAway;

[HarmonyPatch]
public partial class Plugin
{
    private static bool LostPrayerItem { get; set; }

    private static Item PrayerItem{ get; set; }
    private static bool ShowPrayerItem{ get; set; }

    private static void ProcessCraftDoodad(ref Item selectedItem)
    {
        if (CheatModeConfig.Value) return;
        var playerInv = MainGame.me.player.GetMultiInventory(exceptions: null, force_world_zone: "",
            player_mi: MultiInventory.PlayerMultiInventory.IncludePlayer, include_toolbelt: true,
            include_bags: true, sortWGOS: true);
        var item = playerInv.GetItem(selectedItem.id);

        if (item == null)
        {
            WriteLog($"Unable to find item {selectedItem.id} in player inventory");
            return;
        }

        ShowPrayerItem = false;
        PrayerItem = null;

        if (RandomlyUpgradeBasicPrayer.Value && item.id == "b_empty")
        {
            UpgradePrayer(playerInv, item);
        }

        if (SermonOverAndOver.Value || EverydayIsSermonDay.Value)
        {
            MainGame.me.player.SetParam("prayed_this_week", 0f);

            if (AlternateMode.Value)
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
        if (CheatModeConfig.Value) return;
        if (NotifyOnPrayerLoss.Value && LostPrayerItem)
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
        if (CheatModeConfig.Value) return;
        if (PrayerItem != null && ShowPrayerItem)
        {
            Tools.ShowLootAddedIcon(PrayerItem);
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(EnvironmentEngine), nameof(EnvironmentEngine.OnEndOfDay))]
    public static void EnvironmentEngine_OnEndOfDay()
    {
        if (CheatModeConfig.Value) return;
        if (EverydayIsSermonDay.Value)
        {
            WriteLog($"EnvironmentEngine_OnEndOfDay: EverydayIsSermonDay = true - New day, new sermon!");
            MainGame.me.player.SetParam("prayed_this_week", 0f);
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Stats), nameof(Stats.DesignEvent), typeof(string))]
    public static void Stats_DesignEvent(string event_name)
    {
        if (CheatModeConfig.Value) return;
        if (EverydayIsSermonDay.Value && event_name == "day")
        {
            WriteLog($"Stats.DesignEvent: EverydayIsSermonDay = true - New day, new sermon!");
            MainGame.me.player.SetParam("prayed_this_week", 0f);
        }
    }

    private static void RemovePrayCraft(MultiInventory inventory, Item item)
    {
        if (CheatModeConfig.Value) return;
        if (item.id == "b_empty") return;
        LostPrayerItem = false;
        WriteLog($"RemovePrayCraft: {item.id}");
        float roll = Random.Range(0, 101);
        var remove = false;
        if (item.id.EndsWith(":3"))
        {
            WriteLog($"Gold prayer item: 20% chance to lose it. Rolled {roll}/100.");
            remove = roll <= 20;
            //20% chance of loss
        }

        if (item.id.EndsWith(":2"))
        {
            WriteLog($"Silver prayer item: 40% chance to lose it. Rolled {roll}/100.");
            remove = roll <= 40;
        }

        if (item.id.EndsWith(":1"))
        {
            WriteLog($"Bronze prayer item: 60% chance to lose it. Rolled {roll}/100.");
            remove = roll <= 60;
        }

        if (remove)
        {
            LostPrayerItem = true;
            WriteLog($"Removed 1x {item.id}.");
            inventory.RemoveItem(item, 1);
        }
    }

    private static void UpgradePrayer(MultiInventory inventory, Item item)
    {
        if (CheatModeConfig.Value) return;
        if (item.id != "b_empty") return;
        WriteLog($"PrayItem: {item.id}, ItemDef: {item.definition.id}, LinkedCraft: {item.definition.linked_craft.id}");
        var faithItems = GameBalance.me.items_data
            .Where(a => a.type == ItemDefinition.ItemType.Preach)
            .Where(b => MainGame.me.save.unlocked_crafts.Contains(b.id.Split(':')[0]))
            .Where(c => c.id != "b_empty").ToList();
        var newItem = faithItems.RandomElement().id;
        inventory.RemoveItem(item, 1);

        PrayerItem = new Item(newItem, 1);
        ShowPrayerItem = true;
        inventory.AddItem(PrayerItem);

        WriteLog($"UpgradePrayer: Removing 1x {item.id} and adding 1x {newItem}.");
    }

    private static void LowerCraftLevel(MultiInventory inventory, Item item)
    {
        if (CheatModeConfig.Value) return;
        if (item.id == "b_empty") return;
        var oldItemSplit = item.id.Split(':');
        var oldItemName = oldItemSplit[0];
        var oldItemLevel = oldItemSplit[1];
        var oldItemLevelInt = int.Parse(oldItemLevel);

        float roll = Random.Range(0, 101);
        var downgrade = false;
        if (item.id.EndsWith(":3"))
        {
            WriteLog($"Gold prayer item: 20% chance to downgrade it. Rolled {roll}/100.");
            downgrade = roll <= 20;
            //20% chance of downgrade
        }

        if (item.id.EndsWith(":2"))
        {
            WriteLog($"Silver prayer item: 40% chance to downgrade it. Rolled {roll}/100.");
            downgrade = roll <= 40;
        }

        if (item.id.EndsWith(":1"))
        {
            WriteLog($"Bronze prayer item: 60% chance to downgrade it. Rolled {roll}/100.");
            downgrade = roll <= 60;
        }

        if (!downgrade) return;

        switch (oldItemLevelInt)
        {
            case > 1:
                var newItemLevel = oldItemLevelInt - 1;
                var newItem = oldItemName + ":" + newItemLevel;
                WriteLog($"LowerCraftLevel: Removing 1x {item.id} and adding 1x {newItem}.");
                inventory.RemoveItem(item, 1);
                PrayerItem = new Item(newItem, 1);
                ShowPrayerItem = true;
                inventory.AddItem(PrayerItem);

                break;
            case 1:
                WriteLog($"LowerCraftLevel: Removing 1x {item.id} and adding 1x b_empty.");
                inventory.RemoveItem(item, 1);
                PrayerItem = new Item("b_empty", 1);
                ShowPrayerItem = true;
                inventory.AddItem(PrayerItem);
                break;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(CustomScript), nameof(CustomScript.TerminateMe))]
    public static void CustomScript_TerminateMe(ref CustomScript __instance)
    {
        if (__instance == null ||string.IsNullOrEmpty(__instance.script_name)) return;
        if (Time.timeScale >1 && __instance.script_name.ToLowerInvariant().Equals("pray"))
        {
            Time.timeScale = 1f;
        }
    }


    [HarmonyPostfix]
    [HarmonyBefore("p1xel8ted.gyk.miscbitsandbobs")]
    [HarmonyPatch(typeof(PrayCraftGUI), nameof(PrayCraftGUI.OnPrayButtonPressed))]
    public static void PrayCraftGUI_OnPrayButtonPressed(ref PrayCraftGUI __instance)
    {
        if (__instance == null) return;
        WriteLog($"Selected sermon item is {__instance._selected_item?.id}.");
        if (CheatModeConfig.Value)
        {
            MainGame.me.player.SetParam("prayed_this_week", 0f);
            GoFast();
            return;
        }
        ProcessCraftDoodad(ref __instance._selected_item);
        GoFast();
    }

  

    private static void GoFast()
    {
        if (!SpeedUpSermon.Value) return;
        WriteLog($"goFast is true, setting timescale to {SermonSpeed.Value}. Original value is {Time.timeScale}");
        Time.timeScale = SermonSpeed.Value;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.Interact))]
    public static bool WorldGameObject_Interact(ref WorldGameObject __instance)
    {
        if (!__instance.obj_id.Contains("church_pulpit"))
        {
            WriteLog($"Not the pulpit, bailing!");
            return true;
        }

        if (!CheatModeConfig.Value)
        {
            if (!EverydayIsSermonDay.Value)
            {
                WriteLog("EverydayIsSermonDay = false, bailing!");
                return true;
            }

            if (EverydayIsSermonDay.Value && !SermonOverAndOver.Value &&
                MainGame.me.player.GetParam("prayed_this_week") >= 1)
            {
                WriteLog(
                    "EverydayIsSermonDay = true, SermonOverAndOver = false, AlreadyPrayedThisWeek = true - bailing!");
                return true;
            }
        }

        WriteLog($"All good, running the sermon!");
        // __instance.components.craft.Interact(MainGame.me.player, -1);
        if (CheatModeConfig.Value)
        {
            MainGame.me.player.SetParam("prayed_this_week", 0f);
        }
        
        GUIElements.me.pray_craft.Open(__instance);
        return false;
    }
}