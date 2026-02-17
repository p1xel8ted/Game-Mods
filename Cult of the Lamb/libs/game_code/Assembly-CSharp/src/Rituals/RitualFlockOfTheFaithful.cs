// Decompiled with JetBrains decompiler
// Type: src.Rituals.RitualFlockOfTheFaithful
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI;
using Spine;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
namespace src.Rituals;

public class RitualFlockOfTheFaithful : Ritual
{
  public Light ritualLight;
  public Follower sacrificeFollower;
  public int NumGivingDevotion;

  public override void Play()
  {
    base.Play();
    this.StartCoroutine((IEnumerator) this.HeartsOfTheFaithfulRitual());
    PlayerFarming.Instance.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
  }

  public void HandleEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (!(e.Data.Name == "sfxTrigger"))
      return;
    AudioManager.Instance.PlayOneShot("event:/hearts_of_the_faithful/player_rise", PlayerFarming.Instance.gameObject);
    PlayerFarming.Instance.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
  }

  public IEnumerator HeartsOfTheFaithfulRitual()
  {
    RitualFlockOfTheFaithful flockOfTheFaithful = this;
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.gameObject, 8f);
    PlayerFarming.Instance.GoToAndStop(ChurchFollowerManager.Instance.RitualCenterPosition.position, GoToCallback: (System.Action) (() =>
    {
      Interaction_TempleAltar.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      PlayerFarming.Instance.simpleSpineAnimator.Animate("build", 0, true);
      PlayerFarming.Instance.state.transform.DOMove(ChurchFollowerManager.Instance.RitualCenterPosition.position, 0.1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    }));
    yield return (object) flockOfTheFaithful.StartCoroutine((IEnumerator) flockOfTheFaithful.WaitFollowersFormCircle());
    yield return (object) new WaitForSeconds(1f);
    UIHeartsOfTheFaithfulChoiceMenuController.Types upgradeType = UIHeartsOfTheFaithfulChoiceMenuController.Types.Hearts;
    UIUpgradePlayerTreeMenuController playerUpgradeMenuInstance = MonoSingleton<UIManager>.Instance.ShowPlayerUpgradeTree();
    UIUpgradePlayerTreeMenuController treeMenuController1 = playerUpgradeMenuInstance;
    treeMenuController1.OnHide = treeMenuController1.OnHide + (System.Action) (() => { });
    UIUpgradePlayerTreeMenuController treeMenuController2 = playerUpgradeMenuInstance;
    treeMenuController2.OnUpgradeUnlocked = treeMenuController2.OnUpgradeUnlocked + (Action<UpgradeSystem.Type>) (type => upgradeType = RitualFlockOfTheFaithful.GetUpgradeType(type));
    UIUpgradePlayerTreeMenuController treeMenuController3 = playerUpgradeMenuInstance;
    treeMenuController3.OnHidden = treeMenuController3.OnHidden + (System.Action) (() => playerUpgradeMenuInstance = (UIUpgradePlayerTreeMenuController) null);
    while ((UnityEngine.Object) playerUpgradeMenuInstance != (UnityEngine.Object) null)
      yield return (object) null;
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.gameObject, 10f);
    PlayerFarming.Instance.Spine.skeleton.FindBone("ritualring").Rotation += 60f;
    PlayerFarming.Instance.Spine.skeleton.UpdateWorldTransform();
    PlayerFarming.Instance.Spine.skeleton.Update(Time.deltaTime);
    PlayerFarming.Instance.simpleSpineAnimator.Animate("rituals/ritual-start", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("rituals/ritual-loop", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(0.5f);
    flockOfTheFaithful.ritualLight.gameObject.SetActive(true);
    ChurchFollowerManager.Instance.StartRitualOverlay();
    yield return (object) new WaitForSeconds(0.5f);
    BiomeConstants.Instance.ChromaticAbberationTween(2f, BiomeConstants.Instance.ChromaticAberrationDefaultValue, 1f);
    BiomeConstants.Instance.VignetteTween(2f, BiomeConstants.Instance.VignetteDefaultValue, 0.7f);
    flockOfTheFaithful.NumGivingDevotion = 0;
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      ++flockOfTheFaithful.NumGivingDevotion;
      if (followerBrain.CurrentTask is FollowerTask_AttendRitual)
        (followerBrain.CurrentTask as FollowerTask_AttendRitual).WorshipTentacle();
      Follower followerById = FollowerManager.FindFollowerByID(followerBrain.Info.ID);
      if ((UnityEngine.Object) followerById != (UnityEngine.Object) null)
        flockOfTheFaithful.StartCoroutine((IEnumerator) flockOfTheFaithful.SpawnSouls(followerById.transform.position));
      yield return (object) new WaitForSeconds(0.1f);
    }
    DOTween.To((DOGetter<float>) (() => GameManager.GetInstance().CamFollowTarget.targetDistance), (DOSetter<float>) (x => GameManager.GetInstance().CamFollowTarget.targetDistance = x), 6f, 5f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InOutSine);
    while (flockOfTheFaithful.NumGivingDevotion > 0)
      yield return (object) null;
    yield return (object) new WaitForSeconds(0.5f);
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      if (followerBrain.CurrentTask is FollowerTask_AttendRitual)
        (followerBrain.CurrentTask as FollowerTask_AttendRitual).Cheer();
    }
    yield return (object) flockOfTheFaithful.StartCoroutine((IEnumerator) flockOfTheFaithful.EmitParticles(upgradeType));
    yield return (object) new WaitForSeconds(0.5f);
    BiomeConstants.Instance.ChromaticAbberationTween(1f, 1f, BiomeConstants.Instance.ChromaticAberrationDefaultValue);
    BiomeConstants.Instance.VignetteTween(1f, 0.7f, BiomeConstants.Instance.VignetteDefaultValue);
    ChurchFollowerManager.Instance.EndRitualOverlay();
    GameManager.GetInstance().CamFollowTarget.targetDistance = 11f;
    PlayerFarming.Instance.simpleSpineAnimator.Animate("rituals/ritual-stop", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    flockOfTheFaithful.ritualLight.gameObject.SetActive(false);
    float num = 0.0f;
    foreach (FollowerBrain brain in Ritual.FollowerToAttendSermon)
    {
      float Delay = UnityEngine.Random.Range(0.1f, 0.5f);
      num += Delay;
      flockOfTheFaithful.StartCoroutine((IEnumerator) flockOfTheFaithful.DelayFollowerReaction(brain, Delay));
    }
    yield return (object) new WaitForSeconds(2f);
    if (!DataManager.Instance.OnboardedLoyalty)
    {
      PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      DataManager.Instance.OnboardedLoyalty = true;
    }
    yield return (object) new WaitForSeconds(0.5f);
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.PerformRitual);
    flockOfTheFaithful.CompleteRitual();
  }

  public IEnumerator EmitParticles(
    UIHeartsOfTheFaithfulChoiceMenuController.Types chosenUpgrade)
  {
    RitualFlockOfTheFaithful flockOfTheFaithful = this;
    int Loops = 1;
    float Delay = 0.0f;
    while (--Loops >= 0)
    {
      Interaction_TempleAltar.Instance.PulseDisplacementObject(PlayerFarming.Instance.transform.position);
      CameraManager.instance.ShakeCameraForDuration(0.8f, 1f, 0.2f);
      GameManager.GetInstance().CamFollowTarget.targetDistance += 0.2f;
      switch (chosenUpgrade)
      {
        case UIHeartsOfTheFaithfulChoiceMenuController.Types.Hearts:
          AudioManager.Instance.PlayOneShot("event:/hearts_of_the_faithful/hearts_appear", flockOfTheFaithful.gameObject);
          AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", flockOfTheFaithful.gameObject.transform.position);
          BiomeConstants.Instance.EmitHeartPickUpVFX(PlayerFarming.Instance.CameraBone.transform.position, 0.0f, "red", "burst_big");
          break;
        case UIHeartsOfTheFaithfulChoiceMenuController.Types.Strength:
          AudioManager.Instance.PlayOneShot("event:/hearts_of_the_faithful/swords_appear", flockOfTheFaithful.gameObject);
          BiomeConstants.Instance.EmitHeartPickUpVFX(PlayerFarming.Instance.CameraBone.transform.position, 0.0f, "strength", "strength");
          break;
      }
      Delay += 0.1f;
      yield return (object) new WaitForSeconds(0.8f - Delay);
    }
  }

  public static UIHeartsOfTheFaithfulChoiceMenuController.Types GetUpgradeType(
    UpgradeSystem.Type type)
  {
    switch (type)
    {
      case UpgradeSystem.Type.PUpgrade_Heart_1:
      case UpgradeSystem.Type.PUpgrade_Heart_2:
      case UpgradeSystem.Type.PUpgrade_Heart_3:
      case UpgradeSystem.Type.PUpgrade_Heart_4:
      case UpgradeSystem.Type.PUpgrade_Heart_5:
      case UpgradeSystem.Type.PUpgrade_Heart_6:
        return UIHeartsOfTheFaithfulChoiceMenuController.Types.Hearts;
      case UpgradeSystem.Type.PUpgrade_Sword_0:
      case UpgradeSystem.Type.PUpgrade_Sword_1:
      case UpgradeSystem.Type.PUpgrade_Sword_2:
      case UpgradeSystem.Type.PUpgrade_Sword_3:
      case UpgradeSystem.Type.PUpgrade_Axe_0:
      case UpgradeSystem.Type.PUpgrade_Axe_1:
      case UpgradeSystem.Type.PUpgrade_Axe_2:
      case UpgradeSystem.Type.PUpgrade_Axe_3:
      case UpgradeSystem.Type.PUpgrade_Dagger_0:
      case UpgradeSystem.Type.PUpgrade_Dagger_1:
      case UpgradeSystem.Type.PUpgrade_Dagger_2:
      case UpgradeSystem.Type.PUpgrade_Dagger_3:
      case UpgradeSystem.Type.PUpgrade_Gauntlets_0:
      case UpgradeSystem.Type.PUpgrade_Gauntlets_1:
      case UpgradeSystem.Type.PUpgrade_Gauntlets_2:
      case UpgradeSystem.Type.PUpgrade_Gauntlets_3:
      case UpgradeSystem.Type.PUpgrade_Hammer_0:
      case UpgradeSystem.Type.PUpgrade_Hammer_1:
      case UpgradeSystem.Type.PUpgrade_Hammer_2:
      case UpgradeSystem.Type.PUpgrade_Hammer_3:
      case UpgradeSystem.Type.PUpgrade_HeavyAttacks:
      case UpgradeSystem.Type.PUpgrade_HA_Axe:
      case UpgradeSystem.Type.PUpgrade_HA_Dagger:
      case UpgradeSystem.Type.PUpgrade_HA_Hammer:
      case UpgradeSystem.Type.PUpgrade_HA_Gauntlets:
      case UpgradeSystem.Type.PUpgrade_HA_Blunderbuss:
      case UpgradeSystem.Type.PUpgrade_HA_Shield:
        return UIHeartsOfTheFaithfulChoiceMenuController.Types.Strength;
      case UpgradeSystem.Type.PUpgrade_Fireball_0:
      case UpgradeSystem.Type.PUpgrade_Fireball_1:
      case UpgradeSystem.Type.PUpgrade_Fireball_2:
      case UpgradeSystem.Type.PUpgrade_Fireball_3:
      case UpgradeSystem.Type.PUpgrade_EnemyBlast_0:
      case UpgradeSystem.Type.PUpgrade_EnemyBlast_1:
      case UpgradeSystem.Type.PUpgrade_EnemyBlast_2:
      case UpgradeSystem.Type.PUpgrade_EnemyBlast_3:
      case UpgradeSystem.Type.PUpgrade_ProjectileAOE_0:
      case UpgradeSystem.Type.PUpgrade_ProjectileAOE_1:
      case UpgradeSystem.Type.PUpgrade_ProjectileAOE_2:
      case UpgradeSystem.Type.PUpgrade_ProjectileAOE_3:
      case UpgradeSystem.Type.PUpgrade_Tentacles_0:
      case UpgradeSystem.Type.PUpgrade_Tentacles_1:
      case UpgradeSystem.Type.PUpgrade_Tentacles_2:
      case UpgradeSystem.Type.PUpgrade_Tentacles_3:
      case UpgradeSystem.Type.PUpgrade_Vortex_0:
      case UpgradeSystem.Type.PUpgrade_Vortex_1:
      case UpgradeSystem.Type.PUpgrade_Vortex_2:
      case UpgradeSystem.Type.PUpgrade_Vortex_3:
      case UpgradeSystem.Type.PUpgrade_MegaSlash_0:
      case UpgradeSystem.Type.PUpgrade_MegaSlash_1:
      case UpgradeSystem.Type.PUpgrade_MegaSlash_2:
      case UpgradeSystem.Type.PUpgrade_MegaSlash_3:
      case UpgradeSystem.Type.PUpgrade_Ammo_1:
      case UpgradeSystem.Type.PUpgrade_Ammo_2:
      case UpgradeSystem.Type.PUpgrade_Ammo_3:
        return UIHeartsOfTheFaithfulChoiceMenuController.Types.Strength;
      default:
        return UIHeartsOfTheFaithfulChoiceMenuController.Types.Hearts;
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
