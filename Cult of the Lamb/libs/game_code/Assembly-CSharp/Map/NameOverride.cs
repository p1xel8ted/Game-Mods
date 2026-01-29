// Decompiled with JetBrains decompiler
// Type: Map.NameOverride
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
