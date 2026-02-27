// Decompiled with JetBrains decompiler
// Type: RitualWedding
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using Lamb.UI;
using Lamb.UI.FollowerSelect;
using Lamb.UI.Tarot;
using src.Extensions;
using src.UI.Overlays.TutorialOverlay;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RitualWedding : Ritual
{
  private Follower contestant1;
  private FollowerTask_ManualControl Task1;
  private bool Waiting = true;
  private int followerID;
  private EventInstance loopedSound;

  protected override UpgradeSystem.Type RitualType => UpgradeSystem.Type.Ritual_Wedding;

  public override void Play()
  {
    base.Play();
    this.StartCoroutine((IEnumerator) this.RitualRoutine());
  }

  private IEnumerator RitualRoutine()
  {
    RitualWedding ritualWedding = this;
    AudioManager.Instance.PlayOneShot("event:/rituals/generic_start_ritual");
    Interaction_TempleAltar.Instance.SimpleSetCamera.Play();
    PlayerFarming.Instance.GoToAndStop(Interaction_TempleAltar.Instance.PortalEffect.transform.position + Vector3.left * 0.5f, GoToCallback: (System.Action) (() =>
    {
      Interaction_TempleAltar.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      PlayerFarming.Instance.simpleSpineAnimator.Animate("idle", 0, true);
    }));
    yield return (object) ritualWedding.StartCoroutine((IEnumerator) ritualWedding.WaitFollowersFormCircle());
    yield return (object) new WaitForSeconds(1f);
    UIFollowerSelectMenuController followerSelectInstance = MonoSingleton<UIManager>.Instance.FollowerSelectMenuTemplate.Instantiate<UIFollowerSelectMenuController>();
    followerSelectInstance.Show(Ritual.GetFollowersAvailableToAttendSermon(), followerSelectionType: ritualWedding.RitualType);
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
      this.StartCoroutine((IEnumerator) this.SetUpCombatant1Routine());
    });
    UIFollowerSelectMenuController selectMenuController2 = followerSelectInstance;
    selectMenuController2.OnShow = selectMenuController2.OnShow + (System.Action) (() =>
    {
      foreach (FollowerInformationBox followerInfoBox in followerSelectInstance.FollowerInfoBoxes)
      {
        if (followerInfoBox.followBrain.Info.MarriedToLeader)
          followerInfoBox.ShowMarried();
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

  private IEnumerator ContinueRitual()
  {
    RitualWedding ritualWedding = this;
    Interaction_TempleAltar.Instance.SimpleSetCamera.Reset();
    Interaction_TempleAltar.Instance.CloseUpCamera.Play();
    PlayerFarming.Instance.simpleSpineAnimator.Animate("build", 0, true);
    PlayerFarming.Instance.state.facingAngle = Utils.GetAngle(ritualWedding.transform.position, ritualWedding.contestant1.transform.position);
    yield return (object) new WaitForSeconds(2f);
    int WaitCount = 0;
    ++WaitCount;
    ritualWedding.Task1.GoToAndStop(ritualWedding.contestant1, Interaction_TempleAltar.Instance.PortalEffect.transform.position + Vector3.right * 0.5f, (System.Action) (() => ++WaitCount));
    while (WaitCount < 2)
      yield return (object) null;
    PlayerFarming.Instance.state.facingAngle = Utils.GetAngle(ritualWedding.transform.position, ritualWedding.contestant1.transform.position);
    PlayerFarming.Instance.simpleSpineAnimator.Animate("kiss-follower", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("dance", 0, true, 0.0f);
    double num1 = (double) ritualWedding.contestant1.SetBodyAnimation("kiss", true);
    yield return (object) new WaitForSeconds(0.8333333f);
    Vector3 position = Interaction_TempleAltar.Instance.PortalEffect.transform.position;
    BiomeConstants.Instance.EmitHeartPickUpVFX(position, 0.0f, "red", "burst_big");
    BiomeConstants.Instance.EmitConfettiVFX(position);
    CameraManager.instance.ShakeCameraForDuration(0.1f, 1f, 0.5f);
    MMVibrate.Haptic(MMVibrate.HapticTypes.SoftImpact);
    AudioManager.Instance.PlayOneShot("event:/building/building_bell_ring", PlayerFarming.Instance.transform.position);
    Interaction_TempleAltar.Instance.PulseDisplacementObject(Interaction_TempleAltar.Instance.PortalEffect.transform.position - Vector3.back);
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      if (followerBrain.CurrentTaskType == FollowerTaskType.AttendTeaching)
        (followerBrain.CurrentTask as FollowerTask_AttendRitual).Cheer();
    }
    ritualWedding.contestant1.Brain.Info.MarriedToLeader = true;
    yield return (object) new WaitForSeconds(3f);
    double num2 = (double) ritualWedding.contestant1.SetBodyAnimation("dance", true);
    int targetFollowerID = ritualWedding.contestant1.Brain.Info.ID;
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      if (followerBrain.CurrentTaskType == FollowerTaskType.AttendTeaching)
        (followerBrain.CurrentTask as FollowerTask_AttendRitual).Dance();
    }
    yield return (object) new WaitForSeconds(4f);
    if (!TarotCards.IsUnlocked(TarotCards.Card.Lovers2))
    {
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.TRINKET_CARD_UNLOCKED, 1, ritualWedding.contestant1.transform.position).GetComponent<Interaction_TarotCardUnlock>().CardOverride = TarotCards.Card.Lovers2;
      yield return (object) new WaitForSeconds(1f);
    }
    List<int> JealousSpouses = new List<int>();
    FollowerBrain.AddMarriageThoughts();
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      if (followerBrain != ritualWedding.contestant1.Brain)
      {
        if (!followerBrain.Info.MarriedToLeader)
        {
          followerBrain.AddThought(Thought.AttendedWedding);
        }
        else
        {
          followerBrain.AddThought(Thought.AttendedWeddingSpouse);
          JealousSpouses.Add(followerBrain.Info.ID);
          Debug.Log((object) "ATTENDED WEDDING AS SPOUSE!");
        }
      }
    }
    AudioManager.Instance.SetMusicBaseID(SoundConstants.BaseID.Temple);
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.Idle;
    float num3 = 0.0f;
    foreach (FollowerBrain brain in Ritual.FollowerToAttendSermon)
    {
      float Delay = UnityEngine.Random.Range(0.1f, 0.5f);
      num3 += Delay;
      ritualWedding.StartCoroutine((IEnumerator) ritualWedding.DelayFollowerReaction(brain, Delay));
    }
    AudioManager.Instance.StopLoop(ritualWedding.loopedSound);
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
    ChurchFollowerManager.Instance.RemoveBrainFromAudience(ritualWedding.contestant1.Brain);
    ritualWedding.CompleteRitual(targetFollowerID_1: targetFollowerID);
    yield return (object) new WaitForSeconds(1f);
    CultFaithManager.AddThought(Thought.Cult_Wedding, ritualWedding.followerID);
    foreach (int FollowerID in JealousSpouses)
      CultFaithManager.AddThought(Thought.Cult_Wedding_JealousSpouse, FollowerID);
  }

  private IEnumerator SetUpCombatant1Routine()
  {
    RitualWedding ritualWedding = this;
    yield return (object) null;
    ChurchFollowerManager.Instance.RemoveBrainFromAudience(ritualWedding.contestant1.Brain);
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.CurrentTaskType == FollowerTaskType.AttendTeaching)
      {
        allBrain.CurrentTask.RecalculateDestination();
        allBrain.CurrentTask.Setup(FollowerManager.FindFollowerByID(allBrain.Info.ID));
      }
    }
    // ISSUE: reference to a compiler-generated method
    ritualWedding.Task1.GoToAndStop(ritualWedding.contestant1, Interaction_TempleAltar.Instance.PortalEffect.transform.position + Vector3.right * 1f, new System.Action(ritualWedding.\u003CSetUpCombatant1Routine\u003Eb__10_0));
  }

  private IEnumerator EndRitual()
  {
    AudioManager.Instance.StopLoop(this.loopedSound);
    yield return (object) new WaitForSeconds(0.5f);
  }
}
