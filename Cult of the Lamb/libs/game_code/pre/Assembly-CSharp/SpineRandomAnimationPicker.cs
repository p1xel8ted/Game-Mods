// Decompiled with JetBrains decompiler
// Type: SpineRandomAnimationPicker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SpineRandomAnimationPicker : BaseMonoBehaviour
{
  public List<SpineAnimations> spineAnims = new List<SpineAnimations>();
  public bool randomTimeScale = true;
  private SkeletonAnimation Spine;

  private void Start() => this.PickRandomAnimation();

  private void PickRandomAnimation()
  {
    if (this.spineAnims.Count == 0)
      return;
    if ((Object) this.Spine == (Object) null)
      this.Spine = this.gameObject.GetComponent<SkeletonAnimation>();
    if ((Object) this.Spine == (Object) null)
      return;
    SpineAnimations spineAnimations = this.spineAnims.RandomElement<SpineAnimations>();
    if (!string.IsNullOrEmpty(spineAnimations.TriggeredAnimation))
      this.Spine.AnimationState.SetAnimation(0, spineAnimations.TriggeredAnimation, true);
    if (!this.randomTimeScale)
      return;
    this.Spine.timeScale = Random.Range(0.8f, 1.2f);
  }

  private void OnBecameInvisible()
  {
    if ((Object) this.Spine == (Object) null)
      this.Spine = this.gameObject.GetComponent<SkeletonAnimation>();
    if ((Object) this.Spine == (Object) null)
      return;
    this.Spine.enabled = false;
  }

  private void OnBecameVisible()
  {
    if ((Object) this.Spine == (Object) null)
      this.Spine = this.gameObject.GetComponent<SkeletonAnimation>();
    if ((Object) this.Spine == (Object) null)
      return;
    this.Spine.enabled = true;
  }
}
