// Decompiled with JetBrains decompiler
// Type: FollowerTask_Janitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FollowerTask_Janitor : FollowerTask
{
  public const float CLEANING_DURATION_GAME_MINUTES = 4.2f;
  public int _janitorID;
  public int _targetID = -1;
  public Structures_JanitorStation _janitorStation;
  public float _progress;
  public float _gameTimeSinceLastProgress;

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

  public override int GetSubTaskCode() => this._janitorID;

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

  public override void OnStart() => this.SetState(FollowerTaskState.GoingTo);

  public override void TaskTick(float deltaGameTime)
  {
    if (this.State != FollowerTaskState.Doing)
      return;
    float num = 1f;
    this._gameTimeSinceLastProgress += deltaGameTime * num * this._brain.Info.ProductivityMultiplier;
    this.ProgressTask();
  }

  public override void ProgressTask()
  {
    StructureBrain structureById1 = StructureManager.GetStructureByID<StructureBrain>(this._targetID);
    if (structureById1 == null)
    {
      this.Loop();
    }
    else
    {
      if (!structureById1.ReservedForTask)
        return;
      this._progress += this._gameTimeSinceLastProgress;
      this._gameTimeSinceLastProgress = 0.0f;
      if ((double) this._progress < 4.1999998092651367)
        return;
      this._progress = 0.0f;
      StructureBrain structureById2 = StructureManager.GetStructureByID<StructureBrain>(this._targetID);
      if (structureById2 != null)
      {
        if (structureById2 is Structures_Outhouse)
        {
          foreach (InventoryItem inventoryItem in structureById2.Data.Inventory)
            InventoryItem.Spawn((InventoryItem.ITEM_TYPE) inventoryItem.type, inventoryItem.quantity, structureById2.Data.Position, 4f, FollowerLocation.Base);
          structureById2.Data.Inventory.Clear();
          foreach (Interaction_Outhouse outhouse in Interaction_Outhouse.Outhouses)
          {
            if (outhouse.StructureInfo.ID == structureById2.Data.ID)
              outhouse.UpdateGauge();
          }
        }
        else
          structureById2.Remove();
        ++this._janitorStation.SoulCount;
      }
      this.Loop();
    }
  }

  public int GetNextStructure()
  {
    List<StructureBrain> structureBrainList1 = new List<StructureBrain>();
    structureBrainList1.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType(this.Location, StructureBrain.TYPES.POOP));
    structureBrainList1.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType(this.Location, StructureBrain.TYPES.POOP_GLOW));
    structureBrainList1.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType(this.Location, StructureBrain.TYPES.POOP_GOLD));
    structureBrainList1.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType(this.Location, StructureBrain.TYPES.POOP_ROTSTONE));
    structureBrainList1.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType(this.Location, StructureBrain.TYPES.POOP_RAINBOW));
    structureBrainList1.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType(this.Location, StructureBrain.TYPES.POOP_DEVOTION));
    structureBrainList1.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType(this.Location, StructureBrain.TYPES.VOMIT));
    structureBrainList1.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType(this.Location, StructureBrain.TYPES.TOXIC_WASTE));
    if (this._janitorStation.Data.Type == StructureBrain.TYPES.JANITOR_STATION_2)
    {
      foreach (Structures_Outhouse structuresOuthouse in new List<Structures_Outhouse>((IEnumerable<Structures_Outhouse>) StructureManager.GetAllStructuresOfType<Structures_Outhouse>()))
      {
        if (structuresOuthouse.GetPoopCount() > 0)
          structureBrainList1.Add((StructureBrain) structuresOuthouse);
      }
    }
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

  public void Loop()
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

  public override Vector3 UpdateDestination(Follower follower)
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

  public JanitorStation FindJanitaorStation()
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

  [CompilerGenerated]
  public void \u003COnDoingBegin\u003Eb__29_0() => this.ProgressTask();
}
