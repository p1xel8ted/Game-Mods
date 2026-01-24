// Decompiled with JetBrains decompiler
// Type: RitualDivorce
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using Lamb.UI.FollowerSelect;
using src.Extensions;
using src.UI.Overlays.TutorialOverlay;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RitualDivorce : Ritual
{
  public Follower contestant1;
  public FollowerTask_ManualControl Task1;
  public int followerID;
  public bool hasFancyRobes;
  public bool hasFancySuit;

  public override UpgradeSystem.Type RitualType => UpgradeSystem.Type.Ritual_Divorce;

  public override void Play()
  {
    base.Play();
    this.StartCoroutine((IEnumerator) this.RitualRoutine());
  }

  public IEnumerator RitualRoutine()
  {
    RitualDivorce ritualDivorce = this;
    AudioManager.Instance.PlayOneShot("event:/rituals/generic_start_ritual");
    Interaction_TempleAltar.Instance.SimpleSetCamera.Play();
    PlayerFarming.Instance.GoToAndStop(Interaction_TempleAltar.Instance.PortalEffect.transform.position + Vector3.left * 0.5f, Interaction_TempleAltar.Instance.PortalEffect.gameObject, GoToCallback: (System.Action) (() =>
    {
      Interaction_TempleAltar.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      PlayerFarming.Instance.simpleSpineAnimator.Animate("idle", 0, true);
    }));
    yield return (object) ritualDivorce.StartCoroutine((IEnumerator) ritualDivorce.WaitFollowersFormCircle());
    yield return (object) new WaitForSeconds(1f);
    List<FollowerSelectEntry> followerSelectEntries = new List<FollowerSelectEntry>();
    foreach (FollowerBrain followerBrain in Ritual.GetFollowersAvailableToAttendSermon())
    {
      if (followerBrain.Info.MarriedToLeader)
        followerSelectEntries.Add(new FollowerSelectEntry(followerBrain));
      else
        followerSelectEntries.Add(new FollowerSelectEntry(followerBrain, FollowerSelectEntry.Status.Unavailable));
    }
    UIFollowerSelectMenuController followerSelectInstance = MonoSingleton<UIManager>.Instance.FollowerSelectMenuTemplate.Instantiate<UIFollowerSelectMenuController>();
    followerSelectInstance.VotingType = TwitchVoting.VotingType.RITUAL_DIVORCE;
    followerSelectInstance.Show(followerSelectEntries, followerSelectionType: ritualDivorce.RitualType);
    UIFollowerSelectMenuController selectMenuController1 = followerSelectInstance;
    selectMenuController1.OnFollowerSelected = selectMenuController1.OnFollowerSelected + (Action<FollowerInfo>) (followerInfo =>
    {
      AudioManager.Instance.SetMusicBaseID(SoundConstants.BaseID.bongos_singing);
      this.followerID = followerInfo.ID;
      this.contestant1 = FollowerManager.FindFollowerByID(followerInfo.ID);
      AudioManager.Instance.PlayOneShot("event:/rituals/wedding_select_follower", PlayerFarming.Instance.gameObject);
      this.Task1 = new FollowerTask_ManualControl();
      this.contestant1.Brain.HardSwapToTask((FollowerTask) this.Task1);
      GameManager.GetInstance().StartCoroutine((IEnumerator) this.ContinueRitual());
    });
    UIFollowerSelectMenuController selectMenuController2 = followerSelectInstance;
    selectMenuController2.OnShownCompleted = selectMenuController2.OnShownCompleted + (System.Action) (() =>
    {
      foreach (FollowerInformationBox followerInfoBox in followerSelectInstance.FollowerInfoBoxes)
      {
        if (followerInfoBox.followBrain.Info.MarriedToLeader)
          followerInfoBox.ShowMarried(true);
      }
    });
    UIFollowerSelectMenuController selectMenuController3 = followerSelectInstance;
    selectMenuController3.OnCancel = selectMenuController3.OnCancel + (System.Action) (() =>
    {
      Interaction_TempleAltar.Instance.SimpleSetCamera.Reset();
      GameManager.GetInstance().StartCoroutine((IEnumerator) this.EndRitual());
      this.CancelFollowers();
      AudioManager.Instance.StopLoop(this.loopedSound);
      this.CompleteRitual(true);
    });
    UIFollowerSelectMenuController selectMenuController4 = followerSelectInstance;
    selectMenuController4.OnHidden = selectMenuController4.OnHidden + (System.Action) (() => followerSelectInstance = (UIFollowerSelectMenuController) null);
  }

  public IEnumerator ContinueRitual()
  {
    RitualDivorce ritualDivorce = this;
    Interaction_TempleAltar.Instance.SimpleSetCamera.Reset();
    Interaction_TempleAltar.Instance.CloseUpCamera.Play();
    PlayerFarming.Instance.simpleSpineAnimator.Animate("idle", 0, true);
    PlayerFarming.Instance.state.facingAngle = Utils.GetAngle(ritualDivorce.transform.position, ritualDivorce.contestant1.transform.position);
    yield return (object) ritualDivorce.StartCoroutine((IEnumerator) ritualDivorce.SetUpCombatant1Routine());
    PlayerFarming.Instance.simpleSpineAnimator.Animate("bleat", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("reactions/react-happy", 0, false, 0.0f);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    PlayerFarming.Instance.state.facingAngle = Utils.GetAngle(ritualDivorce.transform.position, ritualDivorce.contestant1.transform.position);
    GameObject ring = ChurchFollowerManager.Instance.Ring.gameObject;
    ring.SetActive(true);
    yield return (object) new WaitForSeconds(2.36666656f);
    ring.SetActive(false);
    Vector3 position = Interaction_TempleAltar.Instance.PortalEffect.transform.position;
    CameraManager.instance.ShakeCameraForDuration(0.1f, 1f, 0.5f);
    MMVibrate.Haptic(MMVibrate.HapticTypes.SoftImpact);
    AudioManager.Instance.PlayOneShot("event:/building/building_bell_ring", PlayerFarming.Instance.transform.position);
    Interaction_TempleAltar.Instance.PulseDisplacementObject(Interaction_TempleAltar.Instance.PortalEffect.transform.position - Vector3.back);
    if (ritualDivorce.contestant1.Brain.HasTrait(FollowerTrait.TraitType.MarriedUnhappily))
    {
      double num1 = (double) ritualDivorce.contestant1.SetBodyAnimation("Reactions/react-happy1", false);
    }
    else if (ritualDivorce.contestant1.Brain.HasTrait(FollowerTrait.TraitType.MarriedHappily) || ritualDivorce.contestant1.Brain.HasTrait(FollowerTrait.TraitType.MarriedDevoted))
    {
      double num2 = (double) ritualDivorce.contestant1.SetBodyAnimation("Reactions/react-cry", false);
    }
    else if (ritualDivorce.contestant1.Brain.HasTrait(FollowerTrait.TraitType.MarriedJealous) || ritualDivorce.contestant1.Brain.HasTrait(FollowerTrait.TraitType.MarriedMurderouslyJealous))
    {
      double num3 = (double) ritualDivorce.contestant1.SetBodyAnimation("Reactions/react-non-believers", false);
    }
    else
    {
      double num4 = (double) ritualDivorce.contestant1.SetBodyAnimation("Conversations/react-mean" + UnityEngine.Random.Range(1, 4).ToString(), false);
    }
    if ((double) UnityEngine.Random.value < 0.2)
    {
      ritualDivorce.contestant1.Brain.AddTrait(FollowerTrait.TraitType.JiltedLover);
      ritualDivorce.contestant1.Brain.MakeDissenter();
    }
    ritualDivorce.contestant1.AddBodyAnimation("idle", true, 0.0f);
    yield return (object) new WaitForSeconds(4f);
    ritualDivorce.contestant1.Brain.Info.MarriedToLeader = false;
    ritualDivorce.contestant1.Brain.RemoveTrait(FollowerTrait.TraitType.MarriedDevoted, true);
    ritualDivorce.contestant1.Brain.RemoveTrait(FollowerTrait.TraitType.MarriedHappily, true);
    ritualDivorce.contestant1.Brain.RemoveTrait(FollowerTrait.TraitType.MarriedJealous, true);
    ritualDivorce.contestant1.Brain.RemoveTrait(FollowerTrait.TraitType.MarriedMurderouslyJealous, true);
    ritualDivorce.contestant1.Brain.RemoveTrait(FollowerTrait.TraitType.MarriedUnhappily, true);
    AudioManager.Instance.SetMusicBaseID(SoundConstants.BaseID.Temple);
    PlayerFarming.SetStateForAllPlayers();
    float num5 = 0.0f;
    foreach (FollowerBrain brain in Ritual.FollowerToAttendSermon)
    {
      float Delay = UnityEngine.Random.Range(0.1f, 0.5f);
      num5 += Delay;
      ritualDivorce.StartCoroutine((IEnumerator) ritualDivorce.DelayFollowerReaction(brain, Delay));
    }
    AudioManager.Instance.StopLoop(ritualDivorce.loopedSound);
    Interaction_TempleAltar.Instance.CloseUpCamera.Reset();
    JudgementMeter.ShowModify(1);
    yield return (object) new WaitForSeconds(0.5f);
    if (DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.Wedding))
    {
      UITutorialOverlayController tutorialOverlay = MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.Wedding);
      while ((UnityEngine.Object) tutorialOverlay != (UnityEngine.Object) null)
        yield return (object) null;
      tutorialOverlay = (UITutorialOverlayController) null;
    }
    ChurchFollowerManager.Instance.RemoveBrainFromAudience(ritualDivorce.contestant1.Brain);
    ritualDivorce.CompleteRitual(targetFollowerID_1: ritualDivorce.contestant1.Brain.Info.ID);
    yield return (object) new WaitForSeconds(1f);
    CultFaithManager.AddThought(Thought.Cult_DivorceRitual, ritualDivorce.followerID);
  }

  public IEnumerator SetUpCombatant1Routine()
  {
    yield return (object) null;
    ChurchFollowerManager.Instance.RemoveBrainFromAudience(this.contestant1.Brain);
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.CurrentTaskType == FollowerTaskType.AttendTeaching)
      {
        allBrain.CurrentTask.RecalculateDestination();
        allBrain.CurrentTask.Setup(FollowerManager.FindFollowerByID(allBrain.Info.ID));
      }
    }
    bool isAtDestination = false;
    this.Task1.GoToAndStop(this.contestant1, Interaction_TempleAltar.Instance.PortalEffect.transform.position + Vector3.right * 0.5f, (System.Action) (() =>
    {
      this.contestant1.FacePosition(Interaction_TempleAltar.Instance.PortalEffect.transform.position);
      double num = (double) this.contestant1.SetBodyAnimation("idle", true);
      isAtDestination = true;
    }));
    while (!isAtDestination)
      yield return (object) null;
  }

  public IEnumerator EndRitual()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    RitualDivorce ritualDivorce = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    AudioManager.Instance.StopLoop(ritualDivorce.loopedSound);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(0.5f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }
}
