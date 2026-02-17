// Decompiled with JetBrains decompiler
// Type: RandomChildPickerWeighted
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RandomChildPickerWeighted : BaseMonoBehaviour
{
  public List<RandomChildPickerWeighted.ItemAndProbability> LootToDrop = new List<RandomChildPickerWeighted.ItemAndProbability>();

  public void GetChildren()
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

  public void Start() => this.SelectObejct();

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
