// Decompiled with JetBrains decompiler
// Type: IntRange
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class IntRange
{
  public int Min { get; }

  public int Max { get; }

  public IntRange(int min, int max)
  {
    this.Min = min;
    this.Max = max;
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
