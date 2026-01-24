// Decompiled with JetBrains decompiler
// Type: TypeAndPlacementObjects
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

#nullable disable
public class TypeAndPlacementObjects : BaseMonoBehaviour
{
  public static TypeAndPlacementObjects _Instance;
  public List<global::TypeAndPlacementObject> TypeAndPlacementObject = new List<global::TypeAndPlacementObject>();

  public void Awake() => TypeAndPlacementObjects._Instance = this;

  public void OnDestroy()
  {
    TypeAndPlacementObjects._Instance = (TypeAndPlacementObjects) null;
    foreach (global::TypeAndPlacementObject andPlacementObject in this.TypeAndPlacementObject)
    {
      if (andPlacementObject != null && andPlacementObject.AddrHandle.IsValid())
        Addressables.Release<GameObject>(andPlacementObject.AddrHandle);
    }
    this.TypeAndPlacementObject.Clear();
  }

  public static global::TypeAndPlacementObject GetByType(StructureBrain.TYPES Type)
  {
    if ((Object) TypeAndPlacementObjects._Instance != (Object) null)
    {
      foreach (global::TypeAndPlacementObject byType in TypeAndPlacementObjects._Instance.TypeAndPlacementObject)
      {
        if (byType.Type == Type)
          return byType;
      }
    }
    return (global::TypeAndPlacementObject) null;
  }

  public enum Tier
  {
    Zero,
    One,
    Two,
    Three,
  }
}
