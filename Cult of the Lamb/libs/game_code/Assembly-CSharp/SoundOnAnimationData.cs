// Decompiled with JetBrains decompiler
// Type: SoundOnAnimationData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using FMODUnity;
using Spine.Unity;
using System;

#nullable disable
[Serializable]
public class SoundOnAnimationData
{
  public EventInstance LoopedSound;
  public SkeletonAnimation SkeletonData;
  [SpineAnimation("", "SkeletonData", true, false)]
  public string SkeletonsAnimations;
  [EventRef]
  public string AudioSourcePath = string.Empty;
  public SoundOnAnimationData.Position position;

  public enum Position
  {
    Beginning,
    End,
    Loop,
  }
}
