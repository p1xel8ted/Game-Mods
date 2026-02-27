// Decompiled with JetBrains decompiler
// Type: spineChangeAnimationSimple_UI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class spineChangeAnimationSimple_UI : BaseMonoBehaviour
{
  public SkeletonGraphic SkeletonData;
  [SpineAnimation("", "SkeletonData", true, false)]
  public string skeletonAnimation;
  public bool loop;
  public float waitTime;
  public UnityEvent unityEventOnAnimationEnd;
  public AudioClip sfx;
  private bool playedAnimation;
  private TrackEntry Track;

  private void Start()
  {
  }

  private void Update()
  {
  }

  public void changeAnimation()
  {
    this.Track = this.SkeletonData.AnimationState.SetAnimation(0, this.skeletonAnimation, this.loop);
    this.SkeletonData.AnimationState.End += (Spine.AnimationState.TrackEntryDelegate) (_param1 => this.unityEventOnAnimationEnd.Invoke());
  }
}
