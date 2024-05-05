using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using HarmonyLib;
using Helper;
using UnityEngine;
using WheresMaStorage.lang;

namespace WheresMaStorage;

public static partial class MainPatcher
{
    private static IEnumerator LoadInventories(Action callback = null)
    {
        Log("Loading inventories!");
        _mi = new MultiInventory();
        _refugeeMi = new MultiInventory();
        _mi.all.Clear();
        _refugeeMi.all.Clear();
        // var playerInv = new Inventory(MainGame.me.player);
        // playerInv.data.SetInventorySize(_invSize);
        //
        // _mi.AddInventory(playerInv);
        
        var inventory = new Inventory(MainGame.me.player);
        _mi.AddInventory(inventory, 0);
        inventory.data.SetInventorySize(_invSize);

            var toolbelt = new Item()
            {
                inventory = MainGame.me.player.data.secondary_inventory,
                inventory_size = 7
            };
            _mi.AddInventory(new Inventory(toolbelt));
            

        //var zones = (List<WorldZone>) AccessTools.Field(typeof(WorldZone), "_all_zones").GetValue(null);
        var zones = WorldZone._all_zones;
        var watch = Stopwatch.StartNew();


        foreach (var zone in zones)
        {
            var get = MainGame.me.save.known_world_zones.Exists(a => string.Equals(a, zone.id));
            if (!get) continue;
            var worldZoneMulti =
                zone.GetMultiInventory(player_mi: MultiInventory.PlayerMultiInventory.ExcludePlayer,
                    sortWGOS: true);
            if (worldZoneMulti == null) continue;
            foreach (var inv in worldZoneMulti) // && inv.data.inventory.Count != 0))
            {
                Log($"InventoryName: {inv._obj_id}");
                if (inv._obj_id.Length <= 0)
                    inv.data.sub_name = "Unknown" + "#" + zone.id;
                else
                    inv.data.sub_name = inv._obj_id + "#" + zone.id;


                if (zone.id.ToLowerInvariant().Contains("refugee"))
                {
                    _refugeeMi.AddInventory(inv);
                }
                else
                {
                    _mi.AddInventory(inv);
                }
            }
        }

        for (var i = 0; i < _mi.all.Count; i++)
        {
            foreach (var data in _mi.all[i].data.inventory.Where(data => data != null && !data.IsEmpty() && data.is_bag))
            {
                _mi.AddInventory(new Inventory(data, data.id), i + 1);
            }
        }
        
        watch.Stop();
        _invsLoaded = true;
        Log($"Inventory Loading Complete! Completed in {watch.ElapsedMilliseconds}ms");
        callback?.Invoke();
        yield return true;
    }

    private static IEnumerator LoadWildernessInventories(Action callback = null)
    {
        Log("Loading wilderness inventories!");
        var wgos = CrossModFields.WorldObjects.Where(a =>
            a.data.inventory_size > 0 && string.IsNullOrEmpty(a.GetMyWorldZoneId()));
        var watch = Stopwatch.StartNew();
        WildernessMultiInventories.Clear();
        WildernessInventories.Clear();

        foreach (var wgo in wgos)
        {
            if (wgo.obj_def.inventory_size <= 0 || ExcludeTheseWildernessInventories.Any(wgo.obj_id.Contains) || wgo.data.inventory_size == _cfg.AdditionalInventorySpace) continue;

            var zoneId = wgo.GetMyWorldZoneId();
            wgo.data.sub_name = wgo.obj_id + "#" + zoneId;

            if (!string.IsNullOrEmpty(zoneId)) continue;
            if (wgo.unique_id.ToString().Length <= 5) continue;

            if (wgo.custom_tag.Equals(ShippingBoxTag) || wgo.data.drop_zone_id.Equals(ShippingBoxTag)) continue;

            var exists = WildernessMultiInventories.ContainsKey(wgo);
            if (!exists) WildernessMultiInventories.Add(wgo, wgo.GetMultiInventoryOfWGOWithoutWorldZone());

            var invCount = 0;
            foreach (var inv in wgo.GetMultiInventoryOfWGOWithoutWorldZone().all.Where(inv => !WildernessInventories.Contains(inv)))
            {
                invCount++;
                WildernessInventories.Add(inv);
                Log($"Added {wgo.obj_id}'s ({zoneId}, {wgo.pos3}) Inventory {inv._obj_id} (#{invCount}) - CurrentGD: {wgo.cur_gd_point}, Unique ID: {wgo.unique_id} to WildernessInventories.");
            }
        }

        watch.Stop();
        Log($"Wilderness Inventory Loading Complete! Completed in {watch.ElapsedMilliseconds}ms.");
        callback?.Invoke();
        yield break;
    }

    private static void SetInventorySizeText(BaseInventoryWidget inventoryWidget)
    {
        //Log($"[Bag]: {inventoryWidget.inventory_data.is_bag}, ID: {inventoryWidget.inventory_data.id}");
        if (inventoryWidget.inventory_data.id.Contains(Writer)) return;
        if (inventoryWidget.header_label.text.Contains(Gerry)) return;
        if (!_cfg.ShowWorldZoneInTitles && !_cfg.ShowUsedSpaceInTitles) return;

        string wzLabel;
        string objId;
        bool isPlayer;
        var subNameSplit = inventoryWidget.inventory_data.sub_name.Split('#');
        if (string.IsNullOrEmpty(subNameSplit[0]))
        {
            objId = GetLocalizedString(strings.Player);
            isPlayer = true;
        }
        else
        {
            objId = GJL.L(subNameSplit[0].ToLowerInvariant().Trim() + "_inventory");
            isPlayer = false;
        }

        var zoneId = string.Empty;
        if (subNameSplit.Length > 1) zoneId = subNameSplit[1].ToLowerInvariant().Trim();

        if (inventoryWidget.inventory_data.sub_name.Length > 0)
        {
            var wzId = WorldZone.GetZoneByID(zoneId, false);
            wzLabel = wzId != null ? GJL.L("zone_" + wzId.id) : GetLocalizedString(strings.Wilderness);
        }
        else
        {
            wzLabel = GetLocalizedString(strings.Wilderness);
        }

        var cultureInfo = CrossModFields.Culture;
        var textInfo = cultureInfo.TextInfo;
        wzLabel = textInfo.ToTitleCase(wzLabel);

        var test = new Inventory(inventoryWidget.inventory_data.MakeInventoryCopy());
        var cap = test.size;
        var used = test.data.inventory.Count;

        inventoryWidget.header_label.overflowMethod = UILabel.Overflow.ResizeFreely;

        var header = objId;

        if (inventoryWidget.inventory_data.is_bag) // || objId.StartsWith("bookcase_f_broken"))
        {
            header = GJL.L(inventoryWidget.inventory_data.id);
        }

        //failed to translate or something
        if (header.Contains("_"))
        {
            header = GJL.L(inventoryWidget.inventory_data.id);
        }

        if (_cfg.ShowWorldZoneInTitles && !isPlayer) header = string.Concat(header, $" ({wzLabel})");

        if (_cfg.ShowUsedSpaceInTitles) header = string.Concat(header, $" - {used}/{cap}");

        inventoryWidget.header_label.text = header;
    }

    internal static MultiInventory GetMiInventory(string requester)
    {
        if (requester.Contains("refugee_builddesk") || requester.Contains("storage_builddesk"))
        {
            Log($"{requester} detected, refreshing inventories! Requester: {requester}");
            MainGame.me.StartCoroutine(LoadInventories());
            MainGame.me.StartCoroutine(LoadWildernessInventories());
        }

        // if (requester.Contains("storage_builddesk"))
        // {
        //     Log($"{requester} detected, refreshing inventories! Requester: {requester}");
        //     MainGame.me.StartCoroutine(LoadInventories());
        // } 

        if (WildernessInventories.Count <= 0)
        {
            Log($"WildernessInventory count <= 0! Requester: {requester}");
           return _mi;
        }

        foreach (var inv in WildernessInventories.Where(inv => !_mi.all.Contains(inv)))
        {
            Log($"Added WildernessInventory to MultiInventory Request! Requester: {requester}");
            _mi.AddInventory(inv);
        }

        if (!requester.Contains("zombie")) Log($"MultiInventory Request! Requester: {requester}");


        return _mi;
    }

    //this method gets inserted into the CraftReally method using the transpiler below, overwriting any inventory the game sets during crafting
    public static MultiInventory GetMi(CraftDefinition craft, MultiInventory orig, WorldGameObject otherGameObject)
    {
        if (!_cfg.SharedInventory) return orig;
        if (otherGameObject.GetMyWorldZoneId().Contains("refugee") || otherGameObject.obj_id.Contains("refugee"))
        {
            if (!(Time.time - _timeSeven > LogGap)) return orig;
            _timeSeven = Time.time;
            Log($"[InvRedirect]: Original inventory sent back to requester! IsPlayer: {otherGameObject.is_player}, Object: {otherGameObject.obj_id}, Craft: {craft.id}, Gratitude: {_gratitudeCraft}");
            return orig;
        }

        if (otherGameObject.obj_id.Contains("storage_builddesk"))
        {
            Log($"{otherGameObject.obj_id} detected, refreshing inventories! Requester: {otherGameObject.obj_id}");
            MainGame.me.StartCoroutine(LoadInventories());
        }

        // var wz = otherGameObject.GetMyWorldZone();
        // if (wz != null)
        //     if (
        //         craft.id.Contains("refugee") || otherGameObject.GetMyWorldZoneId().Contains("refugee"))
        //     {
        //         if (otherGameObject.obj_id.Contains("refugee_builddesk") || otherGameObject.obj_id.Contains("storage_builddesk"))
        //         {
        //             Log($"{otherGameObject.obj_id} detected, refreshing inventories! Requester: {otherGameObject.obj_id}");
        //             MainGame.me.StartCoroutine(LoadInventories());
        //         }
        //         if (!(Time.time - _timeSix > LogGap)) return _refugeeMi;
        //         _timeSix = Time.time;
        //
        //         Log(
        //             $"[RefugeeGarden&Desk-InvRedirect]: Returned player multi-inventory to refugee garden!: Requester: {otherGameObject.obj_id}, Craft: {craft.id}, isPlayer: {otherGameObject.is_player}");
        //
        //         return _refugeeMi;
        //     }
        //
        // if (!_cfg.includeRefugeeDepot)
        //     if ((otherGameObject.is_player && craft.id.StartsWith("camp")) || craft.id.Contains("refugee") ||
        //         otherGameObject.obj_id.Contains("refugee"))
        //     {
        //         if (!(Time.time - _timeSeven > LogGap)) return _refugeeMi;
        //         _timeSeven = Time.time;
        //         Log($"[Refugee-InvRedirect]: Returned refugee multi-inventory to them!: Requester: {otherGameObject.obj_id}, Craft: {craft.id}, RefugeeInvCount: {_refugeeMi.all.Count}");
        //         return _refugeeMi;
        //     }

        if ((otherGameObject.is_player && craft.id.Length > 0) || (otherGameObject.has_linked_worker && otherGameObject.linked_worker.obj_id.Contains("zombie")) || otherGameObject.obj_id.Contains("zombie") || otherGameObject.obj_id.StartsWith("mf_") || _gratitudeCraft)
        {
            if ((otherGameObject.has_linked_worker && otherGameObject.linked_worker.obj_id.Contains("zombie")) || otherGameObject.obj_id.Contains("zombie")) _zombieWorker = true;

            if (!(Time.time - _timeEight > LogGap))  return GetMiInventory($"Object: {otherGameObject.obj_id}, Craft: {craft.id}, Gratitude: {_gratitudeCraft}");
            _timeEight = Time.time;
            Log($"[InvRedirect]: Redirected craft inventory to player MultiInventory! Object: {otherGameObject.obj_id}, Craft: {craft.id}, Gratitude: {_gratitudeCraft}");

             return GetMiInventory($"Object: {otherGameObject.obj_id}, Craft: {craft.id}, Gratitude: {_gratitudeCraft}");
        }

        _zombieWorker = false;

        if (!(Time.time - _timeNine > LogGap))  return orig;
        _timeNine = Time.time;
        Log($"[InvRedirect]: Original inventory sent back to requester! IsPlayer: {otherGameObject.is_player}, Object: {otherGameObject.obj_id}, Craft: {craft.id}, Gratitude: {_gratitudeCraft}");
         return orig;
    }
}