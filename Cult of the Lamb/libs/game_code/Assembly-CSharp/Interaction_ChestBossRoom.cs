// Decompiled with JetBrains decompiler
// Type: Interaction_ChestBossRoom
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_ChestBossRoom : Interaction
{
  public bool StartRevealed;
  public InventoryItem.ITEM_TYPE ItemType = InventoryItem.ITEM_TYPE.LOG;
  public InventoryItemDisplay Item;
  public GameObject PlayerPosition;
  public SkeletonAnimation Spine;
  public string sLabel;
  public float Timer;
  public Interaction_ChestBossRoom.State MyState;
  public int MaxToGive = 12;
  public int MinToGive = 8;
  public static Interaction_Chest.ChestEvent OnChestRevealed;
  public InventoryItem.ITEM_TYPE Reward;
  public HealthPlayer healthPlayer;
  public static int RewardCount = -1;
  public static int RevealCount = -1;
  public bool Loop;

  public override void Update()
  {
    base.Update();
    if (this.MyState != Interaction_ChestBossRoom.State.Closed)
      return;
    this.Timer += Time.deltaTime;
  }

  public void Start()
  {
    this.Item.gameObject.SetActive(false);
    this.UpdateLocalisation();
    if (this.MyState == Interaction_ChestBossRoom.State.Hidden)
      this.Spine.gameObject.SetActive(false);
    else
      this.Spine.AnimationState.SetAnimation(0, "closed", true);
    if (!this.StartRevealed)
      return;
    this.Reveal();
  }

  public void Reveal()
  {
    this.Spine.gameObject.SetActive(true);
    this.Spine.AnimationState.SetAnimation(0, "reveal", true);
    this.Spine.AnimationState.AddAnimation(0, "closed", true, 0.0f);
    this.MyState = Interaction_ChestBossRoom.State.Closed;
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sLabel = ScriptLocalization.Interactions.OpenChest;
  }

  public override void GetLabel()
  {
    this.Label = this.MyState != Interaction_ChestBossRoom.State.Closed || (double) this.Timer <= 0.5 ? "" : this.sLabel;
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.MyState != Interaction_ChestBossRoom.State.Closed)
      return;
    base.OnInteract(state);
    this.StartCoroutine(this.InteractionRoutine());
  }

  public IEnumerator InteractionRoutine()
  {
    Interaction_ChestBossRoom interactionChestBossRoom = this;
    interactionChestBossRoom.Loop = false;
    GameManager.GetInstance().OnConversationNew(false);
    GameManager.GetInstance().OnConversationNext(interactionChestBossRoom.state.gameObject, 6f);
    interactionChestBossRoom.playerFarming.GoToAndStop(interactionChestBossRoom.PlayerPosition, interactionChestBossRoom.gameObject);
    while (interactionChestBossRoom.playerFarming.GoToAndStopping)
      yield return (object) null;
    yield return (object) new WaitForSeconds(0.2f);
    interactionChestBossRoom.Spine.AnimationState.SetAnimation(0, "open", false);
    interactionChestBossRoom.Spine.AnimationState.AddAnimation(0, "opened", true, 0.0f);
    interactionChestBossRoom.MyState = Interaction_ChestBossRoom.State.Open;
    for (int i = 0; i < Random.Range(interactionChestBossRoom.MinToGive, interactionChestBossRoom.MaxToGive); ++i)
    {
      InventoryItem.Spawn(interactionChestBossRoom.ItemType, 1, interactionChestBossRoom.transform.position);
      yield return (object) new WaitForSeconds(0.1f);
    }
    InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1, interactionChestBossRoom.transform.position);
    switch (PlayerFarming.Location)
    {
      case FollowerLocation.Dungeon1_5:
      case FollowerLocation.Dungeon1_6:
      case FollowerLocation.Boss_Yngya:
      case FollowerLocation.Boss_Wolf:
        yield return (object) new WaitForSeconds(0.1f);
        List<RewardsItem.ChestRewards> chestRewardsList = new List<RewardsItem.ChestRewards>();
        if (DataManager.CheckIfThereAreSkinsAvailable())
          chestRewardsList.Add(RewardsItem.ChestRewards.FOLLOWER_SKIN);
        if (DataManager.Instance.GetDecorationListFromLocation(PlayerFarming.Location).Count > 0)
          chestRewardsList.Add(RewardsItem.ChestRewards.BASE_DECORATION);
        if (chestRewardsList.Count > 0)
        {
          FoundItemPickUp foundItemPickUp = (FoundItemPickUp) null;
          interactionChestBossRoom.Reward = RewardsItem.Instance.ReturnItemType(chestRewardsList[Random.Range(0, chestRewardsList.Count)]);
          PickUp pickUp = InventoryItem.Spawn(interactionChestBossRoom.Reward, 1, interactionChestBossRoom.transform.position + Vector3.back, 0.0f);
          if ((Object) pickUp != (Object) null)
            foundItemPickUp = pickUp.GetComponent<FoundItemPickUp>();
          if ((Object) foundItemPickUp != (Object) null && (Object) pickUp != (Object) null)
            foundItemPickUp.GetComponent<PickUp>().SetInitialSpeedAndDiraction(4f + Random.Range(-0.5f, 1f), (float) (270 + Random.Range(-90, 90)));
          if ((Object) pickUp != (Object) null)
          {
            FoundItemPickUp component = pickUp.GetComponent<FoundItemPickUp>();
            if ((Object) component != (Object) null)
            {
              component.AutomaticallyInteract = true;
              break;
            }
            break;
          }
          break;
        }
        break;
    }
    GameManager.GetInstance().OnConversationEnd();
    GameManager.GetInstance().CameraResetTargetZoom();
    interactionChestBossRoom.state.CURRENT_STATE = StateMachine.State.Idle;
    Interaction_Chest.ChestEvent onChestRevealed = Interaction_ChestBossRoom.OnChestRevealed;
    if (onChestRevealed != null)
      onChestRevealed();
  }

  public enum State
  {
    Hidden,
    Closed,
    Open,
  }
}
