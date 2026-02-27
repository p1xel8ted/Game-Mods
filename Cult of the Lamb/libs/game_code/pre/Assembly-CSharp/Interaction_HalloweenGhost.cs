// Decompiled with JetBrains decompiler
// Type: Interaction_HalloweenGhost
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_HalloweenGhost : Interaction
{
  public static List<Interaction_HalloweenGhost> HalloweenGhosts = new List<Interaction_HalloweenGhost>();
  public SkeletonAnimation Spine;
  [SerializeField]
  private CritterBee critterBee;
  private FollowerOutfit outfit;
  private FollowerInfo followerInfo;
  private string sString;
  private LayerMask collisionMask;
  private bool destroying;
  private float checkDelayTimestamp;

  public FollowerInfo FollowerInfo => this.followerInfo;

  public void Configure(FollowerInfo follower)
  {
    this.followerInfo = follower;
    this.outfit = new FollowerOutfit(follower);
    this.outfit.SetOutfit(this.Spine, FollowerOutfitType.Ghost, InventoryItem.ITEM_TYPE.NONE, false);
    this.Spine.Skeleton.A = 0.5f;
    this.Spine.AnimationState.SetAnimation(0, "Ghost/move-ghost", true);
    this.collisionMask = (LayerMask) ((int) this.collisionMask | 1 << LayerMask.NameToLayer("Island"));
    this.collisionMask = (LayerMask) ((int) this.collisionMask | 1 << LayerMask.NameToLayer("Obstacles"));
    this.UpdateLocalisation();
  }

  protected override void OnEnable()
  {
    base.OnEnable();
    Interaction_HalloweenGhost.HalloweenGhosts.Add(this);
  }

  protected override void OnDisable()
  {
    base.OnDisable();
    Interaction_HalloweenGhost.HalloweenGhosts.Remove(this);
  }

  protected override void Update()
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

  private void FixedUpdate()
  {
    if ((double) Time.time <= (double) this.checkDelayTimestamp)
      return;
    foreach (Follower follower in Follower.Followers)
    {
      if ((double) Vector3.Distance(this.transform.position, follower.transform.position) < 2.0)
      {
        if (follower.Brain.CurrentOverrideTaskType != FollowerTaskType.None)
          return;
        if ((double) follower.Brain.GetTimeSinceTask(FollowerTaskType.ReactSpookedFromGhost) > 120.0 && (double) TimeManager.TotalElapsedGameTime - (double) StructureAndTime.GetOrAddTime(999, follower.Brain, StructureAndTime.IDTypes.Follower) > 600.0)
        {
          StructureAndTime.SetTime(999, follower.Brain, StructureAndTime.IDTypes.Follower);
          follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ReactGhost());
          follower.FacePosition(this.transform.position);
        }
      }
    }
    this.checkDelayTimestamp = Time.time + 1f;
  }

  private IEnumerator CatchGhost()
  {
    Interaction_HalloweenGhost interactionHalloweenGhost = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionHalloweenGhost.gameObject, 4f);
    Vector3 TargetPosition;
    if ((double) PlayerFarming.Instance.gameObject.transform.position.x < (double) interactionHalloweenGhost.transform.position.x)
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
    PlayerFarming.Instance.playerController.speed = 0.0f;
    PlayerFarming.Instance.GoToAndStop(TargetPosition, interactionHalloweenGhost.gameObject, GoToCallback: (System.Action) (() =>
    {
      PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      PlayerFarming.Instance.simpleSpineAnimator.Animate("reeducate-3", 0, false);
      PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    }));
    AudioManager.Instance.PlayOneShot("event:/ritual_sacrifice/start_ritual", interactionHalloweenGhost.transform.position);
    interactionHalloweenGhost.GetComponent<CritterBee>().enabled = false;
    float num = Mathf.Repeat(Utils.GetAngle(PlayerFarming.Instance.transform.position, interactionHalloweenGhost.transform.position), 360f);
    interactionHalloweenGhost.transform.localScale = new Vector3((double) num <= 90.0 || (double) num >= 270.0 ? -1f : 1f, 1f, 1f);
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
      // ISSUE: reference to a compiler-generated method
      FollowerSkinCustomTarget.Create(PlayerFarming.Instance.transform.position, PlayerFarming.Instance.transform.position, 0.0f, Skin, new System.Action(interactionHalloweenGhost.\u003CCatchGhost\u003Eb__19_1));
    }
    else if (unlockableDecorations.Count > 0)
    {
      StructureBrain.TYPES Decoration = unlockableDecorations[UnityEngine.Random.Range(0, unlockableDecorations.Count)];
      // ISSUE: reference to a compiler-generated method
      DecorationCustomTarget.Create(PlayerFarming.Instance.transform.position, PlayerFarming.Instance.transform.position, 0.0f, Decoration, new System.Action(interactionHalloweenGhost.\u003CCatchGhost\u003Eb__19_2));
    }
    else
    {
      for (int index = 0; index < UnityEngine.Random.Range(3, 6); ++index)
      {
        if (GameManager.HasUnlockAvailable())
          SoulCustomTarget.Create(interactionHalloweenGhost.state.gameObject, interactionHalloweenGhost.transform.position, Color.white, (System.Action) (() => PlayerFarming.Instance.GetSoul(1)));
        else
          InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1, interactionHalloweenGhost.transform.position + Vector3.back, 0.0f).SetInitialSpeedAndDiraction(8f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
      }
      GameManager.GetInstance().OnConversationEnd();
      UnityEngine.Object.Destroy((UnityEngine.Object) interactionHalloweenGhost.gameObject);
    }
  }
}
