// Decompiled with JetBrains decompiler
// Type: RitualWorkThroughNight
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RitualWorkThroughNight : Ritual
{
  public static System.Action OnWorkThroughNightBegan;

  public override UpgradeSystem.Type RitualType => UpgradeSystem.Type.Ritual_WorkThroughNight;

  public override void Play()
  {
    base.Play();
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.RitualRoutine());
  }

  public IEnumerator RitualRoutine()
  {
    RitualWorkThroughNight workThroughNight = this;
    AudioManager.Instance.PlayOneShot("event:/rituals/generic_start_ritual");
    PlayerFarming.Instance.GoToAndStop(ChurchFollowerManager.Instance.RitualCenterPosition.position, GoToCallback: (System.Action) (() =>
    {
      Interaction_TempleAltar.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      PlayerFarming.Instance.simpleSpineAnimator.Animate("idle", 0, true);
      PlayerFarming.Instance.state.transform.DOMove(ChurchFollowerManager.Instance.RitualCenterPosition.position, 0.1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    }));
    Interaction_TempleAltar.Instance.SimpleSetCamera.Play();
    yield return (object) workThroughNight.StartCoroutine((IEnumerator) workThroughNight.WaitFollowersFormCircle());
    yield return (object) new WaitForSeconds(1f);
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
    List<FollowerBrain> followerBrainList = new List<FollowerBrain>((IEnumerable<FollowerBrain>) Ritual.GetFollowersAvailableToAttendSermon());
    for (int index = 0; index < 4 && followerBrainList.Count != 0; ++index)
    {
      FollowerBrain followerBrain = followerBrainList[UnityEngine.Random.Range(0, followerBrainList.Count)];
      followerBrainList.Remove(followerBrain);
      Follower followerById = FollowerManager.FindFollowerByID(followerBrain.Info.ID);
      if ((bool) (UnityEngine.Object) followerById)
        workThroughNight.StartCoroutine((IEnumerator) workThroughNight.MoveFollower(followerById, index));
    }
    foreach (FollowerBrain followerBrain in followerBrainList)
    {
      if (followerBrain.CurrentTask is FollowerTask_AttendRitual)
        (followerBrain.CurrentTask as FollowerTask_AttendRitual).Cheer();
    }
    yield return (object) new WaitForSeconds(7f);
    PlayerFarming.Instance.simpleSpineAnimator.Animate("rituals/ritual-stop", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      if (followerBrain.CurrentTask is FollowerTask_AttendRitual)
        (followerBrain.CurrentTask as FollowerTask_AttendRitual).Cheer();
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
      workThroughNight.StartCoroutine((IEnumerator) workThroughNight.DelayFollowerReaction(brain, Delay));
      brain.Stats.Rest = 100f;
      brain.AddThought(Thought.WorkThroughNight);
      Follower followerById = FollowerManager.FindFollowerByID(brain.Info.ID);
      if ((UnityEngine.Object) followerById != (UnityEngine.Object) null)
        followerById.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(workThroughNight.HandleAnimationStateEvent);
    }
    yield return (object) new WaitForSeconds(1.5f);
    Interaction_TempleAltar.Instance.CloseUpCamera.Reset();
    DataManager.Instance.LastWorkThroughTheNight = TimeManager.TotalElapsedGameTime;
    System.Action throughNightBegan = RitualWorkThroughNight.OnWorkThroughNightBegan;
    if (throughNightBegan != null)
      throughNightBegan();
    workThroughNight.CompleteRitual();
    yield return (object) new WaitForSeconds(1f);
    CultFaithManager.AddThought(Thought.Cult_WorkThroughNight);
  }

  public IEnumerator MoveFollower(Follower follower, int index)
  {
    RitualWorkThroughNight workThroughNight = this;
    List<Vector3> positions = new List<Vector3>()
    {
      Vector3.left,
      Vector3.up,
      Vector3.right,
      Vector3.down
    };
    string[] anims = new string[3]
    {
      "build",
      "dig",
      "mining"
    };
    bool waiting = true;
    follower.HoodOff(onComplete: (System.Action) (() => waiting = false));
    while (waiting)
      yield return (object) null;
    yield return (object) workThroughNight.StartCoroutine((IEnumerator) follower.GoToRoutine(ChurchFollowerManager.Instance.RitualCenterPosition.position + positions[index]));
    double num = (double) follower.SetBodyAnimation(anims[(int) Utils.Repeat((float) index, 3f)], true);
    follower.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(workThroughNight.HandleAnimationStateEvent);
  }

  public void HandleAnimationStateEvent(TrackEntry trackEntry, Spine.Event e)
  {
    string name = e.Data.Name;
    if (!(name == "Chop"))
    {
      int num = name == "Build" ? 1 : 0;
    }
    else
      AudioManager.Instance.PlayOneShot(SoundConstants.GetImpactSoundPathForMaterial(SoundConstants.SoundMaterial.Stone), PlayerFarming.Instance.transform.position);
  }
}
