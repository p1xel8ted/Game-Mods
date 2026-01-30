// Decompiled with JetBrains decompiler
// Type: FollowerTask_Pray
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FollowerTask_Pray : FollowerTask
{
  public int _shrineID;
  public Structures_Shrine _shrine;
  [CompilerGenerated]
  public int \u003CPreferredCircleIndex\u003Ek__BackingField = -1;
  public Follower follower;
  public bool recalculatedPosition = true;

  public override FollowerTaskType Type => FollowerTaskType.Pray;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override int UsingStructureID => this._shrineID;

  public int PrayerID => this._brain.Info.ID;

  public int PreferredCircleIndex
  {
    get => this.\u003CPreferredCircleIndex\u003Ek__BackingField;
    set => this.\u003CPreferredCircleIndex\u003Ek__BackingField = value;
  }

  public new FollowerBrain Brain => this._brain;

  public override float Priorty => 1f;

  public override PriorityCategory GetPriorityCategory(
    FollowerRole FollowerRole,
    WorkerPriority WorkerPriority,
    FollowerBrain brain)
  {
    if (FollowerRole == FollowerRole.Worshipper)
      return PriorityCategory.WorkPriority;
    return brain.HasTrait(FollowerTrait.TraitType.Mutated) ? PriorityCategory.Ignore : PriorityCategory.Low;
  }

  public float _progress
  {
    get => this._brain.Info.PrayProgress;
    set => this._brain.Info.PrayProgress = value;
  }

  public FollowerTask_Pray(int shrineID)
  {
    this._shrineID = shrineID;
    this._shrine = StructureManager.GetStructureByID<Structures_Shrine>(this._shrineID);
  }

  public override int GetSubTaskCode() => this._shrineID;

  public override void ClaimReservations()
  {
    if (this._shrine == null)
      return;
    this._shrine.AddPrayer(this);
  }

  public override void ReleaseReservations()
  {
    if (this._shrine == null)
      return;
    this._shrine.RemovePrayer(this);
  }

  public override void OnStart()
  {
    if (this._shrine != null && this.Brain.HasTrait(FollowerTrait.TraitType.Spy))
      this.Brain.HardSwapToTask((FollowerTask) new FollowerTask_SpyPray(this._shrine.Data.ID));
    this.ClearDestination();
    this.SetState(FollowerTaskState.GoingTo);
  }

  public override void OnArrive() => this.SetState(FollowerTaskState.Idle);

  public override void OnAbort()
  {
    Follower followerById = FollowerManager.FindFollowerByID(this.Brain.Info.ID);
    if (!((UnityEngine.Object) followerById != (UnityEngine.Object) null) || !((UnityEngine.Object) followerById.UIFollowerPrayingProgress != (UnityEngine.Object) null))
      return;
    followerById.UIFollowerPrayingProgress.Hide();
  }

  public override void TaskTick(float deltaGameTime)
  {
    if (this._state == FollowerTaskState.Idle)
    {
      this._progress += deltaGameTime * this._brain.Info.ProductivityMultiplier;
      if ((double) this._progress >= (double) this.GetDurationPerDevotion(this._brain))
      {
        this.SetState(FollowerTaskState.Doing);
        this._progress = 0.0f;
      }
      if ((UnityEngine.Object) this.follower != (UnityEngine.Object) null && this._shrine != null && this._shrine.GetPrayerPosition(this.follower.Brain) == this._shrine.Data.Position)
        this.SetState(FollowerTaskState.GoingTo);
    }
    if ((UnityEngine.Object) this.follower != (UnityEngine.Object) null)
      this.follower.FacePosition(this._shrine.Data.Position);
    if (this._shrine.SoulCount < this._shrine.SoulMax)
      return;
    this.Complete();
  }

  public float GetDurationPerDevotion(Follower forFollower)
  {
    return this.GetDurationPerDevotion(forFollower.Brain);
  }

  public float GetDurationPerDevotion(FollowerBrain brain)
  {
    return 60f / this._shrine.DevotionSpeedMultiplier / brain.DevotionToGive;
  }

  public void UndoStateAnimationChanges(Follower follower)
  {
    SimpleSpineAnimator.SpineChartacterAnimationData animationData = follower.SimpleAnimator.GetAnimationData(StateMachine.State.Idle);
    animationData.Animation = animationData.DefaultAnimation;
    follower.ResetStateAnimations();
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    return this._shrine.GetPrayerPosition(follower.Brain);
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, this._shrine.Data.FullyFueled ? "pray-flame" : "pray");
    this.follower = follower;
    if (this._brain == null || this._shrine == null || !(this._brain.LastPosition == this._shrine.Data.Position))
      return;
    follower.Seeker.CancelCurrentPathRequest();
    this.ClearDestination();
    this.SetState(FollowerTaskState.Doing);
    follower.transform.position = this.UpdateDestination(follower);
  }

  public override void OnIdleBegin(Follower follower)
  {
    base.OnIdleBegin(follower);
    this.follower = follower;
    if (!follower.Brain.HasTrait(FollowerTrait.TraitType.Spy))
      follower.UIFollowerPrayingProgress.Show();
    follower.State.facingAngle = Utils.GetAngle(follower.transform.position, this._shrine.Data.Position);
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, this._shrine.Data.FullyFueled ? "pray-flame" : "pray");
  }

  public override void OnDoingBegin(Follower follower)
  {
    base.OnDoingBegin(follower);
    this.follower = follower;
    if (!follower.Brain.HasTrait(FollowerTrait.TraitType.Spy))
      follower.UIFollowerPrayingProgress.Flash();
    this._brain.GetXP(1f);
    BuildingShrine shrine = this.FindShrine();
    if ((UnityEngine.Object) shrine == (UnityEngine.Object) null)
    {
      int SoulsToDeposit = 0;
      int num = Mathf.Clamp(this._brain.Info.XPLevel, 1, 10);
      SoulsToDeposit += num;
      SoulsToDeposit = (int) ((double) SoulsToDeposit * 0.5 + 0.5);
      SoulsToDeposit = Mathf.Max(1, SoulsToDeposit);
      while (--SoulsToDeposit >= 0)
        SoulCustomTarget.Create(shrine.ReceiveSoulPosition, follower.transform.position, Color.white, (System.Action) (() => this.DepositSoul(1, SoulsToDeposit == 0)), 0.2f);
    }
    else
      follower.StartCoroutine((IEnumerator) this.GiveDevotionRoutine(shrine, follower));
    follower.TimedAnimation(this._shrine.Data.FullyFueled ? "devotion/devotion-collect-flame" : "devotion/devotion-collect", 1f, (System.Action) (() =>
    {
      this.SetState(FollowerTaskState.Idle);
      follower.State.CURRENT_STATE = StateMachine.State.Idle;
    }));
  }

  public override void OnFinaliseBegin(Follower follower)
  {
    follower.UIFollowerPrayingProgress.Hide();
    base.OnFinaliseBegin(follower);
  }

  public override void Cleanup(Follower follower)
  {
    follower.UIFollowerPrayingProgress.Hide();
    this.UndoStateAnimationChanges(follower);
    base.Cleanup(follower);
  }

  public IEnumerator GiveDevotionRoutine(BuildingShrine shrine, Follower follower)
  {
    FollowerTask_Pray followerTaskPray = this;
    yield return (object) new WaitForSeconds(0.2f);
    int SoulsToDeposit = 0;
    SoulsToDeposit += Mathf.Clamp(followerTaskPray._brain.Info.XPLevel, 1, 10);
    SoulsToDeposit = (int) ((double) SoulsToDeposit * 0.5 + 0.5);
    SoulsToDeposit = Mathf.Max(1, SoulsToDeposit);
    while (--SoulsToDeposit >= 0)
    {
      SoulCustomTarget.Create(shrine.ReceiveSoulPosition, follower.transform.position, Color.white, (System.Action) (() => this.DepositSoul(1, SoulsToDeposit == 0)), 0.2f);
      yield return (object) new WaitForSeconds(0.1f);
    }
  }

  public virtual void DepositSoul(int DevotionToGive, bool SetIdle)
  {
    this._shrine.SoulCount += DevotionToGive;
    if (!SetIdle)
      return;
    this.SetState(FollowerTaskState.Idle);
  }

  public BuildingShrine FindShrine()
  {
    BuildingShrine shrine1 = (BuildingShrine) null;
    foreach (BuildingShrine shrine2 in BuildingShrine.Shrines)
    {
      if (shrine2.Structure.Structure_Info.ID == this._shrineID)
      {
        shrine1 = shrine2;
        break;
      }
    }
    return shrine1;
  }

  public override void SimDoingBegin(SimFollower simFollower)
  {
    int num = 0 + Mathf.Clamp(this._brain.Info.XPLevel, 1, 10);
    while (--num >= 0)
      this.DepositSoul(1, num == 0);
  }
}
