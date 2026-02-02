// Decompiled with JetBrains decompiler
// Type: PlayerFleeceManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI.DeathScreen;
using System.Collections.Generic;

#nullable disable
public class PlayerFleeceManager
{
  public static List<PlayerFleeceManager.FleeceType> IS_DLC = new List<PlayerFleeceManager.FleeceType>()
  {
    PlayerFleeceManager.FleeceType.Fleece666,
    PlayerFleeceManager.FleeceType.Fleece667,
    PlayerFleeceManager.FleeceType.Fleece668,
    PlayerFleeceManager.FleeceType.Fleece669,
    PlayerFleeceManager.FleeceType.Fleece670,
    PlayerFleeceManager.FleeceType.Fleece671,
    PlayerFleeceManager.FleeceType.Fleece672,
    PlayerFleeceManager.FleeceType.Fleece673,
    PlayerFleeceManager.FleeceType.Fleece674,
    PlayerFleeceManager.FleeceType.Fleece675,
    PlayerFleeceManager.FleeceType.Fleece678,
    PlayerFleeceManager.FleeceType.Fleece679,
    PlayerFleeceManager.FleeceType.Fleece681,
    PlayerFleeceManager.FleeceType.RatauCloak,
    PlayerFleeceManager.FleeceType.RatauCloakBloody
  };
  public static List<PlayerFleeceManager.FleeceType> NOT_INCLUDED_IN_ACHIEVEMENT = new List<PlayerFleeceManager.FleeceType>()
  {
    PlayerFleeceManager.FleeceType.Outfit_999,
    PlayerFleeceManager.FleeceType.GodOfDeath,
    PlayerFleeceManager.FleeceType.Outfit_1001,
    PlayerFleeceManager.FleeceType.Outfit_1002,
    PlayerFleeceManager.FleeceType.Fleece666,
    PlayerFleeceManager.FleeceType.Fleece667,
    PlayerFleeceManager.FleeceType.Fleece668,
    PlayerFleeceManager.FleeceType.Fleece669,
    PlayerFleeceManager.FleeceType.Fleece670,
    PlayerFleeceManager.FleeceType.Fleece671,
    PlayerFleeceManager.FleeceType.Fleece672,
    PlayerFleeceManager.FleeceType.Fleece673,
    PlayerFleeceManager.FleeceType.Fleece674,
    PlayerFleeceManager.FleeceType.Fleece675,
    PlayerFleeceManager.FleeceType.Fleece678,
    PlayerFleeceManager.FleeceType.Fleece679,
    PlayerFleeceManager.FleeceType.Fleece681,
    PlayerFleeceManager.FleeceType.RatauCloak,
    PlayerFleeceManager.FleeceType.RatauCloakBloody
  };
  public static List<PlayerFleeceManager.FleeceType> NOT_CUSTOMISABLE_FLEECES = new List<PlayerFleeceManager.FleeceType>()
  {
    PlayerFleeceManager.FleeceType.Fleece666,
    PlayerFleeceManager.FleeceType.Fleece667,
    PlayerFleeceManager.FleeceType.Fleece668,
    PlayerFleeceManager.FleeceType.Fleece669,
    PlayerFleeceManager.FleeceType.Fleece670,
    PlayerFleeceManager.FleeceType.Fleece671,
    PlayerFleeceManager.FleeceType.Fleece672,
    PlayerFleeceManager.FleeceType.Fleece673,
    PlayerFleeceManager.FleeceType.Fleece674,
    PlayerFleeceManager.FleeceType.Fleece675,
    PlayerFleeceManager.FleeceType.Fleece679,
    PlayerFleeceManager.FleeceType.Fleece681,
    PlayerFleeceManager.FleeceType.Outfit_999,
    PlayerFleeceManager.FleeceType.Outfit_1001,
    PlayerFleeceManager.FleeceType.Outfit_1002,
    PlayerFleeceManager.FleeceType.Goat
  };
  public static List<PlayerFleeceManager.FleeceType> SinfulPackFleeces = new List<PlayerFleeceManager.FleeceType>()
  {
    PlayerFleeceManager.FleeceType.Outfit_1001
  };
  public static List<PlayerFleeceManager.FleeceType> HereticPackFleeces = new List<PlayerFleeceManager.FleeceType>()
  {
    PlayerFleeceManager.FleeceType.Outfit_999
  };
  public static List<PlayerFleeceManager.FleeceType> PilgrimPackFleeces = new List<PlayerFleeceManager.FleeceType>()
  {
    PlayerFleeceManager.FleeceType.Outfit_1002
  };
  public static List<PlayerFleeceManager.FleeceType> WoolhavenPackFleeces = new List<PlayerFleeceManager.FleeceType>()
  {
    PlayerFleeceManager.FleeceType.RatauCloak,
    PlayerFleeceManager.FleeceType.RatauCloakBloody,
    PlayerFleeceManager.FleeceType.Fleece666,
    PlayerFleeceManager.FleeceType.Fleece667,
    PlayerFleeceManager.FleeceType.Fleece668,
    PlayerFleeceManager.FleeceType.Fleece669,
    PlayerFleeceManager.FleeceType.Fleece670,
    PlayerFleeceManager.FleeceType.Fleece671,
    PlayerFleeceManager.FleeceType.Fleece672,
    PlayerFleeceManager.FleeceType.Fleece673,
    PlayerFleeceManager.FleeceType.Fleece674,
    PlayerFleeceManager.FleeceType.Fleece675,
    PlayerFleeceManager.FleeceType.Fleece678,
    PlayerFleeceManager.FleeceType.Fleece679,
    PlayerFleeceManager.FleeceType.Fleece681
  };
  public static float damageMultiplier = 1f;
  public static PlayerFleeceManager.DamageEvent OnDamageMultiplierModified;
  public static float OneHitKillHP = 1f;
  public static bool OneHitKillGivesRedHearts = false;

  public static float GetCursesDamageMultiplier()
  {
    switch ((PlayerFleeceManager.FleeceType) DataManager.Instance.PlayerFleece)
    {
      case PlayerFleeceManager.FleeceType.Gold:
        return PlayerFleeceManager.damageMultiplier;
      case PlayerFleeceManager.FleeceType.Green:
        return 1f;
      case PlayerFleeceManager.FleeceType.CurseInsteadOfWeapon:
        return 0.75f;
      default:
        return 1f;
    }
  }

  public static float GetCursesFeverMultiplier()
  {
    return DataManager.Instance.PlayerFleece == 2 ? 0.5f : 1f;
  }

  public static float GetWeaponDamageMultiplier()
  {
    switch ((PlayerFleeceManager.FleeceType) DataManager.Instance.PlayerFleece)
    {
      case PlayerFleeceManager.FleeceType.Gold:
        return PlayerFleeceManager.damageMultiplier;
      case PlayerFleeceManager.FleeceType.Green:
        return -0.5f;
      case PlayerFleeceManager.FleeceType.OneHitKills:
        return 10f;
      default:
        return 0.0f;
    }
  }

  public static float GetDamageReceivedMultiplier()
  {
    return DataManager.Instance.PlayerFleece == 1 ? 1f : 0.0f;
  }

  public static float GetHealthMultiplier()
  {
    return !GameManager.IsDungeon(PlayerFarming.Location) || DataManager.Instance.PlayerFleece != 9 ? 1f : 2f;
  }

  public static float GetLootMultiplier(UIDeathScreenOverlayController.Results _result) => 0.0f;

  public static void OnTarotCardPickedUp(PlayerFarming playerFarming)
  {
    if (DataManager.Instance.PlayerFleece != 3 || (double) playerFarming.health.BlackHearts > 0.0)
      return;
    playerFarming.health.BlackHearts = 2f;
  }

  public static int GetFreeTarotCards()
  {
    switch ((PlayerFleeceManager.FleeceType) DataManager.Instance.PlayerFleece)
    {
      case PlayerFleeceManager.FleeceType.White:
        return 4;
      case PlayerFleeceManager.FleeceType.CurseInsteadOfWeapon:
        return 4;
      default:
        return 0;
    }
  }

  public static void IncrementDamageModifier()
  {
    if (DataManager.Instance.PlayerFleece != 1)
      return;
    PlayerFleeceManager.damageMultiplier += 0.05f;
    PlayerFleeceManager.DamageEvent multiplierModified = PlayerFleeceManager.OnDamageMultiplierModified;
    if (multiplierModified == null)
      return;
    multiplierModified(PlayerFleeceManager.damageMultiplier);
  }

  public static void ResetDamageModifier()
  {
    PlayerFleeceManager.damageMultiplier = 0.0f;
    PlayerFleeceManager.DamageEvent multiplierModified = PlayerFleeceManager.OnDamageMultiplierModified;
    if (multiplierModified == null)
      return;
    multiplierModified(PlayerFleeceManager.damageMultiplier);
  }

  public static bool FleeceCausesPoisonOnHit()
  {
    return DataManager.Instance.PlayerFleece == 3 && GameManager.IsDungeon(PlayerFarming.Location);
  }

  public static bool FleecePreventsHealthPickups(bool justRedHeartCheck = false)
  {
    PlayerFleeceManager.FleeceType playerFleece = (PlayerFleeceManager.FleeceType) DataManager.Instance.PlayerFleece;
    return (!justRedHeartCheck || playerFleece != PlayerFleeceManager.FleeceType.OneHitKills || !PlayerFleeceManager.OneHitKillGivesRedHearts) && (uint) (playerFleece - 7) <= 1U;
  }

  public static bool FleeceNoRedHeartsToUse()
  {
    switch ((PlayerFleeceManager.FleeceType) DataManager.Instance.PlayerFleece)
    {
      case PlayerFleeceManager.FleeceType.Blue:
      case PlayerFleeceManager.FleeceType.OneHitKills:
        return true;
      default:
        return false;
    }
  }

  public static bool FleeceSwapsWeaponForCurse() => DataManager.Instance.PlayerFleece == 6;

  public static bool FleeceSwapsCurseForRelic() => DataManager.Instance.PlayerFleece == 10;

  public static bool FleecePreventTarotCards() => DataManager.Instance.PlayerFleece == 4;

  public static bool FleecePreventsRoll() => DataManager.Instance.PlayerFleece == 9;

  public static float AmountToHealOnRoomComplete()
  {
    return DataManager.Instance.PlayerFleece == 9 ? 2f : 0.0f;
  }

  public static float GetRelicChargeMultiplier()
  {
    return DataManager.Instance.PlayerFleece == 10 ? 3f : 1f;
  }

  public static bool FleecePreventsRespawn() => DataManager.Instance.PlayerFleece == 7;

  public static bool FleecePreventsForcedWeapons()
  {
    switch ((PlayerFleeceManager.FleeceType) DataManager.Instance.PlayerFleece)
    {
      case PlayerFleeceManager.FleeceType.CurseInsteadOfWeapon:
      case PlayerFleeceManager.FleeceType.Fleece676:
        return true;
      default:
        return false;
    }
  }

  public static bool BleatToHeal() => DataManager.Instance.PlayerFleece == 8;

  public static bool BleatToBurrow()
  {
    switch ((PlayerFleeceManager.FleeceType) DataManager.Instance.PlayerFleece)
    {
      case PlayerFleeceManager.FleeceType.RatauCloak:
        return true;
      case PlayerFleeceManager.FleeceType.RatauCloakBloody:
        return true;
      default:
        return false;
    }
  }

  public static float FleeceSpeedMultiplier()
  {
    return DataManager.Instance.PlayerFleece == 9 ? 1.65f : 1f;
  }

  public static string GetFleeceDisplayName(PlayerFleeceManager.FleeceType fleeceType)
  {
    return fleeceType == PlayerFleeceManager.FleeceType.Default ? "TarotCards/Fleece0/Name".Localized() : $"TarotCards/{fleeceType}/Name".Localized();
  }

  public enum FleeceType
  {
    Default = 0,
    Gold = 1,
    Green = 2,
    Purple = 3,
    White = 4,
    Blue = 5,
    CurseInsteadOfWeapon = 6,
    OneHitKills = 7,
    HollowHeal = 8,
    NoRolling = 9,
    RelicInsteadOfCurse = 10, // 0x0000000A
    RatauCloak = 11, // 0x0000000B
    RatauCloakBloody = 12, // 0x0000000C
    BlindFaith = 13, // 0x0000000D
    Fleece666 = 666, // 0x0000029A
    Fleece667 = 667, // 0x0000029B
    Fleece668 = 668, // 0x0000029C
    Fleece669 = 669, // 0x0000029D
    Fleece670 = 670, // 0x0000029E
    Fleece671 = 671, // 0x0000029F
    Fleece672 = 672, // 0x000002A0
    Fleece673 = 673, // 0x000002A1
    Fleece674 = 674, // 0x000002A2
    Fleece675 = 675, // 0x000002A3
    Fleece676 = 676, // 0x000002A4
    Fleece677 = 677, // 0x000002A5
    Fleece678 = 678, // 0x000002A6
    Fleece679 = 679, // 0x000002A7
    Fleece681 = 681, // 0x000002A9
    Outfit_999 = 999, // 0x000003E7
    GodOfDeath = 1000, // 0x000003E8
    Outfit_1001 = 1001, // 0x000003E9
    Outfit_1002 = 1002, // 0x000003EA
    Goat = 1003, // 0x000003EB
  }

  public delegate void DamageEvent(float damageMultiplier);
}
