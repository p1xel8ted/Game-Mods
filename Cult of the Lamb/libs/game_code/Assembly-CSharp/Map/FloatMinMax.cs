// Decompiled with JetBrains decompiler
// Type: Map.FloatMinMax
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
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
