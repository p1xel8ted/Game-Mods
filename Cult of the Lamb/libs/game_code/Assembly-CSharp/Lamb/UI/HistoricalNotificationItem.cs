// Decompiled with JetBrains decompiler
// Type: Lamb.UI.HistoricalNotificationItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
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
