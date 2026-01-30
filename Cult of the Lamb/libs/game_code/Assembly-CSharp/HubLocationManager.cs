// Decompiled with JetBrains decompiler
// Type: HubLocationManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[DefaultExecutionOrder(-50)]
public class HubLocationManager : LocationManager
{
  [SerializeField]
  public FollowerLocation _location;
  [SerializeField]
  public Transform _unitLayer;
  [SerializeField]
  public Transform _structureLayer;
  public bool AllowStructures = true;
  public Transform EntranceFromBase;
  public List<HubLocationManager.LocationAndEntrance> LocationAndEntrances = new List<HubLocationManager.LocationAndEntrance>();

  public override FollowerLocation Location => this._location;

  public override Transform UnitLayer => this._unitLayer;

  public override bool SupportsStructures => this.AllowStructures;

  public override Transform StructureLayer => this._structureLayer;

  public override Vector3 GetStartPosition(FollowerLocation prevLocation)
  {
    foreach (HubLocationManager.LocationAndEntrance locationAndEntrance in this.LocationAndEntrances)
    {
      if (prevLocation == locationAndEntrance.Location)
        return locationAndEntrance.Entrance.transform.position;
    }
    return prevLocation == FollowerLocation.None || prevLocation == FollowerLocation.Base || prevLocation == FollowerLocation.HubShore ? this.EntranceFromBase.position : (!((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null) ? this.EntranceFromBase.position : PlayerFarming.Instance.transform.position);
  }

  public override Vector3 GetExitPosition(FollowerLocation destLocation)
  {
    foreach (HubLocationManager.LocationAndEntrance locationAndEntrance in this.LocationAndEntrances)
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

  public override void PostPlaceStructures()
  {
    Debug.Log((object) ("PostPlaceStructures() " + this.Location.ToString()));
    if (this.Location == FollowerLocation.DLC_ShrineRoom)
      StructureManager.PlaceWeed(this.Location);
    else
      StructureManager.PlaceRubble(this.Location);
  }

  [Serializable]
  public class LocationAndEntrance
  {
    public FollowerLocation Location;
    public GameObject Entrance;
  }
}
