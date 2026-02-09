// Decompiled with JetBrains decompiler
// Type: Microsoft.Mixer.InteractivityStateChangedEventArgs
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;

#nullable disable
namespace Microsoft.Mixer;

public class InteractivityStateChangedEventArgs : InteractiveEventArgs
{
  [CompilerGenerated]
  public InteractivityState \u003CState\u003Ek__BackingField;

  public InteractivityState State
  {
    get => this.\u003CState\u003Ek__BackingField;
    set => this.\u003CState\u003Ek__BackingField = value;
  }

  public InteractivityStateChangedEventArgs(InteractiveEventType type, InteractivityState state)
    : base(type)
  {
    this.State = state;
  }
}
