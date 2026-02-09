// Decompiled with JetBrains decompiler
// Type: Unity.Cloud.UserReporting.UserReportEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;

#nullable disable
namespace Unity.Cloud.UserReporting;

public struct UserReportEvent
{
  [CompilerGenerated]
  public SerializableException \u003CException\u003Ek__BackingField;
  [CompilerGenerated]
  public int \u003CFrameNumber\u003Ek__BackingField;
  [CompilerGenerated]
  public UserReportEventLevel \u003CLevel\u003Ek__BackingField;
  [CompilerGenerated]
  public string \u003CMessage\u003Ek__BackingField;
  [CompilerGenerated]
  public string \u003CStackTrace\u003Ek__BackingField;
  [CompilerGenerated]
  public DateTime \u003CTimestamp\u003Ek__BackingField;

  public SerializableException Exception
  {
    readonly get => this.\u003CException\u003Ek__BackingField;
    set => this.\u003CException\u003Ek__BackingField = value;
  }

  public int FrameNumber
  {
    readonly get => this.\u003CFrameNumber\u003Ek__BackingField;
    set => this.\u003CFrameNumber\u003Ek__BackingField = value;
  }

  public string FullMessage => $"{this.Message}{Environment.NewLine}{this.StackTrace}";

  public UserReportEventLevel Level
  {
    readonly get => this.\u003CLevel\u003Ek__BackingField;
    set => this.\u003CLevel\u003Ek__BackingField = value;
  }

  public string Message
  {
    readonly get => this.\u003CMessage\u003Ek__BackingField;
    set => this.\u003CMessage\u003Ek__BackingField = value;
  }

  public string StackTrace
  {
    readonly get => this.\u003CStackTrace\u003Ek__BackingField;
    set => this.\u003CStackTrace\u003Ek__BackingField = value;
  }

  public DateTime Timestamp
  {
    readonly get => this.\u003CTimestamp\u003Ek__BackingField;
    set => this.\u003CTimestamp\u003Ek__BackingField = value;
  }
}
