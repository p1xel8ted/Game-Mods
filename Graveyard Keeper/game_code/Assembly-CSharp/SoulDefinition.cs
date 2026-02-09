// Decompiled with JetBrains decompiler
// Type: SoulDefinition
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class SoulDefinition : BalanceBaseObject
{
  public List<string> parts_ids = new List<string>();
  public int tier;
  public string linked_item_id;

  public Item GenerateSoulItem()
  {
    Item soulItem = new Item(this.linked_item_id, 1)
    {
      inventory_size = 99
    };
    foreach (string partsId in this.parts_ids)
      soulItem.inventory.Add(new Item(partsId, 1));
    soulItem.AddToParams("sins_count", (float) this.parts_ids.Count);
    Debug.Log((object) ("Created soul: " + this.id));
    return soulItem;
  }
}
