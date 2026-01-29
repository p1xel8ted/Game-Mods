// Decompiled with JetBrains decompiler
// Type: TarotCards
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MessagePack;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class TarotCards
{
  [Key(0)]
  public TarotCards.Card Type;
  public static List<TarotCards.Card> COOP_TAROTS_EXCLUDE = new List<TarotCards.Card>()
  {
    TarotCards.Card.CorruptedTradeOff
  };
  [Key(1)]
  public bool Unlocked;
  [Key(2)]
  public float UnlockProgress;
  [Key(3)]
  public bool Used;
  public static TarotCards.Card[] DefaultCards = new TarotCards.Card[15]
  {
    TarotCards.Card.Hearts1,
    TarotCards.Card.Lovers1,
    TarotCards.Card.EyeOfWeakness,
    TarotCards.Card.Telescope,
    TarotCards.Card.DiseasedHeart,
    TarotCards.Card.Spider,
    TarotCards.Card.AttackRate,
    TarotCards.Card.IncreasedDamage,
    TarotCards.Card.IncreaseBlackSoulsDrop,
    TarotCards.Card.NegateDamageChance,
    TarotCards.Card.AmmoEfficient,
    TarotCards.Card.HealTwiceAmount,
    TarotCards.Card.DeathsDoor,
    TarotCards.Card.GiftFromBelow,
    TarotCards.Card.RabbitFoot
  };
  public static TarotCards.Card[] CoopCards = new TarotCards.Card[5]
  {
    TarotCards.Card.CoopBetterTogether,
    TarotCards.Card.CoopBetterApart,
    TarotCards.Card.CoopBonded,
    TarotCards.Card.CoopGoodTiming,
    TarotCards.Card.CoopExplosive
  };
  public static TarotCards.Card[] DLCWithBlueBack = new TarotCards.Card[12]
  {
    TarotCards.Card.EasyMoney,
    TarotCards.Card.EmptyFervourCritical,
    TarotCards.Card.FlameHeart,
    TarotCards.Card.FrostHeart,
    TarotCards.Card.HeartTarotDrawn,
    TarotCards.Card.HighRoller,
    TarotCards.Card.HitKillEnemy,
    TarotCards.Card.Joker,
    TarotCards.Card.KillEnemiesOnResurrect,
    TarotCards.Card.LastChance,
    TarotCards.Card.RoomEnterCritter,
    TarotCards.Card.SummonGhost
  };
  public static TarotCards.Card[] MajorDLCCards = new TarotCards.Card[19]
  {
    TarotCards.Card.FrostedEnemies,
    TarotCards.Card.LastChance,
    TarotCards.Card.Joker,
    TarotCards.Card.FrostHeart,
    TarotCards.Card.FlameHeart,
    TarotCards.Card.EasyMoney,
    TarotCards.Card.HighRoller,
    TarotCards.Card.MutatedNegateHit,
    TarotCards.Card.MutatedDropRotburn,
    TarotCards.Card.MutatedFreezeOnHit,
    TarotCards.Card.MutatedResurrectFullHealth,
    TarotCards.Card.MutatedInvincibility,
    TarotCards.Card.MutatedSpawnRotDemons,
    TarotCards.Card.EmptyFervourCritical,
    TarotCards.Card.KillEnemiesOnResurrect,
    TarotCards.Card.RoomEnterCritter,
    TarotCards.Card.HitKillEnemy,
    TarotCards.Card.SummonGhost,
    TarotCards.Card.HeartTarotDrawn
  };
  public static TarotCards.Card[] CorruptedCards = new TarotCards.Card[10]
  {
    TarotCards.Card.CorruptedHeavy,
    TarotCards.Card.CorruptedHealForRelic,
    TarotCards.Card.CorruptedBlackHeartForRelic,
    TarotCards.Card.NoCorruption,
    TarotCards.Card.CorruptedFullCorruption,
    TarotCards.Card.CorruptedGoopyTrail,
    TarotCards.Card.CorruptedPoisonCoins,
    TarotCards.Card.CorruptedRelicCharge,
    TarotCards.Card.CorruptedTradeOff,
    TarotCards.Card.CorruptedBombsAndHealth
  };
  public static TarotCards.Card[] MysticCards = new TarotCards.Card[8]
  {
    TarotCards.Card.AdventureMapFreedom,
    TarotCards.Card.Recycle,
    TarotCards.Card.StrikeBack,
    TarotCards.Card.SurpriseAttack,
    TarotCards.Card.BossHeal,
    TarotCards.Card.Sin,
    TarotCards.Card.ShuffleNode,
    TarotCards.Card.ExtraMove
  };
  public static TarotCards.Card[] RotCards = new TarotCards.Card[6]
  {
    TarotCards.Card.MutatedDropRotburn,
    TarotCards.Card.MutatedFreezeOnHit,
    TarotCards.Card.MutatedInvincibility,
    TarotCards.Card.MutatedNegateHit,
    TarotCards.Card.MutatedResurrectFullHealth,
    TarotCards.Card.MutatedSpawnRotDemons
  };
  public static List<TarotCards.Card> WitnerCards = new List<TarotCards.Card>()
  {
    TarotCards.Card.FrostedEnemies,
    TarotCards.Card.CursedIce,
    TarotCards.Card.LastChance
  };
  [Key(4)]
  public TarotCards.CardCategory cardCategory;

  public static bool IsRotCard(TarotCards.Card cardType)
  {
    return TarotCards.RotCards.Contains<TarotCards.Card>(cardType);
  }

  public static int TarotCardsUnlockedCount() => DataManager.Instance.PlayerFoundTrinkets.Count;

  public static TarotCards Create(TarotCards.Card Type, bool Unlocked)
  {
    TarotCards tarotCards = new TarotCards();
    tarotCards.Type = Type;
    tarotCards.Unlocked = Unlocked;
    if (Unlocked)
      tarotCards.UnlockProgress = 1f;
    return tarotCards;
  }

  public TarotCards.CardCategory GetCardCategory(TarotCards.Card Type)
  {
    switch (Type)
    {
      case TarotCards.Card.Sword:
      case TarotCards.Card.Dagger:
      case TarotCards.Card.Axe:
      case TarotCards.Card.Blunderbuss:
        return TarotCards.CardCategory.Weapon;
      case TarotCards.Card.Fireball:
      case TarotCards.Card.Tripleshot:
        return TarotCards.CardCategory.Curse;
      default:
        return TarotCards.CardCategory.Trinket;
    }
  }

  public static bool CanBypassAdventureMap()
  {
    if ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null || (UnityEngine.Object) MMBiomeGeneration.BiomeGenerator.Instance == (UnityEngine.Object) null)
      return true;
    int num = PlayerFarming.Location != FollowerLocation.Dungeon1_5 ? 0 : (DataManager.Instance.CurrentDLCNodeType != DungeonWorldMapIcon.NodeType.Dungeon5_GhostNPC || DataManager.Instance.OnboardedRotstoneDungeon ? (!DataManager.Instance.RancherSpokeAboutBrokenShop ? 1 : 0) : 1);
    bool flag1 = PlayerFarming.Location == FollowerLocation.Dungeon1_5 && DataManager.Instance.CurrentDLCNodeType == DungeonWorldMapIcon.NodeType.Dungeon5_MiniBoss && !DataManager.Instance.OnboardedLightningShardDungeon;
    bool flag2 = PlayerFarming.Location == FollowerLocation.Dungeon1_6 && DataManager.Instance.CurrentDLCNodeType == DungeonWorldMapIcon.NodeType.Dungeon6_MiniBoss && !DataManager.Instance.OnboardedYewCursedDungeon;
    return num == 0 && !flag1 && !flag2;
  }

  public static string LocalisedName(TarotCards.Card type)
  {
    int upgradeIndex = 0;
    foreach (TarotCards.TarotCard runTrinket in PlayerFarming.Instance.RunTrinkets)
    {
      if (runTrinket.CardType == type)
      {
        upgradeIndex = runTrinket.UpgradeIndex;
        break;
      }
    }
    return TarotCards.LocalisedName(type, upgradeIndex);
  }

  public static string LocalisedName(TarotCards.Card Card, int upgradeIndex)
  {
    string str1 = "";
    for (int index = 0; index < upgradeIndex; ++index)
      str1 += "+";
    string str2 = "";
    switch (upgradeIndex)
    {
      case 1:
        str2 = "<color=green>";
        break;
      case 2:
        str2 = "<color=purple>";
        break;
    }
    return $"{str2}{LocalizationManager.GetTranslation($"TarotCards/{Card}/Name")}{str1}</color>";
  }

  public static string LocalisedDescription(TarotCards.Card Type, PlayerFarming playerFarming)
  {
    int upgradeIndex = 0;
    foreach (TarotCards.TarotCard runTrinket in PlayerFarming.Instance.RunTrinkets)
    {
      if (runTrinket.CardType == Type)
      {
        upgradeIndex = runTrinket.UpgradeIndex;
        break;
      }
    }
    return TarotCards.LocalisedDescription(Type, upgradeIndex, playerFarming);
  }

  public static string LocalisedDescription(
    TarotCards.Card Type,
    int upgradeIndex,
    PlayerFarming playerFarming)
  {
    string Term = $"TarotCards/{Type}/Description";
    if (upgradeIndex > 0)
      Term += upgradeIndex.ToString();
    switch (Type)
    {
      case TarotCards.Card.CorruptedBombsAndHealth:
      case TarotCards.Card.CorruptedHeavy:
      case TarotCards.Card.CorruptedTradeOff:
      case TarotCards.Card.CorruptedBlackHeartForRelic:
      case TarotCards.Card.CorruptedHealForRelic:
      case TarotCards.Card.CorruptedFullCorruption:
      case TarotCards.Card.CorruptedPoisonCoins:
      case TarotCards.Card.CorruptedRelicCharge:
      case TarotCards.Card.CorruptedGoopyTrail:
        string str1 = LocalizationManager.GetTermTranslation($"TarotCards/{Type}/Positive");
        string str2 = LocalizationManager.GetTermTranslation($"TarotCards/{Type}/Negative");
        string format = LocalizationManager.GetTranslation(Term);
        if (TrinketManager.IsCorruptedNegativeEffectNegated(Type, playerFarming))
        {
          str2 = $"<s>{str2}</s>";
          if (TrinketManager.HasTrinket(TarotCards.Card.NoCorruption, playerFarming))
            format = $"{format}<br><color=#FFD201>{TarotCards.LocalisedName(TarotCards.Card.NoCorruption)}";
        }
        else if (TrinketManager.IsCorruptedPositiveEffectNegated(Type, playerFarming))
        {
          bool flag = true;
          if (Type == TarotCards.Card.CorruptedFullCorruption)
          {
            flag = false;
            foreach (TarotCards.TarotCard tarotCard in playerFarming.CorruptedTrinketsOnlyNegative)
            {
              if (tarotCard.CardType == TarotCards.Card.CorruptedFullCorruption)
                flag = true;
            }
          }
          if (flag)
          {
            str1 = $"<s>{str1}</s>";
            if (TrinketManager.HasTrinket(TarotCards.Card.CorruptedFullCorruption, playerFarming))
              format = $"{format}<br><color=#FFD201>{TarotCards.LocalisedName(TarotCards.Card.CorruptedFullCorruption)}";
          }
        }
        Term = string.Format(format, (object) str1, (object) str2);
        break;
    }
    return LocalizationManager.GetTranslation(Term);
  }

  public static string LocalisedLore(TarotCards.Card Type)
  {
    return LocalizationManager.GetTranslation($"TarotCards/{Type}/Lore");
  }

  public static string Skin(TarotCards.Card Type)
  {
    switch (Type)
    {
      case TarotCards.Card.Hearts1:
        return "Trinkets/TheHearts1";
      case TarotCards.Card.Hearts2:
        return "Trinkets/TheHearts2";
      case TarotCards.Card.Hearts3:
        return "Trinkets/TheHearts3";
      case TarotCards.Card.Lovers1:
        return "Trinkets/TheLovers1";
      case TarotCards.Card.Lovers2:
        return "Trinkets/TheLovers2";
      case TarotCards.Card.Sun:
        return "Trinkets/Sun";
      case TarotCards.Card.Moon:
        return "Trinkets/Moon";
      case TarotCards.Card.GiftFromBelow:
        return "Trinkets/GiftFromBelow";
      case TarotCards.Card.Spider:
        return "Trinkets/Spider";
      case TarotCards.Card.DiseasedHeart:
        return "Trinkets/DiseasedHeart";
      case TarotCards.Card.EyeOfWeakness:
        return "Trinkets/EyeOfWeakness";
      case TarotCards.Card.Arrows:
        return "Trinkets/Arrows";
      case TarotCards.Card.DeathsDoor:
        return "Trinkets/DeathsDoor";
      case TarotCards.Card.TheDeal:
        return "Trinkets/TheDeal";
      case TarotCards.Card.Telescope:
        return "Trinkets/Telescope";
      case TarotCards.Card.HandsOfRage:
        return "Trinkets/HandsOfRage";
      case TarotCards.Card.NaturesGift:
        return "Trinkets/NaturesGift";
      case TarotCards.Card.Skull:
        return "Trinkets/Skull";
      case TarotCards.Card.Potion:
        return "Trinkets/Potion";
      case TarotCards.Card.Sword:
        return "Weapons/Sword";
      case TarotCards.Card.Dagger:
        return "Weapons/Dagger";
      case TarotCards.Card.Axe:
        return "Weapons/Axe";
      case TarotCards.Card.Blunderbuss:
        return "Weapons/Blunderbuss";
      case TarotCards.Card.Fireball:
        return "Curses/Fireball";
      case TarotCards.Card.Tripleshot:
        return "Curses/Tripleshot";
      case TarotCards.Card.Tentacles:
        return "Curses/Tentacles";
      case TarotCards.Card.EnemyBlast:
        return "Curses/EnemyBlast";
      case TarotCards.Card.MovementSpeed:
        return "Trinkets/MovementSpeed";
      case TarotCards.Card.AttackRate:
        return "Trinkets/AttackRate";
      case TarotCards.Card.IncreasedDamage:
        return "Trinkets/IncreasedDamage";
      case TarotCards.Card.IncreaseBlackSoulsDrop:
        return "Trinkets/IncreasedXP";
      case TarotCards.Card.HealChance:
        return "Trinkets/HealChance";
      case TarotCards.Card.NegateDamageChance:
        return "Trinkets/NegateDamageChance";
      case TarotCards.Card.BombOnRoll:
        return "Trinkets/BombOnRoll";
      case TarotCards.Card.GoopOnDamaged:
        return "Trinkets/GoopOnDamaged";
      case TarotCards.Card.GoopOnRoll:
        return "Trinkets/GoopOnRoll";
      case TarotCards.Card.PoisonImmune:
        return "Trinkets/PoisonImmune";
      case TarotCards.Card.DamageOnRoll:
        return "Trinkets/DamageOnRoll";
      case TarotCards.Card.HealTwiceAmount:
        return "Trinkets/HealTwiceAmount";
      case TarotCards.Card.InvincibleWhileHealing:
        return "Trinkets/InvincibleWhileHealing";
      case TarotCards.Card.AmmoEfficient:
        return "Trinkets/AmmoEfficient";
      case TarotCards.Card.BlackSoulAutoRecharge:
        return "Trinkets/BlackSoulAutoRecharge";
      case TarotCards.Card.BlackSoulOnDamage:
        return "Trinkets/BlackSoulOnDamage";
      case TarotCards.Card.NeptunesCurse:
        return "Trinkets/NeptunesCurse";
      case TarotCards.Card.RabbitFoot:
        return "Trinkets/RabbitFoot";
      case TarotCards.Card.HoldToHeal:
        return "Trinkets/HoldToHeal";
      case TarotCards.Card.MoreRelics:
        return "Trinkets/MoreRelics";
      case TarotCards.Card.TentacleOnDamaged:
        return "Trinkets/TentacleOnDamaged";
      case TarotCards.Card.InvincibilityPerRoom:
        return "Trinkets/InvincibilityPerRoom";
      case TarotCards.Card.BombOnDamaged:
        return "Trinkets/BombOnDamaged";
      case TarotCards.Card.ImmuneToTraps:
        return "Trinkets/ImmuneToTraps";
      case TarotCards.Card.ImmuneToProjectiles:
        return "Trinkets/ImmuneToProjectiles";
      case TarotCards.Card.WalkThroughBlocks:
        return "Trinkets/WalkThroughBlocks";
      case TarotCards.Card.DecreaseRelicCharge:
        return "Trinkets/DecreaseRelicCharge";
      case TarotCards.Card.AdventureMapFreedom:
        return "Trinkets/Tree";
      case TarotCards.Card.Recycle:
        return "Trinkets/Recycle";
      case TarotCards.Card.StrikeBack:
        return "Trinkets/StrikeBack";
      case TarotCards.Card.SurpriseAttack:
        return "Trinkets/SurpriseAttack";
      case TarotCards.Card.BossHeal:
        return "Trinkets/BossHeal";
      case TarotCards.Card.NoCorruption:
        return "Trinkets/NoCorruption";
      case TarotCards.Card.Sin:
        return "Trinkets/Sin";
      case TarotCards.Card.ExtraMove:
        return "Trinkets/Ship";
      case TarotCards.Card.ShuffleNode:
        return "Trinkets/Maze";
      case TarotCards.Card.CorruptedBombsAndHealth:
        return "Trinkets/CorruptedBombsAndHealth";
      case TarotCards.Card.CorruptedHeavy:
        return "Trinkets/CorruptedHeavy";
      case TarotCards.Card.CorruptedTradeOff:
        return "Trinkets/CorruptedTradeOff";
      case TarotCards.Card.CorruptedBlackHeartForRelic:
        return "Trinkets/CorruptedBlackHeartForRelic";
      case TarotCards.Card.CorruptedHealForRelic:
        return "Trinkets/CorruptedHealForRelic";
      case TarotCards.Card.CorruptedFullCorruption:
        return "Trinkets/CorruptedFullCorruption";
      case TarotCards.Card.CorruptedPoisonCoins:
        return "Trinkets/CorruptedPoisonCoins";
      case TarotCards.Card.CorruptedRelicCharge:
        return "Trinkets/CorruptedRelicCharge";
      case TarotCards.Card.CorruptedGoopyTrail:
        return "Trinkets/CorruptedGoopyTrail";
      case TarotCards.Card.CoopBetterTogether:
        return "Trinkets/CoopBetterTogether";
      case TarotCards.Card.CoopBetterApart:
        return "Trinkets/CoopBetterApart";
      case TarotCards.Card.CoopBonded:
        return "Trinkets/CoopBonded";
      case TarotCards.Card.CoopGoodTiming:
        return "Trinkets/CoopGoodTiming";
      case TarotCards.Card.CoopExplosive:
        return "Trinkets/CoopExplosive";
      case TarotCards.Card.FrostHeart:
        return "Trinkets/DLC_FrostHeart";
      case TarotCards.Card.FlameHeart:
        return "Trinkets/DLC_FlameHeart";
      case TarotCards.Card.EasyMoney:
        return "Trinkets/DLC_EasyMoney";
      case TarotCards.Card.HighRoller:
        return "Trinkets/DLC_HighRoller";
      case TarotCards.Card.FrostedEnemies:
        return "Trinkets/DLC_FrostsBite";
      case TarotCards.Card.CursedIce:
        return "Trinkets/DLC_CursedIce";
      case TarotCards.Card.LastChance:
        return "Trinkets/DLC_LastChance";
      case TarotCards.Card.Joker:
        return "Trinkets/DLC_Joker";
      case TarotCards.Card.MutatedNegateHit:
        return "Trinkets/MutatedNegateHit";
      case TarotCards.Card.MutatedDropRotburn:
        return "Trinkets/MutatedDropRotburn";
      case TarotCards.Card.MutatedFreezeOnHit:
        return "Trinkets/MutatedFreezeOnHit";
      case TarotCards.Card.MutatedResurrectFullHealth:
        return "Trinkets/MutatedResurrectFullHealth";
      case TarotCards.Card.MutatedInvincibility:
        return "Trinkets/MutatedInvincibility";
      case TarotCards.Card.MutatedSpawnRotDemons:
        return "Trinkets/MutatedSpawnRotDemons";
      case TarotCards.Card.EmptyFervourCritical:
        return "Trinkets/DLC_EmptyFervourCritical";
      case TarotCards.Card.KillEnemiesOnResurrect:
        return "Trinkets/DLC_KillEnemiesOnResurrect";
      case TarotCards.Card.RoomEnterCritter:
        return "Trinkets/DLC_RoomEnterCritter";
      case TarotCards.Card.HitKillEnemy:
        return "Trinkets/DLC_HitKillEnemy";
      case TarotCards.Card.SummonGhost:
        return "Trinkets/DLC_SummonGhost";
      case TarotCards.Card.HeartTarotDrawn:
        return "Trinkets/DLC_HeartTarotDrawn";
      default:
        return "";
    }
  }

  public static bool GetTarotIsDLC(TarotCards.Card cardType) => (uint) (cardType - 84) <= 19U;

  public static bool GetTarotIsCoOp(TarotCards.Card cardType)
  {
    switch (cardType)
    {
      case TarotCards.Card.CoopBetterTogether:
      case TarotCards.Card.CoopBetterApart:
      case TarotCards.Card.CoopBonded:
      case TarotCards.Card.CoopGoodTiming:
      case TarotCards.Card.CoopExplosive:
        return true;
      default:
        return false;
    }
  }

  public static int GetTarotCardWeight(TarotCards.Card cardType)
  {
    switch (cardType)
    {
      case TarotCards.Card.Hearts1:
        return 50;
      case TarotCards.Card.Hearts2:
        return 60;
      case TarotCards.Card.Hearts3:
        return 70;
      case TarotCards.Card.Lovers1:
        return 80 /*0x50*/;
      case TarotCards.Card.Lovers2:
        return 100;
      case TarotCards.Card.Sun:
        return 50;
      case TarotCards.Card.Moon:
        return 50;
      case TarotCards.Card.GiftFromBelow:
        return 50;
      case TarotCards.Card.Spider:
        return 50;
      case TarotCards.Card.DiseasedHeart:
        return 50;
      case TarotCards.Card.EyeOfWeakness:
        return 50;
      case TarotCards.Card.DeathsDoor:
        return 50;
      case TarotCards.Card.TheDeal:
        return 50;
      case TarotCards.Card.Telescope:
        return 50;
      case TarotCards.Card.HandsOfRage:
        return 50;
      case TarotCards.Card.NaturesGift:
        return 50;
      case TarotCards.Card.Skull:
        return 50;
      case TarotCards.Card.Potion:
        return 50;
      case TarotCards.Card.MovementSpeed:
        return 75;
      case TarotCards.Card.AttackRate:
        return 75;
      case TarotCards.Card.IncreasedDamage:
        return 75;
      case TarotCards.Card.IncreaseBlackSoulsDrop:
        return 50;
      case TarotCards.Card.HealChance:
        return 75;
      case TarotCards.Card.NegateDamageChance:
        return 75;
      case TarotCards.Card.BombOnRoll:
        return 50;
      case TarotCards.Card.GoopOnDamaged:
        return 50;
      case TarotCards.Card.GoopOnRoll:
        return 50;
      case TarotCards.Card.PoisonImmune:
        return 50;
      case TarotCards.Card.DamageOnRoll:
        return 100;
      case TarotCards.Card.HealTwiceAmount:
        return 75;
      case TarotCards.Card.AmmoEfficient:
        return 100;
      case TarotCards.Card.BlackSoulAutoRecharge:
        return 100;
      case TarotCards.Card.BlackSoulOnDamage:
        return 75;
      case TarotCards.Card.RabbitFoot:
        return 60;
      default:
        return 100;
    }
  }

  public static int GetMaxTarotCardLevel(TarotCards.Card cardType)
  {
    switch (cardType)
    {
      case TarotCards.Card.Hearts1:
        return 0;
      case TarotCards.Card.Hearts2:
        return 0;
      case TarotCards.Card.Hearts3:
        return 0;
      case TarotCards.Card.Lovers1:
        return 0;
      case TarotCards.Card.Lovers2:
        return 0;
      case TarotCards.Card.Sun:
        return 0;
      case TarotCards.Card.Moon:
        return 0;
      case TarotCards.Card.GiftFromBelow:
        return 1;
      case TarotCards.Card.Spider:
        return 0;
      case TarotCards.Card.DiseasedHeart:
        return 0;
      case TarotCards.Card.EyeOfWeakness:
        return 0;
      case TarotCards.Card.Arrows:
        return 0;
      case TarotCards.Card.DeathsDoor:
        return 2;
      case TarotCards.Card.TheDeal:
        return 0;
      case TarotCards.Card.Telescope:
        return 0;
      case TarotCards.Card.HandsOfRage:
        return 1;
      case TarotCards.Card.NaturesGift:
        return 0;
      case TarotCards.Card.Skull:
        return 0;
      case TarotCards.Card.Potion:
        return 2;
      case TarotCards.Card.MovementSpeed:
        return 2;
      case TarotCards.Card.AttackRate:
        return 2;
      case TarotCards.Card.IncreasedDamage:
        return 1;
      case TarotCards.Card.IncreaseBlackSoulsDrop:
        return 1;
      case TarotCards.Card.HealChance:
        return 1;
      case TarotCards.Card.NegateDamageChance:
        return 1;
      case TarotCards.Card.BombOnRoll:
        return 1;
      case TarotCards.Card.GoopOnDamaged:
        return 0;
      case TarotCards.Card.GoopOnRoll:
        return 1;
      case TarotCards.Card.PoisonImmune:
        return 0;
      case TarotCards.Card.DamageOnRoll:
        return 0;
      case TarotCards.Card.HealTwiceAmount:
        return 0;
      case TarotCards.Card.InvincibleWhileHealing:
        return 0;
      case TarotCards.Card.AmmoEfficient:
        return 2;
      case TarotCards.Card.BlackSoulAutoRecharge:
        return 1;
      case TarotCards.Card.BlackSoulOnDamage:
        return 1;
      case TarotCards.Card.NeptunesCurse:
        return 0;
      case TarotCards.Card.RabbitFoot:
        return 0;
      case TarotCards.Card.MoreRelics:
        return 0;
      case TarotCards.Card.DecreaseRelicCharge:
        return 2;
      case TarotCards.Card.AdventureMapFreedom:
        return 0;
      case TarotCards.Card.Recycle:
        return 0;
      case TarotCards.Card.StrikeBack:
        return 0;
      case TarotCards.Card.SurpriseAttack:
        return 2;
      case TarotCards.Card.BossHeal:
        return 0;
      case TarotCards.Card.NoCorruption:
        return 0;
      case TarotCards.Card.Sin:
        return 0;
      case TarotCards.Card.ExtraMove:
        return 0;
      case TarotCards.Card.ShuffleNode:
        return 0;
      case TarotCards.Card.CorruptedBombsAndHealth:
        return 0;
      case TarotCards.Card.CorruptedHeavy:
        return 0;
      case TarotCards.Card.CorruptedTradeOff:
        return 0;
      case TarotCards.Card.CorruptedBlackHeartForRelic:
        return 0;
      case TarotCards.Card.CorruptedHealForRelic:
        return 0;
      case TarotCards.Card.CorruptedFullCorruption:
        return 0;
      case TarotCards.Card.CorruptedPoisonCoins:
        return 0;
      case TarotCards.Card.CorruptedRelicCharge:
        return 0;
      case TarotCards.Card.CorruptedGoopyTrail:
        return 0;
      case TarotCards.Card.FrostHeart:
        return 0;
      case TarotCards.Card.FlameHeart:
        return 0;
      case TarotCards.Card.EasyMoney:
        return 2;
      case TarotCards.Card.FrostedEnemies:
        return 0;
      case TarotCards.Card.CursedIce:
        return 0;
      case TarotCards.Card.LastChance:
        return 2;
      case TarotCards.Card.Joker:
        return 0;
      case TarotCards.Card.MutatedNegateHit:
        return 2;
      case TarotCards.Card.MutatedInvincibility:
        return 2;
      case TarotCards.Card.MutatedSpawnRotDemons:
        return 2;
      default:
        return 0;
    }
  }

  public static string AnimationSuffix(TarotCards.Card Type)
  {
    switch (Type)
    {
      case TarotCards.Card.Sword:
        return "sword";
      case TarotCards.Card.Dagger:
        return "dagger";
      case TarotCards.Card.Axe:
        return "axe";
      case TarotCards.Card.Blunderbuss:
        return "blunderbuss";
      case TarotCards.Card.Hammer:
        return "hammer";
      case TarotCards.Card.Fireball:
        return "fireball";
      case TarotCards.Card.Tripleshot:
        return "tripleshot";
      case TarotCards.Card.Tentacles:
        return "tentacle";
      case TarotCards.Card.EnemyBlast:
        return "enemyblast";
      case TarotCards.Card.Gauntlet:
        return "guantlets";
      default:
        return $"Card {Type} Animation Suffix not set";
    }
  }

  public static TarotCards.TarotCard DrawRandomCard(
    PlayerFarming playerFarming,
    bool canBeCorrupted = true)
  {
    List<TarotCards.Card> unusedFoundTrinkets = TarotCards.GetUnusedFoundTrinkets(playerFarming, canBeCorrupted);
    if (unusedFoundTrinkets.Count <= 0)
      return (TarotCards.TarotCard) null;
    TarotCards.Card card = unusedFoundTrinkets[UnityEngine.Random.Range(0, unusedFoundTrinkets.Count)];
    int a = 0;
    if (DataManager.Instance.dungeonRun >= 5)
    {
      while ((double) UnityEngine.Random.Range(0.0f, 1f) < 0.27500000596046448 * (double) DataManager.Instance.GetLuckMultiplier())
        ++a;
    }
    int upgrade = Mathf.Min(a, TarotCards.GetMaxTarotCardLevel(card));
    return new TarotCards.TarotCard(card, upgrade);
  }

  public static TarotCards.TarotCard DrawRandomCorruptedCard(PlayerFarming playerFarming)
  {
    List<TarotCards.Card> cardList = new List<TarotCards.Card>();
    foreach (TarotCards.Card corruptedCard in TarotCards.CorruptedCards)
    {
      if (!TrinketManager.HasTrinket(corruptedCard, playerFarming) && !TrinketManager.HasEncounteredTrinket(corruptedCard, playerFarming) && corruptedCard != TarotCards.Card.NoCorruption)
        cardList.Add(corruptedCard);
    }
    return cardList.Count == 0 ? (TarotCards.TarotCard) null : new TarotCards.TarotCard(cardList[UnityEngine.Random.Range(0, cardList.Count)], 0);
  }

  public static TarotCards.Card GiveNewTrinket()
  {
    List<TarotCards.Card> unfoundTrinkets = TarotCards.GetUnfoundTrinkets();
    return unfoundTrinkets[UnityEngine.Random.Range(0, unfoundTrinkets.Count)];
  }

  public static List<TarotCards.Card> GetUnfoundTrinkets()
  {
    List<TarotCards.Card> unfoundTrinkets = new List<TarotCards.Card>();
    foreach (TarotCards.Card allTrinket in DataManager.AllTrinkets)
    {
      bool flag = false;
      foreach (TarotCards.Card playerFoundTrinket in DataManager.Instance.PlayerFoundTrinkets)
      {
        if (playerFoundTrinket == allTrinket)
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        unfoundTrinkets.Add(allTrinket);
    }
    return unfoundTrinkets;
  }

  public static bool IsUnlocked(TarotCards.Card card)
  {
    return DataManager.Instance.PlayerFoundTrinkets.Contains(card);
  }

  public static bool UnlockTrinket(TarotCards.Card card)
  {
    if (DataManager.Instance.PlayerFoundTrinkets.Contains(card))
      return false;
    DataManager.Instance.Alerts.TarotCardAlerts.AddOnce(card);
    DataManager.Instance.PlayerFoundTrinkets.Add(card);
    int num1 = 0;
    for (int index = 0; index < DataManager.Instance.PlayerFoundTrinkets.Count; ++index)
    {
      if (!TarotCards.CoopCards.Contains<TarotCards.Card>(DataManager.Instance.PlayerFoundTrinkets[index]))
        ++num1;
    }
    int num2 = 0;
    for (int index = 0; index < DataManager.AllTrinkets.Count; ++index)
    {
      if (!TarotCards.CoopCards.Contains<TarotCards.Card>(DataManager.AllTrinkets[index]))
        ++num2;
    }
    if (num1 >= num2)
      AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("ALL_TAROTS_UNLOCKED"));
    return true;
  }

  public static void UnlockTrinkets(params TarotCards.Card[] cards)
  {
    foreach (TarotCards.Card card in cards)
      TarotCards.UnlockTrinket(card);
  }

  public static TarotCards.Card UnlockRandomTrinket()
  {
    List<TarotCards.Card> cardList = new List<TarotCards.Card>();
    foreach (TarotCards.Card allTrinket in DataManager.AllTrinkets)
    {
      if (!DataManager.Instance.PlayerFoundTrinkets.Contains(allTrinket))
        cardList.Add(allTrinket);
    }
    if (cardList.Count <= 0)
      return TarotCards.Card.Count;
    TarotCards.Card card = cardList[UnityEngine.Random.Range(0, cardList.Count)];
    DataManager.Instance.PlayerFoundTrinkets.Add(card);
    int num1 = 0;
    for (int index = 0; index < DataManager.Instance.PlayerFoundTrinkets.Count; ++index)
    {
      if (!TarotCards.CoopCards.Contains<TarotCards.Card>(DataManager.Instance.PlayerFoundTrinkets[index]))
        ++num1;
    }
    int num2 = 0;
    for (int index = 0; index < DataManager.AllTrinkets.Count; ++index)
    {
      if (!TarotCards.CoopCards.Contains<TarotCards.Card>(DataManager.AllTrinkets[index]))
        ++num2;
    }
    if (num1 >= num2)
      AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("ALL_TAROTS_UNLOCKED"));
    return card;
  }

  public static List<TarotCards.Card> GetUnusedFoundTrinkets(
    PlayerFarming playerFarming,
    bool canBeCorrupted = true)
  {
    List<TarotCards.Card> unusedFoundTrinkets = new List<TarotCards.Card>();
    foreach (TarotCards.Card playerFoundTrinket in DataManager.Instance.PlayerFoundTrinkets)
    {
      if ((!TarotCards.IsAdventureMapNodeBypassingCard(playerFoundTrinket) || TarotCards.CanBypassAdventureMap()) && (!TarotCards.IsCurseRelatedTarotCard(playerFoundTrinket) || DataManager.Instance.EnabledSpells) && (!TarotCards.IsWeaponRelatedTarotCard(playerFoundTrinket) || DataManager.Instance.PlayerFleece != 6) && (!TarotCards.IsHealthRelatedTarotCard(playerFoundTrinket) || DataManager.Instance.PlayerFleece != 7 && DataManager.Instance.PlayerFleece != 8) && (playerFoundTrinket != TarotCards.Card.EasyMoney || DataManager.Instance.PlayerFleece != 8) && (playerFoundTrinket != TarotCards.Card.MutatedResurrectFullHealth || DataManager.Instance.PlayerFleece != 5))
      {
        if (playerFoundTrinket == TarotCards.Card.MutatedResurrectFullHealth && !UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Ability_Resurrection))
          Debug.Log((object) "Dropping Stolen Bile as player doesnt have resurrect ability...");
        else if ((!TarotCards.IsResourceRelatedCard(playerFoundTrinket) || !DungeonSandboxManager.Active) && (playerFoundTrinket != TarotCards.Card.WalkThroughBlocks || !((UnityEngine.Object) playerFarming != (UnityEngine.Object) null) || (double) playerFarming.playerRelic.PlayerScaleModifier <= 1.0) && (playerFoundTrinket != TarotCards.Card.CorruptedFullCorruption || !TrinketManager.HasTrinket(TarotCards.Card.NoCorruption, playerFarming)) && (playerFoundTrinket != TarotCards.Card.NoCorruption || !TrinketManager.HasTrinket(TarotCards.Card.CorruptedFullCorruption, playerFarming)) && (playerFoundTrinket != TarotCards.Card.CorruptedHeavy || UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_HeavyAttacks)) && (playerFoundTrinket != TarotCards.Card.TheDeal || DataManager.Instance.PlayerFleece != 7) && (playerFoundTrinket != TarotCards.Card.NoCorruption || (double) UnityEngine.Random.value >= 0.5) && (SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter || !TarotCards.WitnerCards.Contains(playerFoundTrinket)) && (canBeCorrupted || !TarotCards.CorruptedCards.Contains<TarotCards.Card>(playerFoundTrinket)) && (playerFarming.CorruptedTrinketsOnlyNegative.Count <= 0 && playerFarming.CorruptedTrinketsOnlyPositive.Count <= 0 || playerFoundTrinket != TarotCards.Card.NoCorruption) && (playerFarming.CorruptedTrinketsOnlyNegative.Count <= 0 && playerFarming.CorruptedTrinketsOnlyPositive.Count <= 0 || playerFoundTrinket != TarotCards.Card.CorruptedFullCorruption) && !TrinketManager.HasTrinket(playerFoundTrinket, playerFarming) && !TrinketManager.HasEncounteredTrinket(playerFoundTrinket, playerFarming))
          unusedFoundTrinkets.Add(playerFoundTrinket);
      }
    }
    if (PlayerFarming.playersCount > 1)
    {
      foreach (TarotCards.Card coopCard in TarotCards.CoopCards)
      {
        if (!TrinketManager.HasTrinket(coopCard) && !TrinketManager.HasEncounteredTrinket(coopCard))
        {
          if (!unusedFoundTrinkets.Contains(coopCard))
            unusedFoundTrinkets.Add(coopCard);
        }
        else
          unusedFoundTrinkets.Remove(coopCard);
      }
      foreach (TarotCards.Card card in TarotCards.COOP_TAROTS_EXCLUDE)
      {
        if (unusedFoundTrinkets.Contains(card))
          unusedFoundTrinkets.Remove(card);
      }
    }
    else
    {
      foreach (TarotCards.Card coopCard in TarotCards.CoopCards)
      {
        if (unusedFoundTrinkets.Contains(coopCard))
          unusedFoundTrinkets.Remove(coopCard);
      }
    }
    return unusedFoundTrinkets;
  }

  public static bool IsHealthRelatedTarotCard(TarotCards.Card card)
  {
    switch (card)
    {
      case TarotCards.Card.Hearts1:
      case TarotCards.Card.Hearts2:
      case TarotCards.Card.Hearts3:
      case TarotCards.Card.Lovers1:
      case TarotCards.Card.Lovers2:
      case TarotCards.Card.DiseasedHeart:
      case TarotCards.Card.DeathsDoor:
      case TarotCards.Card.TheDeal:
      case TarotCards.Card.HealChance:
      case TarotCards.Card.GoopOnDamaged:
      case TarotCards.Card.HealTwiceAmount:
      case TarotCards.Card.BlackSoulOnDamage:
      case TarotCards.Card.TentacleOnDamaged:
      case TarotCards.Card.BombOnDamaged:
      case TarotCards.Card.Recycle:
      case TarotCards.Card.CorruptedBombsAndHealth:
      case TarotCards.Card.CorruptedBlackHeartForRelic:
      case TarotCards.Card.FrostHeart:
      case TarotCards.Card.FlameHeart:
      case TarotCards.Card.EasyMoney:
      case TarotCards.Card.LastChance:
      case TarotCards.Card.Joker:
        return true;
      default:
        return false;
    }
  }

  public static bool IsAdventureMapNodeBypassingCard(TarotCards.Card card)
  {
    switch (card)
    {
      case TarotCards.Card.AdventureMapFreedom:
      case TarotCards.Card.ExtraMove:
      case TarotCards.Card.ShuffleNode:
        return true;
      default:
        return false;
    }
  }

  public static bool IsCurseRelatedTarotCard(TarotCards.Card card)
  {
    switch (card)
    {
      case TarotCards.Card.Arrows:
      case TarotCards.Card.Potion:
      case TarotCards.Card.IncreaseBlackSoulsDrop:
      case TarotCards.Card.AmmoEfficient:
      case TarotCards.Card.BlackSoulAutoRecharge:
      case TarotCards.Card.BlackSoulOnDamage:
        return true;
      default:
        return false;
    }
  }

  public static bool IsWeaponRelatedTarotCard(TarotCards.Card card)
  {
    switch (card)
    {
      case TarotCards.Card.HandsOfRage:
      case TarotCards.Card.AttackRate:
      case TarotCards.Card.IncreasedDamage:
        return true;
      default:
        return false;
    }
  }

  public static bool IsResourceRelatedCard(TarotCards.Card card)
  {
    return card == TarotCards.Card.NaturesGift || card == TarotCards.Card.NeptunesCurse;
  }

  public static void ApplyInstantEffects(TarotCards.TarotCard card, PlayerFarming playerFarming)
  {
    switch (card.CardType)
    {
      case TarotCards.Card.Hearts1:
      case TarotCards.Card.Hearts2:
      case TarotCards.Card.Hearts3:
      case TarotCards.Card.CorruptedBombsAndHealth:
        if (!PlayerFleeceManager.FleecePreventsHealthPickups() && !TrinketManager.IsCorruptedPositiveEffectNegated(card.CardType, playerFarming))
        {
          playerFarming.health.TotalSpiritHearts += TarotCards.GetSpiritHeartCount(card);
          BiomeConstants.Instance.EmitHeartPickUpVFX(playerFarming.CameraBone.transform.position, 0.0f, "red", "burst_big");
          AudioManager.Instance.PlayOneShot("event:/player/collect_blue_heart", playerFarming.transform.position);
          break;
        }
        break;
      case TarotCards.Card.Lovers1:
        if (!PlayerFleeceManager.FleecePreventsHealthPickups())
        {
          playerFarming.GetComponent<HealthPlayer>().BlueHearts += 2f;
          Vector3 position = playerFarming.CameraBone.transform.position;
          BiomeConstants.Instance.EmitHeartPickUpVFX(position, 0.0f, "blue", "burst_big");
          AudioManager.Instance.PlayOneShot("event:/player/collect_blue_heart", position);
          break;
        }
        break;
      case TarotCards.Card.Lovers2:
        if (!PlayerFleeceManager.FleecePreventsHealthPickups())
        {
          playerFarming.GetComponent<HealthPlayer>().BlueHearts += 4f;
          Vector3 position = playerFarming.CameraBone.transform.position;
          BiomeConstants.Instance.EmitHeartPickUpVFX(position, 0.0f, "blue", "burst_big");
          AudioManager.Instance.PlayOneShot("event:/player/collect_blue_heart", position);
          break;
        }
        break;
      case TarotCards.Card.DiseasedHeart:
        if (!PlayerFleeceManager.FleecePreventsHealthPickups())
        {
          playerFarming.GetComponent<HealthPlayer>().BlackHearts += 2f;
          Vector3 position = playerFarming.CameraBone.transform.position;
          BiomeConstants.Instance.EmitHeartPickUpVFX(position, 0.0f, "black", "burst_big");
          AudioManager.Instance.PlayOneShot("event:/player/collect_blue_heart", position);
          break;
        }
        break;
      case TarotCards.Card.TheDeal:
        if (ResurrectOnHud.ResurrectionType == ResurrectionType.Pyre || ResurrectOnHud.ResurrectionType == ResurrectionType.CorruptedMonolith)
          ResurrectOnHud.CachedResurrectionType = ResurrectOnHud.ResurrectionType;
        ResurrectOnHud.ResurrectionType = ResurrectionType.DealTarot;
        break;
      case TarotCards.Card.IncreasedDamage:
        AudioManager.Instance.PlayOneShot("event:/hearts_of_the_faithful/swords_appear", playerFarming.transform.position);
        BiomeConstants.Instance.EmitHeartPickUpVFX(playerFarming.CameraBone.transform.position, 0.0f, "strength", "strength");
        break;
      case TarotCards.Card.CorruptedBlackHeartForRelic:
        if (!PlayerFleeceManager.FleecePreventsHealthPickups() && !TrinketManager.IsCorruptedPositiveEffectNegated(card.CardType, playerFarming))
        {
          playerFarming.GetComponent<HealthPlayer>().BlackHearts += 4f;
          Vector3 position = playerFarming.CameraBone.transform.position;
          BiomeConstants.Instance.EmitHeartPickUpVFX(position, 0.0f, "black", "burst_big");
          AudioManager.Instance.PlayOneShot("event:/player/collect_blue_heart", position);
          break;
        }
        break;
      case TarotCards.Card.FrostHeart:
        if (!PlayerFleeceManager.FleecePreventsHealthPickups())
        {
          playerFarming.GetComponent<HealthPlayer>().IceHearts += 2f;
          Vector3 position = playerFarming.CameraBone.transform.position;
          BiomeConstants.Instance.EmitHeartPickUpVFX(position, 0.0f, "black", "burst_big");
          AudioManager.Instance.PlayOneShot("event:/dlc/player/collect_ice_heart", position);
          break;
        }
        break;
      case TarotCards.Card.FlameHeart:
        if (!PlayerFleeceManager.FleecePreventsHealthPickups())
        {
          playerFarming.GetComponent<HealthPlayer>().FireHearts += 2f;
          Vector3 position = playerFarming.CameraBone.transform.position;
          BiomeConstants.Instance.EmitHeartPickUpVFX(position, 0.0f, "black", "burst_big");
          AudioManager.Instance.PlayOneShot("event:/dlc/tarot/flameheart_trigger", position);
          break;
        }
        break;
      case TarotCards.Card.MutatedInvincibility:
        playerFarming.playerController.PlayInvincibleSFX = true;
        playerFarming.playerController.MakeUntouchable(60f * (float) (card.UpgradeIndex + 1));
        break;
      case TarotCards.Card.MutatedSpawnRotDemons:
        TarotCards.SpawnRotDemons(TarotCards.GetRotDemonsToSpawn(card), playerFarming);
        break;
      case TarotCards.Card.EmptyFervourCritical:
        playerFarming.playerWeapon.CriticalTimer.gameObject.SetActive(true);
        break;
      case TarotCards.Card.SummonGhost:
        Addressables_wrapper.InstantiateAsync((object) "Assets/Prefabs/Familiars/Ghost Familiar.prefab", playerFarming.transform.position, Quaternion.identity, playerFarming.transform.parent, (Action<AsyncOperationHandle<GameObject>>) (o =>
        {
          Familiar component = o.Result.GetComponent<Familiar>();
          component.SetMaster(playerFarming);
          component.Container.transform.localScale = Vector3.zero;
          AudioManager.Instance.PlayOneShot("event:/dlc/tarot/ghost_summon_trigger", component.transform.position);
        }));
        break;
    }
    playerFarming.playerSpells.faithAmmo.Ammo = playerFarming.playerSpells.faithAmmo.Ammo;
    if (!TrinketManager.HasTrinket(TarotCards.Card.HeartTarotDrawn, playerFarming))
      return;
    playerFarming.health.IceHearts += 2f;
    BiomeConstants.Instance.EmitHeartPickUpVFX(playerFarming.transform.position, 0.0f, "red", "burst_big");
    AudioManager.Instance.PlayOneShot("event:/dlc/player/collect_ice_heart", playerFarming.transform.position);
  }

  public static float GetSpiritHeartCount(TarotCards.TarotCard card)
  {
    switch (card.CardType)
    {
      case TarotCards.Card.Hearts1:
        return 1f;
      case TarotCards.Card.Hearts2:
        return 2f;
      case TarotCards.Card.Hearts3:
        return 4f;
      case TarotCards.Card.CorruptedBombsAndHealth:
        return 2f;
      default:
        return 0.0f;
    }
  }

  public static int GetSpiritAmmoCount(TarotCards.TarotCard card)
  {
    return card.CardType == TarotCards.Card.Arrows ? Mathf.Max(1, Mathf.RoundToInt((float) (DataManager.Instance.PLAYER_ARROW_TOTAL_AMMO + DataManager.Instance.PLAYER_SPIRIT_TOTAL_AMMO) * 0.2f)) : 0;
  }

  public static float GetWeaponDamageMultiplerIncrease(TarotCards.TarotCard card)
  {
    switch (card.CardType)
    {
      case TarotCards.Card.Sun:
        return !TimeManager.IsDay ? 0.0f : 0.2f;
      case TarotCards.Card.Moon:
        return !TimeManager.IsNight ? 0.0f : 0.3f;
      case TarotCards.Card.IncreasedDamage:
        return card.UpgradeIndex == 1 ? 0.5f : 0.2f;
      case TarotCards.Card.CorruptedFullCorruption:
        return 3f;
      case TarotCards.Card.CoopBetterTogether:
        return TarotCards.IsWithinDistanceForBetterTogether() ? 1f : 0.0f;
      case TarotCards.Card.CoopBetterApart:
        return TarotCards.IsWithinDistanceForBetterApart() ? 1f : 0.0f;
      default:
        return 0.0f;
    }
  }

  public static bool IsWithinDistanceForBetterTogether()
  {
    if (PlayerFarming.playersCount <= 1 || (double) Vector3.Distance(PlayerFarming.players[0].transform.position, PlayerFarming.players[1].transform.position) >= 3.0)
      return false;
    return TrinketManager.HasTrinket(TarotCards.Card.CoopBetterTogether, PlayerFarming.players[0]) || TrinketManager.HasTrinket(TarotCards.Card.CoopBetterTogether, PlayerFarming.players[1]);
  }

  public static bool IsWithinDistanceForBetterApart()
  {
    if (PlayerFarming.playersCount <= 1 || (double) Vector3.Distance(PlayerFarming.players[0].transform.position, PlayerFarming.players[1].transform.position) <= 7.0)
      return false;
    return TrinketManager.HasTrinket(TarotCards.Card.CoopBetterApart, PlayerFarming.players[0]) || TrinketManager.HasTrinket(TarotCards.Card.CoopBetterApart, PlayerFarming.players[1]);
  }

  public static float GetCurseDamageMultiplerIncrease(TarotCards.TarotCard card)
  {
    if (card.CardType != TarotCards.Card.Potion)
      return 0.0f;
    switch (card.UpgradeIndex)
    {
      case 1:
        return 0.5f;
      case 2:
        return 1f;
      default:
        return 0.25f;
    }
  }

  public static float GetWeaponCritChanceIncrease(TarotCards.TarotCard card)
  {
    return card.CardType == TarotCards.Card.EyeOfWeakness ? 0.1f : 0.0f;
  }

  public static int GetLootIncreaseModifier(
    TarotCards.TarotCard card,
    InventoryItem.ITEM_TYPE itemType)
  {
    return card.CardType == TarotCards.Card.NaturesGift ? 1 : 0;
  }

  public static float GetMovementSpeedMultiplier(
    TarotCards.TarotCard card,
    PlayerFarming playerFarming)
  {
    switch (card.CardType)
    {
      case TarotCards.Card.MovementSpeed:
        switch (card.UpgradeIndex)
        {
          case 1:
            return 0.5f;
          case 2:
            return 1f;
          default:
            return 0.25f;
        }
      case TarotCards.Card.CorruptedHeavy:
        return TrinketManager.IsCorruptedNegativeEffectNegated(TarotCards.Card.CorruptedHeavy, playerFarming) ? 0.0f : -0.25f;
      default:
        return 0.0f;
    }
  }

  public static float GetAttackRateMultiplier(TarotCards.TarotCard card)
  {
    switch (card.CardType)
    {
      case TarotCards.Card.AttackRate:
        switch (card.UpgradeIndex)
        {
          case 1:
            return 0.5f;
          case 2:
            return 2f;
          default:
            return 0.25f;
        }
      case TarotCards.Card.CorruptedGoopyTrail:
        int upgradeIndex = card.UpgradeIndex;
        return -0.25f;
      default:
        return 0.0f;
    }
  }

  public static float GetBlackSoulsMultiplier(TarotCards.TarotCard card)
  {
    if (card.CardType != TarotCards.Card.IncreaseBlackSoulsDrop)
      return 0.0f;
    return card.UpgradeIndex == 1 ? 2f : 1f;
  }

  public static float GetHealChance(TarotCards.TarotCard card)
  {
    if (card.CardType != TarotCards.Card.HealChance)
      return 0.0f;
    return card.UpgradeIndex == 1 ? 0.2f : 0.1f;
  }

  public static float GetNegateDamageChance(TarotCards.TarotCard card)
  {
    switch (card.CardType)
    {
      case TarotCards.Card.NegateDamageChance:
        return card.UpgradeIndex == 1 ? 0.2f : 0.1f;
      case TarotCards.Card.MutatedNegateHit:
        return 0.25f;
      default:
        return 0.0f;
    }
  }

  public static int GetDamageAllEnemiesAmount(TarotCards.TarotCard card)
  {
    if (card.CardType != TarotCards.Card.DeathsDoor)
      return 0;
    switch (card.UpgradeIndex)
    {
      case 1:
        return 3;
      case 2:
        return 10;
      default:
        return 2;
    }
  }

  public static int GetEnemyDamageMultiplier(TarotCards.TarotCard card)
  {
    return card.CardType == TarotCards.Card.CorruptedTradeOff ? 1 : 0;
  }

  public static int GetEnemyPoisonDamageMultiplier(TarotCards.TarotCard card)
  {
    return card.CardType == TarotCards.Card.CorruptedPoisonCoins ? 1 : 0;
  }

  public static int GetDamageTradeOff(TarotCards.TarotCard card)
  {
    if (card.CardType != TarotCards.Card.CorruptedTradeOff)
      return 0;
    int upgradeIndex = card.UpgradeIndex;
    return 10;
  }

  public static int GetHealthAmountMultiplier(TarotCards.TarotCard card)
  {
    return card.CardType == TarotCards.Card.HealTwiceAmount ? 1 : 0;
  }

  public static float GetAmmoEfficiency(TarotCards.TarotCard card)
  {
    if (card.CardType != TarotCards.Card.AmmoEfficient)
      return 0.0f;
    switch (card.UpgradeIndex)
    {
      case 1:
        return 0.5f;
      case 2:
        return 0.75f;
      default:
        return 0.25f;
    }
  }

  public static int GetBlackSoulsOnDamage(TarotCards.TarotCard card)
  {
    if (card.CardType != TarotCards.Card.BlackSoulOnDamage)
      return 0;
    return card.UpgradeIndex == 1 ? 10 : 5;
  }

  public static InventoryItem GetItemToDrop(TarotCards.TarotCard card)
  {
    if (card.CardType != TarotCards.Card.NeptunesCurse || (double) UnityEngine.Random.Range(0.0f, 1f) <= 0.89999997615814209)
      return (InventoryItem) null;
    List<InventoryItem.ITEM_TYPE> itemTypeList = new List<InventoryItem.ITEM_TYPE>()
    {
      InventoryItem.ITEM_TYPE.FISH,
      InventoryItem.ITEM_TYPE.FISH_BIG,
      InventoryItem.ITEM_TYPE.FISH_SMALL
    };
    return new InventoryItem(itemTypeList[UnityEngine.Random.Range(0, itemTypeList.Count)]);
  }

  public static float GetChanceOfGainingBlueHeart(TarotCards.TarotCard card)
  {
    if (card.CardType != TarotCards.Card.GiftFromBelow)
      return 0.0f;
    return card.UpgradeIndex == 1 ? 0.1f : 0.05f;
  }

  public static float GetChanceForRelicsMultiplier(TarotCards.TarotCard card)
  {
    return card.CardType == TarotCards.Card.MoreRelics ? 5f : 0.0f;
  }

  public static float GetRelicChargeMultiplier(TarotCards.TarotCard card)
  {
    if (card.CardType != TarotCards.Card.DecreaseRelicCharge)
      return 0.0f;
    switch (card.UpgradeIndex)
    {
      case 1:
        return 0.5f;
      case 2:
        return 1f;
      default:
        return 0.25f;
    }
  }

  public static float GetRelicDamageMultiplier(TarotCards.TarotCard card)
  {
    if (card.CardType != TarotCards.Card.CorruptedRelicCharge)
      return 0.0f;
    int upgradeIndex = card.UpgradeIndex;
    return 1f;
  }

  public static float GetIceCurseDamageMultiplier(TarotCards.TarotCard card)
  {
    if (card.CardType != TarotCards.Card.CursedIce)
      return 0.0f;
    int upgradeIndex = card.UpgradeIndex;
    return 3f;
  }

  public static float GetInvincibilityTimeEnteringNewRoom(TarotCards.TarotCard card)
  {
    if (card.CardType != TarotCards.Card.InvincibilityPerRoom)
      return 0.0f;
    switch (card.UpgradeIndex)
    {
      case 1:
        return 3.5f;
      case 2:
        return 4f;
      default:
        return 3f;
    }
  }

  public static int GetRotDemonsToSpawn(TarotCards.TarotCard card)
  {
    if (card.CardType != TarotCards.Card.MutatedSpawnRotDemons)
      return 0;
    switch (card.UpgradeIndex)
    {
      case 1:
        return 2;
      case 2:
        return 5;
      default:
        return 1;
    }
  }

  public static float GetHeavyAttackCostMultiplier(TarotCards.TarotCard card)
  {
    return card.CardType == TarotCards.Card.CorruptedHeavy ? 0.0f : 1f;
  }

  public static float GetLightningChanceOnKill(TarotCards.TarotCard card)
  {
    if (card.CardType != TarotCards.Card.SurpriseAttack)
      return 0.0f;
    switch (card.UpgradeIndex)
    {
      case 1:
        return 0.2f;
      case 2:
        return 0.3f;
      default:
        return 0.1f;
    }
  }

  public static float GetCoinsDropMultiplier(TarotCards.TarotCard card)
  {
    if (card.CardType != TarotCards.Card.CorruptedPoisonCoins)
      return 0.0f;
    int upgradeIndex = card.UpgradeIndex;
    return 1f;
  }

  public static float GetSinChance(TarotCards.TarotCard card)
  {
    if (card.CardType != TarotCards.Card.Sin)
      return 0.0f;
    int upgradeIndex = card.UpgradeIndex;
    return 0.04f;
  }

  public static float GetDodgeDistanceMultiplier(TarotCards.TarotCard card)
  {
    if (card.CardType != TarotCards.Card.HighRoller)
      return 0.0f;
    int upgradeIndex = card.UpgradeIndex;
    return 0.5f;
  }

  public static void MutateFollowers(int mutateCount)
  {
    int num = 0;
    List<FollowerInfo> ts = new List<FollowerInfo>((IEnumerable<FollowerInfo>) DataManager.Instance.Followers);
    ts.Shuffle<FollowerInfo>();
    foreach (FollowerInfo follower in ts)
    {
      if (TarotCards.CanMutateFollower(follower) && !FollowerManager.UniqueFollowerIDs.Contains(follower.ID))
      {
        FollowerBrain brainById = FollowerBrain.FindBrainByID(follower.ID);
        if (brainById != null)
        {
          brainById.AddTrait(FollowerTrait.TraitType.Mutated, true);
          ++num;
          if (num >= mutateCount)
            break;
        }
      }
    }
  }

  public static bool CanMutateFollower(FollowerInfo follower)
  {
    return (double) follower.Illness <= 0.0 && follower.CursedState == Thought.None && !FollowerManager.FollowerLocked(in follower.ID) && !follower.Traits.Contains(FollowerTrait.TraitType.Mutated) && !follower.Traits.Contains(FollowerTrait.TraitType.MutatedVisual) && !follower.Traits.Contains(FollowerTrait.TraitType.MutatedImmune);
  }

  public static void SpawnRotDemons(int count, PlayerFarming player)
  {
    for (int index = 0; index < count; ++index)
      player.playerRelic.SpawnRotDemon();
  }

  public enum Card
  {
    Hearts1,
    Hearts2,
    Hearts3,
    Lovers1,
    Lovers2,
    Sun,
    Moon,
    GiftFromBelow,
    Spider,
    DiseasedHeart,
    EyeOfWeakness,
    Arrows,
    DeathsDoor,
    TheDeal,
    Telescope,
    HandsOfRage,
    NaturesGift,
    Skull,
    Potion,
    Sword,
    Dagger,
    Axe,
    Blunderbuss,
    Hammer,
    Fireball,
    Tripleshot,
    Tentacles,
    EnemyBlast,
    ProjectileAOE,
    Vortex,
    MegaSlash,
    MovementSpeed,
    AttackRate,
    IncreasedDamage,
    IncreaseBlackSoulsDrop,
    HealChance,
    NegateDamageChance,
    BombOnRoll,
    GoopOnDamaged,
    GoopOnRoll,
    PoisonImmune,
    DamageOnRoll,
    HealTwiceAmount,
    InvincibleWhileHealing,
    AmmoEfficient,
    BlackSoulAutoRecharge,
    BlackSoulOnDamage,
    NeptunesCurse,
    RabbitFoot,
    Gauntlet,
    HoldToHeal,
    MoreRelics,
    TentacleOnDamaged,
    InvincibilityPerRoom,
    BombOnDamaged,
    ImmuneToTraps,
    ImmuneToProjectiles,
    WalkThroughBlocks,
    DecreaseRelicCharge,
    AdventureMapFreedom,
    Recycle,
    StrikeBack,
    SurpriseAttack,
    BossHeal,
    NoCorruption,
    Sin,
    GoopDamage,
    DoubleCoins,
    ExtraMove,
    ShuffleNode,
    CorruptedBombsAndHealth,
    CorruptedHeavy,
    CorruptedTradeOff,
    CorruptedBlackHeartForRelic,
    CorruptedHealForRelic,
    CorruptedFullCorruption,
    CorruptedPoisonCoins,
    CorruptedRelicCharge,
    CorruptedGoopyTrail,
    CoopBetterTogether,
    CoopBetterApart,
    CoopBonded,
    CoopGoodTiming,
    CoopExplosive,
    FrostHeart,
    FlameHeart,
    EasyMoney,
    HighRoller,
    FrostedEnemies,
    CursedIce,
    LastChance,
    Joker,
    MutatedNegateHit,
    MutatedDropRotburn,
    MutatedFreezeOnHit,
    MutatedResurrectFullHealth,
    MutatedInvincibility,
    MutatedSpawnRotDemons,
    EmptyFervourCritical,
    KillEnemiesOnResurrect,
    RoomEnterCritter,
    HitKillEnemy,
    SummonGhost,
    HeartTarotDrawn,
    Count,
  }

  [MessagePackObject(false)]
  public class TarotCard
  {
    [Key(0)]
    public TarotCards.Card CardType;
    [Key(1)]
    public int UpgradeIndex;

    public TarotCard()
    {
    }

    public TarotCard(TarotCards.Card type, int upgrade)
    {
      this.CardType = type;
      this.UpgradeIndex = upgrade;
    }
  }

  public enum CardCategory
  {
    Weapon,
    Curse,
    Trinket,
  }
}
