// Decompiled with JetBrains decompiler
// Type: Lamb.UI.HistoricalNotificationItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
namespace Lamb.UI;

public class HistoricalNotificationItem : HistoricalNotificationBase<FinalizedItemNotification>
{
  public override void ConfigureImpl(FinalizedItemNotification finalizedNotification)
  {
  }

  public override string GetLocalizedDescription(FinalizedItemNotification finalizedNotification)
  {
    float itemDelta = (float) finalizedNotification.ItemDelta;
    InventoryItem.ITEM_TYPE itemType = finalizedNotification.ItemType;
    return $"{FontImageNames.GetIconByType(itemType)} {((double) itemDelta > 0.0 ? "+" : "")}{itemDelta.ToString()}{InventoryItem.LocalizedName(itemType)} <color=#6E6666>{Inventory.GetItemQuantity((int) itemDelta).ToString()}</color>";
  }
}
