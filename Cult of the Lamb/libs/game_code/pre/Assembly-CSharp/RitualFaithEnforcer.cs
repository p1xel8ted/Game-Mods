// Decompiled with JetBrains decompiler
// Type: RitualFaithEnforcer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using Lamb.UI;
using Lamb.UI.FollowerSelect;
using src.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RitualFaithEnforcer : Ritual
{
  private Follower contestant1;
  private FollowerTask_ManualControl Task1;
  private bool Waiting = true;
  private EventInstance loopedSound;
  private bool waiting;

  protected override UpgradeSystem.Type RitualType => UpgradeSystem.Type.Ritual_AssignFaithEnforcer;

  public override void Play()
  {
    base.Play();
    this.StartCoroutine((IEnumerator) this.RitualRoutine());
  }

  private IEnumerator RitualRoutine()
  {
    RitualFaithEnforcer ritualFaithEnforcer = this;
    AudioManager.Instance.PlayOneShot("event:/rituals/generic_start_ritual");
    Interaction_TempleAltar.Instance.SimpleSetCamera.Play();
    yield return (object) ritualFaithEnforcer.StartCoroutine((IEnumerator) ritualFaithEnforcer.WaitFollowersFormCircle());
    yield return (object) new WaitForSeconds(1f);
    UIFollowerSelectMenuController followerSelectInstance = MonoSingleton<UIManager>.Instance.FollowerSelectMenuTemplate.Instantiate<UIFollowerSelectMenuController>();
    List<FollowerBrain> followerBrains = new List<FollowerBrain>((IEnumerable<FollowerBrain>) Ritual.GetFollowersAvailableToAttendSermon());
    for (int index = followerBrains.Count - 1; index >= 0; --index)
    {
      if (followerBrains[index].Info.CursedState != Thought.None)
        followerBrains.RemoveAt(index);
    }
    followerSelectInstance.Show(followerBrains, followerSelectionType: ritualFaithEnforcer.RitualType);
    UIFollowerSelectMenuController selectMenuController1 = followerSelectInstance;
    selectMenuController1.OnFollowerSelected = selectMenuController1.OnFollowerSelected + (Action<FollowerInfo>) (followerInfo =>
    {
      this.contestant1 = FollowerManager.FindFollowerByID(followerInfo.ID);
      AudioManager.Instance.PlayOneShot("event:/ritual_sacrifice/select_follower", PlayerFarming.Instance.gameObject);
      this.loopedSound = AudioManager.Instance.CreateLoop("event:/sermon/preach_loop", PlayerFarming.Instance.gameObject, true, false);
      this.Task1 = new FollowerTask_ManualControl();
      this.contestant1.Brain.HardSwapToTask((FollowerTask) this.Task1);
      foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
      {
        if (allBrain.Info.FaithEnforcer)
        {
          Follower followerById = FollowerManager.FindFollowerByID(allBrain.Info.ID);
          if ((bool) (UnityEngine.Object) followerById)
            followerById.SetHat(HatType.None);
        }
        allBrain.Info.FaithEnforcer = false;
      }
      GameManager.GetInstance().StartCoroutine((IEnumerator) this.ContinueRitual());
      GameManager.GetInstance().StartCoroutine((IEnumerator) this.SetUpCombatant1Routine());
    });
    UIFollowerSelectMenuController selectMenuController2 = followerSelectInstance;
    selectMenuController2.OnShow = selectMenuController2.OnShow + (System.Action) (() => { });
    UIFollowerSelectMenuController selectMenuController3 = followerSelectInstance;
    selectMenuController3.OnCancel = selectMenuController3.OnCancel + (System.Action) (() =>
    {
      AudioManager.Instance.StopLoop(this.loopedSound);
      Interaction_TempleAltar.Instance.RitualCloseSetCamera.Reset();
      GameManager.GetInstance().StartCoroutine((IEnumerator) this.EndRitual());
      this.CompleteRitual(true);
      this.CancelFollowers();
      Interaction_TempleAltar.Instance.SimpleSetCamera.Reset();
    });
    UIFollowerSelectMenuController selectMenuController4 = followerSelectInstance;
    selectMenuController4.OnHidden = selectMenuController4.OnHidden + (System.Action) (() => followerSelectInstance = (UIFollowerSelectMenuController) null);
  }

  private IEnumerator ContinueRitual()
  {
    RitualFaithEnforcer ritualFaithEnforcer = this;
    AudioManager.Instance.PlayOneShot("event:/ritual_sacrifice/select_follower", PlayerFarming.Instance.gameObject);
    PlayerFarming.Instance.simpleSpineAnimator.Animate("build", 0, true);
    PlayerFarming.Instance.state.facingAngle = Utils.GetAngle(ritualFaithEnforcer.transform.position, ritualFaithEnforcer.contestant1.transform.position);
    yield return (object) new WaitForSeconds(1.5f);
    PlayerFarming.Instance.simpleSpineAnimator.Animate("rituals/ritual-start", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("rituals/ritual-loop", 0, true, 0.0f);
    Interaction_TempleAltar.Instance.CloseUpCamera.Play();
    yield return (object) new WaitForSeconds(0.8333333f);
    Interaction_TempleAltar.Instance.PulseDisplacementObject(Interaction_TempleAltar.Instance.PortalEffect.transform.position - Vector3.back);
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      if (followerBrain.CurrentTaskType == FollowerTaskType.AttendTeaching)
        (followerBrain.CurrentTask as FollowerTask_AttendRitual).Pray(2);
    }
    ChurchFollowerManager.Instance.GodRays.gameObject.SetActive(true);
    ChurchFollowerManager.Instance.GodRays.GetComponent<ParticleSystem>().Play();
    yield return (object) new WaitForSeconds(0.5f);
    ritualFaithEnforcer.waiting = true;
    // ISSUE: reference to a compiler-generated method
    ritualFaithEnforcer.contestant1.GoTo(ChurchFollowerManager.Instance.RitualCenterPosition.position, new System.Action(ritualFaithEnforcer.\u003CContinueRitual\u003Eb__9_0));
    while (ritualFaithEnforcer.waiting)
      yield return (object) null;
    double num1 = (double) ritualFaithEnforcer.contestant1.SetBodyAnimation("Conversations/react-love3", false);
    BiomeConstants.Instance.EmitSmokeExplosionVFX(ritualFaithEnforcer.contestant1.transform.position + Vector3.back);
    AudioManager.Instance.PlayOneShot("event:/Stings/Choir_Short");
    ritualFaithEnforcer.contestant1.Brain.Info.FaithEnforcer = true;
    ritualFaithEnforcer.contestant1.SetOutfit(ritualFaithEnforcer.contestant1.Outfit.CurrentOutfit, false);
    yield return (object) new WaitForSeconds(2f);
    for (int i = 0; i < 2; ++i)
    {
      bool waiting = true;
      ritualFaithEnforcer.contestant1.GoTo(ChurchFollowerManager.Instance.RitualCenterPosition.position + (Vector3) UnityEngine.Random.insideUnitCircle * 1.85f, (System.Action) (() => waiting = false));
      while (waiting)
        yield return (object) null;
      string animName = "Reactions/react-determined1";
      switch (UnityEngine.Random.Range(0, 3))
      {
        case 0:
          animName = "Reactions/react-determined2";
          break;
        case 1:
          animName = "Reactions/react-non-believers";
          break;
      }
      double num2 = (double) ritualFaithEnforcer.contestant1.SetBodyAnimation(animName, false);
      yield return (object) new WaitForSeconds(1f);
    }
    yield return (object) new WaitForSeconds(0.5f);
    ChurchFollowerManager.Instance.GodRays.GetComponent<ParticleSystem>().Stop();
    PlayerFarming.Instance.simpleSpineAnimator.Animate("rituals/ritual-stop", 0, false);
    Interaction_TempleAltar.Instance.SimpleSetCamera.Reset();
    foreach (FollowerBrain brain in Ritual.FollowerToAttendSermon)
    {
      brain.CompleteCurrentTask();
      float Delay = UnityEngine.Random.Range(0.1f, 0.5f);
      ritualFaithEnforcer.StartCoroutine((IEnumerator) ritualFaithEnforcer.DelayFollowerReaction(brain, Delay));
      Follower followerById = FollowerManager.FindFollowerByID(brain.Info.ID);
      if ((bool) (UnityEngine.Object) followerById)
        followerById.Spine.randomOffset = false;
    }
    AudioManager.Instance.StopLoop(ritualFaithEnforcer.loopedSound);
    yield return (object) new WaitForSeconds(0.5f);
    ChurchFollowerManager.Instance.AddBrainToAudience(ritualFaithEnforcer.contestant1.Brain);
    ritualFaithEnforcer.CompleteRitual(targetFollowerID_1: ritualFaithEnforcer.contestant1.Brain.Info.ID);
    yield return (object) new WaitForSeconds(1f);
    CultFaithManager.AddThought(Thought.Cult_FaithEnforcer);
    ChurchFollowerManager.Instance.GodRays.gameObject.SetActive(false);
  }

  private IEnumerator SetUpCombatant1Routine()
  {
    RitualFaithEnforcer ritualFaithEnforcer = this;
    yield return (object) null;
    ChurchFollowerManager.Instance.RemoveBrainFromAudience(ritualFaithEnforcer.contestant1.Brain);
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.CurrentTaskType == FollowerTaskType.AttendTeaching)
      {
        allBrain.CurrentTask.RecalculateDestination();
        allBrain.CurrentTask.Setup(FollowerManager.FindFollowerByID(allBrain.Info.ID));
      }
    }
    // ISSUE: reference to a compiler-generated method
    ritualFaithEnforcer.Task1.GoToAndStop(ritualFaithEnforcer.contestant1, ChurchFollowerManager.Instance.RitualCenterPosition.position, new System.Action(ritualFaithEnforcer.\u003CSetUpCombatant1Routine\u003Eb__10_0));
  }

  private IEnumerator EndRitual()
  {
    AudioManager.Instance.StopLoop(this.loopedSound);
    yield return (object) new WaitForSeconds(1f);
  }
}
