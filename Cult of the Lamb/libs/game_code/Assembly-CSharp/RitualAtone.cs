// Decompiled with JetBrains decompiler
// Type: RitualAtone
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using Lamb.UI;
using Lamb.UI.FollowerSelect;
using src.Extensions;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class RitualAtone : Ritual
{
  public Follower contestant1;
  public FollowerTask_ManualControl Task1;
  public bool Waiting = true;
  public EventInstance loopy;

  public override UpgradeSystem.Type RitualType => UpgradeSystem.Type.Ritual_AtoneSin;

  public override void Play()
  {
    base.Play();
    this.StartCoroutine(this.RitualRoutine());
  }

  public IEnumerator RitualRoutine()
  {
    RitualAtone ritualAtone = this;
    AudioManager.Instance.PlayOneShot("event:/rituals/generic_start_ritual");
    Interaction_TempleAltar.Instance.SimpleSetCamera.Play();
    yield return (object) ritualAtone.StartCoroutine(ritualAtone.WaitFollowersFormCircle());
    yield return (object) new UnityEngine.WaitForSeconds(1f);
    UIFollowerSelectMenuController followerSelectInstance = MonoSingleton<UIManager>.Instance.FollowerSelectMenuTemplate.Instantiate<UIFollowerSelectMenuController>();
    followerSelectInstance.VotingType = TwitchVoting.VotingType.RITUAL_ATONE;
    followerSelectInstance.Show(Ritual.GetFollowerSelectEntriesForSermon(), followerSelectionType: ritualAtone.RitualType);
    UIFollowerSelectMenuController selectMenuController1 = followerSelectInstance;
    selectMenuController1.OnFollowerSelected = selectMenuController1.OnFollowerSelected + (Action<FollowerInfo>) (followerInfo =>
    {
      this.contestant1 = FollowerManager.FindFollowerByID(followerInfo.ID);
      AudioManager.Instance.PlayOneShot("event:/ritual_sacrifice/select_follower", PlayerFarming.Instance.gameObject);
      this.loopedSound = AudioManager.Instance.CreateLoop("event:/sermon/preach_loop", PlayerFarming.Instance.gameObject, true, false);
      this.Task1 = new FollowerTask_ManualControl();
      this.contestant1.Brain.HardSwapToTask((FollowerTask) this.Task1);
      GameManager.GetInstance().StartCoroutine(this.ContinueRitual());
      this.StartCoroutine(this.SetUpCombatant1Routine());
    });
    int SinsToAdd = ritualAtone.GetSinsToAdd();
    UIFollowerSelectMenuController selectMenuController2 = followerSelectInstance;
    selectMenuController2.OnShow = selectMenuController2.OnShow + (System.Action) (() =>
    {
      foreach (FollowerInformationBox followerInfoBox in followerSelectInstance.FollowerInfoBoxes)
        followerInfoBox.ShowPleasure(SinsToAdd);
    });
    UIFollowerSelectMenuController selectMenuController3 = followerSelectInstance;
    selectMenuController3.OnShownCompleted = selectMenuController3.OnShownCompleted + (System.Action) (() =>
    {
      foreach (FollowerInformationBox followerInfoBox in followerSelectInstance.FollowerInfoBoxes)
        followerInfoBox.ShowPleasure(SinsToAdd);
    });
    UIFollowerSelectMenuController selectMenuController4 = followerSelectInstance;
    selectMenuController4.OnCancel = selectMenuController4.OnCancel + (System.Action) (() =>
    {
      AudioManager.Instance.StopLoop(this.loopedSound);
      this.EndRitual();
      this.CompleteRitual(true);
      this.CancelFollowers();
      Interaction_TempleAltar.Instance.SimpleSetCamera.Reset();
    });
    UIFollowerSelectMenuController selectMenuController5 = followerSelectInstance;
    selectMenuController5.OnHidden = selectMenuController5.OnHidden + (System.Action) (() => followerSelectInstance = (UIFollowerSelectMenuController) null);
  }

  public int GetSinsToAdd()
  {
    int sinsToAdd = 0;
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      int num = Mathf.Max(followerBrain.Info.Pleasure, 1);
      sinsToAdd += num;
    }
    return sinsToAdd;
  }

  public IEnumerator ContinueRitual()
  {
    RitualAtone ritualAtone1 = this;
    PlayerFarming.Instance.simpleSpineAnimator.Animate("build", 0, true);
    PlayerFarming.Instance.state.facingAngle = Utils.GetAngle(ritualAtone1.transform.position, ritualAtone1.contestant1.transform.position);
    yield return (object) new UnityEngine.WaitForSeconds(1.5f);
    PlayerFarming.Instance.simpleSpineAnimator.Animate("rituals/ritual-start", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("rituals/ritual-loop", 0, true, 0.0f);
    Interaction_TempleAltar.Instance.CloseUpCamera.Play();
    yield return (object) new UnityEngine.WaitForSeconds(0.8333333f);
    Interaction_TempleAltar.Instance.PulseDisplacementObject(Interaction_TempleAltar.Instance.PortalEffect.transform.position - Vector3.back);
    ChurchFollowerManager.Instance.GodRays.gameObject.SetActive(true);
    ChurchFollowerManager.Instance.GodRays.GetComponent<ParticleSystem>().Play();
    ChurchFollowerManager.Instance.Goop.gameObject.SetActive(true);
    ChurchFollowerManager.Instance.Goop.Play("Show");
    ChurchFollowerManager.Instance.Goop.GetComponentInChildren<MeshRenderer>().material.SetColor("_TintCOlor", Color.black);
    yield return (object) new UnityEngine.WaitForSeconds(0.5f);
    AudioManager.Instance.PlayOneShot("event:/Stings/ritual_ascension_pg", PlayerFarming.Instance.gameObject);
    double num1 = (double) ritualAtone1.contestant1.SetBodyAnimation("Sin/sin-atone-float-up", false);
    ritualAtone1.contestant1.AddBodyAnimation("Sin/sin-atone-float-loop", true, 0.0f);
    Interaction_TempleAltar.Instance.CloseUpCamera.gameObject.SetActive(false);
    yield return (object) new UnityEngine.WaitForSeconds(1f);
    yield return (object) new UnityEngine.WaitForSeconds(1f);
    int id = ritualAtone1.contestant1.Brain.Info.ID;
    Ritual.FollowerToAttendSermon.Remove(ritualAtone1.contestant1.Brain);
    GameManager.GetInstance().WaitForSeconds(2f, new System.Action(ritualAtone1.\u003CContinueRitual\u003Eb__9_0));
    int sinToAdd = 0;
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      RitualAtone ritualAtone = ritualAtone1;
      if ((double) followerBrain.Info.Pleasure < 65.0)
      {
        int sin = Mathf.Max(followerBrain.Info.Pleasure, 1);
        sinToAdd += sin;
        followerBrain.Info.Pleasure = 0;
        Follower f = FollowerManager.FindFollowerByID(followerBrain.Info.ID);
        if ((UnityEngine.Object) f != (UnityEngine.Object) null && sin >= 1)
        {
          double num2 = (double) f.SetBodyAnimation("Sin/sin-stab", false);
          f.AddBodyAnimation("pray", true, 0.0f);
          if (sin > 1)
            f.PleasureUI.Show();
          ritualAtone1.StartCoroutine(ritualAtone1.SpawnSouls(ritualAtone1.contestant1.gameObject, Mathf.Clamp(sin, 5, 25), f.transform.position, 1.33f));
          ((FollowerTask_AttendRitual) f.Brain.CurrentTask).Pray();
          ritualAtone1.StartCoroutine(ritualAtone1.WaitForSeconds(0.85f, (System.Action) (() =>
          {
            AudioManager.Instance.PlayOneShot("event:/enemy/impact_squishy", f.gameObject);
            if (sin <= 1)
              return;
            f.PleasureUI.BarController.ShrinkBarToEmpty(1f);
            ritualAtone.StartCoroutine(ritualAtone.WaitForSeconds(1f, (System.Action) (() =>
            {
              f.PleasureUI.BarController.ShrinkBarToEmpty(1f);
              ritualAtone.StartCoroutine(ritualAtone.WaitForSeconds(1f, (System.Action) (() => f.PleasureUI.Hide())));
            })));
          })));
          yield return (object) new UnityEngine.WaitForSeconds(0.3f);
        }
      }
    }
    yield return (object) new UnityEngine.WaitForSeconds(6f);
    Debug.Log((object) ("ATONE LOOPY STOPPING NOW " + Time.realtimeSinceStartup.ToString()));
    AudioManager.Instance.StopLoop(ritualAtone1.loopy);
    if ((double) (ritualAtone1.contestant1.Brain.Info.Pleasure + sinToAdd) >= 65.0)
    {
      double num3 = (double) ritualAtone1.contestant1.SetBodyAnimation("Sin/sin-atone-float-down-full", false);
      ritualAtone1.contestant1.AddBodyAnimation("Sin/sin-floating", true, 0.0f);
    }
    else
    {
      double num4 = (double) ritualAtone1.contestant1.SetBodyAnimation("Sin/sin-atone-float-down", false);
      ritualAtone1.contestant1.AddBodyAnimation("idle", true, 0.0f);
    }
    yield return (object) new UnityEngine.WaitForSeconds(3f);
    Ritual.FollowerToAttendSermon.Add(ritualAtone1.contestant1.Brain);
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      if (followerBrain.CurrentTask is FollowerTask_AttendRitual)
        (followerBrain.CurrentTask as FollowerTask_AttendRitual).Cheer();
    }
    MMVibrate.StopRumble();
    do
    {
      int amount;
      if ((double) (ritualAtone1.contestant1.Brain.Info.Pleasure + sinToAdd) >= 65.0)
      {
        amount = 65 - ritualAtone1.contestant1.Brain.Info.Pleasure;
        sinToAdd -= amount;
      }
      else
        amount = sinToAdd;
      ritualAtone1.contestant1.Brain.AddPleasureInt(amount);
      if (!ritualAtone1.contestant1.Brain.CanGiveSin())
      {
        yield return (object) new UnityEngine.WaitForSeconds(1.5333333f);
        break;
      }
      while ((UnityEngine.Object) ritualAtone1.contestant1 != (UnityEngine.Object) null && ritualAtone1.contestant1.InGiveSinSequence)
        yield return (object) null;
    }
    while (!((UnityEngine.Object) ritualAtone1.contestant1 == (UnityEngine.Object) null));
    if ((UnityEngine.Object) ritualAtone1.contestant1 != (UnityEngine.Object) null)
    {
      double num5 = (double) ritualAtone1.contestant1.SetBodyAnimation("idle", true);
    }
    GameManager.GetInstance().OnConversationNew();
    ChurchFollowerManager.Instance.GodRays.GetComponent<ParticleSystem>().Stop();
    ChurchFollowerManager.Instance.Goop.Play("Hide");
    Interaction_TempleAltar.Instance.SimpleSetCamera.Reset();
    foreach (FollowerBrain brain in Ritual.FollowerToAttendSermon)
    {
      float Delay = UnityEngine.Random.Range(0.1f, 0.5f);
      ritualAtone1.StartCoroutine(ritualAtone1.DelayFollowerReaction(brain, Delay));
    }
    AudioManager.Instance.StopLoop(ritualAtone1.loopedSound);
    ChurchFollowerManager.Instance.Goop.gameObject.SetActive(false);
    ChurchFollowerManager.Instance.GodRays.SetActive(false);
    ChurchFollowerManager.Instance.Sparkles.Stop();
    JudgementMeter.ShowModify(1);
    yield return (object) new UnityEngine.WaitForSeconds(1.5f);
    ChurchFollowerManager.Instance.Sparkles.gameObject.SetActive(false);
    ritualAtone1.CompleteRitual(targetFollowerID_1: id, PlayFakeBar: false);
    yield return (object) new UnityEngine.WaitForSeconds(1f);
  }

  public IEnumerator WaitForSeconds(float sec, System.Action callback)
  {
    yield return (object) new UnityEngine.WaitForSeconds(sec);
    System.Action action = callback;
    if (action != null)
      action();
  }

  public FollowerBrain.AdorationActions GetAdorationReward(FollowerBrain brain)
  {
    if (brain.Info.XPLevel == 2)
      return FollowerBrain.AdorationActions.AscendedFollower_Lvl2;
    if (brain.Info.XPLevel == 3)
      return FollowerBrain.AdorationActions.AscendedFollower_Lvl3;
    if (brain.Info.XPLevel == 4)
      return FollowerBrain.AdorationActions.AscendedFollower_Lvl4;
    return brain.Info.XPLevel >= 5 ? FollowerBrain.AdorationActions.AscendedFollower_Lvl5 : FollowerBrain.AdorationActions.Sermon;
  }

  public int GetAdorationRewardAmount(FollowerBrain brain)
  {
    return FollowerBrain.AdorationsAndActions[this.GetAdorationReward(brain)];
  }

  public IEnumerator SetUpCombatant1Routine()
  {
    RitualAtone ritualAtone = this;
    yield return (object) null;
    ChurchFollowerManager.Instance.RemoveBrainFromAudience(ritualAtone.contestant1.Brain);
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.CurrentTaskType == FollowerTaskType.AttendTeaching)
      {
        allBrain.CurrentTask.RecalculateDestination();
        allBrain.CurrentTask.Setup(FollowerManager.FindFollowerByID(allBrain.Info.ID));
      }
    }
    ritualAtone.contestant1.SetOutfit(FollowerOutfitType.HorseTown, false);
    ritualAtone.Task1.GoToAndStop(ritualAtone.contestant1, ChurchFollowerManager.Instance.RitualCenterPosition.position, new System.Action(ritualAtone.\u003CSetUpCombatant1Routine\u003Eb__13_0));
  }

  public IEnumerator EndRitualIE()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    RitualAtone ritualAtone = this;
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
    AudioManager.Instance.StopLoop(ritualAtone.loopedSound);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new UnityEngine.WaitForSeconds(1f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator SpawnSouls(GameObject target, int amount, Vector3 fromPosition, float delay)
  {
    yield return (object) new UnityEngine.WaitForSeconds(delay);
    for (int i = 0; i < amount; ++i)
    {
      SoulCustomTargetLerp.Create(target, fromPosition, 0.5f, Color.black).GetComponent<SoulCustomTargetLerp>().Offset = -Vector3.forward;
      yield return (object) new UnityEngine.WaitForSeconds(0.2f);
    }
  }

  public void EndRitual()
  {
    ChurchFollowerManager.Instance.EndRitualOverlay();
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
      followerBrain.CompleteCurrentTask();
  }

  public void OnDestroy() => AudioManager.Instance.StopLoop(this.loopy);

  [CompilerGenerated]
  public void \u003CContinueRitual\u003Eb__9_0()
  {
    double num = (double) this.contestant1.SetBodyAnimation("Sin/sin-atone-float-getsin", true);
    Debug.Log((object) ("ATONE LOOPY STARTING NOW " + Time.realtimeSinceStartup.ToString()));
    this.loopy = AudioManager.Instance.CreateLoop("event:/dialogue/followers/possessed/sinners_pride_loop", this.contestant1.gameObject, true);
  }

  [CompilerGenerated]
  public void \u003CSetUpCombatant1Routine\u003Eb__13_0()
  {
    this.contestant1.FacePosition(Interaction_TempleAltar.Instance.PortalEffect.transform.position);
    double num = (double) this.contestant1.SetBodyAnimation("idle", true);
    this.Waiting = false;
  }
}
