// Decompiled with JetBrains decompiler
// Type: SinWormNPC
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using Spine;
using Spine.Unity;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class SinWormNPC : MonoBehaviour
{
  [SerializeField]
  public SkeletonAnimation SkeletonData;
  [SerializeField]
  [SpineAnimation("", "SkeletonData", true, false)]
  public string initAnimation;
  [SerializeField]
  [SpineAnimation("", "SkeletonData", true, false)]
  public string vomitAnimation;
  [SerializeField]
  [SpineAnimation("", "SkeletonData", true, false)]
  public string loopAnimation;
  [SerializeField]
  public string shoutSfx;
  public bool isPlaying;
  public EventInstance wormVomitLoopInstance;
  public string wormVomitStartSFX = "event:/dlc/env/sin_room/worm_vomit_start";
  public string wormVomitLoopSFX = "event:/dlc/env/sin_room/worm_vomit_loop";

  public void OnEnable()
  {
    if (!this.isPlaying)
      return;
    AudioManager.Instance.StopLoop(this.wormVomitLoopInstance);
    this.wormVomitLoopInstance = AudioManager.Instance.CreateLoop(this.wormVomitLoopSFX, this.gameObject, true);
  }

  public void OnDisable() => AudioManager.Instance.StopLoop(this.wormVomitLoopInstance);

  public void Play()
  {
    if (this.isPlaying)
      return;
    this.isPlaying = true;
    this.loopAnimation = this.SkeletonData.AnimationName;
    if (!string.IsNullOrEmpty(this.shoutSfx))
      AudioManager.Instance.PlayOneShot(this.shoutSfx, this.transform.position);
    AudioManager.Instance.PlayOneShot(this.wormVomitStartSFX, this.gameObject);
    this.SkeletonData.AnimationState.SetAnimation(0, this.initAnimation, false);
    this.SkeletonData.AnimationState.AddAnimation(0, this.vomitAnimation, true, 0.0f).Start += new Spine.AnimationState.TrackEntryDelegate(this.OnVomitLoopStart);
    this.StartCoroutine((IEnumerator) this.WaitForSeconds(UnityEngine.Random.Range(1.5f, 3f), (System.Action) (() =>
    {
      this.SkeletonData.AnimationState.SetAnimation(0, this.loopAnimation, true);
      AudioManager.Instance.StopLoop(this.wormVomitLoopInstance);
      this.isPlaying = false;
    })));
  }

  public IEnumerator WaitForSeconds(float seconds, System.Action callback)
  {
    yield return (object) new UnityEngine.WaitForSeconds(seconds);
    System.Action action = callback;
    if (action != null)
      action();
  }

  public void OnVomitLoopStart(TrackEntry trackEntry)
  {
    this.wormVomitLoopInstance = AudioManager.Instance.CreateLoop(this.wormVomitLoopSFX, this.gameObject, true);
  }

  [CompilerGenerated]
  public void \u003CPlay\u003Eb__11_0()
  {
    this.SkeletonData.AnimationState.SetAnimation(0, this.loopAnimation, true);
    AudioManager.Instance.StopLoop(this.wormVomitLoopInstance);
    this.isPlaying = false;
  }
}
