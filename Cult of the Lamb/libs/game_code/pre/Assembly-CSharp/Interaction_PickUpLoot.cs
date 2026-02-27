// Decompiled with JetBrains decompiler
// Type: Interaction_PickUpLoot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using UnityEngine;

#nullable disable
public class Interaction_PickUpLoot : Interaction
{
  private InventoryItem.ITEM_TYPE itemType;
  private int quantity;

  public void Init(InventoryItem.ITEM_TYPE itemType, int quantity)
  {
    this.itemType = itemType;
    this.quantity = quantity;
  }

  public override void GetLabel()
  {
    if (this.quantity <= 1)
      this.Label = $"{ScriptLocalization.Interactions.Choose} {FontImageNames.GetIconByType(this.itemType)} {InventoryItem.LocalizedName(this.itemType)}";
    else
      this.Label = $"{$"{ScriptLocalization.Interactions.Choose} {FontImageNames.GetIconByType(this.itemType)} {InventoryItem.LocalizedName(this.itemType)}"} x{this.quantity}";
  }

  public override void OnBecomeCurrent()
  {
    base.OnBecomeCurrent();
    AudioManager.Instance.PlayOneShot("event:/ui/arrow_change_selection", this.gameObject);
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    for (int index = 0; index < this.quantity; ++index)
    {
      PickUp pickUp = InventoryItem.Spawn(this.itemType, 1, this.transform.position + Vector3.back, 0.0f);
      if (this.quantity == 1)
        pickUp.SetInitialSpeedAndDiraction(5f, 270f);
      else
        pickUp.SetInitialSpeedAndDiraction(4f + Random.Range(-0.5f, 1f), (float) (270 + Random.Range(-90, 90)));
      pickUp.MagnetDistance = 3f;
      pickUp.CanStopFollowingPlayer = false;
      if (this.itemType == InventoryItem.ITEM_TYPE.DOCTRINE_STONE)
      {
        Interaction_DoctrineStone component = pickUp.GetComponent<Interaction_DoctrineStone>();
        if ((Object) component != (Object) null)
          component.MagnetToPlayer();
      }
      else
      {
        FoundItemPickUp component = pickUp.GetComponent<FoundItemPickUp>();
        if ((Object) component != (Object) null)
          component.MagnetToPlayer();
      }
    }
    AudioManager.Instance.PlayOneShot("event:/Stings/generic_positive", this.gameObject);
    AudioManager.Instance.PlayOneShot("event:/chests/chest_item_spawn", this.gameObject);
    this.enabled = false;
  }
}
