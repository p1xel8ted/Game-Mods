// Decompiled with JetBrains decompiler
// Type: MidasCheckSkin
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
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
