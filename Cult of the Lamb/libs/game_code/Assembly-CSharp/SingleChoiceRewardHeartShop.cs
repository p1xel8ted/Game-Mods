// Decompiled with JetBrains decompiler
// Type: SingleChoiceRewardHeartShop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SingleChoiceRewardHeartShop : SingleChoiceRewardOption
{
  public void OnEnable()
  {
    if (DungeonSandboxManager.Active)
    {
      this.itemOptions.Clear();
      this.itemOptions.Add(new BuyEntry(InventoryItem.ITEM_TYPE.RED_HEART, InventoryItem.ITEM_TYPE.BLACK_GOLD, 1));
    }
    if (this.itemOptions.Count > 0)
    {
      this.AllowDecorationAndSkin = true;
      switch (this.itemOptions[0].itemToBuy)
      {
        case InventoryItem.ITEM_TYPE.RED_HEART:
          float num1 = Random.value;
          if ((double) num1 > 0.75)
            this.itemOptions[0].itemToBuy = InventoryItem.ITEM_TYPE.RED_HEART;
          else if ((double) num1 > 0.5)
            this.itemOptions[0].itemToBuy = InventoryItem.ITEM_TYPE.HALF_HEART;
          else if ((double) num1 > 0.25)
            this.itemOptions[0].itemToBuy = InventoryItem.ITEM_TYPE.BLUE_HEART;
          else
            this.itemOptions[0].itemToBuy = InventoryItem.ITEM_TYPE.HALF_BLUE_HEART;
          this.itemOptions[0].quantity = 1;
          this.itemOptions[0].SingleQuantityItem = true;
          break;
        case InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION:
          if (DataManager.Instance.GetDecorationListFromLocation(PlayerFarming.Location).Count <= 0)
          {
            if ((double) Random.value > 0.75)
              this.itemOptions[0].itemToBuy = InventoryItem.ITEM_TYPE.RED_HEART;
            else if ((double) Random.value > 0.5)
              this.itemOptions[0].itemToBuy = InventoryItem.ITEM_TYPE.HALF_HEART;
            else if ((double) Random.value > 0.25)
              this.itemOptions[0].itemToBuy = InventoryItem.ITEM_TYPE.BLUE_HEART;
            else
              this.itemOptions[0].itemToBuy = InventoryItem.ITEM_TYPE.HALF_BLUE_HEART;
            this.itemOptions[0].quantity = 1;
            this.itemOptions[0].SingleQuantityItem = true;
            break;
          }
          break;
        case InventoryItem.ITEM_TYPE.FOUND_ITEM_FOLLOWERSKIN:
          if (!DataManager.CheckIfThereAreSkinsAvailable())
          {
            if ((double) Random.value > 0.800000011920929)
            {
              this.itemOptions[0].itemToBuy = InventoryItem.ITEM_TYPE.GOLD_REFINED;
              this.itemOptions[0].quantity = 5;
              this.itemOptions[0].SingleQuantityItem = false;
              break;
            }
            if ((double) Random.value > 0.40000000596046448)
            {
              this.itemOptions[0].itemToBuy = InventoryItem.ITEM_TYPE.BLACK_GOLD;
              this.itemOptions[0].quantity = 15;
              this.itemOptions[0].SingleQuantityItem = false;
              break;
            }
            this.itemOptions[0].itemToBuy = InventoryItem.ITEM_TYPE.GOLD_NUGGET;
            this.itemOptions[0].quantity = 25;
            this.itemOptions[0].SingleQuantityItem = false;
            break;
          }
          break;
        case InventoryItem.ITEM_TYPE.DOCTRINE_STONE:
          if (!DoctrineUpgradeSystem.TrySermonsStillAvailable() || !DoctrineUpgradeSystem.TryGetStillDoctrineStone())
          {
            if ((double) Random.value < 0.5)
              this.itemOptions[0].itemToBuy = InventoryItem.ITEM_TYPE.GIFT_SMALL;
            else
              this.itemOptions[0].itemToBuy = InventoryItem.ITEM_TYPE.GIFT_MEDIUM;
            this.itemOptions[0].quantity = 1;
            this.itemOptions[0].SingleQuantityItem = true;
            break;
          }
          break;
        case InventoryItem.ITEM_TYPE.DLC_NECKLACE:
          float num2 = Random.value;
          if ((double) num2 > 0.800000011920929)
            this.itemOptions[0].itemToBuy = InventoryItem.ITEM_TYPE.Necklace_Weird;
          else if ((double) num2 > 0.60000002384185791)
            this.itemOptions[0].itemToBuy = InventoryItem.ITEM_TYPE.Necklace_Winter;
          else if ((double) num2 > 0.40000000596046448)
            this.itemOptions[0].itemToBuy = InventoryItem.ITEM_TYPE.Necklace_Targeted;
          else if ((double) num2 > 0.20000000298023224)
            this.itemOptions[0].itemToBuy = InventoryItem.ITEM_TYPE.Necklace_Frozen;
          else
            this.itemOptions[0].itemToBuy = InventoryItem.ITEM_TYPE.Necklace_Deaths_Door;
          this.itemOptions[0].quantity = 1;
          this.itemOptions[0].SingleQuantityItem = true;
          break;
      }
    }
    if (this.itemOptions.Count > 0)
      this.UpdateQuantityText(this.itemOptions[0]);
    if (this.itemOptions.Count > 0 && this.itemOptions[0].itemToBuy != InventoryItem.ITEM_TYPE.NONE)
      return;
    this.gameObject.SetActive(false);
  }
}
