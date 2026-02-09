// Decompiled with JetBrains decompiler
// Type: GraveRequirement
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class GraveRequirement : BalanceBaseObject
{
  [SerializeField]
  public List<Item> _grave_req = new List<Item>();

  public List<Item> GetRequiredItems()
  {
    List<Item> requiredItems = new List<Item>();
    foreach (Item obj in this._grave_req)
      requiredItems.Add(new Item(obj));
    return requiredItems;
  }
}
