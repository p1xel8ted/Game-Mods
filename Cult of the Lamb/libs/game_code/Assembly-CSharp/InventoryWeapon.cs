// Decompiled with JetBrains decompiler
// Type: InventoryWeapon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;

#nullable disable
[MessagePackObject(false)]
public class InventoryWeapon
{
  [Key(0)]
  public InventoryWeapon.ITEM_TYPE type;
  [Key(1)]
  public int quantity = 1;
  [Key(2)]
  public string name;
  public bool QuantityAsPercentage;

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

  public string GetQuantity() => this.quantity.ToString() + (this.QuantityAsPercentage ? "%" : "");

  public enum ITEM_TYPE
  {
    SWORD,
    SHOVEL,
    SEED_BAG,
    WATERING_CAN,
  }
}
