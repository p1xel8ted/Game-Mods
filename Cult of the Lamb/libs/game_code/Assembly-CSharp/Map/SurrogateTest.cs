// Decompiled with JetBrains decompiler
// Type: Map.SurrogateTest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Malee;
using System;
using UnityEngine;

#nullable disable
namespace Map;

public class SurrogateTest : MonoBehaviour
{
  [SerializeField]
  public SurrogateTest.MyClass[] objects;
  [SerializeField]
  [Reorderable(surrogateType = typeof (GameObject), surrogateProperty = "gameObject")]
  public SurrogateTest.MyClassArray myClassArray;

  [Serializable]
  public class MyClass
  {
    public string name;
    public GameObject gameObject;
  }

  [Serializable]
  public class MyClassArray : ReorderableArray<SurrogateTest.MyClass>
  {
  }
}
