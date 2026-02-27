// Decompiled with JetBrains decompiler
// Type: WeaponShop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine.Unity;
using UnityEngine;

#nullable disable
public class WeaponShop : MonoBehaviour
{
  public SkeletonAnimation Spine;

  public void ChooseObject()
  {
    AudioManager.Instance.PlayOneShot("event:/dialogue/shop_weapons/buy_weaponshop");
    this.Spine.AnimationState.SetAnimation(0, "talk-yes", false);
    this.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
  }
}
