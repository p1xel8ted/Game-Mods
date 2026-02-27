// Decompiled with JetBrains decompiler
// Type: Structures_LumberMine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class Structures_LumberMine : StructureBrain
{
  private InventoryItem FindLogItem()
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
