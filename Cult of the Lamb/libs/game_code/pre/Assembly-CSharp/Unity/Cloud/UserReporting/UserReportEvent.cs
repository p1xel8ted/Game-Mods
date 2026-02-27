// Decompiled with JetBrains decompiler
// Type: Unity.Cloud.UserReporting.UserReportEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace Unity.Cloud.UserReporting;

public struct UserReportEvent
{
  public SerializableException Exception { get; set; }

  public int FrameNumber { get; set; }

  public string FullMessage => $"{this.Message}{Environment.NewLine}{this.StackTrace}";

  public UserReportEventLevel Level { get; set; }

  public string Message { get; set; }

  public string StackTrace { get; set; }

  public DateTime Timestamp { get; set; }
}
