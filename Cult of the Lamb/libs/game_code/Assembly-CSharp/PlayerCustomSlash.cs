// Decompiled with JetBrains decompiler
// Type: PlayerCustomSlash
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using UnityEngine;

#nullable disable
public class PlayerCustomSlash : MonoBehaviour
{
  public SkeletonRendererCustomMaterials customMaterial;
  public EquipmentType cacheWeapon;
  public Material normalMaterial;
  public Material poisonMaterial;
  public Material darkEdgesMaterial;
  public PlayerFarming playerFarming;

  public void Start() => this.playerFarming = this.GetComponent<PlayerFarming>();

  public void Update()
  {
    if (!((Object) this.playerFarming != (Object) null) || !((Object) this.playerFarming.playerWeapon != (Object) null) || this.playerFarming.CurrentWeaponInfo == null || !((Object) this.playerFarming.CurrentWeaponInfo.WeaponData != (Object) null) || this.cacheWeapon == this.playerFarming.CurrentWeaponInfo.WeaponData.EquipmentType)
      return;
    this.customMaterial.customSlotMaterials[0].material = !EquipmentManager.IsPoisonWeapon(this.playerFarming.CurrentWeaponInfo.WeaponData.EquipmentType) ? ((double) Shader.GetGlobalFloat("_Snow_Intensity") > 0.5 || PlayerFarming.Location == FollowerLocation.Dungeon1_5 ? this.darkEdgesMaterial : this.normalMaterial) : this.poisonMaterial;
    this.customMaterial.UpdateMaterials();
    this.cacheWeapon = this.playerFarming.CurrentWeaponInfo.WeaponData.EquipmentType;
  }
}
