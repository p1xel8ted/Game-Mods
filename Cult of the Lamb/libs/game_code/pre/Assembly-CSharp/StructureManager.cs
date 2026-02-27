// Decompiled with JetBrains decompiler
// Type: StructureManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class StructureManager
{
  public static Dictionary<FollowerLocation, List<StructureBrain>> StructureBrains = new Dictionary<FollowerLocation, List<StructureBrain>>();
  public static StructureManager.StructuresPlaced OnStructuresPlaced;
  public static StructureManager.StructureChanged OnStructureAdded;
  public static StructureManager.StructureChanged OnStructureMoved;
  public static StructureManager.StructureChanged OnStructureUpgraded;
  public static StructureManager.StructureChanged OnStructureRemoved;

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
    LocationManager locationManager;
    if (!LocationManager.LocationManagers.TryGetValue(location, out locationManager) || !((UnityEngine.Object) locationManager != (UnityEngine.Object) null))
      return;
    if (!data.PrefabPath.Contains("Assets"))
      data.PrefabPath = $"Assets/{data.PrefabPath}.prefab";
    Addressables.InstantiateAsync((object) data.PrefabPath).Completed += (Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      GameObject gameObject = locationManager.PlaceStructure(data, obj.Result.gameObject);
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
    });
  }

  private static IEnumerator LerpStructure(Transform transform)
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
    if (!(StructureManager.ShouldStructureEmitVFXWhenAdded(data.Type) & emitParticles) || !((UnityEngine.Object) BiomeConstants.Instance != (UnityEngine.Object) null))
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
    StructureBrain.RemoveBrain(brain);
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

  private static void GrowWeeds(
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

  private static void CreateWeeds(
    FollowerLocation location,
    List<Structures_PlacementRegion> PlacementRegions)
  {
    using (List<Structures_PlacementRegion>.Enumerator enumerator = PlacementRegions.GetEnumerator())
    {
label_10:
      while (enumerator.MoveNext())
      {
        Structures_PlacementRegion current = enumerator.Current;
        int num = UnityEngine.Random.Range(3, 5);
        List<PlacementRegion.TileGridTile> tileGridTileList = new List<PlacementRegion.TileGridTile>();
        foreach (PlacementRegion.TileGridTile tileGridTile in current.Data.Grid)
        {
          if (tileGridTile.CanPlaceObstruction)
            tileGridTileList.Add(tileGridTile);
        }
        while (true)
        {
          if (num > 0 && num < tileGridTileList.Count)
          {
            int index = UnityEngine.Random.Range(0, tileGridTileList.Count);
            StructureManager.PlaceWeed(location, tileGridTileList[index], current, -1, 0);
            tileGridTileList.RemoveAt(index);
            --num;
          }
          else
            goto label_10;
        }
      }
    }
  }

  public static void PlantSaplings(
    FollowerLocation location,
    List<Structures_PlacementRegion> PlacementRegions)
  {
    Debug.Log((object) ("PlantSaplings " + (object) PlacementRegions.Count));
    using (List<Structures_PlacementRegion>.Enumerator enumerator = PlacementRegions.GetEnumerator())
    {
label_11:
      while (enumerator.MoveNext())
      {
        Structures_PlacementRegion current = enumerator.Current;
        List<PlacementRegion.TileGridTile> tileGridTileList = new List<PlacementRegion.TileGridTile>();
        foreach (PlacementRegion.TileGridTile tileGridTile in current.Data.Grid)
        {
          if (tileGridTile.CanPlaceObstruction)
            tileGridTileList.Add(tileGridTile);
        }
        int num = UnityEngine.Random.Range(1, 4);
        while (true)
        {
          if (num > 0 && num < tileGridTileList.Count)
          {
            Debug.Log((object) "PLLANT A RANDOM SAPLING!");
            int index = UnityEngine.Random.Range(0, tileGridTileList.Count);
            StructureManager.PlaceSapling(location, tileGridTileList[index], current);
            tileGridTileList.RemoveAt(index);
            --num;
          }
          else
            goto label_11;
        }
      }
    }
  }

  private static void PlaceWeed(
    FollowerLocation location,
    PlacementRegion.TileGridTile t,
    Structures_PlacementRegion p,
    int WeedType,
    int growthStageOffset)
  {
    foreach (KeyValuePair<FollowerLocation, LocationManager> locationManager in LocationManager.LocationManagers)
    {
      if (locationManager.Key == location && (UnityEngine.Object) locationManager.Value != (UnityEngine.Object) null)
      {
        Addressables.InstantiateAsync((object) "Assets/Prefabs/Structures/Buildings/Weeds.prefab", (UnityEngine.Object) locationManager.Value.StructureLayer != (UnityEngine.Object) null ? locationManager.Value.StructureLayer : locationManager.Value.transform, true).Completed += (Action<AsyncOperationHandle<GameObject>>) (obj =>
        {
          if ((UnityEngine.Object) obj.Result == (UnityEngine.Object) null)
            return;
          GameObject result = obj.Result;
          result.transform.position = t.WorldPosition;
          result.GetComponent<WeedManager>().WeedTypeChosen = WeedType;
          result.GetComponent<WeedManager>().GrowthStageOffset = growthStageOffset;
        });
        break;
      }
    }
  }

  private static void PlaceSapling(
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
      List<PlacementRegion.TileGridTile> tileGridTileList = new List<PlacementRegion.TileGridTile>();
      if (!p.Data.WeedsAndRubblePlaced)
      {
        using (List<PlacementRegion.ResourcesAndCount>.Enumerator enumerator = p.ResourcesToPlace.GetEnumerator())
        {
label_42:
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
                  if (num2 > 0 && num1 < 999)
                  {
                    ++num1;
                    int index1 = UnityEngine.Random.Range(0, p.Data.Grid.Count);
                    tileAtWorldPosition = p.Data.Grid[index1];
                    if (current.MinMaxDistanceFromCenter != Vector2.zero)
                    {
                      for (int index2 = 0; index2 < 32 /*0x20*/; ++index2)
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
                  else
                    goto label_42;
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
      }
      foreach (PlacementRegion.TileGridTile t in p.Data.Grid)
      {
        float num = (float) LocationManager._Instance.Random.Next(0, 100);
        if (t.CanPlaceObstruction && (double) num <= 40.0 && p.PlaceWeeds)
          StructureManager.PlaceWeed(location, t, p, LocationManager._Instance.Random.Next(0, 3), LocationManager._Instance.Random.Next(0, 3));
      }
      foreach (PlacementRegion.TileGridTile tileGridTile in tileGridTileList)
        tileGridTile.Obstructed = false;
      p.Data.WeedsAndRubblePlaced = true;
    }
    StructureManager.CreateWeeds(location, PlacementRegions);
    DataManager.Instance.PlacedRubble = true;
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
        structuresFromRole.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType<Structures_FarmerStation>(FollowerLocation.Base));
        break;
      case FollowerRole.StoneMiner:
        structuresFromRole.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.RUBBLE));
        structuresFromRole.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.RUBBLE_BIG));
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
        structuresFromRole.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType<Structures_Kitchen>(FollowerLocation.Base));
        break;
      case FollowerRole.Janitor:
        structuresFromRole.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.JANITOR_STATION));
        break;
      case FollowerRole.Refiner:
        structuresFromRole.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.REFINERY));
        structuresFromRole.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.REFINERY_2));
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
    foreach (StructureBrain allBrain in StructureBrain.AllBrains)
    {
      if (allBrain.Data.ID == ID)
        return allBrain.Data.Type;
    }
    return StructureBrain.TYPES.NONE;
  }

  public static T GetStructureByID<T>(int ID) where T : StructureBrain
  {
    structureById = default (T);
    foreach (StructureBrain allBrain in StructureBrain.AllBrains)
    {
      if (allBrain.Data.ID == ID && allBrain is T structureById)
        break;
    }
    return structureById;
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

  public static List<StructureBrain> GetAllStructuresOfType(StructureBrain.TYPES type)
  {
    List<StructureBrain> structuresOfType = new List<StructureBrain>();
    foreach (StructureBrain allBrain in StructureBrain.AllBrains)
    {
      if (allBrain.Data.Type == type)
        structuresOfType.Add(allBrain);
    }
    return structuresOfType;
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

  public static int GetWasteCount()
  {
    int wasteCount = 0 + StructureManager.GetAllStructuresOfType(FollowerLocation.Base, StructureBrain.TYPES.POOP).Count + StructureManager.GetAllStructuresOfType(FollowerLocation.Base, StructureBrain.TYPES.VOMIT).Count;
    foreach (Structures_DeadWorshipper structuresDeadWorshipper in StructureManager.GetAllStructuresOfType<Structures_DeadWorshipper>(FollowerLocation.Base))
    {
      if (structuresDeadWorshipper.Data.Rotten && !structuresDeadWorshipper.Data.BodyWrapped)
        wasteCount += 4;
      else
        wasteCount += 2;
    }
    foreach (StructureBrain structureBrain in StructureManager.GetAllStructuresOfType<Structures_Outhouse>(FollowerLocation.Base))
    {
      if (structureBrain.IsFull)
        wasteCount += 3;
    }
    foreach (Structures_Meal structuresMeal in StructureManager.GetAllStructuresOfType<Structures_Meal>(FollowerLocation.Base))
    {
      if (structuresMeal.Data != null && (structuresMeal.Data.Rotten || structuresMeal.Data.Burned))
        ++wasteCount;
    }
    return wasteCount;
  }

  public static List<StructureBrain> GetAllStructuresOfType(
    FollowerLocation location,
    StructureBrain.TYPES type)
  {
    List<StructureBrain> structuresOfType = new List<StructureBrain>();
    foreach (StructureBrain structureBrain in StructureManager.StructuresAtLocation(location))
    {
      if (structureBrain.Data.Type == type)
        structuresOfType.Add(structureBrain);
    }
    return structuresOfType;
  }

  public static int GetTotalHomesCount(bool includeBuildSites = false, bool includeUpgradeSites = false)
  {
    int totalHomesCount = 0;
    foreach (StructureBrain allBrain in StructureBrain.AllBrains)
    {
      if (allBrain.Data.Type == StructureBrain.TYPES.BED && !allBrain.Data.IsCollapsed)
        ++totalHomesCount;
      if (allBrain.Data.Type == StructureBrain.TYPES.BED_2 && !allBrain.Data.IsCollapsed)
        ++totalHomesCount;
      if (allBrain.Data.Type == StructureBrain.TYPES.BED_3 && !allBrain.Data.IsCollapsed)
        ++totalHomesCount;
      if (includeBuildSites)
      {
        if ((allBrain.Data.Type == StructureBrain.TYPES.BUILD_SITE || allBrain.Data.Type == StructureBrain.TYPES.BUILDSITE_BUILDINGPROJECT) && (allBrain.Data.ToBuildType == StructureBrain.TYPES.SLEEPING_BAG || allBrain.Data.ToBuildType == StructureBrain.TYPES.BED || allBrain.Data.ToBuildType == StructureBrain.TYPES.BED_2 || allBrain.Data.ToBuildType == StructureBrain.TYPES.BED_3))
          ++totalHomesCount;
      }
      else if (includeUpgradeSites && (allBrain.Data.Type == StructureBrain.TYPES.BUILD_SITE || allBrain.Data.Type == StructureBrain.TYPES.BUILDSITE_BUILDINGPROJECT) && (allBrain.Data.ToBuildType == StructureBrain.TYPES.BED_2 || allBrain.Data.ToBuildType == StructureBrain.TYPES.BED_3))
        ++totalHomesCount;
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

  public static List<T> GetAllStructuresOfType<T>(FollowerLocation location) where T : StructureBrain
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

  public static List<Structures_FarmerPlot> GetAllUnwateredPlots(FollowerLocation location)
  {
    List<Structures_FarmerPlot> allUnwateredPlots = new List<Structures_FarmerPlot>();
    foreach (Structures_FarmerPlot structuresFarmerPlot in StructureManager.GetAllStructuresOfType<Structures_FarmerPlot>(location))
    {
      if (structuresFarmerPlot.CanWater() && !structuresFarmerPlot.ReservedForWatering)
        allUnwateredPlots.Add(structuresFarmerPlot);
    }
    return allUnwateredPlots;
  }

  public static List<Structures_FarmerPlot> GetAllUnseededPlots(FollowerLocation location)
  {
    List<Structures_FarmerPlot> allUnseededPlots = new List<Structures_FarmerPlot>();
    foreach (Structures_FarmerPlot structuresFarmerPlot in StructureManager.GetAllStructuresOfType<Structures_FarmerPlot>(location))
    {
      if (structuresFarmerPlot.CanPlantSeed() && !structuresFarmerPlot.ReservedForWatering)
        allUnseededPlots.Add(structuresFarmerPlot);
    }
    return allUnseededPlots;
  }

  public static List<Structures_FarmerPlot> GetAllUnfertilizedPlots(FollowerLocation location)
  {
    List<Structures_FarmerPlot> unfertilizedPlots = new List<Structures_FarmerPlot>();
    foreach (Structures_FarmerPlot structuresFarmerPlot in StructureManager.GetAllStructuresOfType<Structures_FarmerPlot>(location))
    {
      if (structuresFarmerPlot.CanFertilize() && !structuresFarmerPlot.ReservedForFertilizing)
        unfertilizedPlots.Add(structuresFarmerPlot);
    }
    return unfertilizedPlots;
  }

  public static List<Structures_BerryBush> GetAllUnpickedPlots(FollowerLocation location)
  {
    List<Structures_BerryBush> allUnpickedPlots = new List<Structures_BerryBush>();
    foreach (Structures_BerryBush structuresBerryBush in StructureManager.GetAllStructuresOfType<Structures_BerryBush>(location))
    {
      if (!structuresBerryBush.ReservedForTask && !structuresBerryBush.BerryPicked && !structuresBerryBush.Data.Destroyed)
        allUnpickedPlots.Add(structuresBerryBush);
    }
    return allUnpickedPlots;
  }

  public static List<Structures_Weeds> GetAllAvailableWeeds(FollowerLocation location)
  {
    List<Structures_Weeds> allAvailableWeeds = new List<Structures_Weeds>();
    foreach (Structures_Weeds structuresWeeds in StructureManager.GetAllStructuresOfType<Structures_Weeds>(location))
    {
      if (!structuresWeeds.ReservedForTask && !structuresWeeds.ReservedByPlayer)
        allAvailableWeeds.Add(structuresWeeds);
    }
    return allAvailableWeeds;
  }

  public static List<Structures_BerryBush> GetAllAvailableBushes(FollowerLocation location)
  {
    List<Structures_BerryBush> allAvailableBushes = new List<Structures_BerryBush>();
    foreach (Structures_BerryBush structuresBerryBush in StructureManager.GetAllStructuresOfType<Structures_BerryBush>(location))
    {
      if (!structuresBerryBush.ReservedForTask && !structuresBerryBush.ReservedByPlayer && !structuresBerryBush.BerryPicked)
        allAvailableBushes.Add(structuresBerryBush);
    }
    return allAvailableBushes;
  }

  public static List<Structures_Rubble> GetAllAvailableRubble(FollowerLocation location)
  {
    List<Structures_Rubble> allAvailableRubble = new List<Structures_Rubble>();
    foreach (Structures_Rubble structuresRubble in StructureManager.GetAllStructuresOfType<Structures_Rubble>(location))
    {
      if (!structuresRubble.ReservedForTask && !structuresRubble.ReservedByPlayer)
        allAvailableRubble.Add(structuresRubble);
    }
    return allAvailableRubble;
  }

  public static List<Structures_Tree> GetAllAvailableTrees(FollowerLocation location)
  {
    List<Structures_Tree> allAvailableTrees = new List<Structures_Tree>();
    foreach (Structures_Tree structuresTree in StructureManager.GetAllStructuresOfType<Structures_Tree>(location))
    {
      if (!structuresTree.ReservedForTask && !structuresTree.ReservedByPlayer && !structuresTree.TreeChopped)
        allAvailableTrees.Add(structuresTree);
    }
    return allAvailableTrees;
  }

  public static List<Structures_Waste> GetAllAvailableWaste(FollowerLocation location)
  {
    List<Structures_Waste> allAvailableWaste = new List<Structures_Waste>();
    foreach (Structures_Waste structuresWaste in StructureManager.GetAllStructuresOfType<Structures_Waste>(location))
    {
      if (!structuresWaste.ReservedForTask && !structuresWaste.ReservedByPlayer)
        allAvailableWaste.Add(structuresWaste);
    }
    return allAvailableWaste;
  }

  public static Structures_DeadWorshipper GetClosestUnburiedCorpse(
    FollowerLocation location,
    Vector3 position,
    int offset)
  {
    Structures_DeadWorshipper closestUnburiedCorpse = (Structures_DeadWorshipper) null;
    float maxValue = float.MaxValue;
    SortedList<float, Structures_DeadWorshipper> sortedList = new SortedList<float, Structures_DeadWorshipper>();
    foreach (Structures_DeadWorshipper structuresDeadWorshipper in StructureManager.GetAllStructuresOfType<Structures_DeadWorshipper>(location))
    {
      if (!structuresDeadWorshipper.ReservedForTask)
      {
        float key = Vector3.Distance(position, structuresDeadWorshipper.Data.Position);
        if ((double) key < (double) maxValue)
          sortedList.Add(key, structuresDeadWorshipper);
      }
    }
    if (sortedList.Count > offset)
      closestUnburiedCorpse = sortedList.Values[offset];
    return closestUnburiedCorpse;
  }

  public static Dwelling.DwellingAndSlot GetFreeDwellingAndSlot(
    FollowerLocation location,
    FollowerInfo follower)
  {
    foreach (Structures_Bed structuresBed in StructureManager.GetAllStructuresOfType<Structures_Bed>(location))
    {
      if (!structuresBed.ReservedForTask && !structuresBed.Data.Claimed && !structuresBed.Data.IsCollapsed)
      {
        bool flag = true;
        foreach (FollowerInfo follower1 in DataManager.Instance.Followers)
        {
          if (follower1.DwellingID == structuresBed.Data.ID || follower1.PreviousDwellingID != Dwelling.NO_HOME && follower1 != follower && follower1.PreviousDwellingID == structuresBed.Data.ID && follower1.DwellingID == Dwelling.NO_HOME)
          {
            flag = false;
            break;
          }
        }
        if (flag)
          return new Dwelling.DwellingAndSlot(structuresBed.Data.ID, 0, structuresBed.Level);
      }
    }
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

  private static bool ShouldStructureEmitVFXWhenAdded(StructureBrain.TYPES type)
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
        return false;
      default:
        return true;
    }
  }

  public static void Reset()
  {
    StructureManager.StructureBrains.Clear();
    foreach (StructureBrain brain in new List<StructureBrain>(StructureBrain.AllBrains))
      StructureBrain.RemoveBrain(brain);
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
      if (structuresBedList[index].IsCollapsed || structuresBedList[index].Data.Type == StructureBrain.TYPES.BED_3)
        structuresBedList.Remove(structuresBedList[index]);
    }
    for (int index = 0; index < structuresBedList.Count; ++index)
      structuresBedList[index].Collapse();
  }

  public static void ClearAllWaste()
  {
    List<StructureBrain> structuresOfType1 = StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.VOMIT);
    List<StructureBrain> structuresOfType2 = StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.POOP);
    for (int index = structuresOfType1.Count - 1; index >= 0; --index)
    {
      StructureBrain vomit = structuresOfType1[index];
      GameManager.GetInstance().StartCoroutine((IEnumerator) Delay(UnityEngine.Random.Range(0.0f, 1f), (System.Action) (() =>
      {
        BiomeConstants.Instance.EmitBloodSplatter(vomit.Data.Position, Vector3.back, Color.green);
        BiomeConstants.Instance.EmitBloodDieEffect(vomit.Data.Position, Vector3.back, Color.green);
        AudioManager.Instance.PlayOneShot("event:/followers/poop_pop", vomit.Data.Position);
        Vomit.SpawnLoot(vomit.Data.Position);
        vomit.Remove();
      })));
    }
    for (int index = structuresOfType2.Count - 1; index >= 0; --index)
    {
      StructureBrain poop = structuresOfType2[index];
      GameManager.GetInstance().StartCoroutine((IEnumerator) Delay(UnityEngine.Random.Range(0.0f, 1f), (System.Action) (() =>
      {
        BiomeConstants.Instance.EmitSmokeExplosionVFX(poop.Data.Position);
        AudioManager.Instance.PlayOneShot("event:/followers/poop_pop", poop.Data.Position);
        poop.Remove();
      })));
    }

    static IEnumerator Delay(float delay, System.Action callback)
    {
      yield return (object) new WaitForSeconds(delay);
      System.Action action = callback;
      if (action != null)
        action();
    }
  }

  public delegate void StructuresPlaced();

  public delegate void StructureChanged(StructuresData structure);
}
