// Decompiled with JetBrains decompiler
// Type: RitualFast
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RitualFast : Ritual
{
  public static System.Action OnRitualFastingBegan;
  public Vector3 SermonEffectStartingPos;

  public override UpgradeSystem.Type RitualType => UpgradeSystem.Type.Ritual_Fast;

  public override void Play()
  {
    base.Play();
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.RitualRoutine());
  }

  public IEnumerator RitualRoutine()
  {
    RitualFast ritualFast = this;
    AudioManager.Instance.PlayOneShot("event:/rituals/generic_start_ritual");
    PlayerFarming.Instance.GoToAndStop(ChurchFollowerManager.Instance.RitualCenterPosition.position, GoToCallback: (System.Action) (() =>
    {
      Interaction_TempleAltar.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      PlayerFarming.Instance.simpleSpineAnimator.Animate("idle", 0, true);
      PlayerFarming.Instance.state.transform.DOMove(ChurchFollowerManager.Instance.RitualCenterPosition.position, 0.1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    }));
    Interaction_TempleAltar.Instance.SimpleSetCamera.Play();
    yield return (object) ritualFast.StartCoroutine((IEnumerator) ritualFast.WaitFollowersFormCircle());
    PlayerFarming.Instance.simpleSpineAnimator.Animate("build", 0, true);
    PlayerFarming.Instance.Spine.skeleton.FindBone("ritualring").Rotation += 60f;
    PlayerFarming.Instance.Spine.skeleton.UpdateWorldTransform();
    PlayerFarming.Instance.Spine.skeleton.Update(Time.deltaTime);
    PlayerFarming.Instance.simpleSpineAnimator.Animate("rituals/ritual-start", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("rituals/ritual-loop", 0, true, 0.0f);
    ChurchFollowerManager.Instance.SermonEffect.gameObject.SetActive(true);
    SkeletonAnimation sermonEffect = ChurchFollowerManager.Instance.SermonEffect;
    sermonEffect.AnimationState.SetAnimation(0, "5-in", false);
    sermonEffect.AnimationState.AddAnimation(0, "5-loop", true, 0.0f);
    ritualFast.SermonEffectStartingPos = sermonEffect.transform.position;
    sermonEffect.transform.position = PlayerFarming.Instance.transform.position;
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
        ritualFast.StartCoroutine((IEnumerator) ritualFast.MoveFollower(followerById, index));
    }
    foreach (FollowerBrain followerBrain in followerBrainList)
    {
      if (followerBrain.CurrentTask is FollowerTask_AttendRitual)
        ((FollowerTask_AttendRitual) followerBrain.CurrentTask).Pray();
    }
    yield return (object) new WaitForSeconds(7f);
    sermonEffect.AnimationState.AddAnimation(0, "5-out", false, 0.0f);
    PlayerFarming.Instance.simpleSpineAnimator.Animate("rituals/ritual-stop", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    sermonEffect.transform.position = ritualFast.SermonEffectStartingPos;
    sermonEffect.gameObject.SetActive(false);
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
      ritualFast.StartCoroutine((IEnumerator) ritualFast.DelayFollowerReaction(brain, Delay));
    }
    yield return (object) new WaitForSeconds(1.5f);
    Interaction_TempleAltar.Instance.CloseUpCamera.Reset();
    DataManager.Instance.LastFastDeclared = TimeManager.TotalElapsedGameTime;
    System.Action ritualFastingBegan = RitualFast.OnRitualFastingBegan;
    if (ritualFastingBegan != null)
      ritualFastingBegan();
    ritualFast.CompleteRitual();
    yield return (object) new WaitForSeconds(1f);
    CultFaithManager.AddThought(Thought.Cult_Fast);
  }

  public IEnumerator MoveFollower(Follower follower, int index)
  {
    RitualFast ritualFast = this;
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
    yield return (object) ritualFast.StartCoroutine((IEnumerator) follower.GoToRoutine(ChurchFollowerManager.Instance.RitualCenterPosition.position + positions[index]));
    double num = (double) follower.SetBodyAnimation("Hungry/get-hungry", false);
    follower.AddBodyAnimation("Hungry/idle-hungry", true, 0.0f);
  }
}
