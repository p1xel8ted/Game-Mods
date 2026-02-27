// Decompiled with JetBrains decompiler
// Type: RitualReindoctrinate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Lamb.UI.FollowerSelect;
using src.Extensions;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class RitualReindoctrinate : Ritual
{
  private Follower sacrificeFollower;
  public Light RitualLight;
  private UIFollowerSelectMenuController _followerSelectTemplate;

  protected override UpgradeSystem.Type RitualType => UpgradeSystem.Type.Ritual_Reindoctrinate;

  public override void Play()
  {
    base.Play();
    this.StartCoroutine((IEnumerator) this.SacrificeFollowerRoutine());
  }

  private IEnumerator SacrificeFollowerRoutine()
  {
    RitualReindoctrinate ritualReindoctrinate = this;
    Interaction_TempleAltar.Instance.SimpleSetCamera.Play();
    PlayerFarming.Instance.GoToAndStop(ChurchFollowerManager.Instance.RitualCenterPosition.position + Vector3.left * 0.5f, ChurchFollowerManager.Instance.RitualCenterPosition.gameObject, GoToCallback: (System.Action) (() =>
    {
      Interaction_TempleAltar.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      PlayerFarming.Instance.simpleSpineAnimator.Animate("build", 0, true);
    }));
    yield return (object) ritualReindoctrinate.StartCoroutine((IEnumerator) ritualReindoctrinate.WaitFollowersFormCircle());
    yield return (object) new WaitForSeconds(1f);
    UIFollowerSelectMenuController followerSelectInstance = ritualReindoctrinate._followerSelectTemplate.Instantiate<UIFollowerSelectMenuController>();
    followerSelectInstance.Show(Ritual.GetFollowersAvailableToAttendSermon(), followerSelectionType: ritualReindoctrinate.RitualType);
    UIFollowerSelectMenuController selectMenuController1 = followerSelectInstance;
    selectMenuController1.OnFollowerSelected = selectMenuController1.OnFollowerSelected + (Action<FollowerInfo>) (followerInfo => this.sacrificeFollower = FollowerManager.FindFollowerByID(followerInfo.ID));
    UIFollowerSelectMenuController selectMenuController2 = followerSelectInstance;
    selectMenuController2.OnCancel = selectMenuController2.OnCancel + (System.Action) (() =>
    {
      this.EndRitual();
      this.CompleteRitual(true);
      this.CancelFollowers();
      Interaction_TempleAltar.Instance.SimpleSetCamera.Reset();
    });
    UIFollowerSelectMenuController selectMenuController3 = followerSelectInstance;
    selectMenuController3.OnHidden = selectMenuController3.OnHidden + (System.Action) (() => followerSelectInstance = (UIFollowerSelectMenuController) null);
    while ((UnityEngine.Object) followerSelectInstance != (UnityEngine.Object) null)
      yield return (object) null;
    ritualReindoctrinate.sacrificeFollower.Brain.CompleteCurrentTask();
    FollowerTask_ManualControl Task = new FollowerTask_ManualControl();
    ritualReindoctrinate.sacrificeFollower.Brain.HardSwapToTask((FollowerTask) Task);
    ritualReindoctrinate.sacrificeFollower.Brain.InRitual = true;
    Ritual.FollowerToAttendSermon.Remove(ritualReindoctrinate.sacrificeFollower.Brain);
    yield return (object) null;
    bool WaitForFollower = true;
    ritualReindoctrinate.sacrificeFollower.HoodOff(onComplete: (System.Action) (() =>
    {
      ChurchFollowerManager.Instance.RemoveBrainFromAudience(this.sacrificeFollower.Brain);
      foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
      {
        if (allBrain.CurrentTaskType == FollowerTaskType.AttendTeaching)
        {
          allBrain.CurrentTask.RecalculateDestination();
          allBrain.CurrentTask.Setup(FollowerManager.FindFollowerByID(allBrain.Info.ID));
        }
      }
      Task.GoToAndStop(this.sacrificeFollower, PlayerFarming.Instance.transform.position + Vector3.right, (System.Action) (() =>
      {
        WaitForFollower = false;
        this.sacrificeFollower.State.LookAngle = this.sacrificeFollower.State.facingAngle = Utils.GetAngle(this.transform.position, PlayerFarming.Instance.transform.position);
      }));
    }));
    Interaction_TempleAltar.Instance.SimpleSetCamera.Reset();
    GameManager.GetInstance().OnConversationNext(ritualReindoctrinate.sacrificeFollower.gameObject, 8f);
    GameManager.GetInstance().AddPlayerToCamera();
    while (WaitForFollower)
      yield return (object) null;
    GameManager.GetInstance().OnConversationNext(ritualReindoctrinate.sacrificeFollower.gameObject, 10f);
    GameManager.GetInstance().AddPlayerToCamera();
    FollowerRecruit FollowerRecruit = ritualReindoctrinate.sacrificeFollower.gameObject.AddComponent<FollowerRecruit>();
    FollowerRecruit.Follower = ritualReindoctrinate.sacrificeFollower;
    FollowerRecruit.CameraBone = ritualReindoctrinate.sacrificeFollower.CameraBone;
    FollowerRecruit.RecruitOnComplete = false;
    FollowerRecruit.triggered = true;
    FollowerRecruit.DoRecruit(PlayerFarming.Instance.state, true, false);
    while ((UnityEngine.Object) FollowerRecruit != (UnityEngine.Object) null)
      yield return (object) null;
    double num = (double) ritualReindoctrinate.sacrificeFollower.SetBodyAnimation("cheer", true);
    foreach (FollowerBrain followerBrain in Ritual.GetFollowersAvailableToAttendSermon())
    {
      if (followerBrain.CurrentTask is FollowerTask_AttendRitual)
        (followerBrain.CurrentTask as FollowerTask_AttendRitual).Cheer();
    }
    yield return (object) new WaitForSeconds(2f);
    ritualReindoctrinate.sacrificeFollower.Brain.CompleteCurrentTask();
    ChurchFollowerManager.Instance.RemoveBrainFromAudience(ritualReindoctrinate.sacrificeFollower.Brain);
    ritualReindoctrinate.EndRitual();
    yield return (object) new WaitForSeconds(0.5f);
    ritualReindoctrinate.CompleteRitual(targetFollowerID_1: ritualReindoctrinate.sacrificeFollower.Brain.Info.ID);
  }

  private void EndRitual()
  {
    ChurchFollowerManager.Instance.EndRitualOverlay();
    foreach (FollowerBrain followerBrain in Ritual.GetFollowersAvailableToAttendSermon())
      followerBrain.CompleteCurrentTask();
  }
}
