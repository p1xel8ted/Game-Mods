// Decompiled with JetBrains decompiler
// Type: RitualAscend
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI;
using Lamb.UI.FollowerSelect;
using src.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class RitualAscend : Ritual
{
  public Follower contestant1;
  public FollowerTask_ManualControl Task1;
  public bool Waiting = true;
  public List<PickUp> Pickups = new List<PickUp>();

  public override UpgradeSystem.Type RitualType => UpgradeSystem.Type.Ritual_Ascend;

  public override void Play()
  {
    base.Play();
    this.StartCoroutine((IEnumerator) this.RitualRoutine());
  }

  public IEnumerator RitualRoutine()
  {
    RitualAscend ritualAscend = this;
    AudioManager.Instance.PlayOneShot("event:/rituals/generic_start_ritual");
    Interaction_TempleAltar.Instance.SimpleSetCamera.Play();
    yield return (object) ritualAscend.StartCoroutine((IEnumerator) ritualAscend.WaitFollowersFormCircle());
    yield return (object) new WaitForSeconds(1f);
    UIFollowerSelectMenuController followerSelectInstance = MonoSingleton<UIManager>.Instance.FollowerSelectMenuTemplate.Instantiate<UIFollowerSelectMenuController>();
    followerSelectInstance.VotingType = TwitchVoting.VotingType.RITUAL_ASCEND;
    followerSelectInstance.Show(Ritual.GetFollowerSelectEntriesForSermon(), followerSelectionType: ritualAscend.RitualType);
    UIFollowerSelectMenuController selectMenuController1 = followerSelectInstance;
    selectMenuController1.OnFollowerSelected = selectMenuController1.OnFollowerSelected + (Action<FollowerInfo>) (followerInfo =>
    {
      this.contestant1 = FollowerManager.FindFollowerByID(followerInfo.ID);
      AudioManager.Instance.PlayOneShot("event:/ritual_sacrifice/select_follower", PlayerFarming.Instance.gameObject);
      this.loopedSound = AudioManager.Instance.CreateLoop("event:/sermon/preach_loop", PlayerFarming.Instance.gameObject, true, false);
      this.Task1 = new FollowerTask_ManualControl();
      this.contestant1.Brain.HardSwapToTask((FollowerTask) this.Task1);
      GameManager.GetInstance().StartCoroutine((IEnumerator) this.ContinueRitual());
      this.StartCoroutine((IEnumerator) this.SetUpCombatant1Routine());
    });
    UIFollowerSelectMenuController selectMenuController2 = followerSelectInstance;
    selectMenuController2.OnShow = selectMenuController2.OnShow + (System.Action) (() =>
    {
      foreach (FollowerInformationBox followerInfoBox in followerSelectInstance.FollowerInfoBoxes)
        followerInfoBox.ShowFaithGain(this.GetAdorationRewardAmount(followerInfoBox.followBrain), followerInfoBox.followBrain.Stats.MAX_ADORATION, false);
    });
    UIFollowerSelectMenuController selectMenuController3 = followerSelectInstance;
    selectMenuController3.OnShownCompleted = selectMenuController3.OnShownCompleted + (System.Action) (() =>
    {
      foreach (FollowerInformationBox followerInfoBox in followerSelectInstance.FollowerInfoBoxes)
        followerInfoBox.ShowFaithGain(this.GetAdorationRewardAmount(followerInfoBox.followBrain), followerInfoBox.followBrain.Stats.MAX_ADORATION, false);
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

  public IEnumerator ContinueRitual()
  {
    RitualAscend ritualAscend = this;
    bool desensitisedToDeath = DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.DesensitisedToDeath);
    bool sacrificeEnthusiast = DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.SacrificeEnthusiast);
    PlayerFarming.Instance.simpleSpineAnimator.Animate("build", 0, true);
    PlayerFarming.Instance.state.facingAngle = Utils.GetAngle(ritualAscend.transform.position, ritualAscend.contestant1.transform.position);
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
    ChurchFollowerManager.Instance.Goop.gameObject.SetActive(true);
    ChurchFollowerManager.Instance.Goop.Play("Show");
    if (desensitisedToDeath && !sacrificeEnthusiast)
      ChurchFollowerManager.Instance.Goop.GetComponentInChildren<MeshRenderer>().material.SetColor("_TintCOlor", Color.white);
    else
      ChurchFollowerManager.Instance.Goop.GetComponentInChildren<MeshRenderer>().material.SetColor("_TintCOlor", Color.red);
    yield return (object) new WaitForSeconds(0.5f);
    if (desensitisedToDeath && !sacrificeEnthusiast)
      AudioManager.Instance.PlayOneShot("event:/Stings/ritual_ascension_pg", PlayerFarming.Instance.gameObject);
    else
      AudioManager.Instance.PlayOneShot("event:/Stings/ritual_ascension", PlayerFarming.Instance.gameObject);
    double num = (double) ritualAscend.contestant1.SetBodyAnimation("ascend", false);
    Interaction_TempleAltar.Instance.CloseUpCamera.gameObject.SetActive(false);
    yield return (object) new WaitForSeconds(5.66666651f);
    int i;
    if (desensitisedToDeath)
    {
      Vector3 position = ritualAscend.contestant1.transform.position;
      AudioManager.Instance.PlayOneShot("event:/dialogue/followers/loved", position);
      AudioManager.Instance.PlayOneShot("event:/player/Curses/enemy_charmed", position);
      ChurchFollowerManager.Instance.Sparkles.gameObject.SetActive(true);
      ChurchFollowerManager.Instance.Sparkles.Play();
      HUD_Manager.Instance.XPBarTransitions.gameObject.SetActive(true);
      HUD_Manager.Instance.XPBarTransitions.MoveBackInFunction();
      yield return (object) new WaitForSeconds(1f);
      for (i = 0; i < 10; ++i)
      {
        SoulCustomTarget.Create(PlayerFarming.Instance.gameObject, ritualAscend.contestant1.transform.position + Vector3.back * 5f, Color.white, (System.Action) (() => PlayerFarming.Instance.GetXP(1f)));
        yield return (object) new WaitForSeconds(0.1f);
      }
      yield return (object) new WaitForSeconds(1.5f);
      HUD_Manager.Instance.XPBarTransitions.MoveBackOutFunction();
    }
    if ((sacrificeEnthusiast || !desensitisedToDeath && !sacrificeEnthusiast) && !ritualAscend.contestant1.Brain.Info.IsSnowman)
    {
      Vector3 position = ritualAscend.contestant1.transform.position;
      AudioManager.Instance.PlayOneShot("event:/dialogue/followers/scared_short", position);
      AudioManager.Instance.PlayOneShot("event:/player/harvest_meat", position);
      CameraManager.instance.ShakeCameraForDuration(0.0f, 1f, 2f);
      MMVibrate.RumbleContinuous(0.0f, 1f);
      InventoryItem.ITEM_TYPE meat = InventoryItem.ITEM_TYPE.FOLLOWER_MEAT;
      InventoryItem.ITEM_TYPE bone = InventoryItem.ITEM_TYPE.BONE;
      if (ritualAscend.contestant1.Brain.Info.SkinName == "Mushroom" || ritualAscend.contestant1.Brain.Info.ID == 99996)
      {
        meat = InventoryItem.ITEM_TYPE.MUSHROOM_SMALL;
        bone = InventoryItem.ITEM_TYPE.MUSHROOM_SMALL;
      }
      if (ritualAscend.contestant1.Brain.HasTrait(FollowerTrait.TraitType.Mutated))
      {
        meat = InventoryItem.ITEM_TYPE.MAGMA_STONE;
        bone = InventoryItem.ITEM_TYPE.MAGMA_STONE;
      }
      yield return (object) new WaitForSeconds(1f);
      for (i = 0; i < 3; ++i)
      {
        PickUp component1 = InventoryItem.Spawn(meat, 1, ritualAscend.contestant1.transform.position + Vector3.back * 5f).GetComponent<PickUp>();
        ritualAscend.Pickups.Add(component1);
        component1.enabled = false;
        component1.child.transform.localScale = Vector3.one;
        Vector3 vector3_1 = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
        component1.gameObject.transform.DOMove(ChurchFollowerManager.Instance.RitualCenterPosition.transform.position + vector3_1, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBounce).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
        yield return (object) new WaitForSeconds(0.25f);
        PickUp component2 = InventoryItem.Spawn(bone, 1, ritualAscend.contestant1.transform.position + Vector3.back * 5f).GetComponent<PickUp>();
        ritualAscend.Pickups.Add(component2);
        Vector3 vector3_2 = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
        component2.enabled = false;
        component2.child.transform.localScale = Vector3.one;
        component2.gameObject.transform.DOMove(ChurchFollowerManager.Instance.RitualCenterPosition.transform.position + vector3_2, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBounce).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
        yield return (object) new WaitForSeconds(0.25f);
      }
      AudioManager.Instance.PlayOneShot("event:/player/harvest_meat_done", position);
      PickUp component3 = InventoryItem.Spawn(meat, 1, ritualAscend.contestant1.transform.position + Vector3.back * 5f).GetComponent<PickUp>();
      ritualAscend.Pickups.Add(component3);
      component3.enabled = false;
      component3.child.transform.localScale = Vector3.one;
      Vector3 vector3_3 = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
      component3.gameObject.transform.DOMove(ChurchFollowerManager.Instance.RitualCenterPosition.transform.position + vector3_3, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBounce).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      yield return (object) new WaitForSeconds(0.2f);
      PickUp component4 = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BONE, 1, ritualAscend.contestant1.transform.position + Vector3.back * 5f).GetComponent<PickUp>();
      ritualAscend.Pickups.Add(component4);
      component4.child.transform.localScale = Vector3.one;
      Vector3 vector3_4 = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
      component4.enabled = false;
      component4.gameObject.transform.DOMove(ChurchFollowerManager.Instance.RitualCenterPosition.transform.position + vector3_4, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBounce).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      position = new Vector3();
    }
    PlayerFarming.Instance.simpleSpineAnimator.Animate("rituals/ritual-stop", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    MMVibrate.StopRumble();
    ChurchFollowerManager.Instance.GodRays.GetComponent<ParticleSystem>().Stop();
    ChurchFollowerManager.Instance.Goop.Play("Hide");
    Interaction_TempleAltar.Instance.SimpleSetCamera.Reset();
    int id = ritualAscend.contestant1.Brain.Info.ID;
    Ritual.FollowerToAttendSermon.Remove(ritualAscend.contestant1.Brain);
    ritualAscend.contestant1.Die(NotificationCentre.NotificationType.Ascended, false, force: true);
    foreach (FollowerBrain brain in Ritual.FollowerToAttendSermon)
    {
      float Delay = UnityEngine.Random.Range(0.1f, 0.5f);
      ritualAscend.StartCoroutine((IEnumerator) ritualAscend.DelayFollowerReaction(brain, Delay));
      Follower followerById = FollowerManager.FindFollowerByID(brain.Info.ID);
      if ((bool) (UnityEngine.Object) followerById)
        followerById.Spine.randomOffset = false;
    }
    AudioManager.Instance.StopLoop(ritualAscend.loopedSound);
    FollowerBrain.AdorationActions adorationReward = ritualAscend.GetAdorationReward(ritualAscend.contestant1.Brain);
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      if (followerBrain != ritualAscend.contestant1.Brain)
        followerBrain.AddAdoration(adorationReward, (System.Action) null);
    }
    yield return (object) new WaitForSeconds(1.5f);
    ChurchFollowerManager.Instance.Goop.gameObject.SetActive(false);
    ChurchFollowerManager.Instance.GodRays.SetActive(false);
    ChurchFollowerManager.Instance.Sparkles.Stop();
    foreach (Behaviour pickup in ritualAscend.Pickups)
      pickup.enabled = true;
    ritualAscend.Pickups.Clear();
    JudgementMeter.ShowModify(1);
    yield return (object) new WaitForSeconds(1.5f);
    ChurchFollowerManager.Instance.Sparkles.gameObject.SetActive(false);
    ritualAscend.CompleteRitual(targetFollowerID_1: id);
    yield return (object) new WaitForSeconds(1f);
    CultFaithManager.AddThought(Thought.Cult_Ascend, id);
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
    RitualAscend ritualAscend = this;
    yield return (object) null;
    ChurchFollowerManager.Instance.RemoveBrainFromAudience(ritualAscend.contestant1.Brain);
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.CurrentTaskType == FollowerTaskType.AttendTeaching)
      {
        allBrain.CurrentTask.RecalculateDestination();
        allBrain.CurrentTask.Setup(FollowerManager.FindFollowerByID(allBrain.Info.ID));
      }
    }
    ritualAscend.contestant1.SetOutfit(FollowerOutfitType.HorseTown, false);
    ritualAscend.Task1.GoToAndStop(ritualAscend.contestant1, ChurchFollowerManager.Instance.RitualCenterPosition.position, new System.Action(ritualAscend.\u003CSetUpCombatant1Routine\u003Eb__11_0));
  }

  public IEnumerator EndRitualIE()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    RitualAscend ritualAscend = this;
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
    AudioManager.Instance.StopLoop(ritualAscend.loopedSound);
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
  public void \u003CSetUpCombatant1Routine\u003Eb__11_0()
  {
    this.contestant1.FacePosition(Interaction_TempleAltar.Instance.PortalEffect.transform.position);
    double num = (double) this.contestant1.SetBodyAnimation("idle", true);
    this.Waiting = false;
  }
}
