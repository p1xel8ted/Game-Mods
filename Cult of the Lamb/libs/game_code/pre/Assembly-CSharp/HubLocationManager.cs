// Decompiled with JetBrains decompiler
// Type: HubLocationManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[DefaultExecutionOrder(-50)]
public class HubLocationManager : LocationManager
{
  [SerializeField]
  private FollowerLocation _location;
  [SerializeField]
  private Transform _unitLayer;
  [SerializeField]
  private Transform _structureLayer;
  public bool AllowStructures = true;
  public Transform EntranceFromBase;
  public List<HubLocationManager.LocationAndEntrance> LocationAndEntrances = new List<HubLocationManager.LocationAndEntrance>();

  public override FollowerLocation Location => this._location;

  public override Transform UnitLayer => this._unitLayer;

  public override bool SupportsStructures => this.AllowStructures;

  public override Transform StructureLayer => this._structureLayer;

  protected override Vector3 GetStartPosition(FollowerLocation prevLocation)
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

  protected override void PostPlaceStructures()
  {
    Debug.Log((object) ("PostPlaceStructures() " + (object) this.Location));
    StructureManager.PlaceRubble(this.Location);
  }

  [Serializable]
  public class LocationAndEntrance
  {
    public FollowerLocation Location;
    public GameObject Entrance;
  }
}
