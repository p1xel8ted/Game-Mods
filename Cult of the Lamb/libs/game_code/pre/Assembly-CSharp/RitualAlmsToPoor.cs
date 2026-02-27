// Decompiled with JetBrains decompiler
// Type: RitualAlmsToPoor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using System.Collections;
using UnityEngine;

#nullable disable
public class RitualAlmsToPoor : Ritual
{
  private EventInstance loopedSound;

  protected override UpgradeSystem.Type RitualType => UpgradeSystem.Type.Ritual_AlmsToPoor;

  public override void Play()
  {
    base.Play();
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.RitualRoutine());
  }

  private IEnumerator RitualRoutine()
  {
    RitualAlmsToPoor ritualAlmsToPoor = this;
    AudioManager.Instance.PlayOneShot("event:/rituals/generic_start_ritual");
    Interaction_TempleAltar.Instance.SimpleSetCamera.Play();
    PlayerFarming.Instance.GoToAndStop(ChurchFollowerManager.Instance.RitualCenterPosition.position, GoToCallback: (System.Action) (() =>
    {
      Interaction_TempleAltar.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      PlayerFarming.Instance.simpleSpineAnimator.Animate("build", 0, true);
      PlayerFarming.Instance.state.transform.DOMove(ChurchFollowerManager.Instance.RitualCenterPosition.position, 0.1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    }));
    yield return (object) ritualAlmsToPoor.StartCoroutine((IEnumerator) ritualAlmsToPoor.WaitFollowersFormCircle());
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
    ritualAlmsToPoor.loopedSound = AudioManager.Instance.CreateLoop("event:/rituals/coin_loop", PlayerFarming.Instance.gameObject, true);
    float delay = 0.0f;
    foreach (FollowerBrain followerBrain in Ritual.GetFollowersAvailableToAttendSermon())
    {
      ritualAlmsToPoor.StartCoroutine((IEnumerator) ritualAlmsToPoor.GiveCoins(FollowerManager.FindFollowerByID(followerBrain.Info.ID), 5f, delay));
      delay += 0.1f;
    }
    yield return (object) new WaitForSeconds(5f);
    AudioManager.Instance.StopLoop(ritualAlmsToPoor.loopedSound);
    yield return (object) new WaitForSeconds(1.2f);
    Interaction_TempleAltar.Instance.PulseDisplacementObject(PlayerFarming.Instance.CameraBone.transform.position);
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
    float EndingDelay = 0.0f;
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      FollowerBrain brain = followerBrain;
      brain.AddThought(Thought.AlmsToThePoorRitual);
      brain.AddAdoration(FollowerBrain.AdorationActions.Ritual_AlmsToPoor, (System.Action) (() =>
      {
        float Delay = UnityEngine.Random.Range(0.1f, 0.5f);
        EndingDelay += Delay;
        GameManager.GetInstance().StartCoroutine((IEnumerator) this.DelayFollowerReaction(brain, Delay));
      }));
    }
    yield return (object) new WaitForSeconds(3f + EndingDelay);
    Interaction_TempleAltar.Instance.SimpleSetCamera.Reset();
    ritualAlmsToPoor.CompleteRitual();
    yield return (object) new WaitForSeconds(1f);
    CultFaithManager.AddThought(Thought.Cult_AlmsToPoor);
  }

  private IEnumerator GiveCoins(Follower follower, float totalTime, float delay)
  {
    RitualAlmsToPoor ritualAlmsToPoor = this;
    yield return (object) new WaitForSeconds(delay);
    int randomCoins = UnityEngine.Random.Range(3, 7);
    float increment = (totalTime - delay) / (float) randomCoins;
    for (int i = 0; i < randomCoins; ++i)
    {
      AudioManager.Instance.PlayOneShot("event:/followers/pop_in", PlayerFarming.Instance.transform.position);
      ResourceCustomTarget.Create(follower.gameObject, PlayerFarming.Instance.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, new System.Action(ritualAlmsToPoor.PlayCoinSound));
      yield return (object) new WaitForSeconds(increment);
    }
  }

  private void PlayCoinSound()
  {
    AudioManager.Instance.PlayOneShot("event:/rituals/coins", PlayerFarming.Instance.transform.position);
  }
}
