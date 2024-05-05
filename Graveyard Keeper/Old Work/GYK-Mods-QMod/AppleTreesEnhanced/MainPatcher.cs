using AppleTreesEnhanced.lang;
using HarmonyLib;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using Helper;
using Random = UnityEngine.Random;

namespace AppleTreesEnhanced;

[HarmonyPatch]
[HarmonyAfter("p1xel8ted.GraveyardKeeper.LongerDays")]
public static partial class MainPatcher
{
    private static Config.Options _cfg;
    private static bool _updateDone;
    public static void Patch()
    {
        try
        {
            var harmony = new Harmony("p1xel8ted.GraveyardKeeper.AppleTreesEnhanced");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            
            _cfg = Config.GetOptions();
            _updateDone = false;
        }
        catch (Exception ex)
        {
            Log($"{ex.Message}, {ex.Source}, {ex.StackTrace}", true);
        }
    }

    private struct Constants
    {
        public struct HarvestGrowing
        {
            public const string GardenAppleTree = "tree_apple_garden_empty";
            public const string GardenBerryBush = "bush_berry_garden_empty";
            public const string WorldBerryBush1 = "bush_1";
            public const string WorldBerryBush2 = "bush_2";
            public const string WorldBerryBush3 = "bush_3";
            public const string BeeHouse = "beehouse_1";
        }

        public struct HarvestItem
        {
            public const string AppleTree = "fruit:apple_red_crop";
            public const string BerryBush = "fruit:berry";
        }

        public struct HarvestReady
        {
            public const string GardenAppleTree = "tree_apple_garden_ready";
            public const string GardenBerryBush = "bush_berry_garden_ready";
            public const string WorldBerryBush1 = "bush_1_berry";
            public const string WorldBerryBush2 = "bush_2_berry";
            public const string WorldBerryBush3 = "bush_3_berry";
            public const string BeeHouse = "beehouse_2";
        }

        public struct HarvestSpawner
        {
            public const string GardenAppleTree = "tree_apple_garden_crops_growing";
            public const string GardenBerryBush = "bush_berry_garden";
            public const string WorldBerryBush1 = "bush_1_berry_respawn";
            public const string WorldBerryBush2 = "bush_2_berry_respawn";
            public const string WorldBerryBush3 = "bush_3_berry_respawn";
            public const string BeeHouse = "honey_production";
        }

        public struct OutputItems
        {
            public const string Bee = "bee";
            public const string Honey = "honey";
            public const string Wax = "beeswax";
        }
    }


    private static void ShowMessage(WorldGameObject obj, string message)
    {
        if (!_cfg.ShowHarvestReadyMessages) return;

        Thread.CurrentThread.CurrentUICulture = CrossModFields.Culture;
        var newObjPos = obj.pos3;

        if (obj.obj_id.Contains("berry")) newObjPos.y += 100f;

        if (obj.obj_id.Contains("bee")) newObjPos.y += 100f;

        if (obj.obj_id.Contains("tree")) newObjPos.y += 250f;

        EffectBubblesManager.ShowImmediately(newObjPos, message,
            EffectBubblesManager.BubbleColor.Relation,
            true, 3f);
    }


    private static readonly string[] WorldReadyHarvests =
    {
        Constants.HarvestReady.WorldBerryBush1, Constants.HarvestReady.WorldBerryBush2, Constants.HarvestReady.WorldBerryBush3
    };
    
    private static readonly string[] HarvestSpawners =
    {
        Constants.HarvestSpawner.GardenAppleTree, Constants.HarvestSpawner.GardenBerryBush, Constants.HarvestSpawner.WorldBerryBush1, Constants.HarvestSpawner.WorldBerryBush2, Constants.HarvestSpawner.WorldBerryBush3
    };


    private static readonly string[] SellThesePlease =
    {
        Constants.OutputItems.Bee, Constants.OutputItems.Wax, Constants.OutputItems.Honey
    };


    private static void ProcessRespawn(WorldGameObject wgo, string replaceString, string craftString)
    {
        wgo.ReplaceWithObject(replaceString, true);
        wgo.GetComponent<ChunkedGameObject>().Init(true);
        wgo.TryStartCraft(craftString);
    }

    private static void ProcessBeeRespawn(WorldGameObject wgo)
    {
        wgo.ReplaceWithObject(Constants.HarvestGrowing.BeeHouse, true);
        wgo.GetComponent<ChunkedGameObject>().Init(true);
        wgo.TryStartCraft(Constants.HarvestSpawner.BeeHouse);
    }

    private static void ProcessBeeDropAndRespawn(WorldGameObject wgo)
    {
        var list = ResModificator.ProcessItemsListBeforeDrop(wgo.obj_def.drop_items, wgo, null, null);
        foreach (var item in list.Where(item => item.value > 1))
        {
            item.value = 1;
        }

        wgo.DropItems(list);
        wgo.ReplaceWithObject(Constants.HarvestGrowing.BeeHouse, true);
        wgo.GetComponent<ChunkedGameObject>().Init(true);
        wgo.TryStartCraft(Constants.HarvestSpawner.BeeHouse);
        ShowMessage(wgo, strings.HoneyReady);
    }

    private static void ProcessDropAndRespawn(WorldGameObject wgo, string replaceString, string craftString, string harvestItem, string message, int rand)
    {
        for (var i = 0; i < rand; i++)
        {
            var item = new Item(harvestItem, 1);
            wgo.DropItem(item, Direction.None, force: 5f,
                check_walls: false);
        }

        wgo.ReplaceWithObject(replaceString, true);
        wgo.GetComponent<ChunkedGameObject>().Init(true);
        wgo.TryStartCraft(craftString);
        ShowMessage(wgo, message);
    }

    private static void ProcessGardenBeeHive(WorldGameObject wgo)
    {
        if (!_cfg.IncludeGardenBeeHives) return;

        if (_cfg.RealisticHarvest)
        {
            var o = wgo;
            var dropRand = Random.Range(2, 16);
            GJTimer.AddTimer(dropRand, delegate
            {
                if (o.obj_id == Constants.HarvestGrowing.BeeHouse) return;
                ProcessBeeDropAndRespawn(o);
            });
        }
        else
        {
            ProcessBeeDropAndRespawn(wgo);
        }
    }

    private static void ProcessGardenAppleTree(WorldGameObject wgo)
    {
        if (!_cfg.IncludeGardenTrees) return;
        var rand = 15;

        if (_cfg.RealisticHarvest)
        {
            rand = Random.Range(5, 11);
            var o = wgo;
            var dropRand = Random.Range(2, 16);
            GJTimer.AddTimer(dropRand, delegate
            {
                if (o.obj_id == Constants.HarvestGrowing.GardenAppleTree) return;
                ProcessDropAndRespawn(o, Constants.HarvestGrowing.GardenAppleTree,
                    Constants.HarvestSpawner.GardenAppleTree, Constants.HarvestItem.AppleTree,
                    strings.ApplesReady, rand);
            });
        }
        else
        {
            ProcessDropAndRespawn(wgo, Constants.HarvestGrowing.GardenAppleTree,
                Constants.HarvestSpawner.GardenAppleTree, Constants.HarvestItem.AppleTree,
                strings.ApplesReady, rand);
        }
    }

    private static void ProcessGardenBerryBush(WorldGameObject wgo)
    {
        if (!_cfg.IncludeGardenBerryBushes) return;
        var rand = 4;

        if (_cfg.RealisticHarvest)
        {
            rand = Random.Range(1, 4);
            var o = wgo;
            var dropRand = Random.Range(2, 16);
            GJTimer.AddTimer(dropRand, delegate
            {
                if (o.obj_id == Constants.HarvestGrowing.GardenBerryBush) return;
                ProcessDropAndRespawn(o, Constants.HarvestGrowing.GardenBerryBush,
                    Constants.HarvestSpawner.GardenBerryBush, Constants.HarvestItem.BerryBush,
                    strings.BerriesReady, rand);
            });
        }
        else
        {
            ProcessDropAndRespawn(wgo, Constants.HarvestGrowing.GardenBerryBush,
                Constants.HarvestSpawner.GardenBerryBush, Constants.HarvestItem.BerryBush,
                strings.BerriesReady, rand);
        }
    }

    private static bool IsPlayerBeeHive(WorldGameObject wgo)
    {
        return wgo.obj_def.drop_items.Exists(a => a.id.Equals(Constants.OutputItems.Bee));
    }

    private static void ProcessBerryBush1(WorldGameObject wgo)
    {
        if (!_cfg.IncludeWorldBerryBushes) return;
        var rand = 4;

        if (_cfg.RealisticHarvest)
        {
            rand = Random.Range(1, 3);
            var o = wgo;
            var dropRand = Random.Range(2, 16);
            GJTimer.AddTimer(dropRand, delegate
            {
                if (o.obj_id == Constants.HarvestGrowing.WorldBerryBush1) return;
                ProcessDropAndRespawn(o, Constants.HarvestGrowing.WorldBerryBush1,
                    Constants.HarvestSpawner.WorldBerryBush1, Constants.HarvestItem.BerryBush,
                    strings.BerriesReady, rand);
            });
        }
        else
        {
            ProcessDropAndRespawn(wgo, Constants.HarvestGrowing.WorldBerryBush1,
                Constants.HarvestSpawner.WorldBerryBush1, Constants.HarvestItem.BerryBush,
                strings.BerriesReady, rand);
        }
    }

    private static void ProcessBerryBush2(WorldGameObject wgo)
    {
        if (!_cfg.IncludeWorldBerryBushes) return;
        var rand = 4;

        if (_cfg.RealisticHarvest)
        {
            rand = Random.Range(1, 3);
            var o = wgo;
            var dropRand = Random.Range(2, 16);
            GJTimer.AddTimer(dropRand, delegate
            {
                if (o.obj_id == Constants.HarvestGrowing.WorldBerryBush2) return;
                ProcessDropAndRespawn(o, Constants.HarvestGrowing.WorldBerryBush2,
                    Constants.HarvestSpawner.WorldBerryBush2, Constants.HarvestItem.BerryBush,
                    strings.BerriesReady, rand);
            });
        }
        else
        {
            ProcessDropAndRespawn(wgo, Constants.HarvestGrowing.WorldBerryBush2,
                Constants.HarvestSpawner.WorldBerryBush2, Constants.HarvestItem.BerryBush,
                strings.BerriesReady, rand);
        }
    }

    private static void ProcessBerryBush3(WorldGameObject wgo)
    {
        if (!_cfg.IncludeWorldBerryBushes) return;
        var rand = 4;

        if (_cfg.RealisticHarvest)
        {
            rand = Random.Range(1, 3);
            var o = wgo;
            var dropRand = Random.Range(2, 16);
            GJTimer.AddTimer(dropRand, delegate
            {
                if (o.obj_id == Constants.HarvestGrowing.WorldBerryBush3) return;
                ProcessDropAndRespawn(o, Constants.HarvestGrowing.WorldBerryBush3,
                    Constants.HarvestSpawner.WorldBerryBush3, Constants.HarvestItem.BerryBush,
                    strings.BerriesReady, rand);
            });
        }
        else
        {
            ProcessDropAndRespawn(wgo, Constants.HarvestGrowing.WorldBerryBush3,
                Constants.HarvestSpawner.WorldBerryBush3, Constants.HarvestItem.BerryBush,
                strings.BerriesReady, rand);
        }
    }
}