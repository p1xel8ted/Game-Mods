// Decompiled with JetBrains decompiler
// Type: RandomResource
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMBiomeGeneration;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RandomResource : BaseMonoBehaviour
{
  public List<RandomResource.Resource> Resources = new List<RandomResource.Resource>();
  public int value;
  public int[] Weights;

  public void Awake() => this.PlaceRandom();

  public void PlaceNext()
  {
    this.ClearPrefabs();
    this.value = ++this.value % this.Resources.Count;
    this.PlaceResource();
  }

  public void TestRandom()
  {
    System.Random random = new System.Random(UnityEngine.Random.Range(-2147483647 /*0x80000001*/, int.MaxValue));
    this.Weights = new int[this.Resources.Count];
    int index = -1;
    while (++index < this.Resources.Count)
      this.Weights[index] = this.Resources[index].Probability;
    this.value = RandomResource.GetRandomWeightedIndex(this.Weights, random.NextDouble());
  }

  public void PlaceRandom()
  {
    this.ClearPrefabs();
    if ((UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null && BiomeGenerator.Instance.ForceResource)
    {
      this.Weights = new int[BiomeGenerator.Instance.Resources.Count];
      int index1 = -1;
      while (++index1 < BiomeGenerator.Instance.Resources.Count)
        this.Weights[index1] = BiomeGenerator.Instance.Resources[index1].Probability;
      this.value = BiomeGenerator.Instance?.CurrentRoom == null ? RandomResource.GetRandomWeightedIndex(this.Weights, (double) UnityEngine.Random.Range(0.0f, 1f)) : RandomResource.GetRandomWeightedIndex(this.Weights, BiomeGenerator.Instance.CurrentRoom.RandomSeed.NextDouble());
      int index2 = -1;
      while (++index2 < this.Resources.Count)
      {
        if (this.Resources[index2].ResourceType == BiomeGenerator.Instance.Resources[this.value].ResourceType)
        {
          this.value = index2;
          break;
        }
      }
    }
    else
    {
      this.Weights = new int[this.Resources.Count];
      int index = -1;
      while (++index < this.Resources.Count)
        this.Weights[index] = this.Resources[index].Probability;
      this.value = BiomeGenerator.Instance?.CurrentRoom == null ? RandomResource.GetRandomWeightedIndex(this.Weights, (double) UnityEngine.Random.Range(0.0f, 1f)) : RandomResource.GetRandomWeightedIndex(this.Weights, BiomeGenerator.Instance.CurrentRoom.RandomSeed.NextDouble());
    }
    this.PlaceResource();
  }

  public void PlaceResource()
  {
    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.Resources[this.value].Prefab[UnityEngine.Random.Range(0, this.Resources[this.value].Prefab.Count)]);
    gameObject.transform.parent = this.transform;
    gameObject.transform.localPosition = Vector3.zero;
  }

  public static int GetRandomWeightedIndex(int[] weights, double Random)
  {
    if (weights == null || weights.Length == 0)
      return -1;
    int num1 = 0;
    for (int index = 0; index < weights.Length; ++index)
    {
      if (weights[index] >= 0)
        num1 += weights[index];
    }
    float num2 = 0.0f;
    for (int randomWeightedIndex = 0; randomWeightedIndex < weights.Length; ++randomWeightedIndex)
    {
      if ((double) weights[randomWeightedIndex] > 0.0)
      {
        num2 += (float) weights[randomWeightedIndex] / (float) num1;
        if ((double) num2 >= Random)
          return randomWeightedIndex;
      }
    }
    return -1;
  }

  public void ClearPrefabs()
  {
    int childCount = this.transform.childCount;
    while (--childCount >= 0)
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.transform.GetChild(childCount).gameObject);
  }

  [Serializable]
  public class Resource
  {
    public InventoryItem.ITEM_TYPE ResourceType;
    public List<GameObject> Prefab = new List<GameObject>();
    [Range(0.0f, 100f)]
    public int Probability = 50;
  }
}
