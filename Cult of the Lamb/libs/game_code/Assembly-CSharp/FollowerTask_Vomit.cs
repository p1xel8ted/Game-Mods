// Decompiled with JetBrains decompiler
// Type: FollowerTask_Vomit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine;
using UnityEngine;

#nullable disable
public class FollowerTask_Vomit : FollowerTask
{
  public PlacementRegion.TileGridTile ClosestTile;

  public override FollowerTaskType Type => FollowerTaskType.Vomit;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockReactTasks => true;

  public override bool BlockTaskChanges => true;

  public override int GetSubTaskCode() => 0;

  public override void ClaimReservations()
  {
    if (this.ClosestTile == null)
      return;
    this.ClosestTile.ReservedForWaste = true;
  }

  public override void OnStart() => this.SetState(FollowerTaskState.GoingTo);

  public override void TaskTick(float deltaGameTime)
  {
    if (this.State == FollowerTaskState.Doing || this.State == FollowerTaskState.GoingTo)
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
    infoByType.VariantIndex = this._brain.Info.HasTrait(FollowerTrait.TraitType.Mutated) ? 1 : 0;
    PlacementRegion.TileGridTile tileGridTile = (PlacementRegion.TileGridTile) null;
    if ((bool) (UnityEngine.Object) PlacementRegion.Instance)
      tileGridTile = PlacementRegion.Instance.GetFreeClosestTileGridTileAtWorldPosition(this._brain.LastPosition);
    if (tileGridTile != null)
      StructureManager.BuildStructure(this._brain.Location, infoByType, tileGridTile.WorldPosition, Vector2Int.one, false);
    else
      StructureManager.BuildStructure(this._brain.Location, infoByType, this._brain.LastPosition, Vector2Int.one, false);
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    if ((bool) (UnityEngine.Object) PlacementRegion.Instance)
      this.ClosestTile = PlacementRegion.Instance.GetFreeClosestTileGridTileAtWorldPosition(this._brain.LastPosition);
    if (this.ClosestTile != null)
      return (UnityEngine.Object) follower == (UnityEngine.Object) null ? this._brain.LastPosition : follower.transform.position;
    this.ClosestTile = StructureManager.GetBestWasteTile(this._brain.Location);
    return this.ClosestTile != null ? this.ClosestTile.WorldPosition : TownCentre.Instance.RandomPositionInTownCentre();
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    follower.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
  }

  public override void OnGoingToBegin(Follower follower) => base.OnGoingToBegin(follower);

  public override void OnDoingBegin(Follower follower)
  {
    float timer = 3.5f;
    bool flag1 = false;
    bool flag2 = false;
    string animation = "Sick/chunder";
    if (follower.Brain.Info.IsDrunk)
    {
      animation = "Sick/chunder-drunk";
      flag1 = (double) UnityEngine.Random.value < 0.20000000298023224;
      if (flag1)
      {
        flag2 = (double) UnityEngine.Random.value < 0.30000001192092896;
        animation = "Sick/chunder-drunk-collapse";
        timer = (float) (6.820000171661377 + (flag2 ? 3.0 : 0.0));
      }
    }
    follower.TimedAnimation(animation, timer, new System.Action(((FollowerTask) this).End));
    if (!flag1)
      return;
    if (flag2)
      follower.AddBodyAnimation("Sick/chunder-drunk-passout", false, 0.0f);
    follower.AddBodyAnimation("Sick/chunder-drunk-wake", false, 0.0f);
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    follower.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
  }

  public void HandleAnimationStateEvent(TrackEntry trackEntry, Spine.Event e)
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
