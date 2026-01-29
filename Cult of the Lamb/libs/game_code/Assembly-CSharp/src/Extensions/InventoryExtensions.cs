// Decompiled with JetBrains decompiler
// Type: src.Extensions.InventoryExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace src.Extensions;

public static class InventoryExtensions
{
  public static List<InventoryItem.ITEM_TYPE> TrimEmpty(this List<InventoryItem.ITEM_TYPE> list)
  {
    for (int index = list.Count - 1; index >= 0; --index)
    {
      if (Inventory.GetItemQuantity(list[index]) <= 0)
        list.RemoveAt(index);
    }
    return list;
  }
}
