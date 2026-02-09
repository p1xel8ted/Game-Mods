// Decompiled with JetBrains decompiler
// Type: FollowerTask_StealFromFollowers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class FollowerTask_StealFromFollowers : FollowerTask
{
  public Follower thief;
  public Follower targetFollower;
  public FollowerBrain targetFollowerBrain;
  public Coroutine stealCoroutine;
  public float _maxDistance;
  public float chaseForDuration;
  public float chaseProgress;
  public float updateDestination;
  public bool isSneaking;
  public InventoryItem.ITEM_TYPE[] possibleItems = new InventoryItem.ITEM_TYPE[7]
  {
    InventoryItem.ITEM_TYPE.BONE,
    InventoryItem.ITEM_TYPE.GRASS,
    InventoryItem.ITEM_TYPE.BERRY,
    InventoryItem.ITEM_TYPE.POOP,
    InventoryItem.ITEM_TYPE.FISH,
    InventoryItem.ITEM_TYPE.SEED,
    InventoryItem.ITEM_TYPE.SHELL
  };

  public override FollowerTaskType Type => FollowerTaskType.StealFromFollowers;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockReactTasks => true;

  public override bool BlockTaskChanges => true;

  public override bool BlockSocial => true;

  public FollowerTask_StealFromFollowers(
    Follower thiefFollower,
    FollowerBrain targetBrain,
    float maxDistance)
  {
    this.thief = thiefFollower;
    this.targetFollowerBrain = targetBrain;
    this.targetFollower = FollowerManager.FindFollowerByID(this.targetFollowerBrain.Info.ID);
    this._maxDistance = maxDistance;
    this.chaseForDuration = UnityEngine.Random.Range(120f, 240f);
  }

  public override int GetSubTaskCode() => 0;

  public override void OnStart()
  {
    if (this.targetFollowerBrain != null)
    {
      this.SetState(FollowerTaskState.GoingTo);
      if (!FollowerBrainStats.ShouldWork || !this._brain.CanWork || !(bool) (UnityEngine.Object) this.thief || !((UnityEngine.Object) this.thief.gameObject.GetComponent<Interaction_BackToWork>() == (UnityEngine.Object) null))
        return;
      Interaction_BackToWork interactionBackToWork = this.thief.gameObject.AddComponent<Interaction_BackToWork>();
      interactionBackToWork.Init(this.thief);
      interactionBackToWork.LockPosition = this.thief.transform;
    }
    else
      this.End();
  }

  public override void TaskTick(float deltaGameTime)
  {
    if (this.State == FollowerTaskState.Doing)
      return;
    this.updateDestination -= deltaGameTime;
    if ((double) this.updateDestination <= 0.0)
    {
      this.RecalculateDestination();
      this.updateDestination = 1.5f;
    }
    this.chaseProgress += deltaGameTime;
    if ((double) this.chaseProgress > (double) this.chaseForDuration && this.State == FollowerTaskState.GoingTo || (double) Vector3.Distance(this.Brain.LastPosition, this.targetFollowerBrain.LastPosition) > (double) this._maxDistance)
      this.EndInAnger();
    if ((double) Vector3.Distance(this.Brain.LastPosition, this.targetFollowerBrain.LastPosition) < 5.0 && !this.isSneaking)
    {
      this.isSneaking = true;
      if ((UnityEngine.Object) this.thief != (UnityEngine.Object) null)
      {
        this.thief.SpeedMultiplier = 0.25f;
        this.thief.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "Prison/Unlawful/sneak");
        this.SetState(FollowerTaskState.Idle);
        this.ClearDestination();
        this.SetState(FollowerTaskState.GoingTo);
      }
    }
    if ((double) Vector3.Distance(this.Brain.LastPosition, this.targetFollowerBrain.LastPosition) >= 0.5 || this.State != FollowerTaskState.GoingTo)
      return;
    this.SetState(FollowerTaskState.Doing);
    this.ClearDestination();
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    if (!((UnityEngine.Object) this.targetFollower != (UnityEngine.Object) null))
      return Vector3.zero;
    return this.targetFollowerBrain != null && (UnityEngine.Object) this.thief != (UnityEngine.Object) null ? this.targetFollower.transform.position + (this.targetFollower.transform.position - this.thief.transform.position).normalized : this.targetFollowerBrain.LastPosition;
  }

  public override void OnDoingBegin(Follower follower)
  {
    base.OnDoingBegin(follower);
    if (this.stealCoroutine != null)
      return;
    if ((UnityEngine.Object) this.thief == (UnityEngine.Object) null)
      this.thief = follower;
    if ((UnityEngine.Object) this.thief != (UnityEngine.Object) null)
      this.stealCoroutine = this.thief.StartCoroutine((IEnumerator) this.StealGoldIE());
    else
      this.End();
  }

  public IEnumerator StealGoldIE()
  {
    FollowerTask_StealFromFollowers stealFromFollowers = this;
    stealFromFollowers.thief.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForEndOfFrame();
    stealFromFollowers.thief.FacePosition(stealFromFollowers.targetFollowerBrain.LastPosition);
    double num = (double) stealFromFollowers.thief.SetBodyAnimation("steal-from-player", false);
    stealFromFollowers.thief.AddBodyAnimation("Reactions/react-laugh", false, 0.0f);
    stealFromFollowers.thief.AddBodyAnimation("idle", true, 0.0f);
    yield return (object) new WaitForSeconds(0.16f);
    int itemCount = UnityEngine.Random.Range(1, 6);
    for (int i = 0; i < itemCount; ++i)
    {
      PickUp pickUp = InventoryItem.Spawn(stealFromFollowers.GetRandomStolenItem(), 1, stealFromFollowers.targetFollowerBrain.LastPosition, 0.0f);
      if ((UnityEngine.Object) pickUp != (UnityEngine.Object) null)
      {
        pickUp.Player = stealFromFollowers.thief.gameObject;
        pickUp.SetInitialSpeedAndDiraction(5f, (float) UnityEngine.Random.Range(0, 360));
        pickUp.MagnetToPlayer = true;
        pickUp.AddToInventory = false;
        yield return (object) new WaitForSeconds(0.05f);
      }
    }
    yield return (object) new WaitForSeconds(2f);
    stealFromFollowers.End();
  }

  public void EndInAnger()
  {
    this.SetState(FollowerTaskState.Doing);
    if (!((UnityEngine.Object) this.thief != (UnityEngine.Object) null))
      return;
    this.thief.TimedAnimation("Conversations/react-hate" + UnityEngine.Random.Range(1, 4).ToString(), 2f, new System.Action(((FollowerTask) this).End));
  }

  public override void Cleanup(Follower follower)
  {
    if (this.stealCoroutine != null)
    {
      follower.StopCoroutine(this.stealCoroutine);
      this.stealCoroutine = (Coroutine) null;
    }
    if ((bool) (UnityEngine.Object) follower.GetComponent<Interaction_BackToWork>())
      UnityEngine.Object.Destroy((UnityEngine.Object) follower.GetComponent<Interaction_BackToWork>());
    this.UndoStateAnimationChanges(follower);
    if (!((UnityEngine.Object) this.thief != (UnityEngine.Object) null))
      return;
    this.thief.SpeedMultiplier = 1f;
  }

  public void UndoStateAnimationChanges(Follower follower)
  {
    SimpleSpineAnimator.SpineChartacterAnimationData animationData = follower.SimpleAnimator.GetAnimationData(StateMachine.State.Moving);
    animationData.Animation = animationData.DefaultAnimation;
    follower.ResetStateAnimations();
  }

  public InventoryItem.ITEM_TYPE GetRandomStolenItem()
  {
    return this.possibleItems[UnityEngine.Random.Range(0, this.possibleItems.Length)];
  }
}
