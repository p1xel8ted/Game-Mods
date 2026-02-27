// Decompiled with JetBrains decompiler
// Type: FollowerTask_PrayPassive
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class FollowerTask_PrayPassive : FollowerTask
{
  public const int PRAYER_DURATION_GAME_MINUTES = 150;
  protected int _shrineID;
  public Structures_Shrine_Passive _shrine;

  public override FollowerTaskType Type => FollowerTaskType.PassivePray;

  public override FollowerLocation Location => this._shrine.Data.Location;

  public override int UsingStructureID => this._shrineID;

  public override bool BlockTaskChanges => true;

  public int PrayerID => this._brain.Info.ID;

  public override float Priorty => 100f;

  public override PriorityCategory GetPriorityCategory(
    FollowerRole FollowerRole,
    WorkerPriority WorkerPriority,
    FollowerBrain brain)
  {
    return FollowerRole == FollowerRole.Worshipper || FollowerRole == FollowerRole.Monk ? PriorityCategory.Low : PriorityCategory.OverrideWorkPriority;
  }

  private float _progress
  {
    get => this._brain.Info.PrayProgress;
    set => this._brain.Info.PrayProgress = value;
  }

  public FollowerTask_PrayPassive(int shrineID)
  {
    this._shrineID = shrineID;
    this._shrine = StructureManager.GetStructureByID<Structures_Shrine_Passive>(this._shrineID);
  }

  protected override int GetSubTaskCode() => this._shrineID;

  public override void ClaimReservations()
  {
    if (this._shrine == null)
      return;
    this._shrine.ReservedForTask = true;
  }

  public override void ReleaseReservations()
  {
    if (this._shrine == null)
      return;
    this._shrine.ReservedForTask = false;
  }

  protected override void OnStart()
  {
    this.ClearDestination();
    this.SetState(FollowerTaskState.GoingTo);
  }

  protected override void OnArrive() => this.SetState(FollowerTaskState.Idle);

  protected override void TaskTick(float deltaGameTime)
  {
    if (this._state != FollowerTaskState.Idle)
      return;
    this._progress += deltaGameTime * this._brain.Info.ProductivityMultiplier;
    if ((double) this._progress < (double) this.GetDurationPerDevotion(this._brain))
      return;
    this.SetState(FollowerTaskState.Doing);
  }

  public float GetDurationPerDevotion(Follower forFollower)
  {
    return this.GetDurationPerDevotion(forFollower.Brain);
  }

  public float GetDurationPerDevotion(FollowerBrain followerBrain)
  {
    float num = 150f;
    if (StructureEffectManager.GetEffectAvailability(this._shrineID, StructureEffectManager.EffectType.Shrine_DevotionEffeciency) == StructureEffectManager.State.Active)
      num *= 0.8f;
    return num / this._shrine.DevotionSpeedMultiplier / followerBrain.DevotionToGive;
  }

  private void UndoStateAnimationChanges(Follower follower)
  {
    SimpleSpineAnimator.SpineChartacterAnimationData animationData = follower.SimpleAnimator.GetAnimationData(StateMachine.State.Idle);
    animationData.Animation = animationData.DefaultAnimation;
    follower.ResetStateAnimations();
  }

  protected override Vector3 UpdateDestination(Follower follower)
  {
    return this._shrine.Data.Position + Vector3.down;
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, this._shrine.Data.FullyFueled ? "pray-flame" : "pray");
    this._progress = 0.0f;
  }

  public override void OnIdleBegin(Follower follower)
  {
    base.OnIdleBegin(follower);
    follower.UIFollowerPrayingProgress.Show();
    follower.State.facingAngle = Utils.GetAngle(follower.transform.position, this._shrine.Data.Position);
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, this._shrine.Data.FullyFueled ? "pray-flame" : "pray");
  }

  public override void OnDoingBegin(Follower follower)
  {
    base.OnDoingBegin(follower);
    follower.UIFollowerPrayingProgress.Flash();
    this._brain.GetXP(1f);
    BuildingShrinePassive shrine = this.FindShrine();
    if ((UnityEngine.Object) shrine == (UnityEngine.Object) null)
      this.DepositSoul(1);
    else
      follower.StartCoroutine((IEnumerator) this.GiveDevotionRoutine(shrine, follower));
    follower.TimedAnimation(this._shrine.Data.FullyFueled ? "devotion/devotion-collect-flame" : "devotion/devotion-collect", 2.3f, (System.Action) (() => follower.State.CURRENT_STATE = StateMachine.State.Idle));
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

  protected IEnumerator GiveDevotionRoutine(BuildingShrinePassive shrine, Follower follower)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    FollowerTask_PrayPassive followerTaskPrayPassive = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      // ISSUE: reference to a compiler-generated method
      SoulCustomTarget.Create(shrine.ReceiveSoulPosition, follower.transform.position, Color.white, new System.Action(followerTaskPrayPassive.\u003CGiveDevotionRoutine\u003Eb__35_0), 0.2f);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(0.2f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  protected virtual void DepositSoul(int DevotionToGive)
  {
    float num = 1f + (float) this._brain.Info.XPLevel;
    this._shrine.SoulCount += Mathf.RoundToInt((float) DevotionToGive * num);
    this.SetState(FollowerTaskState.Idle);
    this.Complete();
    this._shrine.SetFollowerPrayed();
  }

  protected BuildingShrinePassive FindShrine()
  {
    BuildingShrinePassive shrine1 = (BuildingShrinePassive) null;
    foreach (BuildingShrinePassive shrine2 in BuildingShrinePassive.Shrines)
    {
      if (shrine2.Structure.Structure_Info.ID == this._shrineID)
      {
        shrine1 = shrine2;
        break;
      }
    }
    return shrine1;
  }

  public override void SimDoingBegin(SimFollower simFollower) => this.DepositSoul(1);
}
