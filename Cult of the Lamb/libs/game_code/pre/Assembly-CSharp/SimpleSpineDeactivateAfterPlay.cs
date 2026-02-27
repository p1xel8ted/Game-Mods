// Decompiled with JetBrains decompiler
// Type: SimpleSpineDeactivateAfterPlay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine;
using Spine.Unity;
using UnityEngine;

#nullable disable
public class SimpleSpineDeactivateAfterPlay : BaseMonoBehaviour
{
  public SkeletonAnimation Spine;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string Animation;

  public bool Init { get; set; }

  private void Update()
  {
    if (this.Init || this.Spine.AnimationState == null)
      return;
    this.Spine.AnimationState.SetAnimation(0, this.Animation, true);
    this.Spine.AnimationState.Complete += new Spine.AnimationState.TrackEntryDelegate(this.AnimationState_Complete);
    this.Init = true;
  }

  private void OnDisable()
  {
    this.Init = false;
    if (!((Object) this.Spine != (Object) null) || this.Spine.AnimationState == null)
      return;
    this.Spine.AnimationState.Complete -= new Spine.AnimationState.TrackEntryDelegate(this.AnimationState_Complete);
  }

  private void AnimationState_Complete(TrackEntry trackEntry) => this.gameObject.SetActive(false);
}
