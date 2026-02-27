// Decompiled with JetBrains decompiler
// Type: TypeAndPlacementObjects
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class TypeAndPlacementObjects : BaseMonoBehaviour
{
  private static TypeAndPlacementObjects _Instance;
  public List<global::TypeAndPlacementObject> TypeAndPlacementObject = new List<global::TypeAndPlacementObject>();

  private void Awake() => TypeAndPlacementObjects._Instance = this;

  private void OnDestroy()
  {
    TypeAndPlacementObjects._Instance = (TypeAndPlacementObjects) null;
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
