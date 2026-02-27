// Decompiled with JetBrains decompiler
// Type: TarotCards
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class TarotCards
{
  public TarotCards.Card Type;
  public bool Unlocked;
  public float UnlockProgress;
  public bool Used;
  public TarotCards.CardCategory cardCategory;

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

  public static string LocalisedName(TarotCards.Card type)
  {
    int upgradeIndex = 0;
    foreach (TarotCards.TarotCard playerRunTrinket in DataManager.Instance.PlayerRunTrinkets)
    {
      if (playerRunTrinket.CardType == type)
      {
        upgradeIndex = playerRunTrinket.UpgradeIndex;
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

  public static string LocalisedDescription(TarotCards.Card Type)
  {
    int upgradeIndex = 0;
    foreach (TarotCards.TarotCard playerRunTrinket in DataManager.Instance.PlayerRunTrinkets)
    {
      if (playerRunTrinket.CardType == Type)
      {
        upgradeIndex = playerRunTrinket.UpgradeIndex;
        break;
      }
    }
    return TarotCards.LocalisedDescription(Type, upgradeIndex);
  }

  public static string LocalisedDescription(TarotCards.Card Type, int upgradeIndex)
  {
    string Term = $"TarotCards/{Type}/Description";
    if (upgradeIndex > 0)
      Term += upgradeIndex.ToString();
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
      default:
        return "";
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

  public static TarotCards.TarotCard DrawRandomCard()
  {
    List<TarotCards.Card> unusedFoundTrinkets = TarotCards.GetUnusedFoundTrinkets();
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
    if (DataManager.Instance.PlayerFoundTrinkets.Count >= DataManager.AllTrinkets.Count)
      AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("ALL_TAROTS_UNLOCKED"));
    return true;
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
    if (DataManager.Instance.PlayerFoundTrinkets.Count >= DataManager.AllTrinkets.Count)
      AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("ALL_TAROTS_UNLOCKED"));
    return card;
  }

  public static List<TarotCards.Card> GetUnusedFoundTrinkets()
  {
    List<TarotCards.Card> unusedFoundTrinkets = new List<TarotCards.Card>();
    foreach (TarotCards.Card playerFoundTrinket in DataManager.Instance.PlayerFoundTrinkets)
    {
      if (!TarotCards.IsCurseRelatedTarotCard(playerFoundTrinket) || DataManager.Instance.EnabledSpells)
      {
        bool flag = false;
        foreach (TarotCards.TarotCard playerRunTrinket in DataManager.Instance.PlayerRunTrinkets)
        {
          if (playerRunTrinket.CardType == playerFoundTrinket)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          unusedFoundTrinkets.Add(playerFoundTrinket);
      }
    }
    return unusedFoundTrinkets;
  }

  private static bool IsCurseRelatedTarotCard(TarotCards.Card card)
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

  public static void ApplyInstantEffects(TarotCards.TarotCard card)
  {
    switch (card.CardType)
    {
      case TarotCards.Card.Hearts1:
      case TarotCards.Card.Hearts2:
      case TarotCards.Card.Hearts3:
        ((HealthPlayer) PlayerFarming.Instance.health).TotalSpiritHearts += TarotCards.GetSpiritHeartCount(card);
        BiomeConstants.Instance.EmitHeartPickUpVFX(PlayerFarming.Instance.CameraBone.transform.position, 0.0f, "red", "burst_big");
        AudioManager.Instance.PlayOneShot("event:/player/collect_blue_heart", PlayerFarming.Instance.transform.position);
        break;
      case TarotCards.Card.Lovers1:
        PlayerFarming.Instance.GetComponent<HealthPlayer>().BlueHearts += 2f;
        Vector3 position1 = PlayerFarming.Instance.CameraBone.transform.position;
        BiomeConstants.Instance.EmitHeartPickUpVFX(position1, 0.0f, "blue", "burst_big");
        AudioManager.Instance.PlayOneShot("event:/player/collect_blue_heart", position1);
        break;
      case TarotCards.Card.Lovers2:
        PlayerFarming.Instance.GetComponent<HealthPlayer>().BlueHearts += 4f;
        Vector3 position2 = PlayerFarming.Instance.CameraBone.transform.position;
        BiomeConstants.Instance.EmitHeartPickUpVFX(position2, 0.0f, "blue", "burst_big");
        AudioManager.Instance.PlayOneShot("event:/player/collect_blue_heart", position2);
        break;
      case TarotCards.Card.DiseasedHeart:
        PlayerFarming.Instance.GetComponent<HealthPlayer>().BlackHearts += 2f;
        Vector3 position3 = PlayerFarming.Instance.CameraBone.transform.position;
        BiomeConstants.Instance.EmitHeartPickUpVFX(position3, 0.0f, "black", "burst_big");
        AudioManager.Instance.PlayOneShot("event:/player/collect_blue_heart", position3);
        break;
      case TarotCards.Card.TheDeal:
        ResurrectOnHud.ResurrectionType = ResurrectionType.DealTarot;
        break;
      case TarotCards.Card.IncreasedDamage:
        AudioManager.Instance.PlayOneShot("event:/hearts_of_the_faithful/swords_appear", PlayerFarming.Instance.transform.position);
        BiomeConstants.Instance.EmitHeartPickUpVFX(PlayerFarming.Instance.CameraBone.transform.position, 0.0f, "strength", "strength");
        break;
    }
    FaithAmmo.Ammo = FaithAmmo.Ammo;
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
      default:
        return 0.0f;
    }
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

  public static float GetMovementSpeedMultiplier(TarotCards.TarotCard card)
  {
    if (card.CardType != TarotCards.Card.MovementSpeed)
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

  public static float GetAttackRateMultiplier(TarotCards.TarotCard card)
  {
    if (card.CardType != TarotCards.Card.AttackRate)
      return 0.0f;
    switch (card.UpgradeIndex)
    {
      case 1:
        return 0.5f;
      case 2:
        return 2f;
      default:
        return 0.25f;
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
    if (card.CardType != TarotCards.Card.NegateDamageChance)
      return 0.0f;
    return card.UpgradeIndex == 1 ? 0.2f : 0.1f;
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
    Count,
  }

  public class TarotCard
  {
    public TarotCards.Card CardType;
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
