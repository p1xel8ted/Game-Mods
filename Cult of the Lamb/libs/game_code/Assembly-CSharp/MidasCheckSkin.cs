// Decompiled with JetBrains decompiler
// Type: MidasCheckSkin
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using UnityEngine;

#nullable disable
public class MidasCheckSkin : MonoBehaviour
{
  public SkeletonAnimation spine;

  public void OnEnable()
  {
    this.spine = this.GetComponent<SkeletonAnimation>();
    this.CheckSkin();
  }

  public void CheckSkin()
  {
    if (DataManager.Instance.MidasBeaten)
      this.spine.Skeleton.SetSkin("Beaten");
    else
      this.spine.Skeleton.SetSkin("Normal");
    this.spine.skeleton.SetSlotsToSetupPose();
    this.spine.AnimationState.Apply(this.spine.skeleton);
  }
}
