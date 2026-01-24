// Decompiled with JetBrains decompiler
// Type: BreakableDecorationObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMBiomeGeneration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class BreakableDecorationObject : BaseMonoBehaviour
{
  public BiomeConstants.TypeOfParticle ParticleType;
  public DestructableObjectAsset DestructableAsset;
  public GameObject DestroyPlaceholder;
  public GameObject DestroyRubble;
  public GameObject SpawnedDecoration;
  public static Dictionary<DestructableObjectAsset, AssetReferenceGameObject> AssignedDecorationReference = new Dictionary<DestructableObjectAsset, AssetReferenceGameObject>();
  public static Dictionary<DestructableObjectAsset, GameObject> AssignedDecorationObject = new Dictionary<DestructableObjectAsset, GameObject>();
  public AsyncOperationHandle<GameObject> handle;

  public void Start() => GameManager.GetInstance().StartCoroutine((IEnumerator) this.Load());

  public IEnumerator Load()
  {
    BreakableDecorationObject decorationObject = this;
    if ((Object) decorationObject.DestroyPlaceholder != (Object) null)
      Object.Destroy((Object) decorationObject.DestroyPlaceholder);
    GameObject original;
    if (BreakableDecorationObject.AssignedDecorationObject.ContainsKey(decorationObject.DestructableAsset))
    {
      original = BreakableDecorationObject.AssignedDecorationObject[decorationObject.DestructableAsset].gameObject;
    }
    else
    {
      int index = -1;
      int[] weights = new int[decorationObject.DestructableAsset.GameObjectAndProbabilities.Count];
      while (++index < decorationObject.DestructableAsset.GameObjectAndProbabilities.Count)
        weights[index] = decorationObject.DestructableAsset.GameObjectAndProbabilities[index].Probability;
      int randomWeightedIndex = Utils.GetRandomWeightedIndex(weights);
      AssetReferenceGameObject assetReference = decorationObject.DestructableAsset.GameObjectAndProbabilities[randomWeightedIndex].GameObjectAddr;
      bool spawnByReference = false;
      if (!BreakableDecorationObject.AssignedDecorationReference.ContainsKey(decorationObject.DestructableAsset))
      {
        BreakableDecorationObject.AssignedDecorationReference.Add(decorationObject.DestructableAsset, assetReference);
        spawnByReference = true;
      }
      yield return (object) null;
      if (spawnByReference)
      {
        AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>((object) assetReference);
        decorationObject.handle = asyncOperationHandle;
        asyncOperationHandle.WaitForCompletion();
        original = asyncOperationHandle.Result;
        if (!BreakableDecorationObject.AssignedDecorationObject.ContainsKey(decorationObject.DestructableAsset))
          BreakableDecorationObject.AssignedDecorationObject.Add(decorationObject.DestructableAsset, original);
      }
      else
      {
        while (!BreakableDecorationObject.AssignedDecorationObject.ContainsKey(decorationObject.DestructableAsset))
          yield return (object) null;
        original = BreakableDecorationObject.AssignedDecorationObject[decorationObject.DestructableAsset].gameObject;
      }
      assetReference = (AssetReferenceGameObject) null;
    }
    decorationObject.SpawnedDecoration = Object.Instantiate<GameObject>(original, decorationObject.transform);
    BiomeGenerator.OnBiomeLeftRoom += new BiomeGenerator.BiomeAction(decorationObject.OnBiomeLeftRoom);
  }

  public void OnDestroy()
  {
    if (!this.handle.IsValid())
      return;
    Addressables.Release<GameObject>(this.handle);
  }

  public void OnBiomeLeftRoom()
  {
    BreakableDecorationObject.AssignedDecorationObject.Clear();
    BreakableDecorationObject.AssignedDecorationReference.Clear();
    BiomeGenerator.OnBiomeLeftRoom -= new BiomeGenerator.BiomeAction(this.OnBiomeLeftRoom);
  }
}
