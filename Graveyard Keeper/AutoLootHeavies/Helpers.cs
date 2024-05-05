using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GYKHelper;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AutoLootHeavies;

public partial class Plugin
{
    private static void WriteLog(string message, bool error = false)
    {
        if (error)
        {
            Log.LogError(message);
        }
        else if (Debug.Value)
        {
            Log.LogInfo(message);
        }
    }

    private static void DropObjectAndNull(BaseCharacterComponent __instance, Item item)
    {
        DropResGameObject.Drop(__instance.tf.position, item,
            __instance.tf.parent,
            __instance.anim_direction,
            3f,
            Random.Range(0, 2), false);

        __instance.SetOverheadItem(null);
    }

    private static (int, int) GetGridLocation()
    {
        const int horizontal = 30;
        const int vertical = 5;
        var tupleList = new List<(int, int)>(horizontal * vertical);

        for (var x = 0; x < vertical; ++x)
        {
            for (var y = 0; y < horizontal; ++y)
            {
                tupleList.Add((x, y));
            }
        }

        var spot = tupleList.RandomElement();
        return spot;
    }


    private static void ShowLootAddedIcon(Item item)
    {
        item.definition.item_size = 1;
        DropCollectGUI.OnDropCollected(item);
        item.definition.item_size = 2;
        Sounds.PlaySound("pickup", null, true);
    }

    private static void TeleportItem(BaseCharacterComponent __instance, Item item)
    {
        var pwo = MainGame.me.player;
        var needEnergy = !ImmersionMode.Value || pwo.IsPlayerInvulnerable() ? 0f : EnergyRequirement;

        if (pwo.energy >= needEnergy)
        {
            pwo.energy -= needEnergy;
            EffectBubblesManager.ShowStackedEnergy(pwo, -needEnergy);

            var loc = GetGridLocation();
            float xAdjustment = loc.Item1 * 75;

            var timber = DesignatedTimberLocation.Value + new Vector3(xAdjustment, 0, 0);
            var ore = DesignatedOreLocation.Value + new Vector3(xAdjustment, 0, 0);
            var stone = DesignatedStoneLocation.Value + new Vector3(xAdjustment, 0, 0);

            var location = item.id switch
            {
                Constants.ItemDefinitionId.Wood => timber,
                Constants.ItemDefinitionId.Ore => ore,
                Constants.ItemDefinitionId.Stone => stone,
                Constants.ItemDefinitionId.Marble => stone,
                _ => MainGame.me.player_pos
            };

            MainGame.me.player.DropItem(item, Direction.IgnoreDirection, location, 0f, false);
            WriteLog($"Teleporting {item.id} to dump site.");
            __instance.SetOverheadItem(null);
        }
        else
        {
            DropObjectAndNull(__instance, item);

            if (Time.time - LastBubbleTime >= 0.5f)
            {
                LastBubbleTime = Time.time;

                EffectBubblesManager.ShowImmediately(pwo.bubble_pos,
                    GJL.L("not_enough_something", $"(en)"),
                    EffectBubblesManager.BubbleColor.Energy, true, 1f);
                WriteLog($"Not enough energy to teleport. Dropping.");
            }
        }
    }


    private static bool TryPutToInventoryAndNull(BaseCharacterComponent __instance, WorldGameObject wgo, List<Item> itemsToInsert)
    {
        var pwo = MainGame.me.player;
        var needEnergy = !ImmersionMode.Value || pwo.IsPlayerInvulnerable() ? 0f : EnergyRequirement;

        if (pwo.energy >= needEnergy)
        {
            wgo.TryPutToInventory(itemsToInsert, out var failed);
            if (failed.Count == 0)
            {
                pwo.energy -= needEnergy;
                EffectBubblesManager.ShowStackedEnergy(pwo, -needEnergy);
                __instance.SetOverheadItem(null);
                return true;
            }
        }
        else if (Time.time - LastBubbleTime > 0.5f)
        {
            LastBubbleTime = Time.time;
            EffectBubblesManager.ShowImmediately(pwo.bubble_pos,
                GJL.L("not_enough_something", $"(en)"),
                EffectBubblesManager.BubbleColor.Energy, true, 1f);
        }

        return false;
    }


    private struct Constants
    {
        public struct ItemDefinitionId
        {
            public const string Marble = "marble";
            public const string Ore = "ore_metal";
            public const string Stone = "stone";
            public const string Wood = "wood";
        }

        public struct ItemObjectId
        {
            public const string Ore = "mf_ore_1";
            public const string Stone = "mf_stones_1";
            public const string Timber = "mf_timber_1";
        }
    }

    // ReSharper disable once SuggestBaseTypeForParameter
    private static void RemoveStockpile(WorldGameObject wgo)
    {
        var stockpile = SortedStockpiles.Find(a => a.Wgo == wgo);
        if (stockpile != null)
        {
            WriteLog($"Removed stockpile: location: {stockpile.Location}, type: {stockpile.Type}, distance: {stockpile.DistanceFromPlayer}");
            SortedStockpiles.Remove(stockpile);
        }
        else
        {
            WriteLog($"Error removing stockpile (null??).", error: true);
        }
    }


    private static Vector3 GetLocation(WorldGameObject wgo)
    {
        return new Vector3((float) Math.Ceiling(wgo.pos3.x), (float) Math.Ceiling(wgo.pos3.y),
            (float) Math.Ceiling(wgo.pos3.z));
    }

    private static float GetDistance(WorldGameObject wgo)
    {
        return Vector3.Distance(MainGame.me.player_pos, wgo.pos3);
    }

    private static Stockpile.StockpileType GetStockpileType(WorldGameObject wgo)
    {
        return wgo.obj_id switch
        {
            { } id when id.Contains(Constants.ItemObjectId.Ore) => Stockpile.StockpileType.Ore,
            { } id when id.Contains(Constants.ItemObjectId.Stone) => Stockpile.StockpileType.Stone,
            { } id when id.Contains(Constants.ItemObjectId.Timber) => Stockpile.StockpileType.Timber,
            _ => Stockpile.StockpileType.Unknown,
        };
    }


    private static bool AddStockpile(WorldGameObject stockpile)
    {
        var exists = SortedStockpiles.Find(a => a.Wgo == stockpile);
        if (exists != null)
        {
            exists.DistanceFromPlayer = GetDistance(stockpile);
            return false;
        }

        var newStockpile = new Stockpile
        (
            GetLocation(stockpile),
            GetStockpileType(stockpile),
            GetDistance(stockpile),
            stockpile
        );

        SortedStockpiles.Add(newStockpile);

        //_needScanning = true;
        return true;
    }


    private static bool OverheadItemIsHeavy(Item item)
    {
        return item.id is Constants.ItemDefinitionId.Wood or
            Constants.ItemDefinitionId.Ore or
            Constants.ItemDefinitionId.Stone or
            Constants.ItemDefinitionId.Marble;
    }


    private static IEnumerator RunFullUpdate()
    {
        if (!MainGame.game_started) yield break;

        var sw = new Stopwatch();
        sw.Start();

        var allObjects = CrossModFields.WorldObjects;
        Objects = allObjects!
            .Where(x => x.obj_id.Contains(Constants.ItemObjectId.Timber) || x.obj_id.Contains(Constants.ItemObjectId.Ore) ||
                        x.obj_id.Contains(Constants.ItemObjectId.Stone))
            .Where(x => x.data.inventory_size > 0)
            .ToList();

        WriteLog($"[ALH]: Scanning world for stockpiles.");
        Objects.RemoveAll(a => a.obj_id.Contains("decor"));

        foreach (var obj in Objects)
        {
            WriteLog($"Found stockpile: location: {GetLocation(obj)}, type: {GetStockpileType(obj)}, distance: {GetDistance(obj)}");
        }

        WriteLog($"[ALH]: Updating stockpiles distance, type etc and sorting by distance to player.");

        foreach (var stockpile in Objects.Where(a => a != null))
        {
            WriteLog(AddStockpile(stockpile) ? $"Added stockpile: location: {GetLocation(stockpile)}, type: {GetStockpileType(stockpile)}, distance: {GetDistance(stockpile)}" : $"Stockpile already exists in list - updating distance from player.");
        }

        SortedStockpiles.Sort((x, y) => x.DistanceFromPlayer.CompareTo(y.DistanceFromPlayer));

        sw.Stop();
        WriteLog($"Scanning, updating, and sorting stockpiles took {sw.ElapsedMilliseconds}ms");
    }


    private static void WorldGameObjectInteract(WorldGameObject obj)
    {
        if (StockpileIsValid(obj))
        {
            AddStockpile(obj);
        }
    }
}