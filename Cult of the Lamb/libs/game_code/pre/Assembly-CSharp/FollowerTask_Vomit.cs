// Decompiled with JetBrains decompiler
// Type: FollowerTask_Vomit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine;
using UnityEngine;

#nullable disable
public class FollowerTask_Vomit : FollowerTask
{
  private PlacementRegion.TileGridTile ClosestTile;

  public override FollowerTaskType Type => FollowerTaskType.Vomit;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockReactTasks => true;

  public override bool BlockTaskChanges => true;

  protected override int GetSubTaskCode() => 0;

  public override void ClaimReservations()
  {
    if (this.ClosestTile == null)
      return;
    this.ClosestTile.ReservedForWaste = true;
  }

  public override void ReleaseReservations()
  {
  }

  protected override void OnStart() => this.SetState(FollowerTaskState.GoingTo);

  protected override void TaskTick(float deltaGameTime)
  {
    if (this.State == FollowerTaskState.Doing)
      return;
    this.SetState(FollowerTaskState.Doing);
  }

  public override void ProgressTask()
  {
    if (this._brain == null)
      return;
    this._brain.Stats.LastVomit = TimeManager.TotalElapsedGameTime;
    StructuresData infoByType = StructuresData.GetInfoByType(StructureBrain.TYPES.VOMIT, 0);
    infoByType.FollowerID = this._brain.Info.ID;
    PlacementRegion.TileGridTile tileGridTile = (PlacementRegion.TileGridTile) null;
    if ((bool) (UnityEngine.Object) PlacementRegion.Instance)
      tileGridTile = StructureManager.GetClosestTileGridTileAtWorldPosition(this._brain.LastPosition, PlacementRegion.Instance.StructureInfo.Grid, 1f);
    if (tileGridTile != null)
    {
      infoByType.GridTilePosition = tileGridTile.Position;
      StructureManager.BuildStructure(this._brain.Location, infoByType, tileGridTile.WorldPosition, Vector2Int.one, false);
    }
    else
      StructureManager.BuildStructure(this._brain.Location, infoByType, this._brain.LastPosition, Vector2Int.one, false);
  }

  protected override Vector3 UpdateDestination(Follower follower)
  {
    return (UnityEngine.Object) follower == (UnityEngine.Object) null ? this._brain.LastPosition : follower.transform.position;
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    follower.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
  }

  public override void OnGoingToBegin(Follower follower) => base.OnGoingToBegin(follower);

  public override void OnDoingBegin(Follower follower)
  {
    follower.TimedAnimation("Sick/chunder", 3.5f, new System.Action(((FollowerTask) this).End));
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    follower.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
  }

  private void HandleAnimationStateEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (!(e.Data.Name == "Vomit"))
      return;
    this.ProgressTask();
  }

  public override void SimDoingBegin(SimFollower simFollower)
  {
    base.SimDoingBegin(simFollower);
    this.GetDestination((Follower) null);
    this.End();
  }
}
