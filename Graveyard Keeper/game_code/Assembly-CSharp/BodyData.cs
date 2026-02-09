// Decompiled with JetBrains decompiler
// Type: BodyData
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class BodyData
{
  public string name;
  [SerializeField]
  public Item _data = new Item();
  public long linked_obj_id;
  public string grave_req_id;
  [SerializeField]
  public int _id;
  [SerializeField]
  public string _definition_id = "";

  public BodyDefinition definition => GameBalance.me.GetData<BodyDefinition>(this._definition_id);

  public int id => this._id;

  public BodyData()
  {
  }

  public Item data => this._data;

  public BodyData(string definition_id)
  {
    this._id = ProjectTools.GenerateIntId();
    this.SetItemByDefinition(definition_id);
  }

  public void SetItemByDefinition(string definition_id)
  {
    this._definition_id = definition_id;
    this.SetItem(this.definition.GenerateBodyItem());
  }

  public void SetItem(Item _item) => this._data = _item;

  public List<ItemDefinition.ItemType> GetExistingBodyParts()
  {
    List<ItemDefinition.ItemType> existingBodyParts = new List<ItemDefinition.ItemType>();
    foreach (Item obj in this._data.inventory)
      existingBodyParts.Add(obj.definition.type);
    return existingBodyParts;
  }

  public bool HasBodyPart(ItemDefinition.ItemType type)
  {
    foreach (Item obj in this._data.inventory)
    {
      if (obj.definition.type == type)
        return true;
    }
    return false;
  }

  public Item GetPartItem(ItemDefinition.ItemType type)
  {
    foreach (Item partItem in this._data.inventory)
    {
      if (partItem.definition.type == type)
        return partItem;
    }
    return (Item) null;
  }
}
