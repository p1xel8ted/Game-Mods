// Decompiled with JetBrains decompiler
// Type: RitualFaithEnforcer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using Lamb.UI.FollowerSelect;
using src.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class RitualFaithEnforcer : Ritual
{
  public Follower contestant1;
  public FollowerTask_ManualControl Task1;
  public bool Waiting = true;
  public bool waiting;

  public override UpgradeSystem.Type RitualType => UpgradeSystem.Type.Ritual_AssignFaithEnforcer;

  public override void Play()
  {
    base.Play();
    this.StartCoroutine((IEnumerator) this.RitualRoutine());
  }

  public IEnumerator RitualRoutine()
  {
    RitualFaithEnforcer ritualFaithEnforcer = this;
    AudioManager.Instance.PlayOneShot("event:/rituals/generic_start_ritual");
    Interaction_TempleAltar.Instance.SimpleSetCamera.Play();
    yield return (object) ritualFaithEnforcer.StartCoroutine((IEnumerator) ritualFaithEnforcer.WaitFollowersFormCircle());
    yield return (object) new WaitForSeconds(1f);
    List<FollowerSelectEntry> followerSelectEntries = new List<FollowerSelectEntry>();
    foreach (FollowerBrain followerBrain in Ritual.GetFollowersAvailableToAttendSermon())
    {
      if (followerBrain.Info.CursedState != Thought.None)
        followerSelectEntries.Add(new FollowerSelectEntry(followerBrain, FollowerManager.GetFollowerCursedStateAvailability(followerBrain)));
      else if (followerBrain.Info.FaithEnforcer)
        followerSelectEntries.Add(new FollowerSelectEntry(followerBrain, FollowerSelectEntry.Status.UnavailableAlreadyFaithEnforcer));
      else
        followerSelectEntries.Add(new FollowerSelectEntry(followerBrain));
    }
    UIFollowerSelectMenuController followerSelectInstance = MonoSingleton<UIManager>.Instance.FollowerSelectMenuTemplate.Instantiate<UIFollowerSelectMenuController>();
    followerSelectInstance.VotingType = TwitchVoting.VotingType.RITUAL_ENFORCER;
    followerSelectInstance.Show(followerSelectEntries, followerSelectionType: ritualFaithEnforcer.RitualType);
    UIFollowerSelectMenuController selectMenuController1 = followerSelectInstance;
    selectMenuController1.OnFollowerSelected = selectMenuController1.OnFollowerSelected + (Action<FollowerInfo>) (followerInfo =>
    {
      this.contestant1 = FollowerManager.FindFollowerByID(followerInfo.ID);
      AudioManager.Instance.PlayOneShot("event:/ritual_sacrifice/select_follower", PlayerFarming.Instance.gameObject);
      this.loopedSound = AudioManager.Instance.CreateLoop("event:/sermon/preach_loop", PlayerFarming.Instance.gameObject, true, false);
      this.Task1 = new FollowerTask_ManualControl();
      this.contestant1.Brain.HardSwapToTask((FollowerTask) this.Task1);
      this.contestant1.Brain.Info.TaxEnforcer = false;
      foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
      {
        if (allBrain.Info.FaithEnforcer)
        {
          allBrain.Info.FaithEnforcer = false;
          Follower followerById = FollowerManager.FindFollowerByID(allBrain.Info.ID);
          if ((bool) (UnityEngine.Object) followerById)
            followerById.SetHat(FollowerHatType.None);
        }
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

  public IEnumerator ContinueRitual()
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
    ritualFaithEnforcer.contestant1.GoTo(ChurchFollowerManager.Instance.RitualCenterPosition.position, new System.Action(ritualFaithEnforcer.\u003CContinueRitual\u003Eb__8_0));
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

  public IEnumerator SetUpCombatant1Routine()
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
    ritualFaithEnforcer.Task1.GoToAndStop(ritualFaithEnforcer.contestant1, ChurchFollowerManager.Instance.RitualCenterPosition.position, new System.Action(ritualFaithEnforcer.\u003CSetUpCombatant1Routine\u003Eb__9_0));
  }

  public IEnumerator EndRitual()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    RitualFaithEnforcer ritualFaithEnforcer = this;
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
    AudioManager.Instance.StopLoop(ritualFaithEnforcer.loopedSound);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(1f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  [CompilerGenerated]
  public void \u003CContinueRitual\u003Eb__8_0() => this.waiting = false;

  [CompilerGenerated]
  public void \u003CSetUpCombatant1Routine\u003Eb__9_0()
  {
    this.contestant1.FacePosition(Interaction_TempleAltar.Instance.PortalEffect.transform.position);
    double num = (double) this.contestant1.SetBodyAnimation("idle", true);
    this.Waiting = false;
  }
}
