// Decompiled with JetBrains decompiler
// Type: SingleChoiceRewardHeartShop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SingleChoiceRewardHeartShop : SingleChoiceRewardOption
{
  private void OnEnable()
  {
    this.AllowDecorationAndSkin = true;
    switch (this.itemOptions[0].itemToBuy)
    {
      case InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION:
        if (DataManager.CheckAvailableDecorations())
          break;
        if ((double) Random.value > 0.75)
          this.itemOptions[0].itemToBuy = InventoryItem.ITEM_TYPE.RED_HEART;
        else if ((double) Random.value > 0.5)
          this.itemOptions[0].itemToBuy = InventoryItem.ITEM_TYPE.HALF_HEART;
        else if ((double) Random.value > 0.25)
          this.itemOptions[0].itemToBuy = InventoryItem.ITEM_TYPE.BLUE_HEART;
        else
          this.itemOptions[0].itemToBuy = InventoryItem.ITEM_TYPE.HALF_BLUE_HEART;
        this.itemOptions[0].quantity = 1;
        break;
      case InventoryItem.ITEM_TYPE.FOUND_ITEM_FOLLOWERSKIN:
        if (DataManager.CheckIfThereAreSkinsAvailable())
          break;
        if ((double) Random.value > 0.800000011920929)
        {
          this.itemOptions[0].itemToBuy = InventoryItem.ITEM_TYPE.GOLD_REFINED;
          this.itemOptions[0].quantity = 5;
          break;
        }
        if ((double) Random.value > 0.40000000596046448)
        {
          this.itemOptions[0].itemToBuy = InventoryItem.ITEM_TYPE.BLACK_GOLD;
          this.itemOptions[0].quantity = 15;
          break;
        }
        this.itemOptions[0].itemToBuy = InventoryItem.ITEM_TYPE.GOLD_NUGGET;
        this.itemOptions[0].quantity = 25;
        break;
      case InventoryItem.ITEM_TYPE.DOCTRINE_STONE:
        if (DoctrineUpgradeSystem.TrySermonsStillAvailable() && DoctrineUpgradeSystem.TryGetStillDoctrineStone())
          break;
        if ((double) Random.value < 0.5)
          this.itemOptions[0].itemToBuy = InventoryItem.ITEM_TYPE.GIFT_SMALL;
        else
          this.itemOptions[0].itemToBuy = InventoryItem.ITEM_TYPE.GIFT_MEDIUM;
        this.itemOptions[0].quantity = 1;
        break;
    }
  }
}
