// Decompiled with JetBrains decompiler
// Type: Structures_LumberMine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class Structures_LumberMine : StructureBrain
{
  public InventoryItem FindLogItem()
  {
    InventoryItem logItem = (InventoryItem) null;
    foreach (InventoryItem inventoryItem in this.Data.Inventory)
    {
      if (inventoryItem.type == 1)
      {
        logItem = inventoryItem;
        break;
      }
    }
    return logItem;
  }

  public int RemainingLumber
  {
    get
    {
      InventoryItem logItem = this.FindLogItem();
      return logItem != null ? logItem.quantity : 0;
    }
  }

  public bool TryTakeLumber()
  {
    bool lumber = false;
    InventoryItem logItem = this.FindLogItem();
    if (logItem.quantity > 0)
    {
      --logItem.quantity;
      lumber = true;
    }
    return lumber;
  }
}
