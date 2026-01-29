// Decompiled with JetBrains decompiler
// Type: IntRange
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;

#nullable disable
public class IntRange
{
  [CompilerGenerated]
  public int \u003CMin\u003Ek__BackingField;
  [CompilerGenerated]
  public int \u003CMax\u003Ek__BackingField;

  public int Min => this.\u003CMin\u003Ek__BackingField;

  public int Max => this.\u003CMax\u003Ek__BackingField;

  public IntRange(int min, int max)
  {
    this.\u003CMin\u003Ek__BackingField = min;
    this.\u003CMax\u003Ek__BackingField = max;
  }

  public int Random() => this.Min == this.Max ? this.Min : UnityEngine.Random.Range(this.Min, this.Max + 1);

  public int Random(int seed)
  {
    if (this.Min == this.Max)
      return this.Min;
    UnityEngine.Random.InitState(seed);
    return UnityEngine.Random.Range(this.Min, this.Max + 1);
  }

  public override string ToString()
  {
    if (this.Min == this.Max)
      return this.Min.ToString();
    return string.Join(" ", (object) this.Min, (object) "-", (object) this.Max);
  }
}
