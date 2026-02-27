// Decompiled with JetBrains decompiler
// Type: DestructableObjectAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

#nullable disable
[CreateAssetMenu]
public class DestructableObjectAsset : ScriptableObject
{
  public List<DestructableObjectAsset.GameObjectAndProbability> GameObjectAndProbabilities = new List<DestructableObjectAsset.GameObjectAndProbability>();

  [Serializable]
  public class GameObjectAndProbability
  {
    public AssetReferenceGameObject GameObjectAddr;
    [Range(0.0f, 100f)]
    public int Probability = 100;
  }
}
