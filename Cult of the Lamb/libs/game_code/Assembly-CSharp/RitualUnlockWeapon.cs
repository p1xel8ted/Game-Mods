// Decompiled with JetBrains decompiler
// Type: RitualUnlockWeapon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using UnityEngine;

#nullable disable
public class RitualUnlockWeapon : Ritual
{
  public int NumGivingDevotion;

  public override UpgradeSystem.Type RitualType => UpgradeSystem.Type.Ritual_UnlockWeapon;

  public override void Play()
  {
    base.Play();
    this.StartCoroutine((IEnumerator) this.HeartsOfTheFaithfulRitual());
  }

  public IEnumerator HeartsOfTheFaithfulRitual()
  {
    RitualUnlockWeapon ritualUnlockWeapon = this;
    AudioManager.Instance.PlayOneShot("event:/rituals/generic_start_ritual");
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.gameObject, 8f);
    PlayerFarming.Instance.GoToAndStop(ChurchFollowerManager.Instance.RitualCenterPosition.position, GoToCallback: (System.Action) (() =>
    {
      Interaction_TempleAltar.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      PlayerFarming.Instance.simpleSpineAnimator.Animate("build", 0, true);
      PlayerFarming.Instance.state.transform.DOMove(ChurchFollowerManager.Instance.RitualCenterPosition.position, 0.1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    }));
    yield return (object) ritualUnlockWeapon.StartCoroutine((IEnumerator) ritualUnlockWeapon.WaitFollowersFormCircle());
    yield return (object) new WaitForSeconds(1.25f);
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.gameObject, 10f);
    PlayerFarming.Instance.Spine.skeleton.FindBone("ritualring").Rotation += 60f;
    PlayerFarming.Instance.Spine.skeleton.UpdateWorldTransform();
    PlayerFarming.Instance.Spine.skeleton.Update(Time.deltaTime);
    PlayerFarming.Instance.simpleSpineAnimator.Animate("rituals/ritual-start", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("rituals/ritual-loop", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(0.5f);
    Interaction_TempleAltar.Instance.RitualLighting.gameObject.SetActive(true);
    ChurchFollowerManager.Instance.StartRitualOverlay();
    yield return (object) new WaitForSeconds(0.5f);
    BiomeConstants.Instance.ChromaticAbberationTween(2f, BiomeConstants.Instance.ChromaticAberrationDefaultValue, 1f);
    BiomeConstants.Instance.VignetteTween(2f, BiomeConstants.Instance.VignetteDefaultValue, 0.7f);
    ritualUnlockWeapon.NumGivingDevotion = 0;
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      if (followerBrain != null && followerBrain.CurrentTask != null && followerBrain.CurrentTask is FollowerTask_AttendRitual)
      {
        ++ritualUnlockWeapon.NumGivingDevotion;
        (followerBrain.CurrentTask as FollowerTask_AttendRitual).WorshipTentacle();
        Follower followerById = FollowerManager.FindFollowerByID(followerBrain.Info.ID);
        ritualUnlockWeapon.StartCoroutine((IEnumerator) ritualUnlockWeapon.SpawnSouls(followerById.transform.position));
        yield return (object) new WaitForSeconds(0.075f);
      }
    }
    DOTween.To((DOGetter<float>) (() => GameManager.GetInstance().CamFollowTarget.targetDistance), (DOSetter<float>) (x => GameManager.GetInstance().CamFollowTarget.targetDistance = x), 6f, 3.75f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InOutSine);
    while (ritualUnlockWeapon.NumGivingDevotion > 0)
      yield return (object) null;
    yield return (object) new WaitForSeconds(0.5f);
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      if (followerBrain.CurrentTask is FollowerTask_AttendRitual)
        (followerBrain.CurrentTask as FollowerTask_AttendRitual).Cheer();
    }
    yield return (object) ritualUnlockWeapon.StartCoroutine((IEnumerator) ritualUnlockWeapon.EmitParticles());
    yield return (object) new WaitForSeconds(0.5f);
    BiomeConstants.Instance.ChromaticAbberationTween(1f, 1f, BiomeConstants.Instance.ChromaticAberrationDefaultValue);
    BiomeConstants.Instance.VignetteTween(1f, 0.7f, BiomeConstants.Instance.VignetteDefaultValue);
    ChurchFollowerManager.Instance.EndRitualOverlay();
    GameManager.GetInstance().CamFollowTarget.targetDistance = 11f;
    PlayerFarming.Instance.simpleSpineAnimator.Animate("rituals/ritual-stop", 0, false);
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().CameraResetTargetZoom();
    Interaction_TempleAltar.Instance.RitualLighting.gameObject.SetActive(false);
    float num = 0.0f;
    foreach (FollowerBrain brain in Ritual.FollowerToAttendSermon)
    {
      float Delay = UnityEngine.Random.Range(0.1f, 0.5f);
      num += Delay;
      ritualUnlockWeapon.StartCoroutine((IEnumerator) ritualUnlockWeapon.DelayFollowerReaction(brain, Delay));
    }
    yield return (object) new WaitForSeconds(2f);
    ritualUnlockWeapon.CompleteRitual();
  }

  public IEnumerator EmitParticles()
  {
    int Loops = 1;
    float Delay = 0.0f;
    while (--Loops >= 0)
    {
      Interaction_TempleAltar.Instance.PulseDisplacementObject(PlayerFarming.Instance.transform.position);
      CameraManager.instance.ShakeCameraForDuration(0.8f, 1f, 0.2f);
      GameManager.GetInstance().CamFollowTarget.targetDistance += 0.2f;
      Delay += 0.1f;
      yield return (object) new WaitForSeconds(0.8f - Delay);
    }
  }

  public IEnumerator SpawnSouls(Vector3 Position)
  {
    float delay = 0.5f;
    float Count = 12f;
    for (int i = 0; (double) i < (double) Count; ++i)
    {
      float num = (float) i / Count;
      SoulCustomTargetLerp.Create(PlayerFarming.Instance.CrownBone.gameObject, Position, 0.5f, Color.red);
      yield return (object) new WaitForSeconds(delay - 0.2f * num);
    }
    --this.NumGivingDevotion;
  }
}
