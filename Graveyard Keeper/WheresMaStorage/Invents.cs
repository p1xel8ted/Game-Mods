using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using GYKHelper;
using UnityEngine;
using WheresMaStorage.lang;

namespace WheresMaStorage;

public static class Invents
{

    
    internal static IEnumerator LoadInventories(Action callback = null)
    {
        Helpers.Log("Loading inventories!");
        Fields.Mi = new MultiInventory();
        Fields.RefugeeMi = new MultiInventory();
        Fields.Mi.all.Clear();
        Fields.RefugeeMi.all.Clear();
        // var playerInv = new Inventory(MainGame.me.player);
        // playerInv.data.SetInventorySize(_invSize);
        //
        // _mi.AddInventory(playerInv);
        
        var inventory = new Inventory(MainGame.me.player);
        Fields.Mi.AddInventory(inventory, 0);
        inventory.data.SetInventorySize(Fields.InvSize);

            var toolbelt = new Item()
            {
                inventory = MainGame.me.player.data.secondary_inventory,
                inventory_size = 7
            };
            Fields.Mi.AddInventory(new Inventory(toolbelt));
            

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
                Helpers.Log($"InventoryName: {inv._obj_id}");
                if (inv._obj_id.Length <= 0)
                    inv.data.sub_name = "Unknown" + "#" + zone.id;
                else
                    inv.data.sub_name = inv._obj_id + "#" + zone.id;


                if (zone.id.ToLowerInvariant().Contains("refugee"))
                {
                    Fields.RefugeeMi.AddInventory(inv);
                }
                else
                {
                    Fields.Mi.AddInventory(inv);
                }
            }
        }

        for (var i = 0; i < Fields.Mi.all.Count; i++)
        {
            foreach (var data in Fields.Mi.all[i].data.inventory.Where(data => data != null && !data.IsEmpty() && data.is_bag))
            {
                Fields.Mi.AddInventory(new Inventory(data, data.id), i + 1);
            }
        }
        
        watch.Stop();
        Fields.InvsLoaded = true;
        Helpers.Log($"Inventory Loading Complete! Completed in {watch.ElapsedMilliseconds}ms");
        callback?.Invoke();
        yield return true;
    }

    internal static IEnumerator LoadWildernessInventories(Action callback = null)
    {
        Helpers.Log("Loading wilderness inventories!");
        var wgos = CrossModFields.WorldObjects.Where(a =>
            a.data.inventory_size > 0 && string.IsNullOrEmpty(a.GetMyWorldZoneId()));
        var watch = Stopwatch.StartNew();
        Fields.WildernessMultiInventories.Clear();
        Fields.WildernessInventories.Clear();

        foreach (var wgo in wgos)
        {
            if (wgo.obj_def.inventory_size <= 0 || Fields.ExcludeTheseWildernessInventories.Any(wgo.obj_id.Contains) || wgo.data.inventory_size == Plugin.AdditionalInventorySpace.Value) continue;

            var zoneId = wgo.GetMyWorldZoneId();
            wgo.data.sub_name = wgo.obj_id + "#" + zoneId;

            if (!string.IsNullOrEmpty(zoneId)) continue;
            if (wgo.unique_id.ToString().Length <= 5) continue;

            if (wgo.custom_tag.Equals(Fields.ShippingBoxTag) || wgo.data.drop_zone_id.Equals(Fields.ShippingBoxTag)) continue;

            var exists = Fields.WildernessMultiInventories.ContainsKey(wgo);
            if (!exists) Fields.WildernessMultiInventories.Add(wgo, wgo.GetMultiInventoryOfWGOWithoutWorldZone());

            var invCount = 0;
            foreach (var inv in wgo.GetMultiInventoryOfWGOWithoutWorldZone().all.Where(inv => !Fields.WildernessInventories.Contains(inv)))
            {
                invCount++;
                Fields.WildernessInventories.Add(inv);
                Helpers.Log($"Added {wgo.obj_id}'s ({zoneId}, {wgo.pos3}) Inventory {inv._obj_id} (#{invCount}) - CurrentGD: {wgo.cur_gd_point}, Unique ID: {wgo.unique_id} to WildernessInventories.");
            }
        }

        watch.Stop();
        Helpers.Log($"Wilderness Inventory Loading Complete! Completed in {watch.ElapsedMilliseconds}ms.");
        callback?.Invoke();
        yield break;
    }

    internal static void SetInventorySizeText(BaseInventoryWidget inventoryWidget)
    {
        //Helpers.Log($"[Bag]: {inventoryWidget.inventory_data.is_bag}, ID: {inventoryWidget.inventory_data.id}");
        if (inventoryWidget.inventory_data.id.Contains(Fields.Writer)) return;
        if (inventoryWidget.header_label.text.Contains(Fields.Gerry)) return;
        if (!Plugin.ShowWorldZoneInTitles.Value && !Plugin.ShowUsedSpaceInTitles.Value) return;

        string wzLabel;
        string objId;
        bool isPlayer;
        var subNameSplit = inventoryWidget.inventory_data.sub_name.Split('#');
        if (string.IsNullOrEmpty(subNameSplit[0]))
        {
            objId = Helpers.GetLocalizedString(strings.Player);
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
            wzLabel = wzId != null ? GJL.L("zone_" + wzId.id) : Helpers.GetLocalizedString(strings.Wilderness);
        }
        else
        {
            wzLabel = Helpers.GetLocalizedString(strings.Wilderness);
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

        if (Plugin.ShowWorldZoneInTitles.Value && !isPlayer) header = string.Concat(header, $" ({wzLabel})");

        if (Plugin.ShowUsedSpaceInTitles.Value) header = string.Concat(header, $" - {used}/{cap}");

        inventoryWidget.header_label.text = header;
    }

    internal static MultiInventory GetMiInventory(string requester)
    {
        if (requester.Contains("refugee_builddesk") || requester.Contains("storage_builddesk"))
        {
            Helpers.Log($"{requester} detected, refreshing inventories! Requester: {requester}");
            MainGame.me.StartCoroutine(LoadInventories());
            MainGame.me.StartCoroutine(LoadWildernessInventories());
        }
        
        if (Fields.WildernessInventories.Count <= 0)
        {
            Helpers.Log($"WildernessInventory count <= 0! Requester: {requester}");
           return Fields.Mi;
        }

        foreach (var inv in Fields.WildernessInventories.Where(inv => !Fields.Mi.all.Contains(inv)))
        {
            Helpers.Log($"Added WildernessInventory to MultiInventory Request! Requester: {requester}");
            Fields.Mi.AddInventory(inv);
        }

        if (!requester.Contains("zombie")) Helpers.Log($"MultiInventory Request! Requester: {requester}");


        return Fields.Mi;
    }

    //this method gets inserted into the CraftReally method using the transpiler below, overwriting any inventory the game sets during crafting
    public static MultiInventory GetMi(CraftDefinition craft, MultiInventory orig, WorldGameObject otherGameObject)
    {
        if (!Plugin.SharedInventory.Value) return orig;
        if (otherGameObject.GetMyWorldZoneId().Contains("refugee") || otherGameObject.obj_id.Contains("refugee"))
        {
            Helpers.Log($"[InvRedirect]: Original inventory sent back to requester! IsPlayer: {otherGameObject.is_player}, Object: {otherGameObject.obj_id}, Craft: {craft.id}, Gratitude: {Fields.GratitudeCraft}");
            return orig;
        }

        if (otherGameObject.obj_id.Contains("storage_builddesk"))
        {
            Helpers.Log($"{otherGameObject.obj_id} detected, refreshing inventories! Requester: {otherGameObject.obj_id}");
            MainGame.me.StartCoroutine(LoadInventories());
        }
        
        if ((otherGameObject.is_player && craft.id.Length > 0) || (otherGameObject.has_linked_worker && otherGameObject.linked_worker.obj_id.Contains("zombie")) || otherGameObject.obj_id.Contains("zombie") || otherGameObject.obj_id.StartsWith("mf_") || Fields.GratitudeCraft)
        {
            if ((otherGameObject.has_linked_worker && otherGameObject.linked_worker.obj_id.Contains("zombie")) || otherGameObject.obj_id.Contains("zombie")) Fields.ZombieWorker = true;

            if (!(Time.time - Fields.TimeEight > Fields.LogGap))  return GetMiInventory($"Object: {otherGameObject.obj_id}, Craft: {craft.id}, Gratitude: {Fields.GratitudeCraft}");
            Fields.TimeEight = Time.time;
            Helpers.Log($"[InvRedirect]: Redirected craft inventory to player MultiInventory! Object: {otherGameObject.obj_id}, Craft: {craft.id}, Gratitude: {Fields.GratitudeCraft}");

             return GetMiInventory($"Object: {otherGameObject.obj_id}, Craft: {craft.id}, Gratitude: {Fields.GratitudeCraft}");
        }

        Fields.ZombieWorker = false;

        if (!(Time.time - Fields.TimeNine > Fields.LogGap))  return orig;
        Fields.TimeNine = Time.time;
        Helpers.Log($"[InvRedirect]: Original inventory sent back to requester! IsPlayer: {otherGameObject.is_player}, Object: {otherGameObject.obj_id}, Craft: {craft.id}, Gratitude: {Fields.GratitudeCraft}");
         return orig;
    }
}