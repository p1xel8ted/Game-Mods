// Decompiled with JetBrains decompiler
// Type: RatooCheckSkin
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
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
