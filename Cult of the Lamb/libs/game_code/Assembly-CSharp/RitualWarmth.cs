// Decompiled with JetBrains decompiler
// Type: RitualWarmth
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using MMTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RitualWarmth : Ritual
{
  public static System.Action OnRitualWarmthBegan;
  public Vector3 SermonEffectStartingPos;

  public override UpgradeSystem.Type RitualType => UpgradeSystem.Type.Ritual_Warmth;

  public override void Play()
  {
    base.Play();
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.RitualRoutine());
  }

  public IEnumerator RitualRoutine()
  {
    RitualWarmth ritualWarmth = this;
    AudioManager.Instance.PlayOneShot("event:/rituals/generic_start_ritual");
    PlayerFarming.Instance.GoToAndStop(ChurchFollowerManager.Instance.RitualCenterPosition.position, GoToCallback: (System.Action) (() =>
    {
      Interaction_TempleAltar.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      PlayerFarming.Instance.simpleSpineAnimator.Animate("idle", 0, true);
      PlayerFarming.Instance.state.transform.DOMove(ChurchFollowerManager.Instance.RitualCenterPosition.position, 0.1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    }));
    Interaction_TempleAltar.Instance.SimpleSetCamera.Play();
    yield return (object) ritualWarmth.StartCoroutine((IEnumerator) ritualWarmth.WaitFollowersFormCircle());
    PlayerFarming.Instance.simpleSpineAnimator.Animate("build", 0, true);
    PlayerFarming.Instance.Spine.skeleton.FindBone("ritualring").Rotation += 60f;
    PlayerFarming.Instance.Spine.skeleton.UpdateWorldTransform();
    PlayerFarming.Instance.Spine.skeleton.Update(Time.deltaTime);
    PlayerFarming.Instance.simpleSpineAnimator.Animate("rituals/ritual-start", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("rituals/ritual-loop", 0, true, 0.0f);
    Interaction_TempleAltar.Instance.RitualLighting.gameObject.SetActive(true);
    ChurchFollowerManager.Instance.StartRitualOverlay();
    AudioManager.Instance.SetFollowersDance(1f);
    BiomeConstants.Instance.ChromaticAbberationTween(2f, BiomeConstants.Instance.ChromaticAberrationDefaultValue, 1f);
    BiomeConstants.Instance.VignetteTween(2f, BiomeConstants.Instance.VignetteDefaultValue, 0.7f);
    DOTween.To((DOGetter<float>) (() => GameManager.GetInstance().CamFollowTarget.targetDistance), (DOSetter<float>) (x => GameManager.GetInstance().CamFollowTarget.targetDistance = x), 6.5f, 1f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutSine);
    yield return (object) new WaitForSeconds(1.2f);
    Interaction_TempleAltar.Instance.SimpleSetCamera.Reset();
    Interaction_TempleAltar.Instance.CloseUpCamera.Play();
    List<FollowerBrain> followerBrainList = new List<FollowerBrain>((IEnumerable<FollowerBrain>) Ritual.GetFollowersAvailableToAttendSermon());
    for (int index = 0; index < 4 && followerBrainList.Count != 0; ++index)
    {
      FollowerBrain followerBrain = followerBrainList[UnityEngine.Random.Range(0, followerBrainList.Count)];
      followerBrainList.Remove(followerBrain);
      Follower followerById = FollowerManager.FindFollowerByID(followerBrain.Info.ID);
      if ((bool) (UnityEngine.Object) followerById)
        ritualWarmth.StartCoroutine((IEnumerator) ritualWarmth.MoveFollower(followerById, index));
    }
    foreach (FollowerBrain followerBrain in followerBrainList)
    {
      if (followerBrain.CurrentTask is FollowerTask_AttendRitual)
        ((FollowerTask_AttendRitual) followerBrain.CurrentTask).Pray();
    }
    yield return (object) new WaitForSeconds(7f);
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
    BiomeBaseManager.Instance.ActivateDoorRoom();
    DoorRoomLocationManager.Instance.SkyAnimator.SetTrigger("TriggerAuroraBorealis");
    Vector3 camPosition = GameManager.GetInstance().CamFollowTarget.transform.position;
    GameManager.GetInstance().CamFollowTarget.ResetTargetCamera(0.0f);
    yield return (object) new WaitForEndOfFrame();
    DoorRoomLocationManager.Instance.SkyAnimator.SetBool("AuroraBorealis", true);
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
    AudioManager.Instance.SetFollowersDance(0.0f);
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      if (followerBrain.CurrentTask is FollowerTask_AttendRitual)
        (followerBrain.CurrentTask as FollowerTask_AttendRitual).Boo();
    }
    yield return (object) new WaitForSeconds(0.5f);
    BiomeConstants.Instance.ChromaticAbberationTween(1f, 1f, BiomeConstants.Instance.ChromaticAberrationDefaultValue);
    BiomeConstants.Instance.VignetteTween(1f, 0.7f, BiomeConstants.Instance.VignetteDefaultValue);
    ChurchFollowerManager.Instance.EndRitualOverlay();
    GameManager.GetInstance().CamFollowTarget.targetDistance = 11f;
    Interaction_TempleAltar.Instance.RitualLighting.gameObject.SetActive(false);
    float num = 0.0f;
    foreach (FollowerBrain brain in Ritual.FollowerToAttendSermon)
    {
      float Delay = UnityEngine.Random.Range(0.1f, 0.5f);
      num += Delay;
      ritualWarmth.StartCoroutine((IEnumerator) ritualWarmth.DelayFollowerReaction(brain, Delay));
    }
    yield return (object) new WaitForSeconds(1.5f);
    Interaction_TempleAltar.Instance.CloseUpCamera.Reset();
    System.Action ritualWarmthBegan = RitualWarmth.OnRitualWarmthBegan;
    if (ritualWarmthBegan != null)
      ritualWarmthBegan();
    ritualWarmth.CompleteRitual();
    yield return (object) new WaitForSeconds(1f);
    CultFaithManager.AddThought(Thought.Cult_WarmthRitual);
    WarmthBar.ForceCountUpdate();
    DataManager.Instance.LastWarmthRitualDeclared = TimeManager.TotalElapsedGameTime;
  }

  public IEnumerator MoveFollower(Follower follower, int index)
  {
    RitualWarmth ritualWarmth = this;
    List<Vector3> positions = new List<Vector3>()
    {
      Vector3.left,
      Vector3.up,
      Vector3.right,
      Vector3.down
    };
    bool waiting = true;
    follower.HoodOff(onComplete: (System.Action) (() => waiting = false));
    while (waiting)
      yield return (object) null;
    yield return (object) ritualWarmth.StartCoroutine((IEnumerator) follower.GoToRoutine(ChurchFollowerManager.Instance.RitualCenterPosition.position + positions[index]));
    follower.FacePosition(PlayerFarming.Instance.transform.position);
    double num = (double) follower.SetBodyAnimation("Snow/shuffle", true);
  }
}
