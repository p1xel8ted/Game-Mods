// Decompiled with JetBrains decompiler
// Type: RitualSnowman
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using MMTools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RitualSnowman : Ritual
{
  public Vector3 averagePosition;

  public override UpgradeSystem.Type RitualType => UpgradeSystem.Type.Ritual_Snowman;

  public override void Play()
  {
    base.Play();
    GameManager.GetInstance().StartCoroutine(this.RitualRoutine());
  }

  public IEnumerator RitualRoutine()
  {
    RitualSnowman ritualSnowman = this;
    AudioManager.Instance.PlayOneShot("event:/rituals/generic_start_ritual");
    PlayerFarming.Instance.GoToAndStop(ChurchFollowerManager.Instance.RitualCenterPosition.position, GoToCallback: (System.Action) (() =>
    {
      Interaction_TempleAltar.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      PlayerFarming.Instance.simpleSpineAnimator.Animate("idle", 0, true);
      PlayerFarming.Instance.state.transform.DOMove(ChurchFollowerManager.Instance.RitualCenterPosition.position, 0.1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    }));
    Interaction_TempleAltar.Instance.SimpleSetCamera.Play();
    yield return (object) ritualSnowman.StartCoroutine(ritualSnowman.WaitFollowersFormCircle());
    yield return (object) new WaitForSeconds(1f);
    AudioManager.Instance.PlayOneShot("event:/dlc/ritual/snowman_imbuelife_start", ritualSnowman.transform.position);
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
    yield return (object) new WaitForSeconds(1.2f);
    Interaction_TempleAltar.Instance.SimpleSetCamera.Reset();
    Interaction_TempleAltar.Instance.CloseUpCamera.Play();
    AudioManager.Instance.PlayOneShot("event:/dlc/ritual/snowman_imbuelife_snow_rise");
    ChurchFollowerManager.Instance.Snow.gameObject.SetActive(true);
    ChurchFollowerManager.Instance.Snow.transform.DOMoveZ(1f, 0.0f);
    ChurchFollowerManager.Instance.Snow.transform.DOMoveZ(0.0f, 1.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart);
    List<FollowerBrain> followerBrainList = new List<FollowerBrain>((IEnumerable<FollowerBrain>) Ritual.GetFollowersAvailableToAttendSermon());
    for (int index = 0; index < 2 && followerBrainList.Count != 0; ++index)
    {
      FollowerBrain followerBrain = followerBrainList[UnityEngine.Random.Range(0, followerBrainList.Count)];
      followerBrainList.Remove(followerBrain);
      Follower followerById = FollowerManager.FindFollowerByID(followerBrain.Info.ID);
      if ((bool) (UnityEngine.Object) followerById)
        ritualSnowman.StartCoroutine(ritualSnowman.MoveFollower(followerById, index));
    }
    foreach (FollowerBrain followerBrain in followerBrainList)
    {
      if (followerBrain.CurrentTask is FollowerTask_AttendRitual)
        ((FollowerTask_AttendRitual) followerBrain.CurrentTask).Pray2();
    }
    yield return (object) new WaitForSeconds(6f);
    bool waiting = true;
    MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 0.4f, "", (System.Action) (() => waiting = false));
    while (waiting)
      yield return (object) null;
    PlayerFarming.Instance.Spine.gameObject.SetActive(false);
    BaseLocationManager.Instance.Activatable = false;
    ChurchLocationManager.Instance.Activatable = false;
    BiomeConstants.Instance.DepthOfFieldTween(0.15f, 8.7f, 26f, 0.0f, 0.0f);
    BiomeBaseManager.Instance.ActivateRoom(false);
    Vector3 camPosition = GameManager.GetInstance().CamFollowTarget.transform.position;
    List<StructureBrain> snowmen = StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.SNOWMAN, newList: true);
    if (snowmen.Count > 0)
    {
      GameManager.GetInstance().CamFollowTarget.ResetTargetCamera(0.0f);
      yield return (object) new WaitForEndOfFrame();
      StructureBrain structureBrain = snowmen[UnityEngine.Random.Range(0, snowmen.Count)];
      GameManager.GetInstance().CamFollowTarget.ClearAllTargets();
      GameManager.GetInstance().CamFollowTarget.SnapTo(structureBrain.Data.Position);
      GameManager.GetInstance().CamFollowTarget.transform.localRotation = Quaternion.Euler(-45f, 0.0f, 0.0f);
    }
    ChurchFollowerManager.Instance.Sparkles.gameObject.SetActive(true);
    Vector3 SparklesStartPos = ChurchFollowerManager.Instance.Sparkles.transform.position;
    ChurchFollowerManager.Instance.Sparkles.transform.parent = BiomeConstants.Instance.transform;
    ChurchFollowerManager.Instance.Sparkles.transform.position = ritualSnowman.averagePosition;
    ChurchFollowerManager.Instance.Sparkles.Play();
    AudioManager.Instance.PlayOneShot("event:/player/Curses/charm_curse", AudioManager.Instance.Listener);
    yield return (object) new WaitForSeconds(1.5f);
    List<FollowerManager.SpawnedFollower> copyFollowers = new List<FollowerManager.SpawnedFollower>();
    List<FollowerInfo> snowmenFollowers = new List<FollowerInfo>();
    foreach (StructureBrain structureBrain in snowmen)
    {
      if (snowmenFollowers.Count < 4)
      {
        FollowerSpecialType followerSpecialType = structureBrain.Data.Level >= 9 ? FollowerSpecialType.Snowman_Great : (structureBrain.Data.Level < 5 ? FollowerSpecialType.Snowman_Bad : FollowerSpecialType.Snowman_Average);
        UnityEngine.Random.InitState(DataManager.Instance.FollowerID + 1);
        int num = UnityEngine.Random.Range(1, 4);
        string ForceSkin;
        switch (followerSpecialType)
        {
          case FollowerSpecialType.Snowman_Great:
            ForceSkin = $"Snowman/Good_{num}";
            break;
          case FollowerSpecialType.Snowman_Average:
            ForceSkin = $"Snowman/Mid_{num}";
            break;
          default:
            ForceSkin = $"Snowman/Bad_{num}";
            break;
        }
        FollowerInfo followerInfo = FollowerInfo.NewCharacter(FollowerLocation.Base, ForceSkin);
        followerInfo.Name = LocalizationManager.GetTranslation("NAMES/Snowman");
        followerInfo.IsSnowman = true;
        followerInfo.FollowerRole = FollowerRole.Worshipper;
        followerInfo.IsSnowman = true;
        followerInfo.Special = followerSpecialType;
        followerInfo.XPLevel = structureBrain.Data.Level;
        followerInfo.SkinColour = 0;
        followerInfo.TraitsSet = true;
        followerInfo.Traits.Clear();
        switch (followerSpecialType)
        {
          case FollowerSpecialType.Snowman_Great:
            followerInfo.Traits.Add(FollowerTrait.TraitType.MasterfulSnowman);
            break;
          case FollowerSpecialType.Snowman_Bad:
            followerInfo.Traits.Add(FollowerTrait.TraitType.ShoddySnowman);
            break;
        }
        FollowerManager.SpawnedFollower spawnedFollower = FollowerManager.SpawnCopyFollower(followerInfo, structureBrain.Data.Position, (Transform) null, FollowerLocation.Base);
        FollowerBrain.SetFollowerCostume(spawnedFollower.Follower.Spine.Skeleton, spawnedFollower.Follower.Brain._directInfoAccess, forceUpdate: true);
        copyFollowers.Add(spawnedFollower);
        snowmenFollowers.Add(followerInfo);
        BiomeConstants.Instance.EmitSmokeExplosionVFX(spawnedFollower.Follower.transform.position);
        spawnedFollower.Follower.Spine.AnimationState.SetAnimation(1, $"Reactions/react-determined{UnityEngine.Random.Range(1, 3)}", false);
        spawnedFollower.Follower.Spine.AnimationState.AddAnimation(1, "idle", true, 0.0f);
        AudioManager.Instance.PlayOneShot("event:/dlc/ritual/snowman_imbuelife_transform", spawnedFollower.Follower.transform.position);
      }
      structureBrain.Remove();
    }
    yield return (object) new WaitForSeconds(2.5f);
    waiting = true;
    MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 0.4f, "", (System.Action) (() => waiting = false));
    while (waiting)
      yield return (object) null;
    for (int index = copyFollowers.Count - 1; index >= 0; --index)
      FollowerManager.CleanUpCopyFollower(copyFollowers[index]);
    ChurchFollowerManager.Instance.Sparkles.transform.position = SparklesStartPos;
    ChurchFollowerManager.Instance.Sparkles.transform.parent = ChurchFollowerManager.Instance.transform;
    ChurchFollowerManager.Instance.Sparkles.gameObject.SetActive(false);
    GameManager.GetInstance().CamFollowTarget.ClearAllTargets();
    GameManager.GetInstance().CamFollowTarget.ForceSnapTo(camPosition);
    Interaction_TempleAltar.Instance.SimpleSetCamera.Play();
    BiomeConstants.Instance.DepthOfFieldTween(0.15f, 8.7f, 26f, 1f, 200f);
    BiomeBaseManager.Instance.ActivateChurch();
    PlayerFarming.Instance.Spine.gameObject.SetActive(true);
    BaseLocationManager.Instance.Activatable = true;
    ChurchLocationManager.Instance.Activatable = true;
    foreach (FollowerBrain followerBrain in Ritual.GetFollowersAvailableToAttendSermon())
    {
      if (followerBrain != null)
        FollowerManager.FindFollowerByID(followerBrain.Info.ID)?.HoodOn("pray", true);
    }
    List<Follower> newSnowmen = new List<Follower>();
    for (int index = 0; index < snowmenFollowers.Count; ++index)
    {
      snowmenFollowers[index].Location = FollowerLocation.Church;
      Follower newFollower = FollowerManager.CreateNewFollower(snowmenFollowers[index], ChurchFollowerManager.Instance.RitualCenterPosition.position);
      newFollower.transform.position = ritualSnowman.GetCirclePosition(newFollower, ChurchFollowerManager.Instance.RitualCenterPosition.position, index, snowmenFollowers.Count);
      newFollower.Brain.DesiredLocation = FollowerLocation.Church;
      newFollower.Brain.CurrentTask.Arrive();
      newFollower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
      newSnowmen.Add(newFollower);
    }
    yield return (object) new WaitForSeconds(0.5f);
    foreach (Follower follower in newSnowmen)
    {
      ChurchFollowerManager.Instance.AddBrainToAudience(follower.Brain);
      Ritual.FollowerToAttendSermon.Add(follower.Brain);
      follower.FacePosition(PlayerFarming.Instance.transform.position);
      double num = (double) follower.SetBodyAnimation("dance", true);
    }
    yield return (object) new WaitForSeconds(1f);
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      if (followerBrain.CurrentTaskType == FollowerTaskType.AttendTeaching)
        (followerBrain.CurrentTask as FollowerTask_AttendRitual).Dance();
    }
    yield return (object) new WaitForSeconds(2f);
    ChurchFollowerManager.Instance.Snow.transform.DOMoveZ(1f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InQuart);
    PlayerFarming.Instance.simpleSpineAnimator.Animate("rituals/ritual-stop", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      if (followerBrain != null && followerBrain.CurrentTask is FollowerTask_AttendRitual)
        (followerBrain.CurrentTask as FollowerTask_AttendRitual).Cheer();
    }
    yield return (object) new WaitForSeconds(0.5f);
    ChurchFollowerManager.Instance.Snow.gameObject.SetActive(false);
    BiomeConstants.Instance.ChromaticAbberationTween(1f, 1f, BiomeConstants.Instance.ChromaticAberrationDefaultValue);
    BiomeConstants.Instance.VignetteTween(1f, 0.7f, BiomeConstants.Instance.VignetteDefaultValue);
    ChurchFollowerManager.Instance.EndRitualOverlay();
    GameManager.GetInstance().CamFollowTarget.targetDistance = 11f;
    Interaction_TempleAltar.Instance.RitualLighting.gameObject.SetActive(false);
    foreach (Follower follower in newSnowmen)
      follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_AttendRitual());
    float num1 = 0.0f;
    foreach (FollowerBrain brain in Ritual.FollowerToAttendSermon)
    {
      if (brain != null)
      {
        float Delay = UnityEngine.Random.Range(0.1f, 0.5f);
        num1 += Delay;
        ritualSnowman.StartCoroutine(ritualSnowman.DelayFollowerReaction(brain, Delay, !brain._directInfoAccess.IsSnowman));
        brain.AddThought(Thought.Snowman_Ritual);
      }
    }
    yield return (object) new WaitForSeconds(1.5f);
    Interaction_TempleAltar.Instance.CloseUpCamera.Reset();
    ritualSnowman.CompleteRitual();
    yield return (object) new WaitForSeconds(1f);
  }

  public IEnumerator MoveFollower(Follower follower, int index)
  {
    RitualSnowman ritualSnowman = this;
    List<Vector3> positions = new List<Vector3>()
    {
      Vector3.left,
      Vector3.right
    };
    bool waiting = true;
    follower.HoodOff(onComplete: (System.Action) (() => waiting = false));
    while (waiting)
      yield return (object) null;
    yield return (object) ritualSnowman.StartCoroutine(follower.GoToRoutine(PlayerFarming.Instance.transform.position + positions[index]));
    follower.State.facingAngle = Utils.GetAngle(follower.transform.position, PlayerFarming.Instance.transform.position);
    double num = (double) follower.SetBodyAnimation("Farming/add-snow", true);
  }

  public Vector3 GetCirclePosition(Follower follower, Vector3 pos, int index, int count)
  {
    if (count <= 12)
    {
      float num = 1f;
      float f = (float) ((double) index * (360.0 / (double) count) * (Math.PI / 180.0));
      return pos + new Vector3(num * Mathf.Cos(f), num * Mathf.Sin(f));
    }
    int b = 8;
    float num1;
    float f1;
    if (index < b)
    {
      num1 = 1f;
      f1 = (float) ((double) index * (360.0 / (double) Mathf.Min(count, b)) * (Math.PI / 180.0));
    }
    else
    {
      num1 = 2f;
      f1 = (float) ((double) (index - b) * (360.0 / (double) (count - b)) * (Math.PI / 180.0));
    }
    return pos + new Vector3(num1 * Mathf.Cos(f1), num1 * Mathf.Sin(f1));
  }
}
