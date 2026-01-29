// Decompiled with JetBrains decompiler
// Type: StructureManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class StructureManager
{
  public static Dictionary<FollowerLocation, List<StructureBrain>> StructureBrains = new Dictionary<FollowerLocation, List<StructureBrain>>();
  public static StructureManager.StructuresPlaced OnStructuresPlaced;
  public static StructureManager.StructureChanged OnStructureAdded;
  public static StructureManager.StructureChanged OnStructureAddedToGrid;
  public static StructureManager.StructureChanged OnStructureMoved;
  public static StructureManager.StructureChanged OnStructureUpgraded;
  public static StructureManager.StructureChanged OnStructureRemoved;
  public static List<StructureBrain> CacheGetAllStructuresOfType = new List<StructureBrain>();
  public static List<Structures_SiloSeed> cachedSilos = new List<Structures_SiloSeed>();
  public static List<Structures_DeadWorshipper> cachedDeadWorshippers = new List<Structures_DeadWorshipper>();
  public static List<Structures_Bed> cachedBeds = new List<Structures_Bed>();
  public static List<StructureBrain.TYPES> _onlyPlaceableInRanch = new List<StructureBrain.TYPES>()
  {
    StructureBrain.TYPES.RANCH_TROUGH,
    StructureBrain.TYPES.RANCH_HUTCH
  };

  public static List<StructureBrain> StructuresAtLocation(FollowerLocation location)
  {
    List<StructureBrain> structureBrainList = (List<StructureBrain>) null;
    if (location == FollowerLocation.None)
      structureBrainList = new List<StructureBrain>();
    else if (!StructureManager.StructureBrains.TryGetValue(location, out structureBrainList))
    {
      structureBrainList = new List<StructureBrain>();
      StructureManager.StructureBrains[location] = structureBrainList;
    }
    return structureBrainList;
  }

  public static List<StructuresData> StructuresDataAtLocation(FollowerLocation location)
  {
    switch (location)
    {
      case FollowerLocation.Base:
        return DataManager.Instance.BaseStructures;
      case FollowerLocation.Hub1:
        return DataManager.Instance.HubStructures;
      case FollowerLocation.HubShore:
        return DataManager.Instance.HubShoreStructures;
      case FollowerLocation.Hub1_Main:
        return DataManager.Instance.Hub1_MainStructures;
      case FollowerLocation.Hub1_Swamp:
        return DataManager.Instance.Hub1_SwampStructures;
      case FollowerLocation.Hub1_Berries:
        return DataManager.Instance.Hub1_BerriesStructures;
      case FollowerLocation.Hub1_RatauOutside:
        return DataManager.Instance.Hub1_RatauOutsideStructures;
      case FollowerLocation.Hub1_RatauInside:
        return DataManager.Instance.Hub1_RatauInsideStructures;
      case FollowerLocation.Hub1_Sozo:
        return DataManager.Instance.Hub1_SozoStructures;
      case FollowerLocation.Hub1_Forest:
        return DataManager.Instance.Hub1_ForestStructures;
      case FollowerLocation.Dungeon_Logs1:
        return DataManager.Instance.Dungeon_Logs1Structures;
      case FollowerLocation.Dungeon_Logs2:
        return DataManager.Instance.Dungeon_Logs2Structures;
      case FollowerLocation.Dungeon_Logs3:
        return DataManager.Instance.Dungeon_Logs3Structures;
      case FollowerLocation.Dungeon_Food1:
        return DataManager.Instance.Dungeon_Food1Structures;
      case FollowerLocation.Dungeon_Food2:
        return DataManager.Instance.Dungeon_Food2Structures;
      case FollowerLocation.Dungeon_Food3:
        return DataManager.Instance.Dungeon_Food3Structures;
      case FollowerLocation.Dungeon_Stone1:
        return DataManager.Instance.Dungeon_Stone1Structures;
      case FollowerLocation.Dungeon_Stone2:
        return DataManager.Instance.Dungeon_Stone2Structures;
      case FollowerLocation.Dungeon_Stone3:
        return DataManager.Instance.Dungeon_Stone3Structures;
      case FollowerLocation.DLC_ShrineRoom:
        return DataManager.Instance.WoolhavenStructures;
      default:
        return new List<StructuresData>();
    }
  }

  public static void BuildStructure(
    FollowerLocation location,
    StructuresData data,
    Vector3 position,
    Vector2Int bounds,
    bool animateIn = true,
    Action<GameObject> callback = null,
    System.Action locationChangedCallback = null,
    bool emitParticles = true)
  {
    data.CreateStructure(location, position, bounds);
    StructureManager.AddStructure(location, data, emitParticles);
    if (!DataManager.Instance.HistoryOfStructures.Contains(data.Type))
      DataManager.Instance.HistoryOfStructures.Add(data.Type);
    if (data.Type == StructureBrain.TYPES.RANCH_FENCE || data.Type == StructureBrain.TYPES.RANCH || data.Type == StructureBrain.TYPES.RANCH_2)
      animateIn = false;
    LocationManager locationManager;
    if (!LocationManager.LocationManagers.TryGetValue(location, out locationManager) || !((UnityEngine.Object) locationManager != (UnityEngine.Object) null))
      return;
    if (!data.PrefabPath.Contains("Assets"))
      data.PrefabPath = $"Assets/{data.PrefabPath}.prefab";
    Addressables_wrapper.InstantiateAsync((object) data.PrefabPath, (Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      GameObject gameObject = locationManager.PlaceStructure(data, obj.Result.gameObject);
      if (!((UnityEngine.Object) gameObject != (UnityEngine.Object) null))
        return;
      if (location == PlayerFarming.Location)
      {
        Vector3 vector3 = gameObject.transform.position + Vector3.forward * 1f;
        Vector3 position1 = gameObject.transform.position;
        gameObject.transform.position = vector3;
        gameObject.transform.localScale = new Vector3((float) data.Direction, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
        if (animateIn)
          gameObject.transform.DOMove(position1, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
        else
          gameObject.transform.position = position1;
        AudioManager.Instance.SetFollowersSing(1f);
        Action<GameObject> action = callback;
        if (action == null)
          return;
        action(gameObject);
      }
      else
      {
        System.Action action = locationChangedCallback;
        if (action == null)
          return;
        action();
      }
    }));
  }

  public static IEnumerator LerpStructure(Transform transform)
  {
    float Progress = 0.0f;
    float Duration = 1f;
    Vector3 StartPosition = transform.position - Vector3.back * 2f;
    Vector3 TargetPosition = transform.position;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      transform.position = Vector3.Lerp(StartPosition, TargetPosition, Mathf.SmoothStep(0.0f, 1f, Progress / Duration));
      yield return (object) null;
    }
    transform.position = TargetPosition;
  }

  public static StructureBrain AddStructure(
    FollowerLocation location,
    StructuresData data,
    bool emitParticles = true,
    bool save = true)
  {
    if (save)
      StructureManager.StructuresDataAtLocation(location).Add(data);
    StructureManager.ConvertFromUpgrade(data);
    StructureBrain brain = StructureBrain.GetOrCreateBrain(data);
    StructureManager.StructureChanged onStructureAdded = StructureManager.OnStructureAdded;
    if (onStructureAdded != null)
      onStructureAdded(data);
    ObjectiveManager.CheckObjectives(Objectives.TYPES.PLACE_STRUCTURES);
    if (!(StructureManager.ShouldStructureEmitVFXWhenAdded(data.Type) & emitParticles) || !((UnityEngine.Object) BiomeConstants.Instance != (UnityEngine.Object) null) || PlayerFarming.Location != FollowerLocation.Base)
      return brain;
    BiomeConstants.Instance.EmitSmokeInteractionVFX(data.Position + new Vector3(0.0f, (float) ((double) data.Bounds.y / 2.0 - 0.5), 0.0f), new Vector3((float) data.Bounds.x, (float) data.Bounds.y, 1f));
    return brain;
  }

  public static void ConvertFromUpgrade(StructuresData structure)
  {
    Structures_PlacementRegion placementRegion = StructureBrain.FindPlacementRegion(structure);
    if (placementRegion == null)
      return;
    int previousUpgradeId = placementRegion.GetPreviousUpgradeID(structure);
    structure.ID = previousUpgradeId != -1 ? previousUpgradeId : structure.ID;
  }

  public static void RemoveStructure(StructureBrain brain)
  {
    brain.Data.Destroyed = true;
    StructureManager.StructuresDataAtLocation(brain.Data.Location).Remove(brain.Data);
    StructureManager.StructuresAtLocation(brain.Data.Location).Remove(brain);
    StructureBrain.RemoveBrain(in brain);
    StructureManager.StructureChanged structureRemoved = StructureManager.OnStructureRemoved;
    if (structureRemoved == null)
      return;
    structureRemoved(brain.Data);
  }

  public static void UpdateWeeds(FollowerLocation location)
  {
    List<Structures_Weeds> Weeds = new List<Structures_Weeds>();
    List<StructureBrain> structureBrainList = StructureManager.StructuresAtLocation(location);
    foreach (StructureBrain structureBrain in structureBrainList)
    {
      if (structureBrain.Data.Type == StructureBrain.TYPES.WEEDS)
        Weeds.Add(structureBrain as Structures_Weeds);
    }
    List<Structures_PlacementRegion> PlacementRegions = new List<Structures_PlacementRegion>();
    foreach (StructureBrain structureBrain in structureBrainList)
    {
      if (structureBrain.Data.Type == StructureBrain.TYPES.PLACEMENT_REGION)
        PlacementRegions.Add(structureBrain as Structures_PlacementRegion);
    }
    if (location == FollowerLocation.Base)
    {
      bool flag = true;
      int num = 0;
      if (TimeManager.CurrentDay - DataManager.Instance.LastDayTreesAtBase < 2)
      {
        flag = false;
      }
      else
      {
        foreach (StructureBrain structureBrain in structureBrainList)
        {
          if (structureBrain.Data.Type == StructureBrain.TYPES.TREE)
            ++num;
        }
      }
      if (flag && num < 5)
      {
        DataManager.Instance.LastDayTreesAtBase = TimeManager.CurrentDay;
        StructureManager.PlantSaplings(location, PlacementRegions);
      }
    }
    if (Weeds.Count <= 0)
    {
      StructureManager.CreateWeeds(location, PlacementRegions);
    }
    else
    {
      if (Weeds.Count >= 120)
        return;
      StructureManager.GrowWeeds(location, Weeds, PlacementRegions);
    }
  }

  public static void GrowWeeds(
    FollowerLocation location,
    List<Structures_Weeds> Weeds,
    List<Structures_PlacementRegion> PlacementRegions)
  {
    foreach (Structures_Weeds weed in Weeds)
    {
      foreach (Structures_PlacementRegion placementRegion in PlacementRegions)
      {
        if (placementRegion.PlaceWeeds && !(new Vector3Int((int) placementRegion.Data.Position.x, (int) placementRegion.Data.Position.y, 0) != weed.Data.PlacementRegionPosition) && (double) UnityEngine.Random.value > 0.34999999403953552)
        {
          List<PlacementRegion.TileGridTile> tileGridTileList = new List<PlacementRegion.TileGridTile>();
          PlacementRegion.TileGridTile tileGridTile1 = placementRegion.GetTileGridTile(weed.Data.GridTilePosition + new Vector2Int(-1, 0));
          if (tileGridTile1 != null && tileGridTile1.CanPlaceObstruction)
            tileGridTileList.Add(tileGridTile1);
          PlacementRegion.TileGridTile tileGridTile2 = placementRegion.GetTileGridTile(weed.Data.GridTilePosition + new Vector2Int(1, 0));
          if (tileGridTile2 != null && tileGridTile2.CanPlaceObstruction)
            tileGridTileList.Add(tileGridTile2);
          PlacementRegion.TileGridTile tileGridTile3 = placementRegion.GetTileGridTile(weed.Data.GridTilePosition + new Vector2Int(0, 1));
          if (tileGridTile3 != null && tileGridTile3.CanPlaceObstruction)
            tileGridTileList.Add(tileGridTile3);
          PlacementRegion.TileGridTile tileGridTile4 = placementRegion.GetTileGridTile(weed.Data.GridTilePosition + new Vector2Int(0, -1));
          if (tileGridTile4 != null && tileGridTile4.CanPlaceObstruction)
            tileGridTileList.Add(tileGridTile4);
          if (tileGridTileList.Count > 0)
            UnityEngine.Random.Range(0, tileGridTileList.Count);
        }
      }
    }
  }

  public static void CreateWeeds(
    FollowerLocation location,
    List<Structures_PlacementRegion> PlacementRegions,
    PolygonCollider2D area = null,
    int count = -1)
  {
    using (List<Structures_PlacementRegion>.Enumerator enumerator = PlacementRegions.GetEnumerator())
    {
label_14:
      while (enumerator.MoveNext())
      {
        Structures_PlacementRegion current = enumerator.Current;
        int num = UnityEngine.Random.Range(3, 5);
        List<PlacementRegion.TileGridTile> tiles = new List<PlacementRegion.TileGridTile>();
        foreach (PlacementRegion.TileGridTile tileGridTile in current.Data.Grid)
        {
          if (tileGridTile.CanPlaceObstruction && ((UnityEngine.Object) area == (UnityEngine.Object) null || area.OverlapPoint((Vector2) tileGridTile.WorldPosition)))
            tiles.Add(tileGridTile);
        }
        if (location == FollowerLocation.Base)
          StructureManager.RemoveValidRanchTilesFromTiles(tiles);
        if (count > -1)
          num = count;
        while (true)
        {
          if (num > 0 && num < tiles.Count)
          {
            int index = UnityEngine.Random.Range(0, tiles.Count);
            StructureManager.PlaceWeed(location, tiles[index], current, UnityEngine.Random.Range(0, 4), UnityEngine.Random.Range(0, 3));
            tiles.RemoveAt(index);
            --num;
          }
          else
            goto label_14;
        }
      }
    }
  }

  public static void PlantSaplings(
    FollowerLocation location,
    List<Structures_PlacementRegion> PlacementRegions)
  {
    if (SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter)
      return;
    Debug.Log((object) ("PlantSaplings " + PlacementRegions.Count.ToString()));
    using (List<Structures_PlacementRegion>.Enumerator enumerator = PlacementRegions.GetEnumerator())
    {
label_15:
      while (enumerator.MoveNext())
      {
        Structures_PlacementRegion current = enumerator.Current;
        List<PlacementRegion.TileGridTile> tiles = new List<PlacementRegion.TileGridTile>();
        foreach (PlacementRegion.TileGridTile tileGridTile in current.Data.Grid)
        {
          if (tileGridTile.CanPlaceObstruction)
            tiles.Add(tileGridTile);
        }
        if (location == FollowerLocation.Base)
          StructureManager.RemoveValidRanchTilesFromTiles(tiles);
        int num = UnityEngine.Random.Range(1, 4);
        while (true)
        {
          if (num > 0 && num < tiles.Count)
          {
            Debug.Log((object) "PLLANT A RANDOM SAPLING!");
            int index = UnityEngine.Random.Range(0, tiles.Count);
            StructureManager.PlaceSapling(location, tiles[index], current);
            tiles.RemoveAt(index);
            --num;
          }
          else
            goto label_15;
        }
      }
    }
  }

  public static void PlaceIceBlock(
    FollowerLocation location,
    List<Structures_PlacementRegion> PlacementRegions)
  {
    using (List<Structures_PlacementRegion>.Enumerator enumerator = PlacementRegions.GetEnumerator())
    {
label_13:
      while (enumerator.MoveNext())
      {
        Structures_PlacementRegion current = enumerator.Current;
        List<PlacementRegion.TileGridTile> tiles = new List<PlacementRegion.TileGridTile>();
        foreach (PlacementRegion.TileGridTile tileGridTile in current.Data.Grid)
        {
          if (tileGridTile.CanPlaceObstruction)
            tiles.Add(tileGridTile);
        }
        if (location == FollowerLocation.Base)
          StructureManager.RemoveValidRanchTilesFromTiles(tiles);
        int num = UnityEngine.Random.Range(1, 1);
        while (true)
        {
          if (num > 0 && num < tiles.Count)
          {
            int index = UnityEngine.Random.Range(0, tiles.Count);
            StructureManager.PlaceIceBlock(location, tiles[index], current);
            tiles.RemoveAt(index);
            --num;
          }
          else
            goto label_13;
        }
      }
    }
  }

  public static void PlaceRotRubble(
    FollowerLocation location,
    List<Structures_PlacementRegion> PlacementRegions)
  {
    using (List<Structures_PlacementRegion>.Enumerator enumerator = PlacementRegions.GetEnumerator())
    {
label_13:
      while (enumerator.MoveNext())
      {
        Structures_PlacementRegion current = enumerator.Current;
        List<PlacementRegion.TileGridTile> tiles = new List<PlacementRegion.TileGridTile>();
        foreach (PlacementRegion.TileGridTile tileGridTile in current.Data.Grid)
        {
          if (tileGridTile.CanPlaceObstruction)
            tiles.Add(tileGridTile);
        }
        if (location == FollowerLocation.Base)
          StructureManager.RemoveValidRanchTilesFromTiles(tiles);
        int num = UnityEngine.Random.Range(1, 1);
        while (true)
        {
          if (num > 0 && num < tiles.Count)
          {
            int index = UnityEngine.Random.Range(0, tiles.Count);
            StructureManager.PlaceRotRubble(location, tiles[index], current);
            tiles.RemoveAt(index);
            --num;
          }
          else
            goto label_13;
        }
      }
    }
  }

  public static void PlaceSnowDrift(
    FollowerLocation location,
    List<Structures_PlacementRegion> PlacementRegions)
  {
    using (List<Structures_PlacementRegion>.Enumerator enumerator = PlacementRegions.GetEnumerator())
    {
label_13:
      while (enumerator.MoveNext())
      {
        Structures_PlacementRegion current = enumerator.Current;
        List<PlacementRegion.TileGridTile> tiles = new List<PlacementRegion.TileGridTile>();
        foreach (PlacementRegion.TileGridTile tileGridTile in current.Data.Grid)
        {
          if (tileGridTile.CanPlaceObstruction)
            tiles.Add(tileGridTile);
        }
        if (location == FollowerLocation.Base)
          StructureManager.RemoveValidRanchTilesFromTiles(tiles);
        int num = UnityEngine.Random.Range(1, 4);
        while (true)
        {
          if (num > 0 && num < tiles.Count)
          {
            int index = UnityEngine.Random.Range(0, tiles.Count);
            StructureManager.PlaceSnowDrift(location, tiles[index], current);
            tiles.RemoveAt(index);
            --num;
          }
          else
            goto label_13;
        }
      }
    }
  }

  public static void PlaceWeed(
    FollowerLocation location,
    List<Structures_PlacementRegion> placementRegions = null)
  {
    List<Structures_PlacementRegion> PlacementRegions = new List<Structures_PlacementRegion>();
    if (placementRegions != null)
      PlacementRegions.AddRange((IEnumerable<Structures_PlacementRegion>) placementRegions);
    foreach (StructureBrain structureBrain in StructureManager.StructuresAtLocation(location))
    {
      if (structureBrain.Data.Type == StructureBrain.TYPES.PLACEMENT_REGION && !PlacementRegions.Contains(structureBrain as Structures_PlacementRegion))
        PlacementRegions.Add(structureBrain as Structures_PlacementRegion);
    }
    foreach (Structures_PlacementRegion p in PlacementRegions)
    {
      List<PlacementRegion.TileGridTile> tiles = new List<PlacementRegion.TileGridTile>((IEnumerable<PlacementRegion.TileGridTile>) p.Data.Grid);
      if (location == FollowerLocation.Base)
        StructureManager.RemoveValidRanchTilesFromTiles(tiles);
      foreach (PlacementRegion.TileGridTile t in tiles)
      {
        float num1 = 40f;
        if (SettingsManager.Settings.Game.PerformanceMode)
          num1 = 30f;
        float num2 = (float) LocationManager.GetLocationManagersRandom(location).Next(0, 100);
        if (t.CanPlaceObstruction && (double) num2 <= (double) num1 && p.PlaceWeeds)
          StructureManager.PlaceWeed(location, t, p, LocationManager.GetLocationManagersRandom(location).Next(0, 4), LocationManager.GetLocationManagersRandom(location).Next(0, 3));
      }
      p.Data.WeedsAndRubblePlaced = true;
    }
    StructureManager.CreateWeeds(location, PlacementRegions);
  }

  public static void PlaceWeed(
    FollowerLocation location,
    PlacementRegion.TileGridTile t,
    Structures_PlacementRegion p,
    int WeedType,
    int growthStageOffset)
  {
    Vector2[] points = new Vector2[0];
    PolygonCollider2D collider = (PolygonCollider2D) null;
    if ((UnityEngine.Object) BiomeBaseManager.Instance != (UnityEngine.Object) null)
    {
      collider = BiomeBaseManager.Instance.Room.Pieces[0].Collider;
      points = collider.points;
    }
    foreach (KeyValuePair<FollowerLocation, LocationManager> locationManager in LocationManager.LocationManagers)
    {
      if (locationManager.Key == location && (UnityEngine.Object) locationManager.Value != (UnityEngine.Object) null)
      {
        Addressables_wrapper.InstantiateAsync((object) "Assets/Prefabs/Structures/Buildings/Weeds.prefab", (UnityEngine.Object) locationManager.Value.StructureLayer != (UnityEngine.Object) null ? locationManager.Value.StructureLayer : locationManager.Value.transform, true, (Action<AsyncOperationHandle<GameObject>>) (obj =>
        {
          if ((UnityEngine.Object) obj.Result == (UnityEngine.Object) null)
            return;
          if (PlayerFarming.Location == FollowerLocation.Base && !StructureManager.EnsureWithinBounds(t.WorldPosition, points, collider))
          {
            UnityEngine.Object.Destroy((UnityEngine.Object) obj.Result);
          }
          else
          {
            GameObject result = obj.Result;
            result.transform.position = t.WorldPosition;
            result.GetComponent<WeedManager>().WeedTypeChosen = WeedType;
            result.GetComponent<WeedManager>().GrowthStageOffset = growthStageOffset;
          }
        }));
        break;
      }
    }
  }

  public static bool EnsureWithinBounds(Vector3 pos, Vector2[] points, PolygonCollider2D collider)
  {
    return Utils.PointWithinPolygon(pos, points) || collider.OverlapPoint((Vector2) pos);
  }

  public static void PlaceSapling(
    FollowerLocation location,
    PlacementRegion.TileGridTile t,
    Structures_PlacementRegion p)
  {
    StructuresData infoByType = StructuresData.GetInfoByType(StructureBrain.TYPES.TREE, 0);
    Vector3 worldPosition = t.WorldPosition;
    infoByType.DontLoadMe = false;
    infoByType.IsSapling = true;
    infoByType.GrowthStage = 1f;
    infoByType.PlacementRegionPosition = new Vector3Int((int) p.Data.Position.x, (int) p.Data.Position.y, 0);
    infoByType.GridTilePosition = t.Position;
    StructureManager.BuildStructure(location, infoByType, worldPosition, new Vector2Int(1, 1));
  }

  public static void PlaceRotRubble(
    FollowerLocation location,
    PlacementRegion.TileGridTile t,
    Structures_PlacementRegion p,
    float chanceToSpawnChildren = 0.5f)
  {
    StructuresData infoByType = StructuresData.GetInfoByType(StructureBrain.TYPES.RUBBLE, 0);
    Vector3 worldPosition = t.WorldPosition;
    infoByType.DontLoadMe = false;
    infoByType.GrowthStage = 1f;
    infoByType.PlacementRegionPosition = new Vector3Int((int) p.Data.Position.x, (int) p.Data.Position.y, 0);
    infoByType.GridTilePosition = t.Position;
    infoByType.VariantIndex = 1;
    StructureManager.BuildStructure(location, infoByType, worldPosition, new Vector2Int(1, 1));
    p.AddStructureToGrid(infoByType);
    for (int x = -1; x < 1; ++x)
    {
      for (int y = -1; y < 1; ++y)
      {
        if ((x != 0 || y != 0) && (double) UnityEngine.Random.value < (double) chanceToSpawnChildren)
        {
          PlacementRegion.TileGridTile tileGridTile = p.GetTileGridTile(t.Position + new Vector2Int(x, y));
          if (tileGridTile != null)
            StructureManager.PlaceRotRubble(location, tileGridTile, p, chanceToSpawnChildren / 1.5f);
        }
      }
    }
  }

  public static void PlaceIceBlock(
    FollowerLocation location,
    PlacementRegion.TileGridTile t,
    Structures_PlacementRegion p)
  {
    StructuresData infoByType = StructuresData.GetInfoByType(StructureBrain.TYPES.ICE_BLOCK, 0);
    Vector3 worldPosition = t.WorldPosition;
    infoByType.DontLoadMe = false;
    infoByType.GrowthStage = 1f;
    infoByType.PlacementRegionPosition = new Vector3Int((int) p.Data.Position.x, (int) p.Data.Position.y, 0);
    infoByType.GridTilePosition = t.Position;
    StructureManager.BuildStructure(location, infoByType, worldPosition, new Vector2Int(1, 1));
  }

  public static void PlaceSnowDrift(
    FollowerLocation location,
    PlacementRegion.TileGridTile t,
    Structures_PlacementRegion p)
  {
    StructuresData infoByType = StructuresData.GetInfoByType(StructureBrain.TYPES.SNOW_DRIFT, 0);
    Vector3 worldPosition = t.WorldPosition;
    infoByType.DontLoadMe = false;
    infoByType.GrowthStage = 1f;
    infoByType.PlacementRegionPosition = new Vector3Int((int) p.Data.Position.x, (int) p.Data.Position.y, 0);
    infoByType.GridTilePosition = t.Position;
    StructureManager.BuildStructure(location, infoByType, worldPosition, new Vector2Int(1, 1));
  }

  public static void PlaceRubble(
    FollowerLocation location,
    List<Structures_PlacementRegion> placementRegions = null)
  {
    List<Structures_PlacementRegion> PlacementRegions = new List<Structures_PlacementRegion>();
    if (placementRegions != null)
      PlacementRegions.AddRange((IEnumerable<Structures_PlacementRegion>) placementRegions);
    foreach (StructureBrain structureBrain in StructureManager.StructuresAtLocation(location))
    {
      if (structureBrain.Data.Type == StructureBrain.TYPES.PLACEMENT_REGION && !PlacementRegions.Contains(structureBrain as Structures_PlacementRegion))
        PlacementRegions.Add(structureBrain as Structures_PlacementRegion);
    }
    foreach (Structures_PlacementRegion p in PlacementRegions)
    {
      if (!p.Data.WeedsAndRubblePlaced)
        StructureManager.PlaceRubble(location, p.ResourcesToPlace, p);
      List<PlacementRegion.TileGridTile> tiles = new List<PlacementRegion.TileGridTile>((IEnumerable<PlacementRegion.TileGridTile>) p.Data.Grid);
      if (location == FollowerLocation.Base)
        StructureManager.RemoveValidRanchTilesFromTiles(tiles);
      foreach (PlacementRegion.TileGridTile t in tiles)
      {
        float num1 = 40f;
        if (SettingsManager.Settings.Game.PerformanceMode)
          num1 = 30f;
        float num2 = (float) LocationManager.GetLocationManagersRandom(location).Next(0, 100);
        if (t.CanPlaceObstruction && (double) num2 <= (double) num1 && p.PlaceWeeds)
          StructureManager.PlaceWeed(location, t, p, LocationManager.GetLocationManagersRandom(location).Next(0, 4), LocationManager.GetLocationManagersRandom(location).Next(0, 3));
      }
      p.Data.WeedsAndRubblePlaced = true;
    }
    StructureManager.CreateWeeds(location, PlacementRegions);
    DataManager.Instance.PlacedRubble = true;
  }

  public static void PlaceRubble(
    FollowerLocation location,
    List<PlacementRegion.ResourcesAndCount> ResourcesToPlace,
    Structures_PlacementRegion p,
    PolygonCollider2D area = null)
  {
    List<PlacementRegion.TileGridTile> tileGridTileList = new List<PlacementRegion.TileGridTile>();
    using (List<PlacementRegion.ResourcesAndCount>.Enumerator enumerator = ResourcesToPlace.GetEnumerator())
    {
label_32:
      while (enumerator.MoveNext())
      {
        PlacementRegion.ResourcesAndCount current = enumerator.Current;
        int num1 = 0;
        int num2 = current.Count + UnityEngine.Random.Range(current.RandomVariation.x, current.RandomVariation.y + 1);
        while (true)
        {
          PlacementRegion.TileGridTile tileAtWorldPosition;
          bool flag;
          StructuresData infoByType;
          Vector3 worldPosition;
          do
          {
            do
            {
              do
              {
                if (num2 > 0 && num1 < 999)
                {
                  ++num1;
                  int index = UnityEngine.Random.Range(0, p.Data.Grid.Count);
                  tileAtWorldPosition = p.Data.Grid[index];
                }
                else
                  goto label_32;
              }
              while ((UnityEngine.Object) area != (UnityEngine.Object) null && !area.OverlapPoint((Vector2) tileAtWorldPosition.WorldPosition));
              if (current.MinMaxDistanceFromCenter != Vector2.zero)
              {
                for (int index = 0; index < 32 /*0x20*/; ++index)
                {
                  Vector3 insideUnitCircle = (Vector3) UnityEngine.Random.insideUnitCircle;
                  insideUnitCircle.y = (double) insideUnitCircle.y <= 0.0 ? Mathf.Lerp((float) -((double) current.MinMaxDistanceFromCenter.x / 2.0), (float) -((double) current.MinMaxDistanceFromCenter.y / 2.0), Mathf.Abs(insideUnitCircle.y)) : Mathf.Lerp(current.MinMaxDistanceFromCenter.x / 2f, current.MinMaxDistanceFromCenter.y / 2f, insideUnitCircle.y);
                  insideUnitCircle.x = (double) insideUnitCircle.x <= 0.0 ? Mathf.Lerp(-current.MinMaxDistanceFromCenter.x, -current.MinMaxDistanceFromCenter.y, Mathf.Abs(insideUnitCircle.x)) : Mathf.Lerp(current.MinMaxDistanceFromCenter.x, current.MinMaxDistanceFromCenter.y, insideUnitCircle.x);
                  tileAtWorldPosition = StructureManager.GetClosestTileGridTileAtWorldPosition(new Vector3(0.0f, -12f, 0.0f) + insideUnitCircle, p.Data.Grid);
                  if (tileAtWorldPosition != null)
                    break;
                }
              }
              flag = true;
              if ((double) current.MinDistanceBetweenSameStructure > 0.0)
              {
                foreach (StructureBrain structureBrain in StructureManager.GetAllStructuresOfType(current.Resource))
                {
                  if ((double) Vector3.Distance(structureBrain.Data.Position, tileAtWorldPosition.WorldPosition) < (double) current.MinDistanceBetweenSameStructure)
                  {
                    flag = false;
                    break;
                  }
                }
              }
            }
            while (!(tileAtWorldPosition.CanPlaceObstruction & flag));
            infoByType = StructuresData.GetInfoByType(current.Resource, 0);
            infoByType.VariantIndex = current.Variant;
            infoByType.CanRegrow = false;
            infoByType.DontLoadMe = false;
            worldPosition = tileAtWorldPosition.WorldPosition;
            infoByType.Bounds = new Vector2Int(infoByType.TILE_WIDTH, infoByType.TILE_HEIGHT);
            int x = -1;
            while (++x < infoByType.Bounds.x)
            {
              int y = -1;
              while (++y < infoByType.Bounds.y)
              {
                PlacementRegion.TileGridTile tileGridTile = p.GetTileGridTile(tileAtWorldPosition.Position + new Vector2Int(x, y));
                if (tileGridTile == null || !tileGridTile.CanPlaceStructure)
                {
                  flag = false;
                  break;
                }
              }
            }
          }
          while (!flag);
          foreach (PlacementRegion.Direction neighbouringTile in current.BlockNeighbouringTiles)
          {
            PlacementRegion.TileGridTile tileGridTile = p.GetTileGridTile(tileAtWorldPosition.Position + PlacementRegion.GetVector3FromDirection(neighbouringTile));
            if (tileGridTile != null)
            {
              tileGridTile.Obstructed = true;
              tileGridTileList.Add(tileGridTile);
            }
          }
          infoByType.PlacementRegionPosition = new Vector3Int((int) p.Data.Position.x, (int) p.Data.Position.y, 0);
          infoByType.GridTilePosition = tileAtWorldPosition.Position;
          StructureManager.BuildStructure(location, infoByType, worldPosition, new Vector2Int(infoByType.TILE_WIDTH, infoByType.TILE_HEIGHT), false);
          p.AddStructureToGrid(infoByType);
          --num2;
        }
      }
    }
    foreach (PlacementRegion.TileGridTile tileGridTile in tileGridTileList)
      tileGridTile.Obstructed = false;
  }

  public static List<StructureBrain> GetStructuresFromRole(FollowerRole role)
  {
    List<StructureBrain> structuresFromRole = new List<StructureBrain>();
    switch (role)
    {
      case FollowerRole.Worshipper:
        structuresFromRole.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.SHRINE));
        structuresFromRole.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.SHRINE_II));
        structuresFromRole.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.SHRINE_III));
        structuresFromRole.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.SHRINE_IV));
        break;
      case FollowerRole.Lumberjack:
        structuresFromRole.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.TREE));
        structuresFromRole.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.LUMBERJACK_STATION));
        structuresFromRole.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.LUMBERJACK_STATION_2));
        break;
      case FollowerRole.Farmer:
        List<Structures_CookingFire> result1 = new List<Structures_CookingFire>();
        StructureManager.TryGetAllStructuresOfType<Structures_CookingFire>(ref result1, FollowerLocation.Base);
        structuresFromRole.AddRange((IEnumerable<StructureBrain>) result1);
        break;
      case FollowerRole.StoneMiner:
        structuresFromRole.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.RUBBLE));
        structuresFromRole.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.RUBBLE_BIG));
        structuresFromRole.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.ICE_BLOCK));
        structuresFromRole.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.BLOODSTONE_MINE));
        structuresFromRole.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.BLOODSTONE_MINE_2));
        break;
      case FollowerRole.Builder:
        structuresFromRole.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.BUILD_SITE));
        break;
      case FollowerRole.Forager:
        structuresFromRole.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.BERRY_BUSH));
        break;
      case FollowerRole.Chef:
        List<Structures_CookingFire> result2 = new List<Structures_CookingFire>();
        StructureManager.TryGetAllStructuresOfType<Structures_CookingFire>(ref result2, FollowerLocation.Base);
        structuresFromRole.AddRange((IEnumerable<StructureBrain>) result2);
        break;
      case FollowerRole.Janitor:
        structuresFromRole.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.JANITOR_STATION));
        structuresFromRole.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.JANITOR_STATION_2));
        break;
      case FollowerRole.Refiner:
        structuresFromRole.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.REFINERY));
        structuresFromRole.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.REFINERY_2));
        break;
      case FollowerRole.Medic:
        structuresFromRole.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.MEDIC));
        break;
      case FollowerRole.Rancher:
        structuresFromRole.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.RANCH_2));
        break;
      case FollowerRole.RotstoneMiner:
        structuresFromRole.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.ROTSTONE_MINE));
        structuresFromRole.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.ROTSTONE_MINE_2));
        break;
    }
    return structuresFromRole;
  }

  public static PlacementRegion.TileGridTile GetClosestTileGridTileAtWorldPosition(
    Vector3 Position,
    List<PlacementRegion.TileGridTile> grid,
    float maxDistance = float.PositiveInfinity)
  {
    PlacementRegion.TileGridTile tileAtWorldPosition = (PlacementRegion.TileGridTile) null;
    foreach (PlacementRegion.TileGridTile tileGridTile in grid)
    {
      if ((tileAtWorldPosition == null || (double) Vector3.Distance(tileGridTile.WorldPosition, Position) < (double) Vector3.Distance(tileAtWorldPosition.WorldPosition, Position)) && (double) Vector3.Distance(tileGridTile.WorldPosition, Position) < (double) maxDistance)
        tileAtWorldPosition = tileGridTile;
    }
    return tileAtWorldPosition;
  }

  public static StructureBrain.TYPES GetStructureTypeByID(int ID)
  {
    StructureBrain result;
    return StructureBrain.TryFindBrainByID(in ID, out result) ? result.Data.Type : StructureBrain.TYPES.NONE;
  }

  public static T GetStructureByID<T>(int ID) where T : StructureBrain
  {
    StructureBrain result;
    return StructureBrain.TryFindBrainByID(in ID, out result) && result is T obj ? obj : default (T);
  }

  public static bool StructureTypeExists(StructureBrain.TYPES type)
  {
    foreach (StructureBrain allBrain in StructureBrain.AllBrains)
    {
      if (allBrain.Data.Type == type)
        return true;
    }
    return false;
  }

  public static bool StructureTypeExists(StructureBrain.TYPES type, FollowerLocation location)
  {
    foreach (StructureBrain structureBrain in StructureManager.StructuresAtLocation(location))
    {
      if (structureBrain.Data.Type == type || structureBrain.Data.ToBuildType == type)
        return true;
    }
    return false;
  }

  public static List<StructureBrain> GetAllStructuresOfType(
    StructureBrain.TYPES type,
    bool includeInConstruction = false,
    bool newList = false)
  {
    StructureManager.CacheGetAllStructuresOfType.Clear();
    foreach (StructureBrain allBrain in StructureBrain.AllBrains)
    {
      if (allBrain.Data.Type == type && (!allBrain.Data.ClaimedByPlayer || type == StructureBrain.TYPES.LEADER_TENT))
        StructureManager.CacheGetAllStructuresOfType.Add(allBrain);
      else if (includeInConstruction && allBrain.Data.ToBuildType == type)
        StructureManager.CacheGetAllStructuresOfType.Add(allBrain);
    }
    return newList ? new List<StructureBrain>((IEnumerable<StructureBrain>) StructureManager.CacheGetAllStructuresOfType) : StructureManager.CacheGetAllStructuresOfType;
  }

  public static List<StructureBrain> GetAllStructuresOfTypes(params StructureBrain.TYPES[] types)
  {
    StructureManager.CacheGetAllStructuresOfType.Clear();
    for (int index = 0; index < types.Length; ++index)
    {
      foreach (StructureBrain allBrain in StructureBrain.AllBrains)
      {
        if (allBrain.Data.Type == types[index] && (!allBrain.Data.ClaimedByPlayer || types[index] == StructureBrain.TYPES.LEADER_TENT))
          StructureManager.CacheGetAllStructuresOfType.Add(allBrain);
      }
    }
    return StructureManager.CacheGetAllStructuresOfType;
  }

  public static List<StructureBrain> GetAllStructuresOfTypes(
    bool includeInConstruction,
    bool newList,
    params StructureBrain.TYPES[] types)
  {
    StructureManager.CacheGetAllStructuresOfType.Clear();
    for (int index = 0; index < types.Length; ++index)
    {
      foreach (StructureBrain allBrain in StructureBrain.AllBrains)
      {
        if (allBrain.Data.Type == types[index] && (!allBrain.Data.ClaimedByPlayer || types[index] == StructureBrain.TYPES.LEADER_TENT))
          StructureManager.CacheGetAllStructuresOfType.Add(allBrain);
        else if (includeInConstruction && allBrain.Data.ToBuildType == types[index])
          StructureManager.CacheGetAllStructuresOfType.Add(allBrain);
      }
    }
    return newList ? new List<StructureBrain>((IEnumerable<StructureBrain>) StructureManager.CacheGetAllStructuresOfType) : StructureManager.CacheGetAllStructuresOfType;
  }

  public static List<StructureBrain> GetAllStructuresOfType(
    in FollowerLocation location,
    StructureBrain.TYPES type,
    bool includeInConstruction = false)
  {
    List<StructureBrain> structuresOfType = new List<StructureBrain>();
    foreach (StructureBrain structureBrain in StructureManager.StructuresAtLocation(location))
    {
      if (structureBrain.Data.Type == type && (!structureBrain.Data.ClaimedByPlayer || type == StructureBrain.TYPES.LEADER_TENT))
        structuresOfType.Add(structureBrain);
      else if (includeInConstruction && structureBrain.Data.ToBuildType == type)
        structuresOfType.Add(structureBrain);
    }
    return structuresOfType;
  }

  public static StructureBrain GetFirstStructureOfType(
    StructureBrain.TYPES type,
    bool includeInConstruction = false)
  {
    foreach (StructureBrain allBrain in StructureBrain.AllBrains)
    {
      if (allBrain.Data.Type == type && (!allBrain.Data.ClaimedByPlayer || type == StructureBrain.TYPES.LEADER_TENT) || includeInConstruction && allBrain.Data.ToBuildType == type)
        return allBrain;
    }
    return (StructureBrain) null;
  }

  public static bool IsBuilt(StructureBrain.TYPES structureType)
  {
    return StructureManager.GetAllStructuresOfType(structureType).Count > 0;
  }

  public static bool IsBuilding(StructureBrain.TYPES structureType)
  {
    return BuildSitePlot.StructureOfTypeUnderConstruction(structureType) || BuildSitePlotProject.StructureOfTypeUnderConstruction(structureType);
  }

  public static bool IsAnyUpgradeBuiltOrBuilding(StructureBrain.TYPES type)
  {
    List<StructureBrain.TYPES> upgradePath = StructuresData.GetUpgradePath(type);
    if (upgradePath != null)
    {
      for (int index = upgradePath.IndexOf(type); index < upgradePath.Count; ++index)
      {
        if (StructureManager.IsBuilt(upgradePath[index]) || StructureManager.IsBuilding(upgradePath[index]))
          return true;
      }
    }
    return false;
  }

  public static bool IsAnyUpgradeBuiltOrBuildingSafe(StructureBrain.TYPES type)
  {
    List<StructureBrain.TYPES> upgradePath = StructuresData.GetUpgradePath(type);
    if (upgradePath != null)
    {
      for (int index = upgradePath.IndexOf(type); index < upgradePath.Count; ++index)
      {
        if (StructureManager.IsBuilt(upgradePath[index]) || StructureManager.IsBuilding(upgradePath[index]))
          return true;
      }
      return false;
    }
    return StructureManager.IsBuilt(type) || StructureManager.IsBuilding(type);
  }

  public static int GetWasteCount()
  {
    int wasteCount1 = StructureManager.AccumulateOnAllStructures(FollowerLocation.Base, (Func<StructureBrain, int>) (s =>
    {
      int wasteCount2;
      switch (s)
      {
        case Structures_Poop _:
          wasteCount2 = 1;
          break;
        case Structures_DeadWorshipper structuresDeadWorshipper2:
          wasteCount2 = !structuresDeadWorshipper2.Data.Rotten || structuresDeadWorshipper2.Data.BodyWrapped ? 2 : 4;
          break;
        case Structures_Outhouse structuresOuthouse2:
          wasteCount2 = structuresOuthouse2.IsFull ? 3 : 0;
          break;
        case Structures_Morgue structuresMorgue2:
          wasteCount2 = structuresMorgue2.IsFull ? 5 : 0;
          break;
        case Structures_Meal structuresMeal2:
          wasteCount2 = structuresMeal2.Data == null || !structuresMeal2.Data.Rotten && !structuresMeal2.Data.Burned ? 0 : 1;
          break;
        case Structures_EggFollower structuresEggFollower2:
          wasteCount2 = structuresEggFollower2.Data == null || !structuresEggFollower2.Data.Rotten ? 0 : 1;
          break;
        case null:
          // ISSUE: reference to a compiler-generated method
          \u003CPrivateImplementationDetails\u003E.ThrowSwitchExpressionException((object) s);
          break;
        default:
          StructureBrain structureBrain = s;
          wasteCount2 = structureBrain.Data == null ? 0 : (structureBrain.Data.Type == StructureBrain.TYPES.VOMIT ? 1 : (structureBrain.Data.Type != StructureBrain.TYPES.DAYCARE || structureBrain.Data.Inventory.Count <= 0 || structureBrain.Data.Inventory[0].quantity < 20 ? 0 : 3));
          break;
      }
      return wasteCount2;
    }));
    foreach (StructureBrain structureBrain in StructureManager.GetAllStructuresOfType<Structures_Ranch>())
    {
      foreach (StructuresData.Ranchable_Animal animal in structureBrain.Data.Animals)
      {
        if (animal.Ailment == Interaction_Ranchable.Ailment.Stinky)
          ++wasteCount1;
        if (animal.State == Interaction_Ranchable.State.Dead)
          wasteCount1 += 2;
      }
    }
    return wasteCount1;
  }

  public static int AccumulateOnAllStructures(
    in FollowerLocation location,
    in Func<StructureBrain, int> Accumulate,
    int initialValue = 0)
  {
    foreach (StructureBrain structureBrain in StructureManager.StructuresAtLocation(location))
      initialValue += Accumulate(structureBrain);
    return initialValue;
  }

  public static int AccumulateOnAllStructures(
    in Func<StructureBrain, int> Accumulate,
    int initialValue = 0)
  {
    foreach (StructureBrain allBrain in StructureBrain.AllBrains)
      initialValue += Accumulate(allBrain);
    return initialValue;
  }

  public static bool TryGetAllStructuresOfType(
    ref List<StructureBrain> result,
    in FollowerLocation location,
    StructureBrain.TYPES type)
  {
    return StructureManager.TryGetAllStructuresOfType<StructureBrain>(ref result, location, (Func<StructureBrain, bool>) (s => s.Data != null && s.Data.Type == type));
  }

  public static int GetTotalHomesCount(bool includeBuildSites = false, bool includeUpgradeSites = false)
  {
    int totalHomesCount = 0;
    foreach (StructureBrain allBrain in StructureBrain.AllBrains)
    {
      if (allBrain.Data.Type != StructureBrain.TYPES.LEADER_TENT)
      {
        if (allBrain.Data.Type == StructureBrain.TYPES.BED && !allBrain.Data.IsCollapsed && !allBrain.Data.IsSnowedUnder)
          ++totalHomesCount;
        if (allBrain.Data.Type == StructureBrain.TYPES.BED_2 && !allBrain.Data.IsCollapsed && !allBrain.Data.IsSnowedUnder)
          ++totalHomesCount;
        if (allBrain.Data.Type == StructureBrain.TYPES.BED_3 && !allBrain.Data.IsCollapsed && !allBrain.Data.IsSnowedUnder)
          ++totalHomesCount;
        if (allBrain.Data.Type == StructureBrain.TYPES.SHARED_HOUSE && !allBrain.Data.IsCollapsed && !allBrain.Data.IsSnowedUnder)
          totalHomesCount += 3;
        if (includeBuildSites)
        {
          if ((allBrain.Data.Type == StructureBrain.TYPES.BUILD_SITE || allBrain.Data.Type == StructureBrain.TYPES.BUILDSITE_BUILDINGPROJECT) && (allBrain.Data.ToBuildType == StructureBrain.TYPES.SLEEPING_BAG || allBrain.Data.ToBuildType == StructureBrain.TYPES.BED || allBrain.Data.ToBuildType == StructureBrain.TYPES.BED_2 || allBrain.Data.ToBuildType == StructureBrain.TYPES.BED_3))
            ++totalHomesCount;
          if ((allBrain.Data.Type == StructureBrain.TYPES.BUILD_SITE || allBrain.Data.Type == StructureBrain.TYPES.BUILDSITE_BUILDINGPROJECT) && allBrain.Data.ToBuildType == StructureBrain.TYPES.SHARED_HOUSE)
            totalHomesCount += 3;
        }
        else if (includeUpgradeSites)
        {
          if ((allBrain.Data.Type == StructureBrain.TYPES.BUILD_SITE || allBrain.Data.Type == StructureBrain.TYPES.BUILDSITE_BUILDINGPROJECT) && (allBrain.Data.ToBuildType == StructureBrain.TYPES.BED_2 || allBrain.Data.ToBuildType == StructureBrain.TYPES.BED_3))
            ++totalHomesCount;
          if ((allBrain.Data.Type == StructureBrain.TYPES.BUILD_SITE || allBrain.Data.Type == StructureBrain.TYPES.BUILDSITE_BUILDINGPROJECT) && allBrain.Data.ToBuildType == StructureBrain.TYPES.SHARED_HOUSE)
            totalHomesCount += 3;
        }
      }
    }
    return totalHomesCount;
  }

  public static List<T> GetAllStructuresOfType<T>() where T : StructureBrain
  {
    List<T> structuresOfType = new List<T>();
    foreach (StructureBrain allBrain in StructureBrain.AllBrains)
    {
      if (allBrain is T obj)
        structuresOfType.Add(obj);
    }
    return structuresOfType;
  }

  public static List<T> GetAllStructuresOfType<T>(in FollowerLocation location) where T : StructureBrain
  {
    List<T> structuresOfType = new List<T>();
    foreach (StructureBrain structureBrain in StructureManager.StructuresAtLocation(location))
    {
      if (structureBrain is T obj)
        structuresOfType.Add(obj);
    }
    if (location == FollowerLocation.Church)
    {
      foreach (StructureBrain structureBrain in StructureManager.StructuresAtLocation(FollowerLocation.Base))
      {
        if (structureBrain is T obj)
          structuresOfType.Add(obj);
      }
    }
    return structuresOfType;
  }

  public static bool TryGetFirstStructureOfType<T>(out T result, in Func<T, bool> predicate = null) where T : StructureBrain
  {
    result = default (T);
    for (int index = 0; index < StructureBrain.AllBrains.Count; ++index)
    {
      result = StructureBrain.AllBrains[index] as T;
      if ((object) result != null && (predicate == null || predicate(result)))
        return true;
    }
    return false;
  }

  public static bool TryGetFirstStructureOfType<T>(
    out T result,
    in FollowerLocation location,
    in Func<T, bool> predicate = null)
    where T : StructureBrain
  {
    result = default (T);
    List<StructureBrain> structureBrainList = StructureManager.StructuresAtLocation(location);
    for (int index = 0; index < structureBrainList.Count; ++index)
    {
      result = structureBrainList[index] as T;
      if ((object) result != null && (predicate == null || predicate(result)))
        return true;
    }
    return false;
  }

  public static bool TryGetMinValueStructureOfType<T>(
    out T result,
    in FollowerLocation location,
    in Func<T, float> Value,
    in Func<T, bool> predicate = null,
    in float initValue = 3.40282347E+38f)
    where T : StructureBrain
  {
    result = default (T);
    float num1 = initValue;
    List<StructureBrain> structureBrainList = StructureManager.StructuresAtLocation(location);
    for (int index = 0; index < structureBrainList.Count; ++index)
    {
      if (structureBrainList[index] is T obj && (predicate == null || predicate(obj)))
      {
        float num2 = Value(obj);
        result = (double) num2 < (double) num1 ? obj : result;
        num1 = (double) num2 < (double) num1 ? num2 : num1;
      }
    }
    return false;
  }

  public static bool TryGetAllStructuresOfType<T>(
    ref List<T> result,
    FollowerLocation location,
    Func<T, bool> predicate = null)
    where T : StructureBrain
  {
    if (result == null)
      result = new List<T>();
    result.Clear();
    List<StructureBrain> structureBrainList = StructureManager.StructuresAtLocation(location);
    if (location == FollowerLocation.Church)
      structureBrainList.AddRange((IEnumerable<StructureBrain>) StructureManager.StructuresAtLocation(FollowerLocation.Base));
    foreach (StructureBrain structureBrain in structureBrainList)
    {
      if (structureBrain is T obj && (predicate == null || predicate(obj)))
        result.Add((T) structureBrain);
    }
    return result.Count > 0;
  }

  public static bool TryGetAllUnseededPlots(
    ref List<Structures_FarmerPlot> result,
    FollowerLocation location)
  {
    List<StructureBrain> signs = StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.FARM_PLOT_SIGN);
    StructureManager.TryGetAllStructuresOfType<Structures_SiloSeed>(ref StructureManager.cachedSilos, location);
    int num = StructureManager.TryGetAllStructuresOfType<Structures_FarmerPlot>(ref result, location, (Func<Structures_FarmerPlot, bool>) (p => p.CanPlantSeed() && !p.ReservedForWatering && Structures_SiloSeed.GetClosestSeeder(p.Data.Position, p.Data.Location, p.GetPrioritisedSeedType(signs), p.Data.Location == location ? StructureManager.cachedSilos : (List<Structures_SiloSeed>) null) != null)) ? 1 : 0;
    StructureManager.cachedSilos.Clear();
    return num != 0;
  }

  public static bool TryGetAllUnwateredPlots(
    ref List<Structures_FarmerPlot> result,
    FollowerLocation location)
  {
    return StructureManager.TryGetAllStructuresOfType<Structures_FarmerPlot>(ref result, location, (Func<Structures_FarmerPlot, bool>) (p => p.CanWater() && !p.ReservedForWatering));
  }

  public static bool TryGetAllUnfertilizedPlots(
    ref List<Structures_FarmerPlot> result,
    FollowerLocation location)
  {
    return StructureManager.TryGetAllStructuresOfType<Structures_FarmerPlot>(ref result, location, (Func<Structures_FarmerPlot, bool>) (p => p.CanFertilize() && !p.ReservedForFertilizing));
  }

  public static bool TryGetAllFullGrownPlots(
    ref List<Structures_FarmerPlot> result,
    FollowerLocation location)
  {
    return StructureManager.TryGetAllStructuresOfType<Structures_FarmerPlot>(ref result, location, (Func<Structures_FarmerPlot, bool>) (p => p.IsFullyGrown));
  }

  public static bool TryGetAllUnpickedPlots(
    ref List<Structures_BerryBush> result,
    FollowerLocation location)
  {
    return StructureManager.TryGetAllStructuresOfType<Structures_BerryBush>(ref result, location, (Func<Structures_BerryBush, bool>) (bb => !bb.ReservedForTask && !bb.ReservedByPlayer && !bb.BerryPicked && !bb.Data.Destroyed));
  }

  public static bool TryGetAllAvailableWeeds(
    ref List<Structures_Weeds> result,
    FollowerLocation location)
  {
    return StructureManager.TryGetAllStructuresOfType<Structures_Weeds>(ref result, location, (Func<Structures_Weeds, bool>) (w => !w.ReservedForTask && !w.ReservedByPlayer));
  }

  public static bool TryGetAllAvailableBushes(
    ref List<Structures_BerryBush> result,
    FollowerLocation location)
  {
    return StructureManager.TryGetAllStructuresOfType<Structures_BerryBush>(ref result, location, (Func<Structures_BerryBush, bool>) (bb => !bb.ReservedForTask && !bb.ReservedByPlayer && !bb.BerryPicked));
  }

  public static bool TryGetAllAvailableRubble(
    ref List<Structures_Rubble> result,
    FollowerLocation location)
  {
    return StructureManager.TryGetAllStructuresOfType<Structures_Rubble>(ref result, location, (Func<Structures_Rubble, bool>) (r => !r.ReservedForTask && !r.ReservedByPlayer));
  }

  public static bool TryGetAllAvailableTrees(
    ref List<Structures_Tree> result,
    FollowerLocation location)
  {
    return StructureManager.TryGetAllStructuresOfType<Structures_Tree>(ref result, location, (Func<Structures_Tree, bool>) (t => !t.ReservedForTask && !t.ReservedByPlayer && !t.TreeChopped));
  }

  public static bool TryGetAllAvailableWaste(
    ref List<Structures_Waste> result,
    FollowerLocation location)
  {
    return StructureManager.TryGetAllStructuresOfType<Structures_Waste>(ref result, location, (Func<Structures_Waste, bool>) (w => !w.ReservedForTask && !w.ReservedByPlayer));
  }

  public static Structures_DeadWorshipper GetClosestUnburiedCorpse(
    FollowerLocation location,
    Vector3 position,
    int offset)
  {
    Structures_DeadWorshipper closestUnburiedCorpse = (Structures_DeadWorshipper) null;
    float maxValue = float.MaxValue;
    SortedList<float, Structures_DeadWorshipper> sortedList = new SortedList<float, Structures_DeadWorshipper>();
    StructureManager.TryGetAllStructuresOfType<Structures_DeadWorshipper>(ref StructureManager.cachedDeadWorshippers, location, (Func<Structures_DeadWorshipper, bool>) (c => !c.ReservedForTask));
    foreach (Structures_DeadWorshipper cachedDeadWorshipper in StructureManager.cachedDeadWorshippers)
    {
      float key = Vector3.Distance(position, cachedDeadWorshipper.Data.Position);
      if ((double) key < (double) maxValue)
        sortedList.Add(key, cachedDeadWorshipper);
    }
    if (sortedList.Count > offset)
      closestUnburiedCorpse = sortedList.Values[offset];
    StructureManager.cachedDeadWorshippers.Clear();
    return closestUnburiedCorpse;
  }

  public static Dwelling.DwellingAndSlot GetFreeDwellingAndSlot(
    FollowerLocation location,
    FollowerInfo follower)
  {
    StructureManager.TryGetAllStructuresOfType<Structures_Bed>(ref StructureManager.cachedBeds, location);
    foreach (Structures_Bed cachedBed in StructureManager.cachedBeds)
    {
      if (cachedBed.Data.Type != StructureBrain.TYPES.LEADER_TENT && !cachedBed.Data.ClaimedByPlayer && !cachedBed.ReservedForTask && !cachedBed.Data.FollowersClaimedSlots.Contains(follower.ID) && !cachedBed.Data.IsCollapsed && !cachedBed.Data.IsSnowedUnder)
      {
        bool flag = true;
        foreach (FollowerInfo follower1 in DataManager.Instance.Followers)
        {
          if (follower1.DwellingID == cachedBed.Data.ID && cachedBed.Data.MultipleFollowerIDs.Count >= cachedBed.SlotCount || follower1.PreviousDwellingID != Dwelling.NO_HOME && follower1 != follower && follower1.PreviousDwellingID == cachedBed.Data.ID && follower1.DwellingID == Dwelling.NO_HOME)
          {
            flag = false;
            break;
          }
        }
        if (flag && cachedBed.Data.MultipleFollowerIDs.Count >= cachedBed.SlotCount)
        {
          for (int index = cachedBed.Data.MultipleFollowerIDs.Count - 1; index >= 0; --index)
          {
            if (FollowerInfo.GetInfoByID(cachedBed.Data.MultipleFollowerIDs[index]) != null)
              flag = false;
            else
              cachedBed.Data.MultipleFollowerIDs.RemoveAt(index);
          }
        }
        if (flag)
        {
          int dwellingslot = 0;
          foreach (int multipleFollowerId in cachedBed.Data.MultipleFollowerIDs)
          {
            FollowerInfo infoById = FollowerInfo.GetInfoByID(multipleFollowerId);
            if ((infoById != null ? (infoById.DwellingSlot != dwellingslot ? 1 : 0) : 1) == 0)
              ++dwellingslot;
            else
              break;
          }
          StructureManager.cachedBeds.Clear();
          return new Dwelling.DwellingAndSlot(cachedBed.Data.ID, dwellingslot, cachedBed.Level);
        }
      }
    }
    StructureManager.cachedBeds.Clear();
    return (Dwelling.DwellingAndSlot) null;
  }

  public static PlacementRegion.TileGridTile GetCloseTile(
    Vector3 Position,
    FollowerLocation location)
  {
    List<Structures_PlacementRegion> structuresPlacementRegionList = new List<Structures_PlacementRegion>();
    foreach (StructureBrain structureBrain in StructureManager.StructuresAtLocation(location))
    {
      if (structureBrain.Data.Type == StructureBrain.TYPES.PLACEMENT_REGION)
        structuresPlacementRegionList.Add(structureBrain as Structures_PlacementRegion);
    }
    PlacementRegion.TileGridTile closeTile = (PlacementRegion.TileGridTile) null;
    float num1 = float.MaxValue;
    foreach (StructureBrain structureBrain in structuresPlacementRegionList)
    {
      foreach (PlacementRegion.TileGridTile tileGridTile in structureBrain.Data.Grid)
      {
        if (tileGridTile != null && !tileGridTile.Occupied && !tileGridTile.Obstructed && !tileGridTile.ReservedForWaste)
        {
          float num2 = Vector2.Distance((Vector2) Position, (Vector2) tileGridTile.WorldPosition);
          if ((double) num2 < (double) num1)
          {
            closeTile = tileGridTile;
            num1 = num2;
          }
        }
      }
    }
    return closeTile;
  }

  public static PlacementRegion.TileGridTile GetBestWasteTile(FollowerLocation location)
  {
    PlacementRegion.TileGridTile bestWasteTile = (PlacementRegion.TileGridTile) null;
    List<PlacementRegion.TileGridTile> tileGridTileList = new List<PlacementRegion.TileGridTile>();
    List<Structures_PlacementRegion> structuresPlacementRegionList = new List<Structures_PlacementRegion>();
    List<StructureBrain> structureBrainList = StructureManager.StructuresAtLocation(location);
    foreach (StructureBrain structureBrain in structureBrainList)
    {
      if (structureBrain.Data.Type == StructureBrain.TYPES.PLACEMENT_REGION)
        structuresPlacementRegionList.Add(structureBrain as Structures_PlacementRegion);
    }
    foreach (Structures_PlacementRegion structuresPlacementRegion in structuresPlacementRegionList)
    {
      int num1 = 50;
      while (--num1 >= 0)
      {
        PlacementRegion.TileGridTile tileGridTile = structuresPlacementRegion.Data.Grid[UnityEngine.Random.Range(0, structuresPlacementRegion.Data.Grid.Count)];
        if (tileGridTile != null && !tileGridTile.Occupied && !tileGridTile.Obstructed && !tileGridTile.ReservedForWaste)
          tileGridTileList.Add(tileGridTile);
      }
      if (tileGridTileList.Count <= 0)
      {
        int num2 = 50;
        while (--num2 >= 0)
        {
          PlacementRegion.TileGridTile tileGridTile = structuresPlacementRegion.Data.Grid[UnityEngine.Random.Range(0, structuresPlacementRegion.Data.Grid.Count)];
          if (tileGridTile != null && !tileGridTile.Occupied)
            tileGridTileList.Add(tileGridTile);
        }
      }
      if (tileGridTileList.Count > 0)
      {
        float num3 = float.MinValue;
        foreach (PlacementRegion.TileGridTile tileGridTile in tileGridTileList)
        {
          float num4 = float.MaxValue;
          foreach (StructureBrain structureBrain in structureBrainList)
          {
            if (structureBrain.Data.Type != StructureBrain.TYPES.PLACEMENT_REGION)
            {
              float num5 = Vector3.Distance(structureBrain.Data.Position, tileGridTile.WorldPosition);
              if ((double) num5 < (double) num4)
                num4 = num5;
            }
          }
          if ((double) num3 < (double) num4)
          {
            num3 = num4;
            bestWasteTile = tileGridTile;
          }
        }
        if (bestWasteTile != null)
          break;
      }
    }
    return bestWasteTile;
  }

  public static bool ShouldStructureEmitVFXWhenAdded(StructureBrain.TYPES type)
  {
    switch (type)
    {
      case StructureBrain.TYPES.NONE:
      case StructureBrain.TYPES.TREE:
      case StructureBrain.TYPES.PLACEMENT_REGION:
      case StructureBrain.TYPES.MEAL:
      case StructureBrain.TYPES.RUBBLE:
      case StructureBrain.TYPES.WEEDS:
      case StructureBrain.TYPES.POOP:
      case StructureBrain.TYPES.MEAL_MEAT:
      case StructureBrain.TYPES.MEAL_GREAT:
      case StructureBrain.TYPES.MEAL_GRASS:
      case StructureBrain.TYPES.MEAL_GOOD_FISH:
      case StructureBrain.TYPES.MEAL_FOLLOWER_MEAT:
      case StructureBrain.TYPES.MEAL_MUSHROOMS:
      case StructureBrain.TYPES.MEAL_POOP:
      case StructureBrain.TYPES.MEAL_ROTTEN:
      case StructureBrain.TYPES.MEAL_EGG:
      case StructureBrain.TYPES.POOP_GOLD:
      case StructureBrain.TYPES.POOP_RAINBOW:
      case StructureBrain.TYPES.POOP_MASSIVE:
      case StructureBrain.TYPES.POOP_DEVOTION:
      case StructureBrain.TYPES.POOP_PET:
      case StructureBrain.TYPES.POOP_GLOW:
      case StructureBrain.TYPES.ICE_BLOCK:
      case StructureBrain.TYPES.POOP_ROTSTONE:
      case StructureBrain.TYPES.SNOW_DRIFT:
        return false;
      default:
        return true;
    }
  }

  public static void Reset()
  {
    StructureManager.StructureBrains.Clear();
    foreach (StructureBrain brain in new List<StructureBrain>((IEnumerable<StructureBrain>) StructureBrain.AllBrains))
      StructureBrain.RemoveBrain(in brain);
  }

  public static void BuildAllStructures()
  {
    foreach (Structures_BuildSite structuresBuildSite in StructureManager.GetAllStructuresOfType<Structures_BuildSite>())
      structuresBuildSite.BuildProgress = (float) StructuresData.BuildDurationGameMinutes(structuresBuildSite.Data.ToBuildType);
    foreach (Structures_BuildSiteProject buildSiteProject in StructureManager.GetAllStructuresOfType<Structures_BuildSiteProject>())
      buildSiteProject.BuildProgress = (float) StructuresData.BuildDurationGameMinutes(buildSiteProject.Data.ToBuildType);
  }

  public static void BreakRandomBeds()
  {
    List<Structures_Bed> structuresBedList = new List<Structures_Bed>((IEnumerable<Structures_Bed>) StructureManager.GetAllStructuresOfType<Structures_Bed>());
    for (int index = structuresBedList.Count - 1; index >= 0; --index)
    {
      if (structuresBedList[index].IsCollapsed || structuresBedList[index].Data.Type == StructureBrain.TYPES.BED_3 || structuresBedList[index].Data.Type == StructureBrain.TYPES.SHARED_HOUSE)
        structuresBedList.Remove(structuresBedList[index]);
    }
    for (int index = 0; index < structuresBedList.Count; ++index)
      structuresBedList[index].Collapse(true, true, false);
  }

  public static void ClearAllWaste()
  {
    List<StructureBrain> structuresOfType = StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.VOMIT);
    List<Structures_Poop> result = new List<Structures_Poop>();
    StructureManager.TryGetAllStructuresOfType<Structures_Poop>(ref result, FollowerLocation.Base);
    for (int index = structuresOfType.Count - 1; index >= 0; --index)
    {
      StructureBrain vomit = structuresOfType[index];
      GameManager.GetInstance().StartCoroutine((IEnumerator) StructureManager.\u003CClearAllWaste\u003Eg__Delay\u007C80_0(UnityEngine.Random.Range(0.0f, 1f), (System.Action) (() =>
      {
        BiomeConstants.Instance.EmitBloodSplatter(vomit.Data.Position, Vector3.back, Color.green);
        BiomeConstants.Instance.EmitBloodDieEffect(vomit.Data.Position, Vector3.back, Color.green);
        AudioManager.Instance.PlayOneShot("event:/followers/poop_pop", vomit.Data.Position);
        Vomit.SpawnLoot(vomit.Data.Position);
        vomit.Remove();
      })));
    }
    for (int index = result.Count - 1; index >= 0; --index)
    {
      StructureBrain poop = (StructureBrain) result[index];
      GameManager.GetInstance().StartCoroutine((IEnumerator) StructureManager.\u003CClearAllWaste\u003Eg__Delay\u007C80_0(UnityEngine.Random.Range(0.0f, 1f), (System.Action) (() =>
      {
        BiomeConstants.Instance.EmitSmokeExplosionVFX(poop.Data.Position);
        AudioManager.Instance.PlayOneShot("event:/followers/poop_pop", poop.Data.Position);
        poop.Remove();
      })));
    }
  }

  public static bool IsCollapsible(StructureBrain.TYPES type)
  {
    switch (type)
    {
      case StructureBrain.TYPES.TREE:
      case StructureBrain.TYPES.COTTON_BUSH:
      case StructureBrain.TYPES.BUILD_PLOT:
      case StructureBrain.TYPES.SHRINE:
      case StructureBrain.TYPES.BUILD_SITE:
      case StructureBrain.TYPES.PLACEMENT_REGION:
      case StructureBrain.TYPES.FARM_PLOT:
      case StructureBrain.TYPES.GRAVE:
      case StructureBrain.TYPES.DEAD_WORSHIPPER:
      case StructureBrain.TYPES.VOMIT:
      case StructureBrain.TYPES.MEAL:
      case StructureBrain.TYPES.BODY_PIT:
      case StructureBrain.TYPES.PLANK_PATH:
      case StructureBrain.TYPES.GRAVE2:
      case StructureBrain.TYPES.RUBBLE:
      case StructureBrain.TYPES.WEEDS:
      case StructureBrain.TYPES.FOOD_STORAGE:
      case StructureBrain.TYPES.BERRY_BUSH:
      case StructureBrain.TYPES.POOP:
      case StructureBrain.TYPES.PUMPKIN_BUSH:
      case StructureBrain.TYPES.TEMPLE:
      case StructureBrain.TYPES.BUILDSITE_BUILDINGPROJECT:
      case StructureBrain.TYPES.SHRINE_II:
      case StructureBrain.TYPES.COLLECTED_RESOURCES_CHEST:
      case StructureBrain.TYPES.MEAL_MEAT:
      case StructureBrain.TYPES.MEAL_GREAT:
      case StructureBrain.TYPES.MEAL_GRASS:
      case StructureBrain.TYPES.MEAL_GOOD_FISH:
      case StructureBrain.TYPES.MEAL_FOLLOWER_MEAT:
      case StructureBrain.TYPES.MUSHROOM_BUSH:
      case StructureBrain.TYPES.RED_FLOWER_BUSH:
      case StructureBrain.TYPES.WHITE_FLOWER_BUSH:
      case StructureBrain.TYPES.MEAL_MUSHROOMS:
      case StructureBrain.TYPES.TEMPLE_II:
      case StructureBrain.TYPES.MEAL_POOP:
      case StructureBrain.TYPES.MEAL_ROTTEN:
      case StructureBrain.TYPES.MISSION_SHRINE:
      case StructureBrain.TYPES.SHRINE_PASSIVE:
      case StructureBrain.TYPES.SHRINE_III:
      case StructureBrain.TYPES.SHRINE_IV:
      case StructureBrain.TYPES.SHRINE_PASSIVE_II:
      case StructureBrain.TYPES.SHRINE_PASSIVE_III:
      case StructureBrain.TYPES.TEMPLE_III:
      case StructureBrain.TYPES.TEMPLE_IV:
      case StructureBrain.TYPES.TILE_PATH:
      case StructureBrain.TYPES.RUBBLE_BIG:
      case StructureBrain.TYPES.RATAU_SHRINE:
      case StructureBrain.TYPES.MEAL_GREAT_FISH:
      case StructureBrain.TYPES.MEAL_BAD_FISH:
      case StructureBrain.TYPES.BEETROOT_BUSH:
      case StructureBrain.TYPES.CAULIFLOWER_BUSH:
      case StructureBrain.TYPES.MEAL_BERRIES:
      case StructureBrain.TYPES.MEAL_MEDIUM_VEG:
      case StructureBrain.TYPES.MEAL_BAD_MIXED:
      case StructureBrain.TYPES.MEAL_MEDIUM_MIXED:
      case StructureBrain.TYPES.MEAL_GREAT_MIXED:
      case StructureBrain.TYPES.MEAL_DEADLY:
      case StructureBrain.TYPES.MEAL_BAD_MEAT:
      case StructureBrain.TYPES.MEAL_GREAT_MEAT:
      case StructureBrain.TYPES.FISHING_SPOT:
      case StructureBrain.TYPES.MEAL_BURNED:
      case StructureBrain.TYPES.TILE_FLOWERS:
      case StructureBrain.TYPES.TILE_HAY:
      case StructureBrain.TYPES.TILE_PLANKS:
      case StructureBrain.TYPES.TILE_SPOOKYPLANKS:
      case StructureBrain.TYPES.TILE_REDGRASS:
      case StructureBrain.TYPES.TILE_ROCKS:
      case StructureBrain.TYPES.TILE_BRICKS:
      case StructureBrain.TYPES.TILE_BLOOD:
      case StructureBrain.TYPES.TILE_WATER:
      case StructureBrain.TYPES.TILE_GOLD:
      case StructureBrain.TYPES.TILE_MOSAIC:
      case StructureBrain.TYPES.TILE_FLOWERSROCKY:
      case StructureBrain.TYPES.WEBBER_SKULL:
      case StructureBrain.TYPES.MEAL_EGG:
      case StructureBrain.TYPES.SHRINE_DISCIPLE_BOOST:
      case StructureBrain.TYPES.POOP_GOLD:
      case StructureBrain.TYPES.POOP_RAINBOW:
      case StructureBrain.TYPES.POOP_MASSIVE:
      case StructureBrain.TYPES.POOP_DEVOTION:
      case StructureBrain.TYPES.POOP_PET:
      case StructureBrain.TYPES.POOP_GLOW:
      case StructureBrain.TYPES.SHRINE_PLEASURE:
      case StructureBrain.TYPES.HOPS_BUSH:
      case StructureBrain.TYPES.GRAPES_BUSH:
      case StructureBrain.TYPES.EGG_FOLLOWER:
      case StructureBrain.TYPES.SHRINE_DISCIPLE_COLLECTION:
      case StructureBrain.TYPES.SOZO_BUSH:
      case StructureBrain.TYPES.SNOW_FRUIT_BUSH:
      case StructureBrain.TYPES.MEAL_SPICY:
      case StructureBrain.TYPES.CHILLI_BUSH:
      case StructureBrain.TYPES.GRASS_BUSH:
      case StructureBrain.TYPES.ICE_BLOCK:
      case StructureBrain.TYPES.MEAL_SNOW_FRUIT:
      case StructureBrain.TYPES.SNOWMAN:
      case StructureBrain.TYPES.FURNACE_1:
      case StructureBrain.TYPES.FURNACE_2:
      case StructureBrain.TYPES.FURNACE_3:
      case StructureBrain.TYPES.TOXIC_WASTE:
      case StructureBrain.TYPES.POOP_ROTSTONE:
      case StructureBrain.TYPES.ICE_SCULPTURE:
      case StructureBrain.TYPES.SNOW_DRIFT:
      case StructureBrain.TYPES.MEAL_MILK_BAD:
      case StructureBrain.TYPES.MEAL_MILK_GOOD:
      case StructureBrain.TYPES.MEAL_MILK_GREAT:
      case StructureBrain.TYPES.DECORATION_DLC_ROT_FLOOR:
      case StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_FLOOR:
      case StructureBrain.TYPES.PROXIMITY_FURNACE:
      case StructureBrain.TYPES.ICE_SCULPTURE_1:
      case StructureBrain.TYPES.ICE_SCULPTURE_2:
      case StructureBrain.TYPES.ICE_SCULPTURE_3:
        return false;
      default:
        return true;
    }
  }

  public static List<StructureBrain.TYPES> RanchingStructures
  {
    get
    {
      return new List<StructureBrain.TYPES>()
      {
        StructureBrain.TYPES.RANCH,
        StructureBrain.TYPES.RANCH_2,
        StructureBrain.TYPES.RANCH_FENCE,
        StructureBrain.TYPES.RANCH_TROUGH,
        StructureBrain.TYPES.RANCH_HUTCH,
        StructureBrain.TYPES.RANCH_CHOPPING_BLOCK
      };
    }
  }

  public static List<StructureBrain.TYPES> IndestructibleByRanchStructures
  {
    get
    {
      return new List<StructureBrain.TYPES>()
      {
        StructureBrain.TYPES.WEBBER_SKULL,
        StructureBrain.TYPES.DEAD_WORSHIPPER,
        StructureBrain.TYPES.EGG_FOLLOWER,
        StructureBrain.TYPES.TREE,
        StructureBrain.TYPES.TREE,
        StructureBrain.TYPES.RUBBLE,
        StructureBrain.TYPES.RUBBLE_BIG,
        StructureBrain.TYPES.POOP,
        StructureBrain.TYPES.POOP_GLOW,
        StructureBrain.TYPES.POOP_GOLD,
        StructureBrain.TYPES.POOP_MASSIVE,
        StructureBrain.TYPES.POOP_PET,
        StructureBrain.TYPES.POOP_RAINBOW,
        StructureBrain.TYPES.POOP_DEVOTION,
        StructureBrain.TYPES.POOP_ROTSTONE,
        StructureBrain.TYPES.VOMIT
      };
    }
  }

  public static List<StructureBrain.TYPES> OnlyPlaceableInRanch
  {
    get => StructureManager._onlyPlaceableInRanch;
  }

  public static Structures_LightningRod IsWithinLightningRod(Vector3 position)
  {
    BoxCollider2D boxCollider2D = GameManager.GetInstance().GetComponent<BoxCollider2D>();
    if ((UnityEngine.Object) boxCollider2D == (UnityEngine.Object) null)
    {
      boxCollider2D = GameManager.GetInstance().gameObject.AddComponent<BoxCollider2D>();
      boxCollider2D.isTrigger = true;
    }
    foreach (Structures_LightningRod structuresLightningRod in new List<Structures_LightningRod>((IEnumerable<Structures_LightningRod>) StructureManager.GetAllStructuresOfType<Structures_LightningRod>()))
    {
      if (!structuresLightningRod.Data.IsCollapsed || structuresLightningRod.Data.IsSnowedUnder)
      {
        float num = structuresLightningRod.Data.Type == StructureBrain.TYPES.LIGHTNING_ROD ? Interaction_LightningRod.EFFECTIVE_DISTANCE_LVL1 : Interaction_LightningRod.EFFECTIVE_DISTANCE_LVL2;
        boxCollider2D.size = Vector2.one * num;
        boxCollider2D.transform.position = structuresLightningRod.Data.Position + Vector3.up * 0.7f;
        boxCollider2D.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, -45f));
        if (boxCollider2D.OverlapPoint((Vector2) position))
          return structuresLightningRod;
      }
    }
    return (Structures_LightningRod) null;
  }

  public static StructureBrain GetPossibleCollapsableStructure()
  {
    List<StructureBrain> collapsableStructures = StructureManager.GetPossibleCollapsableStructures();
    return collapsableStructures.Count <= 0 ? (StructureBrain) null : collapsableStructures[UnityEngine.Random.Range(0, collapsableStructures.Count)];
  }

  public static List<StructureBrain> GetPossibleCollapsableStructures()
  {
    List<StructureBrain> collapsableStructures = new List<StructureBrain>();
    foreach (StructureBrain allBrain in StructureBrain.AllBrains)
    {
      if (StructureManager.IsCollapsible(allBrain.Data.Type) && !allBrain.Data.IsCollapsed && !allBrain.Data.IsSnowedUnder && allBrain.Data.Location == FollowerLocation.Base && allBrain.Data.FollowerID == -1)
        collapsableStructures.Add(allBrain);
    }
    return collapsableStructures;
  }

  public static StructureBrain GetPossibleLighningStrikedStructure()
  {
    List<StructureBrain> collapsableStructures = StructureManager.GetPossibleCollapsableStructures();
    if (collapsableStructures.Count <= 0)
      return (StructureBrain) null;
    StructureBrain structureBrain = collapsableStructures[UnityEngine.Random.Range(0, collapsableStructures.Count)];
    if (structureBrain.Data.Type == StructureBrain.TYPES.VOLCANIC_SPA)
    {
      foreach (Interaction_VolcanicSpa healingBay in Interaction_VolcanicSpa.HealingBays)
      {
        if ((UnityEngine.Object) healingBay != (UnityEngine.Object) null && healingBay.currentSpaOccupants != null && healingBay.currentSpaOccupants.Count > 0)
          return (StructureBrain) null;
      }
    }
    return (StructureBrain) StructureManager.IsWithinLightningRod(structureBrain.Data.Position) ?? structureBrain;
  }

  public static int GetBuildingWarmth(StructureBrain brain)
  {
    return StructureManager.GetBuildingWarmth(brain.Data.Type, brain);
  }

  public static int GetBuildingWarmth(StructureBrain.TYPES type, StructureBrain brain = null)
  {
    switch (type)
    {
      case StructureBrain.TYPES.SHRINE:
      case StructureBrain.TYPES.SHRINE_II:
      case StructureBrain.TYPES.SHRINE_III:
      case StructureBrain.TYPES.SHRINE_IV:
        return brain != null && !brain.Data.FullyFueled ? 0 : 20;
      case StructureBrain.TYPES.DRUM_CIRCLE:
        return brain != null && (double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.LastDrumCircleTime >= 2400.0 ? 0 : 35;
      case StructureBrain.TYPES.SHRINE_PASSIVE:
      case StructureBrain.TYPES.SHRINE_PASSIVE_II:
      case StructureBrain.TYPES.SHRINE_PASSIVE_III:
        return brain != null && !brain.Data.FullyFueled ? 0 : 10;
      case StructureBrain.TYPES.DECORATION_TORCH_BIG:
        return brain != null && !brain.Data.FullyFueled ? 0 : 10;
      case StructureBrain.TYPES.DECORATION_SPIDER_TORCH:
        return brain != null && !brain.Data.FullyFueled ? 0 : 5;
      case StructureBrain.TYPES.DECORATION_OLDFAITH_TORCH:
        return brain != null && !brain.Data.FullyFueled ? 0 : 10;
      case StructureBrain.TYPES.FURNACE_1:
        if (brain == null)
          return 20;
        return brain == null || brain.Data.Fuel <= 0 ? 0 : Mathf.RoundToInt((float) ((double) brain.Data.Fuel / (double) brain.Data.MaxFuel * 100.0));
      case StructureBrain.TYPES.FURNACE_2:
        if (brain == null)
          return 30;
        return brain == null || brain.Data.Fuel <= 0 ? 0 : Mathf.RoundToInt((float) ((double) brain.Data.Fuel / (double) brain.Data.MaxFuel * 100.0));
      case StructureBrain.TYPES.FURNACE_3:
        if (brain == null)
          return 40;
        return brain == null || brain.Data.Fuel <= 0 ? 0 : Mathf.RoundToInt((float) ((double) brain.Data.Fuel / (double) brain.Data.MaxFuel * 100.0));
      default:
        return 0;
    }
  }

  public static StructuresData.Ranchable_Animal GetAnimalByID(int ID)
  {
    for (int index = 0; index < DataManager.Instance.BreakingOutAnimals.Count; ++index)
    {
      if (DataManager.Instance.BreakingOutAnimals[index].ID == ID)
        return DataManager.Instance.BreakingOutAnimals[index];
    }
    for (int index = 0; index < DataManager.Instance.DeadAnimalsTemporaryList.Count; ++index)
    {
      if (DataManager.Instance.DeadAnimalsTemporaryList[index].ID == ID)
        return DataManager.Instance.DeadAnimalsTemporaryList[index];
    }
    for (int index = 0; index < DataManager.Instance.FollowingPlayerAnimals.Length; ++index)
    {
      if (DataManager.Instance.FollowingPlayerAnimals[index] != null && DataManager.Instance.FollowingPlayerAnimals[index].ID == ID)
        return DataManager.Instance.FollowingPlayerAnimals[index];
    }
    List<StructuresData> structuresDataList = new List<StructuresData>();
    foreach (StructuresData baseStructure in DataManager.Instance.BaseStructures)
    {
      if (baseStructure.Type == StructureBrain.TYPES.RANCH || baseStructure.Type == StructureBrain.TYPES.RANCH_2)
        structuresDataList.Add(baseStructure);
    }
    for (int index1 = 0; index1 < structuresDataList.Count; ++index1)
    {
      for (int index2 = 0; index2 < structuresDataList[index1].Animals.Count; ++index2)
      {
        if (structuresDataList[index1].Animals[index2] != null && structuresDataList[index1].Animals[index2].ID == ID)
          return structuresDataList[index1].Animals[index2];
      }
    }
    return (StructuresData.Ranchable_Animal) null;
  }

  public static bool HasAnimalsInTheBase(bool checkPlayerAnimal = false)
  {
    List<StructureBrain> structuresOfTypes = StructureManager.GetAllStructuresOfTypes(StructureBrain.TYPES.RANCH, StructureBrain.TYPES.RANCH_2);
    bool flag = false;
    for (int index = 0; index < structuresOfTypes.Count; ++index)
    {
      foreach (StructuresData.Ranchable_Animal animal in structuresOfTypes[index].Data.Animals)
      {
        if (animal.State != Interaction_Ranchable.State.Dead)
        {
          if (!checkPlayerAnimal)
          {
            animal.IsPlayersAnimal();
            flag = true;
            break;
          }
          flag = true;
          break;
        }
      }
    }
    return flag;
  }

  public static void RemoveValidRanchTilesFromTiles(List<PlacementRegion.TileGridTile> tiles)
  {
    if (PlayerFarming.Location != FollowerLocation.Base)
      return;
    foreach (Structures_Ranch structuresRanch in StructureManager.GetAllStructuresOfType<Structures_Ranch>())
    {
      if (structuresRanch.HasValidEnclosure())
      {
        for (int index = 0; index < structuresRanch.RanchingTiles.Count; ++index)
        {
          if (tiles.Contains(structuresRanch.RanchingTiles[index]))
            tiles.Remove(structuresRanch.RanchingTiles[index]);
        }
      }
    }
  }

  public static bool HasFollowerDeadWorshipper(int followerID)
  {
    foreach (StructureBrain structureBrain in StructureManager.StructuresAtLocation(FollowerLocation.Base))
    {
      if (structureBrain.Data.Type == StructureBrain.TYPES.DEAD_WORSHIPPER && structureBrain.Data.FollowerID == followerID)
        return true;
    }
    return false;
  }

  [CompilerGenerated]
  public static IEnumerator \u003CClearAllWaste\u003Eg__Delay\u007C80_0(
    float delay,
    System.Action callback)
  {
    yield return (object) new WaitForSeconds(delay);
    System.Action action = callback;
    if (action != null)
      action();
  }

  public delegate void StructuresPlaced();

  public delegate void StructureChanged(StructuresData structure);
}
