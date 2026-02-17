// Decompiled with JetBrains decompiler
// Type: Map.Example
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Malee;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Map;

public class Example : MonoBehaviour
{
  public List<Example.ExampleChild> list1;
  [Reorderable]
  public Example.ExampleChildList list2;
  [Reorderable]
  public Example.ExampleChildList list3;
  [Reorderable]
  public Example.StringList list4;
  [Reorderable]
  public Example.VectorList list5;

  [Serializable]
  public class ExampleChild
  {
    public string name;
    public float value;
    public Example.ExampleChild.ExampleEnum myEnum;
    public LayerMask layerMask;
    public long longValue;
    public char charValue;
    public byte byteValue;

    public enum ExampleEnum
    {
      EnumValue1,
      EnumValue2,
      EnumValue3,
    }
  }

  [Serializable]
  public class ExampleChildList : ReorderableArray<Example.ExampleChild>
  {
  }

  [Serializable]
  public class StringList : ReorderableArray<string>
  {
  }

  [Serializable]
  public class VectorList : ReorderableArray<Vector4>
  {
  }
}
