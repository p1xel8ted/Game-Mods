// Decompiled with JetBrains decompiler
// Type: Interaction_HalloweenGhost
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_HalloweenGhost : Interaction
{
  public static List<Interaction_HalloweenGhost> HalloweenGhosts = new List<Interaction_HalloweenGhost>();
  public SkeletonAnimation Spine;
  [SerializeField]
  public CritterBee critterBee;
  public FollowerOutfit outfit;
  public FollowerInfo followerInfo;
  public string sString;
  public LayerMask collisionMask;
  public bool destroying;
  public float checkDelayTimestamp;

  public FollowerInfo FollowerInfo => this.followerInfo;

  public void Configure(FollowerInfo follower)
  {
    this.followerInfo = follower;
    this.outfit = new FollowerOutfit(follower);
    follower.Outfit = FollowerOutfitType.Ghost;
    FollowerBrain.SetFollowerCostume(this.Spine.Skeleton, follower, forceUpdate: true);
    this.Spine.Skeleton.A = 0.5f;
    this.Spine.AnimationState.SetAnimation(0, "Ghost/move-ghost", true);
    this.collisionMask = (LayerMask) ((int) this.collisionMask | 1 << LayerMask.NameToLayer("Island"));
    this.collisionMask = (LayerMask) ((int) this.collisionMask | 1 << LayerMask.NameToLayer("Obstacles"));
    this.UpdateLocalisation();
  }

  public override void OnEnable()
  {
    base.OnEnable();
    Interaction_HalloweenGhost.HalloweenGhosts.Add(this);
  }

  public override void OnDisable()
  {
    base.OnDisable();
    Interaction_HalloweenGhost.HalloweenGhosts.Remove(this);
  }

  public override void Update()
  {
    base.Update();
    if (FollowerBrainStats.IsBloodMoon || this.destroying)
      return;
    this.destroying = true;
    DOTween.To((DOGetter<float>) (() => this.Spine.skeleton.A), (DOSetter<float>) (x => this.Spine.skeleton.A = x), 0.0f, 1f).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject)));
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sString = ScriptLocalization.Interactions.Collect;
  }

  public override void GetLabel()
  {
    this.Label = !this.Interactable || this.destroying ? "" : this.sString;
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (this.destroying)
      return;
    this.Interactable = false;
    this.StartCoroutine((IEnumerator) this.CatchGhost());
  }

  public void FixedUpdate()
  {
    if ((double) Time.time <= (double) this.checkDelayTimestamp)
      return;
    foreach (Follower follower in Follower.Followers)
    {
      if ((double) Vector3.Distance(this.transform.position, follower.transform.position) < 2.0 && follower.Brain.CurrentOverrideTaskType == FollowerTaskType.None && follower.Brain.CurrentTaskType != FollowerTaskType.ManualControl && follower.Brain.CurrentTaskType != FollowerTaskType.EnforcerManualControl && (follower.Brain.CurrentTask == null || !follower.Brain.CurrentTask.BlockTaskChanges) && (double) follower.Brain.GetTimeSinceTask(FollowerTaskType.ReactSpookedFromGhost) > 120.0 && (double) TimeManager.TotalElapsedGameTime - (double) StructureAndTime.GetOrAddTime(999, follower.Brain, StructureAndTime.IDTypes.Follower) > 600.0)
      {
        StructureAndTime.SetTime(999, follower.Brain, StructureAndTime.IDTypes.Follower);
        follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ReactGhost());
        follower.FacePosition(this.transform.position);
      }
    }
    this.checkDelayTimestamp = Time.time + 1f;
  }

  public IEnumerator CatchGhost()
  {
    Interaction_HalloweenGhost interactionHalloweenGhost = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionHalloweenGhost.gameObject, 4f);
    Vector3 TargetPosition;
    if ((double) interactionHalloweenGhost.playerFarming.gameObject.transform.position.x < (double) interactionHalloweenGhost.transform.position.x)
    {
      float distance = Vector3.Distance(interactionHalloweenGhost.transform.position, interactionHalloweenGhost.transform.position + Vector3.left);
      Vector3 normalized = (interactionHalloweenGhost.transform.position + Vector3.left - interactionHalloweenGhost.transform.position).normalized;
      TargetPosition = (UnityEngine.Object) Physics2D.Raycast((Vector2) interactionHalloweenGhost.transform.position, (Vector2) normalized, distance, (int) interactionHalloweenGhost.collisionMask).collider != (UnityEngine.Object) null ? interactionHalloweenGhost.transform.position + Vector3.right : interactionHalloweenGhost.transform.position + Vector3.left;
    }
    else
    {
      float distance = Vector3.Distance(interactionHalloweenGhost.transform.position, interactionHalloweenGhost.transform.position + Vector3.right);
      Vector3 normalized = (interactionHalloweenGhost.transform.position + Vector3.right - interactionHalloweenGhost.transform.position).normalized;
      TargetPosition = (UnityEngine.Object) Physics2D.Raycast((Vector2) interactionHalloweenGhost.transform.position, (Vector2) normalized, distance, (int) interactionHalloweenGhost.collisionMask).collider != (UnityEngine.Object) null ? interactionHalloweenGhost.transform.position + Vector3.left : interactionHalloweenGhost.transform.position + Vector3.right;
    }
    interactionHalloweenGhost.playerFarming.playerController.speed = 0.0f;
    interactionHalloweenGhost.playerFarming.GoToAndStop(TargetPosition, interactionHalloweenGhost.gameObject, GoToCallback: new System.Action(interactionHalloweenGhost.\u003CCatchGhost\u003Eb__19_0));
    AudioManager.Instance.PlayOneShot("event:/ritual_sacrifice/start_ritual", interactionHalloweenGhost.transform.position);
    interactionHalloweenGhost.GetComponent<CritterBee>().enabled = false;
    float angle = Utils.GetAngle(interactionHalloweenGhost.playerFarming.transform.position, interactionHalloweenGhost.transform.position);
    interactionHalloweenGhost.transform.localScale = new Vector3((double) angle <= 90.0 || (double) angle >= 270.0 ? -1f : 1f, 1f, 1f);
    interactionHalloweenGhost.Spine.AnimationState.SetAnimation(0, "Ghost/collect", false);
    yield return (object) new WaitForSeconds(3f);
    interactionHalloweenGhost.gameObject.SetActive(false);
    BiomeConstants.Instance.EmitSmokeInteractionVFX(interactionHalloweenGhost.transform.GetChild(0).transform.position, Vector3.one);
    AudioManager.Instance.PlayOneShot("event:/player/catch_firefly", interactionHalloweenGhost.transform.position);
    SeasonalEventData seasonalEventData = SeasonalEventManager.GetSeasonalEventData(SeasonalEventType.Halloween);
    List<string> unlockableSkins = seasonalEventData.GetUnlockableSkins();
    List<StructureBrain.TYPES> unlockableDecorations = seasonalEventData.GetUnlockableDecorations();
    if (unlockableSkins.Count > 0 && ((double) UnityEngine.Random.value > 0.5 || unlockableDecorations.Count == 0))
    {
      string Skin = unlockableSkins[UnityEngine.Random.Range(0, unlockableSkins.Count)];
      FollowerSkinCustomTarget.Create(interactionHalloweenGhost.playerFarming.transform.position, interactionHalloweenGhost.playerFarming.transform.position, 0.0f, Skin, new System.Action(interactionHalloweenGhost.\u003CCatchGhost\u003Eb__19_1));
    }
    else if (unlockableDecorations.Count > 0)
    {
      StructureBrain.TYPES Decoration = unlockableDecorations[UnityEngine.Random.Range(0, unlockableDecorations.Count)];
      DecorationCustomTarget.Create(interactionHalloweenGhost.playerFarming.transform.position, interactionHalloweenGhost.playerFarming.transform.position, 0.0f, Decoration, new System.Action(interactionHalloweenGhost.\u003CCatchGhost\u003Eb__19_2));
    }
    else
    {
      for (int index = 0; index < UnityEngine.Random.Range(3, 6); ++index)
      {
        if ((GameManager.HasUnlockAvailable() ? 1 : (DataManager.Instance.DeathCatBeaten ? 1 : 0)) != 0)
          SoulCustomTarget.Create(interactionHalloweenGhost.state.gameObject, interactionHalloweenGhost.transform.position, Color.white, new System.Action(interactionHalloweenGhost.\u003CCatchGhost\u003Eb__19_3));
        else
          InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1, interactionHalloweenGhost.transform.position + Vector3.back, 0.0f).SetInitialSpeedAndDiraction(8f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
      }
      GameManager.GetInstance().OnConversationEnd();
      UnityEngine.Object.Destroy((UnityEngine.Object) interactionHalloweenGhost.gameObject);
    }
  }

  [CompilerGenerated]
  public float \u003CUpdate\u003Eb__14_0() => this.Spine.skeleton.A;

  [CompilerGenerated]
  public void \u003CUpdate\u003Eb__14_1(float x) => this.Spine.skeleton.A = x;

  [CompilerGenerated]
  public void \u003CUpdate\u003Eb__14_2() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);

  [CompilerGenerated]
  public void \u003CCatchGhost\u003Eb__19_0()
  {
    this.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    this.playerFarming.simpleSpineAnimator.Animate("reeducate-3", 0, false);
    this.playerFarming.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
  }

  [CompilerGenerated]
  public void \u003CCatchGhost\u003Eb__19_1()
  {
    GameManager.GetInstance().OnConversationEnd();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  [CompilerGenerated]
  public void \u003CCatchGhost\u003Eb__19_2()
  {
    GameManager.GetInstance().OnConversationEnd();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  [CompilerGenerated]
  public void \u003CCatchGhost\u003Eb__19_3() => this.playerFarming.GetSoul(1);
}
