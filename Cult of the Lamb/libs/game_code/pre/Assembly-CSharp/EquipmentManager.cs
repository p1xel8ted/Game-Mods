// Decompiled with JetBrains decompiler
// Type: EquipmentManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public static class EquipmentManager
{
  private static WeaponData[] WeaponsData;
  private static CurseData[] CursesData;
  private const string WeaponsPath = "Data/Equipment Data/Weapons";
  private const string CursesPath = "Data/Equipment Data/Curses";

  public static EquipmentData GetEquipmentData(EquipmentType equipmentType)
  {
    EquipmentData weaponData = (EquipmentData) EquipmentManager.GetWeaponData(equipmentType);
    return (bool) (Object) weaponData ? weaponData : (EquipmentData) EquipmentManager.GetCurseData(equipmentType);
  }

  public static WeaponData GetWeaponData(EquipmentType weaponType)
  {
    if (EquipmentManager.WeaponsData == null)
      EquipmentManager.WeaponsData = Resources.LoadAll<WeaponData>("Data/Equipment Data/Weapons");
    foreach (WeaponData weaponData in EquipmentManager.WeaponsData)
    {
      if (weaponData.EquipmentType == weaponType)
        return weaponData;
    }
    return (WeaponData) null;
  }

  public static CurseData GetCurseData(EquipmentType curseType)
  {
    if (EquipmentManager.CursesData == null)
      EquipmentManager.CursesData = Resources.LoadAll<CurseData>("Data/Equipment Data/Curses");
    foreach (CurseData curseData in EquipmentManager.CursesData)
    {
      if (curseData.EquipmentType == curseType)
        return curseData;
    }
    return (CurseData) null;
  }
}
