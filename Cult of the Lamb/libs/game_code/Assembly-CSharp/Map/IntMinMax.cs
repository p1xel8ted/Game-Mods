// Decompiled with JetBrains decompiler
// Type: Map.IntMinMax
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
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
