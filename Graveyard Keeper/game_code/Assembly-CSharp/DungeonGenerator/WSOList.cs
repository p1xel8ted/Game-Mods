// Decompiled with JetBrains decompiler
// Type: DungeonGenerator.WSOList
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace DungeonGenerator;

[CreateAssetMenu(fileName = "WSOList", menuName = "WSOList", order = 1)]
public class WSOList : ScriptableObject
{
  public List<WorldSimpleObject> wso_list = new List<WorldSimpleObject>();
  public List<float> weights = new List<float>();

  public bool IsCorrectWSOList()
  {
    return this.wso_list != null && this.weights != null && this.wso_list.Count == this.weights.Count;
  }

  public void AddWSO(WorldSimpleObject t_wso, float t_weight)
  {
    if ((Object) t_wso == (Object) null || (double) t_weight < 0.0)
      return;
    this.wso_list.Add(t_wso);
    this.weights.Add(t_weight);
  }

  public void RemoveWSO(WorldSimpleObject t_wso)
  {
    if ((Object) t_wso == (Object) null || !this.IsCorrectWSOList() || !this.wso_list.Contains(t_wso))
      return;
    int index = this.wso_list.IndexOf(t_wso);
    this.wso_list.RemoveAt(index);
    this.weights.RemoveAt(index);
  }

  public WorldSimpleObject GetRandomWSO()
  {
    if (!this.IsCorrectWSOList())
      return (WorldSimpleObject) null;
    if (this.wso_list.Count == 0)
      return (WorldSimpleObject) null;
    if (this.wso_list.Count == 1)
      return this.wso_list[0];
    float num1 = Dungeon.RandomRange(0.0f, this.SumOfWeights());
    float num2 = 0.0f;
    int index;
    for (index = 0; index < this.weights.Count; ++index)
    {
      num2 += this.weights[index];
      if ((double) num2 > (double) num1)
        break;
    }
    if (index >= this.weights.Count)
      index = this.weights.Count - 1;
    return this.wso_list[index];
  }

  public float SumOfWeights()
  {
    float num = 0.0f;
    foreach (float weight in this.weights)
      num += weight;
    return num;
  }
}
