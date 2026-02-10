// Decompiled with JetBrains decompiler
// Type: RitualCannibal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using Lamb.UI;
using Lamb.UI.FollowerSelect;
using src.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RitualCannibal : Ritual
{
  public Follower contestant1;
  public FollowerTask_ManualControl Task1;
  public bool Waiting = true;
  public EventInstance eatingLoop;
  public List<EventInstance> loops = new List<EventInstance>();
  public List<PickUp> Pickups = new List<PickUp>();

  public override UpgradeSystem.Type RitualType => UpgradeSystem.Type.Ritual_Cannibal;

  public override void Play()
  {
    base.Play();
    this.StartCoroutine((IEnumerator) this.RitualRoutine());
  }

  public IEnumerator RitualRoutine()
  {
    RitualCannibal ritualCannibal = this;
    AudioManager.Instance.PlayOneShot("event:/rituals/generic_start_ritual");
    Interaction_TempleAltar.Instance.SimpleSetCamera.Play();
    yield return (object) ritualCannibal.StartCoroutine((IEnumerator) ritualCannibal.WaitFollowersFormCircle());
    yield return (object) new WaitForSeconds(1f);
    UIFollowerSelectMenuController followerSelectInstance = MonoSingleton<UIManager>.Instance.FollowerSelectMenuTemplate.Instantiate<UIFollowerSelectMenuController>();
    followerSelectInstance.VotingType = TwitchVoting.VotingType.RITUAL_CANNIBAL;
    followerSelectInstance.Show(Ritual.GetFollowerSelectEntriesForSermon(), followerSelectionType: ritualCannibal.RitualType);
    UIFollowerSelectMenuController selectMenuController1 = followerSelectInstance;
    selectMenuController1.OnFollowerSelected = selectMenuController1.OnFollowerSelected + (Action<FollowerInfo>) (followerInfo =>
    {
      this.contestant1 = FollowerManager.FindFollowerByID(followerInfo.ID);
      AudioManager.Instance.PlayOneShot("event:/ritual_sacrifice/select_follower", PlayerFarming.Instance.gameObject);
      this.Task1 = new FollowerTask_ManualControl();
      this.contestant1.Brain.HardSwapToTask((FollowerTask) this.Task1);
      GameManager.GetInstance().StartCoroutine((IEnumerator) this.ContinueRitual());
      this.StartCoroutine((IEnumerator) this.SetUpCombatant1Routine());
    });
    UIFollowerSelectMenuController selectMenuController2 = followerSelectInstance;
    selectMenuController2.OnShow = selectMenuController2.OnShow + (System.Action) (() =>
    {
      foreach (FollowerInformationBox followerInfoBox in followerSelectInstance.FollowerInfoBoxes)
        followerInfoBox.ShowPleasure(0);
    });
    UIFollowerSelectMenuController selectMenuController3 = followerSelectInstance;
    selectMenuController3.OnShownCompleted = selectMenuController3.OnShownCompleted + (System.Action) (() =>
    {
      foreach (FollowerInformationBox followerInfoBox in followerSelectInstance.FollowerInfoBoxes)
        followerInfoBox.ShowPleasure(0);
    });
    UIFollowerSelectMenuController selectMenuController4 = followerSelectInstance;
    selectMenuController4.OnCancel = selectMenuController4.OnCancel + (System.Action) (() =>
    {
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
    RitualCannibal ritualCannibal = this;
    AudioManager.Instance.PlayOneShot("event:/Stings/cannibal_ritual");
    PlayerFarming.Instance.simpleSpineAnimator.Animate("build", 0, true);
    PlayerFarming.Instance.state.facingAngle = Utils.GetAngle(ritualCannibal.transform.position, ritualCannibal.contestant1.transform.position);
    ChurchFollowerManager.Instance.StartRitualOverlay();
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
    yield return (object) new WaitForSeconds(1f);
    foreach (FollowerBrain followerBrain in Ritual.GetFollowersAvailableToAttendSermon())
    {
      if (followerBrain != ritualCannibal.contestant1.Brain)
      {
        Follower followerById = FollowerManager.FindFollowerByID(followerBrain.Info.ID);
        if ((UnityEngine.Object) followerById != (UnityEngine.Object) null)
          ritualCannibal.StartCoroutine((IEnumerator) ritualCannibal.FollowerMoveRoutine(followerById));
      }
    }
    yield return (object) new WaitForSeconds(2.5f);
    ritualCannibal.eatingLoop = AudioManager.Instance.CreateLoop("event:/material/body_cannibalised_loop", ritualCannibal.contestant1.gameObject, true);
    if (ritualCannibal.contestant1.Brain.HasTrait(FollowerTrait.TraitType.Mutated))
      ritualCannibal.StartCoroutine((IEnumerator) ritualCannibal.SpawnItem(InventoryItem.ITEM_TYPE.MAGMA_STONE, 10, new Vector2(0.15f, 0.25f), ritualCannibal.contestant1.transform.position));
    else if (!ritualCannibal.contestant1.Brain.Info.IsSnowman)
      ritualCannibal.StartCoroutine((IEnumerator) ritualCannibal.SpawnItem(InventoryItem.ITEM_TYPE.FOLLOWER_MEAT, 10, new Vector2(0.15f, 0.25f), ritualCannibal.contestant1.transform.position));
    CameraManager.instance.ShakeCameraForDuration(0.5f, 0.7f, 2f);
    yield return (object) new WaitForSeconds(2f);
    AudioManager.Instance.StopLoop(ritualCannibal.eatingLoop);
    GameObject godTear = UnityEngine.Object.Instantiate<GameObject>(ritualCannibal.contestant1.rewardPrefab, ritualCannibal.contestant1.Spine.transform.position + new Vector3(0.0f, -0.1f, -1f), Quaternion.identity, ritualCannibal.transform.parent);
    godTear.transform.localScale = Vector3.zero;
    godTear.transform.DOScale(Vector3.one, 1.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack);
    yield return (object) new WaitForSeconds(1.5f);
    PlayerSimpleInventory simpleInventory = PlayerFarming.Instance.simpleInventory;
    Vector3 endValue = new Vector3(simpleInventory.ItemImage.transform.position.x, simpleInventory.ItemImage.transform.position.y - 0.5f, -1f) - Vector3.forward;
    Interaction_TempleAltar.Instance.SimpleSetCamera.Reset();
    AudioManager.Instance.PlayOneShot("event:/Stings/sins_snake_sting", PlayerFarming.Instance.gameObject);
    GameManager.GetInstance().OnConversationNext(godTear, 8f);
    godTear.transform.DOMove(endValue, 2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => godTear.transform.DOScale(0.0f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => UnityEngine.Object.Destroy((UnityEngine.Object) godTear.gameObject)))));
    double num = (double) ritualCannibal.contestant1.SetBodyAnimation("Rituals/cannibal-end", false);
    yield return (object) new WaitForSeconds(1f);
    bool wasZombie = ritualCannibal.contestant1.Brain.HasTrait(FollowerTrait.TraitType.Zombie);
    FollowerManager.FollowerDie(ritualCannibal.contestant1.Brain.Info.ID, NotificationCentre.NotificationType.MurderedByFollower);
    UnityEngine.Object.Destroy((UnityEngine.Object) ritualCannibal.contestant1.gameObject);
    Ritual.FollowerToAttendSermon.Remove(ritualCannibal.contestant1.Brain);
    FaithBarFake.Play(UpgradeSystem.GetRitualFaithChange(ritualCannibal.RitualType));
    yield return (object) new WaitForSeconds(0.75f);
    PlayerFarming.Instance.simpleSpineAnimator.Animate("rituals/ritual-stop", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    MMVibrate.StopRumble();
    for (int index = Ritual.FollowerToAttendSermon.Count - 1; index >= 0; --index)
    {
      float Delay = UnityEngine.Random.Range(0.1f, 0.5f);
      float animTime = 1.5f;
      string anim = "Reactions/react-enlightened1";
      if (wasZombie && (double) UnityEngine.Random.value < 0.5)
      {
        anim = "Sick/chunder";
        animTime = 3f;
      }
      ritualCannibal.StartCoroutine((IEnumerator) ritualCannibal.DelayFollowerReaction(Ritual.FollowerToAttendSermon[index], anim, Delay, false, animTime));
      Follower followerById = FollowerManager.FindFollowerByID(Ritual.FollowerToAttendSermon[index].Info.ID);
      if ((bool) (UnityEngine.Object) followerById)
        followerById.Spine.randomOffset = false;
    }
    GameManager.GetInstance().OnConversationNext(ChurchFollowerManager.Instance.RitualCenterPosition.gameObject);
    yield return (object) new WaitForSeconds(3.5f);
    ChurchFollowerManager.Instance.GodRays.GetComponent<ParticleSystem>().Stop();
    ChurchFollowerManager.Instance.Goop.Play("Hide");
    Interaction_TempleAltar.Instance.SimpleSetCamera.Reset();
    JudgementMeter.ShowModify(1);
    ChurchFollowerManager.Instance.Sparkles.gameObject.SetActive(false);
    int id = ritualCannibal.contestant1.Brain.Info.ID;
    ritualCannibal.CompleteRitual(targetFollowerID_1: ritualCannibal.contestant1.Brain.Info.ID, PlayFakeBar: false);
    yield return (object) new WaitForSeconds(1f);
    CultFaithManager.AddThought(Thought.Cult_CannibalRitual, id);
    Inventory.AddItem(154, 1);
  }

  public IEnumerator SetUpCombatant1Routine()
  {
    bool waiting = true;
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
    this.Task1.GoToAndStop(this.contestant1, ChurchFollowerManager.Instance.RitualCenterPosition.position, (System.Action) (() =>
    {
      this.contestant1.FacePosition(Interaction_TempleAltar.Instance.PortalEffect.transform.position);
      waiting = false;
    }));
    while (waiting)
      yield return (object) null;
    this.contestant1.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForSeconds(1.25f);
    double num = (double) this.contestant1.SetBodyAnimation("Rituals/cannibal-start", false);
    this.contestant1.AddBodyAnimation("Rituals/cannibal-eaten", true, 0.0f);
  }

  public IEnumerator SpawnItem(
    InventoryItem.ITEM_TYPE item,
    int amount,
    Vector2 timeBetween,
    Vector3 pos)
  {
    RitualCannibal ritualCannibal = this;
    yield return (object) new WaitForSeconds(UnityEngine.Random.Range(timeBetween.x, timeBetween.y));
    for (int i = 0; i < amount; ++i)
    {
      PickUp component = InventoryItem.Spawn(item, 1, pos - Vector3.forward).GetComponent<PickUp>();
      component.SetInitialSpeedAndDiraction(4f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
      component.MagnetToPlayer = false;
      ritualCannibal.Pickups.Add(component);
      ritualCannibal.StartCoroutine((IEnumerator) ritualCannibal.DelayedCollect(component));
      yield return (object) new WaitForSeconds(UnityEngine.Random.Range(timeBetween.x, timeBetween.y));
    }
  }

  public IEnumerator DelayedCollect(PickUp g)
  {
    yield return (object) new WaitForSeconds(4f);
    g.PickMeUp();
    g.MagnetToPlayer = true;
  }

  public IEnumerator FollowerMoveRoutine(Follower follower)
  {
    bool waiting = true;
    follower.HoodOff(onComplete: (System.Action) (() => waiting = false));
    while (waiting)
      yield return (object) null;
    Vector3 startingPosition = follower.Brain.LastPosition;
    float angle = Utils.GetAngle(follower.Brain.LastPosition, ChurchFollowerManager.Instance.RitualCenterPosition.position);
    FollowerTask_ManualControl task = new FollowerTask_ManualControl();
    follower.Brain.HardSwapToTask((FollowerTask) task);
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, (double) UnityEngine.Random.value < 0.5 ? "Rituals/cannibal-run" : "Rituals/cannibal-run2");
    waiting = true;
    task.GoToAndStop(follower, ChurchFollowerManager.Instance.RitualCenterPosition.position - (Vector3) Utils.DegreeToVector2(angle) * UnityEngine.Random.Range(0.5f, 0.75f), (System.Action) (() => waiting = false));
    follower.SetOutfit(FollowerOutfitType.Worshipper, true);
    follower.PlayVO("event:/dialogue/followers/cannibal_run", follower.gameObject);
    while (waiting)
      yield return (object) null;
    follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    double num = (double) follower.SetBodyAnimation("Rituals/cannibal-eat", true);
    EventInstance loop = follower.PlayLoopedVO("event:/dialogue/followers/cannibal_eat_loop", follower.gameObject);
    this.loops.Add(loop);
    yield return (object) new WaitForSeconds(UnityEngine.Random.Range(4.5f, 5f));
    AudioManager.Instance.StopLoop(loop);
    this.loops.Remove(loop);
    follower.SimpleAnimator.ResetAnimationsToDefaults();
    yield return (object) null;
    task.GoToAndStop(follower, startingPosition, (System.Action) (() =>
    {
      follower.State.facingAngle = Utils.GetAngle(follower.transform.position, ChurchFollowerManager.Instance.RitualCenterPosition.position);
      follower.State.CURRENT_STATE = StateMachine.State.Idle;
    }));
  }

  public void EndRitual()
  {
    ChurchFollowerManager.Instance.EndRitualOverlay();
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
      followerBrain.CompleteCurrentTask();
  }

  public void OnDestroy()
  {
    AudioManager.Instance.StopLoop(this.eatingLoop);
    for (int index = this.loops.Count - 1; index >= 0; --index)
      AudioManager.Instance.StopLoop(this.loops[index]);
    this.loops.Clear();
  }
}
