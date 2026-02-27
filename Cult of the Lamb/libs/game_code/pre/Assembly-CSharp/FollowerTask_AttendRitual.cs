// Decompiled with JetBrains decompiler
// Type: FollowerTask_AttendRitual
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class FollowerTask_AttendRitual : FollowerTask
{
  private Coroutine hoodOnRoutine;
  private Follower follower;
  public FollowerOutfitType SpecialOutfit = FollowerOutfitType.Custom;
  public bool DestinationIsCircle = true;

  public override FollowerTaskType Type => FollowerTaskType.AttendTeaching;

  public override FollowerLocation Location => FollowerLocation.Church;

  public override bool BlockReactTasks => true;

  public override bool BlockTaskChanges => true;

  public override bool BlockSermon => false;

  protected override int GetSubTaskCode() => 0;

  protected override float SatiationChange(float deltaGameTime) => 0.0f;

  protected override void OnEnd() => base.OnEnd();

  protected override void TaskTick(float deltaGameTime)
  {
    if (PlayerFarming.Location != FollowerLocation.Church)
    {
      this.End();
    }
    else
    {
      if (this._brain.Location == this.Location)
        return;
      this.Start();
    }
  }

  protected override Vector3 UpdateDestination(Follower follower)
  {
    return this.DestinationIsCircle ? ChurchFollowerManager.Instance.GetCirclePosition(this._brain) : ChurchFollowerManager.Instance.GetAudienceMemberPosition(this._brain);
  }

  public void FormCircle()
  {
    this.DestinationIsCircle = true;
    this.RecalculateDestination();
  }

  public void HoodOff() => this.follower.HoodOff();

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    this.SetState(FollowerTaskState.GoingTo);
    this._brain.InRitual = true;
    this.follower = follower;
  }

  public override void OnDoingBegin(Follower follower)
  {
    if (!(bool) (Object) follower)
      return;
    follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    follower.FacePosition(ChurchFollowerManager.Instance.RitualCenterPosition.transform.position);
    this.hoodOnRoutine = GameManager.GetInstance().StartCoroutine((IEnumerator) this.DelayedHood(follower));
  }

  public override void OnGoingToBegin(Follower follower) => base.OnGoingToBegin(follower);

  private IEnumerator DelayedHood(Follower follower)
  {
    yield return (object) new WaitForSeconds(0.35f);
    if ((bool) (Object) follower)
      follower.HoodOn("pray", false);
  }

  public void WorshipTentacle()
  {
    Follower followerById = FollowerManager.FindFollowerByID(this._brain.Info.ID);
    if (!(bool) (Object) followerById)
      return;
    double num = (double) followerById.SetBodyAnimation("worship", true);
  }

  public void Cheer()
  {
    Follower followerById = FollowerManager.FindFollowerByID(this._brain.Info.ID);
    if (!(bool) (Object) followerById)
      return;
    double num = (double) followerById.SetBodyAnimation("cheer", true);
  }

  public void Boo()
  {
    Follower followerById = FollowerManager.FindFollowerByID(this._brain.Info.ID);
    if (!(bool) (Object) followerById)
      return;
    double num = (double) followerById.SetBodyAnimation("boo", true);
  }

  public void Dance()
  {
    Follower followerById = FollowerManager.FindFollowerByID(this._brain.Info.ID);
    if (!(bool) (Object) followerById)
      return;
    double num = (double) followerById.SetBodyAnimation("dance", true);
  }

  public void DanceBrainwashed()
  {
    Follower followerById = FollowerManager.FindFollowerByID(this._brain.Info.ID);
    if (!(bool) (Object) followerById)
      return;
    double num = (double) followerById.SetBodyAnimation("dance-mushroom", true);
  }

  public void Pray(int loopCount = 1)
  {
    Follower followerById = FollowerManager.FindFollowerByID(this._brain.Info.ID);
    if (!(bool) (Object) followerById)
      return;
    followerById.Spine.randomOffset = true;
    for (int index = 0; index < loopCount; ++index)
    {
      followerById.AddBodyAnimation("devotion/devotion-start", false, 0.0f);
      followerById.AddBodyAnimation(Random.Range(0, 2) == 0 ? "devotion/devotion-collect" : "devotion/devotion-collect2", false, 0.0f);
      followerById.AddBodyAnimation("pray", true, 0.0f);
    }
  }

  public void Pray2()
  {
    Follower followerById = FollowerManager.FindFollowerByID(this._brain.Info.ID);
    if (!(bool) (Object) followerById)
      return;
    double num = (double) followerById.SetBodyAnimation("devotion/devotion-start", false);
    followerById.AddBodyAnimation("devotion/devotion-waiting", false, 0.0f);
    followerById.AddBodyAnimation(Random.Range(0, 2) == 0 ? "devotion/devotion-collect" : "devotion/devotion-collect2", false, 0.0f);
    followerById.AddBodyAnimation("pray", true, 0.0f);
  }

  public void Pray3()
  {
    Follower followerById = FollowerManager.FindFollowerByID(this._brain.Info.ID);
    if (!(bool) (Object) followerById)
      return;
    followerById.AddBodyAnimation("pray", true, 0.0f);
  }

  public void Idle()
  {
    Follower followerById = FollowerManager.FindFollowerByID(this._brain.Info.ID);
    if (!(bool) (Object) followerById)
      return;
    double num = (double) followerById.SetBodyAnimation("idle", true);
  }

  public override void OnFinaliseBegin(Follower follower) => base.OnFinaliseBegin(follower);

  protected override void OnComplete() => base.OnComplete();

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    this._brain.InRitual = false;
    if (!(bool) (Object) follower)
      return;
    follower.State.CURRENT_STATE = StateMachine.State.Idle;
    follower.SetOutfit(follower.Brain.Info.Outfit, false);
    double num = (double) follower.SetBodyAnimation("idle", true);
    if (this.SpecialOutfit == FollowerOutfitType.Custom)
      return;
    follower.Outfit.SetOutfit(follower.Spine, this.SpecialOutfit, follower.Brain.Info.Necklace, false);
  }

  public override void SimCleanup(SimFollower simFollower)
  {
    this._brain.InRitual = false;
    base.SimCleanup(simFollower);
  }
}
