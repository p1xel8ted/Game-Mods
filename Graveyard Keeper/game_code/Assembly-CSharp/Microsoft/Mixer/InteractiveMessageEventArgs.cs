// Decompiled with JetBrains decompiler
// Type: Microsoft.Mixer.InteractiveMessageEventArgs
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;

#nullable disable
namespace Microsoft.Mixer;

public class InteractiveMessageEventArgs : InteractiveEventArgs
{
  [CompilerGenerated]
  public string \u003CMessage\u003Ek__BackingField;

  public string Message
  {
    get => this.\u003CMessage\u003Ek__BackingField;
    set => this.\u003CMessage\u003Ek__BackingField = value;
  }

  public InteractiveMessageEventArgs(string message) => this.Message = message;
}
