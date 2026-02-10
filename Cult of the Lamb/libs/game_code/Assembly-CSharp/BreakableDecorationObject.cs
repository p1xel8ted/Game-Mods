// Decompiled with JetBrains decompiler
// Type: BreakableDecorationObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
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
    BreakableDecorationObject context = this;
    if ((Object) context.DestructableAsset == (Object) null)
      Debug.LogError((object) "Load(): DestructableAsset is null", (Object) context);
    else if (BreakableDecorationObject.AssignedDecorationObject == null || BreakableDecorationObject.AssignedDecorationReference == null)
      Debug.LogError((object) "Load(): decoration dictionaries not initialized", (Object) context);
    else if (context.DestructableAsset.GameObjectAndProbabilities == null || context.DestructableAsset.GameObjectAndProbabilities.Count == 0)
    {
      Debug.LogError((object) "Load(): GameObjectAndProbabilities missing/empty", (Object) context);
    }
    else
    {
      if ((Object) context.DestroyPlaceholder != (Object) null)
        Object.Destroy((Object) context.DestroyPlaceholder);
      GameObject original;
      if (BreakableDecorationObject.AssignedDecorationObject.ContainsKey(context.DestructableAsset))
      {
        original = BreakableDecorationObject.AssignedDecorationObject[context.DestructableAsset].gameObject;
      }
      else
      {
        int index = -1;
        int[] weights = new int[context.DestructableAsset.GameObjectAndProbabilities.Count];
        while (++index < context.DestructableAsset.GameObjectAndProbabilities.Count)
          weights[index] = context.DestructableAsset.GameObjectAndProbabilities[index].Probability;
        int randomWeightedIndex = Utils.GetRandomWeightedIndex(weights);
        AssetReferenceGameObject assetReference = context.DestructableAsset.GameObjectAndProbabilities[randomWeightedIndex].GameObjectAddr;
        bool spawnByReference = false;
        if (!BreakableDecorationObject.AssignedDecorationReference.ContainsKey(context.DestructableAsset))
        {
          BreakableDecorationObject.AssignedDecorationReference.Add(context.DestructableAsset, assetReference);
          spawnByReference = true;
        }
        yield return (object) null;
        if (spawnByReference)
        {
          AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>((object) assetReference);
          context.handle = asyncOperationHandle;
          asyncOperationHandle.WaitForCompletion();
          original = asyncOperationHandle.Result;
          if (!BreakableDecorationObject.AssignedDecorationObject.ContainsKey(context.DestructableAsset))
            BreakableDecorationObject.AssignedDecorationObject.Add(context.DestructableAsset, original);
        }
        else
        {
          while (!BreakableDecorationObject.AssignedDecorationObject.ContainsKey(context.DestructableAsset))
            yield return (object) null;
          original = BreakableDecorationObject.AssignedDecorationObject[context.DestructableAsset].gameObject;
        }
        assetReference = (AssetReferenceGameObject) null;
      }
      if ((Object) original == (Object) null)
      {
        Debug.LogError((object) "Load(): spawnable is null (addressables load failed or missing asset)", (Object) context);
      }
      else
      {
        context.SpawnedDecoration = Object.Instantiate<GameObject>(original, context.transform);
        BiomeGenerator.OnBiomeLeftRoom += new BiomeGenerator.BiomeAction(context.OnBiomeLeftRoom);
      }
    }
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
