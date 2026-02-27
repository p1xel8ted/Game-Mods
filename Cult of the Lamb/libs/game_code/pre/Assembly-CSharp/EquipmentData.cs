// Decompiled with JetBrains decompiler
// Type: EquipmentData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  [Space]
  public float Weight;

  public string GetLocalisedTitle()
  {
    return LocalizationManager.GetTranslation($"UpgradeSystem/{this.EquipmentType}/Name");
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
