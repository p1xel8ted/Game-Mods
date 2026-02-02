// Decompiled with JetBrains decompiler
// Type: Unity.Cloud.UserReporting.UserReportMetric
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;

#nullable disable
namespace Unity.Cloud.UserReporting;

public struct UserReportMetric
{
  [CompilerGenerated]
  public int \u003CCount\u003Ek__BackingField;
  [CompilerGenerated]
  public double \u003CMaximum\u003Ek__BackingField;
  [CompilerGenerated]
  public double \u003CMinimum\u003Ek__BackingField;
  [CompilerGenerated]
  public string \u003CName\u003Ek__BackingField;
  [CompilerGenerated]
  public double \u003CSum\u003Ek__BackingField;

  public double Average => this.Sum / (double) this.Count;

  public int Count
  {
    readonly get => this.\u003CCount\u003Ek__BackingField;
    set => this.\u003CCount\u003Ek__BackingField = value;
  }

  public double Maximum
  {
    readonly get => this.\u003CMaximum\u003Ek__BackingField;
    set => this.\u003CMaximum\u003Ek__BackingField = value;
  }

  public double Minimum
  {
    readonly get => this.\u003CMinimum\u003Ek__BackingField;
    set => this.\u003CMinimum\u003Ek__BackingField = value;
  }

  public string Name
  {
    readonly get => this.\u003CName\u003Ek__BackingField;
    set => this.\u003CName\u003Ek__BackingField = value;
  }

  public double Sum
  {
    readonly get => this.\u003CSum\u003Ek__BackingField;
    set => this.\u003CSum\u003Ek__BackingField = value;
  }

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
