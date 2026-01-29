// Decompiled with JetBrains decompiler
// Type: EquipmentData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using UnityEngine;

#nullable disable
public class EquipmentData : ScriptableObject
{
  public EquipmentType EquipmentType;
  public EquipmentType PrimaryEquipmentType;
  [Space]
  public bool CanBreakDodge;
  [Space]
  public Sprite UISprite;
  public Sprite WorldSprite;
  [Space]
  public string PickupAnimationKey;
  public string PerformActionSound;

  public string GetLocalisedTitle()
  {
    return EquipmentManager.IsLegendaryWeapon(this.EquipmentType) ? this.GetLegnedaryWeaponTitle() : LocalizationManager.GetTranslation($"UpgradeSystem/{this.EquipmentType}/Name");
  }

  public string GetLegnedaryWeaponTitle()
  {
    if (this.EquipmentType == EquipmentType.Hammer_Legendary && DataManager.Instance.LegendaryHammerCustomName != null && DataManager.Instance.LegendaryHammerCustomName.Length > 0)
      return DataManager.Instance.LegendaryHammerCustomName;
    if (this.EquipmentType == EquipmentType.Sword_Legendary && DataManager.Instance.LegendarySwordCustomName != null && DataManager.Instance.LegendarySwordCustomName.Length > 0)
      return DataManager.Instance.LegendarySwordCustomName;
    if (this.EquipmentType == EquipmentType.Dagger_Legendary && DataManager.Instance.LegendaryDaggerCustomName != null && DataManager.Instance.LegendaryDaggerCustomName.Length > 0)
      return DataManager.Instance.LegendaryDaggerCustomName;
    if (this.EquipmentType == EquipmentType.Gauntlet_Legendary && DataManager.Instance.LegendaryGauntletCustomName != null && DataManager.Instance.LegendaryGauntletCustomName.Length > 0)
      return DataManager.Instance.LegendaryGauntletCustomName;
    if (this.EquipmentType == EquipmentType.Blunderbuss_Legendary && DataManager.Instance.LegendaryBlunderbussCustomName != null && DataManager.Instance.LegendaryBlunderbussCustomName.Length > 0)
      return DataManager.Instance.LegendaryBlunderbussCustomName;
    if (this.EquipmentType == EquipmentType.Axe_Legendary && DataManager.Instance.LegendaryAxeCustomName != null && DataManager.Instance.LegendaryAxeCustomName.Length > 0)
      return DataManager.Instance.LegendaryAxeCustomName;
    return this.EquipmentType == EquipmentType.Chain_Legendary && DataManager.Instance.LegendaryChainCustomName != null && DataManager.Instance.LegendaryChainCustomName.Length > 0 ? DataManager.Instance.LegendaryChainCustomName : LocalizationManager.GetTranslation($"UpgradeSystem/{this.EquipmentType}/Name");
  }

  public string GetLocalisedDescription()
  {
    return this.PrimaryEquipmentType == EquipmentType.ProjectileAOE ? $"<color=#FFD201>{ScriptLocalization.UI_Settings_Controls.HoldToAim}</color><br>{LocalizationManager.GetTranslation($"UpgradeSystem/{this.EquipmentType}/Description")}" : LocalizationManager.GetTranslation($"UpgradeSystem/{this.EquipmentType}/Description");
  }

  public string GetLocalisedLore()
  {
    return LocalizationManager.GetTranslation($"UpgradeSystem/{this.EquipmentType}/Lore");
  }
}
