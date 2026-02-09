// Decompiled with JetBrains decompiler
// Type: Microsoft.Mixer.InteractiveTextResult
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;

#nullable disable
namespace Microsoft.Mixer;

public struct InteractiveTextResult
{
  [CompilerGenerated]
  public InteractiveParticipant \u003CParticipant\u003Ek__BackingField;
  [CompilerGenerated]
  public string \u003CText\u003Ek__BackingField;

  public InteractiveParticipant Participant
  {
    readonly get => this.\u003CParticipant\u003Ek__BackingField;
    set => this.\u003CParticipant\u003Ek__BackingField = value;
  }

  public string Text
  {
    readonly get => this.\u003CText\u003Ek__BackingField;
    set => this.\u003CText\u003Ek__BackingField = value;
  }
}
