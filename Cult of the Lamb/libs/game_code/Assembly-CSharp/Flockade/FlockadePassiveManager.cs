// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadePassiveManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Flockade;

public class FlockadePassiveManager
{
  public Dictionary<FlockadeGameBoardSide, int> _choiceAmountBonus = new Dictionary<FlockadeGameBoardSide, int>();
  public Dictionary<FlockadeGameBoardSide, float> _pointsWonInDuelMultiplier = new Dictionary<FlockadeGameBoardSide, float>();
  public bool _isOrderOfDuelsAndBlessingsApplicationReversed;
  public Dictionary<FlockadeGameBoardSide, List<Func<Sequence>>> _choiceAmountBonusQueried = new Dictionary<FlockadeGameBoardSide, List<Func<Sequence>>>();
  public Dictionary<FlockadeGameBoardSide, List<Func<Sequence>>> _pointsWonInDuelMultiplierQueried = new Dictionary<FlockadeGameBoardSide, List<Func<Sequence>>>();
  public List<Func<Sequence>> _orderOfDuelsAndBlessingsApplicationQueried = new List<Func<Sequence>>();

  public void SubscribeToChoiceAmountBonusQueries(
    FlockadeGameBoardSide side,
    Func<Sequence> sequence)
  {
    if (this._choiceAmountBonusQueried.TryAdd(side, new List<Func<Sequence>>()
    {
      sequence
    }))
      return;
    this._choiceAmountBonusQueried[side].Add(sequence);
  }

  public void UnsubscribeFromChoiceAmountBonusQueries(
    FlockadeGameBoardSide side,
    Func<Sequence> sequence)
  {
    List<Func<Sequence>> funcList;
    if (!this._choiceAmountBonusQueried.TryGetValue(side, out funcList))
      return;
    funcList.Remove(sequence);
  }

  public void SubscribeToPointsWonInDuelMultiplierQueries(
    FlockadeGameBoardSide side,
    Func<Sequence> sequence)
  {
    if (this._pointsWonInDuelMultiplierQueried.TryAdd(side, new List<Func<Sequence>>()
    {
      sequence
    }))
      return;
    this._pointsWonInDuelMultiplierQueried[side].Add(sequence);
  }

  public void UnsubscribeFromPointsWonInDuelMultiplierQueries(
    FlockadeGameBoardSide side,
    Func<Sequence> sequence)
  {
    List<Func<Sequence>> funcList;
    if (!this._pointsWonInDuelMultiplierQueried.TryGetValue(side, out funcList))
      return;
    funcList.Remove(sequence);
  }

  public void SubscribeToOrderOfDuelsAndBlessingsApplicationQueries(Func<Sequence> sequence)
  {
    this._orderOfDuelsAndBlessingsApplicationQueried.Add(sequence);
  }

  public void UnsubscribeFromOrderOfDuelsAndBlessingsApplicationQueries(Func<Sequence> sequence)
  {
    this._orderOfDuelsAndBlessingsApplicationQueried.Remove(sequence);
  }

  public int GetChoiceAmountBonus(FlockadeGameBoardSide side)
  {
    return CollectionExtensions.GetValueOrDefault<FlockadeGameBoardSide, int>((IReadOnlyDictionary<FlockadeGameBoardSide, int>) this._choiceAmountBonus, side);
  }

  public Sequence GetChoiceAmountBonus(FlockadeGameBoardSide side, out int result)
  {
    result = this.GetChoiceAmountBonus(side);
    List<Func<Sequence>> source;
    return !this._choiceAmountBonusQueried.TryGetValue(side, out source) || !source.Any<Func<Sequence>>() ? (Sequence) null : FlockadeUtils.Combine((IEnumerable<Tween>) source.Select<Func<Sequence>, Sequence>((Func<Func<Sequence>, Sequence>) (sequence => sequence == null ? (Sequence) null : sequence())).ToArray<Sequence>());
  }

  public float GetPointsWonInDuelMultiplier(FlockadeGameBoardSide side)
  {
    return CollectionExtensions.GetValueOrDefault<FlockadeGameBoardSide, float>((IReadOnlyDictionary<FlockadeGameBoardSide, float>) this._pointsWonInDuelMultiplier, side, 1f);
  }

  public Sequence GetPointsWonInDuelMultiplier(FlockadeGameBoardSide side, out float result)
  {
    result = this.GetPointsWonInDuelMultiplier(side);
    List<Func<Sequence>> source;
    return !this._pointsWonInDuelMultiplierQueried.TryGetValue(side, out source) || !source.Any<Func<Sequence>>() ? (Sequence) null : FlockadeUtils.Combine((IEnumerable<Tween>) source.Select<Func<Sequence>, Sequence>((Func<Func<Sequence>, Sequence>) (sequence => sequence == null ? (Sequence) null : sequence())).ToArray<Sequence>());
  }

  public bool IsOrderOfDuelsAndBlessingsApplicationReversed()
  {
    return this._isOrderOfDuelsAndBlessingsApplicationReversed;
  }

  public Sequence IsOrderOfDuelsAndBlessingsApplicationReversed(out bool result)
  {
    result = this.IsOrderOfDuelsAndBlessingsApplicationReversed();
    return !this._orderOfDuelsAndBlessingsApplicationQueried.Any<Func<Sequence>>() ? (Sequence) null : FlockadeUtils.Combine((IEnumerable<Tween>) this._orderOfDuelsAndBlessingsApplicationQueried.Select<Func<Sequence>, Sequence>((Func<Func<Sequence>, Sequence>) (sequence => sequence == null ? (Sequence) null : sequence())).ToArray<Sequence>());
  }

  public void ApplyBonusToChoicesAmount(FlockadeGameBoardSide side, int bonus)
  {
    this._choiceAmountBonus.TryAdd(side, 0);
    this._choiceAmountBonus[side] += bonus;
  }

  public void ApplyMultiplierToPointsWonInDuel(FlockadeGameBoardSide side, float multiplier)
  {
    this._pointsWonInDuelMultiplier.TryAdd(side, 1f);
    this._pointsWonInDuelMultiplier[side] *= multiplier;
  }

  public void ReverseCurrentOrderOfDuelsAndBlessingsApplication()
  {
    this._isOrderOfDuelsAndBlessingsApplicationReversed = !this._isOrderOfDuelsAndBlessingsApplicationReversed;
  }

  public FlockadePassiveManager Clone()
  {
    FlockadePassiveManager flockadePassiveManager = new FlockadePassiveManager();
    foreach (KeyValuePair<FlockadeGameBoardSide, int> choiceAmountBonu in this._choiceAmountBonus)
      flockadePassiveManager._choiceAmountBonus.Add(choiceAmountBonu.Key, choiceAmountBonu.Value);
    foreach (KeyValuePair<FlockadeGameBoardSide, float> keyValuePair in this._pointsWonInDuelMultiplier)
      flockadePassiveManager._pointsWonInDuelMultiplier.Add(keyValuePair.Key, keyValuePair.Value);
    flockadePassiveManager._isOrderOfDuelsAndBlessingsApplicationReversed = this._isOrderOfDuelsAndBlessingsApplicationReversed;
    return flockadePassiveManager;
  }
}
