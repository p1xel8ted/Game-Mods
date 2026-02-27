// Decompiled with JetBrains decompiler
// Type: BreakableDecorationObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using MMBiomeGeneration;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BreakableDecorationObject : BaseMonoBehaviour
{
  public BiomeConstants.TypeOfParticle ParticleType;
  public DestructableObjectAsset DestructableAsset;
  public GameObject DestroyPlaceholder;
  public GameObject DestroyRubble;
  private GameObject SpawnedDecoration;
  public static Dictionary<DestructableObjectAsset, GameObject> AssignedDecorationObject = new Dictionary<DestructableObjectAsset, GameObject>();

  private void Start()
  {
    Object.Destroy((Object) this.DestroyPlaceholder);
    GameObject gameObject;
    if (BreakableDecorationObject.AssignedDecorationObject.ContainsKey(this.DestructableAsset))
    {
      gameObject = BreakableDecorationObject.AssignedDecorationObject[this.DestructableAsset].gameObject;
    }
    else
    {
      int index = -1;
      int[] weights = new int[this.DestructableAsset.GameObjectAndProbabilities.Count];
      while (++index < this.DestructableAsset.GameObjectAndProbabilities.Count)
        weights[index] = this.DestructableAsset.GameObjectAndProbabilities[index].Probability;
      gameObject = this.DestructableAsset.GameObjectAndProbabilities[Utils.GetRandomWeightedIndex(weights)].GameObject;
      BreakableDecorationObject.AssignedDecorationObject.Add(this.DestructableAsset, gameObject);
    }
    this.SpawnedDecoration = Object.Instantiate<GameObject>(gameObject, this.transform);
    BiomeGenerator.OnBiomeLeftRoom += new BiomeGenerator.BiomeAction(this.OnBiomeLeftRoom);
  }

  private void OnBiomeLeftRoom()
  {
    BreakableDecorationObject.AssignedDecorationObject.Clear();
    BiomeGenerator.OnBiomeLeftRoom -= new BiomeGenerator.BiomeAction(this.OnBiomeLeftRoom);
  }
}
