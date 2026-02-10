// Decompiled with JetBrains decompiler
// Type: RitualSacrifice
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.FollowerSelect;
using MMTools;
using Spine;
using src.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class RitualSacrifice : Ritual
{
  public Follower sacrificeFollower;
  public Light RitualLight;
  public InventoryItem.ITEM_TYPE necklace;

  public override UpgradeSystem.Type RitualType => UpgradeSystem.Type.Ritual_Sacrifice;

  public override void Play()
  {
    base.Play();
    this.StartCoroutine((IEnumerator) this.SacrificeFollowerRoutine());
  }

  public IEnumerator SacrificeFollowerRoutine()
  {
    RitualSacrifice ritualSacrifice = this;
    yield return (object) ritualSacrifice.StartCoroutine((IEnumerator) ritualSacrifice.CentreAndAnimatePlayer());
    Interaction_TempleAltar.Instance.SimpleSetCamera.Play();
    Debug.Log((object) "Ritual sacrifice begin gather");
    yield return (object) ritualSacrifice.StartCoroutine((IEnumerator) ritualSacrifice.WaitFollowersFormCircle());
    Debug.Log((object) "Ritual sacrifice end gather");
    PlayerFarming.Instance.simpleSpineAnimator.Animate("rituals/ritual-start", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("rituals/ritual-loop", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(1f);
    bool Cancelled = false;
    UIFollowerSelectMenuController followerSelectInstance = (UIFollowerSelectMenuController) null;
    followerSelectInstance = GameManager.GetInstance().UpgradePlayerConfiguration.HasUnlockAvailable(true) ? MonoSingleton<UIManager>.Instance.SacrificeFollowerMenuTemplate.Instantiate<UIFollowerSelectMenuController>() : MonoSingleton<UIManager>.Instance.FollowerSelectMenuTemplate.Instantiate<UIFollowerSelectMenuController>();
    followerSelectInstance.VotingType = TwitchVoting.VotingType.RITUAL_SACRIFICE;
    followerSelectInstance.Show(Ritual.GetFollowerSelectEntriesForSermon(), followerSelectionType: ritualSacrifice.RitualType);
    UIFollowerSelectMenuController selectMenuController1 = followerSelectInstance;
    selectMenuController1.OnFollowerSelected = selectMenuController1.OnFollowerSelected + (Action<FollowerInfo>) (followerInfo =>
    {
      this.sacrificeFollower = FollowerManager.FindFollowerByID(followerInfo.ID);
      UIManager.PlayAudio("event:/ritual_sacrifice/ritual_begin");
    });
    UIFollowerSelectMenuController selectMenuController2 = followerSelectInstance;
    selectMenuController2.OnShow = selectMenuController2.OnShow + (System.Action) (() =>
    {
      foreach (FollowerInformationBox followerInfoBox in followerSelectInstance.FollowerInfoBoxes)
      {
        if (followerInfoBox.followBrain.Info.SacrificialType == InventoryItem.ITEM_TYPE.NONE)
          followerInfoBox.followBrain.Info.SacrificialType = InventoryItem.ITEM_TYPE.BLACK_GOLD;
        if (!GameManager.GetInstance().UpgradePlayerConfiguration.HasUnlockAvailable(true))
        {
          if (followerInfoBox.followBrain.Info.SacrificialValue > 0)
            followerInfoBox.ShowRewardItem(followerInfoBox.followBrain.Info.SacrificialType, followerInfoBox.followBrain.Info.SacrificialValue);
          else
            followerInfoBox.ShowCostItem(InventoryItem.ITEM_TYPE.BLACK_GOLD, followerInfoBox.followBrain.Info.SacrificialValue);
        }
        else
          followerInfoBox.ShowDevotionGain(RitualSacrifice.GetDevotionGain(followerInfoBox.followBrain.Info.XPLevel));
      }
    });
    UIFollowerSelectMenuController selectMenuController3 = followerSelectInstance;
    selectMenuController3.OnShownCompleted = selectMenuController3.OnShownCompleted + (System.Action) (() =>
    {
      foreach (FollowerInformationBox followerInfoBox in followerSelectInstance.FollowerInfoBoxes)
      {
        if (followerInfoBox.followBrain.Info.SacrificialType == InventoryItem.ITEM_TYPE.NONE)
          followerInfoBox.followBrain.Info.SacrificialType = InventoryItem.ITEM_TYPE.BLACK_GOLD;
        if (!GameManager.GetInstance().UpgradePlayerConfiguration.HasUnlockAvailable(true))
        {
          if (followerInfoBox.followBrain.Info.SacrificialValue > 0)
            followerInfoBox.ShowRewardItem(followerInfoBox.followBrain.Info.SacrificialType, followerInfoBox.followBrain.Info.SacrificialValue);
          else
            followerInfoBox.ShowCostItem(InventoryItem.ITEM_TYPE.BLACK_GOLD, followerInfoBox.followBrain.Info.SacrificialValue);
        }
        else
          followerInfoBox.ShowDevotionGain(RitualSacrifice.GetDevotionGain(followerInfoBox.followBrain.Info.XPLevel));
      }
    });
    UIFollowerSelectMenuController selectMenuController4 = followerSelectInstance;
    selectMenuController4.OnCancel = selectMenuController4.OnCancel + (System.Action) (() =>
    {
      GameManager.GetInstance().StartCoroutine((IEnumerator) this.EndRitual());
      Cancelled = true;
      this.CompleteRitual(true);
      this.CancelFollowers();
    });
    UIFollowerSelectMenuController selectMenuController5 = followerSelectInstance;
    selectMenuController5.OnHidden = selectMenuController5.OnHidden + (System.Action) (() => followerSelectInstance = (UIFollowerSelectMenuController) null);
    while ((UnityEngine.Object) followerSelectInstance != (UnityEngine.Object) null && !Cancelled)
      yield return (object) null;
    if (!Cancelled)
    {
      AudioManager.Instance.PlayOneShot("event:/ritual_sacrifice/select_follower", PlayerFarming.Instance.gameObject);
      ritualSacrifice.sacrificeFollower.Brain.CompleteCurrentTask();
      FollowerTask_ManualControl nextTask = new FollowerTask_ManualControl();
      ritualSacrifice.sacrificeFollower.Brain.HardSwapToTask((FollowerTask) nextTask);
      ritualSacrifice.sacrificeFollower.Brain.InRitual = true;
      yield return (object) null;
      ritualSacrifice.sacrificeFollower.SetOutfit(ritualSacrifice.sacrificeFollower.Brain.Info.Outfit, true);
      ritualSacrifice.sacrificeFollower.HoodOff(onComplete: (System.Action) (() =>
      {
        ChurchFollowerManager.Instance.RemoveBrainFromAudience(this.sacrificeFollower.Brain);
        foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
        {
          if (allBrain.CurrentTaskType == FollowerTaskType.AttendTeaching)
          {
            allBrain.CurrentTask.RecalculateDestination();
            allBrain.CurrentTask.Setup(FollowerManager.FindFollowerByID(allBrain.Info.ID));
          }
        }
        this.sacrificeFollower.Spine.AnimationState.SetAnimation(1, "walk", true);
        this.sacrificeFollower.gameObject.transform.DOMove(Interaction_TempleAltar.Instance.PortalEffect.transform.position, 2.5f).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
        {
          Interaction_TempleAltar.Instance.SimpleSetCamera.Reset();
          GameManager.GetInstance().StartCoroutine((IEnumerator) this.DoSacrificeRoutine());
        }));
      }));
      GameManager.GetInstance().OnConversationNext(ritualSacrifice.sacrificeFollower.gameObject);
    }
  }

  public static int GetDevotionGain(int XPLevel)
  {
    return Mathf.Clamp(40 + (Mathf.Clamp(XPLevel, 1, 10) - 1) * 20, 30, 100);
  }

  public IEnumerator DoSacrificeRoutine()
  {
    RitualSacrifice ritualSacrifice = this;
    if ((UnityEngine.Object) ritualSacrifice.RitualLight != (UnityEngine.Object) null)
    {
      ritualSacrifice.RitualLight.gameObject.SetActive(true);
      ritualSacrifice.RitualLight.color = Color.black;
      ritualSacrifice.RitualLight.DOColor(Color.red, 1f);
      ritualSacrifice.RitualLight.DOIntensity(1.5f, 1f);
    }
    ritualSacrifice.necklace = ritualSacrifice.sacrificeFollower.Brain.Info.Necklace;
    bool isSecret = ritualSacrifice.sacrificeFollower.Brain.Info.Necklace == InventoryItem.ITEM_TYPE.Necklace_Light && !DataManager.Instance.HasBaalSkin || ritualSacrifice.sacrificeFollower.Brain.Info.Necklace == InventoryItem.ITEM_TYPE.Necklace_Dark && !DataManager.Instance.HasAymSkin;
    GameManager.GetInstance().OnConversationNext(ritualSacrifice.sacrificeFollower.gameObject, 4f);
    ChurchFollowerManager.Instance.PlayOverlay(ChurchFollowerManager.OverlayType.Sacrifice, "1");
    ritualSacrifice.sacrificeFollower.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(ritualSacrifice.HandleSacrificeAnimationStateEvent);
    ritualSacrifice.sacrificeFollower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    if (!isSecret)
      Ritual.FollowerToAttendSermon.Remove(ritualSacrifice.sacrificeFollower.Brain);
    ChurchFollowerManager.Instance.SacrificeTentacles.gameObject.SetActive(true);
    yield return (object) new WaitForEndOfFrame();
    ChurchFollowerManager.Instance.SacrificeTentacles.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(ritualSacrifice.HandleSacrificeAnimationStateEvent);
    if (isSecret)
    {
      ChurchFollowerManager.Instance.SacrificeTentacles.AnimationState.SetAnimation(0, "sacrifice-tentacles-secret", false);
      double num = (double) ritualSacrifice.sacrificeFollower.SetBodyAnimation("sacrifice-tentacles-secret", false);
      ritualSacrifice.sacrificeFollower.AddBodyAnimation("Forneus/scared", true, 0.0f);
    }
    else if (DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.SacrificeEnthusiast) && ritualSacrifice.sacrificeFollower.Brain.Info.CursedState != Thought.Dissenter)
    {
      ChurchFollowerManager.Instance.SacrificeTentacles.AnimationState.SetAnimation(0, "sacrifice-tentacles", false);
      double num = (double) ritualSacrifice.sacrificeFollower.SetBodyAnimation("sacrifice-tentacles", false);
    }
    else
    {
      ChurchFollowerManager.Instance.SacrificeTentacles.AnimationState.SetAnimation(0, "sacrifice-tentacles-scared", false);
      double num = (double) ritualSacrifice.sacrificeFollower.SetBodyAnimation("sacrifice-tentacles-scared", false);
    }
    int followerID = ritualSacrifice.sacrificeFollower.Brain.Info.ID;
    yield return (object) new WaitForSeconds(0.5f);
    ritualSacrifice.PlaySacrificePortalEffect();
    Interaction_TempleAltar.Instance.PulseDisplacementObject(Interaction_TempleAltar.Instance.PortalEffect.transform.position);
    BiomeConstants.Instance.ChromaticAbberationTween(1f, BiomeConstants.Instance.ChromaticAberrationDefaultValue, 7f);
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.CurrentTaskType == FollowerTaskType.AttendTeaching && allBrain.CurrentTask != null)
        (allBrain.CurrentTask as FollowerTask_AttendRitual).WorshipTentacle();
    }
    yield return (object) new WaitForSeconds(1.5f);
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.CurrentTaskType == FollowerTaskType.AttendTeaching && allBrain.CurrentTask != null)
        (allBrain.CurrentTask as FollowerTask_AttendRitual).Cheer();
    }
    ChurchFollowerManager.Instance.StartRitualOverlay();
    AudioManager.Instance.PlayOneShot("event:/ritual_sacrifice/ritual_end", PlayerFarming.Instance.gameObject);
    yield return (object) new WaitForSeconds(0.7f);
    Interaction_TempleAltar.Instance.PulseDisplacementObject(ritualSacrifice.sacrificeFollower.CameraBone.transform.position);
    yield return (object) new WaitForSeconds(3.16666651f);
    Interaction_TempleAltar.Instance.PulseDisplacementObject(Interaction_TempleAltar.Instance.PortalEffect.transform.position);
    if (isSecret)
      ritualSacrifice.sacrificeFollower.Outfit.SetOutfit(ritualSacrifice.sacrificeFollower.Spine, ritualSacrifice.sacrificeFollower.Outfit.CurrentOutfit, InventoryItem.ITEM_TYPE.NONE, false);
    yield return (object) new WaitForSeconds(0.3f);
    GameManager.GetInstance().OnConversationNext(Interaction_TempleAltar.Instance.RitualCameraPosition, 8f);
    yield return (object) new WaitForSeconds(0.5f);
    if (isSecret)
    {
      yield return (object) new WaitForSeconds(9f);
      GameManager.GetInstance().OnConversationNext(ritualSacrifice.sacrificeFollower.gameObject, 4f);
      foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
      {
        if (allBrain.CurrentTaskType == FollowerTaskType.AttendTeaching)
          (allBrain.CurrentTask as FollowerTask_AttendRitual).Pray();
      }
      yield return (object) new WaitForSeconds(2f);
      List<ConversationEntry> Entries = new List<ConversationEntry>()
      {
        new ConversationEntry(ritualSacrifice.gameObject, ""),
        new ConversationEntry(ritualSacrifice.gameObject, ""),
        new ConversationEntry(ritualSacrifice.gameObject, "")
      };
      for (int index = 0; index < Entries.Count; ++index)
      {
        string translation = LocalizationManager.GetTranslation($"Conversation_NPC/{(ritualSacrifice.sacrificeFollower.Brain.Info.ID == 99994 ? (object) "Baal" : (object) "Aym")}/Intro/{index}");
        Entries[index].SkeletonData = ritualSacrifice.sacrificeFollower.Spine;
        Entries[index].TermToSpeak = translation;
        Entries[index].CharacterName = ritualSacrifice.sacrificeFollower.Brain.Info.ID == 99994 ? ScriptLocalization.NAMES.Guardian1 : ScriptLocalization.NAMES.Guardian2;
        Entries[index].soundPath = ritualSacrifice.sacrificeFollower.Brain.Info.ID == 99994 ? "event:/dialogue/followers/boss/fol_guardian_b" : "event:/dialogue/followers/boss/fol_guardian_a";
        Entries[index].Speaker = ritualSacrifice.sacrificeFollower.gameObject;
        Entries[index].SetZoom = true;
        Entries[index].Zoom = 6f;
      }
      if (ritualSacrifice.sacrificeFollower.Brain.Info.ID != 99994)
        Entries.RemoveAt(Entries.Count - 1);
      bool waiting = true;
      MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) (() => waiting = false)), false);
      while (waiting)
        yield return (object) null;
      GameManager.GetInstance().OnConversationNext(Interaction_TempleAltar.Instance.RitualCameraPosition, 8f);
    }
    else
    {
      if (!GameManager.GetInstance().UpgradePlayerConfiguration.HasUnlockAvailable(true))
      {
        yield return (object) ritualSacrifice.StartCoroutine((IEnumerator) ritualSacrifice.GiveSoulsRoutines(Interaction_TempleAltar.Instance.PortalEffect.transform, ritualSacrifice.sacrificeFollower.Brain.Info.SacrificialValue, followerID));
      }
      else
      {
        double xpBySermon = (double) DoctrineUpgradeSystem.GetXPBySermon(SermonCategory.PlayerUpgrade);
        float f = Mathf.Ceil(DoctrineUpgradeSystem.GetXPTargetBySermon(SermonCategory.PlayerUpgrade) * ((float) RitualSacrifice.GetDevotionGain(ritualSacrifice.sacrificeFollower.Brain.Info.XPLevel) / 100f) * 10f);
        yield return (object) ritualSacrifice.StartCoroutine((IEnumerator) ritualSacrifice.UpgradePlayer(Mathf.RoundToInt(f)));
      }
      yield return (object) new WaitForSeconds(1f);
      GameManager.GetInstance().OnConversationNext(Interaction_TempleAltar.Instance.RitualCameraPosition, 6f);
      if (!ritualSacrifice.sacrificeFollower.Brain.Info.IsSnowman && !ritualSacrifice.sacrificeFollower.Brain.HasTrait(FollowerTrait.TraitType.Mutated))
        yield return (object) ritualSacrifice.StartCoroutine((IEnumerator) ritualSacrifice.GiveBonesAndMeat(Interaction_TempleAltar.Instance.PortalEffect.transform));
    }
    yield return (object) new WaitForSeconds(0.5f);
    ritualSacrifice.StopSacrificePortalEffect();
    ritualSacrifice.sacrificeFollower.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(ritualSacrifice.HandleSacrificeAnimationStateEvent);
    ChurchFollowerManager.Instance.SacrificeTentacles.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(ritualSacrifice.HandleSacrificeAnimationStateEvent);
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      if (ritualSacrifice.sacrificeFollower.Brain.Info.CursedState == Thought.OldAge)
        followerBrain.AddThought(Thought.SacrificedOldFollower);
    }
    if (!isSecret)
    {
      FollowerManager.FollowerDie(ritualSacrifice.sacrificeFollower.Brain.Info.ID, NotificationCentre.NotificationType.SacrificeFollower);
      UnityEngine.Object.Destroy((UnityEngine.Object) ritualSacrifice.sacrificeFollower.gameObject);
    }
    PlayerFarming.Instance.simpleSpineAnimator.Animate("rituals/ritual-stop", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    float num1 = 0.5f;
    foreach (FollowerBrain brain in Ritual.FollowerToAttendSermon)
    {
      float Delay = UnityEngine.Random.Range(0.1f, 0.5f);
      num1 += Delay;
      ritualSacrifice.StartCoroutine((IEnumerator) ritualSacrifice.DelayFollowerReaction(brain, Delay));
    }
    yield return (object) new WaitForSeconds(1.5f);
    JudgementMeter.ShowModify(DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.SacrificeEnthusiast) ? 1 : -1);
    yield return (object) new WaitForSeconds(2f);
    ChurchFollowerManager.Instance.EndRitualOverlay();
    ChurchFollowerManager.Instance.SacrificeTentacles.gameObject.SetActive(false);
    if (isSecret)
    {
      ChurchFollowerManager.Instance.AddBrainToAudience(ritualSacrifice.sacrificeFollower.Brain);
      ritualSacrifice.sacrificeFollower.Brain.InRitual = false;
    }
    Interaction_TempleAltar.Instance.SimpleSetCamera.Reset();
    ++DataManager.Instance.STATS_Sacrifices;
    ritualSacrifice.CompleteRitual(targetFollowerID_1: followerID);
    yield return (object) new WaitForSeconds(1f);
    if (DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.SacrificeEnthusiast))
      CultFaithManager.AddThought(Thought.Cult_Sacrifice_Trait, followerID);
    else
      CultFaithManager.AddThought(Thought.Cult_Sacrifice, followerID);
  }

  public IEnumerator EndRitual()
  {
    Interaction_TempleAltar.Instance.SimpleSetCamera.Reset();
    PlayerFarming.Instance.simpleSpineAnimator.Animate("rituals/ritual-stop", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(0.6666667f);
  }

  public void HandleSacrificeAnimationStateEvent(TrackEntry trackEntry, Spine.Event e)
  {
    switch (e.Data.Name)
    {
      case "SFX/tentacleSecret":
        AudioManager.Instance.PlayOneShot("event:/ritual_sacrifice/just_tentacles", this.sacrificeFollower.CameraBone.gameObject);
        break;
      case "SECRET_SKIN_SWAP":
        int id = this.sacrificeFollower.Brain._directInfoAccess.ID;
        if (this.necklace == InventoryItem.ITEM_TYPE.Necklace_Light && !DataManager.Instance.HasBaalSkin)
        {
          DataManager.Instance.Followers_Dead.Insert(0, this.sacrificeFollower.Brain._directInfoAccess);
          DataManager.Instance.Followers_Dead_IDs.Insert(0, this.sacrificeFollower.Brain._directInfoAccess.ID);
          FollowerInfo info = FollowerInfo.NewCharacter(FollowerLocation.Church);
          info.Traits.AddRange((IEnumerable<FollowerTrait.TraitType>) this.sacrificeFollower.Brain._directInfoAccess.Traits);
          if (info.Traits.Contains(FollowerTrait.TraitType.Mutated))
            info.Traits.Remove(FollowerTrait.TraitType.Mutated);
          info.TraitsSet = true;
          info.ID = 99994;
          info.Name = ScriptLocalization.NAMES.Guardian1;
          info.SkinName = "Boss Baal";
          info.Necklace = InventoryItem.ITEM_TYPE.NONE;
          info.SkinColour = 0;
          info.SkinVariation = 0;
          info.Clothing = FollowerClothingType.None;
          info.ClothingVariant = string.Empty;
          info.Outfit = FollowerOutfitType.None;
          for (int index = 0; index < WorshipperData.Instance.Characters.Count; ++index)
          {
            if (WorshipperData.Instance.Characters[index].Title == "Baal")
            {
              info.SkinCharacter = index;
              break;
            }
          }
          this.sacrificeFollower.Brain._directInfoAccess = info;
          this.sacrificeFollower.Brain.Info = new FollowerBrainInfo(info, this.sacrificeFollower.Brain);
          this.sacrificeFollower.Brain.Stats = new FollowerBrainStats(info, this.sacrificeFollower.Brain);
          DataManager.Instance.HasBaalSkin = true;
        }
        else if (this.necklace == InventoryItem.ITEM_TYPE.Necklace_Dark && !DataManager.Instance.HasAymSkin)
        {
          DataManager.Instance.Followers_Dead.Insert(0, this.sacrificeFollower.Brain._directInfoAccess);
          DataManager.Instance.Followers_Dead_IDs.Insert(0, this.sacrificeFollower.Brain._directInfoAccess.ID);
          FollowerInfo info = FollowerInfo.NewCharacter(FollowerLocation.Church);
          info.Traits.AddRange((IEnumerable<FollowerTrait.TraitType>) this.sacrificeFollower.Brain._directInfoAccess.Traits);
          if (info.Traits.Contains(FollowerTrait.TraitType.Mutated))
            info.Traits.Remove(FollowerTrait.TraitType.Mutated);
          info.TraitsSet = true;
          info.ID = 99995;
          info.Name = ScriptLocalization.NAMES.Guardian2;
          info.SkinName = "Boss Aym";
          info.Necklace = InventoryItem.ITEM_TYPE.NONE;
          info.SkinColour = 0;
          info.SkinVariation = 0;
          info.Clothing = FollowerClothingType.None;
          info.ClothingVariant = string.Empty;
          info.Outfit = FollowerOutfitType.None;
          for (int index = 0; index < WorshipperData.Instance.Characters.Count; ++index)
          {
            if (WorshipperData.Instance.Characters[index].Title == "Aym")
            {
              info.SkinCharacter = index;
              break;
            }
          }
          this.sacrificeFollower.Brain._directInfoAccess = info;
          this.sacrificeFollower.Brain.Info = new FollowerBrainInfo(info, this.sacrificeFollower.Brain);
          this.sacrificeFollower.Brain.Stats = new FollowerBrainStats(info, this.sacrificeFollower.Brain);
          DataManager.Instance.HasAymSkin = true;
        }
        DataManager.Instance.Followers.Add(this.sacrificeFollower.Brain._directInfoAccess);
        DataManager.Instance.Followers.Sort((Comparison<FollowerInfo>) ((a, b) => a.ID.CompareTo(b.ID)));
        FollowerBrain.AllBrains.Sort((Comparison<FollowerBrain>) ((a, b) =>
        {
          if (a.Info == null && b.Info == null)
            return 0;
          if (a.Info == null && b.Info != null)
            return -1;
          return a.Info != null && b.Info == null ? 1 : a.Info.ID.CompareTo(b.Info.ID);
        }));
        FollowerManager.RemoveFollower(id);
        this.sacrificeFollower.Spine.Skeleton.SetSkin(this.sacrificeFollower.Brain.Info.SkinName);
        foreach (WorshipperData.SlotAndColor slotAndColour in WorshipperData.Instance.GetColourData("Boss Aym").SlotAndColours[0].SlotAndColours)
        {
          Slot slot = this.sacrificeFollower.Spine.Skeleton.FindSlot(slotAndColour.Slot);
          if (slot != null)
            slot.SetColor(slotAndColour.color);
        }
        this.sacrificeFollower.Outfit.SetInfo(this.sacrificeFollower.Brain._directInfoAccess);
        this.sacrificeFollower.Outfit.SetOutfit(this.sacrificeFollower.Spine, false);
        FollowerBrain.SetFollowerCostume(this.sacrificeFollower.Spine.Skeleton, this.sacrificeFollower.Brain.Info.XPLevel, this.sacrificeFollower.Brain.Info.SkinName, this.sacrificeFollower.Brain.Info.SkinColour, this.sacrificeFollower.Brain.Info.Outfit, this.sacrificeFollower.Brain.Info.Hat, this.sacrificeFollower.Brain.Info.Clothing, this.sacrificeFollower.Brain.Info.Customisation, this.sacrificeFollower.Brain.Info.Special, this.sacrificeFollower.Brain.Info.Necklace, this.sacrificeFollower.Brain.Info.ClothingVariant, this.sacrificeFollower.Brain._directInfoAccess);
        Ritual.OnEnd += new Action<bool>(this.UpdateSpecialOutfit);
        break;
      case "Shake-small":
        CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
        GameManager.GetInstance().OnConversationNext(this.sacrificeFollower.CameraBone, 6f);
        Interaction_TempleAltar.Instance.PulseDisplacementObject(this.sacrificeFollower.CameraBone.transform.position);
        break;
      case "Shake-big":
        CameraManager.instance.ShakeCameraForDuration(0.6f, 0.7f, 0.6f);
        GameManager.GetInstance().OnConversationNext(this.sacrificeFollower.CameraBone, 8f);
        Interaction_TempleAltar.Instance.PulseDisplacementObject(this.sacrificeFollower.CameraBone.transform.position);
        BiomeConstants.Instance.ImpactFrameForDuration();
        break;
      case "CamOffset-Add":
        GameManager.GetInstance().CamFollowTarget.SetOffset(new Vector3(0.0f, 0.0f, 1f));
        BiomeConstants.Instance.DepthOfFieldTween(0.5f, 7f, 8f, 1f, 150f);
        BiomeConstants.Instance.chromaticAbberration.intensity.value = 1f;
        break;
      case "CamOffset-Remove":
        GameManager.GetInstance().CamFollowTarget.SetOffset(Vector3.zero);
        BiomeConstants.Instance.DepthOfFieldTween(0.15f, 8.7f, 26f, 1f, 200f);
        BiomeConstants.Instance.ChromaticAbberationTween(1f, 1f, BiomeConstants.Instance.ChromaticAberrationDefaultValue);
        break;
    }
  }

  public IEnumerator GiveSoulsRoutines(
    Transform sacrificeTransform,
    int SacrificialValue,
    int followerID)
  {
    RitualSacrifice ritualSacrifice = this;
    Interaction_TempleAltar.Instance.PortalEffect.AnimationState.SetAnimation(0, "pulse", true);
    float SoulsToGive = (float) SacrificialValue;
    float increment = 2f / SoulsToGive;
    while ((double) --SoulsToGive >= 0.0)
    {
      AudioManager.Instance.PlayOneShot("event:/followers/pop_in", sacrificeTransform.position);
      if (ritualSacrifice.sacrificeFollower.Brain.Info.SacrificialType == InventoryItem.ITEM_TYPE.SOUL)
        SoulCustomTarget.Create(PlayerFarming.Instance.CameraBone, sacrificeTransform.position, Color.white, (System.Action) (() => PlayerFarming.Instance.GetXP(1f)));
      else
        ResourceCustomTarget.Create(PlayerFarming.Instance.CameraBone, sacrificeTransform.position, ritualSacrifice.sacrificeFollower.Brain.Info.SacrificialType, new System.Action(ritualSacrifice.\u003CGiveSoulsRoutines\u003Eb__11_1));
      CameraManager.instance.ShakeCameraForDuration(0.3f, 0.4f, 0.2f);
      yield return (object) new WaitForSeconds(increment);
    }
  }

  public IEnumerator GiveBonesAndMeat(Transform sacrificeTransform)
  {
    int BonesAndMeat = UnityEngine.Random.Range(3, 5);
    for (int i = 0; i < BonesAndMeat; ++i)
    {
      AudioManager.Instance.PlayOneShot("event:/followers/pop_in", sacrificeTransform.position);
      ResourceCustomTarget.Create(PlayerFarming.Instance.CameraBone, sacrificeTransform.position, InventoryItem.ITEM_TYPE.BONE, (System.Action) (() => Inventory.AddItem(9, 1)));
      yield return (object) new WaitForSeconds(0.1f);
      AudioManager.Instance.PlayOneShot("event:/followers/pop_in", sacrificeTransform.position);
      ResourceCustomTarget.Create(PlayerFarming.Instance.CameraBone, sacrificeTransform.position, InventoryItem.ITEM_TYPE.FOLLOWER_MEAT, (System.Action) (() => Inventory.AddItem(62, 1)));
      yield return (object) new WaitForSeconds(0.1f);
    }
  }

  public IEnumerator UpgradePlayer(int PerCentGain)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    RitualSacrifice ritualSacrifice = this;
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
    SermonController objectOfType = UnityEngine.Object.FindObjectOfType<SermonController>();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) ritualSacrifice.StartCoroutine((IEnumerator) objectOfType.SacrificeLevelUp(PerCentGain, new System.Action(ritualSacrifice.StopCheering)));
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void StopCheering()
  {
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.CurrentTaskType == FollowerTaskType.AttendTeaching)
        (allBrain.CurrentTask as FollowerTask_AttendRitual).WorshipTentacle();
    }
  }

  public void UpdateSpecialOutfit(bool canceled)
  {
    Ritual.OnEnd -= new Action<bool>(this.UpdateSpecialOutfit);
    this.sacrificeFollower.Outfit.SetOutfit(this.sacrificeFollower.Spine, false);
  }

  [CompilerGenerated]
  public void \u003CGiveSoulsRoutines\u003Eb__11_1()
  {
    Inventory.AddItem((int) this.sacrificeFollower.Brain.Info.SacrificialType, 1);
  }
}
