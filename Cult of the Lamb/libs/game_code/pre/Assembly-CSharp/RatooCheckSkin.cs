// Decompiled with JetBrains decompiler
// Type: RatooCheckSkin
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine.Unity;
using UnityEngine;

#nullable disable
public class RatooCheckSkin : MonoBehaviour
{
  public SkeletonAnimation spine;

  private void OnEnable() => this.CheckSkin();

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
