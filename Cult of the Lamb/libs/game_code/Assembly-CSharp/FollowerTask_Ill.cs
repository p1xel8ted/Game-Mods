// Decompiled with JetBrains decompiler
// Type: FollowerTask_Ill
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FollowerTask_Ill : FollowerTask
{
  public float _gameTimeToNextStateUpdate;
  public bool GoToVomit;
  public bool ForceFirstVomit = true;
  public PlacementRegion.TileGridTile ClosestTile;

  public override FollowerTaskType Type => FollowerTaskType.Ill;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockReactTasks => true;

  public override bool BlockSermon => true;

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
      }
      if (this._state != FollowerTaskState.Idle)
        return;
      this._gameTimeToNextStateUpdate -= deltaGameTime;
      if ((double) this._gameTimeToNextStateUpdate <= 0.0)
      {
        this.GoToVomit = false;
        this.ClearDestination();
        this.SetState(FollowerTaskState.GoingTo);
        this._gameTimeToNextStateUpdate = UnityEngine.Random.Range(3f, 5f);
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
        }
        else
        {
          this.GoToVomit = false;
          this.ClearDestination();
          this.SetState(FollowerTaskState.GoingTo);
          this._gameTimeToNextStateUpdate = UnityEngine.Random.Range(3f, 5f);
        }
      }
    }
  }

  public override void ProgressTask()
  {
    this._brain.Stats.LastVomit = TimeManager.TotalElapsedGameTime;
    StructuresData infoByType = StructuresData.GetInfoByType(StructureBrain.TYPES.VOMIT, 0);
    infoByType.FollowerID = this._brain.Info.ID;
    if (!this._currentDestination.HasValue)
    {
      StructureManager.BuildStructure(this._brain.Location, infoByType, this._brain.LastPosition, Vector2Int.one, false);
    }
    else
    {
      PlacementRegion.TileGridTile tileGridTile = (PlacementRegion.TileGridTile) null;
      if ((bool) (UnityEngine.Object) PlacementRegion.Instance)
        tileGridTile = StructureManager.GetClosestTileGridTileAtWorldPosition(this._currentDestination.Value, PlacementRegion.Instance.StructureInfo.Grid);
      if (tileGridTile != null)
      {
        infoByType.GridTilePosition = tileGridTile.Position;
        StructureManager.BuildStructure(this._brain.Location, infoByType, tileGridTile.WorldPosition, Vector2Int.one, false);
      }
      else
        StructureManager.BuildStructure(this._brain.Location, infoByType, this._currentDestination.Value, Vector2Int.one, false);
    }
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    if (!this.GoToVomit)
      return TownCentre.RandomCircleFromTownCentre(10f);
    this.ClosestTile = StructureManager.GetBestWasteTile(this._brain.Location);
    if (this.ClosestTile != null)
    {
      this.ClosestTile.ReservedForWaste = true;
      return this.ClosestTile.WorldPosition;
    }
    return (UnityEngine.Object) follower == (UnityEngine.Object) null ? this._brain.LastPosition : follower.transform.position;
  }

  public override float RestChange(float deltaGameTime) => 100f;

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    follower.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Sick/idle-sick");
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "Sick/walk-sick");
  }

  public override void OnDoingBegin(Follower follower)
  {
    this.GoToVomit = false;
    follower.TimedAnimation("Sick/chunder", 3.5f, (System.Action) (() => this.SetState(FollowerTaskState.Idle)));
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    follower.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "idle");
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "run");
  }

  public void HandleAnimationStateEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (!(e.Data.Name == "Vomit"))
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
    if (structure.Type != StructureBrain.TYPES.VOMIT || structure.FollowerID != this._brain.Info.ID)
      return;
    this.SetState(FollowerTaskState.Idle);
  }

  public override void SimDoingBegin(SimFollower simFollower)
  {
    base.SimDoingBegin(simFollower);
    this.GetDestination((Follower) null);
    this.ProgressTask();
  }

  [CompilerGenerated]
  public void \u003COnDoingBegin\u003Eb__21_0() => this.SetState(FollowerTaskState.Idle);
}
