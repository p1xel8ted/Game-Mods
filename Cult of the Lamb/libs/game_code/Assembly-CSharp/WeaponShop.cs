// Decompiled with JetBrains decompiler
// Type: WeaponShop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using UnityEngine;

#nullable disable
public class WeaponShop : MonoBehaviour
{
  public SkeletonAnimation Spine;
  [SerializeField]
  public string VOtoPlay = "event:/dialogue/shop_weapons/buy_weaponshop";

  public void ChooseObject()
  {
    AudioManager.Instance.PlayOneShot(this.VOtoPlay);
    this.Spine.AnimationState.SetAnimation(0, "talk-yes", false);
    this.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
  }
}
