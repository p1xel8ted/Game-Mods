// Decompiled with JetBrains decompiler
// Type: FollowerTask_BuildProject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine;
using System;
using UnityEngine;

#nullable disable
public class FollowerTask_BuildProject : FollowerTask
{
  public bool _helpingPlayer;
  public int _buildSiteID;
  public Structures_BuildSiteProject _buildSite;
  public float _gameTimeSinceLastProgress;

  public override FollowerTaskType Type => FollowerTaskType.Build;

  public override FollowerLocation Location => this._buildSite.Data.Location;

  public override bool BlockTaskChanges => this._helpingPlayer;

  public override bool BlockReactTasks => true;

  public override int UsingStructureID => this._buildSiteID;

  public override float Priorty => 100f;

  public override PriorityCategory GetPriorityCategory(
    FollowerRole FollowerRole,
    WorkerPriority WorkerPriority,
    FollowerBrain brain)
  {
    return PriorityCategory.OverrideWorkPriority;
  }

  public FollowerTask_BuildProject(int buildSiteID)
  {
    this._buildSiteID = buildSiteID;
    this._buildSite = StructureManager.GetStructureByID<Structures_BuildSiteProject>(this._buildSiteID);
  }

  public FollowerTask_BuildProject(BuildSitePlotProject buildSite)
  {
    this._helpingPlayer = true;
    this._buildSiteID = buildSite.StructureInfo.ID;
    this._buildSite = buildSite.StructureBrain;
    Interaction_PlayerBuildProject.PlayerActivatingEnd += new Action<BuildSitePlotProject>(this.OnPlayerActivatingEnd);
  }

  public override int GetSubTaskCode() => this._buildSiteID;

  public override void ClaimReservations()
  {
    if (this._helpingPlayer)
      return;
    Structures_BuildSiteProject structureById = StructureManager.GetStructureByID<Structures_BuildSiteProject>(this._buildSiteID);
    if (structureById != null && structureById.AvailableSlotCount > 0)
      ++structureById.UsedSlotCount;
    else
      this.End();
  }

  public override void ReleaseReservations()
  {
    if (this._helpingPlayer)
      return;
    Structures_BuildSiteProject structureById = StructureManager.GetStructureByID<Structures_BuildSiteProject>(this._buildSiteID);
    if (structureById == null)
      return;
    --structureById.UsedSlotCount;
  }

  public override void OnStart()
  {
    Structures_BuildSiteProject structureById = StructureManager.GetStructureByID<Structures_BuildSiteProject>(this._buildSiteID);
    if (structureById != null)
    {
      structureById.OnBuildComplete += new System.Action(this.OnBuildComplete);
      this.SetState(FollowerTaskState.GoingTo);
    }
    else
      this.End();
  }

  public override void OnComplete()
  {
    Structures_BuildSiteProject structureById = StructureManager.GetStructureByID<Structures_BuildSiteProject>(this._buildSiteID);
    if (structureById != null)
      structureById.OnBuildComplete -= new System.Action(this.OnBuildComplete);
    Interaction_PlayerBuildProject.PlayerActivatingEnd -= new Action<BuildSitePlotProject>(this.OnPlayerActivatingEnd);
  }

  public override void TaskTick(float deltaGameTime)
  {
    if (this.State != FollowerTaskState.Doing)
      return;
    this._gameTimeSinceLastProgress += deltaGameTime;
  }

  public override void ProgressTask()
  {
    StructureManager.GetStructureByID<Structures_BuildSiteProject>(this._buildSiteID).BuildProgress += this._gameTimeSinceLastProgress * this._brain.Info.ProductivityMultiplier;
    this._gameTimeSinceLastProgress = 0.0f;
  }

  public void OnBuildComplete()
  {
    this._brain.GetXP(1f);
    this.End();
  }

  public void OnPlayerActivatingEnd(BuildSitePlotProject buildSite)
  {
    if (buildSite.StructureInfo.ID != this._buildSiteID)
      return;
    this.End();
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    Structures_BuildSiteProject structureById = StructureManager.GetStructureByID<Structures_BuildSiteProject>(this._buildSiteID);
    return structureById.Data.Position + new Vector3(0.0f, (float) structureById.Data.Bounds.y / 2f) + (Vector3) (UnityEngine.Random.insideUnitCircle * ((float) structureById.Data.Bounds.x * 0.5f));
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    follower.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
  }

  public override void OnGoingToBegin(Follower follower)
  {
    this._currentDestination = new Vector3?(this.UpdateDestination(follower));
    base.OnGoingToBegin(follower);
  }

  public override void OnDoingBegin(Follower follower)
  {
    Structures_BuildSiteProject structureById = StructureManager.GetStructureByID<Structures_BuildSiteProject>(this._buildSiteID);
    follower.State.facingAngle = Utils.GetAngle(follower.transform.position, structureById.Data.Position);
    follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    double num = (double) follower.SetBodyAnimation(follower.Brain.CurrentState.Type == FollowerStateType.Motivated ? "build-fast-scared" : "build", true);
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    follower.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
  }

  public void HandleAnimationStateEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (!(e.Data.Name == "Build"))
      return;
    this.ProgressTask();
  }

  public BuildSitePlotProject FindPlot()
  {
    BuildSitePlotProject plot = (BuildSitePlotProject) null;
    foreach (BuildSitePlotProject buildSitePlot in BuildSitePlotProject.BuildSitePlots)
    {
      if (buildSitePlot.StructureInfo.ID == this._buildSiteID)
      {
        plot = buildSitePlot;
        break;
      }
    }
    return plot;
  }
}
