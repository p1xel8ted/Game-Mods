// Decompiled with JetBrains decompiler
// Type: Objectives_LegendaryWeaponRun
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MessagePack;
using System;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class Objectives_LegendaryWeaponRun : ObjectivesData
{
  [Key(16 /*0x10*/)]
  public EquipmentType LegendaryWeapon;
  [IgnoreMember]
  public bool complete;

  public override string Text
  {
    get
    {
      string str = "";
      if (!DataManager.Instance.WeaponPool.Contains(this.LegendaryWeapon))
      {
        switch (this.LegendaryWeapon)
        {
          case EquipmentType.Sword_Legendary:
            str = LocalizationManager.GetTranslation("Objectives/GroupTitles/LegendarySword");
            break;
          case EquipmentType.Axe_Legendary:
            str = LocalizationManager.GetTranslation("Objectives/GroupTitles/LegendaryAxe");
            break;
          case EquipmentType.Hammer_Legendary:
            str = LocalizationManager.GetTranslation("Objectives/GroupTitles/LegendaryHammer");
            break;
          case EquipmentType.Dagger_Legendary:
            str = LocalizationManager.GetTranslation("Objectives/GroupTitles/LegendaryDagger");
            break;
          case EquipmentType.Gauntlet_Legendary:
            str = LocalizationManager.GetTranslation("Objectives/GroupTitles/LegendaryGauntlets");
            break;
          case EquipmentType.Blunderbuss_Legendary:
            str = LocalizationManager.GetTranslation("Objectives/GroupTitles/LegendaryBlunderbuss");
            break;
          case EquipmentType.Chain_Legendary:
            str = LocalizationManager.GetTranslation("Objectives/GroupTitles/LegendaryChain");
            break;
        }
      }
      else
        str = EquipmentManager.GetEquipmentData(this.LegendaryWeapon).GetLocalisedTitle();
      return string.Format(LocalizationManager.GetTranslation("Objectives/LegendaryDungeonRun"), (object) str);
    }
  }

  public Objectives_LegendaryWeaponRun()
  {
  }

  public Objectives_LegendaryWeaponRun(
    string groupId,
    EquipmentType legendaryWeapon,
    float questExpireDuration = -1f)
    : base(groupId, questExpireDuration)
  {
    this.Type = Objectives.TYPES.LEGENDARY_WEAPON_RUN;
    this.LegendaryWeapon = legendaryWeapon;
  }

  public override void Init(bool initialAssigning)
  {
    base.Init(initialAssigning);
    this.complete = false;
  }

  public override ObjectivesDataFinalized GetFinalizedData()
  {
    Objectives_LegendaryWeaponRun.FinalizedData_LegendaryWeaponRun finalizedData = new Objectives_LegendaryWeaponRun.FinalizedData_LegendaryWeaponRun();
    finalizedData.GroupId = this.GroupId;
    finalizedData.Index = this.Index;
    finalizedData.LegendaryWeapon = this.LegendaryWeapon;
    finalizedData.UniqueGroupID = this.UniqueGroupID;
    return (ObjectivesDataFinalized) finalizedData;
  }

  public void CheckComplete(EquipmentType weapon)
  {
    if (weapon == this.LegendaryWeapon)
      this.IsComplete = true;
    this.CheckComplete();
  }

  public override bool CheckComplete() => this.IsComplete;

  [MessagePackObject(false)]
  [Serializable]
  public class FinalizedData_LegendaryWeaponRun : ObjectivesDataFinalized
  {
    [Key(3)]
    public EquipmentType LegendaryWeapon;

    public override string GetText()
    {
      EquipmentData equipmentData = EquipmentManager.GetEquipmentData(this.LegendaryWeapon);
      return string.Format(LocalizationManager.GetTranslation("Objectives/LegendaryDungeonRun"), (object) equipmentData.GetLocalisedTitle());
    }
  }
}
