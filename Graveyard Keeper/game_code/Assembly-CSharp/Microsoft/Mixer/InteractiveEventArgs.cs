// Decompiled with JetBrains decompiler
// Type: Microsoft.Mixer.InteractiveEventArgs
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;

#nullable disable
namespace Microsoft.Mixer;

public class InteractiveEventArgs : EventArgs
{
  [CompilerGenerated]
  public DateTime \u003CTime\u003Ek__BackingField;
  [CompilerGenerated]
  public int \u003CErrorCode\u003Ek__BackingField;
  [CompilerGenerated]
  public string \u003CErrorMessage\u003Ek__BackingField;
  [CompilerGenerated]
  public InteractiveEventType \u003CEventType\u003Ek__BackingField;

  public InteractiveEventArgs()
  {
    this.Time = DateTime.UtcNow;
    this.ErrorCode = 0;
    this.ErrorMessage = string.Empty;
  }

  public DateTime Time
  {
    get => this.\u003CTime\u003Ek__BackingField;
    set => this.\u003CTime\u003Ek__BackingField = value;
  }

  public int ErrorCode
  {
    get => this.\u003CErrorCode\u003Ek__BackingField;
    set => this.\u003CErrorCode\u003Ek__BackingField = value;
  }

  public string ErrorMessage
  {
    get => this.\u003CErrorMessage\u003Ek__BackingField;
    set => this.\u003CErrorMessage\u003Ek__BackingField = value;
  }

  public InteractiveEventType EventType
  {
    get => this.\u003CEventType\u003Ek__BackingField;
    set => this.\u003CEventType\u003Ek__BackingField = value;
  }

  public InteractiveEventArgs(InteractiveEventType type)
  {
    this.Time = DateTime.UtcNow;
    this.ErrorCode = 0;
    this.ErrorMessage = string.Empty;
    this.EventType = type;
  }

  public InteractiveEventArgs(InteractiveEventType type, int errorCode, string errorMessage)
  {
    this.Time = DateTime.UtcNow;
    this.ErrorCode = errorCode;
    this.ErrorMessage = errorMessage;
    this.EventType = type;
  }
}
