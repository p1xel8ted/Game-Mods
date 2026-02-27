// Decompiled with JetBrains decompiler
// Type: RitualPurge
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
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class RitualPurge : Ritual
{
  public static System.Action OnPurgeBegan;
  public List<EventInstance> loops = new List<EventInstance>();
  public EventInstance music;
  public EventInstance fightLoop;
  public Structure target;
  public bool canAnimate = true;

  public override UpgradeSystem.Type RitualType => UpgradeSystem.Type.Ritual_Purge;

  public override void Play()
  {
    base.Play();
    GameManager.GetInstance().StartCoroutine(this.RitualRoutine());
  }

  public IEnumerator RitualRoutine()
  {
    RitualPurge ritualPurge = this;
    AudioManager.Instance.PlayOneShot("event:/rituals/generic_start_ritual");
    Interaction_TempleAltar.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    PlayerFarming.Instance.simpleSpineAnimator.Animate("idle", 0, true);
    Interaction_TempleAltar.Instance.SimpleSetCamera.Play();
    yield return (object) ritualPurge.StartCoroutine(ritualPurge.WaitFollowersFormCircle());
    yield return (object) new WaitForSeconds(1f);
    SimulationManager.Pause();
    PlayerFarming.Instance.simpleSpineAnimator.Animate("build", 0, true);
    PlayerFarming.Instance.Spine.skeleton.FindBone("ritualring").Rotation += 60f;
    PlayerFarming.Instance.Spine.skeleton.UpdateWorldTransform();
    PlayerFarming.Instance.Spine.skeleton.Update(Time.deltaTime);
    PlayerFarming.Instance.simpleSpineAnimator.Animate("rituals/ritual-start", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("rituals/ritual-loop", 0, true, 0.0f);
    Interaction_TempleAltar.Instance.RitualLighting.gameObject.SetActive(true);
    ChurchFollowerManager.Instance.StartRitualOverlay();
    BiomeConstants.Instance.ChromaticAbberationTween(2f, BiomeConstants.Instance.ChromaticAberrationDefaultValue, 1f);
    BiomeConstants.Instance.VignetteTween(2f, BiomeConstants.Instance.VignetteDefaultValue, 0.7f);
    DOTween.To((DOGetter<float>) (() => GameManager.GetInstance().CamFollowTarget.targetDistance), (DOSetter<float>) (x => GameManager.GetInstance().CamFollowTarget.targetDistance = x), 6.5f, 1f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutSine);
    yield return (object) new WaitForSeconds(1f);
    bool waiting = true;
    FollowerInfo winningFollowerInfo = (FollowerInfo) null;
    UIFollowerSelectMenuController followerSelectMenu = MonoSingleton<UIManager>.Instance.FollowerSelectMenuTemplate.Instantiate<UIFollowerSelectMenuController>();
    followerSelectMenu.VotingType = TwitchVoting.VotingType.RITUAL_PURGE;
    followerSelectMenu.Show(ritualPurge.GetFollowerSelectEntries(), false, UpgradeSystem.Type.Count, true, DataManager.Instance.HasPerformedPleasureRitual, true, false, true);
    followerSelectMenu.SetBackgroundState(false);
    followerSelectMenu.SetHeaderText(LocalizationManager.GetTranslation("UI/SelectPurgeLeader"));
    UIFollowerSelectMenuController selectMenuController1 = followerSelectMenu;
    selectMenuController1.OnFollowerSelected = selectMenuController1.OnFollowerSelected + (Action<FollowerInfo>) (info =>
    {
      winningFollowerInfo = info;
      waiting = false;
    });
    UIFollowerSelectMenuController selectMenuController2 = followerSelectMenu;
    selectMenuController2.OnShow = selectMenuController2.OnShow + (System.Action) (() =>
    {
      foreach (FollowerInformationBox followerInfoBox in followerSelectMenu.FollowerInfoBoxes)
      {
        FollowerBrain.PleasureActions pleasureAction = followerInfoBox.FollowerInfo.CursedState == Thought.Dissenter ? FollowerBrain.PleasureActions.PurgeDissenter : FollowerBrain.PleasureActions.Purge;
        followerInfoBox.ShowPleasure(DataManager.Instance.HasPerformedPleasureRitual ? FollowerBrain.GetPleasureAmount(pleasureAction) : 65);
      }
    });
    UIFollowerSelectMenuController selectMenuController3 = followerSelectMenu;
    selectMenuController3.OnShownCompleted = selectMenuController3.OnShownCompleted + (System.Action) (() =>
    {
      foreach (FollowerInformationBox followerInfoBox in followerSelectMenu.FollowerInfoBoxes)
      {
        FollowerBrain.PleasureActions pleasureAction = followerInfoBox.FollowerInfo.CursedState == Thought.Dissenter ? FollowerBrain.PleasureActions.PurgeDissenter : FollowerBrain.PleasureActions.Purge;
        followerInfoBox.ShowPleasure(DataManager.Instance.HasPerformedPleasureRitual ? FollowerBrain.GetPleasureAmount(pleasureAction) : 65);
      }
    });
    UIFollowerSelectMenuController selectMenuController4 = followerSelectMenu;
    selectMenuController4.OnCancel = selectMenuController4.OnCancel + (System.Action) (() =>
    {
      AudioManager.Instance.StopLoop(this.loopedSound);
      BiomeConstants.Instance.ChromaticAbberationTween(1f, 1f, BiomeConstants.Instance.ChromaticAberrationDefaultValue);
      BiomeConstants.Instance.VignetteTween(1f, 0.7f, BiomeConstants.Instance.VignetteDefaultValue);
      this.EndRitual();
      this.CancelFollowers();
      this.CompleteRitual(true);
    });
    UIFollowerSelectMenuController selectMenuController5 = followerSelectMenu;
    selectMenuController5.OnHidden = selectMenuController5.OnHidden + (System.Action) (() => followerSelectMenu = (UIFollowerSelectMenuController) null);
    while (waiting)
      yield return (object) null;
    ritualPurge.music = AudioManager.Instance.CreateLoop("event:/Stings/purge_ritual_loop", true);
    Follower selectedFollower = FollowerManager.FindFollowerByID(winningFollowerInfo.ID);
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      Follower followerById = FollowerManager.FindFollowerByID(followerBrain.Info.ID);
      if ((UnityEngine.Object) followerById != (UnityEngine.Object) null)
      {
        UnityEngine.Random.Range(1, 5);
        string NewAnimation1 = "Riot/riot-purge-run2";
        string NewAnimation2 = (UnityEngine.Object) followerById == (UnityEngine.Object) selectedFollower ? "Riot/riot-purge-idle4" : "Riot/riot-purge-idle1";
        followerById.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, NewAnimation2);
        followerById.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, NewAnimation1);
      }
    }
    Ritual.FollowerToAttendSermon.Remove(selectedFollower.Brain);
    waiting = true;
    selectedFollower.HoodOff(onComplete: (System.Action) (() =>
    {
      this.loops.Add(selectedFollower.PlayLoopedVO("event:/dialogue/followers/pitchfork_loop", selectedFollower.gameObject));
      selectedFollower.GoTo(ChurchFollowerManager.Instance.RitualCenterPosition.position, (System.Action) (() => waiting = false));
    }));
    GameManager.GetInstance().WaitForSeconds(1.5f, (System.Action) (() =>
    {
      foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
      {
        Follower follower = FollowerManager.FindFollowerByID(followerBrain.Info.ID);
        if ((UnityEngine.Object) follower != (UnityEngine.Object) null)
        {
          follower.HoodOff(onComplete: (System.Action) (() =>
          {
            follower.Brain.CurrentTask.SetState(FollowerTaskState.Idle);
            this.loops.Add(follower.PlayLoopedVO("event:/dialogue/followers/pitchfork_loop", follower.gameObject));
          }));
          follower.OverridingEmotions = true;
          follower.SetFaceAnimation("Emotions/emotion-angry", true);
        }
      }
    }));
    while (waiting)
      yield return (object) null;
    int i;
    for (i = 0; i < 4; ++i)
    {
      yield return (object) new WaitForSeconds(1f);
      waiting = true;
      selectedFollower.GoTo(ChurchFollowerManager.Instance.RitualCenterPosition.position + (Vector3) UnityEngine.Random.insideUnitCircle * 1.85f, (System.Action) (() => waiting = false));
      while (waiting)
        yield return (object) null;
      foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
      {
        Follower followerById = FollowerManager.FindFollowerByID(followerBrain.Info.ID);
        if ((UnityEngine.Object) followerById != (UnityEngine.Object) null)
        {
          string NewAnimation = "Riot/riot-purge-idle" + (i + 1).ToString();
          followerById.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, NewAnimation);
        }
      }
    }
    selectedFollower.GoTo(ChurchFollowerManager.Instance.DoorPosition.position, (System.Action) null);
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      Follower follower = FollowerManager.FindFollowerByID(followerBrain.Info.ID);
      if ((UnityEngine.Object) follower != (UnityEngine.Object) null)
        follower.HoodOff(onComplete: (System.Action) (() => follower.GoTo(ChurchFollowerManager.Instance.DoorPosition.position, (System.Action) null)));
    }
    yield return (object) new WaitForSeconds(2f);
    for (int index = ritualPurge.loops.Count - 1; index >= 0; --index)
      AudioManager.Instance.StopLoop(ritualPurge.loops[index]);
    ritualPurge.loops.Clear();
    waiting = true;
    MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 0.4f, "", (System.Action) (() => waiting = false));
    while (waiting)
      yield return (object) null;
    Ritual.FollowerToAttendSermon.Add(selectedFollower.Brain);
    List<FollowerManager.SpawnedFollower> followers = new List<FollowerManager.SpawnedFollower>();
    Ritual.FollowerToAttendSermon.Shuffle<FollowerBrain>();
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      FollowerManager.SpawnedFollower spawnedFollower = FollowerManager.SpawnCopyFollower(followerBrain._directInfoAccess, TownCentre.Instance.Centre.position, (Transform) null, FollowerLocation.Base);
      spawnedFollower.Follower.gameObject.SetActive(false);
      followers.Add(spawnedFollower);
    }
    Ritual.FollowerToAttendSermon.Remove(selectedFollower.Brain);
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      player.Spine.gameObject.SetActive(false);
      player.SetCoopIndicatorVisibility(false);
    }
    BaseLocationManager.Instance.Activatable = false;
    ChurchLocationManager.Instance.Activatable = false;
    BiomeConstants.Instance.DepthOfFieldTween(0.15f, 8.7f, 26f, 0.0f, 0.0f);
    BiomeBaseManager.Instance.ActivateRoom(false);
    Vector3 camPosition = GameManager.GetInstance().CamFollowTarget.transform.position;
    GameManager.GetInstance().CamFollowTarget.ResetTargetCamera(0.0f);
    GameManager.GetInstance().CamFollowTarget.ClearAllTargets();
    Vector3 vector3 = BuildingShrine.Shrines[0].transform.position - GameManager.GetInstance().CamFollowTarget.transform.forward * 10f;
    GameManager.GetInstance().CamFollowTarget.transform.localRotation = Quaternion.Euler(-45f, 0.0f, 0.0f);
    List<GameObject> hiddenObjects = new List<GameObject>();
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
      gameObject.SetActive(false);
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
    List<StructureBrain> possibleStructures = new List<StructureBrain>();
    List<StructureBrain> ts = new List<StructureBrain>((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType<StructureBrain>());
    ts.Shuffle<StructureBrain>();
    foreach (StructureBrain structureBrain in ts)
    {
      if (StructureManager.IsCollapsible(structureBrain.Data.Type) && !structureBrain.Data.IsCollapsed && !structureBrain.Data.IsSnowedUnder && (double) structureBrain.Data.Position.y < -2.0 && (double) structureBrain.Data.Position.y > -26.0 && (double) structureBrain.Data.Position.x > -20.0 && (double) structureBrain.Data.Position.x < 20.0 && (structureBrain.Data.Type.ToString().Contains("DECORATION") || structureBrain.Data.Type.ToString().Contains("BED")))
        possibleStructures.Add(structureBrain);
    }
    List<FollowerManager.SpawnedFollower> possibleFollowersForTerrified = new List<FollowerManager.SpawnedFollower>();
    List<FollowerManager.SpawnedFollower> possibleFollowersForInjured = new List<FollowerManager.SpawnedFollower>();
    foreach (FollowerManager.SpawnedFollower spawnedFollower in followers)
    {
      if (!spawnedFollower.FollowerBrain.HasTrait(FollowerTrait.TraitType.Scared) || !spawnedFollower.FollowerBrain.HasTrait(FollowerTrait.TraitType.CriminalScarred))
        possibleFollowersForTerrified.Add(spawnedFollower);
      if (spawnedFollower.FollowerBrain.Info.CursedState == Thought.None)
        possibleFollowersForInjured.Add(spawnedFollower);
    }
    HUD_Manager.Instance.NotificationCenter.Show(true);
    NotificationCentre.Instance.ClearNotifications();
    NotificationCentre.NotificationsEnabled = false;
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = true;
    int rand = UnityEngine.Random.Range(3, 5);
    int terrifiedFollowersCount = 0;
    int injuredFollowersCount = 0;
    int structuresDestroyedCount = 0;
    ChurchFollowerManager.Instance.SetOverlayCanvasOrder(-1);
    for (i = 0; i < rand; ++i)
    {
      int num1 = 0;
      while (num1++ < 25)
      {
        int num2 = UnityEngine.Random.Range(0, 3);
        if (num2 == 0 && possibleStructures.Count > 0 && structuresDestroyedCount <= 1)
        {
          ++structuresDestroyedCount;
          StructureBrain targetStructure = possibleStructures[UnityEngine.Random.Range(0, possibleStructures.Count)];
          possibleStructures.Remove(targetStructure);
          yield return (object) GameManager.GetInstance().StartCoroutine(ritualPurge.ShowFollowersDestroyingStructure(followers, targetStructure));
          break;
        }
        if (num2 == 1 && possibleFollowersForInjured.Count > 1 && injuredFollowersCount < 1)
        {
          ++injuredFollowersCount;
          yield return (object) GameManager.GetInstance().StartCoroutine(ritualPurge.ShowFollowersFighting(possibleFollowersForInjured));
          for (int index = possibleFollowersForInjured.Count - 1; index >= 0; --index)
          {
            if (possibleFollowersForInjured[index].FollowerBrain.Info.CursedState == Thought.Injured)
              possibleFollowersForInjured.RemoveAt(index);
          }
          break;
        }
        if (possibleFollowersForTerrified.Count > 0 && terrifiedFollowersCount < 1 && i > 0)
        {
          ++terrifiedFollowersCount;
          FollowerManager.SpawnedFollower targetFollower = possibleFollowersForTerrified[UnityEngine.Random.Range(0, possibleFollowersForTerrified.Count)];
          possibleFollowersForTerrified.Remove(targetFollower);
          yield return (object) GameManager.GetInstance().StartCoroutine(ritualPurge.ShowFollowerCrying(targetFollower));
          break;
        }
      }
      if (i < rand - 1)
      {
        waiting = true;
        MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 0.2f, "", (System.Action) (() => waiting = false));
        while (waiting)
          yield return (object) null;
        foreach (FollowerManager.SpawnedFollower spawnedFollower in followers)
          spawnedFollower.Follower.gameObject.SetActive(false);
        MMTransition.ResumePlay();
      }
    }
    waiting = true;
    MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 0.4f, "", (System.Action) (() => waiting = false));
    while (waiting)
      yield return (object) null;
    ChurchFollowerManager.Instance.SetOverlayCanvasOrder(0);
    MMTransition.ResumePlay();
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = false;
    for (int index = 0; index < followers.Count; ++index)
      FollowerManager.CleanUpCopyFollower(followers[index]);
    followers.Clear();
    foreach (GameObject gameObject in hiddenObjects)
    {
      if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
        gameObject.SetActive(true);
    }
    hiddenObjects.Clear();
    HUD_Manager.Instance.NotificationCenter.Hide(true);
    NotificationCentre.NotificationsEnabled = true;
    BiomeConstants.Instance.DepthOfFieldTween(0.15f, 8.7f, 26f, 1f, 200f);
    GameManager.GetInstance().CamFollowTarget.targetDistance = 11f;
    GameManager.GetInstance().CamFollowTarget.ClearAllTargets();
    GameManager.GetInstance().CamFollowTarget.ForceSnapTo(camPosition);
    Interaction_TempleAltar.Instance.SimpleSetCamera.Play();
    BiomeBaseManager.Instance.ActivateChurch();
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      player.Spine.gameObject.SetActive(true);
      player.SetCoopIndicatorVisibility(true);
    }
    BaseLocationManager.Instance.Activatable = true;
    ChurchLocationManager.Instance.Activatable = true;
    Ritual.FollowerToAttendSermon.Add(selectedFollower.Brain);
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
          ritualPurge.loops.Add(followerById.PlayLoopedVO("event:/dialogue/followers/pitchfork_loop", followerById.gameObject));
        }
      }
    }
    yield return (object) null;
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
      followerBrain?.HardSwapToTask((FollowerTask) new FollowerTask_AttendRitual(false));
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      followerBrain.CurrentTask.RecalculateDestination();
      followerBrain.CurrentTask.Arrive();
    }
    yield return (object) new WaitForSeconds(0.5f);
    Ritual.FollowerToAttendSermon.Remove(selectedFollower.Brain);
    waiting = true;
    selectedFollower.GoTo(ChurchFollowerManager.Instance.RitualCenterPosition.position, (System.Action) (() => waiting = false));
    while (waiting)
    {
      if (selectedFollower.Brain.CurrentTaskType == FollowerTaskType.ChangeLocation || selectedFollower.Brain.Location != PlayerFarming.Location)
      {
        selectedFollower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
        selectedFollower.transform.position = ChurchFollowerManager.Instance.DoorPosition.position;
        if (selectedFollower.Brain.CurrentTaskType == FollowerTaskType.ChangeLocation)
          selectedFollower.Brain.CurrentTask.Arrive();
      }
      yield return (object) null;
    }
    yield return (object) new WaitForEndOfFrame();
    double num3 = (double) selectedFollower.SetBodyAnimation("cheer", true);
    int num4 = DataManager.Instance.HasPerformedPleasureRitual ? 1 : 0;
    if (!DataManager.Instance.HasPerformedPleasureRitual)
      selectedFollower.Brain.AddPleasure(FollowerBrain.PleasureActions.Testing);
    else if (selectedFollower.Brain.Info.CursedState == Thought.Dissenter)
      selectedFollower.Brain.AddPleasure(FollowerBrain.PleasureActions.PurgeDissenter);
    else
      selectedFollower.Brain.AddPleasure(FollowerBrain.PleasureActions.Purge);
    yield return (object) new WaitForSeconds(0.5f);
    AudioManager.Instance.StopLoop(ritualPurge.music);
    for (int index = ritualPurge.loops.Count - 1; index >= 0; --index)
      AudioManager.Instance.StopLoop(ritualPurge.loops[index]);
    ritualPurge.loops.Clear();
    if (!selectedFollower.Brain.CanGiveSin())
      yield return (object) new WaitForSeconds(2f);
    while (selectedFollower.InGiveSinSequence)
      yield return (object) null;
    double num5 = (double) selectedFollower.SetBodyAnimation("idle", true);
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      if (followerBrain.CurrentTask is FollowerTask_AttendRitual)
        (followerBrain.CurrentTask as FollowerTask_AttendRitual).Cheer();
    }
    Ritual.FollowerToAttendSermon.Add(selectedFollower.Brain);
    yield return (object) new WaitForSeconds(1.5f);
    AudioManager.Instance.SetFollowersDance(0.0f);
    BiomeConstants.Instance.ChromaticAbberationTween(1f, 1f, BiomeConstants.Instance.ChromaticAberrationDefaultValue);
    BiomeConstants.Instance.VignetteTween(1f, 0.7f, BiomeConstants.Instance.VignetteDefaultValue);
    ChurchFollowerManager.Instance.EndRitualOverlay();
    GameManager.GetInstance().CamFollowTarget.targetDistance = 11f;
    Interaction_TempleAltar.Instance.RitualLighting.gameObject.SetActive(false);
    float num6 = 0.0f;
    for (int index = Ritual.FollowerToAttendSermon.Count - 1; index >= 0; --index)
    {
      float Delay = UnityEngine.Random.Range(0.1f, 0.5f);
      num6 += Delay;
      ritualPurge.StartCoroutine(ritualPurge.DelayFollowerReaction(Ritual.FollowerToAttendSermon[index], Delay));
      if (Ritual.FollowerToAttendSermon[index].HasTrait(FollowerTrait.TraitType.Scared) || Ritual.FollowerToAttendSermon[index].HasTrait(FollowerTrait.TraitType.Terrified) || Ritual.FollowerToAttendSermon[index].HasTrait(FollowerTrait.TraitType.CriminalScarred) || Ritual.FollowerToAttendSermon[index].HasTrait(FollowerTrait.TraitType.MissionaryTerrified))
        Ritual.FollowerToAttendSermon[index].AddThought((Thought) UnityEngine.Random.Range(343, 346));
      else
        Ritual.FollowerToAttendSermon[index].AddThought((Thought) UnityEngine.Random.Range(340, 343));
    }
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      Follower followerById = FollowerManager.FindFollowerByID(followerBrain.Info.ID);
      if ((UnityEngine.Object) followerById != (UnityEngine.Object) null)
      {
        followerById.SimpleAnimator.ResetAnimationsToDefaults();
        followerById.OverridingEmotions = false;
      }
    }
    SimulationManager.UnPause();
    yield return (object) new WaitForSeconds(1.5f);
    Interaction_TempleAltar.Instance.CloseUpCamera.Reset();
    DataManager.Instance.HasPerformedPleasureRitual = true;
    DataManager.Instance.TimeSinceLastFollowerFight = TimeManager.TotalElapsedGameTime + 30f;
    System.Action onPurgeBegan = RitualPurge.OnPurgeBegan;
    if (onPurgeBegan != null)
      onPurgeBegan();
    ritualPurge.CompleteRitual(PlayFakeBar: false);
    yield return (object) new WaitForSeconds(1f);
    CultFaithManager.AddThought(Thought.Cult_PurgeRitual);
  }

  public IEnumerator ShowFollowersFighting(
    List<FollowerManager.SpawnedFollower> spawnedFollowers)
  {
    spawnedFollowers.Shuffle<FollowerManager.SpawnedFollower>();
    FollowerManager.SpawnedFollower f1 = spawnedFollowers[0];
    FollowerManager.SpawnedFollower f2 = spawnedFollowers[1];
    f1.Follower.gameObject.SetActive(true);
    f2.Follower.gameObject.SetActive(true);
    Vector3 position = this.FindPosition();
    f1.Follower.transform.position = position + Vector3.left / 2.5f + Vector3.up * 0.05f;
    f2.Follower.transform.position = position + Vector3.right / 2.5f;
    f1.Follower.Spine.AnimationState.SetAnimation(1, "fight", true);
    f2.Follower.Spine.AnimationState.SetAnimation(1, "fight", true);
    List<WeedManager> weeds = new List<WeedManager>();
    for (int index = WeedManager.WeedManagers.Count - 1; index >= 0; --index)
    {
      if ((double) Vector3.Distance(WeedManager.WeedManagers[index].transform.position, position) < 2.0)
      {
        weeds.Add(WeedManager.WeedManagers[index]);
        WeedManager.WeedManagers[index].gameObject.SetActive(false);
      }
    }
    GameManager.GetInstance().CamFollowTarget.ForceSnapTo(position - GameManager.GetInstance().CamFollowTarget.transform.forward * 4f - Vector3.forward);
    yield return (object) new WaitForEndOfFrame();
    this.fightLoop = AudioManager.Instance.CreateLoop("event:/followers/fight_loop", true, (bool) (UnityEngine.Object) ChurchFollowerManager.Instance.RitualCenterPosition.gameObject);
    yield return (object) new WaitForSeconds(0.1f);
    f1.Follower.FacePosition(f2.Follower.transform.position);
    f2.Follower.FacePosition(f1.Follower.transform.position);
    yield return (object) new WaitForSeconds(2f);
    if ((double) UnityEngine.Random.value < 0.5)
    {
      FollowerManager.SpawnedFollower spawnedFollower = f1;
      f1 = f2;
      f2 = spawnedFollower;
    }
    AudioManager.Instance.StopLoop(this.fightLoop);
    double num1 = (double) f2.Follower.SetBodyAnimation("fight-kill", false);
    f2.Follower.Spine.AnimationState.Event += (Spine.AnimationState.TrackEntryEventDelegate) ((trackEntry, e) =>
    {
      if (!(e.Data.Name == "VO/Crazy"))
        return;
      AudioManager.Instance.PlayOneShot("event:/rituals/fight_pit_kill", f2.Follower.gameObject);
    });
    double num2 = (double) f1.Follower.SetBodyAnimation("Reactions/react-scared-long", false);
    yield return (object) new WaitForSeconds(1.1f);
    NotificationCentre.NotificationsEnabled = true;
    f1.FollowerBrain.MakeInjured(true);
    FollowerBrain.SetFollowerCostume(f1.Follower.Spine.Skeleton, f1.FollowerBrain._directInfoAccess, forceUpdate: true, setData: false);
    double num3 = (double) f1.Follower.SetBodyAnimation("Injured/idle", true);
    yield return (object) new WaitForSeconds(1f);
    double num4 = (double) f2.Follower.SetBodyAnimation("Reactions/react-laugh", true);
    AudioManager.Instance.PlayOneShot("event:/dialogue/followers/laugh", ChurchFollowerManager.Instance.RitualCenterPosition.position);
    NotificationCentre.Instance.PlayFollowerNotification(NotificationCentre.NotificationType.InjuredFromFightSingle, f1.FollowerBrain.Info, NotificationFollower.Animation.Sad);
    NotificationCentre.NotificationsEnabled = false;
    yield return (object) new WaitForSeconds(2f);
    GameManager.GetInstance().WaitForSecondsRealtime(0.5f, (System.Action) (() =>
    {
      foreach (Component component in weeds)
        component.gameObject.SetActive(true);
    }));
  }

  public IEnumerator ShowFollowersDestroyingStructure(
    List<FollowerManager.SpawnedFollower> spawnedFollowers,
    StructureBrain targetStructure)
  {
    GameManager.GetInstance().CamFollowTarget.ForceSnapTo(targetStructure.Data.Position - GameManager.GetInstance().CamFollowTarget.transform.forward * 5f - Vector3.forward);
    this.target = this.GetStructure(targetStructure);
    for (int index = 0; index < spawnedFollowers.Count; ++index)
    {
      Follower follower = spawnedFollowers[index].Follower;
      if (!((UnityEngine.Object) follower == (UnityEngine.Object) null))
      {
        follower.gameObject.SetActive(true);
        Vector3 vector3 = targetStructure.Data.Bounds.y > 1 ? Vector3.up * ((float) targetStructure.Data.Bounds.y / 2f) : Vector3.zero;
        follower.transform.position = this.GetCirclePosition(this.target.transform.position + vector3, Mathf.Max(1.5f, (float) targetStructure.Data.Bounds.x / 2f), index, spawnedFollowers.Count);
        follower.State.facingAngle = Utils.GetAngle(follower.transform.position, targetStructure.Data.Position);
        follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
        double num1 = (double) follower.SetBodyAnimation((double) UnityEngine.Random.value < 0.5 ? "Riot/riot-destroy-structure" : "Riot/riot-destroy-structure2", true);
        if ((double) UnityEngine.Random.value < 0.20000000298023224)
        {
          double num2 = (double) follower.SetBodyAnimation("Riot/riot-purge-cheer" + UnityEngine.Random.Range(1, 4).ToString(), true);
        }
        else
          this.loops.Add(follower.PlayLoopedVO("event:/dialogue/followers/pitchfork_loop", follower.gameObject));
      }
    }
    GameManager.GetInstance().StartCoroutine(this.ShakeTargetStructure());
    yield return (object) new WaitForSeconds(3f);
    BiomeConstants.Instance.EmitSmokeInteractionVFX(this.target.transform.position, Vector3.one * (float) targetStructure.Data.Bounds.x);
    targetStructure.Collapse();
    AudioManager.Instance.PlayOneShot("event:/material/building_collapse", ChurchFollowerManager.Instance.RitualCenterPosition.position);
    NotificationCentre.NotificationsEnabled = true;
    NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("Notifications/StructureCollapsed", $"<color=#FFD201>{targetStructure.Data.GetLocalizedName()}</color>");
    NotificationCentre.NotificationsEnabled = false;
    yield return (object) new WaitForSeconds(0.5f);
    for (int index = 0; index < spawnedFollowers.Count; ++index)
    {
      double num = (double) spawnedFollowers[index].Follower.SetBodyAnimation("cheer", true);
    }
    yield return (object) new WaitForSeconds(2f);
    for (int index = this.loops.Count - 1; index >= 0; --index)
      AudioManager.Instance.StopLoop(this.loops[index]);
    this.loops.Clear();
  }

  public IEnumerator ShakeTargetStructure()
  {
    RitualPurge ritualPurge = this;
    float time = 0.0f;
    float timeBetween = 0.0f;
    while ((double) time < 3.0)
    {
      time += Time.deltaTime;
      timeBetween += Time.deltaTime;
      if ((double) timeBetween > 0.05000000074505806)
      {
        timeBetween = 0.0f;
        if ((double) UnityEngine.Random.value < 0.699999988079071)
          AudioManager.Instance.PlayOneShot("event:/material/wood_barrel_impact", ChurchFollowerManager.Instance.RitualCenterPosition.position);
      }
      if (ritualPurge.canAnimate)
      {
        ritualPurge.target.transform.DOKill();
        ritualPurge.target.transform.localScale = new Vector3((float) ritualPurge.target.Brain.Data.Direction, 1f, 1f);
        ritualPurge.target.transform.rotation = Quaternion.identity;
        ritualPurge.target.transform.DOPunchRotation((Vector3) (UnityEngine.Random.insideUnitCircle * (float) UnityEngine.Random.Range(2, 10)), 1f).SetEase<Tweener>(Ease.InOutBounce);
        ritualPurge.target.transform.DOPunchScale(ritualPurge.target.transform.localScale * UnityEngine.Random.Range(0.025f, 0.05f), 0.25f).SetEase<Tweener>(Ease.InOutBounce);
        ritualPurge.canAnimate = false;
        GameManager.GetInstance().WaitForSeconds(UnityEngine.Random.Range(1f, 2f), new System.Action(ritualPurge.\u003CShakeTargetStructure\u003Eb__12_0));
      }
      yield return (object) null;
    }
  }

  public IEnumerator ShowFollowerCrying(FollowerManager.SpawnedFollower targetFollower)
  {
    targetFollower.FollowerBrain.AddTrait(FollowerTrait.TraitType.Scared);
    targetFollower.Follower.transform.position = this.FindPosition();
    double num = (double) targetFollower.Follower.SetBodyAnimation("unconverted", true);
    targetFollower.Follower.gameObject.SetActive(true);
    List<WeedManager> weeds = new List<WeedManager>();
    for (int index = WeedManager.WeedManagers.Count - 1; index >= 0; --index)
    {
      if ((double) Vector3.Distance(WeedManager.WeedManagers[index].transform.position, targetFollower.Follower.transform.position) < 2.0)
      {
        weeds.Add(WeedManager.WeedManagers[index]);
        WeedManager.WeedManagers[index].gameObject.SetActive(false);
      }
    }
    GameManager.GetInstance().CamFollowTarget.ForceSnapTo(targetFollower.Follower.transform.position - GameManager.GetInstance().CamFollowTarget.transform.forward * 2f - Vector3.forward / 2f);
    targetFollower.Follower.PlayVO("event:/dialogue/followers/scared_very_long", ChurchFollowerManager.Instance.RitualCenterPosition.gameObject);
    yield return (object) new WaitForSeconds(1f);
    NotificationCentre.NotificationsEnabled = true;
    NotificationCentre.Instance.PlayFaithNotification("Notifications/GainedTrait", 0.0f, NotificationBase.Flair.None, targetFollower.FollowerBrain.Info.ID, targetFollower.FollowerBrain.Info.Name, FollowerTrait.GetLocalizedTitle(FollowerTrait.TraitType.Scared));
    NotificationCentre.NotificationsEnabled = false;
    yield return (object) new WaitForSeconds(3f);
    GameManager.GetInstance().WaitForSecondsRealtime(0.35f, (System.Action) (() =>
    {
      foreach (Component component in weeds)
        component.gameObject.SetActive(true);
    }));
  }

  public Vector3 FindPosition()
  {
    Vector3 a1 = new Vector3((float) UnityEngine.Random.Range(-20, 20), (float) UnityEngine.Random.Range(0, -25), 0.0f);
    List<StructureBrain> structureBrainList = new List<StructureBrain>((IEnumerable<StructureBrain>) StructureBrain.AllBrains);
    int num = 0;
    while (num++ < 99)
    {
      a1 = new Vector3((float) UnityEngine.Random.Range(-20, 20), (float) UnityEngine.Random.Range(0, -25), 0.0f);
      bool flag = false;
      foreach (StructureBrain structureBrain in structureBrainList)
      {
        float a2 = Mathf.Sqrt(Mathf.Pow((float) structureBrain.Data.Bounds.x, 2f) + Mathf.Pow((float) structureBrain.Data.Bounds.y, 2f)) * 0.5f;
        if ((double) Vector3.Distance(a1, structureBrain.Data.Position) < (double) Mathf.Max(a2, 3f))
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        break;
    }
    return a1;
  }

  public Vector3 GetCirclePosition(Vector3 center, float maxDistance, int index, int count)
  {
    if (count <= 12)
    {
      float num = maxDistance;
      float f = (float) ((double) index * (360.0 / (double) count) * (Math.PI / 180.0));
      return center + new Vector3(num * Mathf.Cos(f), num * Mathf.Sin(f));
    }
    int b = 8;
    float num1;
    float f1;
    if (index < b)
    {
      num1 = maxDistance;
      f1 = (float) ((double) index * (360.0 / (double) Mathf.Min(count, b)) * (Math.PI / 180.0));
    }
    else
    {
      num1 = maxDistance + 1f;
      f1 = (float) ((double) (index - b) * (360.0 / (double) (count - b)) * (Math.PI / 180.0));
    }
    return center + new Vector3(num1 * Mathf.Cos(f1), num1 * Mathf.Sin(f1));
  }

  public Structure GetStructure(StructureBrain target)
  {
    foreach (Structure structure in Structure.Structures)
    {
      if ((UnityEngine.Object) structure != (UnityEngine.Object) null && structure.Brain != null && structure.Brain.Data != null && target != null && target.Data != null && structure.Brain.Data.ID == target.Data.ID)
        return structure;
    }
    return (Structure) null;
  }

  public List<FollowerSelectEntry> GetFollowerSelectEntries()
  {
    List<FollowerSelectEntry> followerSelectEntries = new List<FollowerSelectEntry>();
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
      followerSelectEntries.Add(new FollowerSelectEntry(followerBrain, FollowerManager.GetFollowerAvailabilityStatus(followerBrain)));
    return followerSelectEntries;
  }

  public void EndRitual()
  {
    AudioManager.Instance.StopLoop(this.loopedSound);
    Interaction_TempleAltar.Instance.RitualLighting.gameObject.SetActive(false);
    ChurchFollowerManager.Instance.EndRitualOverlay();
    GameManager.GetInstance().CamFollowTarget.targetDistance = 11f;
    Interaction_TempleAltar.Instance.SimpleSetCamera.Reset();
  }

  public void OnDestroy()
  {
    AudioManager.Instance.StopLoop(this.fightLoop);
    for (int index = this.loops.Count - 1; index >= 0; --index)
      AudioManager.Instance.StopLoop(this.loops[index]);
    this.loops.Clear();
    int num = (int) this.music.stop(STOP_MODE.IMMEDIATE);
    if (!AudioManager.Instance.CurrentEventInstanceIsPlayingPath(this.music, "event:/Stings/purge_ritual_loop"))
      return;
    AudioManager.Instance.StopCurrentMusic();
  }

  [CompilerGenerated]
  public void \u003CShakeTargetStructure\u003Eb__12_0() => this.canAnimate = true;
}
