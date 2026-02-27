// Decompiled with JetBrains decompiler
// Type: InventoryWeapon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class InventoryWeapon
{
  public InventoryWeapon.ITEM_TYPE type;
  public int quantity = 1;
  public string name;
  private bool QuantityAsPercentage;

  public InventoryWeapon(InventoryWeapon.ITEM_TYPE type, int quantity)
  {
    this.type = type;
    this.quantity = quantity;
    switch (type)
    {
      case InventoryWeapon.ITEM_TYPE.SWORD:
        this.name = "Sword";
        this.QuantityAsPercentage = true;
        break;
      case InventoryWeapon.ITEM_TYPE.SHOVEL:
        this.name = "Shovel";
        this.QuantityAsPercentage = false;
        break;
      case InventoryWeapon.ITEM_TYPE.SEED_BAG:
        this.name = "Seed Bag";
        this.QuantityAsPercentage = true;
        break;
      case InventoryWeapon.ITEM_TYPE.WATERING_CAN:
        this.name = "Watering Can";
        this.QuantityAsPercentage = true;
        break;
    }
  }

  public string GetQuantity()
  {
    return this.quantity.ToString() + (this.QuantityAsPercentage ? (object) "%" : (object) "");
  }

  public enum ITEM_TYPE
  {
    SWORD,
    SHOVEL,
    SEED_BAG,
    WATERING_CAN,
  }
}
