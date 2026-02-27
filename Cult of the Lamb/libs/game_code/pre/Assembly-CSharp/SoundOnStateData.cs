// Decompiled with JetBrains decompiler
// Type: SoundOnStateData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
