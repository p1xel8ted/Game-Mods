// Decompiled with JetBrains decompiler
// Type: RitualMidwinter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Spine.Unity.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RitualMidwinter : Ritual
{
  public string customGiftOpenEvent = "event:/dlc/ritual/midwinter_celebration_gift_open";
  public string defaultGiftOpenEvent = "event:/Stings/gamble_win";

  public override UpgradeSystem.Type RitualType => UpgradeSystem.Type.Ritual_Midwinter;

  public override void Play()
  {
    base.Play();
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.RitualRoutine());
  }

  public IEnumerator RitualRoutine()
  {
    RitualMidwinter ritualMidwinter = this;
    AudioManager.Instance.PlayOneShot("event:/rituals/generic_start_ritual");
    PlayerFarming.Instance.GoToAndStop(ChurchFollowerManager.Instance.RitualCenterPosition.position, GoToCallback: (System.Action) (() =>
    {
      Interaction_TempleAltar.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      PlayerFarming.Instance.simpleSpineAnimator.Animate("idle", 0, true);
      PlayerFarming.Instance.state.transform.DOMove(ChurchFollowerManager.Instance.RitualCenterPosition.position, 0.1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    }));
    Interaction_TempleAltar.Instance.SimpleSetCamera.Play();
    yield return (object) ritualMidwinter.StartCoroutine((IEnumerator) ritualMidwinter.WaitFollowersFormCircle());
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
    AudioManager.Instance.PlayOneShot("event:/dlc/ritual/midwinter_celebration_take_off_hoods");
    List<FollowerBrain> followers = new List<FollowerBrain>((IEnumerable<FollowerBrain>) Ritual.FollowerToAttendSermon);
    for (int index = 0; index < followers.Count && index + 1 < followers.Count; index += 2)
    {
      Follower followerById1 = FollowerManager.FindFollowerByID(followers[index].Info.ID);
      Follower followerById2 = FollowerManager.FindFollowerByID(followers[index + 1].Info.ID);
      if ((UnityEngine.Object) followerById1 != (UnityEngine.Object) null && (UnityEngine.Object) followerById2 != (UnityEngine.Object) null)
        ritualMidwinter.StartCoroutine((IEnumerator) ritualMidwinter.GiftRoutine(followerById1, followerById2));
    }
    if (followers.Count % 2 == 1)
    {
      yield return (object) new WaitForSeconds(2f);
      Follower followerById = FollowerManager.FindFollowerByID(followers[followers.Count - 1].Info.ID);
      double num = (double) followerById.SetBodyAnimation("Reactions/react-cry", false);
      followerById.AddBodyAnimation("idle", true, 0.0f);
      yield return (object) new WaitForSeconds(8f);
    }
    else
      yield return (object) new WaitForSeconds(10f);
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      followerBrain.Stats.Freezing = 0.0f;
      followerBrain.FrozeToDeath = false;
    }
    yield return (object) new WaitForSeconds(2f);
    PlayerFarming.Instance.simpleSpineAnimator.Animate("rituals/ritual-stop", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      if (followerBrain != null && followerBrain.CurrentTask is FollowerTask_AttendRitual)
        (followerBrain.CurrentTask as FollowerTask_AttendRitual).Cheer();
    }
    yield return (object) new WaitForSeconds(0.5f);
    BiomeConstants.Instance.ChromaticAbberationTween(1f, 1f, BiomeConstants.Instance.ChromaticAberrationDefaultValue);
    BiomeConstants.Instance.VignetteTween(1f, 0.7f, BiomeConstants.Instance.VignetteDefaultValue);
    ChurchFollowerManager.Instance.EndRitualOverlay();
    GameManager.GetInstance().CamFollowTarget.targetDistance = 11f;
    Interaction_TempleAltar.Instance.RitualLighting.gameObject.SetActive(false);
    float num1 = 0.0f;
    foreach (FollowerBrain brain in Ritual.FollowerToAttendSermon)
    {
      if (brain != null)
      {
        float Delay = UnityEngine.Random.Range(0.1f, 0.5f);
        num1 += Delay;
        ritualMidwinter.StartCoroutine((IEnumerator) ritualMidwinter.DelayFollowerReaction(brain, Delay));
      }
    }
    yield return (object) new WaitForSeconds(1.5f);
    Interaction_TempleAltar.Instance.CloseUpCamera.Reset();
    ritualMidwinter.CompleteRitual();
    WorldManipulatorManager.TriggerManipulation(WorldManipulatorManager.Manipulations.CureCursedFollowers);
    yield return (object) new WaitForSeconds(1f);
    AudioManager.Instance.PlayOneShot("event:/dlc/ritual/midwinter_celebration_end");
    CultFaithManager.AddThought(Thought.Cult_MidwinterRitual);
  }

  public IEnumerator MoveFollower(Follower follower, int index)
  {
    RitualMidwinter ritualMidwinter = this;
    List<Vector3> positions = new List<Vector3>()
    {
      Vector3.left,
      Vector3.right
    };
    string[] anims = new string[4]
    {
      "Farming/add-seed-mushroom",
      "Farming/add-seed-pumpkin",
      "Farming/add-seed-redflower",
      "Farming/add-seed"
    };
    bool waiting = true;
    follower.HoodOff(onComplete: (System.Action) (() => waiting = false));
    while (waiting)
      yield return (object) null;
    yield return (object) ritualMidwinter.StartCoroutine((IEnumerator) follower.GoToRoutine(PlayerFarming.Instance.transform.position + positions[index]));
    follower.State.facingAngle = Utils.GetAngle(follower.transform.position, PlayerFarming.Instance.transform.position);
    double num = (double) follower.SetBodyAnimation(anims[UnityEngine.Random.Range(0, anims.Length)], false);
    follower.AddBodyAnimation(anims[UnityEngine.Random.Range(0, anims.Length)], false, 0.0f);
    follower.AddBodyAnimation(anims[UnityEngine.Random.Range(0, anims.Length)], false, 0.0f);
    follower.AddBodyAnimation("idle", true, 0.0f);
  }

  public void ReplaceGiftOpenEvent(Follower follower, string sfxEvent)
  {
    foreach (followerSpineEventListeners spineEventListener in follower.gameObject.GetComponentInChildren<FollowerSpineEventListener>(true).spineEventListeners)
    {
      if (spineEventListener.eventName == "GiftReveal")
        spineEventListener.soundPath = sfxEvent;
    }
  }

  public IEnumerator GiftRoutine(Follower f1, Follower f2)
  {
    this.ReplaceGiftOpenEvent(f1, this.customGiftOpenEvent);
    int waitingCounter = 0;
    f1.HoodOff(onComplete: (System.Action) (() => ++waitingCounter));
    f2.HoodOff(onComplete: (System.Action) (() => ++waitingCounter));
    while (waitingCounter < 2)
      yield return (object) null;
    waitingCounter = 0;
    Vector3 vector3 = (f1.transform.position + f2.transform.position) / 2f;
    f1.GoTo(vector3 + Vector3.left / 2f, (System.Action) (() => ++waitingCounter));
    f2.GoTo(vector3 + Vector3.right / 2f, (System.Action) (() => ++waitingCounter));
    while (waitingCounter < 2)
      yield return (object) null;
    if ((double) UnityEngine.Random.value < 0.5)
    {
      Follower follower = f1;
      f1 = f2;
      f2 = follower;
    }
    f1.FacePosition(f2.transform.position);
    f2.FacePosition(f1.transform.position);
    yield return (object) new WaitForSeconds(1f);
    int rand = UnityEngine.Random.Range(0, 2);
    AudioManager.Instance.PlayOneShot("event:/dlc/ritual/midwinter_celebration_gift_appear", f2.transform.position);
    double num1 = (double) f2.SetBodyAnimation(rand == 0 ? "Gifts/gift-give-small" : "Gifts/gift-give-medium", false);
    f2.AddBodyAnimation("idle", true, 0.0f);
    yield return (object) new WaitForSeconds(2.16666675f);
    double num2 = (double) f1.SetBodyAnimation(rand == 0 ? $"Gifts/gift-small-{UnityEngine.Random.Range(1, 4)}" : $"Gifts/gift-medium-{UnityEngine.Random.Range(1, 4)}", false);
    f1.AddBodyAnimation("idle", true, 0.0f);
    double num3 = (double) f2.SetBodyAnimation($"Gifts/gift-aftergive-{UnityEngine.Random.Range(1, 3)}", false);
    f2.AddBodyAnimation("idle", true, 0.0f);
    this.ReplaceGiftOpenEvent(f1, this.defaultGiftOpenEvent);
  }
}
