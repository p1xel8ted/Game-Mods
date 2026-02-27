// Decompiled with JetBrains decompiler
// Type: Map.NestedExample
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
