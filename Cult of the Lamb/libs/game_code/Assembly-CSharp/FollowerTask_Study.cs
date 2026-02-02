// Decompiled with JetBrains decompiler
// Type: FollowerTask_Study
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FollowerTask_Study : FollowerTask
{
  public static FollowerBrain[] assignedSeats = new FollowerBrain[4];
  public float progress;
  public int structureID;
  public Structures_Temple temple;
  public Follower follower;

  public override FollowerTaskType Type => FollowerTaskType.Study;

  public override FollowerLocation Location => FollowerLocation.Church;

  public override bool BlockReactTasks => true;

  public override float Priorty => 20f;

  public FollowerTask_Study(int structID)
  {
    this.structureID = structID;
    this.temple = StructureManager.GetStructureByID<Structures_Temple>(this.structureID);
  }

  public override void ClaimReservations() => this.temple.AddStudier(this);

  public override void ReleaseReservations() => this.temple.RemoveStudier(this);

  public override int GetSubTaskCode() => 0;

  public override PriorityCategory GetPriorityCategory(
    FollowerRole FollowerRole,
    WorkerPriority WorkerPriority,
    FollowerBrain brain)
  {
    switch (FollowerRole)
    {
      case FollowerRole.Worshipper:
      case FollowerRole.Worker:
      case FollowerRole.Lumberjack:
      case FollowerRole.Farmer:
        return PriorityCategory.Low;
      case FollowerRole.Monk:
        return TimeManager.GetOverrideScheduledActivity() != ScheduledActivity.Work ? PriorityCategory.Medium : PriorityCategory.Low;
      default:
        return PriorityCategory.Low;
    }
  }

  public override void OnStart()
  {
    Debug.Log((object) ("START! " + this._brain.Info.CacheXP.ToString()));
    TimeManager.OnNewPhaseStarted += new System.Action(this.OnNewPhaseStarted);
    this.SetState(FollowerTaskState.GoingTo);
  }

  public override void OnArrive() => this.SetState(FollowerTaskState.Doing);

  public override void TaskTick(float deltaGameTime)
  {
    if (this.State != FollowerTaskState.Doing)
      return;
    if ((double) DataManager.Instance.TempleStudyXP < (double) Structures_Temple.TempleMaxStudyXP)
    {
      this.progress += deltaGameTime * this._brain.Info.ProductivityMultiplier;
      if ((bool) (UnityEngine.Object) this.follower)
      {
        this.follower.FollowerRadialProgress.UpdateBar(DataManager.Instance.TempleStudyXP / Structures_Temple.TempleMaxStudyXP);
        if (this.follower.State.CURRENT_STATE != StateMachine.State.CustomAnimation)
        {
          this.follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
          double num = (double) this.follower.SetBodyAnimation("studying", true);
        }
      }
      if ((double) this.progress < 12.0)
        return;
      this.progress = 0.0f;
      DataManager.Instance.TempleStudyXP += 0.05f;
    }
    else
      this.Complete();
  }

  public new void OnNewPhaseStarted()
  {
    if (TimeManager.CurrentPhase != DayPhase.Night)
      return;
    this.End();
  }

  public void AssignSeat(Follower follower)
  {
    FollowerTask_Study.assignedSeats[ChurchFollowerManager.Instance.GetClosestSlotIndex(follower.Brain.LastPosition)] = follower.Brain;
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    if (!(bool) (UnityEngine.Object) ChurchFollowerManager.Instance)
      return new Vector3(0.0f, -12f, 0.0f);
    for (int index = 0; index < Structures_Temple.AvailableStudySlots; ++index)
    {
      if (index < Structures_Temple.AvailableStudySlots && (FollowerTask_Study.assignedSeats[index] == null || FollowerTask_Study.assignedSeats[index].Info.ID == follower.Brain.Info.ID || !(FollowerTask_Study.assignedSeats[index].CurrentTask is FollowerTask_Study)))
      {
        FollowerTask_Study.assignedSeats[index] = follower.Brain;
        return ChurchFollowerManager.Instance.GetSlotPosition(index);
      }
    }
    Debug.Log((object) "Whoops! No appropriate place to go");
    this.Complete();
    return new Vector3(0.0f, -12f, 0.0f);
  }

  public override void Setup(Follower follower) => base.Setup(follower);

  public override void OnDoingBegin(Follower follower)
  {
    this.follower = follower;
    follower.FollowerRadialProgress.Show();
    this.AssignSeat(follower);
  }

  public override void OnFinaliseBegin(Follower follower)
  {
    double num = (double) follower.SetBodyAnimation("idle", true);
    follower.FollowerRadialProgress.Hide();
    this.follower = (Follower) null;
    base.OnFinaliseBegin(follower);
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    follower.FollowerRadialProgress.Hide();
    follower.State.CURRENT_STATE = StateMachine.State.Idle;
    double num = (double) follower.SetBodyAnimation("idle", true);
    this.follower = (Follower) null;
  }
}
