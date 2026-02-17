// Decompiled with JetBrains decompiler
// Type: RitualHalloween
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using MMTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RitualHalloween : Ritual
{
  public static System.Action OnHalloweenRitualBegan;

  public override UpgradeSystem.Type RitualType => UpgradeSystem.Type.Ritual_Halloween;

  public override void Play()
  {
    base.Play();
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.RitualRoutine());
  }

  public IEnumerator RitualRoutine()
  {
    RitualHalloween ritualHalloween = this;
    DataManager.Instance.LastHalloween = TimeManager.TotalElapsedGameTime;
    AudioManager.Instance.PlayOneShot("event:/rituals/generic_start_ritual");
    Interaction_TempleAltar.Instance.SimpleSetCamera.Play();
    PlayerFarming.Instance.GoToAndStop(ChurchFollowerManager.Instance.RitualCenterPosition.position, GoToCallback: (System.Action) (() =>
    {
      Interaction_TempleAltar.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      PlayerFarming.Instance.simpleSpineAnimator.Animate("idle", 0, true);
      PlayerFarming.Instance.state.transform.DOMove(ChurchFollowerManager.Instance.RitualCenterPosition.position, 0.1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    }));
    yield return (object) ritualHalloween.StartCoroutine((IEnumerator) ritualHalloween.WaitFollowersFormCircle());
    yield return (object) new WaitForSeconds(1f);
    PlayerFarming.Instance.Spine.skeleton.FindBone("ritualring").Rotation += 60f;
    PlayerFarming.Instance.Spine.skeleton.UpdateWorldTransform();
    PlayerFarming.Instance.Spine.skeleton.Update(Time.deltaTime);
    PlayerFarming.Instance.simpleSpineAnimator.Animate("rituals/ritual-start", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("rituals/ritual-loop", 0, true, 0.0f);
    Interaction_TempleAltar.Instance.RitualLighting.gameObject.SetActive(true);
    ChurchFollowerManager.Instance.StartRitualOverlay();
    BiomeConstants.Instance.ChromaticAbberationTween(2f, BiomeConstants.Instance.ChromaticAberrationDefaultValue, 1f);
    BiomeConstants.Instance.VignetteTween(2f, BiomeConstants.Instance.VignetteDefaultValue, 0.7f);
    Interaction_TempleAltar.Instance.SimpleSetCamera.Reset();
    Interaction_TempleAltar.Instance.CloseUpCamera.Play();
    yield return (object) new WaitForSeconds(1.5f);
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      if (followerBrain.CurrentTask is FollowerTask_AttendRitual)
        ((FollowerTask_AttendRitual) followerBrain.CurrentTask).Pray2();
    }
    Vector3 targetCameraPosition = GameManager.GetInstance().CamFollowTarget.CurrentTargetCameraPosition;
    Vector3 normalized = (ChurchFollowerManager.Instance.RitualCenterPosition.position - targetCameraPosition).normalized;
    DOTween.To((DOGetter<Vector3>) (() => GameManager.GetInstance().CamFollowTarget.CurrentTargetCameraPosition), (DOSetter<Vector3>) (x => GameManager.GetInstance().CamFollowTarget.CurrentTargetCameraPosition = x), GameManager.GetInstance().CamFollowTarget.CurrentTargetCameraPosition + normalized, 2.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
    List<FollowerBrain> followerBrainList = new List<FollowerBrain>((IEnumerable<FollowerBrain>) Ritual.GetFollowersAvailableToAttendSermon());
    for (int index = 0; index < 2 && followerBrainList.Count != 0; ++index)
    {
      FollowerBrain followerBrain = followerBrainList[UnityEngine.Random.Range(0, followerBrainList.Count)];
      followerBrainList.Remove(followerBrain);
      Follower followerById = FollowerManager.FindFollowerByID(followerBrain.Info.ID);
      if ((bool) (UnityEngine.Object) followerById)
        ritualHalloween.StartCoroutine((IEnumerator) ritualHalloween.DanceFolower(followerById, index));
    }
    yield return (object) new WaitForSeconds(2.5f);
    AudioManager.Instance.PlayOneShot("event:/rituals/consume_follower");
    yield return (object) new WaitForSeconds(1.5f);
    AudioManager.Instance.PlayOneShot("event:/rituals/funeral_ghost");
    yield return (object) new WaitForSeconds(1.5f);
    bool waiting = true;
    MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 0.4f, "", (System.Action) (() => waiting = false));
    while (waiting)
      yield return (object) null;
    ChurchFollowerManager.Instance.DisableAllOverlays();
    PlayerFarming.Instance.Spine.gameObject.SetActive(false);
    DoorRoomLocationManager.Instance.Activatable = false;
    BaseLocationManager.Instance.Activatable = false;
    ChurchLocationManager.Instance.Activatable = false;
    BiomeConstants.Instance.DepthOfFieldTween(0.15f, 8.7f, 26f, 0.0f, 0.0f);
    BiomeBaseManager.Instance.DoorRoom.regenerateNavOnEnable = false;
    BiomeBaseManager.Instance.ActivateDoorRoom();
    DoorRoomLocationManager.Instance.SkyAnimator.SetTrigger("TriggerBloodMoon");
    Vector3 camPosition = GameManager.GetInstance().CamFollowTarget.transform.position;
    GameManager.GetInstance().CamFollowTarget.ResetTargetCamera(0.0f);
    yield return (object) new WaitForEndOfFrame();
    DoorRoomLocationManager.Instance.SkyAnimator.SetBool("BloodMoon", true);
    GameManager.GetInstance().CamFollowTarget.ClearAllTargets();
    GameManager.GetInstance().CamFollowTarget.ForceSnapTo(new Vector3(-0.1999771f, 36.77f, -16.67f));
    GameManager.GetInstance().CamFollowTarget.transform.localRotation = Quaternion.Euler(-69.138f, 0.0f, 0.0f);
    yield return (object) new WaitForSeconds(5.5f);
    waiting = true;
    MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 0.4f, "", (System.Action) (() => waiting = false));
    while (waiting)
      yield return (object) null;
    BiomeConstants.Instance.DepthOfFieldTween(0.15f, 8.7f, 26f, 1f, 200f);
    GameManager.GetInstance().CamFollowTarget.targetDistance = 11f;
    GameManager.GetInstance().CamFollowTarget.ClearAllTargets();
    GameManager.GetInstance().CamFollowTarget.ForceSnapTo(camPosition);
    BiomeBaseManager.Instance.DoorRoom.regenerateNavOnEnable = true;
    BiomeBaseManager.Instance.ActivateChurch();
    PlayerFarming.Instance.Spine.gameObject.SetActive(true);
    BaseLocationManager.Instance.Activatable = true;
    ChurchLocationManager.Instance.Activatable = true;
    DoorRoomLocationManager.Instance.Activatable = true;
    Interaction_TempleAltar.Instance.CloseUpCamera.Reset();
    Interaction_TempleAltar.Instance.SimpleSetCamera.Play();
    yield return (object) new WaitForSeconds(1f);
    PlayerFarming.Instance.simpleSpineAnimator.Animate("rituals/ritual-stop", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      if (followerBrain.CurrentTask is FollowerTask_AttendRitual)
        (followerBrain.CurrentTask as FollowerTask_AttendRitual).Cheer();
    }
    yield return (object) new WaitForSeconds(1f);
    BiomeConstants.Instance.ChromaticAbberationTween(1f, 1f, BiomeConstants.Instance.ChromaticAberrationDefaultValue);
    BiomeConstants.Instance.VignetteTween(1f, 0.7f, BiomeConstants.Instance.VignetteDefaultValue);
    BaseLocationManager.Instance.EnableBloodMoon();
    ChurchFollowerManager.Instance.EndRitualOverlay();
    GameManager.GetInstance().CamFollowTarget.targetDistance = 11f;
    Interaction_TempleAltar.Instance.RitualLighting.gameObject.SetActive(false);
    float num = 0.0f;
    foreach (FollowerBrain brain in Ritual.FollowerToAttendSermon)
    {
      float Delay = UnityEngine.Random.Range(0.1f, 0.5f);
      num += Delay;
      ritualHalloween.StartCoroutine((IEnumerator) ritualHalloween.DelayFollowerReaction(brain, Delay));
    }
    yield return (object) new WaitForSeconds(1.5f);
    ritualHalloween.CompleteRitual();
    Interaction_TempleAltar.Instance.SimpleSetCamera.Reset();
    System.Action halloweenRitualBegan = RitualHalloween.OnHalloweenRitualBegan;
    if (halloweenRitualBegan != null)
      halloweenRitualBegan();
    yield return (object) new WaitForSeconds(1f);
  }

  public IEnumerator MoveFollower(Follower follower, int index, List<Vector3> positions)
  {
    for (int i = 0; i < 4; ++i)
    {
      ++index;
      if (index > positions.Count - 1)
        index = 0;
      bool waiting = true;
      follower.transform.DOMoveX((PlayerFarming.Instance.transform.position + positions[index]).x, 2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => waiting = false));
      follower.transform.DOMoveY((PlayerFarming.Instance.transform.position + positions[index]).y, 2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear);
      while (waiting)
        yield return (object) null;
    }
  }

  public IEnumerator DanceFolower(Follower follower, int index)
  {
    RitualHalloween ritualHalloween = this;
    List<Vector3> positions = new List<Vector3>()
    {
      Vector3.left,
      Vector3.right
    };
    bool waiting = true;
    follower.HoodOff(onComplete: (System.Action) (() => waiting = false));
    while (waiting)
      yield return (object) null;
    yield return (object) ritualHalloween.StartCoroutine((IEnumerator) follower.GoToRoutine(PlayerFarming.Instance.transform.position + positions[index] * 0.75f));
    follower.State.facingAngle = Utils.GetAngle(follower.transform.position, PlayerFarming.Instance.transform.position);
    double num = (double) follower.SetBodyAnimation("Sermons/bloodmoon", true);
  }
}
