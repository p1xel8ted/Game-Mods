// Decompiled with JetBrains decompiler
// Type: RitualFightpit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using Lamb.UI.FollowerSelect;
using Lamb.UI.Tarot;
using src.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class RitualFightpit : Ritual
{
  public static Follower contestant1;
  public static Follower contestant2;
  public FollowerTask_ManualControl Task1;
  public FollowerTask_ManualControl Task2;
  public bool Waiting = true;

  public override UpgradeSystem.Type RitualType => UpgradeSystem.Type.Ritual_Fightpit;

  public override void Play()
  {
    base.Play();
    this.StartCoroutine((IEnumerator) this.RitualRoutine());
  }

  public IEnumerator RitualRoutine()
  {
    RitualFightpit ritualFightpit = this;
    AudioManager.Instance.PlayOneShot("event:/rituals/generic_start_ritual");
    yield return (object) ritualFightpit.StartCoroutine((IEnumerator) ritualFightpit.CentreAndAnimatePlayer());
    Interaction_TempleAltar.Instance.SimpleSetCamera.Play();
    yield return (object) ritualFightpit.StartCoroutine((IEnumerator) ritualFightpit.WaitFollowersFormCircle());
    PlayerFarming.Instance.simpleSpineAnimator.Animate("rituals/ritual-start", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("rituals/ritual-loop", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(1f);
    List<FollowerSelectEntry> followerSelectEntries = Ritual.GetFollowerSelectEntriesForSermon();
    UIFollowerSelectMenuController followerSelectInstance1 = MonoSingleton<UIManager>.Instance.FollowerSelectMenuTemplate.Instantiate<UIFollowerSelectMenuController>();
    followerSelectInstance1.VotingType = TwitchVoting.VotingType.RITUAL_FIGHT_PIT;
    followerSelectInstance1.Show(followerSelectEntries, followerSelectionType: ritualFightpit.RitualType);
    UIFollowerSelectMenuController selectMenuController1 = followerSelectInstance1;
    selectMenuController1.OnFollowerSelected = selectMenuController1.OnFollowerSelected + (Action<FollowerInfo>) (followerInfo =>
    {
      RitualFightpit.contestant1 = FollowerManager.FindFollowerByID(followerInfo.ID);
      AudioManager.Instance.PlayOneShot("event:/rituals/wedding_select_follower", PlayerFarming.Instance.gameObject);
      this.loopedSound = AudioManager.Instance.CreateLoop("event:/sermon/preach_loop", PlayerFarming.Instance.gameObject, true, false);
      this.Task1 = new FollowerTask_ManualControl();
      RitualFightpit.contestant1.Brain.HardSwapToTask((FollowerTask) this.Task1);
      this.StartCoroutine((IEnumerator) this.SetUpCombatant1Routine());
    });
    UIFollowerSelectMenuController selectMenuController2 = followerSelectInstance1;
    selectMenuController2.OnCancel = selectMenuController2.OnCancel + (System.Action) (() =>
    {
      AudioManager.Instance.StopLoop(this.loopedSound);
      GameManager.GetInstance().StartCoroutine((IEnumerator) this.RitualFinished(true, -1, -1));
      this.CancelFollowers();
    });
    UIFollowerSelectMenuController selectMenuController3 = followerSelectInstance1;
    selectMenuController3.OnHidden = selectMenuController3.OnHidden + (System.Action) (() => followerSelectInstance1 = (UIFollowerSelectMenuController) null);
    while ((UnityEngine.Object) followerSelectInstance1 != (UnityEngine.Object) null || ritualFightpit.Waiting)
      yield return (object) null;
    ritualFightpit.Waiting = true;
    foreach (FollowerSelectEntry followerSelectEntry in followerSelectEntries)
    {
      if (followerSelectEntry.FollowerInfo == RitualFightpit.contestant1.Brain._directInfoAccess)
      {
        followerSelectEntries.Remove(followerSelectEntry);
        break;
      }
    }
    followerSelectEntries.Add(new FollowerSelectEntry(RitualFightpit.contestant1, FollowerSelectEntry.Status.UnavailableInFightPit));
    UIFollowerSelectMenuController followerSelectInstance2 = MonoSingleton<UIManager>.Instance.FollowerSelectMenuTemplate.Instantiate<UIFollowerSelectMenuController>();
    followerSelectInstance2.VotingType = TwitchVoting.VotingType.RITUAL_FIGHT_PIT;
    followerSelectInstance2.Show(followerSelectEntries, followerSelectionType: ritualFightpit.RitualType);
    UIFollowerSelectMenuController selectMenuController4 = followerSelectInstance2;
    selectMenuController4.OnFollowerSelected = selectMenuController4.OnFollowerSelected + (Action<FollowerInfo>) (followerInfo =>
    {
      AudioManager.Instance.SetMusicBaseID(SoundConstants.BaseID.fight_pit_drums);
      RitualFightpit.contestant2 = FollowerManager.FindFollowerByID(followerInfo.ID);
      AudioManager.Instance.PlayOneShot("event:/rituals/wedding_select_follower", PlayerFarming.Instance.gameObject);
      this.loopedSound = AudioManager.Instance.CreateLoop("event:/sermon/preach_loop", PlayerFarming.Instance.gameObject, true, false);
      this.Task2 = new FollowerTask_ManualControl();
      RitualFightpit.contestant2.Brain.HardSwapToTask((FollowerTask) this.Task2);
      this.StartCoroutine((IEnumerator) this.SetUpCombatant2Routine());
    });
    UIFollowerSelectMenuController selectMenuController5 = followerSelectInstance2;
    selectMenuController5.OnCancel = selectMenuController5.OnCancel + (System.Action) (() =>
    {
      GameManager.GetInstance().StartCoroutine((IEnumerator) this.RitualFinished(true, -1, -1));
      this.CancelFollowers();
    });
    UIFollowerSelectMenuController selectMenuController6 = followerSelectInstance2;
    selectMenuController6.OnHidden = selectMenuController6.OnHidden + (System.Action) (() => followerSelectInstance2 = (UIFollowerSelectMenuController) null);
    while ((UnityEngine.Object) followerSelectInstance2 != (UnityEngine.Object) null || ritualFightpit.Waiting)
      yield return (object) null;
    Interaction_TempleAltar.Instance.CloseUpCamera.Play();
    yield return (object) new WaitForSeconds(1f);
    double num1 = (double) RitualFightpit.contestant1.SetBodyAnimation("Reactions/react-bow", false);
    double num2 = (double) RitualFightpit.contestant2.SetBodyAnimation("Reactions/react-bow", false);
    yield return (object) new WaitForSeconds(1.93333328f);
    int WaitCount = 0;
    ritualFightpit.Task1.GoToAndStop(RitualFightpit.contestant1, Interaction_TempleAltar.Instance.PortalEffect.transform.position + Vector3.right * 0.4f, (System.Action) (() => ++WaitCount));
    ritualFightpit.Task2.GoToAndStop(RitualFightpit.contestant2, Interaction_TempleAltar.Instance.PortalEffect.transform.position + Vector3.left * 0.4f, (System.Action) (() => ++WaitCount));
    while (WaitCount < 2)
      yield return (object) null;
    AudioManager.Instance.PlayOneShot("event:/rituals/fight_pit", RitualFightpit.contestant1.transform.position);
    double num3 = (double) RitualFightpit.contestant1.SetBodyAnimation("fight", true);
    double num4 = (double) RitualFightpit.contestant2.SetBodyAnimation("fight", true);
    ++RitualFightpit.contestant1.Brain._directInfoAccess.FightPitsFought;
    ++RitualFightpit.contestant2.Brain._directInfoAccess.FightPitsFought;
    BiomeConstants.Instance.EmitSmokeExplosionVFX(Vector3.Lerp(RitualFightpit.contestant1.transform.position, RitualFightpit.contestant2.transform.position, 0.5f));
    CameraManager.instance.ShakeCameraForDuration(0.1f, 1f, 3f);
    MMVibrate.Haptic(MMVibrate.HapticTypes.SoftImpact);
    yield return (object) new WaitForSeconds(3f);
    if ((double) UnityEngine.Random.value <= 0.5)
    {
      Follower contestant1 = RitualFightpit.contestant1;
      RitualFightpit.contestant1 = RitualFightpit.contestant2;
      RitualFightpit.contestant2 = contestant1;
    }
    if (RitualFightpit.contestant1.Brain.Info.Clothing == FollowerClothingType.Warrior || RitualFightpit.contestant1.Brain.Info.Clothing == FollowerClothingType.Special_2)
    {
      Follower contestant1 = RitualFightpit.contestant1;
      RitualFightpit.contestant1 = RitualFightpit.contestant2;
      RitualFightpit.contestant2 = contestant1;
    }
    foreach (StoryData storyObjective in DataManager.Instance.StoryObjectives)
    {
      foreach (StoryDataItem storyDataItem in Quests.GetChildStoryDataItemsFromStoryDataItem(storyObjective.EntryStoryItem))
      {
        foreach (ObjectivesData objective in DataManager.Instance.Objectives)
        {
          if (storyDataItem.Objective != null && objective.Index == storyDataItem.Objective.Index && storyDataItem.Objective is Objectives_PerformRitual && ((Objectives_PerformRitual) storyDataItem.Objective).Ritual == UpgradeSystem.Type.Ritual_Fightpit && (RitualFightpit.contestant1.Brain.Info.ID == ((Objectives_PerformRitual) storyDataItem.Objective).TargetFollowerID_1 && RitualFightpit.contestant2.Brain.Info.ID == ((Objectives_PerformRitual) storyDataItem.Objective).TargetFollowerID_2 || RitualFightpit.contestant1.Brain.Info.ID == ((Objectives_PerformRitual) storyDataItem.Objective).TargetFollowerID_2 && RitualFightpit.contestant2.Brain.Info.ID == ((Objectives_PerformRitual) storyDataItem.Objective).TargetFollowerID_1) && RitualFightpit.contestant1.Brain.Info.ID == ((Objectives_PerformRitual) storyDataItem.Objective).TargetFollowerID_1)
          {
            Follower contestant1 = RitualFightpit.contestant1;
            RitualFightpit.contestant1 = RitualFightpit.contestant2;
            RitualFightpit.contestant2 = contestant1;
            break;
          }
        }
      }
    }
    double num5 = (double) RitualFightpit.contestant1.SetBodyAnimation("fight-lose", false);
    RitualFightpit.contestant1.AddBodyAnimation("unconverted", true, 0.0f);
    double num6 = (double) RitualFightpit.contestant2.SetBodyAnimation("fight-win", false);
    RitualFightpit.contestant2.AddBodyAnimation("cheer", true, 0.0f);
    --RitualFightpit.contestant1.Brain._directInfoAccess.FightPitWinStreak;
    if (RitualFightpit.contestant1.Brain._directInfoAccess.FightPitWinStreak < -3)
      RitualFightpit.contestant1.Brain._directInfoAccess.FightPitWinStreak = -3;
    ++RitualFightpit.contestant2.Brain._directInfoAccess.FightPitWinStreak;
    if (RitualFightpit.contestant1.Brain._directInfoAccess.FightPitWinStreak > 3)
      RitualFightpit.contestant1.Brain._directInfoAccess.FightPitWinStreak = 3;
    if (RitualFightpit.contestant1.Brain._directInfoAccess.FightPitWinStreak <= -3 && !RitualFightpit.contestant1.Brain.HasTrait(FollowerTrait.TraitType.Scared) && !RitualFightpit.contestant1.Brain.HasTrait(FollowerTrait.TraitType.Argumentative))
      RitualFightpit.contestant1.AddTrait(FollowerTrait.TraitType.Scared, true);
    if (RitualFightpit.contestant2.Brain._directInfoAccess.FightPitWinStreak >= 3 && !RitualFightpit.contestant2.Brain.HasTrait(FollowerTrait.TraitType.Argumentative) && !RitualFightpit.contestant2.Brain.HasTrait(FollowerTrait.TraitType.Scared))
      RitualFightpit.contestant2.AddTrait(FollowerTrait.TraitType.Argumentative, true);
    yield return (object) new WaitForSeconds(1.1f);
    AudioManager.Instance.SetMusicBaseID(SoundConstants.BaseID.Temple);
    Interaction_TempleAltar.Instance.PulseDisplacementObject(Interaction_TempleAltar.Instance.PortalEffect.transform.position - Vector3.back);
    CameraManager.instance.ShakeCameraForDuration(0.8f, 1f, 0.2f);
    yield return (object) new WaitForSeconds(0.9f);
    Interaction_TempleAltar.Instance.SimpleSetCamera.Play();
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      if (followerBrain.CurrentTask is FollowerTask_AttendRitual)
        (followerBrain.CurrentTask as FollowerTask_AttendRitual).Cheer();
    }
    yield return (object) new WaitForSeconds(1.5f);
    Interaction_TempleAltar.Instance.SimpleSetCamera.Reset();
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.gameObject);
    yield return (object) new WaitForSeconds(0.5f);
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      if (followerBrain.CurrentTaskType == FollowerTaskType.AttendTeaching)
        (followerBrain.CurrentTask as FollowerTask_AttendRitual).WorshipTentacle();
    }
    GameObject g = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/UI/Choice Indicator"), GameObject.FindWithTag("Canvas").transform) as GameObject;
    ChoiceIndicator c = g.GetComponent<ChoiceIndicator>();
    c.Offset = new Vector3(0.0f, -350f);
    c.Show("<sprite name=\"icon_ThumbsUp\">", "<sprite name=\"icon_ThumbsDown\">", (System.Action) (() => this.StartCoroutine((IEnumerator) this.ShowMercy())), (System.Action) (() => this.StartCoroutine((IEnumerator) this.Execute())), PlayerFarming.Instance.transform.position);
    while ((UnityEngine.Object) g != (UnityEngine.Object) null)
    {
      c.UpdatePosition(PlayerFarming.Instance.transform.position);
      yield return (object) null;
    }
  }

  public void CheckGiveOutfit()
  {
    ++DataManager.Instance.FightPitRituals;
    if (DataManager.Instance.FightPitRituals < 3 || !DataManager.Instance.TailorEnabled || DataManager.Instance.UnlockedClothing.Contains(FollowerClothingType.Warrior) || FoundItemPickUp.IsOutfitPickUpActive(FollowerClothingType.Warrior))
      return;
    InventoryItem.Spawn(InventoryItem.ITEM_TYPE.FOUND_ITEM_OUTFIT, 1, RitualFightpit.contestant1.transform.position).GetComponent<FoundItemPickUp>().clothingType = FollowerClothingType.Warrior;
  }

  public IEnumerator SetUpCombatant1Routine()
  {
    RitualFightpit ritualFightpit = this;
    yield return (object) null;
    ChurchFollowerManager.Instance.RemoveBrainFromAudience(RitualFightpit.contestant1.Brain);
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.CurrentTaskType == FollowerTaskType.AttendTeaching)
      {
        allBrain.CurrentTask.RecalculateDestination();
        allBrain.CurrentTask.Setup(FollowerManager.FindFollowerByID(allBrain.Info.ID));
      }
    }
    ritualFightpit.Task1.GoToAndStop(RitualFightpit.contestant1, Interaction_TempleAltar.Instance.PortalEffect.transform.position + Vector3.right * 1f, new System.Action(ritualFightpit.\u003CSetUpCombatant1Routine\u003Eb__10_0));
  }

  public IEnumerator SetUpCombatant2Routine()
  {
    RitualFightpit ritualFightpit = this;
    yield return (object) null;
    ChurchFollowerManager.Instance.RemoveBrainFromAudience(RitualFightpit.contestant2.Brain);
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.CurrentTaskType == FollowerTaskType.AttendTeaching)
      {
        allBrain.CurrentTask.RecalculateDestination();
        allBrain.CurrentTask.Setup(FollowerManager.FindFollowerByID(allBrain.Info.ID));
      }
    }
    ritualFightpit.Task2.GoToAndStop(RitualFightpit.contestant2, Interaction_TempleAltar.Instance.PortalEffect.transform.position + Vector3.left * 1f, new System.Action(ritualFightpit.\u003CSetUpCombatant2Routine\u003Eb__11_0));
  }

  public IEnumerator ShowMercy()
  {
    GameManager.GetInstance().OnConversationNext(RitualFightpit.contestant1.gameObject);
    GameManager.GetInstance().AddToCamera(RitualFightpit.contestant2.gameObject);
    yield return (object) new WaitForSeconds(1f);
    double num1 = (double) RitualFightpit.contestant1.SetBodyAnimation("Conversations/greet-nice", false);
    RitualFightpit.contestant1.AddBodyAnimation("idle", true, 0.0f);
    double num2 = (double) RitualFightpit.contestant2.SetBodyAnimation("Conversations/greet-hate", false);
    RitualFightpit.contestant2.AddBodyAnimation("idle", true, 0.0f);
    if (!TarotCards.IsUnlocked(TarotCards.Card.Lovers2))
    {
      Interaction_TarotCardUnlock objectOfType = UnityEngine.Object.FindObjectOfType<Interaction_TarotCardUnlock>();
      if ((UnityEngine.Object) objectOfType == (UnityEngine.Object) null || (bool) (UnityEngine.Object) objectOfType && objectOfType.CardOverride != TarotCards.Card.Lovers2)
      {
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.TRINKET_CARD_UNLOCKED, 1, RitualFightpit.contestant1.transform.position).GetComponent<Interaction_TarotCardUnlock>().CardOverride = TarotCards.Card.Lovers2;
        yield return (object) new WaitForSeconds(1f);
      }
    }
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      followerBrain.AddThought(Thought.FightPitMercy);
      if (followerBrain.CurrentTask is FollowerTask_AttendRitual)
        (followerBrain.CurrentTask as FollowerTask_AttendRitual).Boo();
    }
    yield return (object) new WaitForSeconds(2f);
    RitualFightpit.contestant1.Brain.AddAdoration(FollowerBrain.AdorationActions.FightPitMercy, (System.Action) null);
    ChurchFollowerManager.Instance.AddBrainToAudience(RitualFightpit.contestant1.Brain);
    ChurchFollowerManager.Instance.AddBrainToAudience(RitualFightpit.contestant2.Brain);
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.EndRitual(RitualFightpit.contestant1.Brain.Info.ID, RitualFightpit.contestant2.Brain.Info.ID));
    this.CheckGiveOutfit();
  }

  public IEnumerator Execute()
  {
    int followerID_1 = RitualFightpit.contestant1.Brain.Info.ID;
    int followerID_2 = RitualFightpit.contestant2.Brain.Info.ID;
    GameManager.GetInstance().OnConversationNext(RitualFightpit.contestant1.gameObject, 6f);
    GameManager.GetInstance().AddToCamera(RitualFightpit.contestant2.gameObject);
    yield return (object) new WaitForSeconds(0.5f);
    Ritual.FollowerToAttendSermon.Remove(RitualFightpit.contestant1.Brain);
    double num = (double) RitualFightpit.contestant2.SetBodyAnimation("fight-kill", false);
    RitualFightpit.contestant2.AddBodyAnimation("cheer", true, 0.0f);
    yield return (object) new WaitForSeconds(1.1f);
    AudioManager.Instance.PlayOneShot("event:/rituals/fight_pit_kill", RitualFightpit.contestant2.transform.position);
    float seconds = RitualFightpit.contestant1.SetBodyAnimation("fight-die", false);
    Interaction_TempleAltar.Instance.PulseDisplacementObject(RitualFightpit.contestant1.CameraBone.transform.position - Vector3.back);
    CameraManager.instance.ShakeCameraForDuration(0.8f, 1f, 0.2f);
    yield return (object) new WaitForSeconds(seconds);
    AudioManager.Instance.PlayOneShot("event:/followers/pop_in", RitualFightpit.contestant1.transform.position);
    if (RitualFightpit.contestant1.Brain.HasTrait(FollowerTrait.TraitType.Mutated))
    {
      AudioManager.Instance.PlayOneShot("event:/player/harvest_meat_done", RitualFightpit.contestant1.transform.position);
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.MAGMA_STONE, 10, RitualFightpit.contestant1.transform.position);
    }
    else if (!RitualFightpit.contestant1.Brain.Info.IsSnowman)
    {
      AudioManager.Instance.PlayOneShot("event:/player/harvest_meat_done", RitualFightpit.contestant1.transform.position);
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.FOLLOWER_MEAT, 3, RitualFightpit.contestant1.transform.position);
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BONE, 2, RitualFightpit.contestant1.transform.position);
    }
    BiomeConstants.Instance.EmitSmokeInteractionVFX(RitualFightpit.contestant1.transform.position, new Vector3(0.5f, 0.5f, 1f));
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
    FollowerManager.FollowerDie(RitualFightpit.contestant1.Brain.Info.ID, NotificationCentre.NotificationType.KilledInAFightPit);
    this.CheckGiveOutfit();
    UnityEngine.Object.Destroy((UnityEngine.Object) RitualFightpit.contestant1.gameObject);
    ChurchFollowerManager.Instance.AddBrainToAudience(RitualFightpit.contestant2.Brain);
    yield return (object) null;
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      if (followerBrain.CurrentTaskType == FollowerTaskType.AttendTeaching)
        (followerBrain.CurrentTask as FollowerTask_AttendRitual).Cheer();
    }
    yield return (object) new WaitForSeconds(2f);
    RitualFightpit.contestant2.Brain.AddAdoration(FollowerBrain.AdorationActions.FightPitDeath, (System.Action) null);
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.EndRitual(followerID_1, followerID_2));
  }

  public IEnumerator EndRitual(int follower1, int follower2)
  {
    RitualFightpit ritualFightpit = this;
    JudgementMeter.ShowModify(-1);
    AudioManager.Instance.StopLoop(ritualFightpit.loopedSound);
    float EndingDelay = 0.0f;
    yield return (object) null;
    foreach (FollowerBrain brain in Ritual.GetFollowersAvailableToAttendSermon())
    {
      float Delay = UnityEngine.Random.Range(0.1f, 0.5f);
      EndingDelay += Delay;
      ritualFightpit.StartCoroutine((IEnumerator) ritualFightpit.DelayFollowerReaction(brain, Delay));
    }
    yield return (object) new WaitForSeconds(1f);
    GameManager.GetInstance().StartCoroutine((IEnumerator) ritualFightpit.RitualFinished(false, follower1, follower2));
  }

  public IEnumerator RitualFinished(bool cancelled, int follower1, int follower2)
  {
    RitualFightpit ritualFightpit = this;
    AudioManager.Instance.SetMusicBaseID(SoundConstants.BaseID.Temple);
    AudioManager.Instance.StopLoop(ritualFightpit.loopedSound);
    ritualFightpit.CompleteRitual(cancelled, follower1, follower2);
    Interaction_TempleAltar.Instance.SimpleSetCamera.Reset();
    yield return (object) new WaitForSeconds(1f);
    if (!cancelled)
      CultFaithManager.AddThought(Thought.Cult_FightPit);
  }

  [CompilerGenerated]
  public void \u003CSetUpCombatant1Routine\u003Eb__10_0()
  {
    RitualFightpit.contestant1.FacePosition(Interaction_TempleAltar.Instance.PortalEffect.transform.position);
    double num = (double) RitualFightpit.contestant1.SetBodyAnimation("idle", true);
    this.Waiting = false;
  }

  [CompilerGenerated]
  public void \u003CSetUpCombatant2Routine\u003Eb__11_0()
  {
    RitualFightpit.contestant2.FacePosition(Interaction_TempleAltar.Instance.PortalEffect.transform.position);
    double num = (double) RitualFightpit.contestant2.SetBodyAnimation("idle", true);
    this.Waiting = false;
  }
}
