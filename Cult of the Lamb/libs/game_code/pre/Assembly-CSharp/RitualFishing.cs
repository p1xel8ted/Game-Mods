// Decompiled with JetBrains decompiler
// Type: RitualFishing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RitualFishing : Ritual
{
  public static System.Action OnFishingRitualBegan;
  private EventInstance loopedInstance;
  private EventInstance loopedInstance1;
  private Follower Follower1;
  private Follower Follower2;

  protected override UpgradeSystem.Type RitualType => UpgradeSystem.Type.Ritual_FishingRitual;

  public override void Play()
  {
    base.Play();
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.RitualRoutine());
  }

  private void OnDisable()
  {
    AudioManager.Instance.StopLoop(this.loopedInstance);
    AudioManager.Instance.StopLoop(this.loopedInstance1);
  }

  private IEnumerator RitualRoutine()
  {
    RitualFishing ritualFishing = this;
    AudioManager.Instance.PlayOneShot("event:/rituals/generic_start_ritual");
    PlayerFarming.Instance.GoToAndStop(ChurchFollowerManager.Instance.RitualCenterPosition.position, GoToCallback: (System.Action) (() =>
    {
      Interaction_TempleAltar.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      PlayerFarming.Instance.simpleSpineAnimator.Animate("idle", 0, true);
      PlayerFarming.Instance.state.transform.DOMove(ChurchFollowerManager.Instance.RitualCenterPosition.position, 0.1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    }));
    Interaction_TempleAltar.Instance.SimpleSetCamera.Play();
    yield return (object) ritualFishing.StartCoroutine((IEnumerator) ritualFishing.WaitFollowersFormCircle());
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
    ChurchFollowerManager.Instance.Water.gameObject.SetActive(true);
    ChurchFollowerManager.Instance.Water.transform.localScale = Vector3.zero;
    ChurchFollowerManager.Instance.Water.transform.DOScale(new Vector3(0.5f, 0.5f), 1.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    ritualFishing.loopedInstance1 = AudioManager.Instance.CreateLoop("event:/player/watering", ChurchFollowerManager.Instance.Water, true);
    ritualFishing.loopedInstance = AudioManager.Instance.CreateLoop("event:/atmos/hub_shore/water_lapping", ChurchFollowerManager.Instance.Water, true);
    AudioManager.Instance.SetEventInstanceParameter(ritualFishing.loopedInstance, "intensity", 1f);
    AudioManager.Instance.PlayLoop(ritualFishing.loopedInstance);
    Interaction_TempleAltar.Instance.SimpleSetCamera.Reset();
    Interaction_TempleAltar.Instance.CloseUpCamera.Play();
    List<FollowerBrain> followerBrainList = new List<FollowerBrain>((IEnumerable<FollowerBrain>) Ritual.GetFollowersAvailableToAttendSermon());
    for (int index = 0; index < 2 && followerBrainList.Count != 0; ++index)
    {
      FollowerBrain followerBrain = followerBrainList[UnityEngine.Random.Range(0, followerBrainList.Count)];
      followerBrainList.Remove(followerBrain);
      Follower followerById = FollowerManager.FindFollowerByID(followerBrain.Info.ID);
      if ((bool) (UnityEngine.Object) followerById)
      {
        if (index == 0)
          ritualFishing.Follower1 = followerById;
        else
          ritualFishing.Follower2 = followerById;
        ritualFishing.StartCoroutine((IEnumerator) ritualFishing.MoveFollower(followerById, index));
      }
    }
    foreach (FollowerBrain followerBrain in followerBrainList)
    {
      if (followerBrain.CurrentTask is FollowerTask_AttendRitual)
        ((FollowerTask_AttendRitual) followerBrain.CurrentTask).Pray2();
    }
    yield return (object) new WaitForSeconds(1.5f);
    AudioManager.Instance.StopLoop(ritualFishing.loopedInstance1);
    yield return (object) new WaitForSeconds(7.5f);
    if ((bool) (UnityEngine.Object) ritualFishing.Follower1)
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.FISH_BIG, 1, ritualFishing.Follower1.transform.position);
    yield return (object) new WaitForSeconds(0.2f);
    if ((bool) (UnityEngine.Object) ritualFishing.Follower2)
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.FISH_BIG, 1, ritualFishing.Follower2.transform.position);
    AudioManager.Instance.StopLoop(ritualFishing.loopedInstance);
    ChurchFollowerManager.Instance.Water.transform.DOScale(new Vector3(0.0f, 0.0f), 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
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
      ritualFishing.StartCoroutine((IEnumerator) ritualFishing.DelayFollowerReaction(brain, Delay));
      brain.AddThought(Thought.FishingRitual);
    }
    yield return (object) new WaitForSeconds(1.5f);
    DataManager.Instance.LastFishingDeclared = TimeManager.TotalElapsedGameTime;
    Interaction_TempleAltar.Instance.CloseUpCamera.Reset();
    System.Action fishingRitualBegan = RitualFishing.OnFishingRitualBegan;
    if (fishingRitualBegan != null)
      fishingRitualBegan();
    ritualFishing.CompleteRitual();
    yield return (object) new WaitForSeconds(1f);
    CultFaithManager.AddThought(Thought.Cult_FishingRitual);
  }

  private IEnumerator MoveFollower(Follower follower, int index)
  {
    RitualFishing ritualFishing = this;
    List<Vector3> positions = new List<Vector3>()
    {
      Vector3.left,
      Vector3.right
    };
    bool waiting = true;
    follower.HoodOff(onComplete: (System.Action) (() => waiting = false));
    while (waiting)
      yield return (object) null;
    yield return (object) ritualFishing.StartCoroutine((IEnumerator) follower.GoToRoutine(PlayerFarming.Instance.transform.position + positions[index]));
    follower.State.facingAngle = Utils.GetAngle(follower.transform.position, PlayerFarming.Instance.transform.position);
    double num = (double) follower.SetBodyAnimation("Fishing/fishing-start", false);
    follower.AddBodyAnimation("Fishing/fishing-reel", false, 0.0f);
    follower.AddBodyAnimation("Fishing/fishing-reel", false, 0.0f);
    follower.AddBodyAnimation("Fishing/fishing-catch-big", false, 0.0f);
    follower.AddBodyAnimation("idle", true, 0.0f);
  }
}
