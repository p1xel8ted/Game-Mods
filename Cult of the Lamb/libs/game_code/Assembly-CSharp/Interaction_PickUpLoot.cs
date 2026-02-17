// Decompiled with JetBrains decompiler
// Type: Interaction_PickUpLoot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_PickUpLoot : Interaction
{
  public string customPickupSFX = "";
  public InventoryItem.ITEM_TYPE itemType;
  public int quantity;

  public void Init(InventoryItem.ITEM_TYPE itemType, int quantity)
  {
    this.itemType = itemType;
    this.quantity = quantity;
    if (itemType != InventoryItem.ITEM_TYPE.NONE)
      return;
    this.gameObject.SetActive(false);
  }

  public override void GetLabel()
  {
    if (this.itemType == InventoryItem.ITEM_TYPE.NONE)
      this.Label = ScriptLocalization.FollowerInteractions.MakeDemand;
    else if (this.quantity <= 1)
      this.Label = $"{ScriptLocalization.Interactions.Choose} {FontImageNames.GetIconByType(this.itemType)} {InventoryItem.LocalizedName(this.itemType)}";
    else
      this.Label = $"{$"{ScriptLocalization.Interactions.Choose} {FontImageNames.GetIconByType(this.itemType)} {InventoryItem.LocalizedName(this.itemType)}"} x{LocalizeIntegration.ReverseText(this.quantity.ToString())}";
  }

  public override void OnBecomeCurrent(PlayerFarming playerFarming)
  {
    base.OnBecomeCurrent(playerFarming);
    AudioManager.Instance.PlayOneShot("event:/ui/arrow_change_selection", this.gameObject);
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (this.itemType == InventoryItem.ITEM_TYPE.PLEASURE_POINT)
      this.StartCoroutine((IEnumerator) this.PleasurePointIE());
    else if (this.itemType != InventoryItem.ITEM_TYPE.NONE)
    {
      if (!string.IsNullOrEmpty(this.customPickupSFX))
        AudioManager.Instance.PlayOneShot(this.customPickupSFX, this.transform.position);
      for (int index = 0; index < this.quantity; ++index)
      {
        PickUp pickUp = InventoryItem.Spawn(this.itemType, 1, this.transform.position + Vector3.back, 0.0f);
        if (this.quantity == 1)
          pickUp.SetInitialSpeedAndDiraction(5f, 270f);
        else
          pickUp.SetInitialSpeedAndDiraction(4f + Random.Range(-0.5f, 1f), (float) (270 + Random.Range(-90, 90)));
        pickUp.MagnetDistance = 3f;
        pickUp.CanStopFollowingPlayer = false;
        switch (this.itemType)
        {
          case InventoryItem.ITEM_TYPE.GIFT_SMALL:
          case InventoryItem.ITEM_TYPE.GIFT_MEDIUM:
            pickUp.MagnetToPlayer = false;
            break;
          case InventoryItem.ITEM_TYPE.DOCTRINE_STONE:
            Interaction_DoctrineStone component1 = pickUp.GetComponent<Interaction_DoctrineStone>();
            if ((Object) component1 != (Object) null)
            {
              component1.MagnetToPlayer();
              break;
            }
            break;
          default:
            FoundItemPickUp component2 = pickUp.GetComponent<FoundItemPickUp>();
            if ((Object) component2 != (Object) null)
            {
              component2.MagnetToPlayer();
              break;
            }
            break;
        }
      }
      AudioManager.Instance.PlayOneShot("event:/Stings/generic_positive", this.gameObject);
      AudioManager.Instance.PlayOneShot("event:/chests/chest_item_spawn", this.gameObject);
    }
    this.enabled = false;
  }

  public IEnumerator PleasurePointIE()
  {
    Interaction_PickUpLoot interactionPickUpLoot = this;
    AudioManager.Instance.PlayOneShot("event:/Stings/sins_snake_sting", interactionPickUpLoot.transform.position);
    float Timer = 0.0f;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionPickUpLoot.playerFarming.gameObject, 5f);
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
    PlayerSimpleInventory component = interactionPickUpLoot.state.gameObject.GetComponent<PlayerSimpleInventory>();
    Vector3 BookTargetPosition = new Vector3(component.ItemImage.transform.position.x, component.ItemImage.transform.position.y, -1f);
    interactionPickUpLoot.state.CURRENT_STATE = StateMachine.State.FoundItem;
    interactionPickUpLoot.transform.DOShakeScale(2.5f, 0.2f);
    while ((double) (Timer += Time.deltaTime) < 2.0)
    {
      interactionPickUpLoot.transform.position = Vector3.Lerp(interactionPickUpLoot.transform.position, BookTargetPosition, 5f * Time.deltaTime);
      yield return (object) null;
    }
    interactionPickUpLoot.transform.position = BookTargetPosition;
    Inventory.AddItem(154, 1);
    yield return (object) new WaitForSeconds(0.5f);
    interactionPickUpLoot.state.CURRENT_STATE = StateMachine.State.Idle;
    GameManager.GetInstance().OnConversationEnd();
    Object.Destroy((Object) interactionPickUpLoot.gameObject);
  }
}
