// Decompiled with JetBrains decompiler
// Type: BodyDefinition
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class BodyDefinition : BalanceBaseObject
{
  public List<string> parts_ids = new List<string>();
  public int tier;
  public string linked_item_id;

  public Item GenerateBodyItem()
  {
    Item bodyItem = new Item(this.linked_item_id, 1);
    bodyItem.inventory_size = 99;
    if (string.IsNullOrEmpty(this.linked_item_id))
      Debug.LogError((object) ("linked_item_id is empty for body definition: " + this.id));
    foreach (string partsId in this.parts_ids)
      bodyItem.inventory.Add(new Item(partsId, 1));
    Debug.Log((object) ("Created body " + this.id));
    return bodyItem;
  }
}
