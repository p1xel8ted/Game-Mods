// Decompiled with JetBrains decompiler
// Type: RitualNudism
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.FollowerSelect;
using MMTools;
using src.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class RitualNudism : Ritual
{
  public static System.Action OnNudismBegan;
  public List<EventInstance> loops = new List<EventInstance>();
  public bool givenOutfit;

  public override UpgradeSystem.Type RitualType => UpgradeSystem.Type.Ritual_Nudism;

  public override void Play()
  {
    base.Play();
    GameManager.GetInstance().StartCoroutine(this.RitualRoutine());
  }

  public IEnumerator RitualRoutine()
  {
    RitualNudism ritualNudism = this;
    AudioManager.Instance.PlayOneShot("event:/rituals/generic_start_ritual");
    Interaction_TempleAltar.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    PlayerFarming.Instance.simpleSpineAnimator.Animate("idle", 0, true);
    Interaction_TempleAltar.Instance.SimpleSetCamera.Play();
    yield return (object) ritualNudism.StartCoroutine(ritualNudism.WaitFollowersFormCircle());
    yield return (object) new WaitForSeconds(1f);
    PlayerFarming.Instance.simpleSpineAnimator.Animate("build", 0, true);
    PlayerFarming.Instance.Spine.skeleton.FindBone("ritualring").Rotation += 60f;
    PlayerFarming.Instance.Spine.skeleton.UpdateWorldTransform();
    PlayerFarming.Instance.Spine.skeleton.Update(Time.deltaTime);
    PlayerFarming.Instance.simpleSpineAnimator.Animate("rituals/ritual-start", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("rituals/ritual-loop", 0, true, 0.0f);
    Interaction_TempleAltar.Instance.RitualLighting.gameObject.SetActive(true);
    float previousNudismDeclared = DataManager.Instance.LastNudismDeclared;
    DataManager.Instance.LastNudismDeclared = TimeManager.TotalElapsedGameTime;
    BiomeConstants.Instance.VignetteTween(2f, BiomeConstants.Instance.VignetteDefaultValue, 0.7f);
    DOTween.To((DOGetter<float>) (() => GameManager.GetInstance().CamFollowTarget.targetDistance), (DOSetter<float>) (x => GameManager.GetInstance().CamFollowTarget.targetDistance = x), 6.5f, 1f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutSine);
    yield return (object) new WaitForSeconds(1f);
    AudioManager.Instance.SetMusicBaseID(SoundConstants.BaseID.nudist_intro);
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      Follower follower = FollowerManager.FindFollowerByID(followerBrain.Info.ID);
      if ((UnityEngine.Object) follower != (UnityEngine.Object) null)
        follower.HoodOff(onComplete: (System.Action) (() =>
        {
          follower.GoTo(ChurchFollowerManager.Instance.DoorPosition.position, (System.Action) null);
          if (follower.Brain.Info.IsDrunk)
            follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "prance-drunk");
          else
            follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "prance");
          if ((double) UnityEngine.Random.value < 0.5)
            this.loops.Add(follower.PlayLoopedVO("event:/dialogue/followers/naked_laughing", follower.gameObject));
          else
            this.loops.Add(follower.PlayLoopedVO("event:/dialogue/followers/naked_laughing_hehe", follower.gameObject));
        }));
    }
    yield return (object) new WaitForSeconds(2f);
    bool waiting = true;
    MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 0.4f, "", (System.Action) (() => waiting = false));
    while (waiting)
      yield return (object) null;
    List<GameObject> hiddenObjects = new List<GameObject>();
    List<FollowerManager.SpawnedFollower> spawnedCopyFollowers = new List<FollowerManager.SpawnedFollower>();
    List<FollowerManager.SpawnedFollower> followers = new List<FollowerManager.SpawnedFollower>();
    Ritual.FollowerToAttendSermon.Shuffle<FollowerBrain>();
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      FollowerManager.SpawnedFollower spawnedFollower = FollowerManager.SpawnCopyFollower(followerBrain._directInfoAccess, TownCentre.Instance.Centre.position, (Transform) null, FollowerLocation.Base);
      followers.Add(spawnedFollower);
      spawnedCopyFollowers.Add(spawnedFollower);
      if (followerBrain.Info.IsDrunk)
      {
        double num1 = (double) spawnedFollower.Follower.SetBodyAnimation("prance-drunk", true);
      }
      else
      {
        double num2 = (double) spawnedFollower.Follower.SetBodyAnimation("prance", true);
      }
    }
    Vector3 previousPlayerPos = PlayerFarming.Instance.transform.position;
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      player.Spine.gameObject.SetActive(false);
      player.SetCoopIndicatorVisibility(false);
    }
    PlayerFarming.Instance.transform.position = ChurchFollowerManager.Instance.RitualCenterPosition.position;
    BaseLocationManager.Instance.Activatable = false;
    ChurchLocationManager.Instance.Activatable = false;
    BiomeConstants.Instance.DepthOfFieldTween(0.15f, 8.7f, 26f, 0.0f, 0.0f);
    BiomeBaseManager.Instance.ActivateRoom(false);
    Vector3 camPosition = GameManager.GetInstance().CamFollowTarget.transform.position;
    GameManager.GetInstance().CamFollowTarget.ResetTargetCamera(0.0f);
    yield return (object) new WaitForEndOfFrame();
    GameManager.GetInstance().CamFollowTarget.ClearAllTargets();
    BuildingShrine shrine = BuildingShrine.Shrines[0];
    Vector3 position1 = shrine.transform.position - GameManager.GetInstance().CamFollowTarget.transform.forward * 10f;
    GameManager.GetInstance().CamFollowTarget.ForceSnapTo(position1);
    GameManager.GetInstance().CamFollowTarget.transform.localRotation = Quaternion.Euler(-45f, 0.0f, 0.0f);
    shrine.ShowNudismDecos();
    foreach (Component deadWorshipper in DeadWorshipper.DeadWorshippers)
      hiddenObjects.Add(deadWorshipper.gameObject);
    foreach (Component poop in Interaction_Poop.Poops)
      hiddenObjects.Add(poop.gameObject);
    foreach (Component vomit in Vomit.Vomits)
      hiddenObjects.Add(vomit.gameObject);
    foreach (Follower follower in Follower.Followers)
    {
      if (follower.Brain.Info.CursedState == Thought.Child)
        hiddenObjects.Add(follower.gameObject);
      follower.HideAllFollowerIcons();
    }
    foreach (GameObject gameObject in hiddenObjects)
    {
      if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
        gameObject.SetActive(false);
    }
    yield return (object) new WaitForSeconds(0.1f);
    float rotation = 0.0f;
    float rotationSpeed = 30f;
    float rotationSpeedMultiplier = 1f;
    float progress = 0.0f;
    int direction = 1;
    float directionProgress = 0.0f;
    float directionChangeTarget = UnityEngine.Random.Range(6f, 9f);
    Follower winningFollower = (Follower) null;
    FollowerInfo winningFollowerInfo = (FollowerInfo) null;
    bool cancelRitual = false;
    CanvasGroup canvasGroup = PlayerFarming.Instance.indicator.ContainerImage.GetComponent<CanvasGroup>();
    foreach (Follower follower in Follower.Followers)
    {
      if (follower.Brain.Info.CursedState == Thought.Ill || follower.Brain.Info.CursedState == Thought.BecomeStarving)
      {
        double num = (double) follower.SetBodyAnimation(follower.Brain.CurrentState.OverrideIdleAnim != null ? follower.Brain.CurrentState.OverrideIdleAnim : "idle", true);
        if (follower.State.CURRENT_STATE == StateMachine.State.Moving)
          follower.State.CURRENT_STATE = StateMachine.State.Idle;
      }
      else if (follower.Brain.HasTrait(FollowerTrait.TraitType.Zombie))
      {
        double num = (double) follower.SetBodyAnimation("Zombie/zombie-idle", true);
        if (follower.State.CURRENT_STATE == StateMachine.State.Moving)
          follower.State.CURRENT_STATE = StateMachine.State.Idle;
      }
      else if (follower.Brain.HasTrait(FollowerTrait.TraitType.ExistentialDread) || follower.Brain.HasTrait(FollowerTrait.TraitType.MissionaryTerrified))
      {
        double num = (double) follower.SetBodyAnimation("Existential Dread/dread-idle", true);
        if (follower.State.CURRENT_STATE == StateMachine.State.Moving)
          follower.State.CURRENT_STATE = StateMachine.State.Idle;
      }
    }
    GameManager.GetInstance().WaitForSeconds(1.5f, (System.Action) (() =>
    {
      Vector3 from = GameManager.GetInstance().CamFollowTarget.transform.position;
      Vector3 to = new Vector3(GameManager.GetInstance().CamFollowTarget.transform.position.x - 9f, GameManager.GetInstance().CamFollowTarget.transform.position.y, GameManager.GetInstance().CamFollowTarget.transform.position.z);
      float time = 0.0f;
      DOTween.To((DOGetter<float>) (() => time), (DOSetter<float>) (x => time = x), 1f, 2f).OnUpdate<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => GameManager.GetInstance().CamFollowTarget.ForceSnapTo(Vector3.Lerp(from, to, time / 2f)))).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InOutSine);
      GameManager.GetInstance().WaitForSeconds(2f, (System.Action) (() =>
      {
        UIFollowerSelectMenuController followerSelectMenu = MonoSingleton<UIManager>.Instance.FollowerSelectMenuTemplate.Instantiate<UIFollowerSelectMenuController>();
        followerSelectMenu.VotingType = TwitchVoting.VotingType.RITUAL_NUDISM;
        followerSelectMenu.Show(this.GetFollowerSelectEntries(), false, UpgradeSystem.Type.Count, true, DataManager.Instance.HasPerformedPleasureRitual, true, false, true);
        followerSelectMenu.SetBackgroundState(false);
        followerSelectMenu.SetHeaderText(LocalizationManager.GetTranslation("UI/SelectMayflowerLeader"));
        followerSelectMenu.OnFollowerSelected += (Action<FollowerInfo>) (info => winningFollowerInfo = info);
        followerSelectMenu.OnShow += (System.Action) (() =>
        {
          foreach (FollowerInformationBox followerInfoBox in followerSelectMenu.FollowerInfoBoxes)
          {
            FollowerBrain.PleasureActions pleasureAction = followerInfoBox.FollowerInfo.IsDrunk ? FollowerBrain.PleasureActions.NudismDrunk : FollowerBrain.PleasureActions.NudismDrunk;
            followerInfoBox.ShowPleasure(DataManager.Instance.HasPerformedPleasureRitual ? FollowerBrain.GetPleasureAmount(pleasureAction) : 65);
          }
        });
        followerSelectMenu.OnShownCompleted += (System.Action) (() =>
        {
          foreach (FollowerInformationBox followerInfoBox in followerSelectMenu.FollowerInfoBoxes)
          {
            FollowerBrain.PleasureActions pleasureAction = followerInfoBox.FollowerInfo.IsDrunk ? FollowerBrain.PleasureActions.NudismDrunk : FollowerBrain.PleasureActions.NudismDrunk;
            followerInfoBox.ShowPleasure(DataManager.Instance.HasPerformedPleasureRitual ? FollowerBrain.GetPleasureAmount(pleasureAction) : 65);
          }
        });
        followerSelectMenu.OnCancel += (System.Action) (() =>
        {
          cancelRitual = true;
          DataManager.Instance.LastNudismDeclared = previousNudismDeclared;
        });
      }));
    }));
    while (winningFollowerInfo == null && !cancelRitual)
    {
      progress += Time.deltaTime;
      directionProgress += Time.deltaTime;
      if ((double) directionProgress > (double) directionChangeTarget)
      {
        direction *= -1;
        directionProgress = 0.0f;
        directionChangeTarget = UnityEngine.Random.Range(6f, 9f);
      }
      rotation += rotationSpeed * rotationSpeedMultiplier * Time.deltaTime * (float) direction;
      for (int index = 0; index < spawnedCopyFollowers.Count; ++index)
      {
        if (spawnedCopyFollowers[index] != null)
        {
          Vector3 position2 = ritualNudism.GetPosition(index, followers.Count, rotation);
          spawnedCopyFollowers[index].Follower.FacePosition(position2);
          spawnedCopyFollowers[index].Follower.transform.position = position2;
        }
      }
      yield return (object) null;
    }
    for (int index = ritualNudism.loops.Count - 1; index >= 0; --index)
      AudioManager.Instance.StopLoop(ritualNudism.loops[index]);
    ritualNudism.loops.Clear();
    waiting = true;
    MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 0.4f, "", (System.Action) (() => waiting = false));
    while (waiting)
      yield return (object) null;
    Interactor.Overriding = false;
    canvasGroup.alpha = 1f;
    shrine.HideNudismDecos();
    foreach (Follower follower in Follower.Followers)
      follower.ShowAllFollowerIcons();
    for (int index = 0; index < spawnedCopyFollowers.Count; ++index)
      FollowerManager.CleanUpCopyFollower(spawnedCopyFollowers[index]);
    spawnedCopyFollowers.Clear();
    foreach (GameObject gameObject in hiddenObjects)
      gameObject.SetActive(true);
    hiddenObjects.Clear();
    BiomeConstants.Instance.DepthOfFieldTween(0.15f, 8.7f, 26f, 1f, 200f);
    GameManager.GetInstance().CamFollowTarget.targetDistance = 11f;
    GameManager.GetInstance().CamFollowTarget.ClearAllTargets();
    GameManager.GetInstance().CamFollowTarget.ForceSnapTo(camPosition);
    Interaction_TempleAltar.Instance.SimpleSetCamera.Play();
    BiomeBaseManager.Instance.ActivateChurch(false);
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      player.Spine.gameObject.SetActive(true);
      player.SetCoopIndicatorVisibility(true);
    }
    PlayerFarming.Instance.transform.position = previousPlayerPos;
    BaseLocationManager.Instance.Activatable = true;
    ChurchLocationManager.Instance.Activatable = true;
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      if (followerBrain != null)
      {
        if (followerBrain.CurrentTask is FollowerTask_AttendRitual)
          ((FollowerTask_AttendRitual) followerBrain.CurrentTask).hoodOn = false;
        Follower followerById = FollowerManager.FindFollowerByID(followerBrain.Info.ID);
        if ((UnityEngine.Object) followerById != (UnityEngine.Object) null)
        {
          followerById.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
          followerById.transform.position = ChurchFollowerManager.Instance.DoorPosition.position;
          if (winningFollowerInfo != null && followerBrain.Info.ID == winningFollowerInfo.ID)
            winningFollower = followerById;
        }
      }
    }
    yield return (object) null;
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
      followerBrain?.HardSwapToTask((FollowerTask) new FollowerTask_AttendRitual(false));
    if ((UnityEngine.Object) winningFollower != (UnityEngine.Object) null)
    {
      ChurchFollowerManager.Instance.RemoveBrainFromAudience(winningFollower.Brain);
      Ritual.FollowerToAttendSermon.Remove(winningFollower.Brain);
    }
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      followerBrain.CurrentTask.RecalculateDestination();
      followerBrain.CurrentTask.Arrive();
    }
    yield return (object) new WaitForSeconds(1f);
    if (cancelRitual)
    {
      AudioManager.Instance.SetMusicBaseID(SoundConstants.BaseID.Temple);
      foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
        followerBrain.CheckChangeState();
      yield return (object) new WaitForSeconds(2.5f);
      BiomeConstants.Instance.VignetteTween(1f, 0.7f, BiomeConstants.Instance.VignetteDefaultValue);
      Interaction_TempleAltar.Instance.CloseUpCamera.Reset();
      ritualNudism.EndRitual();
      ritualNudism.CompleteRitual(true);
    }
    else
    {
      winningFollower.Brain.Info.NudistWinner = true;
      FollowerBrain.SetFollowerCostume(winningFollower.Spine.Skeleton, winningFollower.Brain._directInfoAccess, forceUpdate: true);
      winningFollower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
      winningFollower.transform.position = ChurchFollowerManager.Instance.RitualCenterPosition.position;
      winningFollower.FacePosition(ChurchFollowerManager.Instance.DoorPosition.position);
      winningFollower.Spine.transform.localPosition = new Vector3(0.0f, 0.0f, -15f);
      winningFollower.Spine.transform.DOLocalMove(Vector3.zero, 4f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
      ChurchFollowerManager.Instance.GodRays.gameObject.SetActive(true);
      ChurchFollowerManager.Instance.GodRays.GetComponent<ParticleSystem>().Play();
      ChurchFollowerManager.Instance.Goop.gameObject.SetActive(true);
      ChurchFollowerManager.Instance.Goop.Play("Show");
      ChurchFollowerManager.Instance.Goop.GetComponentInChildren<MeshRenderer>().material.SetColor("_TintCOlor", Color.white);
      AudioManager.Instance.PlayOneShot("event:/Stings/ritual_ascension_pg", PlayerFarming.Instance.gameObject);
      yield return (object) null;
      double num3 = (double) winningFollower.SetBodyAnimation("floating", true);
      yield return (object) new WaitForSeconds(2.5f);
      foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
      {
        if (followerBrain.CurrentTask is FollowerTask_AttendRitual)
          (followerBrain.CurrentTask as FollowerTask_AttendRitual).Cheer();
      }
      yield return (object) new WaitForSeconds(1.5f);
      double num4 = (double) winningFollower.SetBodyAnimation("Egg/mating-pose3", false);
      winningFollower.AddBodyAnimation("dance", true, 0.0f);
      ChurchFollowerManager.Instance.GodRays.GetComponent<ParticleSystem>().Stop();
      ChurchFollowerManager.Instance.Goop.Play("Hide");
      yield return (object) new WaitForSeconds(1.5333333f);
      ChurchFollowerManager.Instance.Goop.gameObject.SetActive(false);
      ChurchFollowerManager.Instance.GodRays.SetActive(false);
      ChurchFollowerManager.Instance.Sparkles.Stop();
      if (!DataManager.Instance.HasPerformedPleasureRitual)
      {
        winningFollower.Brain.AddPleasure(FollowerBrain.PleasureActions.Testing);
      }
      else
      {
        float multiplier = DataManager.Instance.HasPerformedPleasureRitual ? 1f : 10f;
        if (winningFollower.Brain.Info.IsDrunk)
          winningFollower.Brain.AddPleasure(FollowerBrain.PleasureActions.NudismDrunk, multiplier);
        else
          winningFollower.Brain.AddPleasure(FollowerBrain.PleasureActions.NudismWinner, multiplier);
      }
      DataManager.Instance.HasPerformedPleasureRitual = true;
      if (!winningFollower.Brain.CanGiveSin())
        yield return (object) new WaitForSeconds(1.5333333f);
      while (winningFollower.InGiveSinSequence)
        yield return (object) null;
      double num5 = (double) winningFollower.SetBodyAnimation("idle", true);
      foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
      {
        if (followerBrain.CurrentTask is FollowerTask_AttendRitual)
          (followerBrain.CurrentTask as FollowerTask_AttendRitual).Dance();
      }
      yield return (object) new WaitForSeconds(1f);
      AudioManager.Instance.SetFollowersDance(0.0f);
      BiomeConstants.Instance.VignetteTween(1f, 0.7f, BiomeConstants.Instance.VignetteDefaultValue);
      GameManager.GetInstance().CamFollowTarget.targetDistance = 11f;
      Interaction_TempleAltar.Instance.RitualLighting.gameObject.SetActive(false);
      if ((UnityEngine.Object) winningFollower != (UnityEngine.Object) null)
      {
        Ritual.FollowerToAttendSermon.Add(winningFollower.Brain);
        ChurchFollowerManager.Instance.AddBrainToAudience(winningFollower.Brain);
      }
      float num6 = 0.0f;
      for (int index = Ritual.FollowerToAttendSermon.Count - 1; index >= 0; --index)
      {
        float Delay = UnityEngine.Random.Range(0.1f, 0.5f);
        num6 += Delay;
        ritualNudism.StartCoroutine(ritualNudism.DelayFollowerReaction(Ritual.FollowerToAttendSermon[index], Delay));
        Ritual.FollowerToAttendSermon[index].AddThought((Thought) UnityEngine.Random.Range(346, 350));
      }
      if (DataManager.Instance.TailorEnabled && !DataManager.Instance.UnlockedClothing.Contains(FollowerClothingType.Naked) && !ritualNudism.givenOutfit)
      {
        ++DataManager.Instance.NudeClothingCount;
        if (DataManager.Instance.NudeClothingCount >= 5)
        {
          InventoryItem.Spawn(InventoryItem.ITEM_TYPE.FOUND_ITEM_OUTFIT, 1, ChurchFollowerManager.Instance.RitualCenterPosition.position).GetComponent<FoundItemPickUp>().clothingType = FollowerClothingType.Naked;
          ritualNudism.givenOutfit = true;
          yield return (object) new WaitForSeconds(0.5f);
        }
      }
      yield return (object) new WaitForSeconds(1.5f);
      Interaction_TempleAltar.Instance.CloseUpCamera.Reset();
      System.Action onNudismBegan = RitualNudism.OnNudismBegan;
      if (onNudismBegan != null)
        onNudismBegan();
      ritualNudism.CompleteRitual(PlayFakeBar: false);
      BaseLocationManager.Instance.EnableNudism();
      yield return (object) new WaitForSeconds(1f);
      CultFaithManager.AddThought(Thought.Cult_NudistRitual);
      DataManager.Instance.LastNudismDeclared = TimeManager.TotalElapsedGameTime;
    }
  }

  public List<FollowerSelectEntry> GetFollowerSelectEntries()
  {
    List<FollowerSelectEntry> followerSelectEntries = new List<FollowerSelectEntry>();
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
      followerSelectEntries.Add(new FollowerSelectEntry(followerBrain, FollowerManager.GetFollowerAvailabilityStatus(followerBrain)));
    return followerSelectEntries;
  }

  public void SelectedLoser(
    FollowerManager.SpawnedFollower losingFollower,
    List<FollowerManager.SpawnedFollower> followers)
  {
    for (int index = 0; index < losingFollower.Follower.transform.childCount; ++index)
    {
      if (losingFollower.Follower.transform.GetChild(index).CompareTag("Decoration"))
      {
        UnityEngine.Object.Destroy((UnityEngine.Object) losingFollower.Follower.transform.GetChild(index).gameObject);
        break;
      }
    }
    float time = 0.0f;
    DOTween.To((DOGetter<float>) (() => time), (DOSetter<float>) (x => time = x), 1f, 1f).OnUpdate<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => losingFollower.Follower.Spine.Skeleton.A = 1f - time));
    followers[followers.IndexOf(losingFollower)] = (FollowerManager.SpawnedFollower) null;
  }

  public Vector3 GetPosition(int index, int count, float offset, float distance = 3f)
  {
    float f = (float) (((double) index * (360.0 / (double) count) + (double) offset) * (Math.PI / 180.0));
    return TownCentre.Instance.Centre.transform.position + new Vector3(distance * Mathf.Cos(f), distance * Mathf.Sin(f));
  }

  public IEnumerator SetFollowerExhausted(
    FollowerManager.SpawnedFollower follower,
    float duration,
    List<FollowerManager.SpawnedFollower> followers)
  {
    double num = (double) follower.Follower.SetBodyAnimation("Fatigued/walk-fatigued", true);
    Addressables.InstantiateAsync((object) "Assets/Plugins/Epic Toon FX/Prefabs/Misc/SleepingZzZ.prefab", follower.Follower.transform).Completed += (Action<AsyncOperationHandle<GameObject>>) (obj => obj.Result.transform.localPosition = new Vector3(0.0f, 0.0f, -1f));
    yield return (object) new WaitForSeconds(duration);
    this.SelectedLoser(follower, followers);
  }

  public void EndRitual()
  {
    Interaction_TempleAltar.Instance.RitualLighting.gameObject.SetActive(false);
    GameManager.GetInstance().CamFollowTarget.targetDistance = 11f;
    Interaction_TempleAltar.Instance.SimpleSetCamera.Reset();
  }

  public void OnDestroy()
  {
    for (int index = this.loops.Count - 1; index >= 0; --index)
      AudioManager.Instance.StopLoop(this.loops[index]);
    this.loops.Clear();
  }
}
