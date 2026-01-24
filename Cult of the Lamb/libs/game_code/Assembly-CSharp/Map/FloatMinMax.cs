// Decompiled with JetBrains decompiler
// Type: Map.FloatMinMax
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;

#nullable disable
namespace Map;

[Serializable]
public class FloatMinMax
{
  public float min;
  public float max;

  public float GetValue() => UnityEngine.Random.Range(this.min, this.max);
}
