// Decompiled with JetBrains decompiler
// Type: RitualHeartsOfTheFaithful
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI;
using Spine;
using src.Extensions;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class RitualHeartsOfTheFaithful : Ritual
{
  public Light ritualLight;
  public Follower sacrificeFollower;
  public int NumGivingDevotion;
  public UIHeartsOfTheFaithfulChoiceMenuController _heartsOfTheFaithfulMenuTemplate;

  public void Play(
    UIHeartsOfTheFaithfulChoiceMenuController heartsOfTheFaithfulMenuTemplate)
  {
    this._heartsOfTheFaithfulMenuTemplate = heartsOfTheFaithfulMenuTemplate;
    this.Play();
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
    RitualHeartsOfTheFaithful heartsOfTheFaithful = this;
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.gameObject, 8f);
    PlayerFarming.Instance.GoToAndStop(ChurchFollowerManager.Instance.RitualCenterPosition.position, GoToCallback: (System.Action) (() =>
    {
      Interaction_TempleAltar.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      PlayerFarming.Instance.simpleSpineAnimator.Animate("build", 0, true);
      PlayerFarming.Instance.state.transform.DOMove(ChurchFollowerManager.Instance.RitualCenterPosition.position, 0.1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    }));
    yield return (object) heartsOfTheFaithful.StartCoroutine((IEnumerator) heartsOfTheFaithful.WaitFollowersFormCircle());
    yield return (object) new WaitForSeconds(1f);
    UIHeartsOfTheFaithfulChoiceMenuController.Types chosenUpgrade = UIHeartsOfTheFaithfulChoiceMenuController.Types.Hearts;
    UIHeartsOfTheFaithfulChoiceMenuController heartsOfTheFaithfulMenuInstance = heartsOfTheFaithful._heartsOfTheFaithfulMenuTemplate.Instantiate<UIHeartsOfTheFaithfulChoiceMenuController>();
    heartsOfTheFaithfulMenuInstance.Show();
    heartsOfTheFaithfulMenuInstance.OnChoiceMade += (Action<UIHeartsOfTheFaithfulChoiceMenuController.Types>) (type => chosenUpgrade = type);
    UIHeartsOfTheFaithfulChoiceMenuController choiceMenuController = heartsOfTheFaithfulMenuInstance;
    choiceMenuController.OnHidden = choiceMenuController.OnHidden + (System.Action) (() => heartsOfTheFaithfulMenuInstance = (UIHeartsOfTheFaithfulChoiceMenuController) null);
    while ((UnityEngine.Object) heartsOfTheFaithfulMenuInstance != (UnityEngine.Object) null)
      yield return (object) null;
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.gameObject, 10f);
    PlayerFarming.Instance.Spine.skeleton.FindBone("ritualring").Rotation += 60f;
    PlayerFarming.Instance.Spine.skeleton.UpdateWorldTransform();
    PlayerFarming.Instance.Spine.skeleton.Update(Time.deltaTime);
    PlayerFarming.Instance.simpleSpineAnimator.Animate("rituals/ritual-start", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("rituals/ritual-loop", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(0.5f);
    heartsOfTheFaithful.ritualLight.gameObject.SetActive(true);
    ChurchFollowerManager.Instance.StartRitualOverlay();
    yield return (object) new WaitForSeconds(0.5f);
    BiomeConstants.Instance.ChromaticAbberationTween(2f, BiomeConstants.Instance.ChromaticAberrationDefaultValue, 1f);
    BiomeConstants.Instance.VignetteTween(2f, BiomeConstants.Instance.VignetteDefaultValue, 0.7f);
    heartsOfTheFaithful.NumGivingDevotion = 0;
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      ++heartsOfTheFaithful.NumGivingDevotion;
      if (followerBrain.CurrentTask is FollowerTask_AttendRitual)
        (followerBrain.CurrentTask as FollowerTask_AttendRitual).WorshipTentacle();
      Follower followerById = FollowerManager.FindFollowerByID(followerBrain.Info.ID);
      if ((UnityEngine.Object) followerById != (UnityEngine.Object) null)
        heartsOfTheFaithful.StartCoroutine((IEnumerator) heartsOfTheFaithful.SpawnSouls(followerById.transform.position));
      yield return (object) new WaitForSeconds(0.1f);
    }
    DOTween.To((DOGetter<float>) (() => GameManager.GetInstance().CamFollowTarget.targetDistance), (DOSetter<float>) (x => GameManager.GetInstance().CamFollowTarget.targetDistance = x), 6f, 5f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InOutSine);
    while (heartsOfTheFaithful.NumGivingDevotion > 0)
      yield return (object) null;
    yield return (object) new WaitForSeconds(0.5f);
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      if (followerBrain.CurrentTask is FollowerTask_AttendRitual)
        (followerBrain.CurrentTask as FollowerTask_AttendRitual).Cheer();
    }
    yield return (object) heartsOfTheFaithful.StartCoroutine((IEnumerator) heartsOfTheFaithful.EmitParticles(chosenUpgrade));
    yield return (object) new WaitForSeconds(0.5f);
    BiomeConstants.Instance.ChromaticAbberationTween(1f, 1f, BiomeConstants.Instance.ChromaticAberrationDefaultValue);
    BiomeConstants.Instance.VignetteTween(1f, 0.7f, BiomeConstants.Instance.VignetteDefaultValue);
    ChurchFollowerManager.Instance.EndRitualOverlay();
    GameManager.GetInstance().CamFollowTarget.targetDistance = 11f;
    PlayerFarming.Instance.simpleSpineAnimator.Animate("rituals/ritual-stop", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    heartsOfTheFaithful.ritualLight.gameObject.SetActive(false);
    float num = 0.0f;
    foreach (FollowerBrain brain in Ritual.FollowerToAttendSermon)
    {
      float Delay = UnityEngine.Random.Range(0.1f, 0.5f);
      num += Delay;
      heartsOfTheFaithful.StartCoroutine((IEnumerator) heartsOfTheFaithful.DelayFollowerReaction(brain, Delay));
    }
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.PerformRitual);
    yield return (object) new WaitForSeconds(2f);
    yield return (object) new WaitForSeconds(0.5f);
    switch (chosenUpgrade)
    {
      case UIHeartsOfTheFaithfulChoiceMenuController.Types.Hearts:
        HealthPlayer objectOfType = UnityEngine.Object.FindObjectOfType<HealthPlayer>();
        ++objectOfType.totalHP;
        objectOfType.HP = objectOfType.totalHP;
        ++DataManager.Instance.PLAYER_HEARTS_LEVEL;
        break;
      case UIHeartsOfTheFaithfulChoiceMenuController.Types.Strength:
        ++DataManager.Instance.PLAYER_HEARTS_LEVEL;
        break;
    }
    heartsOfTheFaithful.CompleteRitual();
  }

  public IEnumerator EmitParticles(
    UIHeartsOfTheFaithfulChoiceMenuController.Types chosenUpgrade)
  {
    RitualHeartsOfTheFaithful heartsOfTheFaithful = this;
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
          AudioManager.Instance.PlayOneShot("event:/hearts_of_the_faithful/hearts_appear", heartsOfTheFaithful.gameObject);
          AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", heartsOfTheFaithful.gameObject.transform.position);
          BiomeConstants.Instance.EmitHeartPickUpVFX(PlayerFarming.Instance.CameraBone.transform.position, 0.0f, "red", "burst_big");
          break;
        case UIHeartsOfTheFaithfulChoiceMenuController.Types.Strength:
          AudioManager.Instance.PlayOneShot("event:/hearts_of_the_faithful/swords_appear", heartsOfTheFaithful.gameObject);
          BiomeConstants.Instance.EmitHeartPickUpVFX(PlayerFarming.Instance.CameraBone.transform.position, 0.0f, "strength", "strength");
          break;
      }
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
