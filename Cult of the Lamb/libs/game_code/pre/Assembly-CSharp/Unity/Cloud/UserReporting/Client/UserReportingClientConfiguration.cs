// Decompiled with JetBrains decompiler
// Type: Unity.Cloud.UserReporting.Client.UserReportingClientConfiguration
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace Unity.Cloud.UserReporting.Client;

public class UserReportingClientConfiguration
{
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

  public int FramesPerMeasure { get; internal set; }

  public int MaximumEventCount { get; internal set; }

  public int MaximumMeasureCount { get; internal set; }

  public int MaximumScreenshotCount { get; internal set; }

  public MetricsGatheringMode MetricsGatheringMode { get; internal set; }
}
