// Decompiled with JetBrains decompiler
// Type: RitualTaxEnforcer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
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
public class RitualTaxEnforcer : Ritual
{
  public Follower contestant1;
  public FollowerTask_ManualControl Task1;
  public bool Waiting = true;
  public bool waiting = true;
  public int count;

  public override UpgradeSystem.Type RitualType => UpgradeSystem.Type.Ritual_AssignTaxCollector;

  public override void Play()
  {
    base.Play();
    this.StartCoroutine((IEnumerator) this.RitualRoutine());
  }

  public IEnumerator RitualRoutine()
  {
    RitualTaxEnforcer ritualTaxEnforcer = this;
    AudioManager.Instance.PlayOneShot("event:/rituals/generic_start_ritual");
    Interaction_TempleAltar.Instance.SimpleSetCamera.Play();
    yield return (object) ritualTaxEnforcer.StartCoroutine((IEnumerator) ritualTaxEnforcer.WaitFollowersFormCircle());
    yield return (object) new WaitForSeconds(1f);
    List<FollowerSelectEntry> followerSelectEntries = new List<FollowerSelectEntry>();
    foreach (FollowerBrain followerBrain in Ritual.GetFollowersAvailableToAttendSermon())
    {
      if (followerBrain.Info.CursedState != Thought.None)
        followerSelectEntries.Add(new FollowerSelectEntry(followerBrain, FollowerManager.GetFollowerCursedStateAvailability(followerBrain)));
      else if (followerBrain.Info.TaxEnforcer)
        followerSelectEntries.Add(new FollowerSelectEntry(followerBrain, FollowerSelectEntry.Status.UnavailableAlreadyTaxEnforcer));
      else
        followerSelectEntries.Add(new FollowerSelectEntry(followerBrain));
    }
    UIFollowerSelectMenuController followerSelectInstance = MonoSingleton<UIManager>.Instance.FollowerSelectMenuTemplate.Instantiate<UIFollowerSelectMenuController>();
    followerSelectInstance.VotingType = TwitchVoting.VotingType.RITUAL_ENFORCER;
    followerSelectInstance.Show(followerSelectEntries, followerSelectionType: ritualTaxEnforcer.RitualType);
    UIFollowerSelectMenuController selectMenuController1 = followerSelectInstance;
    selectMenuController1.OnFollowerSelected = selectMenuController1.OnFollowerSelected + (Action<FollowerInfo>) (followerInfo =>
    {
      this.contestant1 = FollowerManager.FindFollowerByID(followerInfo.ID);
      AudioManager.Instance.PlayOneShot("event:/ritual_sacrifice/select_follower", PlayerFarming.Instance.gameObject);
      this.loopedSound = AudioManager.Instance.CreateLoop("event:/sermon/preach_loop", PlayerFarming.Instance.gameObject, true, false);
      this.Task1 = new FollowerTask_ManualControl();
      this.contestant1.Brain.HardSwapToTask((FollowerTask) this.Task1);
      this.contestant1.Brain.Info.FaithEnforcer = false;
      foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
      {
        if (allBrain.Info.TaxEnforcer)
        {
          allBrain.Info.TaxEnforcer = false;
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
    RitualTaxEnforcer ritualTaxEnforcer = this;
    AudioManager.Instance.PlayOneShot("event:/ritual_sacrifice/select_follower", PlayerFarming.Instance.gameObject);
    PlayerFarming.Instance.simpleSpineAnimator.Animate("build", 0, true);
    PlayerFarming.Instance.state.facingAngle = Utils.GetAngle(ritualTaxEnforcer.transform.position, ritualTaxEnforcer.contestant1.transform.position);
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
    yield return (object) new WaitForSeconds(0.5f);
    ritualTaxEnforcer.waiting = true;
    ritualTaxEnforcer.contestant1.GoTo(ChurchFollowerManager.Instance.RitualCenterPosition.position, new System.Action(ritualTaxEnforcer.\u003CContinueRitual\u003Eb__9_0));
    while (ritualTaxEnforcer.waiting)
      yield return (object) null;
    double num1 = (double) ritualTaxEnforcer.contestant1.SetBodyAnimation("Conversations/react-love3", false);
    BiomeConstants.Instance.EmitSmokeExplosionVFX(ritualTaxEnforcer.contestant1.transform.position + Vector3.back);
    AudioManager.Instance.PlayOneShot("event:/Stings/Choir_Short");
    ritualTaxEnforcer.contestant1.Brain.Info.TaxEnforcer = true;
    ritualTaxEnforcer.contestant1.SetOutfit(ritualTaxEnforcer.contestant1.Outfit.CurrentOutfit, false);
    yield return (object) new WaitForSeconds(2f);
    for (int i = 0; i < 2; ++i)
    {
      ritualTaxEnforcer.waiting = true;
      ritualTaxEnforcer.contestant1.GoTo(ChurchFollowerManager.Instance.RitualCenterPosition.position + (Vector3) UnityEngine.Random.insideUnitCircle * 1.85f, new System.Action(ritualTaxEnforcer.\u003CContinueRitual\u003Eb__9_2));
      while (ritualTaxEnforcer.waiting)
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
      double num2 = (double) ritualTaxEnforcer.contestant1.SetBodyAnimation(animName, false);
      yield return (object) new WaitForSeconds(1f);
    }
    ritualTaxEnforcer.waiting = true;
    ritualTaxEnforcer.contestant1.GoTo(ChurchFollowerManager.Instance.RitualCenterPosition.position, new System.Action(ritualTaxEnforcer.\u003CContinueRitual\u003Eb__9_1));
    while (ritualTaxEnforcer.waiting)
      yield return (object) null;
    ritualTaxEnforcer.contestant1.AddBodyAnimation("tax-enforcer", false, 0.0f);
    yield return (object) new WaitForSeconds(1.5f);
    ritualTaxEnforcer.contestant1.AddBodyAnimation("Conversations/react-nice3", false, 0.0f);
    ritualTaxEnforcer.contestant1.AddBodyAnimation("idle", true, 0.0f);
    ritualTaxEnforcer.count = 0;
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      if (followerBrain != ritualTaxEnforcer.contestant1.Brain)
      {
        AudioManager.Instance.PlayOneShot("event:/rituals/coins", followerBrain.LastPosition);
        ResourceCustomTarget.Create(ritualTaxEnforcer.contestant1.gameObject, followerBrain.LastPosition, InventoryItem.ITEM_TYPE.BLACK_GOLD, (System.Action) null);
        (followerBrain.CurrentTask as FollowerTask_AttendRitual).Pray(2);
        ++ritualTaxEnforcer.count;
        yield return (object) new WaitForSeconds(0.1f);
      }
      yield return (object) null;
    }
    ritualTaxEnforcer.contestant1.Brain._directInfoAccess.TaxCollected += ritualTaxEnforcer.count;
    yield return (object) new WaitForSeconds(1f);
    PlayerFarming.Instance.simpleSpineAnimator.Animate("rituals/ritual-stop", 0, false);
    Interaction_TempleAltar.Instance.SimpleSetCamera.Reset();
    foreach (FollowerBrain brain in Ritual.FollowerToAttendSermon)
    {
      brain.CompleteCurrentTask();
      float Delay = UnityEngine.Random.Range(0.1f, 0.5f);
      ritualTaxEnforcer.StartCoroutine((IEnumerator) ritualTaxEnforcer.DelayFollowerReaction(brain, Delay));
      Follower followerById = FollowerManager.FindFollowerByID(brain.Info.ID);
      if ((bool) (UnityEngine.Object) followerById)
        followerById.Spine.randomOffset = false;
    }
    AudioManager.Instance.StopLoop(ritualTaxEnforcer.loopedSound);
    yield return (object) new WaitForSeconds(0.5f);
    ChurchFollowerManager.Instance.AddBrainToAudience(ritualTaxEnforcer.contestant1.Brain);
    ritualTaxEnforcer.CompleteRitual(targetFollowerID_1: ritualTaxEnforcer.contestant1.Brain.Info.ID);
    yield return (object) new WaitForSeconds(1f);
    CultFaithManager.AddThought(Thought.Cult_TaxEnforcer);
  }

  public IEnumerator SetUpCombatant1Routine()
  {
    RitualTaxEnforcer ritualTaxEnforcer = this;
    yield return (object) null;
    ChurchFollowerManager.Instance.RemoveBrainFromAudience(ritualTaxEnforcer.contestant1.Brain);
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.CurrentTaskType == FollowerTaskType.AttendTeaching)
      {
        allBrain.CurrentTask.RecalculateDestination();
        allBrain.CurrentTask.Setup(FollowerManager.FindFollowerByID(allBrain.Info.ID));
      }
    }
    ritualTaxEnforcer.Task1.GoToAndStop(ritualTaxEnforcer.contestant1, ChurchFollowerManager.Instance.RitualCenterPosition.position, new System.Action(ritualTaxEnforcer.\u003CSetUpCombatant1Routine\u003Eb__10_0));
  }

  public IEnumerator EndRitual()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    RitualTaxEnforcer ritualTaxEnforcer = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      Interaction_TempleAltar.Instance.state.CURRENT_STATE = StateMachine.State.Idle;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    AudioManager.Instance.StopLoop(ritualTaxEnforcer.loopedSound);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(1f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  [CompilerGenerated]
  public void \u003CContinueRitual\u003Eb__9_0() => this.waiting = false;

  [CompilerGenerated]
  public void \u003CContinueRitual\u003Eb__9_2() => this.waiting = false;

  [CompilerGenerated]
  public void \u003CContinueRitual\u003Eb__9_1() => this.waiting = false;

  [CompilerGenerated]
  public void \u003CSetUpCombatant1Routine\u003Eb__10_0()
  {
    this.contestant1.FacePosition(Interaction_TempleAltar.Instance.PortalEffect.transform.position);
    double num = (double) this.contestant1.SetBodyAnimation("idle", true);
    this.Waiting = false;
  }
}
