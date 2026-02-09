// Decompiled with JetBrains decompiler
// Type: Microsoft.Mixer.InteractiveJoystickEventArgs
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;

#nullable disable
namespace Microsoft.Mixer;

public class InteractiveJoystickEventArgs : InteractiveEventArgs
{
  [CompilerGenerated]
  public string \u003CControlID\u003Ek__BackingField;
  [CompilerGenerated]
  public double \u003CX\u003Ek__BackingField;
  [CompilerGenerated]
  public double \u003CY\u003Ek__BackingField;
  [CompilerGenerated]
  public InteractiveParticipant \u003CParticipant\u003Ek__BackingField;

  public string ControlID
  {
    get => this.\u003CControlID\u003Ek__BackingField;
    set => this.\u003CControlID\u003Ek__BackingField = value;
  }

  public double X
  {
    get => this.\u003CX\u003Ek__BackingField;
    set => this.\u003CX\u003Ek__BackingField = value;
  }

  public double Y
  {
    get => this.\u003CY\u003Ek__BackingField;
    set => this.\u003CY\u003Ek__BackingField = value;
  }

  public InteractiveParticipant Participant
  {
    get => this.\u003CParticipant\u003Ek__BackingField;
    set => this.\u003CParticipant\u003Ek__BackingField = value;
  }

  public InteractiveJoystickEventArgs(
    InteractiveEventType type,
    string id,
    InteractiveParticipant participant,
    double x,
    double y)
    : base(type)
  {
    this.ControlID = id;
    this.Participant = participant;
    this.X = x;
    this.Y = -1.0 * y;
  }
}
