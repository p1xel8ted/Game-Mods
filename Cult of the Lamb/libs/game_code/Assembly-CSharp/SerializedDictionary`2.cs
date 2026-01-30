// Decompiled with JetBrains decompiler
// Type: SerializedDictionary`2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public abstract class SerializedDictionary<TKey, TValue> : 
  Dictionary<TKey, TValue>,
  ISerializationCallbackReceiver
{
  [SerializeField]
  [HideInInspector]
  public List<TKey> keyData = new List<TKey>();
  [SerializeField]
  [HideInInspector]
  public List<TValue> valueData = new List<TValue>();

  void ISerializationCallbackReceiver.OnAfterDeserialize()
  {
    this.Clear();
    for (int index = 0; index < this.keyData.Count && index < this.valueData.Count; ++index)
      this[this.keyData[index]] = this.valueData[index];
  }

  void ISerializationCallbackReceiver.OnBeforeSerialize()
  {
    this.keyData.Clear();
    this.valueData.Clear();
    foreach (KeyValuePair<TKey, TValue> keyValuePair in (Dictionary<TKey, TValue>) this)
    {
      this.keyData.Add(keyValuePair.Key);
      this.valueData.Add(keyValuePair.Value);
    }
  }
}
