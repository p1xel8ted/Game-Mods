// Decompiled with JetBrains decompiler
// Type: FollowerTask_FindPlaceToDie
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FollowerTask_FindPlaceToDie : FollowerTask
{
  private NotificationCentre.NotificationType _deathNotificationType;
  private PlacementRegion.TileGridTile ClosestTile;

  public override FollowerTaskType Type => FollowerTaskType.FindPlaceToDie;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool DisablePickUpInteraction => true;

  public override bool BlockTaskChanges => true;

  public FollowerTask_FindPlaceToDie(
    NotificationCentre.NotificationType deathNotificationType)
  {
    this._deathNotificationType = deathNotificationType;
  }

  protected override int GetSubTaskCode() => 0;

  public override void ClaimReservations()
  {
    if (this.ClosestTile == null)
      return;
    this.ClosestTile.ReservedForWaste = true;
  }

  public override void ReleaseReservations()
  {
    if (this.ClosestTile == null)
      return;
    this.ClosestTile.ReservedForWaste = false;
  }

  protected override void OnStart() => this.SetState(FollowerTaskState.GoingTo);

  protected override void OnArrive() => this.End();

  protected override void TaskTick(float deltaGameTime)
  {
  }

  protected override Vector3 UpdateDestination(Follower follower)
  {
    Vector3 vector3 = Vector3.zero;
    List<StructureBrain> structuresFromRole = StructureManager.GetStructuresFromRole(FollowerRole.Worshipper);
    if (structuresFromRole.Count > 0)
    {
      vector3 = (structuresFromRole[0] as Structures_Shrine).Data.Position + (Vector3) Random.insideUnitCircle.normalized * 3f;
    }
    else
    {
      this.ClosestTile = StructureManager.GetBestWasteTile(this._brain.Location);
      if (this.ClosestTile != null)
      {
        this.ClosestTile.ReservedForWaste = true;
        vector3 = this.ClosestTile.WorldPosition;
      }
    }
    return vector3;
  }

  public override void OnFinaliseBegin(Follower follower)
  {
    follower.DieWithAnimation("tantrum-hungry", 3.2f, deathNotificationType: this._deathNotificationType);
  }

  public override void SimFinaliseEnd(SimFollower simFollower)
  {
    this.GetDestination((Follower) null);
    simFollower.Die(this._deathNotificationType, this._currentDestination.Value);
  }
}
