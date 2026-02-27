// Decompiled with JetBrains decompiler
// Type: RitualSacrifice
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI;
using Lamb.UI.FollowerSelect;
using Spine;
using src.Extensions;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class RitualSacrifice : Ritual
{
  private Follower sacrificeFollower;
  public Light RitualLight;

  protected override UpgradeSystem.Type RitualType => UpgradeSystem.Type.Ritual_Sacrifice;

  public override void Play()
  {
    base.Play();
    this.StartCoroutine((IEnumerator) this.SacrificeFollowerRoutine());
  }

  private IEnumerator SacrificeFollowerRoutine()
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
    followerSelectInstance = GameManager.GetInstance().UpgradePlayerConfiguration.HasUnlockAvailable() ? MonoSingleton<UIManager>.Instance.SacrificeFollowerMenuTemplate.Instantiate<UIFollowerSelectMenuController>() : MonoSingleton<UIManager>.Instance.FollowerSelectMenuTemplate.Instantiate<UIFollowerSelectMenuController>();
    followerSelectInstance.Show(Ritual.GetFollowersAvailableToAttendSermon(), followerSelectionType: ritualSacrifice.RitualType);
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
        if (!GameManager.GetInstance().UpgradePlayerConfiguration.HasUnlockAvailable())
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
    selectMenuController3.OnCancel = selectMenuController3.OnCancel + (System.Action) (() =>
    {
      GameManager.GetInstance().StartCoroutine((IEnumerator) this.EndRitual());
      Cancelled = true;
      this.CompleteRitual(true);
      this.CancelFollowers();
    });
    UIFollowerSelectMenuController selectMenuController4 = followerSelectInstance;
    selectMenuController4.OnHidden = selectMenuController4.OnHidden + (System.Action) (() => followerSelectInstance = (UIFollowerSelectMenuController) null);
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

  public static int GetDevotionGain(int XPLevel) => Mathf.Clamp(40 + (XPLevel - 1) * 20, 30, 100);

  private IEnumerator DoSacrificeRoutine()
  {
    RitualSacrifice ritualSacrifice = this;
    if ((UnityEngine.Object) ritualSacrifice.RitualLight != (UnityEngine.Object) null)
    {
      ritualSacrifice.RitualLight.gameObject.SetActive(true);
      ritualSacrifice.RitualLight.color = Color.black;
      ritualSacrifice.RitualLight.DOColor(Color.red, 1f);
      ritualSacrifice.RitualLight.DOIntensity(1.5f, 1f);
    }
    GameManager.GetInstance().OnConversationNext(ritualSacrifice.sacrificeFollower.gameObject, 4f);
    ChurchFollowerManager.Instance.PlayOverlay(ChurchFollowerManager.OverlayType.Sacrifice, "1");
    ritualSacrifice.sacrificeFollower.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(ritualSacrifice.HandleSacrificeAnimationStateEvent);
    ritualSacrifice.sacrificeFollower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    Ritual.FollowerToAttendSermon.Remove(ritualSacrifice.sacrificeFollower.Brain);
    if (DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.SacrificeEnthusiast) && ritualSacrifice.sacrificeFollower.Brain.Info.CursedState != Thought.Dissenter)
    {
      double num1 = (double) ritualSacrifice.sacrificeFollower.SetBodyAnimation("sacrifice-tentacles", false);
    }
    else
    {
      double num2 = (double) ritualSacrifice.sacrificeFollower.SetBodyAnimation("sacrifice-tentacles-scared", false);
    }
    int followerID = ritualSacrifice.sacrificeFollower.Brain.Info.ID;
    yield return (object) new WaitForSeconds(0.5f);
    ritualSacrifice.PlaySacrificePortalEffect();
    Interaction_TempleAltar.Instance.PulseDisplacementObject(Interaction_TempleAltar.Instance.PortalEffect.transform.position);
    BiomeConstants.Instance.ChromaticAbberationTween(1f, BiomeConstants.Instance.ChromaticAberrationDefaultValue, 7f);
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.CurrentTaskType == FollowerTaskType.AttendTeaching)
        (allBrain.CurrentTask as FollowerTask_AttendRitual).WorshipTentacle();
    }
    yield return (object) new WaitForSeconds(1.5f);
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.CurrentTaskType == FollowerTaskType.AttendTeaching)
        (allBrain.CurrentTask as FollowerTask_AttendRitual).Cheer();
    }
    ChurchFollowerManager.Instance.StartRitualOverlay();
    AudioManager.Instance.PlayOneShot("event:/ritual_sacrifice/ritual_end", PlayerFarming.Instance.gameObject);
    yield return (object) new WaitForSeconds(0.7f);
    Interaction_TempleAltar.Instance.PulseDisplacementObject(ritualSacrifice.sacrificeFollower.CameraBone.transform.position);
    yield return (object) new WaitForSeconds(3.16666651f);
    Interaction_TempleAltar.Instance.PulseDisplacementObject(Interaction_TempleAltar.Instance.PortalEffect.transform.position);
    yield return (object) new WaitForSeconds(0.3f);
    GameManager.GetInstance().OnConversationNext(Interaction_TempleAltar.Instance.RitualCameraPosition, 8f);
    yield return (object) new WaitForSeconds(0.5f);
    if (!GameManager.GetInstance().UpgradePlayerConfiguration.HasUnlockAvailable())
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
    yield return (object) ritualSacrifice.StartCoroutine((IEnumerator) ritualSacrifice.GiveBonesAndMeat(Interaction_TempleAltar.Instance.PortalEffect.transform));
    yield return (object) new WaitForSeconds(0.5f);
    ritualSacrifice.StopSacrificePortalEffect();
    ritualSacrifice.sacrificeFollower.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(ritualSacrifice.HandleSacrificeAnimationStateEvent);
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      if (ritualSacrifice.sacrificeFollower.Brain.Info.CursedState == Thought.OldAge)
        followerBrain.AddThought(Thought.SacrificedOldFollower);
    }
    FollowerManager.FollowerDie(ritualSacrifice.sacrificeFollower.Brain.Info.ID, NotificationCentre.NotificationType.SacrificeFollower);
    UnityEngine.Object.Destroy((UnityEngine.Object) ritualSacrifice.sacrificeFollower.gameObject);
    PlayerFarming.Instance.simpleSpineAnimator.Animate("rituals/ritual-stop", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    float num3 = 0.5f;
    foreach (FollowerBrain brain in Ritual.FollowerToAttendSermon)
    {
      float Delay = UnityEngine.Random.Range(0.1f, 0.5f);
      num3 += Delay;
      ritualSacrifice.StartCoroutine((IEnumerator) ritualSacrifice.DelayFollowerReaction(brain, Delay));
    }
    yield return (object) new WaitForSeconds(1.5f);
    JudgementMeter.ShowModify(DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.SacrificeEnthusiast) ? 1 : -1);
    yield return (object) new WaitForSeconds(2f);
    ChurchFollowerManager.Instance.EndRitualOverlay();
    Interaction_TempleAltar.Instance.SimpleSetCamera.Reset();
    bool completedQuest = false;
    foreach (ObjectivesData objective in DataManager.Instance.Objectives)
    {
      if (objective.Type == Objectives.TYPES.PERFORM_RITUAL && ((Objectives_PerformRitual) objective).Ritual == UpgradeSystem.Type.Ritual_Sacrifice && ((Objectives_PerformRitual) objective).TargetFollowerID_1 == followerID)
      {
        completedQuest = true;
        break;
      }
      if (objective.Type == Objectives.TYPES.CUSTOM && ((Objectives_Custom) objective).CustomQuestType == Objectives.CustomQuestTypes.KillFollower && ((Objectives_Custom) objective).TargetFollowerID == followerID)
      {
        completedQuest = true;
        break;
      }
    }
    ++DataManager.Instance.STATS_Sacrifices;
    ritualSacrifice.CompleteRitual(targetFollowerID_1: followerID);
    yield return (object) new WaitForSeconds(1f);
    if (DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.SacrificeEnthusiast))
      CultFaithManager.AddThought(Thought.Cult_Sacrifice_Trait, followerID);
    else
      CultFaithManager.AddThought(Thought.Cult_Sacrifice, followerID, completedQuest ? 0.0f : 1f);
  }

  private IEnumerator EndRitual()
  {
    Interaction_TempleAltar.Instance.SimpleSetCamera.Reset();
    PlayerFarming.Instance.simpleSpineAnimator.Animate("rituals/ritual-stop", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(0.6666667f);
  }

  private void HandleSacrificeAnimationStateEvent(TrackEntry trackEntry, Spine.Event e)
  {
    switch (e.Data.Name)
    {
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

  private IEnumerator GiveSoulsRoutines(
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
      {
        SoulCustomTarget.Create(PlayerFarming.Instance.CameraBone, sacrificeTransform.position, Color.white, (System.Action) (() => PlayerFarming.Instance.GetXP(1f)));
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        ResourceCustomTarget.Create(PlayerFarming.Instance.CameraBone, sacrificeTransform.position, ritualSacrifice.sacrificeFollower.Brain.Info.SacrificialType, new System.Action(ritualSacrifice.\u003CGiveSoulsRoutines\u003Eb__10_1));
      }
      CameraManager.instance.ShakeCameraForDuration(0.3f, 0.4f, 0.2f);
      yield return (object) new WaitForSeconds(increment);
    }
  }

  private IEnumerator GiveBonesAndMeat(Transform sacrificeTransform)
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

  private IEnumerator UpgradePlayer(int PerCentGain)
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

  private void StopCheering()
  {
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.CurrentTaskType == FollowerTaskType.AttendTeaching)
        (allBrain.CurrentTask as FollowerTask_AttendRitual).WorshipTentacle();
    }
  }
}
