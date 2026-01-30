// Decompiled with JetBrains decompiler
// Type: Map.NestedExample
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Malee;
using System;
using UnityEngine;

#nullable disable
namespace Map;

public class NestedExample : MonoBehaviour
{
  [Reorderable]
  public NestedExample.ExampleChildList list;

  [Serializable]
  public class ExampleChild
  {
    [Reorderable(singleLine = true)]
    public NestedExample.NestedChildList nested;
  }

  [Serializable]
  public class NestedChild
  {
    public float myValue;
  }

  [Serializable]
  public class NestedChildCustomDrawer
  {
    public bool myBool;
    public GameObject myGameObject;
  }

  [Serializable]
  public class ExampleChildList : ReorderableArray<NestedExample.ExampleChild>
  {
  }

  [Serializable]
  public class NestedChildList : ReorderableArray<NestedExample.NestedChildCustomDrawer>
  {
  }
}
