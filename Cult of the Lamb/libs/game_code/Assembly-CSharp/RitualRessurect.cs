// Decompiled with JetBrains decompiler
// Type: RitualRessurect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using Lamb.UI;
using Lamb.UI.FollowerSelect;
using src.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RitualRessurect : Ritual
{
  public FollowerBrain resurrectingFollower;
  public Light RitualLight;

  public override UpgradeSystem.Type RitualType => UpgradeSystem.Type.Ritual_Ressurect;

  public override void Play()
  {
    base.Play();
    this.StartCoroutine(this.RitualRoutine());
  }

  public IEnumerator RitualRoutine()
  {
    RitualRessurect ritualRessurect = this;
    AudioManager.Instance.PlayOneShot("event:/rituals/generic_start_ritual");
    yield return (object) ritualRessurect.StartCoroutine(ritualRessurect.CentreAndAnimatePlayer());
    Interaction_TempleAltar.Instance.SimpleSetCamera.Play();
    Debug.Log((object) "Ritual sacrifice begin gather");
    yield return (object) ritualRessurect.StartCoroutine(ritualRessurect.WaitFollowersFormCircle());
    Debug.Log((object) "Ritual sacrifice end gather");
    SimulationManager.Pause();
    PlayerFarming.Instance.simpleSpineAnimator.Animate("rituals/ritual-start", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("rituals/ritual-loop", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(1f);
    List<FollowerSelectEntry> followerSelectEntries = new List<FollowerSelectEntry>();
    foreach (FollowerInfo followerInfo in DataManager.Instance.Followers_Dead)
    {
      if (!followerInfo.FrozeToDeath || !StructureManager.HasFollowerDeadWorshipper(followerInfo.ID))
      {
        if (followerInfo.Hat == FollowerHatType.Enforcer || followerInfo.Hat == FollowerHatType.FaithEnforcer)
          followerInfo.Hat = FollowerHatType.None;
        followerSelectEntries.Add(new FollowerSelectEntry(followerInfo));
      }
    }
    UIFollowerSelectMenuController followerSelectInstance = MonoSingleton<UIManager>.Instance.FollowerSelectMenuTemplate.Instantiate<UIFollowerSelectMenuController>();
    followerSelectInstance.VotingType = TwitchVoting.VotingType.RITUAL_RESURRECT;
    followerSelectInstance.Show(followerSelectEntries, followerSelectionType: ritualRessurect.RitualType);
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
    GameManager.GetInstance().StartCoroutine(ritualRessurect.DoRessurectRoutine());
  }

  public IEnumerator DoRessurectRoutine()
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
      if (allBrain != null && allBrain.CurrentTask != null && allBrain.CurrentTask is FollowerTask_AttendRitual currentTask)
        currentTask.WorshipTentacle();
    }
    yield return (object) new WaitForSeconds(0.5f);
    AudioManager.Instance.PlayOneShot("event:/rituals/resurrect");
    ++DataManager.Instance.ResurrectRitualCount;
    DataManager.Instance.Followers_Dead.Remove(ritualRessurect.resurrectingFollower._directInfoAccess);
    DataManager.Instance.Followers_Dead_IDs.Remove(ritualRessurect.resurrectingFollower._directInfoAccess.ID);
    DataManager.Instance.Followers_Imprisoned_IDs.Remove(ritualRessurect.resurrectingFollower._directInfoAccess.ID);
    foreach (Structures_Grave structuresGrave in StructureManager.GetAllStructuresOfType<Structures_Grave>(FollowerLocation.Base))
    {
      if (structuresGrave.Data.FollowerID == ritualRessurect.resurrectingFollower.Info.ID)
        structuresGrave.Data.FollowerID = -1;
    }
    foreach (Structures_Crypt structuresCrypt in StructureManager.GetAllStructuresOfType<Structures_Crypt>(FollowerLocation.Base))
    {
      if (structuresCrypt.Data.MultipleFollowerIDs.Contains(ritualRessurect.resurrectingFollower.Info.ID))
        structuresCrypt.WithdrawBody(ritualRessurect.resurrectingFollower.Info.ID);
    }
    foreach (Structures_Morgue structuresMorgue in StructureManager.GetAllStructuresOfType<Structures_Morgue>(FollowerLocation.Base))
    {
      if (structuresMorgue.Data.MultipleFollowerIDs.Contains(ritualRessurect.resurrectingFollower.Info.ID))
        structuresMorgue.WithdrawBody(ritualRessurect.resurrectingFollower.Info.ID);
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
    revivedFollower.SetOutfit(FollowerOutfitType.Follower, false);
    revivedFollower.Brain.CheckChangeState();
    revivedFollower.HideAllFollowerIcons();
    revivedFollower.Interaction_FollowerInteraction.eventListener.SetPitchAndVibrator(ritualRessurect.resurrectingFollower._directInfoAccess.follower_pitch, ritualRessurect.resurrectingFollower._directInfoAccess.follower_vibrato, ritualRessurect.resurrectingFollower._directInfoAccess.ID);
    ThoughtData thoughtData = (ThoughtData) null;
    float num1 = UnityEngine.Random.value;
    if (!revivedFollower.Brain._directInfoAccess.HadFuneral && TimeManager.CurrentDay > 20)
    {
      if ((double) num1 < 0.079999998211860657)
      {
        if (revivedFollower.Brain.Info.ID != 99996 || DataManager.Instance.SozoNoLongerBrainwashed)
        {
          revivedFollower.AddTrait(FollowerTrait.TraitType.Zombie, true);
          revivedFollower.Brain.CurrentState = (FollowerState) new FollowerState_Zombie();
          thoughtData = FollowerThoughts.GetData((Thought) UnityEngine.Random.Range(454, 458));
        }
      }
      else if ((double) num1 < 0.15000000596046448 && !revivedFollower.Brain.HasTrait(FollowerTrait.TraitType.ExistentialDread))
      {
        revivedFollower.AddTrait(FollowerTrait.TraitType.ExistentialDread, true);
        revivedFollower.Brain.CurrentState = (FollowerState) new FollowerState_ExistentialDread();
        thoughtData = FollowerThoughts.GetData((Thought) UnityEngine.Random.Range(410, 415));
      }
    }
    if (thoughtData != null)
    {
      thoughtData.Init();
      revivedFollower.Brain._directInfoAccess.Thoughts.Add(thoughtData);
    }
    if (!revivedFollower.Brain.HasTrait(FollowerTrait.TraitType.ExistentialDread))
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
      deadWorshipper1.Structure.Brain.Remove();
    GameManager.GetInstance().OnConversationNext(revivedFollower.gameObject, 8f);
    string animName = "Sermons/resurrect";
    if (revivedFollower.Brain.HasTrait(FollowerTrait.TraitType.Zombie))
      animName = "Zombie/zombie-resurrect";
    double num2 = (double) revivedFollower.SetBodyAnimation(animName, false);
    revivedFollower.AddBodyAnimation("Reactions/react-enlightened1", false, 0.0f);
    revivedFollower.AddBodyAnimation("idle", true, 0.0f);
    yield return (object) new WaitForSeconds(2f);
    Interaction_TempleAltar.Instance.PulseDisplacementObject(revivedFollower.transform.position);
    yield return (object) new WaitForSeconds(2f);
    Interaction_TempleAltar.Instance.SimpleSetCamera.Reset();
    PlayerFarming.Instance.simpleSpineAnimator.Animate("rituals/ritual-stop", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(0.6666667f);
    revivedFollower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_AttendTeaching());
    float num3 = 0.5f;
    foreach (FollowerBrain brain in Ritual.FollowerToAttendSermon)
    {
      float Delay = UnityEngine.Random.Range(0.1f, 0.5f);
      num3 += Delay;
      ritualRessurect.StartCoroutine(ritualRessurect.DelayFollowerReaction(brain, Delay));
    }
    yield return (object) new WaitForSeconds(1f);
    if (revivedFollower.Brain.HasTrait(FollowerTrait.TraitType.ExistentialDread))
    {
      ChurchFollowerManager.Instance.RemoveBrainFromAudience(revivedFollower.Brain);
      revivedFollower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ExistentialDread());
    }
    BiomeConstants.Instance.ChromaticAbberationTween(0.25f, 7f, BiomeConstants.Instance.ChromaticAberrationDefaultValue);
    ritualRessurect.EndRitual();
    yield return (object) new WaitForSeconds(0.5f);
    ritualRessurect.CompleteRitual(targetFollowerID_1: revivedFollower.Brain.Info.ID);
    yield return (object) new WaitForSeconds(1f);
    CultFaithManager.AddThought(Thought.Cult_Ressurection, revivedFollower.Brain.Info.ID);
  }

  public void EndRitual()
  {
    this.StopSacrificePortalEffect();
    ChurchFollowerManager.Instance.StopSacrificePortalEffect();
  }
}
