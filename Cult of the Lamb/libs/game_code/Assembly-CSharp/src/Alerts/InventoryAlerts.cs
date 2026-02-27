// Decompiled with JetBrains decompiler
// Type: src.Alerts.InventoryAlerts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using System;

#nullable disable
namespace src.Alerts;

[MessagePackObject(false)]
[Serializable]
public class InventoryAlerts : AlertCategory<InventoryItem.ITEM_TYPE>
{
  public InventoryAlerts()
  {
    Inventory.OnItemAddedToInventory += new Inventory.ItemAddedToInventory(this.OnItemAdded);
  }

  void object.Finalize()
  {
    try
    {
      Inventory.OnItemAddedToInventory -= new Inventory.ItemAddedToInventory(this.OnItemAdded);
    }
    finally
    {
      // ISSUE: explicit finalizer call
      base.Finalize();
    }
  }

  public void OnItemAdded(InventoryItem.ITEM_TYPE itemType, int Delta) => this.AddOnce(itemType);

  public override bool HasAlert(InventoryItem.ITEM_TYPE alert)
  {
    return base.HasAlert(alert) && Inventory.GetItemQuantity(alert) > 0;
  }

  [IgnoreMember]
  public override int Total
  {
    get
    {
      int total = 0;
      foreach (InventoryItem.ITEM_TYPE alert in this._alerts)
      {
        if (this.HasAlert(alert))
          ++total;
      }
      return total;
    }
  }
}
