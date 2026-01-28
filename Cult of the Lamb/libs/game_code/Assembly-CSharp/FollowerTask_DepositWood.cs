// Decompiled with JetBrains decompiler
// Type: FollowerTask_DepositWood
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FollowerTask_DepositWood : FollowerTask
{
  public const float DEPOSIT_DURATION_GAME_MINUTES = 5f;
  public int _lumberjackStationID;
  public Structures_LumberjackStation _lumberjackStation;
  public float _progress;

  public override FollowerTaskType Type => FollowerTaskType.DepositWood;

  public override FollowerLocation Location => this._lumberjackStation.Data.Location;

  public override int UsingStructureID => this._lumberjackStationID;

  public FollowerTask_DepositWood(int lumberjackStationID)
  {
    this._lumberjackStationID = lumberjackStationID;
    this._lumberjackStation = StructureManager.GetStructureByID<Structures_LumberjackStation>(this._lumberjackStationID);
  }

  public override int GetSubTaskCode() => this._lumberjackStationID;

  public override void OnStart() => this.SetState(FollowerTaskState.GoingTo);

  public override void OnEnd()
  {
    Structures_LumberjackStation structureById = StructureManager.GetStructureByID<Structures_LumberjackStation>(this._lumberjackStationID);
    for (int index = 0; index < this._brain.Stats.CachedLumber; ++index)
      structureById.Data.Inventory.Add(new InventoryItem(InventoryItem.ITEM_TYPE.LOG));
    this._brain.Stats.CachedLumber = 0;
    this._brain.Stats.CachedLumberjackStationID = 0;
    base.OnEnd();
  }

  public override void TaskTick(float deltaGameTime)
  {
    if (this.State != FollowerTaskState.Doing || (double) (this._progress += deltaGameTime) < 5.0)
      return;
    this.End();
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    LumberjackStation lumberjackStation = this.FindLumberjackStation();
    return !((Object) lumberjackStation == (Object) null) ? lumberjackStation.FollowerPosition.transform.position : follower.transform.position;
  }

  public override void OnDoingBegin(Follower follower)
  {
    follower.State.CURRENT_STATE = StateMachine.State.CustomAction0;
  }

  public LumberjackStation FindLumberjackStation()
  {
    LumberjackStation lumberjackStation1 = (LumberjackStation) null;
    foreach (LumberjackStation lumberjackStation2 in LumberjackStation.LumberjackStations)
    {
      if (lumberjackStation2.StructureInfo.ID == this._lumberjackStationID)
      {
        lumberjackStation1 = lumberjackStation2;
        break;
      }
    }
    return lumberjackStation1;
  }
}
