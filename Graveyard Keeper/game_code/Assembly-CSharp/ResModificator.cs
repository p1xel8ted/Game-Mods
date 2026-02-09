// Decompiled with JetBrains decompiler
// Type: ResModificator
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class ResModificator
{
  public static List<Item> ProcessItemsListBeforeDrop(
    List<Item> items,
    WorldGameObject wgo,
    WorldGameObject character,
    Item exclude_item = null)
  {
    List<Item> objList1 = new List<Item>();
    List<Item> objList2 = new List<Item>();
    List<float> floatList = new List<float>();
    float maxInclusive = 0.0f;
    foreach (Item obj in items)
    {
      if (obj.chance_group == -1)
      {
        if (obj.self_chance.EvaluateChance(wgo, character))
          objList1.Add(new Item(obj));
      }
      else
      {
        float num = obj.common_chance.EvaluateFloat(wgo, character);
        if ((double) num > 0.0)
        {
          objList2.Add(new Item(obj));
          floatList.Add(num);
          maxInclusive += num;
        }
      }
    }
    float num1 = Random.Range(0.0f, maxInclusive);
    float num2 = 0.0f;
    for (int index = 0; index < floatList.Count; ++index)
    {
      num2 += floatList[index];
      if ((double) num2 >= (double) num1)
      {
        objList1.Add(objList2[index]);
        break;
      }
    }
    if (exclude_item != null)
    {
      string str1 = exclude_item.id.Split(':')[0];
      for (int index = 0; index < objList1.Count; ++index)
      {
        string str2 = objList1[index].id.Split(':')[0];
        if (str1 == str2)
          objList1.RemoveAt(index);
      }
    }
    foreach (Item obj in objList1)
    {
      if (obj.min_value != null && !obj.min_value.HasNoExpresion())
      {
        int minInclusive = Mathf.RoundToInt(obj.min_value.EvaluateFloat(wgo, character));
        int num3 = Mathf.RoundToInt(obj.max_value.EvaluateFloat(wgo, character));
        if (minInclusive < 0)
          minInclusive = 0;
        obj.value = num3 < minInclusive ? minInclusive : Random.Range(minInclusive, num3 + 1);
      }
    }
    return objList1;
  }
}
