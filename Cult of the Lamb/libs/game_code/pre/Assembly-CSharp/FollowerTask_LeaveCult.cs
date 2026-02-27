// Decompiled with JetBrains decompiler
// Type: FollowerTask_LeaveCult
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class FollowerTask_LeaveCult : FollowerTask
{
  private NotificationCentre.NotificationType _leaveNotificationType;

  public override FollowerTaskType Type => FollowerTaskType.LeaveCult;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool DisablePickUpInteraction => true;

  public override bool BlockTaskChanges => true;

  public FollowerTask_LeaveCult(
    NotificationCentre.NotificationType leaveNotificationType)
  {
    this._leaveNotificationType = leaveNotificationType;
  }

  protected override int GetSubTaskCode() => 0;

  protected override void OnStart()
  {
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    follower.Interaction_FollowerInteraction.Interactable = false;
    follower.TimedAnimation("tantrum-big", 6f, (System.Action) (() =>
    {
      // ISSUE: reference to a compiler-generated method
      this.\u003C\u003En__0(follower);
      this.SetState(FollowerTaskState.GoingTo);
    }));
  }

  public override void OnGoingToBegin(Follower follower)
  {
    base.OnGoingToBegin(follower);
    follower.Interaction_FollowerInteraction.Interactable = false;
  }

  public override void SimSetup(SimFollower simFollower)
  {
    base.SimSetup(simFollower);
    this.SetState(FollowerTaskState.GoingTo);
  }

  protected override void TaskTick(float deltaGameTime)
  {
  }

  protected override void OnArrive()
  {
    base.OnArrive();
    Follower follower = FollowerManager.FindFollowerByID(this._brain.Info.ID);
    if ((UnityEngine.Object) follower != (UnityEngine.Object) null)
      follower.TimedAnimation("wave-angry", 1.93333328f, (System.Action) (() => follower.StartCoroutine((IEnumerator) this.FrameDelay((System.Action) (() => follower.TimedAnimation("spawn-out-angry", 0.8333333f, (System.Action) (() =>
      {
        Inventory.ChangeItemQuantity(20, -Mathf.Min(Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.BLACK_GOLD), Mathf.FloorToInt(this._brain.Stats.DissentGold)));
        this.End();
      })))))));
    else
      this.End();
  }

  private IEnumerator FrameDelay(System.Action callback)
  {
    yield return (object) new WaitForEndOfFrame();
    System.Action action = callback;
    if (action != null)
      action();
  }

  protected override Vector3 UpdateDestination(Follower follower)
  {
    return (UnityEngine.Object) follower != (UnityEngine.Object) null ? BaseLocationManager.Instance.GetExitPosition(FollowerLocation.Lumberjack) : Vector3.zero;
  }

  public override void OnFinaliseEnd(Follower follower)
  {
    follower.Brain.LeftCult = true;
    follower.Brain.LeftCultWithReason = this._leaveNotificationType;
  }

  public override void SimFinaliseEnd(SimFollower simFollower)
  {
    simFollower.Brain.LeftCult = true;
    simFollower.Brain.LeftCultWithReason = this._leaveNotificationType;
  }
}
