// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Alerts.InventoryAlert
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
namespace Lamb.UI.Alerts;

public class InventoryAlert : AlertBadge<InventoryItem.ITEM_TYPE>
{
  public override AlertCategory<InventoryItem.ITEM_TYPE> _source
  {
    get => (AlertCategory<InventoryItem.ITEM_TYPE>) DataManager.Instance.Alerts.Inventory;
  }
}
