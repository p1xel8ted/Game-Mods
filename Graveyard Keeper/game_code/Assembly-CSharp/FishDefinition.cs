// Decompiled with JetBrains decompiler
// Type: FishDefinition
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

#nullable disable
[Serializable]
public class FishDefinition : BalanceBaseObject
{
  public string item_id;
  public string fish_preset;
  public List<FishDefinition.WeightData> base_weight;
  public float[] time_of_day_mods;
  public float[] distance_mods;
  public List<float> rod_mods;
  public FishDefinition.BaitData no_bait_mod = new FishDefinition.BaitData();
  public List<FishDefinition.BaitData> baits_mod = new List<FishDefinition.BaitData>();

  public float GetTotalWeight(
    string reservoir_name,
    bool is_night,
    int distance,
    int rod_lvl,
    string bait)
  {
    if (string.IsNullOrEmpty(reservoir_name))
    {
      Debug.LogError((object) "Reservoir is null!");
      return 0.0f;
    }
    FishDefinition.WeightData weightData1 = (FishDefinition.WeightData) null;
    foreach (FishDefinition.WeightData weightData2 in this.base_weight)
    {
      if (weightData2.data_name == reservoir_name)
        weightData1 = weightData2;
    }
    if (weightData1 == null)
    {
      Debug.LogError((object) $"Not found reservoir \"{reservoir_name}\" in fish \"{this.id}\"");
      return 0.0f;
    }
    float num1 = weightData1.weight * this.time_of_day_mods[is_night ? 0 : 1];
    if (distance < 1 || distance > 3)
    {
      Debug.LogError((object) $"Fishing distance is wrong: \"{distance.ToString()}\"");
      return 0.0f;
    }
    float num2 = num1 * this.distance_mods[distance - 1];
    if (rod_lvl < 0 || rod_lvl >= this.rod_mods.Count)
    {
      Debug.LogError((object) $"Fishing rod is wrong: \"{rod_lvl.ToString()}\"");
      return 0.0f;
    }
    float num3 = num2 * this.rod_mods[rod_lvl];
    float totalWeight;
    if (string.IsNullOrEmpty(bait))
    {
      totalWeight = num3 * this.no_bait_mod.weight_k;
    }
    else
    {
      FishDefinition.BaitData baitData1 = (FishDefinition.BaitData) null;
      foreach (FishDefinition.BaitData baitData2 in this.baits_mod)
      {
        if (baitData2.bait_name == bait)
          baitData1 = baitData2;
      }
      if (baitData1 == null)
        baitData1 = this.no_bait_mod;
      totalWeight = num3 * baitData1.weight_k;
    }
    return totalWeight;
  }

  [Serializable]
  public class WeightData
  {
    public string data_name;
    public float weight;

    public static List<FishDefinition.WeightData> ParseWeightDatas(string data)
    {
      List<FishDefinition.WeightData> weightDatas = new List<FishDefinition.WeightData>();
      string[] strArray1 = data.Replace(" ", "").Replace("\n", "").Replace("&#xd", "").Split(new char[1]
      {
        ';'
      }, StringSplitOptions.RemoveEmptyEntries);
      if (strArray1.Length != 0)
      {
        foreach (string str in strArray1)
        {
          string[] strArray2 = str.Trim().Split('=');
          if (strArray2.Length != 2)
          {
            Debug.LogError((object) "Wrong weights data!");
          }
          else
          {
            float result = 0.0f;
            if (!float.TryParse(strArray2[1], NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out result))
              Debug.LogError((object) $"Wrong weights data: can not parse \"{str}\"");
            else
              weightDatas.Add(new FishDefinition.WeightData()
              {
                data_name = strArray2[0],
                weight = result
              });
          }
        }
      }
      return weightDatas;
    }
  }

  [Serializable]
  public class BaitData
  {
    public string bait_name;
    public float weight_k;
    public float wait_time;
  }
}
