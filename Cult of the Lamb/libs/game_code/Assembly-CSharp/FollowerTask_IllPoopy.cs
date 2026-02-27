// Decompiled with JetBrains decompiler
// Type: FollowerTask_IllPoopy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Spine;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FollowerTask_IllPoopy : FollowerTask
{
  public float _gameTimeToNextStateUpdate;
  public Follower follower;
  public bool GoToVomit;
  public bool ForceFirstVomit = true;
  public PlacementRegion.TileGridTile ClosestWasteTile;

  public override FollowerTaskType Type => FollowerTaskType.IllPoopy;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockReactTasks => true;

  public override bool BlockSermon => true;

  public FollowerTask_IllPoopy(bool forceFirstVomit = false)
  {
    this.ForceFirstVomit = forceFirstVomit;
  }

  public override int GetSubTaskCode() => 0;

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

  public override void OnArrive()
  {
    if (this.GoToVomit)
      this.SetState(FollowerTaskState.Doing);
    else
      this.SetState(FollowerTaskState.Idle);
  }

  public override void TaskTick(float deltaGameTime)
  {
    if ((double) this._brain.Stats.Illness <= 0.0)
    {
      Debug.Log((object) "CCC");
      this.End();
    }
    else
    {
      if (this.ForceFirstVomit && (double) IllnessBar.IllnessNormalized > 0.05000000074505806)
      {
        this.ForceFirstVomit = false;
        this.GoToVomit = true;
        this.ClearDestination();
        this.SetState(FollowerTaskState.GoingTo);
        if ((bool) (UnityEngine.Object) this.follower)
        {
          this.follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "Sick/run-poopy");
          ((FollowerState_Ill) this.follower.Brain.CurrentState).SpeedMultiplier = 5f;
        }
      }
      if (this._state != FollowerTaskState.Idle)
        return;
      this._gameTimeToNextStateUpdate -= deltaGameTime;
      if ((double) this._gameTimeToNextStateUpdate <= 0.0)
      {
        this.GoToVomit = false;
        this.ClearDestination();
        this.SetState(FollowerTaskState.GoingTo);
        this._gameTimeToNextStateUpdate = UnityEngine.Random.Range(4f, 6f);
        if (!(bool) (UnityEngine.Object) this.follower)
          return;
        this.follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "Sick/walk-sick");
        ((FollowerState_Ill) this.follower.Brain.CurrentState).SpeedMultiplier = 1f;
      }
      else
      {
        if ((double) TimeManager.TotalElapsedGameTime - (double) this._brain.Stats.LastVomit <= 360.0 && (double) UnityEngine.Random.Range(0.0f, 1f) > 1.0 / 500.0)
          return;
        if ((double) IllnessBar.IllnessNormalized > 0.05000000074505806)
        {
          this.GoToVomit = true;
          this.ClearDestination();
          this.SetState(FollowerTaskState.GoingTo);
          if (!(bool) (UnityEngine.Object) this.follower)
            return;
          this.follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "Sick/run-poopy");
          ((FollowerState_Ill) this.follower.Brain.CurrentState).SpeedMultiplier = 5f;
        }
        else
        {
          this.GoToVomit = false;
          this.ClearDestination();
          this.SetState(FollowerTaskState.GoingTo);
          this._gameTimeToNextStateUpdate = UnityEngine.Random.Range(4f, 6f);
          if (!(bool) (UnityEngine.Object) this.follower)
            return;
          this.follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "Sick/walk-sick");
          ((FollowerState_Ill) this.follower.Brain.CurrentState).SpeedMultiplier = 1f;
        }
      }
    }
  }

  public override void ProgressTask()
  {
    StructureBrain.TYPES Type = StructureBrain.TYPES.POOP;
    if (this._brain.Info.HasTrait(FollowerTrait.TraitType.RoyalPooper))
      Type = StructureBrain.TYPES.POOP_GOLD;
    else if (this._brain.Info.HasTrait(FollowerTrait.TraitType.RotstonePooper) || this._brain.Info.HasTrait(FollowerTrait.TraitType.Mutated))
      Type = StructureBrain.TYPES.POOP_ROTSTONE;
    StructuresData infoByType = StructuresData.GetInfoByType(Type, 0);
    infoByType.FollowerID = this._brain.Info.ID;
    this._brain.Stats.LastVomit = TimeManager.TotalElapsedGameTime;
    if (this.ClosestWasteTile != null)
    {
      infoByType.GridTilePosition = this.ClosestWasteTile.Position;
      StructureManager.BuildStructure(this._brain.Location, infoByType, this.ClosestWasteTile.WorldPosition, Vector2Int.one, false, new Action<GameObject>(this.LerpPoop));
    }
    else
      StructureManager.BuildStructure(this._brain.Location, infoByType, this._brain.LastPosition, Vector2Int.one, false, new Action<GameObject>(this.LerpPoop));
  }

  public void LerpPoop(GameObject poop)
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

  public override Vector3 UpdateDestination(Follower follower)
  {
    return this.GoToVomit ? this.GetPoopPosition() : TownCentre.RandomCircleFromTownCentre(10f);
  }

  public override float RestChange(float deltaGameTime) => 100f;

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    follower.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Sick/idle-sick");
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "Sick/walk-sick");
    this.follower = follower;
  }

  public override void OnDoingBegin(Follower follower)
  {
    this.GoToVomit = false;
    follower.TimedAnimation("poop", 1.5333333f, (System.Action) (() =>
    {
      follower.FacePosition(this._brain.LastPosition);
      if (follower.Brain.HasTrait(FollowerTrait.TraitType.Coprophiliac))
        follower.TimedAnimation("Reactions/react-laugh", 3.33333325f, new System.Action(this.ContinueAfterReaction), false);
      else
        follower.TimedAnimation("Reactions/react-embarrassed", 3f, new System.Action(this.ContinueAfterReaction), false);
    }));
  }

  public void ContinueAfterReaction()
  {
    this.follower.FacePosition(this._brain.LastPosition);
    this.SetState(FollowerTaskState.Idle);
    this.follower.Brain.Stats.Bathroom = 0.0f;
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    follower.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "idle");
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "run");
    if (follower.Brain.CurrentState.Type != FollowerStateType.Ill)
      return;
    ((FollowerState_Ill) follower.Brain.CurrentState).SpeedMultiplier = 1f;
  }

  public void HandleAnimationStateEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (!(e.Data.Name == "Poop"))
      return;
    this.ProgressTask();
  }

  public override float VomitChange(float deltaGameTime) => 0.0f;

  public override void SimSetup(SimFollower simFollower)
  {
    base.SimSetup(simFollower);
    StructureManager.OnStructureAdded += new StructureManager.StructureChanged(this.OnStructureAdded);
  }

  public override void SimCleanup(SimFollower simFollower)
  {
    base.SimCleanup(simFollower);
    StructureManager.OnStructureAdded -= new StructureManager.StructureChanged(this.OnStructureAdded);
  }

  public void OnStructureAdded(StructuresData structure)
  {
    if (structure.Type != StructureBrain.TYPES.POOP || structure.FollowerID != this._brain.Info.ID)
      return;
    this.SetState(FollowerTaskState.Idle);
  }

  public override void SimDoingBegin(SimFollower simFollower)
  {
    base.SimDoingBegin(simFollower);
    this.GetDestination((Follower) null);
    this.ProgressTask();
  }

  public Vector3 GetPoopPosition()
  {
    Structures_Outhouse structuresOuthouse1 = (Structures_Outhouse) null;
    foreach (Structures_Outhouse structuresOuthouse2 in StructureManager.GetAllStructuresOfType<Structures_Outhouse>())
    {
      if (structuresOuthouse1 == null || (double) Vector3.Distance(this._brain.LastPosition, structuresOuthouse2.Data.Position) < (double) Vector3.Distance(this._brain.LastPosition, structuresOuthouse1.Data.Position))
        structuresOuthouse1 = structuresOuthouse2;
    }
    if (structuresOuthouse1 != null)
    {
      Vector3 normalized = (this._brain.LastPosition - structuresOuthouse1.Data.Position).normalized;
      this.ClosestWasteTile = StructureManager.GetCloseTile(structuresOuthouse1.Data.Position + normalized, FollowerLocation.Base);
      if (this.ClosestWasteTile != null)
      {
        this.ClosestWasteTile.ReservedForWaste = true;
        List<PlacementRegion.TileGridTile> followerPositions = this.GetAvailableFollowerPositions(this.ClosestWasteTile);
        return followerPositions.Count > 0 ? followerPositions[UnityEngine.Random.Range(0, followerPositions.Count)].WorldPosition : this.ClosestWasteTile.WorldPosition;
      }
    }
    this.ClosestWasteTile = StructureManager.GetBestWasteTile(this._brain.Location);
    if (this.ClosestWasteTile != null)
    {
      this.ClosestWasteTile.ReservedForWaste = true;
      List<PlacementRegion.TileGridTile> followerPositions = this.GetAvailableFollowerPositions(this.ClosestWasteTile);
      return followerPositions.Count > 0 ? followerPositions[UnityEngine.Random.Range(0, followerPositions.Count)].WorldPosition : this.ClosestWasteTile.WorldPosition;
    }
    return (UnityEngine.Object) this.follower == (UnityEngine.Object) null ? this._brain.LastPosition : this.follower.transform.position;
  }

  public List<PlacementRegion.TileGridTile> GetAvailableFollowerPositions(
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
}
