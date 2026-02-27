// Decompiled with JetBrains decompiler
// Type: Unity.Cloud.UserReporting.UserReportMeasure
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace Unity.Cloud.UserReporting;

public struct UserReportMeasure
{
  public int EndFrameNumber { get; set; }

  public List<UserReportNamedValue> Metadata { get; set; }

  public List<UserReportMetric> Metrics { get; set; }

  public int StartFrameNumber { get; set; }
}
