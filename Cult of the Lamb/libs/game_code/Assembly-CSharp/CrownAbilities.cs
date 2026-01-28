// Decompiled with JetBrains decompiler
// Type: CrownAbilities
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MessagePack;
using System;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class CrownAbilities
{
  [Key(0)]
  public CrownAbilities.TYPE Type;
  [Key(1)]
  public bool Unlocked;
  [Key(2)]
  public float UnlockProgress;
  [Key(3)]
  public bool Used;

  public static int CrownAbilitiesUnlocked()
  {
    int num = 0;
    foreach (CrownAbilities crownAbilities in DataManager.Instance.CrownAbilitiesUnlocked)
    {
      if (crownAbilities.Unlocked)
        ++num;
    }
    return num;
  }

  public static int GetCrownAbilitiesCost(CrownAbilities.TYPE Type)
  {
    switch (Type)
    {
      case CrownAbilities.TYPE.Combat_Arrows:
        return 1;
      case CrownAbilities.TYPE.Combat_Arrows_I:
        return 1;
      case CrownAbilities.TYPE.Followers_CheerUp:
        return 1;
      case CrownAbilities.TYPE.Followers_Charisma:
        return 1;
      case CrownAbilities.TYPE.Abilities_GrappleHook:
        return 1;
      case CrownAbilities.TYPE.Abilities_FishingRod:
        return 1;
      default:
        return 1;
    }
  }

  public static void OnUnlockAbility(CrownAbilities.TYPE Types)
  {
    switch (Types)
    {
      case CrownAbilities.TYPE.Combat_Arrows:
        UnityEngine.Object.FindObjectOfType<HUD_Ammo>().Play();
        UIAbilityUnlock.Play(UIAbilityUnlock.Ability.Arrows);
        break;
      case CrownAbilities.TYPE.Combat_HeavyAttack:
        UIAbilityUnlock.Play(UIAbilityUnlock.Ability.HeavyAttack);
        break;
      case CrownAbilities.TYPE.Abilities_GrappleHook:
        UIAbilityUnlock.Play(UIAbilityUnlock.Ability.GrappleHook);
        break;
      case CrownAbilities.TYPE.Abilities_FishingRod:
        UIAbilityUnlock.Play(UIAbilityUnlock.Ability.FishingRod);
        break;
      case CrownAbilities.TYPE.Abilities_Heart_I:
      case CrownAbilities.TYPE.Abilities_Heart_II:
      case CrownAbilities.TYPE.Abilities_Heart_III:
        HealthPlayer objectOfType = UnityEngine.Object.FindObjectOfType<HealthPlayer>();
        objectOfType.totalHP += 2f;
        objectOfType.HP = objectOfType.totalHP;
        break;
    }
  }

  public static void UnlockAbility(CrownAbilities.TYPE Types)
  {
    if (CrownAbilities.CrownAbilityUnlocked(Types))
      return;
    DataManager.Instance.CrownAbilitiesUnlocked.Add(new CrownAbilities()
    {
      Type = Types,
      Unlocked = true
    });
    CrownAbilities.OnUnlockAbility(Types);
  }

  public static bool CrownAbilityUnlocked(CrownAbilities.TYPE Types)
  {
    if (DataManager.Instance.CrownAbilitiesUnlocked.Count == 0)
      return false;
    foreach (CrownAbilities crownAbilities in DataManager.Instance.CrownAbilitiesUnlocked)
    {
      if (crownAbilities.Type == Types)
        return true;
    }
    return false;
  }

  public static string LocalisedName(CrownAbilities.TYPE Type)
  {
    return LocalizationManager.GetTranslation($"Abilities/{Type}/Title");
  }

  public static string LocalisedDescription(CrownAbilities.TYPE Type)
  {
    return LocalizationManager.GetTranslation($"Abilities/{Type}/Description");
  }

  public static string LocalisedExplanation(CrownAbilities.TYPE Type)
  {
    return LocalizationManager.GetTranslation($"Abilities/{Type}/Explanation");
  }

  public enum TYPE
  {
    Combat_Arrows,
    Combat_HeavyAttack,
    Combat_Arrows_I,
    Combat_Hearts,
    Followers_CheerUp,
    Followers_Charisma,
    Followers_Bliss,
    Abilities_GrappleHook,
    Abilities_FishingRod,
    Abilities_SpecialKey,
    Abilities_Hunting,
    Abilities_Heart_I,
    Abilities_Heart_II,
    Abilities_Heart_III,
  }
}
