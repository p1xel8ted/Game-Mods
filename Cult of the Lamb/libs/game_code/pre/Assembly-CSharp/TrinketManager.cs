// Decompiled with JetBrains decompiler
// Type: TrinketManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class TrinketManager
{
  public static Dictionary<TarotCards.Card, float> _cardCooldowns = new Dictionary<TarotCards.Card, float>();

  public static event TrinketManager.TrinketsUpdated OnTrinketsCleared;

  public static event TrinketManager.TrinketUpdated OnTrinketAdded;

  public static event TrinketManager.TrinketUpdated OnTrinketRemoved;

  public static event TrinketManager.TrinketUpdated OnTrinketCooldownStart;

  public static event TrinketManager.TrinketUpdated OnTrinketCooldownEnd;

  public static void AddTrinket(TarotCards.TarotCard card)
  {
    PlayerFleeceManager.OnTarotCardPickedUp();
    if (!DataManager.Instance.PlayerRunTrinkets.Contains(card))
      DataManager.Instance.PlayerRunTrinkets.Add(card);
    TarotCards.ApplyInstantEffects(card);
    TrinketManager.TrinketUpdated onTrinketAdded = TrinketManager.OnTrinketAdded;
    if (onTrinketAdded == null)
      return;
    onTrinketAdded(card.CardType);
  }

  public static void RemoveTrinket(TarotCards.Card card)
  {
    for (int index = DataManager.Instance.PlayerRunTrinkets.Count - 1; index >= 0; --index)
    {
      if (DataManager.Instance.PlayerRunTrinkets[index].CardType == card)
        DataManager.Instance.PlayerRunTrinkets.RemoveAt(index);
    }
    TrinketManager.TrinketUpdated onTrinketRemoved = TrinketManager.OnTrinketRemoved;
    if (onTrinketRemoved == null)
      return;
    onTrinketRemoved(card);
  }

  public static TarotCards.TarotCard RemoveRandomTrinket()
  {
    TarotCards.TarotCard playerRunTrinket = DataManager.Instance.PlayerRunTrinkets[Random.Range(0, DataManager.Instance.PlayerRunTrinkets.Count)];
    DataManager.Instance.PlayerRunTrinkets.Remove(playerRunTrinket);
    TrinketManager.TrinketUpdated onTrinketRemoved = TrinketManager.OnTrinketRemoved;
    if (onTrinketRemoved != null)
      onTrinketRemoved(playerRunTrinket.CardType);
    return playerRunTrinket;
  }

  public static void RemoveAllTrinkets()
  {
    DataManager.Instance.PlayerRunTrinkets.Clear();
    TrinketManager.TrinketsUpdated onTrinketsCleared = TrinketManager.OnTrinketsCleared;
    if (onTrinketsCleared == null)
      return;
    onTrinketsCleared();
  }

  public static bool HasTrinket(TarotCards.Card card)
  {
    for (int index = 0; index < DataManager.Instance.PlayerRunTrinkets.Count; ++index)
    {
      if (DataManager.Instance.PlayerRunTrinkets[index].CardType == card)
        return true;
    }
    return false;
  }

  public static void UpdateCooldowns(float deltaTime)
  {
    foreach (TarotCards.Card card in new List<TarotCards.Card>((IEnumerable<TarotCards.Card>) TrinketManager._cardCooldowns.Keys))
    {
      if ((double) TrinketManager._cardCooldowns[card] > 0.0)
      {
        TrinketManager._cardCooldowns[card] = Mathf.Max(TrinketManager._cardCooldowns[card] - deltaTime, 0.0f);
        if ((double) TrinketManager._cardCooldowns[card] <= 0.0)
        {
          TrinketManager.TrinketUpdated trinketCooldownEnd = TrinketManager.OnTrinketCooldownEnd;
          if (trinketCooldownEnd != null)
            trinketCooldownEnd(card);
        }
      }
    }
  }

  public static void TriggerCooldown(TarotCards.Card card)
  {
    TrinketManager._cardCooldowns[card] = TrinketManager.GetCardMaxCooldownSeconds(card);
    TrinketManager.TrinketUpdated trinketCooldownStart = TrinketManager.OnTrinketCooldownStart;
    if (trinketCooldownStart == null)
      return;
    trinketCooldownStart(card);
  }

  public static bool IsOnCooldown(TarotCards.Card card)
  {
    return (double) TrinketManager.GetRemainingCooldownSeconds(card) > 0.0;
  }

  public static float GetRemainingCooldownSeconds(TarotCards.Card card)
  {
    float remainingCooldownSeconds = 0.0f;
    TrinketManager._cardCooldowns.TryGetValue(card, out remainingCooldownSeconds);
    return remainingCooldownSeconds;
  }

  public static float GetRemainingCooldownPercent(TarotCards.Card card)
  {
    float remainingCooldownSeconds = TrinketManager.GetRemainingCooldownSeconds(card);
    float maxCooldownSeconds = TrinketManager.GetCardMaxCooldownSeconds(card);
    return (double) maxCooldownSeconds != 0.0 ? remainingCooldownSeconds / maxCooldownSeconds : 0.0f;
  }

  public static int GetSpiritAmmo()
  {
    int spiritAmmo = 0;
    foreach (TarotCards.TarotCard playerRunTrinket in DataManager.Instance.PlayerRunTrinkets)
      spiritAmmo += TarotCards.GetSpiritAmmoCount(playerRunTrinket);
    return spiritAmmo;
  }

  public static float GetWeaponDamageMultiplerIncrease()
  {
    float multiplerIncrease = 0.0f;
    foreach (TarotCards.TarotCard playerRunTrinket in DataManager.Instance.PlayerRunTrinkets)
      multiplerIncrease += TarotCards.GetWeaponDamageMultiplerIncrease(playerRunTrinket);
    return multiplerIncrease;
  }

  public static float GetCurseDamageMultiplerIncrease()
  {
    float multiplerIncrease = 0.0f;
    foreach (TarotCards.TarotCard playerRunTrinket in DataManager.Instance.PlayerRunTrinkets)
      multiplerIncrease += TarotCards.GetCurseDamageMultiplerIncrease(playerRunTrinket);
    return multiplerIncrease;
  }

  public static float GetWeaponCritChanceIncrease()
  {
    float critChanceIncrease = 0.0f;
    foreach (TarotCards.TarotCard playerRunTrinket in DataManager.Instance.PlayerRunTrinkets)
      critChanceIncrease += TarotCards.GetWeaponCritChanceIncrease(playerRunTrinket);
    return critChanceIncrease;
  }

  public static int GetLootIncreaseModifier(InventoryItem.ITEM_TYPE itemType)
  {
    int increaseModifier = 0;
    foreach (TarotCards.TarotCard playerRunTrinket in DataManager.Instance.PlayerRunTrinkets)
      increaseModifier += TarotCards.GetLootIncreaseModifier(playerRunTrinket, itemType);
    return increaseModifier;
  }

  private static float GetCardMaxCooldownSeconds(TarotCards.Card card)
  {
    TarotCards.TarotCard tarotCard = (TarotCards.TarotCard) null;
    for (int index = 0; index < DataManager.Instance.PlayerRunTrinkets.Count; ++index)
    {
      if (DataManager.Instance.PlayerRunTrinkets[index].CardType == card)
      {
        tarotCard = DataManager.Instance.PlayerRunTrinkets[index];
        break;
      }
    }
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
        return tarotCard.UpgradeIndex == 1 ? 1.5f : 2.5f;
      default:
        return 0.0f;
    }
  }

  public static int GetDamageAllEnemiesAmount(TarotCards.Card card)
  {
    int allEnemiesAmount = 0;
    foreach (TarotCards.TarotCard playerRunTrinket in DataManager.Instance.PlayerRunTrinkets)
      allEnemiesAmount += TarotCards.GetDamageAllEnemiesAmount(playerRunTrinket);
    return allEnemiesAmount;
  }

  public static float GetAttackRateMultiplier()
  {
    float attackRateMultiplier = 0.0f;
    foreach (TarotCards.TarotCard playerRunTrinket in DataManager.Instance.PlayerRunTrinkets)
      attackRateMultiplier += TarotCards.GetAttackRateMultiplier(playerRunTrinket);
    return attackRateMultiplier;
  }

  public static float GetMovementSpeedMultiplier()
  {
    float movementSpeedMultiplier = 1f;
    foreach (TarotCards.TarotCard playerRunTrinket in DataManager.Instance.PlayerRunTrinkets)
      movementSpeedMultiplier += TarotCards.GetMovementSpeedMultiplier(playerRunTrinket);
    return movementSpeedMultiplier;
  }

  public static float GetBlackSoulsMultiplier()
  {
    float blackSoulsMultiplier = 1f;
    foreach (TarotCards.TarotCard playerRunTrinket in DataManager.Instance.PlayerRunTrinkets)
      blackSoulsMultiplier += TarotCards.GetBlackSoulsMultiplier(playerRunTrinket);
    return blackSoulsMultiplier;
  }

  public static float GetHealChance()
  {
    float healChance = 0.0f;
    foreach (TarotCards.TarotCard playerRunTrinket in DataManager.Instance.PlayerRunTrinkets)
      healChance += TarotCards.GetHealChance(playerRunTrinket);
    return healChance;
  }

  public static float GetNegateDamageChance()
  {
    float negateDamageChance = 0.0f;
    foreach (TarotCards.TarotCard playerRunTrinket in DataManager.Instance.PlayerRunTrinkets)
      negateDamageChance += TarotCards.GetNegateDamageChance(playerRunTrinket);
    return negateDamageChance;
  }

  public static int GetHealthAmountMultiplier()
  {
    int amountMultiplier = 1;
    foreach (TarotCards.TarotCard playerRunTrinket in DataManager.Instance.PlayerRunTrinkets)
      amountMultiplier += TarotCards.GetHealthAmountMultiplier(playerRunTrinket);
    return amountMultiplier;
  }

  public static float GetAmmoEfficiencyMultiplier()
  {
    float efficiencyMultiplier = 1f;
    foreach (TarotCards.TarotCard playerRunTrinket in DataManager.Instance.PlayerRunTrinkets)
      efficiencyMultiplier += TarotCards.GetAmmoEfficiency(playerRunTrinket);
    return efficiencyMultiplier;
  }

  public static int GetBlackSoulsOnDamaged()
  {
    int blackSoulsOnDamaged = 0;
    foreach (TarotCards.TarotCard playerRunTrinket in DataManager.Instance.PlayerRunTrinkets)
      blackSoulsOnDamaged += TarotCards.GetBlackSoulsOnDamage(playerRunTrinket);
    return blackSoulsOnDamaged;
  }

  public static InventoryItem[] GetItemsToDrop()
  {
    List<InventoryItem> inventoryItemList = new List<InventoryItem>();
    foreach (TarotCards.TarotCard playerRunTrinket in DataManager.Instance.PlayerRunTrinkets)
    {
      InventoryItem itemToDrop = TarotCards.GetItemToDrop(playerRunTrinket);
      if (itemToDrop != null)
        inventoryItemList.Add(itemToDrop);
    }
    return inventoryItemList.ToArray();
  }

  public static float GetChanceOfGainingBlueHeart()
  {
    float gainingBlueHeart = 0.0f;
    foreach (TarotCards.TarotCard playerRunTrinket in DataManager.Instance.PlayerRunTrinkets)
      gainingBlueHeart += TarotCards.GetChanceOfGainingBlueHeart(playerRunTrinket);
    return gainingBlueHeart;
  }

  public static bool DropBombOnRoll()
  {
    for (int index = 0; index < DataManager.Instance.PlayerRunTrinkets.Count; ++index)
    {
      if (DataManager.Instance.PlayerRunTrinkets[index].CardType == TarotCards.Card.BombOnRoll)
        return true;
    }
    return false;
  }

  public static bool DropBlackGoopOnRoll()
  {
    for (int index = 0; index < DataManager.Instance.PlayerRunTrinkets.Count; ++index)
    {
      if (DataManager.Instance.PlayerRunTrinkets[index].CardType == TarotCards.Card.GoopOnRoll)
        return true;
    }
    return false;
  }

  public static bool DropBlackGoopOnDamaged()
  {
    for (int index = 0; index < DataManager.Instance.PlayerRunTrinkets.Count; ++index)
    {
      if (DataManager.Instance.PlayerRunTrinkets[index].CardType == TarotCards.Card.GoopOnDamaged)
        return true;
    }
    return false;
  }

  public static bool IsPoisonImmune()
  {
    for (int index = 0; index < DataManager.Instance.PlayerRunTrinkets.Count; ++index)
    {
      if (DataManager.Instance.PlayerRunTrinkets[index].CardType == TarotCards.Card.PoisonImmune)
        return true;
    }
    return false;
  }

  public static bool DamageEnemyOnRoll()
  {
    for (int index = 0; index < DataManager.Instance.PlayerRunTrinkets.Count; ++index)
    {
      if (DataManager.Instance.PlayerRunTrinkets[index].CardType == TarotCards.Card.DamageOnRoll)
        return true;
    }
    return false;
  }

  public static bool CanNegateDamage()
  {
    foreach (TarotCards.TarotCard playerRunTrinket in DataManager.Instance.PlayerRunTrinkets)
    {
      if (playerRunTrinket.CardType == TarotCards.Card.InvincibleWhileHealing)
        return true;
    }
    return false;
  }

  public delegate void TrinketsUpdated();

  public delegate void TrinketUpdated(TarotCards.Card trinket);
}
