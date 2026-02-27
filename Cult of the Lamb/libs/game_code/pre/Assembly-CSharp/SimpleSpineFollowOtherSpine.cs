// Decompiled with JetBrains decompiler
// Type: SimpleSpineFollowOtherSpine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine;
using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class SimpleSpineFollowOtherSpine : BaseMonoBehaviour
{
  public SkeletonAnimation SpineToFollow;
  public SkeletonAnimation Spine;
  private MeshRenderer meshRenderer;
  private MeshRenderer MeshRendererToFollow;
  public Material material;
  public bool UseCoroutine;
  public bool setMaterial = true;

  private void OnEnable()
  {
    this.SpineToFollow.AnimationState.Start += new Spine.AnimationState.TrackEntryDelegate(this.MirrorAnimation);
    this.meshRenderer = this.Spine.GetComponent<MeshRenderer>();
    this.MeshRendererToFollow = this.SpineToFollow.GetComponent<MeshRenderer>();
  }

  private void OnDisable()
  {
    this.SpineToFollow.AnimationState.Start -= new Spine.AnimationState.TrackEntryDelegate(this.MirrorAnimation);
  }

  private void MirrorAnimation(TrackEntry trackEntry)
  {
    this.Spine.AnimationState.SetAnimation(0, this.SpineToFollow.AnimationName, this.SpineToFollow.AnimationState.GetCurrent(0).Loop).MixDuration = 0.0f;
    this.Spine.AnimationState.TimeScale = 0.0f;
  }

  private IEnumerator ApplyMaterial()
  {
    yield return (object) new WaitForEndOfFrame();
    this.meshRenderer.material = this.material;
  }

  private void LateUpdate()
  {
    if (this.setMaterial)
      this.meshRenderer.material = this.material;
    this.Spine.state.GetCurrent(0).TrackTime = this.SpineToFollow.state.GetCurrent(0).TrackTime;
    this.Spine.AnimationState.Apply(this.Spine.skeleton);
    if (this.meshRenderer.enabled != this.MeshRendererToFollow.enabled)
      this.meshRenderer.enabled = this.MeshRendererToFollow.enabled;
    if ((double) this.Spine.skeleton.ScaleX == (double) this.SpineToFollow.skeleton.ScaleX)
      return;
    this.Spine.skeleton.ScaleX = this.SpineToFollow.skeleton.ScaleX;
  }
}
