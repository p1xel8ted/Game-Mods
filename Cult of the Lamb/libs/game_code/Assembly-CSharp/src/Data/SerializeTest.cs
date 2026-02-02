// Decompiled with JetBrains decompiler
// Type: src.Data.SerializeTest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
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
