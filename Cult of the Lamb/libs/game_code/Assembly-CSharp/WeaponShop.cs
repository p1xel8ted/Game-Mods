// Decompiled with JetBrains decompiler
// Type: WeaponShop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
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
