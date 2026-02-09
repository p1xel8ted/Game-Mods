// Decompiled with JetBrains decompiler
// Type: DarkTonic.MasterAudio.OcclusionFreqChangeInfo
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace DarkTonic.MasterAudio;

[Serializable]
public class OcclusionFreqChangeInfo
{
  public SoundGroupVariation ActingVariation;
  public float StartFrequency;
  public float TargetFrequency;
  public float StartTime;
  public float CompletionTime;
  public bool IsActive = true;
}
