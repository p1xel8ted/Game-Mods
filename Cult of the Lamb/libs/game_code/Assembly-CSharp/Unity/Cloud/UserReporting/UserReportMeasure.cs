// Decompiled with JetBrains decompiler
// Type: Unity.Cloud.UserReporting.UserReportMeasure
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;

#nullable disable
namespace Unity.Cloud.UserReporting;

public struct UserReportMeasure
{
  [CompilerGenerated]
  public int \u003CEndFrameNumber\u003Ek__BackingField;
  [CompilerGenerated]
  public List<UserReportNamedValue> \u003CMetadata\u003Ek__BackingField;
  [CompilerGenerated]
  public List<UserReportMetric> \u003CMetrics\u003Ek__BackingField;
  [CompilerGenerated]
  public int \u003CStartFrameNumber\u003Ek__BackingField;

  public int EndFrameNumber
  {
    readonly get => this.\u003CEndFrameNumber\u003Ek__BackingField;
    set => this.\u003CEndFrameNumber\u003Ek__BackingField = value;
  }

  public List<UserReportNamedValue> Metadata
  {
    readonly get => this.\u003CMetadata\u003Ek__BackingField;
    set => this.\u003CMetadata\u003Ek__BackingField = value;
  }

  public List<UserReportMetric> Metrics
  {
    readonly get => this.\u003CMetrics\u003Ek__BackingField;
    set => this.\u003CMetrics\u003Ek__BackingField = value;
  }

  public int StartFrameNumber
  {
    readonly get => this.\u003CStartFrameNumber\u003Ek__BackingField;
    set => this.\u003CStartFrameNumber\u003Ek__BackingField = value;
  }
}
