// Decompiled with JetBrains decompiler
// Type: RewardRoomLocationManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Map;
using MMBiomeGeneration;
using MMRoomGeneration;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RewardRoomLocationManager : LocationManager
{
  [SerializeField]
  public FollowerLocation _location;
  [SerializeField]
  public Transform _unitLayer;
  [SerializeField]
  public Transform _structureLayer;
  public bool AllowStructures = true;
  public Transform EntranceFromBase;
  public List<RewardRoomLocationManager.LocationAndEntrance> LocationAndEntrances = new List<RewardRoomLocationManager.LocationAndEntrance>();
  [Space]
  [SerializeField]
  public RewardRoomLocationManager.ExtraSpawnable[] extraSpawnables = new RewardRoomLocationManager.ExtraSpawnable[0];
  public PlacementRegion placementRegion;
  public static List<global::StructuresData> createdStructures = new List<global::StructuresData>();
  public GenerateRoom room;
  public bool triggered;

  public override FollowerLocation Location => this._location;

  public override Transform UnitLayer => this._unitLayer;

  public override bool SupportsStructures => this.AllowStructures;

  public override Transform StructureLayer => this._structureLayer;

  public override void Awake()
  {
    base.Awake();
    foreach (RewardRoomLocationManager.ExtraSpawnable extraSpawnable in this.extraSpawnables)
    {
      for (int index1 = 0; index1 < extraSpawnable.ResourcesToPlace.Count; ++index1)
      {
        int num = 0;
        for (int index2 = 0; index2 < extraSpawnable.ResourcesToPlace[index1].Count; ++index2)
        {
          if ((double) UnityEngine.Random.Range(0.0f, 1f) <= (double) extraSpawnable.Probability)
            ++num;
        }
        if (num > 0)
        {
          extraSpawnable.ResourcesToPlace[index1].Count = num;
          this.placementRegion?.ResourcesToPlace.Add(extraSpawnable.ResourcesToPlace[index1]);
        }
      }
    }
    StructureManager.OnStructuresPlaced += new StructureManager.StructuresPlaced(this.OnStructuresPlaced);
  }

  public void OnStructuresPlaced()
  {
    if (!this.triggered)
      GameManager.GetInstance().StartCoroutine((IEnumerator) this.FrameWait());
    this.triggered = true;
  }

  public IEnumerator FrameWait()
  {
    RewardRoomLocationManager roomLocationManager = this;
    while ((UnityEngine.Object) BiomeGenerator.Instance.CurrentRoom.GameObject == (UnityEngine.Object) null)
      yield return (object) null;
    while ((UnityEngine.Object) roomLocationManager.placementRegion == (UnityEngine.Object) null && (UnityEngine.Object) roomLocationManager != (UnityEngine.Object) null)
    {
      roomLocationManager.placementRegion = roomLocationManager.GetComponentInParent<PlacementRegion>();
      yield return (object) null;
    }
    if (!((UnityEngine.Object) roomLocationManager == (UnityEngine.Object) null))
    {
      roomLocationManager.room = roomLocationManager.GetComponentInParent<GenerateRoom>();
      StructureManager.PlaceRubble(roomLocationManager.Location, new List<Structures_PlacementRegion>()
      {
        roomLocationManager.placementRegion.structure.Brain as Structures_PlacementRegion
      });
      MapManager.Instance.OnMapShown += new System.Action(roomLocationManager.BiomeGenerator_OnBiomeLeftRoom);
      RewardRoomLocationManager.createdStructures = roomLocationManager.StructuresData;
    }
  }

  public void BiomeGenerator_OnBiomeLeftRoom()
  {
    this.triggered = false;
    MapManager.Instance.OnMapShown -= new System.Action(this.BiomeGenerator_OnBiomeLeftRoom);
    for (int index = RewardRoomLocationManager.createdStructures.Count - 1; index >= 0; --index)
    {
      StructureBrain brain = StructureBrain.GetOrCreateBrain(RewardRoomLocationManager.createdStructures[index]);
      brain.ForceRemoved = true;
      brain.Remove();
    }
  }

  public override Vector3 GetStartPosition(FollowerLocation prevLocation)
  {
    Vector3 startPosition;
    if (prevLocation == this.Location)
    {
      startPosition = this.EntranceFromBase.position;
    }
    else
    {
      foreach (RewardRoomLocationManager.LocationAndEntrance locationAndEntrance in this.LocationAndEntrances)
      {
        if (prevLocation == locationAndEntrance.Location)
          return locationAndEntrance.Entrance.transform.position;
      }
      startPosition = prevLocation == FollowerLocation.None || prevLocation == FollowerLocation.Base || prevLocation == FollowerLocation.HubShore ? this.EntranceFromBase.position : (!((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null) ? this.EntranceFromBase.position : PlayerFarming.Instance.transform.position);
    }
    return startPosition;
  }

  public override Vector3 GetExitPosition(FollowerLocation destLocation)
  {
    foreach (RewardRoomLocationManager.LocationAndEntrance locationAndEntrance in this.LocationAndEntrances)
    {
      if (destLocation == locationAndEntrance.Location)
        return locationAndEntrance.Entrance.transform.position;
    }
    Vector3 exitPosition;
    switch (destLocation)
    {
      case FollowerLocation.Church:
      case FollowerLocation.Base:
        exitPosition = this.EntranceFromBase.position;
        break;
      default:
        exitPosition = base.GetExitPosition(destLocation);
        break;
    }
    return exitPosition;
  }

  [Serializable]
  public struct ExtraSpawnable
  {
    public List<PlacementRegion.ResourcesAndCount> ResourcesToPlace;
    public float Probability;
  }

  [Serializable]
  public class LocationAndEntrance
  {
    public FollowerLocation Location;
    public GameObject Entrance;
  }
}
