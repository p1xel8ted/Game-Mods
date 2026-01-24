// Decompiled with JetBrains decompiler
// Type: RitualAlmsToPoor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using System.Collections;
using UnityEngine;

#nullable disable
public class RitualAlmsToPoor : Ritual
{
  public new EventInstance loopedSound;
  public FollowerManager.SpawnedFollower midas;

  public override UpgradeSystem.Type RitualType => UpgradeSystem.Type.Ritual_AlmsToPoor;

  public override void Play()
  {
    base.Play();
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.RitualRoutine());
  }

  public IEnumerator RitualRoutine()
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
    if (DataManager.Instance.HasMidasHiding && !MidasBaseController.EncounteredMidasInTemple)
    {
      MidasBaseController.EncounteredMidasInTemple = true;
      ritualAlmsToPoor.StartCoroutine((IEnumerator) ritualAlmsToPoor.MidasIE());
      yield return (object) new WaitForSeconds(2.5f);
    }
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
    float totalTime = 5f;
    int maxCoins = 7;
    foreach (FollowerBrain followerBrain in Ritual.GetFollowersAvailableToAttendSermon())
    {
      ritualAlmsToPoor.StartCoroutine((IEnumerator) ritualAlmsToPoor.GiveCoins(FollowerManager.FindFollowerByID(followerBrain.Info.ID), totalTime, delay));
      delay += 0.1f;
    }
    float deathtimer = BiomeConstants.Instance.HitFX_Blocked.GetComponent<destroyMe>().deathtimer;
    BiomeConstants.Instance.HitFX_Blocked.CreatePool((int) ((double) (Ritual.FollowerToAttendSermon.Count * maxCoins) / ((double) totalTime / (double) deathtimer)), true);
    yield return (object) null;
    ResourceCustomTarget.CreatePool(Ritual.FollowerToAttendSermon.Count * maxCoins);
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
      if (DataManager.Instance.HasMidasHiding)
        brain.AddThought(Thought.AlmsToThePoorRitual_Midas);
      else
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

  public IEnumerator GiveCoins(Follower follower, float totalTime, float delay)
  {
    RitualAlmsToPoor ritualAlmsToPoor = this;
    if (!((UnityEngine.Object) follower == (UnityEngine.Object) null))
    {
      yield return (object) new WaitForSeconds(delay);
      int randomCoins = UnityEngine.Random.Range(3, 7);
      float increment = (totalTime - delay) / (float) randomCoins;
      for (int i = 0; i < randomCoins; ++i)
      {
        AudioManager.Instance.PlayOneShot("event:/followers/pop_in", ChurchFollowerManager.Instance.RitualCenterPosition.position);
        GameObject gameObject = follower.gameObject;
        if (DataManager.Instance.HasMidasHiding)
        {
          gameObject = ritualAlmsToPoor.midas.Follower.gameObject;
          DataManager.Instance.MidasStolenGold.Add(new InventoryItem(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1));
        }
        ResourceCustomTarget.Create(gameObject, ChurchFollowerManager.Instance.RitualCenterPosition.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, new System.Action(ritualAlmsToPoor.PlayCoinSound));
        yield return (object) new WaitForSeconds(increment);
      }
    }
  }

  public void PlayCoinSound()
  {
    AudioManager.Instance.PlayOneShot("event:/rituals/coins", ChurchFollowerManager.Instance.RitualCenterPosition.position);
  }

  public IEnumerator MidasIE()
  {
    this.midas = FollowerManager.SpawnCopyFollower(DataManager.Instance.MidasFollowerInfo, ChurchFollowerManager.Instance.DoorPosition.position, (Transform) null, FollowerLocation.Church);
    this.midas.FollowerBrain.ResetStats();
    yield return (object) new WaitForSeconds(1f);
    bool waiting = true;
    this.midas.Follower.LockToGround = true;
    this.midas.Follower.GoTo(Interaction_TempleAltar.Instance.playerFarming.transform.position + Vector3.left + Vector3.down, (System.Action) (() => waiting = false));
    while (waiting)
      yield return (object) null;
    this.midas.Follower.Spine.AnimationState.SetAnimation(1, "Jeer/jeer-plotting" + ((double) UnityEngine.Random.value < 0.5 ? "2" : ""), true);
    yield return (object) new WaitForSeconds(5f);
    waiting = true;
    this.midas.Follower.GoTo(ChurchFollowerManager.Instance.DoorPosition.position, (System.Action) (() => waiting = false));
    while (waiting)
      yield return (object) null;
    FollowerManager.CleanUpCopyFollower(this.midas);
  }
}
