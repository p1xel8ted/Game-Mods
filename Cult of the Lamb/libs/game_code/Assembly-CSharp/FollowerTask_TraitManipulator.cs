// Decompiled with JetBrains decompiler
// Type: FollowerTask_TraitManipulator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using UnityEngine;

#nullable disable
public class FollowerTask_TraitManipulator : FollowerTask
{
  public Structures_TraitManipulator manipulatorStructure;
  public int refineryID;
  public Follower follower;
  public EventInstance loopingSound;

  public override FollowerTaskType Type => FollowerTaskType.TraitManipulator;

  public override FollowerLocation Location => this.manipulatorStructure.Data.Location;

  public override bool BlockSocial => true;

  public override float Priorty => 25f;

  public override PriorityCategory GetPriorityCategory(
    FollowerRole FollowerRole,
    WorkerPriority WorkerPriority,
    FollowerBrain brain)
  {
    return FollowerRole == FollowerRole.TraitManipulator ? PriorityCategory.WorkPriority : PriorityCategory.Low;
  }

  public FollowerTask_TraitManipulator(int refineryID)
  {
    this.refineryID = refineryID;
    this.manipulatorStructure = StructureManager.GetStructureByID<Structures_TraitManipulator>(refineryID);
  }

  public override int GetSubTaskCode() => this.refineryID;

  public override void ClaimReservations()
  {
    Structures_TraitManipulator structureById = StructureManager.GetStructureByID<Structures_TraitManipulator>(this.refineryID);
    if (structureById == null)
      return;
    structureById.ReservedForTask = true;
  }

  public override void ReleaseReservations()
  {
    Structures_TraitManipulator structureById = StructureManager.GetStructureByID<Structures_TraitManipulator>(this.refineryID);
    if (structureById == null)
      return;
    structureById.ReservedForTask = false;
  }

  public override void Setup(Follower follower) => base.Setup(follower);

  public override void OnStart() => this.SetState(FollowerTaskState.GoingTo);

  public override void TaskTick(float deltaGameTime)
  {
    if (TimeManager.IsNight && !this.Brain._directInfoAccess.WorkThroughNight || this.manipulatorStructure.Data.FollowerID == -1)
      this.End();
    if (this.State != FollowerTaskState.Doing)
      return;
    this.manipulatorStructure.Data.Progress += deltaGameTime * this._brain.Info.ProductivityMultiplier;
    if ((double) this.manipulatorStructure.Data.Progress < (double) this.manipulatorStructure.Duration)
      return;
    this.Complete();
  }

  public override void OnDoingBegin(Follower follower)
  {
    base.OnDoingBegin(follower);
    follower.FacePosition(this.manipulatorStructure.Data.Position);
    follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    double num = (double) follower.SetBodyAnimation("Rituals/lobotomy-manipulating", true);
    this.follower = follower;
    this.loopingSound = AudioManager.Instance.CreateLoop("event:/dlc/building/exorcismaltar/exorcism_exorcising_loop", follower.gameObject, true);
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    AudioManager.Instance.StopLoop(this.loopingSound);
  }

  public override void OnEnd()
  {
    base.OnEnd();
    AudioManager.Instance.StopLoop(this.loopingSound);
  }

  public override void OnAbort()
  {
    base.OnAbort();
    AudioManager.Instance.StopLoop(this.loopingSound);
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    Interaction_TraitManipulator manipulator = this.FindManipulator();
    return !((Object) manipulator != (Object) null) ? this.manipulatorStructure.Data.Position + new Vector3(0.0f, 1f) : manipulator.FollowerPosition.transform.position;
  }

  public Interaction_TraitManipulator FindManipulator()
  {
    foreach (Interaction_TraitManipulator traitManipulator in Interaction_TraitManipulator.TraitManipulators)
    {
      if ((Object) traitManipulator != (Object) null && (Object) traitManipulator.Structure != (Object) null && traitManipulator.Structure.Structure_Info != null && traitManipulator.Structure.Structure_Info.ID == this.refineryID)
        return traitManipulator;
    }
    return (Interaction_TraitManipulator) null;
  }
}
