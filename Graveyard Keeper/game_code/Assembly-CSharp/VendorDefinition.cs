// Decompiled with JetBrains decompiler
// Type: VendorDefinition
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class VendorDefinition : BalanceBaseObject
{
  [SerializeField]
  public List<string> product_types = new List<string>();
  public int start_tire;
  public float start_money;
  public float daily_money_income;
  public float[] levelup_costs;
  public List<VendorDefinition.CountModificator> count_modificators = new List<VendorDefinition.CountModificator>();
  public List<VendorDefinition.ItemModificator> not_buying = new List<VendorDefinition.ItemModificator>();
  public List<VendorDefinition.ItemModificator> not_selling = new List<VendorDefinition.ItemModificator>();
  public List<ExpressionRes> additional_types = new List<ExpressionRes>();

  public List<string> GetProductTypes()
  {
    List<string> productTypes = new List<string>();
    productTypes.AddRange((IEnumerable<string>) this.product_types);
    for (int index = 0; index < this.additional_types.Count; ++index)
    {
      if (this.additional_types[index].expression.EvaluateBoolean(MainGame.me.player, MainGame.me.player))
        productTypes.Add(this.additional_types[index].name);
    }
    return productTypes;
  }

  public void AddCountModificator(
    VendorDefinition.CountModificator count_modificator)
  {
    if (this.count_modificators == null)
      this.count_modificators = new List<VendorDefinition.CountModificator>();
    if (this.count_modificators.Count == 0)
    {
      this.count_modificators.Add(count_modificator);
    }
    else
    {
      for (int index = 0; index < this.count_modificators.Count; ++index)
      {
        if (this.count_modificators[index].tier < count_modificator.tier)
        {
          if (index == this.count_modificators.Count - 1)
          {
            this.count_modificators.Add(count_modificator);
            break;
          }
        }
        else
        {
          if (this.count_modificators[index].tier == count_modificator.tier)
          {
            this.count_modificators.Insert(index, count_modificator);
            break;
          }
          if (this.count_modificators[index].tier > count_modificator.tier)
          {
            this.count_modificators.Insert(index > 0 ? index - 1 : 0, count_modificator);
            break;
          }
        }
      }
    }
  }

  public void SortCountModificators()
  {
    if (this.count_modificators == null || this.count_modificators.Count <= 1)
      return;
    this.count_modificators.Sort((Comparison<VendorDefinition.CountModificator>) ((left, right) =>
    {
      if (left.tier < right.tier)
        return -1;
      return left.tier > right.tier ? 1 : 0;
    }));
  }

  [Serializable]
  public struct CountModificator
  {
    public string item_name;
    public int tier;
    public int base_count;
  }

  [Serializable]
  public struct ItemModificator
  {
    public string item_name;
    public int tier;
  }
}
