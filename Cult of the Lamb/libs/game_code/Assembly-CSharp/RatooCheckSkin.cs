// Decompiled with JetBrains decompiler
// Type: RatooCheckSkin
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using UnityEngine;

#nullable disable
public class RatooCheckSkin : MonoBehaviour
{
  public SkeletonAnimation spine;

  public void OnEnable() => this.CheckSkin();

  public void CheckSkin()
  {
    if (!DataManager.Instance.RatooGivenHeart)
      this.spine.Skeleton.SetSkin("normal");
    else
      this.spine.Skeleton.SetSkin("heart");
    this.spine.skeleton.SetSlotsToSetupPose();
    this.spine.AnimationState.Apply(this.spine.skeleton);
  }
}
