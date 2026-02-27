// Decompiled with JetBrains decompiler
// Type: SpineSetMultipleSkins
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SpineSetMultipleSkins : BaseMonoBehaviour
{
  [SpineSkin("", "", true, false, false)]
  public List<string> Skin = new List<string>();
  public SkeletonAnimation Spine;
  private Spine.Skin spineSkin;

  private void OnEnable() => this.UpdateSkin();

  private void UpdateSkin()
  {
    if ((Object) this.Spine == (Object) null)
      this.Spine = this.gameObject.GetComponent<SkeletonAnimation>();
    this.Spine.skeleton.Data.FindSkin(this.Skin[0]);
    this.spineSkin = new Spine.Skin("combined");
    foreach (string skinName in this.Skin)
      this.spineSkin.AddSkin(this.Spine.skeleton.Data.FindSkin(skinName));
    this.Spine.skeleton.SetSkin(this.spineSkin);
  }
}
