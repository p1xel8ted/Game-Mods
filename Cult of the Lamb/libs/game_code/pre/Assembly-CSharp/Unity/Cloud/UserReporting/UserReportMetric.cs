// Decompiled with JetBrains decompiler
// Type: Unity.Cloud.UserReporting.UserReportMetric
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace Unity.Cloud.UserReporting;

public struct UserReportMetric
{
  public double Average => this.Sum / (double) this.Count;

  public int Count { get; set; }

  public double Maximum { get; set; }

  public double Minimum { get; set; }

  public string Name { get; set; }

  public double Sum { get; set; }

  public void Sample(double value)
  {
    if (this.Count == 0)
    {
      this.Minimum = double.MaxValue;
      this.Maximum = double.MinValue;
    }
    ++this.Count;
    this.Sum += value;
    this.Minimum = Math.Min(this.Minimum, value);
    this.Maximum = Math.Max(this.Maximum, value);
  }
}
