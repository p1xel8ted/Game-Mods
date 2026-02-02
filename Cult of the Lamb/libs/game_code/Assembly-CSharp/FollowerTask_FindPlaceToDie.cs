// Decompiled with JetBrains decompiler
// Type: FollowerTask_FindPlaceToDie
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FollowerTask_FindPlaceToDie : FollowerTask
{
  public NotificationCentre.NotificationType _deathNotificationType;
  public PlacementRegion.TileGridTile ClosestTile;

  public override FollowerTaskType Type => FollowerTaskType.FindPlaceToDie;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool DisablePickUpInteraction => true;

  public override bool BlockTaskChanges => true;

  public FollowerTask_FindPlaceToDie(
    NotificationCentre.NotificationType deathNotificationType)
  {
    this._deathNotificationType = deathNotificationType;
  }

  public override int GetSubTaskCode() => 0;

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

  public override void OnStart() => this.SetState(FollowerTaskState.GoingTo);

  public override void OnArrive() => this.End();

  public override void TaskTick(float deltaGameTime)
  {
    if (this._deathNotificationType == NotificationCentre.NotificationType.DiedFromRot && PlayerFarming.Location != FollowerLocation.Base)
    {
      this.Abort();
    }
    else
    {
      if (this._deathNotificationType != NotificationCentre.NotificationType.FrozeToDeath || SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter)
        return;
      this._brain.FrozeToDeath = false;
      this._brain._directInfoAccess.CursedState = Thought.None;
      this._brain._directInfoAccess.Freezing = 0.0f;
      this.Abort();
    }
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    Vector3 zero = Vector3.zero;
    List<StructureBrain> structuresFromRole = StructureManager.GetStructuresFromRole(FollowerRole.Worshipper);
    if (structuresFromRole.Count > 0 && (Object) follower != (Object) null && !follower.Brain.DiedFromRot)
      return (structuresFromRole[0] as Structures_Shrine).Data.Position + (Vector3) Random.insideUnitCircle.normalized * 3f;
    Vector2 direction = new Vector2(Random.Range(-1f, 1f), Random.Range(0.0f, -1f));
    LayerMask layerMask = (LayerMask) ((int) new LayerMask() | 1 << LayerMask.NameToLayer("Island"));
    return (Vector3) (Physics2D.Raycast((Vector2) Vector3.zero, direction, 1000f, (int) layerMask).point + -direction * Random.Range(3f, 8f));
  }

  public override void OnFinaliseBegin(Follower follower)
  {
    if (this._deathNotificationType == NotificationCentre.NotificationType.DiedFromOldAge && (double) Random.value >= 0.25)
    {
      string str = "Die/die-alt-" + Random.Range(1, 4).ToString();
    }
    if (this.Brain.FrozeToDeath)
      follower.DieWithAnimation("Freezing/die", "Freezing/dead", deathNotificationType: this._deathNotificationType);
    else if (this.Brain.DiedFromOverheating)
      follower.DieWithAnimation("Overheated/die", "Overheated/dead", deathNotificationType: this._deathNotificationType);
    else if (this.Brain.DiedFromRot)
      follower.DieWithAnimation("die-rot", deathNotificationType: this._deathNotificationType);
    else
      follower.DieWithAnimation("die", deathNotificationType: this._deathNotificationType);
  }

  public override void SimFinaliseEnd(SimFollower simFollower)
  {
    this.GetDestination((Follower) null);
    simFollower.Die(this._deathNotificationType, this._currentDestination.Value);
  }
}
