// Decompiled with JetBrains decompiler
// Type: FollowerTask_InstantPoop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Spine;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FollowerTask_InstantPoop : FollowerTask
{
  private PlacementRegion.TileGridTile ClosestWasteTile;

  public override FollowerTaskType Type => FollowerTaskType.Bathroom;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockTaskChanges => true;

  public override bool BlockReactTasks => true;

  protected override int GetSubTaskCode() => this.UsingStructureID;

  public override void ClaimReservations()
  {
    if (this.ClosestWasteTile == null)
      return;
    this.ClosestWasteTile.ReservedForWaste = true;
  }

  public override void ReleaseReservations()
  {
    if (this.ClosestWasteTile == null)
      return;
    this.ClosestWasteTile.ReservedForWaste = false;
  }

  protected override void OnStart() => this.SetState(FollowerTaskState.GoingTo);

  public override void ProgressTask()
  {
    StructuresData infoByType = StructuresData.GetInfoByType(StructureBrain.TYPES.POOP, 0);
    infoByType.FollowerID = this._brain.Info.ID;
    this._brain.AddThought(Thought.BathroomOutside);
    this.ClosestWasteTile = StructureManager.GetCloseTile(this._brain.LastPosition, FollowerLocation.Base);
    if (this.ClosestWasteTile != null)
    {
      infoByType.GridTilePosition = this.ClosestWasteTile.Position;
      StructureManager.BuildStructure(this._brain.Location, infoByType, this.ClosestWasteTile.WorldPosition, Vector2Int.one, false, new Action<GameObject>(this.LerpPoop));
    }
    else
      StructureManager.BuildStructure(this._brain.Location, infoByType, this._brain.LastPosition, Vector2Int.one, false, new Action<GameObject>(this.LerpPoop));
    this._brain.Stats.TargetBathroom = 0.0f;
  }

  private void LerpPoop(GameObject poop)
  {
    Follower followerById = FollowerManager.FindFollowerByID(this._brain.Info.ID);
    if ((UnityEngine.Object) followerById == (UnityEngine.Object) null)
      return;
    Vector3 position = poop.transform.position;
    poop.transform.position = followerById.transform.position;
    poop.transform.localScale = Vector3.zero;
    poop.transform.DOMove(position, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
    poop.transform.DOScale(1f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
  }

  protected override Vector3 UpdateDestination(Follower follower)
  {
    return !((UnityEngine.Object) follower == (UnityEngine.Object) null) ? follower.transform.position : this._brain.LastPosition;
  }

  private List<PlacementRegion.TileGridTile> GetAvailableFollowerPositions(
    PlacementRegion.TileGridTile poopPosition)
  {
    List<PlacementRegion.TileGridTile> followerPositions = new List<PlacementRegion.TileGridTile>();
    PlacementRegion.TileGridTile closeTile1 = StructureManager.GetCloseTile(poopPosition.WorldPosition + Vector3.up * 0.5f, FollowerLocation.Base);
    if (closeTile1 != null)
      followerPositions.Add(closeTile1);
    PlacementRegion.TileGridTile closeTile2 = StructureManager.GetCloseTile(poopPosition.WorldPosition - Vector3.up * 0.5f, FollowerLocation.Base);
    if (closeTile2 != null)
      followerPositions.Add(closeTile2);
    PlacementRegion.TileGridTile closeTile3 = StructureManager.GetCloseTile(poopPosition.WorldPosition + Vector3.right * 0.5f, FollowerLocation.Base);
    if (closeTile3 != null)
      followerPositions.Add(closeTile3);
    PlacementRegion.TileGridTile closeTile4 = StructureManager.GetCloseTile(poopPosition.WorldPosition - Vector3.right * 0.5f, FollowerLocation.Base);
    if (closeTile4 != null)
      followerPositions.Add(closeTile4);
    return followerPositions;
  }

  public override void OnDoingBegin(Follower follower)
  {
    if (!((UnityEngine.Object) follower != (UnityEngine.Object) null))
      return;
    this.GetDestination(follower);
    follower.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
    follower.TimedAnimation("poop", 1.5333333f, (System.Action) (() =>
    {
      follower.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
      follower.FacePosition(this._currentDestination.Value);
      if (this._brain.HasTrait(FollowerTrait.TraitType.Coprophiliac))
        follower.TimedAnimation("Reactions/react-laugh", 3.33333325f, new System.Action(((FollowerTask) this).End), false);
      else
        follower.TimedAnimation("Reactions/react-embarrassed", 3f, new System.Action(((FollowerTask) this).End), false);
    }));
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    follower.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "run");
  }

  private void HandleAnimationStateEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (!(e.Data.Name == "Poop"))
      return;
    this.ProgressTask();
  }

  public override void SimDoingBegin(SimFollower simFollower)
  {
    base.SimDoingBegin(simFollower);
    this.GetDestination((Follower) null);
    this.End();
  }

  protected override void OnEnd() => base.OnEnd();

  protected override void TaskTick(float deltaGameTime)
  {
    if (this.State == FollowerTaskState.Doing)
      return;
    this.SetState(FollowerTaskState.Doing);
  }
}
