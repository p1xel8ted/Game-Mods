// Decompiled with JetBrains decompiler
// Type: SoundOnStateData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
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
