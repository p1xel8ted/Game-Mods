// Decompiled with JetBrains decompiler
// Type: RitualWedding
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
  public Follower contestant1;
  public FollowerTask_ManualControl Task1;
  public int followerID;
  public bool isDivorce;
  public bool hasFancyRobes;
  public bool hasFancySuit;

  public override UpgradeSystem.Type RitualType
  {
    get => this.isDivorce ? UpgradeSystem.Type.Ritual_Divorce : UpgradeSystem.Type.Ritual_Wedding;
  }

  public override void Play()
  {
    base.Play();
    this.StartCoroutine(this.RitualRoutine());
  }

  public IEnumerator RitualRoutine()
  {
    RitualWedding ritualWedding = this;
    ritualWedding.isDivorce = false;
    AudioManager.Instance.PlayOneShot("event:/rituals/generic_start_ritual");
    Interaction_TempleAltar.Instance.SimpleSetCamera.Play();
    PlayerFarming.Instance.GoToAndStop(Interaction_TempleAltar.Instance.PortalEffect.transform.position + Vector3.left * 0.5f, GoToCallback: (System.Action) (() =>
    {
      Interaction_TempleAltar.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      PlayerFarming.Instance.simpleSpineAnimator.Animate("idle", 0, true);
    }));
    yield return (object) ritualWedding.StartCoroutine(ritualWedding.WaitFollowersFormCircle());
    yield return (object) new WaitForSeconds(1f);
    List<FollowerSelectEntry> followerSelectEntries = new List<FollowerSelectEntry>();
    foreach (FollowerBrain followerBrain in Ritual.GetFollowersAvailableToAttendSermon())
      followerSelectEntries.Add(new FollowerSelectEntry(followerBrain, FollowerManager.GetFollowerAvailabilityStatus(followerBrain, 0, true)));
    UIFollowerSelectMenuController followerSelectInstance = MonoSingleton<UIManager>.Instance.FollowerSelectMenuTemplate.Instantiate<UIFollowerSelectMenuController>();
    followerSelectInstance.VotingType = TwitchVoting.VotingType.RITUAL_MARRY;
    followerSelectInstance.Show(followerSelectEntries, followerSelectionType: ritualWedding.RitualType);
    UIFollowerSelectMenuController selectMenuController1 = followerSelectInstance;
    selectMenuController1.OnFollowerHighlighted = selectMenuController1.OnFollowerHighlighted + (Action<FollowerInfo>) (followerInfo =>
    {
      if (followerInfo.MarriedToLeader)
        followerSelectInstance.ShowCustomAcceptTerm("UI/Divorce");
      else
        followerSelectInstance.HideCustomAcceptTerm();
    });
    UIFollowerSelectMenuController selectMenuController2 = followerSelectInstance;
    selectMenuController2.OnFollowerSelected = selectMenuController2.OnFollowerSelected + (Action<FollowerInfo>) (followerInfo =>
    {
      AudioManager.Instance.SetMusicBaseID(SoundConstants.BaseID.bongos_singing);
      this.followerID = followerInfo.ID;
      this.contestant1 = FollowerManager.FindFollowerByID(followerInfo.ID);
      AudioManager.Instance.PlayOneShot("event:/rituals/wedding_select_follower", PlayerFarming.Instance.gameObject);
      this.Task1 = new FollowerTask_ManualControl();
      this.contestant1.Brain.HardSwapToTask((FollowerTask) this.Task1);
      if (!this.contestant1.Brain.Info.MarriedToLeader)
        GameManager.GetInstance().StartCoroutine(this.ContinueRitual());
      else
        GameManager.GetInstance().StartCoroutine(this.ContinueRitualDivorce());
    });
    UIFollowerSelectMenuController selectMenuController3 = followerSelectInstance;
    selectMenuController3.OnShownCompleted = selectMenuController3.OnShownCompleted + (System.Action) (() =>
    {
      foreach (FollowerInformationBox followerInfoBox in followerSelectInstance.FollowerInfoBoxes)
      {
        if (followerInfoBox.followBrain.Info.MarriedToLeader)
          followerInfoBox.ShowMarried(true);
      }
    });
    UIFollowerSelectMenuController selectMenuController4 = followerSelectInstance;
    selectMenuController4.OnCancel = selectMenuController4.OnCancel + (System.Action) (() =>
    {
      Interaction_TempleAltar.Instance.SimpleSetCamera.Reset();
      GameManager.GetInstance().StartCoroutine(this.EndRitual());
      this.CancelFollowers();
      AudioManager.Instance.StopLoop(this.loopedSound);
      this.CompleteRitual(true);
    });
    UIFollowerSelectMenuController selectMenuController5 = followerSelectInstance;
    selectMenuController5.OnHidden = selectMenuController5.OnHidden + (System.Action) (() => followerSelectInstance = (UIFollowerSelectMenuController) null);
  }

  public IEnumerator ContinueRitualDivorce()
  {
    RitualWedding ritualWedding = this;
    ritualWedding.isDivorce = true;
    Interaction_TempleAltar.Instance.SimpleSetCamera.Reset();
    Interaction_TempleAltar.Instance.CloseUpCamera.Play();
    PlayerFarming.Instance.simpleSpineAnimator.Animate("idle", 0, true);
    PlayerFarming.Instance.state.facingAngle = Utils.GetAngle(ritualWedding.transform.position, ritualWedding.contestant1.transform.position);
    yield return (object) ritualWedding.StartCoroutine(ritualWedding.SetUpCombatant1Routine());
    PlayerFarming.Instance.simpleSpineAnimator.Animate("bleat", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("reactions/react-happy", 0, false, 0.0f);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    PlayerFarming.Instance.state.facingAngle = Utils.GetAngle(ritualWedding.transform.position, ritualWedding.contestant1.transform.position);
    GameObject ring = ChurchFollowerManager.Instance.Ring.gameObject;
    ring.SetActive(true);
    yield return (object) new WaitForSeconds(2.36666656f);
    ring.SetActive(false);
    Vector3 position = Interaction_TempleAltar.Instance.PortalEffect.transform.position;
    CameraManager.instance.ShakeCameraForDuration(0.1f, 1f, 0.5f);
    MMVibrate.Haptic(MMVibrate.HapticTypes.SoftImpact);
    AudioManager.Instance.PlayOneShot("event:/building/building_bell_ring", PlayerFarming.Instance.transform.position);
    Interaction_TempleAltar.Instance.PulseDisplacementObject(Interaction_TempleAltar.Instance.PortalEffect.transform.position - Vector3.back);
    if (ritualWedding.contestant1.Brain.HasTrait(FollowerTrait.TraitType.MarriedUnhappily))
    {
      double num1 = (double) ritualWedding.contestant1.SetBodyAnimation("Reactions/react-happy1", false);
    }
    else if (ritualWedding.contestant1.Brain.HasTrait(FollowerTrait.TraitType.MarriedHappily) || ritualWedding.contestant1.Brain.HasTrait(FollowerTrait.TraitType.MarriedDevoted))
    {
      double num2 = (double) ritualWedding.contestant1.SetBodyAnimation("Reactions/react-cry", false);
    }
    else if (ritualWedding.contestant1.Brain.HasTrait(FollowerTrait.TraitType.MarriedJealous) || ritualWedding.contestant1.Brain.HasTrait(FollowerTrait.TraitType.MarriedMurderouslyJealous))
    {
      double num3 = (double) ritualWedding.contestant1.SetBodyAnimation("Reactions/react-non-believers", false);
    }
    else
    {
      double num4 = (double) ritualWedding.contestant1.SetBodyAnimation("Conversations/react-mean" + UnityEngine.Random.Range(1, 4).ToString(), false);
    }
    if ((double) UnityEngine.Random.value < 0.2)
    {
      ritualWedding.contestant1.Brain.AddTrait(FollowerTrait.TraitType.JiltedLover);
      ritualWedding.contestant1.Brain.MakeDissenter();
      Thought[] thoughtArray = new Thought[3]
      {
        Thought.JiltedLover_1,
        Thought.JiltedLover_2,
        Thought.JiltedLover_3
      };
      ritualWedding.contestant1.Brain.AddThought(thoughtArray[UnityEngine.Random.Range(0, thoughtArray.Length)]);
    }
    ritualWedding.contestant1.AddBodyAnimation("idle", true, 0.0f);
    yield return (object) new WaitForSeconds(4f);
    ritualWedding.contestant1.Brain.Info.MarriedToLeader = false;
    ritualWedding.contestant1.Brain.RemoveTrait(FollowerTrait.TraitType.MarriedDevoted, true);
    ritualWedding.contestant1.Brain.RemoveTrait(FollowerTrait.TraitType.MarriedHappily, true);
    ritualWedding.contestant1.Brain.RemoveTrait(FollowerTrait.TraitType.MarriedJealous, true);
    ritualWedding.contestant1.Brain.RemoveTrait(FollowerTrait.TraitType.MarriedMurderouslyJealous, true);
    ritualWedding.contestant1.Brain.RemoveTrait(FollowerTrait.TraitType.MarriedUnhappily, true);
    AudioManager.Instance.SetMusicBaseID(SoundConstants.BaseID.Temple);
    PlayerFarming.SetStateForAllPlayers();
    float num5 = 0.0f;
    foreach (FollowerBrain brain in Ritual.FollowerToAttendSermon)
    {
      float Delay = UnityEngine.Random.Range(0.1f, 0.5f);
      num5 += Delay;
      ritualWedding.StartCoroutine(ritualWedding.DelayFollowerReaction(brain, Delay));
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
    ritualWedding.CompleteRitual(targetFollowerID_1: ritualWedding.contestant1.Brain.Info.ID);
    yield return (object) new WaitForSeconds(1f);
    CultFaithManager.AddThought(Thought.Cult_DivorceRitual, ritualWedding.followerID);
  }

  public IEnumerator ContinueRitual()
  {
    RitualWedding ritualWedding = this;
    Interaction_TempleAltar.Instance.SimpleSetCamera.Reset();
    Interaction_TempleAltar.Instance.CloseUpCamera.Play();
    PlayerFarming.Instance.simpleSpineAnimator.Animate("build", 0, true);
    PlayerFarming.Instance.state.facingAngle = Utils.GetAngle(ritualWedding.transform.position, ritualWedding.contestant1.transform.position);
    yield return (object) ritualWedding.StartCoroutine(ritualWedding.SetUpCombatant1Routine());
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
    if (ritualWedding.contestant1.Brain.Info.HasTrait(FollowerTrait.TraitType.Zombie))
    {
      yield return (object) new WaitForSeconds(0.8333333f);
      PlayerFarming.Instance.simpleSpineAnimator.Animate("eat-react-bad", 0, false);
      PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("dance", 0, true, 0.0f);
      yield return (object) new WaitForSeconds(0.233333334f);
      AudioManager.Instance.PlayOneShot("event:/dialogue/followers/hold_back_vom", ritualWedding.gameObject);
      yield return (object) new WaitForSeconds(0.733333349f);
      AudioManager.Instance.PlayOneShot("event:/dialogue/followers/vom", ritualWedding.gameObject);
      yield return (object) new WaitForSeconds(1.0333333f);
    }
    else
      yield return (object) new WaitForSeconds(3f);
    if (ritualWedding.contestant1.Brain.HasThought(Thought.Dissenter))
    {
      double num2 = (double) ritualWedding.contestant1.SetBodyAnimation("tantrum", false);
      ritualWedding.contestant1.AddBodyAnimation("idle", true, 0.0f);
    }
    else
    {
      double num3 = (double) ritualWedding.contestant1.SetBodyAnimation("dance", true);
    }
    int targetFollowerID = ritualWedding.contestant1.Brain.Info.ID;
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      if (followerBrain.CurrentTaskType == FollowerTaskType.AttendTeaching)
        (followerBrain.CurrentTask as FollowerTask_AttendRitual).Dance();
    }
    yield return (object) new WaitForSeconds(4f);
    if (!TarotCards.IsUnlocked(TarotCards.Card.Lovers2))
    {
      Interaction_TarotCardUnlock objectOfType = UnityEngine.Object.FindObjectOfType<Interaction_TarotCardUnlock>();
      if ((UnityEngine.Object) objectOfType == (UnityEngine.Object) null || (bool) (UnityEngine.Object) objectOfType && objectOfType.CardOverride != TarotCards.Card.Lovers2)
      {
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.TRINKET_CARD_UNLOCKED, 1, ritualWedding.contestant1.transform.position).GetComponent<Interaction_TarotCardUnlock>().CardOverride = TarotCards.Card.Lovers2;
        yield return (object) new WaitForSeconds(1f);
      }
    }
    if (DataManager.Instance.UnlockedClothing.Contains(FollowerClothingType.Robes_Fancy))
      ritualWedding.hasFancyRobes = true;
    if (DataManager.Instance.UnlockedClothing.Contains(FollowerClothingType.Suit_Fancy))
      ritualWedding.hasFancySuit = true;
    ++DataManager.Instance.weddingsPerformed;
    if (DataManager.Instance.TailorEnabled && DataManager.Instance.weddingsPerformed >= 3)
    {
      if (!ritualWedding.hasFancyRobes && !FoundItemPickUp.IsOutfitPickUpActive(FollowerClothingType.Robes_Fancy))
      {
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.FOUND_ITEM_OUTFIT, 1, ritualWedding.contestant1.transform.position).GetComponent<FoundItemPickUp>().clothingType = FollowerClothingType.Robes_Fancy;
        yield return (object) new WaitForSeconds(0.5f);
      }
      if (!ritualWedding.hasFancySuit && !FoundItemPickUp.IsOutfitPickUpActive(FollowerClothingType.Suit_Fancy))
      {
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.FOUND_ITEM_OUTFIT, 1, ritualWedding.contestant1.transform.position).GetComponent<FoundItemPickUp>().clothingType = FollowerClothingType.Suit_Fancy;
        yield return (object) new WaitForSeconds(0.5f);
      }
    }
    List<int> JealousSpouses = new List<int>();
    FollowerBrain.AddMarriageThoughts();
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      if (followerBrain != ritualWedding.contestant1.Brain)
      {
        if (!followerBrain.Info.MarriedToLeader || followerBrain.HasTrait(FollowerTrait.TraitType.Polyamory))
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
    PlayerFarming.SetStateForAllPlayers();
    float num4 = 0.0f;
    foreach (FollowerBrain brain in Ritual.FollowerToAttendSermon)
    {
      float Delay = UnityEngine.Random.Range(0.1f, 0.5f);
      num4 += Delay;
      ritualWedding.StartCoroutine(ritualWedding.DelayFollowerReaction(brain, Delay));
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
    ritualWedding.CalculateMarriageTraits(ritualWedding.contestant1.Brain);
    ritualWedding.CompleteRitual(targetFollowerID_1: targetFollowerID);
    yield return (object) new WaitForSeconds(1f);
    CultFaithManager.AddThought(Thought.Cult_Wedding, ritualWedding.followerID);
    foreach (int FollowerID in JealousSpouses)
      CultFaithManager.AddThought(Thought.Cult_Wedding_JealousSpouse, FollowerID);
  }

  public void CalculateMarriageTraits(FollowerBrain followerBrain)
  {
    if (followerBrain.HasThought(Thought.Dissenter))
    {
      followerBrain.AddTrait(FollowerTrait.TraitType.MarriedUnhappily, true);
      this.AddDistressedThought(followerBrain);
    }
    else
    {
      if (followerBrain.HasTrait(FollowerTrait.TraitType.MarriedHappily) || followerBrain.HasTrait(FollowerTrait.TraitType.MarriedUnhappily) || followerBrain.HasTrait(FollowerTrait.TraitType.MarriedMurderouslyJealous) || followerBrain.HasTrait(FollowerTrait.TraitType.MarriedJealous) || (double) UnityEngine.Random.value >= 0.40000000596046448)
        return;
      float num = UnityEngine.Random.value;
      if ((double) num < 0.15000000596046448 && !followerBrain.HasTrait(FollowerTrait.TraitType.Polyamory))
      {
        followerBrain.AddTrait(FollowerTrait.TraitType.MarriedMurderouslyJealous, true);
        this.AddDistressedThought(followerBrain);
      }
      else if ((double) num < 0.30000001192092896)
      {
        followerBrain.AddTrait(FollowerTrait.TraitType.MarriedUnhappily, true);
        this.AddDistressedThought(followerBrain);
      }
      else if ((double) num < 0.800000011920929)
      {
        followerBrain.AddTrait(FollowerTrait.TraitType.MarriedHappily, true);
        this.AddInspiredThought(followerBrain);
      }
      else
      {
        if ((double) num >= 1.0 || followerBrain.HasTrait(FollowerTrait.TraitType.Polyamory))
          return;
        followerBrain.AddTrait(FollowerTrait.TraitType.MarriedJealous, true);
        this.AddDistressedThought(followerBrain);
      }
    }
  }

  public void AddInspiredThought(FollowerBrain followerBrain)
  {
    Thought randomThoughtFromSet = FollowerThoughts.GetRandomThoughtFromSet(Thought.Inspired_0, Thought.Inspired_1, Thought.Inspired_2, Thought.Inspired_3, Thought.Inspired_4);
    if (followerBrain.HasThought(randomThoughtFromSet))
      return;
    followerBrain.AddThought(randomThoughtFromSet);
  }

  public void AddDistressedThought(FollowerBrain followerBrain)
  {
    Thought randomThoughtFromSet = FollowerThoughts.GetRandomThoughtFromSet(Thought.Distressed_0, Thought.Distressed_1, Thought.Distressed_2, Thought.Distressed_3);
    if (followerBrain.HasThought(randomThoughtFromSet))
      return;
    followerBrain.AddThought(randomThoughtFromSet);
  }

  public IEnumerator SetUpCombatant1Routine()
  {
    yield return (object) null;
    ChurchFollowerManager.Instance.RemoveBrainFromAudience(this.contestant1.Brain);
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.CurrentTaskType == FollowerTaskType.AttendTeaching)
      {
        allBrain.CurrentTask.RecalculateDestination();
        allBrain.CurrentTask.Setup(FollowerManager.FindFollowerByID(allBrain.Info.ID));
      }
    }
    bool isAtDestination = false;
    this.Task1.GoToAndStop(this.contestant1, Interaction_TempleAltar.Instance.PortalEffect.transform.position + Vector3.right * 0.5f, (System.Action) (() =>
    {
      this.contestant1.FacePosition(Interaction_TempleAltar.Instance.PortalEffect.transform.position);
      double num = (double) this.contestant1.SetBodyAnimation("idle", true);
      isAtDestination = true;
    }));
    while (!isAtDestination)
      yield return (object) null;
  }

  public IEnumerator EndRitual()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    RitualWedding ritualWedding = this;
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
    AudioManager.Instance.StopLoop(ritualWedding.loopedSound);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(0.5f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }
}
