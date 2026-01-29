// Decompiled with JetBrains decompiler
// Type: SoundOnStateData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using FMODUnity;
using System;

#nullable disable
[Serializable]
public class SoundOnStateData
{
  public StateMachine.State State;
  public EventInstance LoopedSound;
  [EventRef]
  public string AudioSourcePath = string.Empty;
  public SoundOnStateData.Position position;

  public enum Position
  {
    Beginning,
    End,
    Loop,
  }
}
