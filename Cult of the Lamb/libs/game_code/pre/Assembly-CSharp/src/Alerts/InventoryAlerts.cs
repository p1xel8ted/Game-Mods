// Decompiled with JetBrains decompiler
// Type: src.Alerts.InventoryAlerts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace src.Alerts;

public class InventoryAlerts : AlertCategory<InventoryItem.ITEM_TYPE>
{
  public InventoryAlerts()
  {
    Inventory.OnItemAddedToInventory += new Inventory.ItemAddedToInventory(this.OnItemAdded);
  }

  ~InventoryAlerts()
  {
    Inventory.OnItemAddedToInventory -= new Inventory.ItemAddedToInventory(this.OnItemAdded);
  }

  private void OnItemAdded(InventoryItem.ITEM_TYPE itemType, int Delta) => this.AddOnce(itemType);

  public override bool HasAlert(InventoryItem.ITEM_TYPE alert)
  {
    return base.HasAlert(alert) && Inventory.GetItemQuantity(alert) > 0;
  }

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
