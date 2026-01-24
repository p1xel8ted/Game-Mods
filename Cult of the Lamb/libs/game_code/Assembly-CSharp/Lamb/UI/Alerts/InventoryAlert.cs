// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Alerts.InventoryAlert
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
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
