// Decompiled with JetBrains decompiler
// Type: RitualRessurect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using Lamb.UI;
using Lamb.UI.FollowerSelect;
using src.Extensions;
using src.UI.Overlays.TutorialOverlay;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class RitualRessurect : Ritual
{
  private FollowerBrain resurrectingFollower;
  public Light RitualLight;
  private float chanceForZombie = 0.1f;

  protected override UpgradeSystem.Type RitualType => UpgradeSystem.Type.Ritual_Ressurect;

  public override void Play()
  {
    base.Play();
    this.StartCoroutine((IEnumerator) this.RitualRoutine());
  }

  private IEnumerator RitualRoutine()
  {
    RitualRessurect ritualRessurect = this;
    AudioManager.Instance.PlayOneShot("event:/rituals/generic_start_ritual");
    yield return (object) ritualRessurect.StartCoroutine((IEnumerator) ritualRessurect.CentreAndAnimatePlayer());
    Interaction_TempleAltar.Instance.SimpleSetCamera.Play();
    Debug.Log((object) "Ritual sacrifice begin gather");
    yield return (object) ritualRessurect.StartCoroutine((IEnumerator) ritualRessurect.WaitFollowersFormCircle());
    Debug.Log((object) "Ritual sacrifice end gather");
    SimulationManager.Pause();
    PlayerFarming.Instance.simpleSpineAnimator.Animate("rituals/ritual-start", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("rituals/ritual-loop", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(1f);
    UIFollowerSelectMenuController followerSelectInstance = MonoSingleton<UIManager>.Instance.FollowerSelectMenuTemplate.Instantiate<UIFollowerSelectMenuController>();
    followerSelectInstance.Show(DataManager.Instance.Followers_Dead, followerSelectionType: ritualRessurect.RitualType);
    UIFollowerSelectMenuController selectMenuController1 = followerSelectInstance;
    selectMenuController1.OnFollowerSelected = selectMenuController1.OnFollowerSelected + (Action<FollowerInfo>) (followerInfo => this.resurrectingFollower = FollowerBrain.GetOrCreateBrain(followerInfo));
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
    AudioManager.Instance.PlayOneShot("event:/ritual_sacrifice/select_follower", PlayerFarming.Instance.gameObject);
    yield return (object) null;
    Interaction_TempleAltar.Instance.SimpleSetCamera.Reset();
    GameManager.GetInstance().StartCoroutine((IEnumerator) ritualRessurect.DoRessurectRoutine());
  }

  private IEnumerator DoRessurectRoutine()
  {
    RitualRessurect ritualRessurect = this;
    if ((UnityEngine.Object) ritualRessurect.RitualLight != (UnityEngine.Object) null)
    {
      ritualRessurect.RitualLight.gameObject.SetActive(true);
      ritualRessurect.RitualLight.color = Color.black;
      ritualRessurect.RitualLight.DOColor(Color.red, 1f);
      ritualRessurect.RitualLight.DOIntensity(1.5f, 1f);
    }
    ChurchFollowerManager.Instance.PlayOverlay(ChurchFollowerManager.OverlayType.Ritual, "resurrect");
    yield return (object) new WaitForSeconds(0.5f);
    ritualRessurect.PlaySacrificePortalEffect();
    Interaction_TempleAltar.Instance.PulseDisplacementObject(Interaction_TempleAltar.Instance.PortalEffect.transform.position);
    BiomeConstants.Instance.ChromaticAbberationTween(1f, BiomeConstants.Instance.ChromaticAberrationDefaultValue, 7f);
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.CurrentTaskType == FollowerTaskType.AttendTeaching)
        (allBrain.CurrentTask as FollowerTask_AttendRitual).WorshipTentacle();
    }
    yield return (object) new WaitForSeconds(0.5f);
    AudioManager.Instance.PlayOneShot("event:/rituals/resurrect");
    bool isZombie = DataManager.Instance.ResurrectRitualCount == 1;
    if (!isZombie && DataManager.Instance.ResurrectRitualCount != 0 && (double) UnityEngine.Random.Range(0.0f, 1f) < (double) ritualRessurect.chanceForZombie)
      isZombie = true;
    isZombie = false;
    ++DataManager.Instance.ResurrectRitualCount;
    DataManager.Instance.Followers_Dead.Remove(ritualRessurect.resurrectingFollower._directInfoAccess);
    DataManager.Instance.Followers_Dead_IDs.Remove(ritualRessurect.resurrectingFollower._directInfoAccess.ID);
    foreach (Structures_Grave structuresGrave in StructureManager.GetAllStructuresOfType<Structures_Grave>(FollowerLocation.Base))
    {
      if (structuresGrave.Data.FollowerID == ritualRessurect.resurrectingFollower.Info.ID)
        structuresGrave.Data.FollowerID = -1;
    }
    ritualRessurect.resurrectingFollower.ResetStats();
    if (ritualRessurect.resurrectingFollower.Info.Age > ritualRessurect.resurrectingFollower.Info.LifeExpectancy)
      ritualRessurect.resurrectingFollower.Info.LifeExpectancy = ritualRessurect.resurrectingFollower.Info.Age + UnityEngine.Random.Range(20, 30);
    else
      ritualRessurect.resurrectingFollower.Info.LifeExpectancy += UnityEngine.Random.Range(20, 30);
    FollowerTask_ManualControl nextTask = new FollowerTask_ManualControl();
    ritualRessurect.resurrectingFollower.HardSwapToTask((FollowerTask) nextTask);
    ritualRessurect.resurrectingFollower.Location = FollowerLocation.Church;
    ritualRessurect.resurrectingFollower.DesiredLocation = FollowerLocation.Church;
    ritualRessurect.resurrectingFollower.CurrentTask.Arrive();
    Follower revivedFollower = FollowerManager.CreateNewFollower(ritualRessurect.resurrectingFollower._directInfoAccess, ChurchFollowerManager.Instance.RitualCenterPosition.position);
    revivedFollower.SetOutfit(FollowerOutfitType.Worker, false);
    revivedFollower.Brain.CheckChangeState();
    Ritual.FollowerToAttendSermon.Add(revivedFollower.Brain);
    revivedFollower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    GameManager.GetInstance().OnConversationNext(revivedFollower.gameObject, 5f);
    revivedFollower.Spine.gameObject.SetActive(false);
    yield return (object) new WaitForSeconds(1f);
    revivedFollower.Spine.gameObject.SetActive(true);
    DeadWorshipper deadWorshipper1 = (DeadWorshipper) null;
    foreach (DeadWorshipper deadWorshipper2 in DeadWorshipper.DeadWorshippers)
    {
      if (deadWorshipper2.StructureInfo.FollowerID == ritualRessurect.resurrectingFollower.Info.ID)
      {
        deadWorshipper1 = deadWorshipper2;
        break;
      }
    }
    if ((UnityEngine.Object) deadWorshipper1 != (UnityEngine.Object) null)
      StructureManager.RemoveStructure(deadWorshipper1.Structure.Brain);
    GameManager.GetInstance().OnConversationNext(revivedFollower.gameObject, 8f);
    double num1 = (double) revivedFollower.SetBodyAnimation("Sermons/resurrect", false);
    if (isZombie)
    {
      revivedFollower.AddBodyAnimation("Insane/be-weird", false, 0.0f);
      revivedFollower.AddBodyAnimation("Insane/idle-insane", true, 0.0f);
      revivedFollower.Brain.ApplyCurseState(Thought.Zombie);
      yield return (object) new WaitForSeconds(4.5f);
    }
    else
    {
      revivedFollower.AddBodyAnimation("Reactions/react-enlightened1", false, 0.0f);
      revivedFollower.AddBodyAnimation("idle", true, 0.0f);
      yield return (object) new WaitForSeconds(2f);
    }
    if (isZombie)
    {
      float num2 = 0.0f;
      foreach (FollowerBrain brain in Ritual.FollowerToAttendSermon)
      {
        float Delay = UnityEngine.Random.Range(0.1f, 0.5f);
        num2 += Delay;
        ritualRessurect.StartCoroutine((IEnumerator) ritualRessurect.DelayFollowerReaction(brain, "Reactions/react-spooked", Delay));
      }
      yield return (object) new WaitForSeconds(2f);
      if (DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.Zombies))
      {
        UITutorialOverlayController tutorialOverlay = MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.Zombies);
        while ((UnityEngine.Object) tutorialOverlay != (UnityEngine.Object) null)
          yield return (object) null;
        tutorialOverlay = (UITutorialOverlayController) null;
      }
    }
    Interaction_TempleAltar.Instance.PulseDisplacementObject(revivedFollower.transform.position);
    yield return (object) new WaitForSeconds(2f);
    Interaction_TempleAltar.Instance.SimpleSetCamera.Reset();
    PlayerFarming.Instance.simpleSpineAnimator.Animate("rituals/ritual-stop", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(0.6666667f);
    float num3 = 0.5f;
    foreach (FollowerBrain brain in Ritual.FollowerToAttendSermon)
    {
      float Delay = UnityEngine.Random.Range(0.1f, 0.5f);
      num3 += Delay;
      ritualRessurect.StartCoroutine((IEnumerator) ritualRessurect.DelayFollowerReaction(brain, Delay));
    }
    yield return (object) new WaitForSeconds(1f);
    BiomeConstants.Instance.ChromaticAbberationTween(0.25f, 7f, BiomeConstants.Instance.ChromaticAberrationDefaultValue);
    ritualRessurect.EndRitual();
    bool completedQuest = false;
    foreach (ObjectivesData objective in DataManager.Instance.Objectives)
    {
      if (objective.Type == Objectives.TYPES.PERFORM_RITUAL && ((Objectives_PerformRitual) objective).Ritual == UpgradeSystem.Type.Ability_Resurrection && ((Objectives_PerformRitual) objective).TargetFollowerID_1 == revivedFollower.Brain.Info.ID)
      {
        completedQuest = true;
        break;
      }
    }
    yield return (object) new WaitForSeconds(0.5f);
    ritualRessurect.CompleteRitual(targetFollowerID_1: revivedFollower.Brain.Info.ID);
    yield return (object) new WaitForSeconds(1f);
    CultFaithManager.AddThought(Thought.Cult_Ressurection, revivedFollower.Brain.Info.ID, completedQuest ? 0.0f : 1f);
  }

  private void EndRitual()
  {
    this.StopSacrificePortalEffect();
    ChurchFollowerManager.Instance.StopSacrificePortalEffect();
  }
}
