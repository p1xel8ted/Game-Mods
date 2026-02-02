// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadePointCounter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

#nullable disable
namespace Flockade;

[RequireComponent(typeof (RectTransform))]
public class FlockadePointCounter : MonoBehaviour, IFlockadeCounter
{
  public const string _LEFT_SIDE_POINT_SCORED_SOUND = "event:/dlc/ui/flockade_minigame/point_gain_player_tick";
  public const string _RIGHT_SIDE_POINT_SCORED_SOUND = "event:/dlc/ui/flockade_minigame/point_gain_opponent_tick";
  public const float _BETWEEN_POINTS_APPEARANCE_DISAPPEARANCE_DELAY = 0.0333333351f;
  [SerializeField]
  public ParentConstraint _animationSlot;
  [SerializeField]
  public FlockadePointCounter.MarkerGroup[] _markerGroups;
  public List<(int value, FlockadePointCounter.EnumerationStrategy enumerationStrategy, FlockadeAnimation customAnimationOverCurrentGroupPrefab, (bool X, bool Y) customAnimationOverCurrentGroupFlip)> _setCountWhileFrozen = new List<(int, FlockadePointCounter.EnumerationStrategy, FlockadeAnimation, (bool, bool))>();
  public List<(int skip, int amount)> _setNextAsBonusWhileFrozen = new List<(int, int)>();
  public int _countBeforeFreeze;
  public bool _frozen;
  public LayoutGroup[] _layoutGroups;
  public RectTransform _rectTransform;
  public FlockadeGameBoardSide _side;
  public int _count;

  public int Count
  {
    get => this._count;
    set
    {
      this.SetCount(value, new FlockadePointCounter.EnumerationStrategy(FlockadePointCounter.EnumerateNormally), customAnimationOverCurrentGroupFlip: ());
    }
  }

  public virtual void Awake()
  {
    this._layoutGroups = this.GetComponentsInChildren<LayoutGroup>();
    this._rectTransform = this.GetComponent<RectTransform>();
  }

  public void Configure(FlockadeGameBoardSide side) => this._side = side;

  public void Freeze()
  {
    if (this._frozen)
      return;
    this._countBeforeFreeze = this._count;
    this._frozen = true;
  }

  public void Unfreeze()
  {
    if (!this._frozen)
      return;
    this._count = this._countBeforeFreeze;
    this._frozen = false;
    foreach ((int skip, int amount) tuple in this._setNextAsBonusWhileFrozen)
      this.SetNextAsBonus(tuple.skip, tuple.amount);
    this._setNextAsBonusWhileFrozen.Clear();
    foreach ((int value, FlockadePointCounter.EnumerationStrategy enumerationStrategy, FlockadeAnimation customAnimationOverCurrentGroupPrefab, (bool X, bool Y) customAnimationOverCurrentGroupFlip) tuple in this._setCountWhileFrozen)
      this.SetCount(tuple.value, tuple.enumerationStrategy, tuple.customAnimationOverCurrentGroupPrefab, tuple.customAnimationOverCurrentGroupFlip);
    this._setCountWhileFrozen.Clear();
  }

  public void SetNextAsBonus(int skip = 0, int amount = 0)
  {
    if (this._frozen)
    {
      this._setNextAsBonusWhileFrozen.Add((skip, amount));
    }
    else
    {
      foreach (FlockadePointCounter.MarkerGroup markerGroup in this._markerGroups)
      {
        if (amount <= 0)
          break;
        if (!((IEnumerable<FlockadePointMarker>) markerGroup.Markers).Any<FlockadePointMarker>((Func<FlockadePointMarker, bool>) (marker => marker.Active)))
        {
          if (skip > 0)
          {
            --skip;
          }
          else
          {
            foreach (FlockadePointMarker marker in markerGroup.Markers)
              marker.Bonus = true;
            --amount;
          }
        }
      }
    }
  }

  public void Wipe()
  {
    this.SetCount(0, new FlockadePointCounter.EnumerationStrategy(FlockadePointCounter.EnumerateFromLeftToRight), customAnimationOverCurrentGroupFlip: ());
  }

  public static void EnumerateNormally(
    int previous,
    int current,
    FlockadePointCounter.ForEachAction action)
  {
    if (current == previous)
      return;
    bool flag = current < previous;
    int num1 = flag ? Mathf.Max(previous - 1, 0) : previous;
    int num2 = flag ? current : Mathf.Max(current - 1, 0);
    for (int index = num1; (flag ? (index >= num2 ? 1 : 0) : (index <= num2 ? 1 : 0)) != 0; index += flag ? -1 : 1)
      action(index, index == num2);
  }

  public static void EnumerateFromLeftToRight(
    int previous,
    int current,
    FlockadePointCounter.ForEachAction action)
  {
    if (current == previous)
      return;
    int num1 = current < previous ? 1 : 0;
    int num2 = num1 != 0 ? current : previous;
    int num3 = num1 != 0 ? Mathf.Max(previous - 1, 0) : Mathf.Max(current - 1, 0);
    bool flag = FlockadeUtils.NextMultipleOf(5, num3 + 1) - 1 == num3;
    int num4 = -1;
    for (int index1 = num2; index1 <= num3; ++index1)
    {
      if (index1 != num4)
      {
        int index2 = FlockadeUtils.NextMultipleOf(5, index1 + 1) - 1;
        if (index2 <= num3 && index2 != num4)
        {
          num4 = index2;
          action(index2, false);
        }
        action(index1, index1 == (flag ? num3 - 1 : num3));
      }
    }
  }

  public void PlacePointStrikes()
  {
    foreach (FlockadePointCounter.MarkerGroup markerGroup in this._markerGroups)
    {
      foreach (Component marker in markerGroup.Markers)
      {
        FlockadePointStrike component;
        if (marker.TryGetComponent<FlockadePointStrike>(out component))
          component.Place();
      }
    }
  }

  public void SetCount(
    int value,
    FlockadeAnimation customAnimationOverCurrentGroupPrefab,
    (bool X, bool Y) customAnimationOverCurrentGroupFlip = default ((bool X, bool Y)))
  {
    this.SetCount(value, new FlockadePointCounter.EnumerationStrategy(FlockadePointCounter.EnumerateNormally), customAnimationOverCurrentGroupPrefab, customAnimationOverCurrentGroupFlip);
  }

  public void SetCount(
    int value,
    FlockadePointCounter.EnumerationStrategy enumerationStrategy,
    FlockadeAnimation customAnimationOverCurrentGroupPrefab = null,
    (bool X, bool Y) customAnimationOverCurrentGroupFlip = default ((bool X, bool Y)))
  {
    int previous = this._count;
    this._count = Mathf.Clamp(value, 0, this._markerGroups.Length);
    if (this._frozen)
    {
      this._setCountWhileFrozen.Add((value, enumerationStrategy, customAnimationOverCurrentGroupPrefab, customAnimationOverCurrentGroupFlip));
    }
    else
    {
      float delay = 0.0f;
      if ((bool) (UnityEngine.Object) customAnimationOverCurrentGroupPrefab)
      {
        for (int index = 0; index < this._animationSlot.sourceCount; ++index)
        {
          ConstraintSource source = this._animationSlot.GetSource(index) with
          {
            weight = index >= previous || index / 5 != Mathf.Max((previous - 1) / 5, 0) ? 0.0f : 1f
          };
          this._animationSlot.SetSource(index, source);
        }
        FlockadeAnimation flockadeAnimation = UnityEngine.Object.Instantiate<FlockadeAnimation>(customAnimationOverCurrentGroupPrefab, this._animationSlot.transform);
        flockadeAnimation.Flip = (this._side == FlockadeGameBoardSide.Left ? customAnimationOverCurrentGroupFlip.X : !customAnimationOverCurrentGroupFlip.X, customAnimationOverCurrentGroupFlip.Y);
        flockadeAnimation.Play();
      }
      this.SetLayoutGroupsEnabled(false);
      enumerationStrategy(previous, this._count, (FlockadePointCounter.ForEachAction) ((index, isLast) =>
      {
        foreach (FlockadePointMarker marker1 in this._markerGroups[index].Markers)
        {
          FlockadePointMarker marker = marker1;
          bool scoring = this._count >= previous;
          marker.SetActive(scoring, isLast ? (System.Action) (() =>
          {
            this.SetLayoutGroupsEnabled(true);
            LayoutRebuilder.ForceRebuildLayoutImmediate(this._rectTransform);
            this.PlacePointStrikes();
          }) : (System.Action) null).PrependCallback((TweenCallback) (() =>
          {
            if (!scoring || marker.Bonus)
              return;
            AudioManager.Instance.PlayOneShot(this._side == FlockadeGameBoardSide.Left ? "event:/dlc/ui/flockade_minigame/point_gain_player_tick" : "event:/dlc/ui/flockade_minigame/point_gain_opponent_tick");
          })).PrependInterval(delay);
          delay += 0.0333333351f;
        }
      }));
    }
  }

  public void SetLayoutGroupsEnabled(bool value)
  {
    foreach (Behaviour layoutGroup in this._layoutGroups)
      layoutGroup.enabled = value;
  }

  [Serializable]
  public class MarkerGroup
  {
    public FlockadePointMarker[] Markers;
  }

  public delegate void ForEachAction(int index, bool isLast);

  public delegate void EnumerationStrategy(
    int previous,
    int current,
    FlockadePointCounter.ForEachAction action);
}
