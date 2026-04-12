namespace WheresMaStorage;

public static class Invents
{
    internal static IEnumerator LoadInventories(Action callback = null)
    {
        if (Plugin.DebugEnabled) Helpers.Log("[LoadInventories] start");

        Fields.Mi = new MultiInventory();

        var playerInventory = new Inventory(MainGame.me.player);
        Fields.Mi.AddInventory(playerInventory, 0);
        playerInventory.data.SetInventorySize(Fields.PlayerInventorySize);

        var toolbelt = new Item
        {
            id = "Toolbelt",
            inventory = MainGame.me.player.data.secondary_inventory,
            inventory_size = 7
        };
        Fields.Mi.AddInventory(new Inventory(toolbelt));

        var zones = WorldZone._all_zones;
        var watch = Stopwatch.StartNew();

        var zonesVisited = 0;
        var zonesSkipped = 0;
        var invsAdded = 0;

        foreach (var zone in zones)
        {
            // BUG 1 FIX: was a.Contains(zone.id) — backwards. Now zone.id.Contains(a) so "refugees_camp".Contains("refugees") = true
            if (Fields.AlwaysSkipZones.Any(a => zone.id.ToLowerInvariant().Contains(a)))
            {
                zonesSkipped++;
                continue;
            }

            //if it's in a zone we haven't seen, skip
            if (!MainGame.me.save.known_world_zones.Exists(a => string.Equals(a, zone.id)))
            {
                zonesSkipped++;
                continue;
            }

            var worldZoneMulti = zone.GetMultiInventory(player_mi: MultiInventory.PlayerMultiInventory.ExcludePlayer, sortWGOS: true);
            if (worldZoneMulti == null)
            {
                zonesSkipped++;
                continue;
            }

            zonesVisited++;
            foreach (var inv in worldZoneMulti.Where(inv => !Fields.AlwaysSkipZones.Any(inv._obj_id.ToLowerInvariant().Contains)))
            {
                inv.data.sub_name = string.IsNullOrEmpty(inv._obj_id) ? $"Unknown#{zone.id}" : $"{inv._obj_id}#{zone.id}";

                Fields.Mi.AddInventory(inv);
                invsAdded++;
            }
        }

        //adds bags to the inventory
        var bagsAdded = 0;
        for (var i = 0; i < Fields.Mi.all.Count; i++)
        {
            var inventoriesToAdd = Fields.Mi.all[i].data.inventory
                .Where(data => data != null && !data.IsEmpty() && data.is_bag)
                .Select(data => new Inventory(data, data.id))
                .ToList();

            foreach (var inv in inventoriesToAdd)
            {
                Fields.Mi.AddInventory(inv, i + 1);
                bagsAdded++;
            }
        }

        watch.Stop();
        Fields.InventoriesLoaded = true;
        if (Plugin.DebugEnabled) Helpers.Log($"[LoadInventories] done: zones visited={zonesVisited} skipped={zonesSkipped}, inventories={invsAdded} (+{bagsAdded} bags), total Mi={Fields.Mi.all.Count} in {watch.ElapsedMilliseconds}ms");
        callback?.Invoke();
        yield return true;
    }


    internal static IEnumerator LoadWildernessInventories(Action callback = null)
    {
        if (Plugin.DebugEnabled) Helpers.Log("[LoadWilderness] start");

        var wgos = WorldMap._objs
            .Where(a => a.data.inventory_size > 0 && string.IsNullOrEmpty(a.GetMyWorldZoneId()))
            .ToList();

        var watch = Stopwatch.StartNew();
        Fields.WildernessMultiInventories.Clear();
        Fields.WildernessInventories.Clear();

        var excludedInventories = Fields.ExcludeTheseWildernessInventories;
        var additionalInventorySpace = Plugin.AdditionalInventorySpace.Value;
        var modifyInventorySize = Plugin.ModifyInventorySize.Value;

        foreach (var wgo in wgos)
        {
            if (wgo.obj_def.inventory_size <= 0 || excludedInventories.Any(wgo.obj_id.Contains)) continue;

            if (modifyInventorySize)
            {
                var size = Helpers.OriginalInventorySizes.TryGetValue(wgo.obj_id, out var originalSize)
                    ? originalSize
                    : wgo.obj_def.inventory_size;

                size += additionalInventorySpace;

                if (wgo.obj_def.inventory_size == size)
                {
                    continue;
                }
            }

            var zoneId = wgo.GetMyWorldZoneId();
            wgo.data.sub_name = $"{wgo.obj_id}#{zoneId}";

            if (!string.IsNullOrEmpty(zoneId) || wgo.unique_id.ToString().Length <= 5) continue;

            if (wgo.custom_tag.Equals(Fields.ShippingBoxTag) || wgo.data.drop_zone_id.Equals(Fields.ShippingBoxTag)) continue;

            if (!Fields.WildernessMultiInventories.ContainsKey(wgo))
            {
                Fields.WildernessMultiInventories[wgo] = wgo.GetMultiInventoryOfWGOWithoutWorldZone();
            }

            var multiInventory = Fields.WildernessMultiInventories[wgo];

            foreach (var inv in multiInventory.all.Where(inv => !Fields.WildernessInventories.Contains(inv)))
            {
                Fields.WildernessInventories.Add(inv);
            }
        }

        watch.Stop();
        if (Plugin.DebugEnabled) Helpers.Log($"[LoadWilderness] done: wgos scanned={wgos.Count}, inventories={Fields.WildernessInventories.Count} (multi={Fields.WildernessMultiInventories.Count}) in {watch.ElapsedMilliseconds}ms");
        callback?.Invoke();
        yield break;
    }


    internal static void SetInventorySizeText(BaseInventoryWidget inventoryWidget)
    {
        if (inventoryWidget.inventory_data.id.Contains(Fields.Writer)) return;
        if (inventoryWidget.header_label.text.Contains(Fields.Gerry)) return;
        if (!Plugin.ShowWorldZoneInTitles.Value && !Plugin.ShowUsedSpaceInTitles.Value) return;

        string objId;
        bool isPlayer;
        var subNameSplit = inventoryWidget.inventory_data.sub_name.Split('#');

        if (string.IsNullOrEmpty(subNameSplit[0]))
        {
            objId = Lang.Get("Player");
            isPlayer = true;
        }
        else
        {
            objId = GJL.L(subNameSplit[0].ToLowerInvariant().Trim() + "_inventory");
            isPlayer = false;
        }

        var zoneId = subNameSplit.Length > 1 ? subNameSplit[1].ToLowerInvariant().Trim() : string.Empty;
        var wzLabel = GetWorldZoneLabel(zoneId);

        var cultureInfo = CultureInfo.GetCultureInfo(
            GameSettings.me.language.Replace('_', '-').ToLower(CultureInfo.InvariantCulture).Trim());
        var textInfo = cultureInfo.TextInfo;
        wzLabel = textInfo.ToTitleCase(wzLabel);

        // BUG 3 FIX: was creating a full deep-copy via MakeInventoryCopy() just to read size and count
        var cap = inventoryWidget.inventory_data.inventory_size;
        var used = inventoryWidget.inventory_data.inventory.Count;

        inventoryWidget.header_label.overflowMethod = UILabel.Overflow.ResizeFreely;

        var header = inventoryWidget.inventory_data.is_bag ? GJL.L(inventoryWidget.inventory_data.id) : objId;

        // Fallback to inventory id if translation failed
        if (header.Contains("_"))
        {
            header = GJL.L(inventoryWidget.inventory_data.id);
        }

        var sb = new StringBuilder(header);

        if (Plugin.ShowWorldZoneInTitles.Value && !isPlayer)
        {
            sb.Append($" ({wzLabel})");
        }

        if (Plugin.ShowUsedSpaceInTitles.Value)
        {
            sb.Append($" - {used}/{cap}");
        }

        inventoryWidget.header_label.text = sb.ToString();

        string GetWorldZoneLabel(string zone)
        {
            if (string.IsNullOrEmpty(zone)) return Lang.Get("Wilderness");
            var wzId = WorldZone.GetZoneByID(zone, false);
            return wzId != null ? GJL.L("zone_" + wzId.id) : Lang.Get("Wilderness");
        }
    }

    internal static MultiInventory GetMiInventory(string requester, string zone)
    {
        var requesterInQuarry = zone.Contains("stone_workyard") || zone.Contains("marble_deposit");
        var requesterInZombieMill = zone.Contains("zombie_mill");

        if (requester.Contains("refugee_builddesk") || requester.Contains("storage_builddesk") || requesterInQuarry)
        {
            if (Plugin.DebugEnabled) Helpers.Log($"[GetMiInventory] triggering reload (requester={requester} zone={zone})");
            MainGame.me.StartCoroutine(LoadInventories());
            MainGame.me.StartCoroutine(LoadWildernessInventories());
        }

        var wildAdded = 0;
        foreach (var inv in Fields.WildernessInventories.Where(inv => !Fields.Mi.all.Contains(inv)))
        {
            Fields.Mi.AddInventory(inv);
            wildAdded++;
        }

        var tempList = Fields.Mi.all.ToList();
        var quarryRemoved = 0;
        if (Plugin.ExcludeQuarryFromSharedInventory.Value && !requesterInQuarry)
        {
            foreach (var inv in tempList.Where(inv => inv.data.sub_name.Contains("stone_workyard") || inv.data.sub_name.Contains("marble_deposit")))
            {
                Fields.Mi.all.Remove(inv);
                quarryRemoved++;
            }
        }

        var zombieMillRemoved = 0;
        if (Plugin.ExcludeZombieMillFromSharedInventory.Value && !requesterInZombieMill)
        {
            foreach (var inv in tempList.Where(inv => inv.data.sub_name.Contains("zombie_mill")))
            {
                Fields.Mi.all.Remove(inv);
                zombieMillRemoved++;
            }
        }

        if (Plugin.DebugEnabled) Helpers.Log($"[GetMiInventory] returning Mi (size={Fields.Mi.all.Count}): +{wildAdded} wilderness, -{quarryRemoved} quarry, -{zombieMillRemoved} zombie-mill (requester={requester} zone={zone})");
        return Fields.Mi;
    }

    // This method gets inserted into the CraftReally method using the transpiler below, overwriting any inventory the game sets during crafting
    public static MultiInventory GetMi(CraftDefinition craft, MultiInventory orig, WorldGameObject otherGameObject)
    {
        if (!Plugin.SharedInventory.Value)
        {
            if (Plugin.DebugEnabled) Helpers.Log($"[GetMi] SharedInventory disabled, returning orig for craft={craft?.id} obj={otherGameObject?.obj_id}");
            return orig;
        }

        var objId = otherGameObject.obj_id;
        var worldZoneId = otherGameObject.GetMyWorldZoneId();
        var isPlayer = otherGameObject.is_player;
        var hasLinkedWorker = otherGameObject.has_linked_worker;
        var linkedWorkerObjId = hasLinkedWorker ? otherGameObject.linked_worker.obj_id : string.Empty;

        var isQuarry = worldZoneId.Contains("stone_workyard") || worldZoneId.Contains("marble_deposit");
        var isWell = objId.Contains("well");
        var isZombieMill = worldZoneId.Contains("zombie_mill");

        var isZombie = objId.Contains("zombie") || linkedWorkerObjId.Contains("zombie");
        Fields.ZombieWorker = isZombie;

        if (Fields.AlwaysSkipZones.Any(a => objId.Contains(a) || worldZoneId.Contains(a)))
        {
            if (Plugin.DebugEnabled) Helpers.Log($"[GetMi] skip (AlwaysSkipZones match) obj={objId} zone={worldZoneId}");
            return orig;
        }

        if (Plugin.ExcludeWellsFromSharedInventory.Value && isWell)
        {
            if (Plugin.DebugEnabled) Helpers.Log($"[GetMi] skip (well exclusion) obj={objId}");
            return orig;
        }

        if (Plugin.ExcludeZombieMillFromSharedInventory.Value && isZombieMill)
        {
            if (Plugin.DebugEnabled) Helpers.Log($"[GetMi] skip (zombie mill exclusion) obj={objId} zone={worldZoneId}");
            return orig;
        }

        if (!Plugin.AllowZombiesAccessToSharedInventory.Value && isZombie)
        {
            if (Plugin.DebugEnabled) Helpers.Log($"[GetMi] skip (zombie, shared disallowed) obj={objId} linkedWorker={linkedWorkerObjId}");
            return orig;
        }

        if (objId.Contains("storage_builddesk"))
        {
            if (Plugin.DebugEnabled) Helpers.Log($"[GetMi] storage_builddesk interaction → triggering LoadInventories");
            MainGame.me.StartCoroutine(LoadInventories());
        }

        var isSpecialObject = isZombie || objId.StartsWith("mf_") || Fields.GratitudeCraft;

        if (isPlayer && craft.id.Length > 0 || isSpecialObject)
        {
            if (Plugin.DebugEnabled) Helpers.Log($"[GetMi] injecting shared multi (isPlayer={isPlayer},special={isSpecialObject},gratitude={Fields.GratitudeCraft}) craft={craft.id} obj={objId} zone={worldZoneId}");
            return GetMiInventory($"{objId}", otherGameObject.GetMyWorldZoneId());
        }

        if (Plugin.DebugEnabled) Helpers.Log($"[GetMi] no-match, returning orig craft={craft.id} obj={objId} zone={worldZoneId}");
        return orig;
    }

}
