// Decompiled with JetBrains decompiler
// Type: RitualBrainwashing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RitualBrainwashing : Ritual
{
  public static System.Action OnBrainwashingRitualBegan;
  public Tween punchTween;

  public override UpgradeSystem.Type RitualType => UpgradeSystem.Type.Ritual_Brainwashing;

  public override void Play()
  {
    foreach (FollowerBrain followerBrain in Ritual.GetFollowersAvailableToAttendSermon(true))
    {
      if (followerBrain.CurrentTask != null && (followerBrain.CurrentTask is FollowerTask_AttendRitual || followerBrain.CurrentTask is FollowerTask_AttendTeaching))
        followerBrain.CurrentTask.Abort();
    }
    Interaction_TempleAltar.Instance.FrontWall.SetActive(false);
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.RitualRoutine());
  }

  public IEnumerator RitualRoutine()
  {
    RitualBrainwashing ritualBrainwashing = this;
    AudioManager.Instance.PlayOneShot("event:/rituals/generic_start_ritual");
    PlayerFarming.Instance.GoToAndStop(ChurchFollowerManager.Instance.RitualCenterPosition.position + Vector3.up, ChurchFollowerManager.Instance.RitualCenterPosition.gameObject, GoToCallback: (System.Action) (() =>
    {
      Interaction_TempleAltar.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      PlayerFarming.Instance.simpleSpineAnimator.Animate("idle", 0, true);
      PlayerFarming.Instance.state.transform.DOMove(ChurchFollowerManager.Instance.RitualCenterPosition.position + Vector3.up, 0.1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    }));
    Interaction_TempleAltar.Instance.SimpleSetCamera.Play();
    yield return (object) ritualBrainwashing.StartCoroutine((IEnumerator) ritualBrainwashing.WaitFollowersFormCircle(true));
    yield return (object) new WaitForSeconds(1f);
    PlayerFarming.Instance.simpleSpineAnimator.Animate("build", 0, true);
    PlayerFarming.Instance.Spine.skeleton.FindBone("ritualring").Rotation += 60f;
    PlayerFarming.Instance.Spine.skeleton.UpdateWorldTransform();
    PlayerFarming.Instance.Spine.skeleton.Update(Time.deltaTime);
    PlayerFarming.Instance.simpleSpineAnimator.Animate("rituals/ritual-start", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("rituals/ritual-loop", 0, true, 0.0f);
    DOTween.To((DOGetter<float>) (() => GameManager.GetInstance().CamFollowTarget.targetDistance), (DOSetter<float>) (x => GameManager.GetInstance().CamFollowTarget.targetDistance = x), 6.5f, 1f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutSine);
    yield return (object) new WaitForSeconds(1.2f);
    Interaction_TempleAltar.Instance.SimpleSetCamera.Reset();
    Interaction_TempleAltar.Instance.CloseUpCamera.Play();
    List<FollowerBrain> followers = new List<FollowerBrain>((IEnumerable<FollowerBrain>) Ritual.GetFollowersAvailableToAttendSermon(true));
    yield return (object) new WaitForSeconds(1f);
    ChurchFollowerManager.Instance.Mushrooms.SetActive(true);
    ChurchFollowerManager.Instance.Mushrooms.transform.DOPunchScale(Vector3.one * 0.25f, 0.25f);
    foreach (FollowerBrain followerBrain in followers)
    {
      if (followerBrain.CurrentTask is FollowerTask_AttendRitual)
        ((FollowerTask_AttendRitual) followerBrain.CurrentTask).Pray3();
    }
    yield return (object) new WaitForSeconds(1f);
    float delay = 0.0f;
    foreach (FollowerBrain followerBrain in Ritual.GetFollowersAvailableToAttendSermon())
    {
      ritualBrainwashing.StartCoroutine((IEnumerator) ritualBrainwashing.GiveShrooms(FollowerManager.FindFollowerByID(followerBrain.Info.ID), 5f, delay));
      delay += 0.1f;
    }
    yield return (object) new WaitForSeconds(1f);
    BiomeConstants.Instance.PsychedelicFadeIn(5f);
    AudioManager.Instance.SetMusicPsychedelic(1f);
    yield return (object) new WaitForSeconds(4f);
    ChurchFollowerManager.Instance.Mushrooms.transform.DOScale(0.0f, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
    {
      ChurchFollowerManager.Instance.Mushrooms.SetActive(false);
      ChurchFollowerManager.Instance.Mushrooms.transform.localScale = Vector3.one;
    }));
    yield return (object) new WaitForSeconds(1f);
    Transform transform = GameManager.GetInstance().CamFollowTarget.transform;
    GameManager.GetInstance().CamFollowTarget.enabled = false;
    Vector3 position = transform.position;
    Vector3 forward = transform.forward;
    transform.position = position;
    GameManager.GetInstance().CamFollowTarget.enabled = true;
    followers = new List<FollowerBrain>((IEnumerable<FollowerBrain>) Ritual.GetFollowersAvailableToAttendSermon());
    foreach (FollowerBrain followerBrain in followers)
    {
      if (followerBrain.CurrentTask is FollowerTask_AttendRitual)
      {
        Follower followerById = FollowerManager.FindFollowerByID(followerBrain.Info.ID);
        if ((bool) (UnityEngine.Object) followerById)
        {
          followerById.SetFaceAnimation("Emotions/emotion-brainwashed", true);
          followerById.SetOutfit(followerById.Brain.Info.Outfit, true);
          ((FollowerTask_AttendRitual) followerBrain.CurrentTask).Idle();
        }
      }
    }
    yield return (object) new WaitForSeconds(1f);
    foreach (FollowerBrain followerBrain in followers)
    {
      if (followerBrain.CurrentTask is FollowerTask_AttendRitual)
        ((FollowerTask_AttendRitual) followerBrain.CurrentTask).DanceBrainwashed();
    }
    yield return (object) new WaitForSeconds(3f);
    PlayerFarming.Instance.simpleSpineAnimator.Animate("rituals/ritual-stop", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(0.5f);
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      if (followerBrain.CurrentTask is FollowerTask_AttendRitual)
        (followerBrain.CurrentTask as FollowerTask_AttendRitual).Cheer();
    }
    GameManager.GetInstance().CamFollowTarget.targetDistance = 11f;
    float num = 1.5f;
    foreach (FollowerBrain brain in Ritual.FollowerToAttendSermon)
    {
      float Delay = UnityEngine.Random.Range(0.1f, 0.5f);
      num += Delay;
      ritualBrainwashing.StartCoroutine((IEnumerator) ritualBrainwashing.DelayFollowerReaction(brain, Delay));
    }
    BiomeConstants.Instance.PsychedelicFadeOut(1.5f);
    AudioManager.Instance.SetMusicPsychedelic(0.0f);
    yield return (object) new WaitForSeconds(1.5f);
    Interaction_TempleAltar.Instance.CloseUpCamera.Reset();
    foreach (FollowerBrain followerBrain in followers)
    {
      followerBrain.AddThought(Thought.Brainwashed);
      if (followerBrain.HasTrait(FollowerTrait.TraitType.MushroomEncouraged))
        followerBrain.AddThought(Thought.FollowerBrainwashedSubstanceEncouraged);
      else if (followerBrain.HasTrait(FollowerTrait.TraitType.MushroomBanned))
      {
        followerBrain.AddThought(Thought.FollowerBrainwashedSubstanceBanned);
        if ((double) UnityEngine.Random.Range(0.0f, 1f) < 0.5)
          followerBrain.MakeSick();
      }
      else
        followerBrain.AddThought(Thought.FollowerBrainwashed);
    }
    ritualBrainwashing.CompleteRitual();
    yield return (object) new WaitForSeconds(1f);
    if (DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.MushroomEncouraged))
      CultFaithManager.AddThought(Thought.Cult_MushroomEncouraged_Trait);
    else
      CultFaithManager.AddThought(Thought.Brainwashed);
    DataManager.Instance.LastBrainwashed = TimeManager.TotalElapsedGameTime;
    if (!DataManager.Instance.PerformedMushroomRitual)
    {
      ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/VisitSozo", Objectives.CustomQuestTypes.SozoReturn));
      ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.SozoPerformRitual);
    }
    DataManager.Instance.PerformedMushroomRitual = true;
    System.Action brainwashingRitualBegan = RitualBrainwashing.OnBrainwashingRitualBegan;
    if (brainwashingRitualBegan != null)
      brainwashingRitualBegan();
  }

  public IEnumerator GiveShrooms(Follower follower, float totalTime, float delay)
  {
    if (!((UnityEngine.Object) follower == (UnityEngine.Object) null))
    {
      yield return (object) new WaitForSeconds(delay);
      int randomCoins = UnityEngine.Random.Range(3, 7);
      float increment = (totalTime - delay) / (float) randomCoins;
      for (int i = 0; i < randomCoins; ++i)
      {
        if (this.punchTween == null || !this.punchTween.active)
          this.punchTween = (Tween) ChurchFollowerManager.Instance.Mushrooms.transform.DOPunchScale(Vector3.one * 0.1f, increment - 0.05f);
        AudioManager.Instance.PlayOneShot("event:/followers/pop_in", ChurchFollowerManager.Instance.Mushrooms.transform.position);
        ResourceCustomTarget.Create(follower.gameObject, ChurchFollowerManager.Instance.Mushrooms.transform.position, InventoryItem.ITEM_TYPE.MUSHROOM_SMALL, (System.Action) null);
        yield return (object) new WaitForSeconds(increment);
      }
    }
  }
}
