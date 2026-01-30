// Decompiled with JetBrains decompiler
// Type: RitualDonation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RitualDonation : Ritual
{
  public const int COINS_PER_FOLLOWER = 3;
  public const int MAX_COINS = 200;

  public override UpgradeSystem.Type RitualType => UpgradeSystem.Type.Ritual_DonationRitual;

  public override void Play()
  {
    base.Play();
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.RitualRoutine());
  }

  public IEnumerator RitualRoutine()
  {
    RitualDonation ritualDonation = this;
    AudioManager.Instance.PlayOneShot("event:/rituals/generic_start_ritual");
    PlayerFarming.Instance.GoToAndStop(ChurchFollowerManager.Instance.RitualCenterPosition.position, GoToCallback: (System.Action) (() =>
    {
      Interaction_TempleAltar.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      PlayerFarming.Instance.simpleSpineAnimator.Animate("build", 0, true);
      PlayerFarming.Instance.state.transform.DOMove(ChurchFollowerManager.Instance.RitualCenterPosition.position, 0.1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    }));
    yield return (object) ritualDonation.StartCoroutine((IEnumerator) ritualDonation.WaitFollowersFormCircle());
    if (DataManager.Instance.HasMidasHiding && !MidasBaseController.EncounteredMidasInTemple)
    {
      MidasBaseController.EncounteredMidasInTemple = true;
      GameManager.GetInstance().StartCoroutine((IEnumerator) ritualDonation.MidasIE());
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
    float delay = 0.0f;
    int num1 = 0;
    int num2 = 0;
    int num3 = 2;
    float totalTime = 5f;
    List<FollowerBrain> availableToAttendSermon = Ritual.GetFollowersAvailableToAttendSermon();
    foreach (FollowerBrain followerBrain in availableToAttendSermon)
    {
      int num4 = Mathf.Clamp(followerBrain.Info.XPLevel, 1, 10);
      int coinPerFollower = 3 + num4;
      num2 += num4;
      num1 += coinPerFollower;
      ritualDonation.StartCoroutine((IEnumerator) ritualDonation.GiveCoins(FollowerManager.FindFollowerByID(followerBrain.Info.ID), totalTime, delay, coinPerFollower, availableToAttendSermon.Count));
      delay += 0.1f;
    }
    if (DataManager.Instance.HasMidasHiding)
      DataManager.Instance.MidasStolenGold.Add(new InventoryItem(InventoryItem.ITEM_TYPE.BLACK_GOLD, num1));
    else
      Inventory.AddItem(20, num1);
    float deathtimer = BiomeConstants.Instance.HitFX_Blocked.GetComponent<destroyMe>().deathtimer;
    BiomeConstants.Instance.HitFX_Blocked.CreatePool((int) ((double) (num2 * num3) / ((double) totalTime / (double) deathtimer)), true);
    yield return (object) null;
    ResourceCustomTarget.CreatePool((int) ((double) Ritual.FollowerToAttendSermon.Count * 1.5));
    yield return (object) new WaitForSeconds(6.2f);
    if (DataManager.Instance.HasMidasHiding)
    {
      Interaction_TempleAltar.Instance.PulseDisplacementObject(ChurchFollowerManager.Instance.RitualCenterPosition.position);
    }
    else
    {
      Interaction_TempleAltar.Instance.PulseDisplacementObject(PlayerFarming.Instance.CameraBone.transform.position);
      PlayerFarming.Instance.simpleSpineAnimator.Animate("rituals/ritual-stop", 0, false);
      PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    }
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
    float num5 = 0.0f;
    foreach (FollowerBrain brain in Ritual.FollowerToAttendSermon)
    {
      if (DataManager.Instance.HasMidasHiding)
        brain.AddThought(Thought.DonationRitual_Midas);
      float Delay = UnityEngine.Random.Range(0.1f, 0.5f);
      num5 += Delay;
      ritualDonation.StartCoroutine((IEnumerator) ritualDonation.DelayFollowerReaction(brain, Delay));
    }
    yield return (object) new WaitForSeconds(1.5f);
    ritualDonation.CompleteRitual();
    yield return (object) new WaitForSeconds(1f);
    CultFaithManager.AddThought(Thought.Cult_DonationRitual);
  }

  public IEnumerator GiveCoins(
    Follower follower,
    float totalTime,
    float delay,
    int coinPerFollower,
    int totalFollowers)
  {
    if (!((UnityEngine.Object) follower == (UnityEngine.Object) null))
    {
      yield return (object) new WaitForSeconds(delay);
      int coins = coinPerFollower;
      if (Mathf.RoundToInt((float) (3 * totalFollowers)) >= 200)
        coins = Mathf.RoundToInt((float) (200 / totalFollowers));
      float increment = (totalTime - delay) / (float) coins;
      for (int i = 0; i < coins; ++i)
      {
        AudioManager.Instance.PlayOneShot("event:/followers/pop_in", ChurchFollowerManager.Instance.RitualCenterPosition.position);
        ResourceCustomTarget.Create(ChurchFollowerManager.Instance.RitualCenterPosition.gameObject, follower.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, (System.Action) null);
        yield return (object) new WaitForSeconds(increment);
      }
    }
  }

  public IEnumerator MidasIE()
  {
    RitualDonation ritualDonation = this;
    FollowerManager.SpawnedFollower follower = FollowerManager.SpawnCopyFollower(DataManager.Instance.MidasFollowerInfo, ChurchFollowerManager.Instance.DoorPosition.position, (Transform) null, FollowerLocation.Church);
    follower.FollowerBrain.ResetStats();
    yield return (object) new WaitForSeconds(1f);
    bool waiting = true;
    follower.Follower.LockToGround = true;
    follower.Follower.GoTo(Interaction_TempleAltar.Instance.playerFarming.transform.position, (System.Action) (() => waiting = false));
    while (waiting)
      yield return (object) null;
    PlayerFarming player = Interaction_TempleAltar.Instance.playerFarming;
    AudioManager.Instance.PlayOneShot("event:/player/gethit", player.transform.position);
    BiomeConstants.Instance.EmitHitVFX(player.transform.position, Quaternion.identity.z, "HitFX_Blocked");
    player.simpleSpineAnimator.FlashRedTint();
    CameraManager.instance.ShakeCameraForDuration(1.5f, 1.7f, 0.2f);
    player.state.facingAngle = Utils.GetAngle(PlayerFarming.Instance.transform.position, ritualDonation.transform.position);
    player.state.CURRENT_STATE = StateMachine.State.HitThrown;
    player.transform.DOMoveX(player.transform.position.x + ((double) player.transform.position.x > (double) follower.Follower.transform.position.x ? 1f : -1f), 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
    player.transform.DOMoveY(player.transform.position.y + 1f, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
    GameManager.GetInstance().OnConversationNext(follower.Follower.gameObject, 8f);
    yield return (object) new WaitForSeconds(0.15f);
    player.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    player.Spine.AnimationState.AddAnimation(0, "reactions/react-angry", false, 0.0f);
    player.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    player.state.facingAngle = Utils.GetAngle(ritualDonation.transform.position, follower.Follower.transform.position);
    follower.Follower.Spine.AnimationState.SetAnimation(1, "recruit-start", false);
    follower.Follower.Spine.AnimationState.AddAnimation(1, "recruit-loop", true, 0.0f);
    yield return (object) new WaitForSeconds(5f);
    follower.Follower.Spine.AnimationState.SetAnimation(1, "recruit-end2", false);
    yield return (object) new WaitForSeconds(1.23f);
    follower.Follower.Spine.AnimationState.SetAnimation(1, "Jeer/jeer-plotting" + ((double) UnityEngine.Random.value < 0.5 ? "2" : ""), false);
    yield return (object) new WaitForSeconds(2.3f);
    follower.Follower.State.CURRENT_STATE = StateMachine.State.Idle;
    waiting = true;
    follower.Follower.GoTo(ChurchFollowerManager.Instance.DoorPosition.position, (System.Action) (() => waiting = false));
    while (waiting)
      yield return (object) null;
    FollowerManager.CleanUpCopyFollower(follower);
  }
}
