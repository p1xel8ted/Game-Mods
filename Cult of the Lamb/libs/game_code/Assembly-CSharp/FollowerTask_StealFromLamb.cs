// Decompiled with JetBrains decompiler
// Type: FollowerTask_StealFromLamb
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FollowerTask_StealFromLamb : FollowerTask
{
  public StateMachine.State prevState;

  public override FollowerTaskType Type => FollowerTaskType.StealFromPlayer;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockReactTasks => true;

  public override bool BlockTaskChanges => true;

  public override bool BlockSocial => true;

  public override int GetSubTaskCode() => 0;

  public override void TaskTick(float deltaGameTime)
  {
    if (!((Object) PlayerFarming.Instance == (Object) null))
      return;
    this.End();
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    this.prevState = PlayerFarming.Instance.state.CURRENT_STATE;
    this.LambHitSequence(follower);
    follower.StartCoroutine(this.StealGoldIE(follower));
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    return PlayerFarming.Instance.transform.position;
  }

  public IEnumerator StealGoldIE(Follower follower)
  {
    FollowerTask_StealFromLamb taskStealFromLamb = this;
    follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForEndOfFrame();
    follower.FacePosition(PlayerFarming.Instance.transform.position);
    double num = (double) follower.SetBodyAnimation("steal-from-player", false);
    follower.AddBodyAnimation("Reactions/react-laugh", false, 0.0f);
    follower.AddBodyAnimation("idle", true, 0.0f);
    yield return (object) new WaitForSeconds(0.16f);
    for (int i = 0; i < 5; ++i)
    {
      PickUp pickUp = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1, PlayerFarming.Instance.transform.position, 0.0f);
      pickUp.SetInitialSpeedAndDiraction(5f, (float) Random.Range(0, 360));
      pickUp.Player = follower.gameObject;
      pickUp.MagnetToPlayer = true;
      pickUp.AddToInventory = false;
      yield return (object) new WaitForSeconds(0.05f);
    }
    int Quantity = Mathf.CeilToInt((float) Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.BLACK_GOLD) * 0.1f);
    Inventory.ChangeItemQuantity(InventoryItem.ITEM_TYPE.BLACK_GOLD, -Quantity);
    if (taskStealFromLamb.Brain.Info.ID == 100006)
      DataManager.Instance.MidasStolenGold.Add(new InventoryItem(InventoryItem.ITEM_TYPE.BLACK_GOLD, Quantity));
    NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("Notifications/StoleGold", follower.Brain.Info.Name, Quantity.ToString());
    yield return (object) new WaitForSeconds(2f);
    taskStealFromLamb.End();
  }

  public void LambHitSequence(Follower follower)
  {
    PlayerFarming instance = PlayerFarming.Instance;
    if (follower.Brain.Info.ID == 100006)
      AudioManager.Instance.PlayOneShot("event:/dlc/env/midas/steal_from_player", instance.transform.position);
    else
      AudioManager.Instance.PlayOneShot("event:/player/gethit", instance.transform.position);
    BiomeConstants.Instance.EmitHitVFX(instance.transform.position, Quaternion.identity.z, "HitFX_Blocked");
    instance.simpleSpineAnimator.FlashWhite(false);
    CameraManager.instance.ShakeCameraForDuration(1.5f, 1.7f, 0.2f);
    instance.state.facingAngle = Utils.GetAngle(PlayerFarming.Instance.transform.position, follower.transform.position);
    instance.state.CURRENT_STATE = StateMachine.State.HitThrown;
    instance.playerController.MakeUntouchable(1f, false);
    instance.transform.DOMoveX(instance.transform.position.x, 0.4f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => PlayerFarming.Instance.state.CURRENT_STATE = this.prevState));
  }

  [CompilerGenerated]
  public void \u003CLambHitSequence\u003Eb__16_0()
  {
    PlayerFarming.Instance.state.CURRENT_STATE = this.prevState;
  }
}
