// Decompiled with JetBrains decompiler
// Type: FollowerTask_Build
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine;
using System;
using UnityEngine;

#nullable disable
public class FollowerTask_Build : FollowerTask
{
  public bool _helpingPlayer;
  public int _buildSiteID;
  public Structures_BuildSite _buildSite;
  public float _gameTimeSinceLastProgress;
  public Follower follower;

  public override FollowerTaskType Type => FollowerTaskType.Build;

  public override FollowerLocation Location => this._buildSite.Data.Location;

  public override bool BlockTaskChanges => false;

  public override bool BlockReactTasks => true;

  public override int UsingStructureID => this._buildSiteID;

  public override float Priorty => this._priority;

  public override PriorityCategory GetPriorityCategory(
    FollowerRole FollowerRole,
    WorkerPriority WorkerPriority,
    FollowerBrain brain)
  {
    if (brain == null || brain.CurrentOverrideTaskType == FollowerTaskType.None || (UnityEngine.Object) Interaction_Temple.Instance == (UnityEngine.Object) null)
    {
      switch (FollowerRole)
      {
        case FollowerRole.Lumberjack:
        case FollowerRole.StoneMiner:
        case FollowerRole.Builder:
          return PriorityCategory.WorkPriority;
      }
    }
    return PriorityCategory.Medium;
  }

  public FollowerTask_Build(int buildSiteID)
  {
    this._helpingPlayer = false;
    this._buildSiteID = buildSiteID;
    this._buildSite = StructureManager.GetStructureByID<Structures_BuildSite>(this._buildSiteID);
    this._priority = StructuresData.GetCategory(this._buildSite.Data.ToBuildType) == StructureBrain.Categories.AESTHETIC ? 80f : 100f;
  }

  public FollowerTask_Build(BuildSitePlot buildSite)
  {
    this._helpingPlayer = true;
    this._buildSiteID = buildSite.StructureInfo.ID;
    this._buildSite = buildSite.StructureBrain;
    Interaction_PlayerBuild.PlayerActivatingEnd += new Action<BuildSitePlot>(this.OnPlayerActivatingEnd);
    this._priority = StructuresData.GetCategory(this._buildSite.Data.ToBuildType) == StructureBrain.Categories.AESTHETIC ? 80f : 100f;
  }

  public override int GetSubTaskCode() => this._buildSiteID;

  public override void ClaimReservations()
  {
    if (this._helpingPlayer)
      return;
    Structures_BuildSite structureById = StructureManager.GetStructureByID<Structures_BuildSite>(this._buildSiteID);
    if (structureById != null && structureById.AvailableSlotCount > 0)
      ++structureById.UsedSlotCount;
    else
      this.End();
  }

  public override void ReleaseReservations()
  {
    if (this._helpingPlayer)
      return;
    Structures_BuildSite structureById = StructureManager.GetStructureByID<Structures_BuildSite>(this._buildSiteID);
    if (structureById == null)
      return;
    --structureById.UsedSlotCount;
  }

  public override void OnStart()
  {
    Structures_BuildSite structureById = StructureManager.GetStructureByID<Structures_BuildSite>(this._buildSiteID);
    if (structureById != null)
    {
      structureById.OnBuildComplete += new System.Action(this.OnBuildComplete);
      this.ClearDestination();
      this.SetState(FollowerTaskState.GoingTo);
    }
    else
      this.End();
  }

  public override void OnComplete()
  {
    Structures_BuildSite structureById = StructureManager.GetStructureByID<Structures_BuildSite>(this._buildSiteID);
    if (structureById != null)
      structureById.OnBuildComplete -= new System.Action(this.OnBuildComplete);
    Interaction_PlayerBuild.PlayerActivatingEnd -= new Action<BuildSitePlot>(this.OnPlayerActivatingEnd);
  }

  public override void TaskTick(float deltaGameTime)
  {
    if (this.State != FollowerTaskState.Doing)
      return;
    this._gameTimeSinceLastProgress += deltaGameTime;
    if (!((UnityEngine.Object) this.follower == (UnityEngine.Object) null))
      return;
    this.ProgressTask();
  }

  public override void ProgressTask()
  {
    Structures_BuildSite structureById = StructureManager.GetStructureByID<Structures_BuildSite>(this._buildSiteID);
    if (structureById != null)
      structureById.BuildProgress += this._gameTimeSinceLastProgress * this._brain.Info.ProductivityMultiplier;
    this._gameTimeSinceLastProgress = 0.0f;
  }

  public void OnBuildComplete()
  {
    this._brain.GetXP(1f);
    this.End();
  }

  public void OnPlayerActivatingEnd(BuildSitePlot buildSite)
  {
    if (buildSite.StructureInfo.ID != this._buildSiteID)
      return;
    this.End();
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    Structures_BuildSite structureById = StructureManager.GetStructureByID<Structures_BuildSite>(this._buildSiteID);
    return structureById.Data.Position + new Vector3(0.0f, (float) structureById.Data.Bounds.y / 2f) + (Vector3) (UnityEngine.Random.insideUnitCircle * ((float) structureById.Data.Bounds.x * 0.5f));
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    follower.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
    this.follower = follower;
  }

  public override void OnDoingBegin(Follower follower)
  {
    Structures_BuildSite structureById = StructureManager.GetStructureByID<Structures_BuildSite>(this._buildSiteID);
    follower.State.facingAngle = Utils.GetAngle(follower.transform.position, structureById.Data.Position);
    follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    double num = (double) follower.SetBodyAnimation(follower.Brain.CurrentState == null || !follower.Brain.HasThought(Thought.Intimidated) ? "build" : "build-fast-scared", true);
    this.follower = follower;
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

  public BuildSitePlot FindPlot()
  {
    BuildSitePlot plot = (BuildSitePlot) null;
    foreach (BuildSitePlot buildSitePlot in BuildSitePlot.BuildSitePlots)
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
