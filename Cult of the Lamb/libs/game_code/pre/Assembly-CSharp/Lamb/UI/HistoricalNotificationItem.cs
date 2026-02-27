// Decompiled with JetBrains decompiler
// Type: Lamb.UI.HistoricalNotificationItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace Lamb.UI;

public class HistoricalNotificationItem : HistoricalNotificationBase<FinalizedItemNotification>
{
  protected override void ConfigureImpl(FinalizedItemNotification finalizedNotification)
  {
  }

  protected override string GetLocalizedDescription(FinalizedItemNotification finalizedNotification)
  {
    float itemDelta = (float) finalizedNotification.ItemDelta;
    InventoryItem.ITEM_TYPE itemType = finalizedNotification.ItemType;
    return $"{FontImageNames.GetIconByType(itemType)} {((double) itemDelta > 0.0 ? (object) "+" : (object) "")}{(object) itemDelta}{InventoryItem.LocalizedName(itemType)} <color=#6E6666>{(object) Inventory.GetItemQuantity((int) itemDelta)}</color>";
  }
}
