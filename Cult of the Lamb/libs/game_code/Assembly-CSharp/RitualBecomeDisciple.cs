// Decompiled with JetBrains decompiler
// Type: RitualBecomeDisciple
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using Lamb.UI.FollowerSelect;
using src.Extensions;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Unify;
using UnityEngine;

#nullable disable
public class RitualBecomeDisciple : Ritual
{
  public Follower contestant1;
  public FollowerTask_ManualControl Task1;
  public bool Waiting = true;

  public override UpgradeSystem.Type RitualType => UpgradeSystem.Type.Ritual_BecomeDisciple;

  public override void Play()
  {
    base.Play();
    this.StartCoroutine(this.RitualRoutine());
  }

  public IEnumerator RitualRoutine()
  {
    RitualBecomeDisciple ritualBecomeDisciple = this;
    AudioManager.Instance.PlayOneShot("event:/rituals/generic_start_ritual");
    Interaction_TempleAltar.Instance.SimpleSetCamera.Play();
    yield return (object) ritualBecomeDisciple.StartCoroutine(ritualBecomeDisciple.WaitFollowersFormCircle());
    yield return (object) new WaitForSeconds(1f);
    UIFollowerSelectMenuController followerSelectInstance = MonoSingleton<UIManager>.Instance.FollowerSelectMenuTemplate.Instantiate<UIFollowerSelectMenuController>();
    followerSelectInstance.VotingType = TwitchVoting.VotingType.RITUAL_BECOMEDISCIPLE;
    followerSelectInstance.Show(Ritual.GetFollowerSelectEntriesForSermon(10, true), followerSelectionType: ritualBecomeDisciple.RitualType);
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
    UIFollowerSelectMenuController selectMenuController2 = followerSelectInstance;
    selectMenuController2.OnShow = selectMenuController2.OnShow + (System.Action) (() => { });
    UIFollowerSelectMenuController selectMenuController3 = followerSelectInstance;
    selectMenuController3.OnCancel = selectMenuController3.OnCancel + (System.Action) (() =>
    {
      AudioManager.Instance.StopLoop(this.loopedSound);
      this.EndRitual();
      this.CompleteRitual(true);
      this.CancelFollowers();
      Interaction_TempleAltar.Instance.SimpleSetCamera.Reset();
    });
    UIFollowerSelectMenuController selectMenuController4 = followerSelectInstance;
    selectMenuController4.OnHidden = selectMenuController4.OnHidden + (System.Action) (() => followerSelectInstance = (UIFollowerSelectMenuController) null);
  }

  public IEnumerator ContinueRitual()
  {
    RitualBecomeDisciple ritualBecomeDisciple = this;
    PlayerFarming.Instance.simpleSpineAnimator.Animate("build", 0, true);
    PlayerFarming.Instance.state.facingAngle = Utils.GetAngle(ritualBecomeDisciple.transform.position, ritualBecomeDisciple.contestant1.transform.position);
    yield return (object) new WaitForSeconds(1.5f);
    PlayerFarming.Instance.simpleSpineAnimator.Animate("rituals/ritual-start", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("rituals/ritual-loop", 0, true, 0.0f);
    Interaction_TempleAltar.Instance.CloseUpCamera.Play();
    yield return (object) new WaitForSeconds(0.8333333f);
    ChurchFollowerManager.Instance.PlayOverlay(ChurchFollowerManager.OverlayType.Ritual, "resurrect");
    Interaction_TempleAltar.Instance.PulseDisplacementObject(Interaction_TempleAltar.Instance.PortalEffect.transform.position - Vector3.back);
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      if (followerBrain.CurrentTaskType == FollowerTaskType.AttendTeaching)
        (followerBrain.CurrentTask as FollowerTask_AttendRitual).Pray(2);
    }
    ChurchFollowerManager.Instance.GodRays.gameObject.SetActive(true);
    ChurchFollowerManager.Instance.GodRays.GetComponent<ParticleSystem>().Play();
    ChurchFollowerManager.Instance.Goop.gameObject.SetActive(true);
    ChurchFollowerManager.Instance.Goop.Play("Show");
    ChurchFollowerManager.Instance.Goop.GetComponentInChildren<MeshRenderer>().material.SetColor("_TintCOlor", Color.white);
    yield return (object) new WaitForSeconds(0.5f);
    AudioManager.Instance.PlayOneShot("event:/Stings/ritual_disciple", PlayerFarming.Instance.gameObject);
    double num1 = (double) ritualBecomeDisciple.contestant1.SetBodyAnimation("Disciple/promote", false);
    Interaction_TempleAltar.Instance.CloseUpCamera.gameObject.SetActive(false);
    yield return (object) new WaitForSeconds(7f);
    ritualBecomeDisciple.contestant1.Brain.BecomeDisciple();
    FollowerBrain.SetFollowerCostume(ritualBecomeDisciple.contestant1.Spine.Skeleton, ritualBecomeDisciple.contestant1.Brain._directInfoAccess, forceUpdate: true);
    double num2 = (double) ritualBecomeDisciple.contestant1.SetBodyAnimation("idle", true);
    float t = 0.0f;
    while ((double) t < 1.0)
    {
      t += Time.deltaTime;
      FollowerBrain.SetFollowerCostume(ritualBecomeDisciple.contestant1.Spine.Skeleton, ritualBecomeDisciple.contestant1.Brain._directInfoAccess, forceUpdate: true);
      yield return (object) null;
    }
    Vector3 position = ritualBecomeDisciple.contestant1.transform.position;
    AudioManager.Instance.PlayOneShot("event:/dialogue/followers/loved", position);
    AudioManager.Instance.PlayOneShot("event:/player/Curses/enemy_charmed", position);
    ChurchFollowerManager.Instance.Sparkles.gameObject.SetActive(true);
    ChurchFollowerManager.Instance.Sparkles.Play();
    PlayerFarming.Instance.simpleSpineAnimator.Animate("rituals/ritual-stop", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    MMVibrate.StopRumble();
    ChurchFollowerManager.Instance.GodRays.GetComponent<ParticleSystem>().Stop();
    ChurchFollowerManager.Instance.Goop.Play("Hide");
    Interaction_TempleAltar.Instance.SimpleSetCamera.Reset();
    foreach (FollowerBrain brain in Ritual.FollowerToAttendSermon)
    {
      float Delay = UnityEngine.Random.Range(0.1f, 0.5f);
      ritualBecomeDisciple.StartCoroutine(ritualBecomeDisciple.DelayFollowerReaction(brain, Delay));
      Follower followerById = FollowerManager.FindFollowerByID(brain.Info.ID);
      if ((bool) (UnityEngine.Object) followerById)
        followerById.Spine.randomOffset = false;
    }
    AudioManager.Instance.StopLoop(ritualBecomeDisciple.loopedSound);
    ChurchFollowerManager.Instance.AddBrainToAudience(ritualBecomeDisciple.contestant1.Brain);
    yield return (object) new WaitForSeconds(1.5f);
    ChurchFollowerManager.Instance.Goop.gameObject.SetActive(false);
    ChurchFollowerManager.Instance.GodRays.SetActive(false);
    ChurchFollowerManager.Instance.Sparkles.Stop();
    JudgementMeter.ShowModify(1);
    ChurchFollowerManager.Instance.EndRitualOverlay();
    yield return (object) new WaitForSeconds(1.5f);
    ChurchFollowerManager.Instance.Sparkles.gameObject.SetActive(false);
    int id = ritualBecomeDisciple.contestant1.Brain.Info.ID;
    ritualBecomeDisciple.CompleteRitual(targetFollowerID_1: ritualBecomeDisciple.contestant1.Brain.Info.ID);
    ++DataManager.Instance.DisciplesCreated;
    if (DataManager.Instance.DisciplesCreated >= 12)
      AchievementsWrapper.UnlockAchievement(Achievements.Instance.Lookup("DISCIPLES_12"));
    yield return (object) new WaitForSeconds(1f);
    CultFaithManager.AddThought(Thought.Cult_BecomeDisciple, id);
  }

  public IEnumerator SetUpCombatant1Routine()
  {
    RitualBecomeDisciple ritualBecomeDisciple = this;
    yield return (object) null;
    ChurchFollowerManager.Instance.RemoveBrainFromAudience(ritualBecomeDisciple.contestant1.Brain);
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.CurrentTaskType == FollowerTaskType.AttendTeaching)
      {
        allBrain.CurrentTask.RecalculateDestination();
        allBrain.CurrentTask.Setup(FollowerManager.FindFollowerByID(allBrain.Info.ID));
      }
    }
    ritualBecomeDisciple.Task1.GoToAndStop(ritualBecomeDisciple.contestant1, ChurchFollowerManager.Instance.RitualCenterPosition.position, new System.Action(ritualBecomeDisciple.\u003CSetUpCombatant1Routine\u003Eb__8_0));
  }

  public IEnumerator EndRitualIE()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    RitualBecomeDisciple ritualBecomeDisciple = this;
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
    AudioManager.Instance.StopLoop(ritualBecomeDisciple.loopedSound);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(1f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator SpawnSouls(GameObject target, Vector3 fromPosition, float delay)
  {
    yield return (object) new WaitForSeconds(delay);
    for (int i = 0; i < 1; ++i)
    {
      SoulCustomTargetLerp.Create(target, fromPosition, 0.5f, Color.white).GetComponent<SoulCustomTargetLerp>().Offset = -Vector3.forward;
      yield return (object) new WaitForSeconds(0.2f);
    }
  }

  public void EndRitual()
  {
    ChurchFollowerManager.Instance.EndRitualOverlay();
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
      followerBrain.CompleteCurrentTask();
  }

  [CompilerGenerated]
  public void \u003CSetUpCombatant1Routine\u003Eb__8_0()
  {
    this.contestant1.FacePosition(Interaction_TempleAltar.Instance.PortalEffect.transform.position);
    double num = (double) this.contestant1.SetBodyAnimation("idle", true);
    this.Waiting = false;
  }
}
