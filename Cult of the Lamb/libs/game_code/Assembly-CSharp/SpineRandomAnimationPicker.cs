// Decompiled with JetBrains decompiler
// Type: SpineRandomAnimationPicker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SpineRandomAnimationPicker : BaseMonoBehaviour
{
  public List<SpineAnimations> spineAnims = new List<SpineAnimations>();
  public bool randomTimeScale = true;
  public SkeletonAnimation Spine;

  public bool IsUsingLOD => (Object) this.Spine != (Object) null && this.Spine.IsLOD;

  public void Start() => this.PickRandomAnimation();

  public void PickRandomAnimation()
  {
    if (this.spineAnims.Count == 0)
      return;
    if ((Object) this.Spine == (Object) null)
      this.Spine = this.gameObject.GetComponent<SkeletonAnimation>();
    if ((Object) this.Spine == (Object) null)
      return;
    SpineAnimations spineAnimations = this.spineAnims.RandomElement<SpineAnimations>();
    if (!string.IsNullOrEmpty(spineAnimations.TriggeredAnimation) && this.Spine.AnimationState != null)
      this.Spine.AnimationState.SetAnimation(0, spineAnimations.TriggeredAnimation, true);
    if (!this.randomTimeScale)
      return;
    this.Spine.timeScale = Random.Range(0.8f, 1.2f);
  }

  public void OnBecameInvisible()
  {
    if ((Object) this.Spine == (Object) null)
      this.Spine = this.gameObject.GetComponent<SkeletonAnimation>();
    if ((Object) this.Spine == (Object) null || this.IsUsingLOD)
      return;
    this.Spine.enabled = false;
  }

  public void OnBecameVisible()
  {
    if ((Object) this.Spine == (Object) null)
      this.Spine = this.gameObject.GetComponent<SkeletonAnimation>();
    if ((Object) this.Spine == (Object) null || this.IsUsingLOD)
      return;
    this.Spine.enabled = true;
  }
}
