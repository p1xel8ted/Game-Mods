using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Helper;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AutoLootHeavies;

[HarmonyPatch]
public static partial class MainPatcher
{
    private static readonly List<Stockpile> SortedStockpiles = new();
    private static Config.Options _cfg;

    private static float _lastBubbleTime;

    // private static bool _needScanning = true;
    private static List<WorldGameObject> _objects;

    private static float _xAdjustment;
    private const float EnergyRequirement = 3f;

    public static void Patch()
    {
        try
        {
            _cfg = Config.GetOptions();
            var harmony = new Harmony("p1xel8ted.GraveyardKeeper.AutoLootHeavies");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            //  _needScanning = true;
        }
        catch (Exception ex)
        {
            Log($"{ex.Message}, {ex.Source}, {ex.StackTrace}", true);
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
        var tupleList = new List<(int, int)>();

        if (tupleList.Count <= 0)
        {
            var grid = new int[vertical][];
            for (var x = 0; x < grid.Length; ++x) grid[x] = new int[horizontal];

            for (var x = 0; x < grid.Length; ++x)
            for (var y = 0; y < grid[x].Length; ++y)
            {
                var tu = (x, y);
                if (!tupleList.Contains(tu)) tupleList.Add(tu);
            }
        }

        var spot = tupleList.RandomElement();
        tupleList.Remove(spot);
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
        var needEnergy = EnergyRequirement;
        if (_cfg.disableImmersionMode) needEnergy = 0f;

        if (pwo.IsPlayerInvulnerable()) needEnergy = 0f;

        if (pwo.energy >= needEnergy)
        {
            pwo.energy -= needEnergy;
            EffectBubblesManager.ShowStackedEnergy(pwo, -needEnergy);

            var loc = GetGridLocation();

            _xAdjustment = loc.Item1 * 75;

            var timber = _cfg.designatedTimberLocation;
            var ore = _cfg.designatedOreLocation;
            var stone = _cfg.designatedStoneLocation;

            timber.x += _xAdjustment;
            ore.x += _xAdjustment;
            stone.x += _xAdjustment;

            var location = item.id switch
            {
                Constants.ItemDefinitionId.Wood => timber,
                Constants.ItemDefinitionId.Ore => ore,
                Constants.ItemDefinitionId.Stone => stone,
                Constants.ItemDefinitionId.Marble => stone,
                _ => MainGame.me.player_pos
            };

            MainGame.me.player.DropItem(item, Direction.IgnoreDirection, location, 0f, false);
            Log($"Teleporting {item.id} to dump site.");
            __instance.SetOverheadItem(null);
        }
        else
        {
            DropObjectAndNull(__instance, item);

            if (Time.time - _lastBubbleTime < 0.5f) return;

            _lastBubbleTime = Time.time;

            EffectBubblesManager.ShowImmediately(pwo.bubble_pos,
                GJL.L("not_enough_something", $"(en)"),
                EffectBubblesManager.BubbleColor.Energy, true, 1f);
            Log($"Not enough energy to teleport. Dropping.");
        }
    }

    private static bool TryPutToInventoryAndNull(BaseCharacterComponent __instance, WorldGameObject wgo,
        List<Item> itemsToInsert)
    {
        List<Item> failed = new();
        failed.Clear();
        var pwo = MainGame.me.player;
        var needEnergy = EnergyRequirement;
        if (_cfg.disableImmersionMode) needEnergy = 0f;

        if (pwo.IsPlayerInvulnerable()) needEnergy = 0f;

        if (pwo.energy >= needEnergy)
        {
            wgo.TryPutToInventory(itemsToInsert, out failed);
            if (failed.Count <= 0)
            {
                pwo.energy -= needEnergy;
                EffectBubblesManager.ShowStackedEnergy(pwo, -needEnergy);
                __instance.SetOverheadItem(null);
                return true;
            }

            return false;
        }

        if (Time.time - _lastBubbleTime > 0.5f)
        {
            _lastBubbleTime = Time.time;
            EffectBubblesManager.ShowImmediately(pwo.bubble_pos,
                GJL.L("not_enough_something", $"(en)"),
                EffectBubblesManager.BubbleColor.Energy, true, 1f);
        }

        return false;
    }

    private static void UpdateConfig()
    {
        Config.WriteOptions();
        _cfg = Config.GetOptions();
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
        var stockpile = SortedStockpiles.Find(a => a.GetStockpileObject() == wgo);
        if (stockpile != null)
        {
            Log($"Removed stockpile: location: {stockpile.GetLocation()}, type: {stockpile.GetStockpileType()}, distance: {stockpile.GetDistanceFromPlayer()}");
            SortedStockpiles.Remove(stockpile);
        }
        else
        {
            Log($"Error removing stockpile (null??).");
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
        Stockpile.StockpileType type;
        if (wgo.obj_id.Contains(Constants.ItemObjectId.Ore))
        {
            type = Stockpile.StockpileType.Ore;
        }
        else if (wgo.obj_id.Contains(Constants.ItemObjectId.Stone))
        {
            type = Stockpile.StockpileType.Stone;
        }
        else if (wgo.obj_id.Contains(Constants.ItemObjectId.Timber))
        {
            type = Stockpile.StockpileType.Timber;
        }
        else
        {
            type = Stockpile.StockpileType.Unknown;
        }

        return type;
    }

    private static bool AddStockpile(WorldGameObject stockpile)
    {
        var exists = SortedStockpiles.Find(a => a.GetStockpileObject() == stockpile);
        if (exists!=null)
        {
            exists.SetDistanceFromPlayer(GetDistance(stockpile));
            return false;
        }

        var location = GetLocation(stockpile);

        var distance = GetDistance(stockpile);

        var type = GetStockpileType(stockpile);

        var newStockpile = new Stockpile(location, type, distance, stockpile);

        SortedStockpiles.Add(newStockpile);
     
        //_needScanning = true;
        return true;
    }


    private static bool OverheadItemIsHeavy(Item item)
    {
        return item.id.Contains(Constants.ItemDefinitionId.Wood) ||
               item.id.Contains(Constants.ItemDefinitionId.Ore) ||
               item.id.Contains(Constants.ItemDefinitionId.Stone) ||
               item.id.Contains(Constants.ItemDefinitionId.Marble);
    }


    private static IEnumerator RunFullUpdate()
    {
        if (!MainGame.game_started) yield break;
        // if (_needScanning)
        // {
        var sw = new Stopwatch();
        sw.Start();
        
        //scan stockpiles
        
        var allObjects = CrossModFields.WorldObjects;
        _objects = allObjects!
            .Where(x => x.obj_id.Contains(Constants.ItemObjectId.Timber) || x.obj_id.Contains(Constants.ItemObjectId.Ore) ||
                        x.obj_id.Contains(Constants.ItemObjectId.Stone))
            .Where(x => x.data.inventory_size > 0)
            .ToList();

        Log($"[ALH]: Scanning world for stockpiles.");
        _objects.RemoveAll(a => a.obj_id.Contains("decor"));
        foreach (var obj in _objects)
        {
            Log($"Found stockpile: location: {GetLocation(obj)}, type: {GetStockpileType(obj)}, distance: {GetDistance(obj)}");
        }

        //update stockpiles
        Log($"[ALH]: Updating stockpiles distance, type etc and sorting by distance to player.");
        foreach (var stockpile in _objects.Where(a => a != null))
        {
            Log(AddStockpile(stockpile) ? $"Added stockpile: location: {GetLocation(stockpile)}, type: {GetStockpileType(stockpile)}, distance: {GetDistance(stockpile)}" : $"Stockpile already exists in list - updating distance from player.");
        }

        //sort them by distance from player
        SortedStockpiles.Sort((x, y) => x.GetDistanceFromPlayer().CompareTo(y.GetDistanceFromPlayer()));
        
        sw.Stop();
        Log($"Scanning, updating, and sorting stockpiles took {sw.ElapsedMilliseconds}ms");
    }
}