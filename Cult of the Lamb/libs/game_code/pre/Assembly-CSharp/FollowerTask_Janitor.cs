// Decompiled with JetBrains decompiler
// Type: FollowerTask_Janitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FollowerTask_Janitor : FollowerTask
{
  public const float CLEANING_DURATION_GAME_MINUTES = 4.2f;
  private int _janitorID;
  private int _targetID = -1;
  private Structures_JanitorStation _janitorStation;
  private float _progress;
  private float _gameTimeSinceLastProgress;

  public override FollowerTaskType Type => FollowerTaskType.Janitor;

  public override FollowerLocation Location => this._janitorStation.Data.Location;

  public override int UsingStructureID => this._janitorID;

  public override bool BlockReactTasks => true;

  public override float Priorty => 22f;

  public override PriorityCategory GetPriorityCategory(
    FollowerRole FollowerRole,
    WorkerPriority WorkerPriority,
    FollowerBrain brain)
  {
    return FollowerRole == FollowerRole.Janitor ? PriorityCategory.WorkPriority : PriorityCategory.Low;
  }

  public FollowerTask_Janitor(int janitorID)
  {
    this._janitorID = janitorID;
    this._janitorStation = StructureManager.GetStructureByID<Structures_JanitorStation>(this._janitorID);
  }

  protected override int GetSubTaskCode() => this._janitorID;

  public override void ClaimReservations()
  {
    Structures_JanitorStation structureById1 = StructureManager.GetStructureByID<Structures_JanitorStation>(this._janitorID);
    if (structureById1 != null)
      structureById1.ReservedForTask = true;
    StructureBrain structureById2 = StructureManager.GetStructureByID<StructureBrain>(this._targetID);
    if (structureById2 == null)
      return;
    structureById2.ReservedForTask = true;
  }

  public override void ReleaseReservations()
  {
    Structures_JanitorStation structureById1 = StructureManager.GetStructureByID<Structures_JanitorStation>(this._janitorID);
    if (structureById1 != null)
      structureById1.ReservedForTask = false;
    StructureBrain structureById2 = StructureManager.GetStructureByID<StructureBrain>(this._targetID);
    if (structureById2 == null)
      return;
    structureById2.ReservedForTask = false;
  }

  protected override void OnStart() => this.SetState(FollowerTaskState.GoingTo);

  protected override void TaskTick(float deltaGameTime)
  {
    if (this.State != FollowerTaskState.Doing)
      return;
    float num = 1f;
    this._gameTimeSinceLastProgress += deltaGameTime * num;
    this.ProgressTask();
  }

  public override void ProgressTask()
  {
    StructureBrain structureById = StructureManager.GetStructureByID<StructureBrain>(this._targetID);
    if (structureById == null)
    {
      this.Loop();
    }
    else
    {
      if (!structureById.ReservedForTask)
        return;
      this._progress += this._gameTimeSinceLastProgress;
      this._gameTimeSinceLastProgress = 0.0f;
      if ((double) this._progress < 4.1999998092651367)
        return;
      this._progress = 0.0f;
      StructureManager.GetStructureByID<StructureBrain>(this._targetID)?.Remove();
      this.Loop();
    }
  }

  private int GetNextStructure()
  {
    List<StructureBrain> structureBrainList1 = new List<StructureBrain>();
    structureBrainList1.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType(this.Location, StructureBrain.TYPES.POOP));
    structureBrainList1.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType(this.Location, StructureBrain.TYPES.VOMIT));
    List<StructureBrain> structureBrainList2 = new List<StructureBrain>();
    foreach (StructureBrain structureBrain in structureBrainList1)
    {
      if (!structureBrain.ReservedByPlayer && !structureBrain.ReservedForTask)
        structureBrainList2.Add(structureBrain);
    }
    if (structureBrainList2.Count <= 0)
      return -1;
    StructureBrain structureBrain1 = (StructureBrain) null;
    foreach (StructureBrain structureBrain2 in structureBrainList2)
    {
      if (structureBrain1 == null || (double) Vector3.Distance(structureBrain2.Data.Position, this._brain.LastPosition) < (double) Vector3.Distance(structureBrain1.Data.Position, this._brain.LastPosition))
        structureBrain1 = structureBrain2;
    }
    return structureBrain1.Data.ID;
  }

  private void Loop()
  {
    int nextStructure = this.GetNextStructure();
    if (nextStructure != -1)
    {
      StructureBrain structureById = StructureManager.GetStructureByID<StructureBrain>(nextStructure);
      this._targetID = structureById.Data.ID;
      structureById.ReservedForTask = true;
      this.ClearDestination();
      this.SetState(FollowerTaskState.GoingTo);
    }
    else
      this.End();
  }

  public Structure GetStructure(int ID)
  {
    foreach (Structure structure in Structure.Structures)
    {
      if ((UnityEngine.Object) structure != (UnityEngine.Object) null && structure.Brain != null && structure.Brain.Data != null && structure.Brain.Data.ID == ID)
        return structure;
    }
    return (Structure) null;
  }

  protected override Vector3 UpdateDestination(Follower follower)
  {
    StructureBrain structureById = StructureManager.GetStructureByID<StructureBrain>(this._targetID);
    if (structureById != null)
      return structureById.Data.Position;
    JanitorStation janitaorStation = this.FindJanitaorStation();
    if ((UnityEngine.Object) janitaorStation != (UnityEngine.Object) null)
      return janitaorStation.transform.position;
    this.End();
    return Vector3.zero;
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    if (this._targetID == -1)
      return;
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "run-janitor");
  }

  public override void OnDoingBegin(Follower follower)
  {
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "run");
    if (this._targetID == -1)
    {
      follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "run-janitor");
      this.ProgressTask();
    }
    else
    {
      Structure structure = this.GetStructure(this._targetID);
      if (!((UnityEngine.Object) structure != (UnityEngine.Object) null))
        return;
      follower.FacePosition(structure.transform.position);
      if (!structure.Brain.ReservedForTask)
        return;
      follower.TimedAnimation("sweep-floor", 4.2f, (System.Action) (() => this.ProgressTask()));
    }
  }

  public override void Cleanup(Follower follower)
  {
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "run");
    base.Cleanup(follower);
  }

  private JanitorStation FindJanitaorStation()
  {
    JanitorStation janitaorStation = (JanitorStation) null;
    foreach (JanitorStation janitorStation in JanitorStation.JanitorStations)
    {
      if (janitorStation.StructureInfo.ID == this._janitorID)
      {
        janitaorStation = janitorStation;
        break;
      }
    }
    return janitaorStation;
  }
}
