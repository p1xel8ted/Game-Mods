// Decompiled with JetBrains decompiler
// Type: TrinketManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class TrinketManager
{
  public static List<TarotCards.TarotCard> AllTrinkets = new List<TarotCards.TarotCard>();

  public static event TrinketManager.TrinketsUpdated OnTrinketsCleared;

  public static event TrinketManager.TrinketUpdated OnTrinketAdded;

  public static event TrinketManager.TrinketUpdated OnTrinketRemoved;

  public static event TrinketManager.TrinketUpdated OnTrinketCooldownStart;

  public static event TrinketManager.TrinketUpdated OnTrinketCooldownEnd;

  public static void AddTrinket(TarotCards.TarotCard card, PlayerFarming playerFarming)
  {
    if ((Object) playerFarming == (Object) null)
      playerFarming = PlayerFarming.Instance;
    if ((Object) playerFarming == (Object) null || TrinketManager.AllTrinkets == null)
      return;
    PlayerFleeceManager.OnTarotCardPickedUp(playerFarming);
    if (!TrinketManager.AllTrinkets.Contains(card))
      TrinketManager.AllTrinkets.Add(card);
    if (!DataManager.Instance.PlayerRunTrinketsContains(card, playerFarming))
      DataManager.Instance.PlayerRunTrinketsAdd(card, playerFarming);
    TarotCards.ApplyInstantEffects(card, playerFarming);
    TrinketManager.TrinketUpdated onTrinketAdded = TrinketManager.OnTrinketAdded;
    if (onTrinketAdded == null)
      return;
    onTrinketAdded(card.CardType, playerFarming);
  }

  public static void RemoveTrinket(TarotCards.Card card, PlayerFarming playerFarming)
  {
    for (int index = TrinketManager.AllTrinkets.Count - 1; index >= 0; --index)
    {
      if (TrinketManager.AllTrinkets[index].CardType == card)
        TrinketManager.AllTrinkets.RemoveAt(index);
    }
    for (int index = playerFarming.RunTrinkets.Count - 1; index >= 0; --index)
    {
      if (playerFarming.RunTrinkets[index].CardType == card)
        playerFarming.RunTrinkets.RemoveAt(index);
    }
    TrinketManager.TrinketUpdated onTrinketRemoved = TrinketManager.OnTrinketRemoved;
    if (onTrinketRemoved == null)
      return;
    onTrinketRemoved(card, playerFarming);
  }

  public static TarotCards.TarotCard RemoveRandomTrinket(PlayerFarming playerFarming)
  {
    TarotCards.TarotCard runTrinket = playerFarming.RunTrinkets[Random.Range(0, playerFarming.RunTrinkets.Count)];
    DataManager.Instance.PlayerTrinketsRemove(runTrinket, playerFarming);
    TrinketManager.TrinketUpdated onTrinketRemoved = TrinketManager.OnTrinketRemoved;
    if (onTrinketRemoved != null)
      onTrinketRemoved(runTrinket.CardType, playerFarming);
    return runTrinket;
  }

  public static void RemoveAllTrinkets()
  {
    for (int index = 0; index < PlayerFarming.playersCount; ++index)
    {
      PlayerFarming.players[index].RunTrinkets.Clear();
      TrinketManager.TrinketsUpdated onTrinketsCleared = TrinketManager.OnTrinketsCleared;
      if (onTrinketsCleared != null)
        onTrinketsCleared(PlayerFarming.players[index]);
    }
  }

  public static int CountTrinket(TarotCards.Card card)
  {
    int num = 0;
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      for (int index = 0; index < player.RunTrinkets.Count; ++index)
      {
        if (player.RunTrinkets[index].CardType == card)
          ++num;
      }
    }
    return num;
  }

  public static bool HasTrinket(TarotCards.Card card)
  {
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      for (int index = 0; index < player.RunTrinkets.Count; ++index)
      {
        if (player.RunTrinkets[index].CardType == card)
          return true;
      }
    }
    return false;
  }

  public static bool HasTrinket(TarotCards.Card card, PlayerFarming playerFarming)
  {
    if ((Object) playerFarming == (Object) null)
      return TrinketManager.HasTrinket(card);
    for (int index = 0; index < playerFarming.RunTrinkets.Count; ++index)
    {
      TarotCards.TarotCard runTrinket = playerFarming.RunTrinkets[index];
      if ((runTrinket != null ? (runTrinket.CardType == card ? 1 : 0) : 0) != 0)
        return true;
    }
    return false;
  }

  public static void AddEncounteredTrinket(TarotCards.TarotCard card, PlayerFarming playerFarming)
  {
    if (DataManager.Instance.PlayerFoundTrinketsContains(card, playerFarming))
      return;
    DataManager.Instance.PlayerFoundTrinketsAdd(card, playerFarming);
  }

  public static bool HasEncounteredTrinket(TarotCards.Card card)
  {
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      for (int index = 0; index < player.FoundTrinkets.Count; ++index)
      {
        if (player.FoundTrinkets[index].CardType == card)
          return true;
      }
    }
    return false;
  }

  public static bool HasEncounteredTrinket(TarotCards.Card card, PlayerFarming playerFarming)
  {
    if ((Object) playerFarming == (Object) null)
      return TrinketManager.HasEncounteredTrinket(card);
    for (int index = 0; index < playerFarming.FoundTrinkets.Count; ++index)
    {
      if (playerFarming.FoundTrinkets[index].CardType == card)
        return true;
    }
    return false;
  }

  public static void RemoveAllEncounteredTrinkets()
  {
    for (int index = 0; index < PlayerFarming.playersCount; ++index)
    {
      PlayerFarming.players[index].FoundTrinkets.Clear();
      TrinketManager.TrinketsUpdated onTrinketsCleared = TrinketManager.OnTrinketsCleared;
      if (onTrinketsCleared != null)
        onTrinketsCleared(PlayerFarming.players[index]);
    }
  }

  public static void UpdateCooldowns(float deltaTime)
  {
    for (int index1 = 0; index1 < PlayerFarming.players.Count; ++index1)
    {
      PlayerFarming player = PlayerFarming.players[index1];
      foreach (TarotCards.Card card in new List<TarotCards.Card>((IEnumerable<TarotCards.Card>) player.CardCooldowns.Keys))
      {
        if ((double) player.CardCooldowns[card] > 0.0)
        {
          player.CardCooldowns[card] = Mathf.Max(player.CardCooldowns[card] - deltaTime, 0.0f);
          if ((double) player.CardCooldowns[card] <= 0.0)
          {
            for (int index2 = 0; index2 < PlayerFarming.playersCount; ++index2)
            {
              TrinketManager.TrinketUpdated trinketCooldownEnd = TrinketManager.OnTrinketCooldownEnd;
              if (trinketCooldownEnd != null)
                trinketCooldownEnd(card, PlayerFarming.players[index2]);
            }
          }
        }
      }
    }
  }

  public static void TriggerCooldown(TarotCards.Card card, PlayerFarming playerFarming)
  {
    playerFarming.CardCooldowns[card] = TrinketManager.GetCardMaxCooldownSeconds(card, playerFarming);
    TrinketManager.TrinketUpdated trinketCooldownStart = TrinketManager.OnTrinketCooldownStart;
    if (trinketCooldownStart == null)
      return;
    trinketCooldownStart(card, playerFarming);
  }

  public static void TriggerForAllCooldown(TarotCards.Card card)
  {
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      player.CardCooldowns[card] = TrinketManager.GetCardMaxCooldownSeconds(card, player);
      TrinketManager.TrinketUpdated trinketCooldownStart = TrinketManager.OnTrinketCooldownStart;
      if (trinketCooldownStart != null)
        trinketCooldownStart(card, player);
    }
  }

  public static bool IsOnCooldown(TarotCards.Card card, PlayerFarming playerFarming)
  {
    return (double) TrinketManager.GetRemainingCooldownSeconds(card, playerFarming) > 0.0;
  }

  public static float GetRemainingCooldownSeconds(TarotCards.Card card, PlayerFarming playerFarming)
  {
    float remainingCooldownSeconds = 0.0f;
    if ((Object) playerFarming != (Object) null && playerFarming.CardCooldowns != null)
      playerFarming.CardCooldowns.TryGetValue(card, out remainingCooldownSeconds);
    return remainingCooldownSeconds;
  }

  public static float GetRemainingCooldownPercent(TarotCards.Card card, PlayerFarming playerFarming)
  {
    float remainingCooldownSeconds = TrinketManager.GetRemainingCooldownSeconds(card, playerFarming);
    float maxCooldownSeconds = TrinketManager.GetCardMaxCooldownSeconds(card, playerFarming);
    return (double) maxCooldownSeconds != 0.0 ? remainingCooldownSeconds / maxCooldownSeconds : 0.0f;
  }

  public static int GetSpiritAmmo(PlayerFarming playerFarming)
  {
    int spiritAmmo = 0;
    foreach (TarotCards.TarotCard runTrinket in playerFarming.RunTrinkets)
      spiritAmmo += TarotCards.GetSpiritAmmoCount(runTrinket);
    return spiritAmmo;
  }

  public static float GetWeaponDamageMultiplerIncrease(PlayerFarming playerFarming)
  {
    float multiplerIncrease = 0.0f;
    foreach (TarotCards.TarotCard runTrinket in playerFarming.RunTrinkets)
    {
      if (runTrinket.CardType != TarotCards.Card.CorruptedFullCorruption || !TrinketManager.IsCorruptedPositiveEffectNegated(runTrinket.CardType, playerFarming))
        multiplerIncrease += TarotCards.GetWeaponDamageMultiplerIncrease(runTrinket);
    }
    return multiplerIncrease;
  }

  public static float GetCurseDamageMultiplerIncrease(PlayerFarming playerFarming)
  {
    float multiplerIncrease = 0.0f;
    foreach (TarotCards.TarotCard runTrinket in playerFarming.RunTrinkets)
      multiplerIncrease += TarotCards.GetCurseDamageMultiplerIncrease(runTrinket);
    return multiplerIncrease;
  }

  public static float GetWeaponCritChanceIncrease(PlayerFarming playerFarming)
  {
    float critChanceIncrease = 0.0f;
    foreach (TarotCards.TarotCard runTrinket in playerFarming.RunTrinkets)
      critChanceIncrease += TarotCards.GetWeaponCritChanceIncrease(runTrinket);
    return critChanceIncrease;
  }

  public static int GetLootIncreaseModifier(
    InventoryItem.ITEM_TYPE itemType,
    PlayerFarming playerFarming)
  {
    int increaseModifier = 0;
    foreach (TarotCards.TarotCard runTrinket in playerFarming.RunTrinkets)
      increaseModifier += TarotCards.GetLootIncreaseModifier(runTrinket, itemType);
    return increaseModifier;
  }

  public static float GetCardMaxCooldownSeconds(TarotCards.Card card, PlayerFarming playerFarming)
  {
    if ((Object) playerFarming == (Object) null || playerFarming.RunTrinkets == null)
      return 0.0f;
    TarotCards.TarotCard tarotCard = (TarotCards.TarotCard) null;
    for (int index = 0; index < playerFarming.RunTrinkets.Count; ++index)
    {
      if (playerFarming.RunTrinkets[index].CardType == card)
      {
        tarotCard = playerFarming.RunTrinkets[index];
        break;
      }
    }
    if (tarotCard == null)
      return float.MaxValue;
    switch (card)
    {
      case TarotCards.Card.TheDeal:
        return float.MaxValue;
      case TarotCards.Card.HandsOfRage:
        return tarotCard.UpgradeIndex == 1 ? 5f : 10f;
      case TarotCards.Card.BombOnRoll:
        return tarotCard.UpgradeIndex == 1 ? 5f : 10f;
      case TarotCards.Card.GoopOnRoll:
        return tarotCard.UpgradeIndex == 1 ? 5f : 10f;
      case TarotCards.Card.BlackSoulAutoRecharge:
        float maxCooldownSeconds;
        switch (tarotCard.UpgradeIndex)
        {
          case 1:
            maxCooldownSeconds = 1.5f;
            break;
          case 2:
            maxCooldownSeconds = 0.2f;
            break;
          default:
            maxCooldownSeconds = 2.5f;
            break;
        }
        if (DataManager.Instance.PlayerFleece == 6)
          maxCooldownSeconds /= 4f;
        return maxCooldownSeconds;
      case TarotCards.Card.CorruptedGoopyTrail:
        return 0.25f;
      default:
        return 0.0f;
    }
  }

  public static int GetIceHeartsToHealOnRevive(PlayerFarming playerFarming)
  {
    for (int index = 0; index < playerFarming.RunTrinkets.Count; ++index)
    {
      if (playerFarming.RunTrinkets[index].CardType == TarotCards.Card.LastChance)
      {
        switch (playerFarming.RunTrinkets[index].UpgradeIndex)
        {
          case 1:
            return 4;
          case 2:
            return 6;
          default:
            return 2;
        }
      }
    }
    return 0;
  }

  public static int GetDamageAllEnemiesAmount(TarotCards.Card card, PlayerFarming playerFarming)
  {
    int allEnemiesAmount = 0;
    foreach (TarotCards.TarotCard runTrinket in playerFarming.RunTrinkets)
      allEnemiesAmount += TarotCards.GetDamageAllEnemiesAmount(runTrinket);
    return allEnemiesAmount;
  }

  public static int GetEnemyDamageMultiplier(PlayerFarming playerFarming)
  {
    int damageMultiplier = 1;
    if ((Object) playerFarming == (Object) null || playerFarming.RunTrinkets == null)
      return damageMultiplier;
    foreach (TarotCards.TarotCard runTrinket in playerFarming.RunTrinkets)
      damageMultiplier += TarotCards.GetEnemyDamageMultiplier(runTrinket);
    return damageMultiplier;
  }

  public static int GetEnemyPoisonDamageMultiplier(PlayerFarming playerFarming)
  {
    int damageMultiplier = 1;
    if ((Object) playerFarming == (Object) null || playerFarming.RunTrinkets == null)
      return damageMultiplier;
    foreach (TarotCards.TarotCard runTrinket in playerFarming.RunTrinkets)
      damageMultiplier += TarotCards.GetEnemyPoisonDamageMultiplier(runTrinket);
    return damageMultiplier;
  }

  public static int GetDamageTradeOff(PlayerFarming playerFarming)
  {
    if ((Object) playerFarming == (Object) null || playerFarming.RunTrinkets == null)
      return 0;
    for (int index = 0; index < playerFarming.RunTrinkets.Count; ++index)
    {
      if (playerFarming.RunTrinkets[index].CardType == TarotCards.Card.CorruptedTradeOff)
        return TarotCards.GetDamageTradeOff(playerFarming.RunTrinkets[index]);
    }
    return 0;
  }

  public static float GetAttackRateMultiplier(PlayerFarming playerFarming)
  {
    float attackRateMultiplier = 0.0f;
    foreach (TarotCards.TarotCard runTrinket in playerFarming.RunTrinkets)
    {
      if (!TrinketManager.IsCorruptedNegativeEffectNegated(TarotCards.Card.CorruptedGoopyTrail, playerFarming) || runTrinket.CardType != TarotCards.Card.CorruptedGoopyTrail)
        attackRateMultiplier += TarotCards.GetAttackRateMultiplier(runTrinket);
    }
    return attackRateMultiplier;
  }

  public static float GetMovementSpeedMultiplier(PlayerFarming playerFarming)
  {
    float movementSpeedMultiplier = 1f;
    foreach (TarotCards.TarotCard runTrinket in playerFarming.RunTrinkets)
      movementSpeedMultiplier += TarotCards.GetMovementSpeedMultiplier(runTrinket, playerFarming);
    return movementSpeedMultiplier;
  }

  public static float GetBlackSoulsMultiplier(PlayerFarming playerFarming)
  {
    float blackSoulsMultiplier = 1f;
    foreach (TarotCards.TarotCard runTrinket in playerFarming.RunTrinkets)
      blackSoulsMultiplier += TarotCards.GetBlackSoulsMultiplier(runTrinket);
    return blackSoulsMultiplier;
  }

  public static float GetSinChance(PlayerFarming playerFarming)
  {
    float sinChance = 0.0f;
    foreach (TarotCards.TarotCard runTrinket in playerFarming.RunTrinkets)
      sinChance += TarotCards.GetSinChance(runTrinket);
    return sinChance;
  }

  public static float GetHealChance(PlayerFarming playerFarming)
  {
    float healChance = 0.0f;
    foreach (TarotCards.TarotCard runTrinket in playerFarming.RunTrinkets)
      healChance += TarotCards.GetHealChance(runTrinket);
    return healChance;
  }

  public static float GetNegateDamageChance(PlayerFarming playerFarming)
  {
    float negateDamageChance = 0.0f;
    foreach (TarotCards.TarotCard runTrinket in playerFarming.RunTrinkets)
      negateDamageChance += TarotCards.GetNegateDamageChance(runTrinket);
    return negateDamageChance;
  }

  public static int GetHealthAmountMultiplier(PlayerFarming playerFarming)
  {
    int amountMultiplier = 1;
    foreach (TarotCards.TarotCard runTrinket in playerFarming.RunTrinkets)
      amountMultiplier += TarotCards.GetHealthAmountMultiplier(runTrinket);
    return amountMultiplier;
  }

  public static float GetAmmoEfficiencyMultiplier(PlayerFarming playerFarming)
  {
    float efficiencyMultiplier = 1f;
    foreach (TarotCards.TarotCard runTrinket in playerFarming.RunTrinkets)
      efficiencyMultiplier += TarotCards.GetAmmoEfficiency(runTrinket);
    return efficiencyMultiplier;
  }

  public static int GetBlackSoulsOnDamaged(PlayerFarming playerFarming)
  {
    int blackSoulsOnDamaged = 0;
    foreach (TarotCards.TarotCard runTrinket in playerFarming.RunTrinkets)
      blackSoulsOnDamaged += TarotCards.GetBlackSoulsOnDamage(runTrinket);
    return blackSoulsOnDamaged;
  }

  public static InventoryItem[] GetItemsToDrop(PlayerFarming playerFarming)
  {
    List<InventoryItem> inventoryItemList = new List<InventoryItem>();
    foreach (TarotCards.TarotCard runTrinket in playerFarming.RunTrinkets)
    {
      InventoryItem itemToDrop = TarotCards.GetItemToDrop(runTrinket);
      if (itemToDrop != null)
        inventoryItemList.Add(itemToDrop);
    }
    return inventoryItemList.ToArray();
  }

  public static float GetChanceOfGainingBlueHeart(PlayerFarming playerFarming)
  {
    float gainingBlueHeart = 0.0f;
    foreach (TarotCards.TarotCard runTrinket in playerFarming.RunTrinkets)
      gainingBlueHeart += TarotCards.GetChanceOfGainingBlueHeart(runTrinket);
    return gainingBlueHeart;
  }

  public static float GetChanceForRelicsMultiplier(PlayerFarming playerFarming)
  {
    float relicsMultiplier = 1f;
    foreach (TarotCards.TarotCard runTrinket in playerFarming.RunTrinkets)
      relicsMultiplier += TarotCards.GetChanceForRelicsMultiplier(runTrinket);
    return relicsMultiplier;
  }

  public static float GetRelicChargeMultiplier(PlayerFarming playerFarming)
  {
    float chargeMultiplier = 1f;
    foreach (TarotCards.TarotCard runTrinket in playerFarming.RunTrinkets)
      chargeMultiplier += TarotCards.GetRelicChargeMultiplier(runTrinket);
    return chargeMultiplier;
  }

  public static float GetRelicDamageMultiplier(PlayerFarming playerFarming)
  {
    float damageMultiplier = 1f;
    foreach (TarotCards.TarotCard runTrinket in playerFarming.RunTrinkets)
    {
      if (runTrinket.CardType != TarotCards.Card.CorruptedRelicCharge || !TrinketManager.IsCorruptedPositiveEffectNegated(TarotCards.Card.CorruptedRelicCharge, playerFarming))
        damageMultiplier += TarotCards.GetRelicDamageMultiplier(runTrinket);
    }
    return damageMultiplier;
  }

  public static float GetIceCurseDamageMultiplier(PlayerFarming playerFarming)
  {
    float damageMultiplier = 0.0f;
    foreach (TarotCards.TarotCard runTrinket in playerFarming.RunTrinkets)
      damageMultiplier += TarotCards.GetIceCurseDamageMultiplier(runTrinket);
    return damageMultiplier;
  }

  public static float GetInvincibilityTimeEnteringNewRoom(PlayerFarming playerFarming)
  {
    float timeEnteringNewRoom = 0.0f;
    foreach (TarotCards.TarotCard runTrinket in playerFarming.RunTrinkets)
      timeEnteringNewRoom += TarotCards.GetInvincibilityTimeEnteringNewRoom(runTrinket);
    return timeEnteringNewRoom;
  }

  public static float GetHeavyAttackCostMultiplier(PlayerFarming playerFarming)
  {
    float attackCostMultiplier = 1f;
    foreach (TarotCards.TarotCard runTrinket in playerFarming.RunTrinkets)
    {
      if (runTrinket.CardType == TarotCards.Card.CorruptedHeavy && !TrinketManager.IsCorruptedPositiveEffectNegated(TarotCards.Card.CorruptedHeavy, playerFarming))
        attackCostMultiplier *= TarotCards.GetHeavyAttackCostMultiplier(runTrinket);
    }
    return attackCostMultiplier;
  }

  public static float GetLightningChanceOnKill(PlayerFarming playerFarming)
  {
    float lightningChanceOnKill = 0.0f;
    foreach (TarotCards.TarotCard runTrinket in playerFarming.RunTrinkets)
      lightningChanceOnKill += TarotCards.GetLightningChanceOnKill(runTrinket);
    return lightningChanceOnKill;
  }

  public static float GetCoinsDropMultiplier(PlayerFarming playerFarming)
  {
    float coinsDropMultiplier = 1f;
    foreach (TarotCards.TarotCard runTrinket in playerFarming.RunTrinkets)
    {
      if (runTrinket.CardType != TarotCards.Card.CorruptedPoisonCoins || !TrinketManager.IsCorruptedPositiveEffectNegated(TarotCards.Card.CorruptedPoisonCoins, playerFarming))
        coinsDropMultiplier += TarotCards.GetCoinsDropMultiplier(runTrinket);
    }
    return coinsDropMultiplier;
  }

  public static float GetDodgeDistanceMultiplier(PlayerFarming playerFarming)
  {
    float distanceMultiplier = 1f;
    foreach (TarotCards.TarotCard runTrinket in playerFarming.RunTrinkets)
      distanceMultiplier += TarotCards.GetDodgeDistanceMultiplier(runTrinket);
    return distanceMultiplier;
  }

  public static int GetAmountOfCorruptedBombs(PlayerFarming playerFarming) => Random.Range(1, 4);

  public static bool DropBombOnRoll(PlayerFarming playerFarming)
  {
    for (int index = 0; index < playerFarming.RunTrinkets.Count; ++index)
    {
      if (playerFarming.RunTrinkets[index].CardType == TarotCards.Card.BombOnRoll)
        return true;
    }
    return false;
  }

  public static bool DropBlackGoopOnRoll(PlayerFarming playerFarming)
  {
    for (int index = 0; index < playerFarming.RunTrinkets.Count; ++index)
    {
      if (playerFarming.RunTrinkets[index].CardType == TarotCards.Card.GoopOnRoll)
        return true;
    }
    return false;
  }

  public static float ChanceToDropWoolOnHit(PlayerFarming playerFarming)
  {
    float num = 0.1f;
    for (int index = 0; index < playerFarming.RunTrinkets.Count; ++index)
    {
      if (playerFarming.RunTrinkets[index].CardType == TarotCards.Card.EasyMoney)
        return num * (float) (playerFarming.RunTrinkets[index].UpgradeIndex + 1);
    }
    return 0.0f;
  }

  public static bool DropBlackGoopOnDamaged(PlayerFarming playerFarming)
  {
    for (int index = 0; index < playerFarming.RunTrinkets.Count; ++index)
    {
      if (playerFarming.RunTrinkets[index].CardType == TarotCards.Card.GoopOnDamaged)
        return true;
    }
    return false;
  }

  public static bool DropBombOnDamaged(PlayerFarming playerFarming)
  {
    for (int index = 0; index < playerFarming.RunTrinkets.Count; ++index)
    {
      if (playerFarming.RunTrinkets[index].CardType == TarotCards.Card.BombOnDamaged)
        return true;
    }
    return false;
  }

  public static bool DropTentacleOnDamaged(PlayerFarming playerFarming)
  {
    for (int index = 0; index < playerFarming.RunTrinkets.Count; ++index)
    {
      if (playerFarming.RunTrinkets[index].CardType == TarotCards.Card.TentacleOnDamaged)
        return true;
    }
    return false;
  }

  public static bool StrikeLightningOnDamaged(PlayerFarming playerFarming)
  {
    for (int index = 0; index < playerFarming.RunTrinkets.Count; ++index)
    {
      if (playerFarming.RunTrinkets[index].CardType == TarotCards.Card.StrikeBack)
        return true;
    }
    return false;
  }

  public static bool IsPoisonImmune(PlayerFarming playerFarming)
  {
    for (int index = 0; index < playerFarming.RunTrinkets.Count; ++index)
    {
      if (playerFarming.RunTrinkets[index].CardType == TarotCards.Card.PoisonImmune)
        return true;
    }
    return false;
  }

  public static bool IsBurnImmune(PlayerFarming playerFarming) => false;

  public static bool IsIceImmune(PlayerFarming playerFarming) => false;

  public static bool DamageEnemyOnRoll(PlayerFarming playerFarming)
  {
    for (int index = 0; index < playerFarming.RunTrinkets.Count; ++index)
    {
      if (playerFarming.RunTrinkets[index].CardType == TarotCards.Card.DamageOnRoll)
        return true;
    }
    return false;
  }

  public static bool CanNegateDamage(PlayerFarming playerFarming)
  {
    foreach (TarotCards.TarotCard runTrinket in playerFarming.RunTrinkets)
    {
      if (runTrinket.CardType == TarotCards.Card.InvincibleWhileHealing)
        return true;
    }
    return false;
  }

  public static bool AreRelicsFragile(PlayerFarming playerFarming)
  {
    return TrinketManager.HasTrinket(TarotCards.Card.CorruptedBlackHeartForRelic, playerFarming) && !TrinketManager.IsCorruptedNegativeEffectNegated(TarotCards.Card.CorruptedBlackHeartForRelic, playerFarming);
  }

  public static bool IsCorruptedNegativeEffectNegated(
    TarotCards.Card card,
    PlayerFarming playerFarming)
  {
    foreach (TarotCards.TarotCard tarotCard in playerFarming.CorruptedTrinketsOnlyPositive)
    {
      if (tarotCard.CardType == card)
        return true;
    }
    return TrinketManager.HasTrinket(TarotCards.Card.NoCorruption, playerFarming);
  }

  public static bool IsCorruptedPositiveEffectNegated(
    TarotCards.Card card,
    PlayerFarming playerFarming)
  {
    foreach (TarotCards.TarotCard tarotCard in playerFarming.CorruptedTrinketsOnlyPositive)
    {
      if (tarotCard.CardType == TarotCards.Card.CorruptedFullCorruption)
        return false;
    }
    foreach (TarotCards.TarotCard tarotCard in playerFarming.CorruptedTrinketsOnlyNegative)
    {
      if (tarotCard.CardType == card || tarotCard.CardType == TarotCards.Card.CorruptedFullCorruption)
        return true;
    }
    return card != TarotCards.Card.CorruptedFullCorruption && TrinketManager.HasTrinket(TarotCards.Card.CorruptedFullCorruption, playerFarming) && !TrinketManager.HasTrinket(TarotCards.Card.NoCorruption, playerFarming);
  }

  public delegate void TrinketsUpdated(PlayerFarming playerFarming);

  public delegate void TrinketUpdated(TarotCards.Card trinket, PlayerFarming playerFarming);
}
