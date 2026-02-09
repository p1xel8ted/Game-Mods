// Decompiled with JetBrains decompiler
// Type: Structures_Daycare
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class Structures_Daycare : StructureBrain
{
  public const int MAX_POOPS = 20;

  public int Capacity => 3;

  public override bool IsFull => this.Data.MultipleFollowerIDs.Count >= this.Capacity;

  public float BoundariesRadius => 0.8f;

  public override void DepositItem(InventoryItem.ITEM_TYPE type, int quantity = 1)
  {
    if (this.Data.Inventory.Count > 0)
    {
      if (this.Data.Inventory[0].quantity >= 20)
        return;
      base.DepositItem(type, quantity);
    }
    else
      base.DepositItem(type, quantity);
  }

  public override bool SnowedUnder(bool showNotifications = true, bool refreshFollowerTasks = true)
  {
    return this.Data.MultipleFollowerIDs.Count <= 0 && base.SnowedUnder(showNotifications, refreshFollowerTasks);
  }
}
