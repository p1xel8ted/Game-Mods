// Decompiled with JetBrains decompiler
// Type: Microsoft.Mixer.InteractiveMouseButtonEventArgs
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace Microsoft.Mixer;

public class InteractiveMouseButtonEventArgs : InteractiveEventArgs
{
  [CompilerGenerated]
  public string \u003CControlID\u003Ek__BackingField;
  [CompilerGenerated]
  public InteractiveParticipant \u003CParticipant\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CIsPressed\u003Ek__BackingField;
  [CompilerGenerated]
  public Vector3 \u003CPosition\u003Ek__BackingField;

  public string ControlID
  {
    get => this.\u003CControlID\u003Ek__BackingField;
    set => this.\u003CControlID\u003Ek__BackingField = value;
  }

  public InteractiveParticipant Participant
  {
    get => this.\u003CParticipant\u003Ek__BackingField;
    set => this.\u003CParticipant\u003Ek__BackingField = value;
  }

  public bool IsPressed
  {
    get => this.\u003CIsPressed\u003Ek__BackingField;
    set => this.\u003CIsPressed\u003Ek__BackingField = value;
  }

  public Vector3 Position
  {
    get => this.\u003CPosition\u003Ek__BackingField;
    set => this.\u003CPosition\u003Ek__BackingField = value;
  }

  public InteractiveMouseButtonEventArgs(
    string id,
    InteractiveParticipant participant,
    bool isPressed,
    Vector3 position)
    : base(InteractiveEventType.MouseButton)
  {
    this.ControlID = id;
    this.Participant = participant;
    this.IsPressed = isPressed;
    this.Position = position;
  }
}
