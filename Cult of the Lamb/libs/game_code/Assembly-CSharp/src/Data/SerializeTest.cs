// Decompiled with JetBrains decompiler
// Type: src.Data.SerializeTest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace src.Data;

[Serializable]
public class SerializeTest
{
  public Vector3 TestVector = new Vector3(1f, 10f, 200f);
  public float TestFloat = 10f;
  public bool Bool = true;
}
