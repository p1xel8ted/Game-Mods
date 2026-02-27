// Decompiled with JetBrains decompiler
// Type: RitualElightenment
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RitualElightenment : Ritual
{
  public static System.Action OnEnlightenmentRitualBegan;

  protected override UpgradeSystem.Type RitualType => UpgradeSystem.Type.Ritual_Enlightenment;

  public override void Play()
  {
    base.Play();
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.RitualRoutine());
  }

  private IEnumerator RitualRoutine()
  {
    RitualElightenment ritualElightenment = this;
    AudioManager.Instance.PlayOneShot("event:/rituals/generic_start_ritual");
    Interaction_TempleAltar.Instance.SimpleSetCamera.Play();
    PlayerFarming.Instance.GoToAndStop(ChurchFollowerManager.Instance.RitualCenterPosition.position, GoToCallback: (System.Action) (() =>
    {
      Interaction_TempleAltar.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      PlayerFarming.Instance.simpleSpineAnimator.Animate("idle", 0, true);
      PlayerFarming.Instance.state.transform.DOMove(ChurchFollowerManager.Instance.RitualCenterPosition.position, 0.1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    }));
    yield return (object) ritualElightenment.StartCoroutine((IEnumerator) ritualElightenment.WaitFollowersFormCircle());
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
    Vector3 t = GameManager.GetInstance().CamFollowTarget.CurrentTargetCameraPosition;
    Vector3 normalized = (ChurchFollowerManager.Instance.RitualCenterPosition.position - t).normalized;
    DOTween.To((DOGetter<Vector3>) (() => GameManager.GetInstance().CamFollowTarget.CurrentTargetCameraPosition), (DOSetter<Vector3>) (x => GameManager.GetInstance().CamFollowTarget.CurrentTargetCameraPosition = x), GameManager.GetInstance().CamFollowTarget.CurrentTargetCameraPosition + normalized, 2.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
    yield return (object) new WaitForSeconds(2.5f);
    DOTween.To((DOGetter<Vector3>) (() => GameManager.GetInstance().CamFollowTarget.CurrentTargetCameraPosition), (DOSetter<Vector3>) (x => GameManager.GetInstance().CamFollowTarget.CurrentTargetCameraPosition = x), t, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    AudioManager.Instance.PlayOneShot("event:/rituals/enlightenment_beam", PlayerFarming.Instance.gameObject);
    ChurchFollowerManager.Instance.Light.SetActive(true);
    ChurchFollowerManager.Instance.Light.transform.DOScaleX(1.75f, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack);
    ChurchFollowerManager.Instance.Light.transform.DOScaleY(1.75f, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack);
    Interaction_TempleAltar.Instance.PulseDisplacementObject(PlayerFarming.Instance.CameraBone.transform.position);
    yield return (object) new WaitForSeconds(4.25f);
    ChurchFollowerManager.Instance.Light.SetActive(true);
    ChurchFollowerManager.Instance.Light.transform.DOScaleX(0.0f, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack);
    ChurchFollowerManager.Instance.Light.transform.DOScaleY(0.0f, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack);
    yield return (object) new WaitForSeconds(0.5f);
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
    ChurchFollowerManager.Instance.EndRitualOverlay();
    GameManager.GetInstance().CamFollowTarget.targetDistance = 11f;
    Interaction_TempleAltar.Instance.RitualLighting.gameObject.SetActive(false);
    float num = 0.0f;
    foreach (FollowerBrain brain in Ritual.FollowerToAttendSermon)
    {
      float Delay = UnityEngine.Random.Range(0.1f, 0.5f);
      num += Delay;
      ritualElightenment.StartCoroutine((IEnumerator) ritualElightenment.DelayFollowerReaction(brain, Delay));
      brain.AddThought(Thought.EnlightenmentRitual);
    }
    Interaction_TempleAltar.Instance.CloseUpCamera.Reset();
    yield return (object) new WaitForSeconds(1.5f);
    DataManager.Instance.LastEnlightenment = TimeManager.TotalElapsedGameTime;
    System.Action enlightenmentRitualBegan = RitualElightenment.OnEnlightenmentRitualBegan;
    if (enlightenmentRitualBegan != null)
      enlightenmentRitualBegan();
    ritualElightenment.CompleteRitual();
    yield return (object) new WaitForSeconds(1f);
    CultFaithManager.AddThought(Thought.Cult_Enlightenment);
  }

  private IEnumerator MoveFollower(Follower follower, int index)
  {
    RitualElightenment ritualElightenment = this;
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
    yield return (object) ritualElightenment.StartCoroutine((IEnumerator) follower.GoToRoutine(ChurchFollowerManager.Instance.RitualCenterPosition.position + positions[index]));
    double num = (double) follower.SetBodyAnimation("dance-hooded", true);
  }
}
