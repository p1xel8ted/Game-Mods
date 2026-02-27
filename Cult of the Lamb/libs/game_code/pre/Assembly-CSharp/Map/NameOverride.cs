// Decompiled with JetBrains decompiler
// Type: Map.NameOverride
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Malee;
using System;
using UnityEngine;

#nullable disable
namespace Map;

public class NameOverride : MonoBehaviour
{
  public string nameOverride = "Car";
  public string nestedNameOverride = "Car Part";
  [Reorderable(null, "Car", null)]
  public NameOverride.ExampleChildList autoNameList;
  [Reorderable]
  public NameOverride.DynamicExampleChildList dynamicNameList;

  [Serializable]
  public class ExampleChild
  {
    [Reorderable(null, "Car Part", null)]
    public NameOverride.StringList nested;
  }

  [Serializable]
  public class DynamicExampleChild
  {
    [Reorderable]
    public NameOverride.StringList nested;
  }

  [Serializable]
  public class ExampleChildList : ReorderableArray<NameOverride.ExampleChild>
  {
  }

  [Serializable]
  public class DynamicExampleChildList : ReorderableArray<NameOverride.DynamicExampleChild>
  {
  }

  [Serializable]
  public class StringList : ReorderableArray<string>
  {
  }
}
