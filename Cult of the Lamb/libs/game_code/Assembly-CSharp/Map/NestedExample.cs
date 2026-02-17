// Decompiled with JetBrains decompiler
// Type: Map.NestedExample
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
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
