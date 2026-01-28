// Decompiled with JetBrains decompiler
// Type: SpineSetMultipleSkins
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SpineSetMultipleSkins : BaseMonoBehaviour
{
  [SpineSkin("", "", true, false, false)]
  public List<string> Skin = new List<string>();
  public SkeletonAnimation Spine;
  public Spine.Skin spineSkin;

  public void OnEnable() => this.UpdateSkin();

  public void UpdateSkin()
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
