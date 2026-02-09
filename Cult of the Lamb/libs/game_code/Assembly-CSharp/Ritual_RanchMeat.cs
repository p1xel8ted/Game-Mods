// Decompiled with JetBrains decompiler
// Type: Ritual_RanchMeat
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Ritual_RanchMeat : Ritual
{
  public static System.Action OnRitualBegun;
  public Vector3 averagePosition;

  public override UpgradeSystem.Type RitualType => UpgradeSystem.Type.Ritual_RanchMeat;

  public override void Play()
  {
    base.Play();
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.RitualRoutine());
  }

  public IEnumerator RitualRoutine()
  {
    Ritual_RanchMeat ritualRanchMeat = this;
    AudioManager.Instance.PlayOneShot("event:/dlc/ritual/gorging_start");
    AudioManager.Instance.PlayOneShot("event:/rituals/generic_start_ritual");
    Interaction_TempleAltar.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    PlayerFarming.Instance.simpleSpineAnimator.Animate("idle", 0, true);
    Interaction_TempleAltar.Instance.SimpleSetCamera.Play();
    yield return (object) ritualRanchMeat.StartCoroutine((IEnumerator) ritualRanchMeat.WaitFollowersFormCircle());
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
    AudioManager.Instance.PlayOneShot("event:/Stings/harvest");
    List<FollowerBrain> followerBrainList = new List<FollowerBrain>((IEnumerable<FollowerBrain>) Ritual.GetFollowersAvailableToAttendSermon());
    int waiting = 0;
    List<Follower> followers = new List<Follower>();
    for (int index = 0; index < 2 && followerBrainList.Count != 0; ++index)
    {
      FollowerBrain followerBrain = followerBrainList[UnityEngine.Random.Range(0, followerBrainList.Count)];
      followerBrainList.Remove(followerBrain);
      Follower followerById = FollowerManager.FindFollowerByID(followerBrain.Info.ID);
      if ((bool) (UnityEngine.Object) followerById)
      {
        followers.Add(followerById);
        ritualRanchMeat.StartCoroutine((IEnumerator) ritualRanchMeat.MoveFollower(followerById, index, (System.Action) (() => ++waiting)));
      }
    }
    foreach (FollowerBrain followerBrain in followerBrainList)
    {
      if (followerBrain.CurrentTask is FollowerTask_AttendRitual)
        ((FollowerTask_AttendRitual) followerBrain.CurrentTask).Pray2();
    }
    while (waiting < 2)
      yield return (object) null;
    StructuresData.Ranchable_Animal animal = new StructuresData.Ranchable_Animal()
    {
      State = Interaction_Ranchable.State.Animating,
      Type = InventoryItem.ITEM_TYPE.ANIMAL_GOAT,
      Colour = UnityEngine.Random.Range(0, 3)
    };
    animal.Ears = animal.Head = animal.Horns = UnityEngine.Random.Range(1, 5);
    animal.Age = 30;
    animal.ID = -1;
    Interaction_Ranchable ranchAnimal = (Interaction_Ranchable) null;
    BiomeBaseManager.Instance.SpawnAnimal(animal, ChurchFollowerManager.Instance.RitualCenterPosition.position, false, (Interaction_Ranch) null, (Transform) null, (Action<Interaction_Ranchable>) (ranchable =>
    {
      ranchable.ClearCurrentPath();
      ranchable.enabled = false;
      ranchable.CurrentState = Interaction_Ranchable.State.Animating;
      ranchAnimal = ranchable;
      AudioManager.Instance.PlayOneShot("event:/dlc/ritual/gorging_animal_appear");
      ranchable.Spine.AnimationState.SetAnimation(0, "appear", false);
      ranchable.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
      if ((double) ranchable.transform.position.x <= 0.0)
        return;
      ranchable.transform.localScale = new Vector3(-1f, 1f, 1f);
    }));
    yield return (object) new WaitForSeconds(1f);
    waiting = 0;
    for (int index = 0; index < 2; ++index)
      ritualRanchMeat.StartCoroutine((IEnumerator) ritualRanchMeat.Harvest(followers[index], index, (System.Action) (() => ++waiting)));
    yield return (object) new WaitForSeconds(2f);
    ranchAnimal.Die(false);
    yield return (object) new WaitForSeconds(1f);
    while (waiting < 2)
      yield return (object) null;
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      if (followerBrain != null && followerBrain.CurrentTask is FollowerTask_AttendRitual)
        (followerBrain.CurrentTask as FollowerTask_AttendRitual).Cheer();
    }
    yield return (object) new WaitForSeconds(2f);
    UnityEngine.Object.Destroy((UnityEngine.Object) ranchAnimal.gameObject);
    BiomeConstants.Instance.EmitSmokeExplosionVFX(ranchAnimal.transform.position);
    yield return (object) new WaitForSeconds(1f);
    PlayerFarming.Instance.simpleSpineAnimator.Animate("rituals/ritual-stop", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    BiomeConstants.Instance.ChromaticAbberationTween(1f, 1f, BiomeConstants.Instance.ChromaticAberrationDefaultValue);
    BiomeConstants.Instance.VignetteTween(1f, 0.7f, BiomeConstants.Instance.VignetteDefaultValue);
    ChurchFollowerManager.Instance.EndRitualOverlay();
    GameManager.GetInstance().CamFollowTarget.targetDistance = 11f;
    Interaction_TempleAltar.Instance.RitualLighting.gameObject.SetActive(false);
    float num = 0.0f;
    foreach (FollowerBrain brain in Ritual.FollowerToAttendSermon)
    {
      if (brain != null)
      {
        float Delay = UnityEngine.Random.Range(0.1f, 0.5f);
        num += Delay;
        ritualRanchMeat.StartCoroutine((IEnumerator) ritualRanchMeat.DelayFollowerReaction(brain, Delay));
        brain.AddThought(Thought.RanchMeat);
      }
    }
    yield return (object) new WaitForSeconds(1.5f);
    DataManager.Instance.LastRanchRitualMeat = TimeManager.TotalElapsedGameTime;
    CultFaithManager.AddThought(Thought.Cult_RanchMeat);
    Interaction_TempleAltar.Instance.CloseUpCamera.Reset();
    System.Action onRitualBegun = Ritual_RanchMeat.OnRitualBegun;
    if (onRitualBegun != null)
      onRitualBegun();
    ritualRanchMeat.CompleteRitual();
  }

  public IEnumerator MoveFollower(Follower follower, int index, System.Action callback)
  {
    Ritual_RanchMeat ritualRanchMeat = this;
    List<Vector3> positions = new List<Vector3>()
    {
      Vector3.left,
      Vector3.right
    };
    bool waiting = true;
    follower.HoodOff(onComplete: (System.Action) (() => waiting = false));
    while (waiting)
      yield return (object) null;
    FollowerBrain.SetFollowerCostume(follower.Spine.Skeleton, follower.Brain._directInfoAccess.XPLevel, follower.Brain._directInfoAccess.SkinName, follower.Brain._directInfoAccess.SkinColour, follower.Brain._directInfoAccess.Outfit, FollowerHatType.Ranch, follower.Brain._directInfoAccess.Clothing, follower.Brain._directInfoAccess.Customisation, follower.Brain._directInfoAccess.Special, follower.Brain._directInfoAccess.Necklace, follower.Brain._directInfoAccess.ClothingVariant, follower.Brain._directInfoAccess);
    yield return (object) ritualRanchMeat.StartCoroutine((IEnumerator) follower.GoToRoutine(ChurchFollowerManager.Instance.RitualCenterPosition.position + positions[index]));
    follower.FacePosition(ChurchFollowerManager.Instance.RitualCenterPosition.position);
    System.Action action = callback;
    if (action != null)
      action();
  }

  public IEnumerator Harvest(Follower follower, int index, System.Action callback)
  {
    Ritual_RanchMeat ritualRanchMeat = this;
    List<Vector3> positions = new List<Vector3>()
    {
      Vector3.left,
      Vector3.right
    };
    string[] anims = new string[2]
    {
      "action",
      "Ranching/add-wool"
    };
    yield return (object) ritualRanchMeat.StartCoroutine((IEnumerator) follower.GoToRoutine(ChurchFollowerManager.Instance.RitualCenterPosition.position + positions[index] * 0.5f));
    AudioManager.Instance.PlayOneShot("event:/dlc/ritual/gorging_butcher");
    follower.State.facingAngle = Utils.GetAngle(follower.transform.position, PlayerFarming.Instance.transform.position);
    double num = (double) follower.SetBodyAnimation(anims[0], true);
    ritualRanchMeat.StartCoroutine((IEnumerator) ritualRanchMeat.SpawnItem(InventoryItem.ITEM_TYPE.MEAT, 5, new Vector2(0.4f, 0.5f), follower.transform.position));
    yield return (object) new WaitForSeconds(3f);
    yield return (object) ritualRanchMeat.StartCoroutine((IEnumerator) follower.GoToRoutine(ChurchFollowerManager.Instance.RitualCenterPosition.position + positions[index]));
    follower.FacePosition(ChurchFollowerManager.Instance.RitualCenterPosition.position);
    System.Action action = callback;
    if (action != null)
      action();
  }

  public IEnumerator SpawnItem(
    InventoryItem.ITEM_TYPE item,
    int amount,
    Vector2 timeBetween,
    Vector3 pos)
  {
    Ritual_RanchMeat ritualRanchMeat = this;
    yield return (object) new WaitForSeconds(UnityEngine.Random.Range(timeBetween.x, timeBetween.y));
    for (int i = 0; i < amount; ++i)
    {
      PickUp component = InventoryItem.Spawn(item, 1, pos - Vector3.forward).GetComponent<PickUp>();
      component.SetInitialSpeedAndDiraction(4f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
      component.MagnetToPlayer = false;
      ritualRanchMeat.StartCoroutine((IEnumerator) ritualRanchMeat.DelayedCollect(component));
      yield return (object) new WaitForSeconds(UnityEngine.Random.Range(timeBetween.x, timeBetween.y));
    }
  }

  public IEnumerator DelayedCollect(PickUp g)
  {
    yield return (object) new WaitForSeconds(4f);
    g.PickMeUp();
    g.MagnetToPlayer = true;
  }
}
