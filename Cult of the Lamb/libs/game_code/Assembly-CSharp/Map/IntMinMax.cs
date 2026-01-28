// Decompiled with JetBrains decompiler
// Type: Map.IntMinMax
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;

#nullable disable
namespace Map;

[Serializable]
public class IntMinMax
{
  public int min;
  public int max;

  public int GetValue() => UnityEngine.Random.Range(this.min, this.max + 1);
}
