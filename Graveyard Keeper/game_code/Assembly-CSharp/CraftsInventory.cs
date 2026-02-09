// Decompiled with JetBrains decompiler
// Type: CraftsInventory
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class CraftsInventory
{
  [SerializeField]
  public List<string> _craft_ids = new List<string>();
  public List<ObjectCraftDefinition> additional_crafts;
  public bool is_building = true;
  public CraftDefinition.CraftType craft_type;

  public void AddCraft(string id) => this._craft_ids.Add(id);

  public void RemoveCraft(string id) => this._craft_ids.Remove(id);

  public List<CraftDefinition> GetCraftsList()
  {
    List<CraftDefinition> craftsList = new List<CraftDefinition>();
    foreach (string craftId in this._craft_ids)
      craftsList.Add(GameBalance.me.GetData<CraftDefinition>(craftId));
    return craftsList;
  }

  public List<ObjectCraftDefinition> GetObjectCraftsList()
  {
    List<ObjectCraftDefinition> objectCraftsList = new List<ObjectCraftDefinition>();
    foreach (string craftId in this._craft_ids)
      objectCraftsList.Add(GameBalance.me.GetData<ObjectCraftDefinition>(craftId));
    if (this.additional_crafts != null)
      objectCraftsList.AddRange((IEnumerable<ObjectCraftDefinition>) this.additional_crafts);
    return objectCraftsList;
  }
}
