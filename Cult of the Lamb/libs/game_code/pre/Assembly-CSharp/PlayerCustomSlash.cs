// Decompiled with JetBrains decompiler
// Type: PlayerCustomSlash
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine.Unity;
using UnityEngine;

#nullable disable
public class PlayerCustomSlash : MonoBehaviour
{
  public SkeletonRendererCustomMaterials customMaterial;
  public EquipmentType cacheWeapon;
  public Material normalMaterial;
  public Material poisonMaterial;

  private void Update()
  {
    if (!((Object) PlayerFarming.Instance != (Object) null) || !((Object) PlayerFarming.Instance.playerWeapon != (Object) null) || PlayerFarming.Instance.playerWeapon.CurrentWeapon == null || !((Object) PlayerFarming.Instance.playerWeapon.CurrentWeapon.WeaponData != (Object) null) || this.cacheWeapon == PlayerFarming.Instance.playerWeapon.CurrentWeapon.WeaponData.EquipmentType)
      return;
    this.customMaterial.customSlotMaterials[0].material = PlayerFarming.Instance.playerWeapon.CurrentWeapon.WeaponData.EquipmentType == EquipmentType.Axe_Poison || PlayerFarming.Instance.playerWeapon.CurrentWeapon.WeaponData.EquipmentType == EquipmentType.Dagger_Poison || PlayerFarming.Instance.playerWeapon.CurrentWeapon.WeaponData.EquipmentType == EquipmentType.Gauntlet_Poison || PlayerFarming.Instance.playerWeapon.CurrentWeapon.WeaponData.EquipmentType == EquipmentType.Hammer_Poison || PlayerFarming.Instance.playerWeapon.CurrentWeapon.WeaponData.EquipmentType == EquipmentType.Hammer_Poison || PlayerFarming.Instance.playerWeapon.CurrentWeapon.WeaponData.EquipmentType == EquipmentType.Sword_Poison ? this.poisonMaterial : this.normalMaterial;
    this.customMaterial.UpdateMaterials();
    this.cacheWeapon = PlayerFarming.Instance.playerWeapon.CurrentWeapon.WeaponData.EquipmentType;
  }
}
