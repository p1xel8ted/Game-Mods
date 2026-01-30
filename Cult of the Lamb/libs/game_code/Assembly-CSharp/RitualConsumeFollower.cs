// Decompiled with JetBrains decompiler
// Type: RitualConsumeFollower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI;
using Lamb.UI.FollowerSelect;
using Spine;
using src.Extensions;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class RitualConsumeFollower : Ritual
{
  public Follower sacrificeFollower;
  public int NumGivingDevotion;

  public override UpgradeSystem.Type RitualType => UpgradeSystem.Type.Ritual_ConsumeFollower;

  public override void Play()
  {
    base.Play();
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.HeartsOfTheFaithfulRitual());
  }

  public IEnumerator HeartsOfTheFaithfulRitual()
  {
    RitualConsumeFollower ritualConsumeFollower = this;
    Interaction_TempleAltar.Instance.SimpleSetCamera.Play();
    PlayerFarming.Instance.GoToAndStop(ChurchFollowerManager.Instance.RitualCenterPosition.position + Vector3.left * 0.75f, ChurchFollowerManager.Instance.RitualCenterPosition.gameObject, GoToCallback: (System.Action) (() =>
    {
      Interaction_TempleAltar.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      PlayerFarming.Instance.simpleSpineAnimator.Animate("build", 0, true);
      PlayerFarming.Instance.state.transform.DOMove(ChurchFollowerManager.Instance.RitualCenterPosition.position + Vector3.left * 0.75f, 0.1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      if (!PlayerFarming.Instance.isLamb || PlayerFarming.Instance.IsGoat)
        AudioManager.Instance.PlayOneShot("event:/sermon/goat_sermon_speech_bubble", PlayerFarming.Instance.transform.position);
      else
        AudioManager.Instance.PlayOneShot("event:/sermon/sermon_speech_bubble", PlayerFarming.Instance.transform.position);
    }));
    yield return (object) ritualConsumeFollower.StartCoroutine((IEnumerator) ritualConsumeFollower.WaitFollowersFormCircle());
    yield return (object) new WaitForSeconds(1f);
    UIFollowerSelectMenuController followerSelectInstance = MonoSingleton<UIManager>.Instance.FollowerSelectMenuTemplate.Instantiate<UIFollowerSelectMenuController>();
    followerSelectInstance.Show(Ritual.GetFollowerSelectEntriesForSermon(), followerSelectionType: ritualConsumeFollower.RitualType);
    UIFollowerSelectMenuController selectMenuController1 = followerSelectInstance;
    selectMenuController1.OnFollowerSelected = selectMenuController1.OnFollowerSelected + (Action<FollowerInfo>) (followerInfo =>
    {
      AudioManager.Instance.PlayOneShot("event:/ritual_sacrifice/select_follower", PlayerFarming.Instance.gameObject);
      this.sacrificeFollower = FollowerManager.FindFollowerByID(followerInfo.ID);
    });
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
    ritualConsumeFollower.sacrificeFollower.Brain.CompleteCurrentTask();
    FollowerTask_ManualControl Task = new FollowerTask_ManualControl();
    ritualConsumeFollower.sacrificeFollower.Brain.HardSwapToTask((FollowerTask) Task);
    ritualConsumeFollower.sacrificeFollower.Brain.InRitual = true;
    Ritual.FollowerToAttendSermon.Remove(ritualConsumeFollower.sacrificeFollower.Brain);
    yield return (object) null;
    bool WaitForFollower = true;
    ritualConsumeFollower.sacrificeFollower.HoodOff(onComplete: (System.Action) (() =>
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
      Task.GoToAndStop(this.sacrificeFollower, PlayerFarming.Instance.transform.position + Vector3.right * 1.75f, (System.Action) (() => WaitForFollower = false));
    }));
    Interaction_TempleAltar.Instance.SimpleSetCamera.Reset();
    GameManager.GetInstance().OnConversationNext(ritualConsumeFollower.sacrificeFollower.gameObject, 8f);
    GameManager.GetInstance().AddPlayerToCamera();
    while (WaitForFollower)
      yield return (object) null;
    GameManager.GetInstance().OnConversationNext(ritualConsumeFollower.sacrificeFollower.gameObject, 12f);
    GameManager.GetInstance().AddPlayerToCamera();
    PlayerFarming.Instance.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(ritualConsumeFollower.HandleEvent);
    PlayerFarming.Instance.simpleSpineAnimator.Animate("sacrifice", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    double num = (double) ritualConsumeFollower.sacrificeFollower.SetBodyAnimation("sacrifice", false);
    ritualConsumeFollower.sacrificeFollower.State.LookAngle = ritualConsumeFollower.sacrificeFollower.State.facingAngle = Utils.GetAngle(ritualConsumeFollower.transform.position, PlayerFarming.Instance.transform.position);
    ChurchFollowerManager.Instance.StartRitualOverlay();
    DOTween.To((DOGetter<float>) (() => GameManager.GetInstance().CamFollowTarget.targetDistance), (DOSetter<float>) (x => GameManager.GetInstance().CamFollowTarget.targetDistance = x), 4f, 1f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InOutSine).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    yield return (object) new WaitForSeconds(3.23333335f);
    Interaction_TempleAltar.Instance.PulseDisplacementObject(ritualConsumeFollower.sacrificeFollower.transform.position);
    CameraManager.instance.ShakeCameraForDuration(1.5f, 2f, 0.75f);
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      if (followerBrain.CurrentTask is FollowerTask_AttendRitual)
        (followerBrain.CurrentTask as FollowerTask_AttendRitual).Cheer();
    }
    Vector3 position = ritualConsumeFollower.sacrificeFollower.transform.position;
    if (!ritualConsumeFollower.sacrificeFollower.Brain.Info.IsSnowman)
    {
      for (int i = 0; i < UnityEngine.Random.Range(3, 4); ++i)
      {
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.FOLLOWER_MEAT, 1, position);
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BONE, 1, position);
        yield return (object) new WaitForSeconds(0.2f);
      }
    }
    PlayerFarming.Instance.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(ritualConsumeFollower.HandleEvent);
    int followerID = ritualConsumeFollower.sacrificeFollower.Brain.Info.ID;
    FollowerManager.FollowerDie(ritualConsumeFollower.sacrificeFollower.Brain.Info.ID, NotificationCentre.NotificationType.ConsumeFollower);
    for (int index = 0; index < 20; ++index)
      SoulCustomTarget.Create(PlayerFarming.Instance.gameObject, ritualConsumeFollower.sacrificeFollower.gameObject.transform.position, Color.white, (System.Action) null);
    UnityEngine.Object.Destroy((UnityEngine.Object) ritualConsumeFollower.sacrificeFollower.gameObject);
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().CamFollowTarget.targetDistance += 2f;
    ritualConsumeFollower.EndRitual();
    HUD_Manager.Instance.XPBarTransitions.gameObject.SetActive(true);
    HUD_Manager.Instance.XPBarTransitions.MoveBackInFunction();
    yield return (object) new WaitForSeconds(1f);
    int Delta = Mathf.CeilToInt((float) (DataManager.GetTargetXP(Mathf.Min(DataManager.Instance.Level, DataManager.TargetXP.Count - 1)) - DataManager.Instance.XP));
    PlayerFarming.Instance.GetXP((float) Delta);
    yield return (object) new WaitForSeconds(1.5f);
    ChurchFollowerManager.Instance.EndRitualOverlay();
    HUD_Manager.Instance.XPBarTransitions.MoveBackOutFunction();
    yield return (object) new WaitForSeconds(0.5f);
    ritualConsumeFollower.CompleteRitual(targetFollowerID_1: followerID);
    yield return (object) new WaitForSeconds(1f);
    CultFaithManager.AddThought(Thought.Cult_ConsumeFollower, followerID);
  }

  public void HandleEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (!(e.Data.Name == "sfxTrigger"))
      return;
    AudioManager.Instance.PlayOneShot("event:/rituals/consume_follower", PlayerFarming.Instance.transform.position);
  }

  public void EndRitual()
  {
    PlayerFarming.Instance.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
      followerBrain.CompleteCurrentTask();
  }
}
