// Decompiled with JetBrains decompiler
// Type: RandomChildPickerWeighted
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RandomChildPickerWeighted : BaseMonoBehaviour
{
  public List<RandomChildPickerWeighted.ItemAndProbability> LootToDrop = new List<RandomChildPickerWeighted.ItemAndProbability>();

  private void GetChildren()
  {
    this.LootToDrop = new List<RandomChildPickerWeighted.ItemAndProbability>();
    int index = -1;
    while (++index < this.transform.childCount)
      this.LootToDrop.Add(new RandomChildPickerWeighted.ItemAndProbability()
      {
        GameObject = this.transform.GetChild(index).gameObject,
        parent = this
      });
  }

  private void Start() => this.SelectObejct();

  public void SelectObejct()
  {
    int[] weights = new int[this.LootToDrop.Count];
    int index = -1;
    while (++index < this.LootToDrop.Count)
    {
      weights[index] = this.LootToDrop[index].Probability;
      this.LootToDrop[index].GameObject.SetActive(false);
    }
    this.LootToDrop[Utils.GetRandomWeightedIndex(weights)].GameObject.SetActive(true);
  }

  [Serializable]
  public class ItemAndProbability
  {
    public GameObject GameObject;
    [Range(1f, 100f)]
    public int Probability = 1;
    public RandomChildPickerWeighted parent;

    public float TotalProbability
    {
      get
      {
        float totalProbability = 0.0f;
        foreach (RandomChildPickerWeighted.ItemAndProbability itemAndProbability in this.parent.LootToDrop)
          totalProbability += (float) itemAndProbability.Probability;
        return totalProbability;
      }
    }
  }
}
