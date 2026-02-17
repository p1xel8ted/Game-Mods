// Decompiled with JetBrains decompiler
// Type: SimpleSpineFollowOtherSpine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine;
using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class SimpleSpineFollowOtherSpine : BaseMonoBehaviour
{
  public SkeletonAnimation SpineToFollow;
  public SkeletonAnimation Spine;
  public MeshRenderer meshRenderer;
  public MeshRenderer MeshRendererToFollow;
  public Material material;
  public bool UseCoroutine;
  public bool setMaterial = true;

  public void OnEnable()
  {
    this.SpineToFollow.AnimationState.Start += new Spine.AnimationState.TrackEntryDelegate(this.MirrorAnimation);
    this.meshRenderer = this.Spine.GetComponent<MeshRenderer>();
    this.MeshRendererToFollow = this.SpineToFollow.GetComponent<MeshRenderer>();
  }

  public void OnDisable()
  {
    this.SpineToFollow.AnimationState.Start -= new Spine.AnimationState.TrackEntryDelegate(this.MirrorAnimation);
  }

  public void MirrorAnimation(TrackEntry trackEntry)
  {
    this.Spine.AnimationState.SetAnimation(0, this.SpineToFollow.AnimationName, this.SpineToFollow.AnimationState.GetCurrent(0).Loop).MixDuration = 0.0f;
    this.Spine.AnimationState.TimeScale = 0.0f;
  }

  public IEnumerator ApplyMaterial()
  {
    yield return (object) new WaitForEndOfFrame();
    this.meshRenderer.material = this.material;
  }

  public void LateUpdate()
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
