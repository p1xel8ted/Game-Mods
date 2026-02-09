// Decompiled with JetBrains decompiler
// Type: DarkTonic.MasterAudio.PlaySoundResult
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace DarkTonic.MasterAudio;

[SerializeField]
public class PlaySoundResult
{
  [CompilerGenerated]
  public bool \u003CSoundPlayed\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CSoundScheduled\u003Ek__BackingField;
  [CompilerGenerated]
  public SoundGroupVariation \u003CActingVariation\u003Ek__BackingField;

  public PlaySoundResult()
  {
    this.SoundPlayed = false;
    this.SoundScheduled = false;
    this.ActingVariation = (SoundGroupVariation) null;
  }

  public bool SoundPlayed
  {
    get => this.\u003CSoundPlayed\u003Ek__BackingField;
    set => this.\u003CSoundPlayed\u003Ek__BackingField = value;
  }

  public bool SoundScheduled
  {
    get => this.\u003CSoundScheduled\u003Ek__BackingField;
    set => this.\u003CSoundScheduled\u003Ek__BackingField = value;
  }

  public SoundGroupVariation ActingVariation
  {
    get => this.\u003CActingVariation\u003Ek__BackingField;
    set => this.\u003CActingVariation\u003Ek__BackingField = value;
  }
}
