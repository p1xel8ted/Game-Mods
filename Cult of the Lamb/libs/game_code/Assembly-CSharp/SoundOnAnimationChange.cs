// Decompiled with JetBrains decompiler
// Type: SoundOnAnimationChange
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SoundOnAnimationChange : BaseMonoBehaviour
{
  public List<SoundOnAnimationData> Sounds = new List<SoundOnAnimationData>();
  public SkeletonAnimation spine;
  public int AnimationTrack;
  public string currentAnimationDebug;
  public string cs;

  public string CurrentAnimation
  {
    set
    {
      if (this.cs != value)
      {
        foreach (SoundOnAnimationData sound in this.Sounds)
        {
          AudioManager.Instance.StopLoop(sound.LoopedSound);
          if (sound.SkeletonData.state.GetCurrent(this.AnimationTrack).ToString() == this.cs && sound.position == SoundOnAnimationData.Position.Beginning || sound.SkeletonData.state.GetCurrent(this.AnimationTrack).ToString() == this.cs && sound.position == SoundOnAnimationData.Position.End)
            AudioManager.Instance.PlayOneShot(sound.AudioSourcePath, this.transform.position);
          else if (sound.SkeletonData.state.GetCurrent(this.AnimationTrack).ToString() == sound.SkeletonsAnimations && sound.position == SoundOnAnimationData.Position.Loop)
            sound.LoopedSound = AudioManager.Instance.CreateLoop(sound.AudioSourcePath, this.gameObject, true);
        }
      }
      this.cs = value;
    }
  }

  public void OnEnable()
  {
  }

  public void OnDisable() => this.DisableLoops();

  public void Start()
  {
  }

  public void DisableLoops()
  {
    foreach (SoundOnAnimationData sound in this.Sounds)
    {
      if (sound.position == SoundOnAnimationData.Position.Loop)
        AudioManager.Instance.StopLoop(sound.LoopedSound);
    }
  }

  public void Update()
  {
    if (!((Object) this.spine != (Object) null) || this.spine.state.GetCurrent(this.AnimationTrack) == null)
      return;
    this.CurrentAnimation = this.spine.state.GetCurrent(this.AnimationTrack).ToString();
    this.currentAnimationDebug = this.spine.state.GetCurrent(this.AnimationTrack).ToString();
  }
}
