// Decompiled with JetBrains decompiler
// Type: src.Data.SerializeTest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
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
