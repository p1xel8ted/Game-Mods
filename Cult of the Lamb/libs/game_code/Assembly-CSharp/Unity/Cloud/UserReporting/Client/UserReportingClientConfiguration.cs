// Decompiled with JetBrains decompiler
// Type: Unity.Cloud.UserReporting.Client.UserReportingClientConfiguration
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;

#nullable disable
namespace Unity.Cloud.UserReporting.Client;

public class UserReportingClientConfiguration
{
  [CompilerGenerated]
  public int \u003CFramesPerMeasure\u003Ek__BackingField;
  [CompilerGenerated]
  public int \u003CMaximumEventCount\u003Ek__BackingField;
  [CompilerGenerated]
  public int \u003CMaximumMeasureCount\u003Ek__BackingField;
  [CompilerGenerated]
  public int \u003CMaximumScreenshotCount\u003Ek__BackingField;
  [CompilerGenerated]
  public MetricsGatheringMode \u003CMetricsGatheringMode\u003Ek__BackingField;

  public UserReportingClientConfiguration()
  {
    this.MaximumEventCount = 100;
    this.MaximumMeasureCount = 300;
    this.FramesPerMeasure = 60;
    this.MaximumScreenshotCount = 10;
  }

  public UserReportingClientConfiguration(
    int maximumEventCount,
    int maximumMeasureCount,
    int framesPerMeasure,
    int maximumScreenshotCount)
  {
    this.MaximumEventCount = maximumEventCount;
    this.MaximumMeasureCount = maximumMeasureCount;
    this.FramesPerMeasure = framesPerMeasure;
    this.MaximumScreenshotCount = maximumScreenshotCount;
  }

  public UserReportingClientConfiguration(
    int maximumEventCount,
    MetricsGatheringMode metricsGatheringMode,
    int maximumMeasureCount,
    int framesPerMeasure,
    int maximumScreenshotCount)
  {
    this.MaximumEventCount = maximumEventCount;
    this.MetricsGatheringMode = metricsGatheringMode;
    this.MaximumMeasureCount = maximumMeasureCount;
    this.FramesPerMeasure = framesPerMeasure;
    this.MaximumScreenshotCount = maximumScreenshotCount;
  }

  public int FramesPerMeasure
  {
    get => this.\u003CFramesPerMeasure\u003Ek__BackingField;
    set => this.\u003CFramesPerMeasure\u003Ek__BackingField = value;
  }

  public int MaximumEventCount
  {
    get => this.\u003CMaximumEventCount\u003Ek__BackingField;
    set => this.\u003CMaximumEventCount\u003Ek__BackingField = value;
  }

  public int MaximumMeasureCount
  {
    get => this.\u003CMaximumMeasureCount\u003Ek__BackingField;
    set => this.\u003CMaximumMeasureCount\u003Ek__BackingField = value;
  }

  public int MaximumScreenshotCount
  {
    get => this.\u003CMaximumScreenshotCount\u003Ek__BackingField;
    set => this.\u003CMaximumScreenshotCount\u003Ek__BackingField = value;
  }

  public MetricsGatheringMode MetricsGatheringMode
  {
    get => this.\u003CMetricsGatheringMode\u003Ek__BackingField;
    set => this.\u003CMetricsGatheringMode\u003Ek__BackingField = value;
  }
}
