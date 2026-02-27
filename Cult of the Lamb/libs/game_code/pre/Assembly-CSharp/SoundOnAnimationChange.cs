// Decompiled with JetBrains decompiler
// Type: SoundOnAnimationChange
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private string cs;

  private string CurrentAnimation
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

  private void OnEnable()
  {
  }

  private void OnDisable() => this.DisableLoops();

  private void Start()
  {
  }

  private void DisableLoops()
  {
    foreach (SoundOnAnimationData sound in this.Sounds)
    {
      if (sound.position == SoundOnAnimationData.Position.Loop)
        AudioManager.Instance.StopLoop(sound.LoopedSound);
    }
  }

  private void Update()
  {
    if (!((Object) this.spine != (Object) null) || this.spine.state.GetCurrent(this.AnimationTrack) == null)
      return;
    this.CurrentAnimation = this.spine.state.GetCurrent(this.AnimationTrack).ToString();
    this.currentAnimationDebug = this.spine.state.GetCurrent(this.AnimationTrack).ToString();
  }
}
