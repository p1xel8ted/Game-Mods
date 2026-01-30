// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeGameBoard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using src.UINavigator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Flockade;

[RequireComponent(typeof (CanvasGroup), typeof (RectTransform))]
public class FlockadeGameBoard : MonoBehaviour, IFlockadeGameBoard
{
  public const string _EQUAL_SIDES_WARNING = "Each side must contain the same amount of tiles!";
  public const string _SAME_GRID_LAYOUTS_WARNING = "Each grid layout must be the same!";
  public const float _DELAY_AFTER_TILES_DUELLING_START_TO_FIGHT = 0.05f;
  public const float _DELAY_AFTER_INDICATORS_STATE_CHANGED = 1f;
  public const float _RELATIVE_TIME_TO_START_DUELLING_TILES_FROM_END_OF_GAME_PIECE_EXIT = -0.05f;
  public const float _SHAKE_DURATION = 0.65f;
  public const string _DUEL_START_SOUND = "event:/dlc/ui/flockade_minigame/battle_duel_start";
  public const string _ROUND_START_SOUND = "event:/dlc/ui/flockade_minigame/round_start_board";
  public Coroutine _fastForward;
  [CompilerGenerated]
  public bool \u003CIsFastForwarding\u003Ek__BackingField;
  [SerializeField]
  public FlockadeFight _fight;
  [SerializeField]
  public FlockadeGameBoardIndicators _indicators;
  [SerializeField]
  public GridLayoutGroup _leftGrid;
  [SerializeField]
  public GridLayoutGroup _rightGrid;
  [SerializeField]
  public FlockadeGameBoardTile[] _leftSide;
  [SerializeField]
  public FlockadeGameBoardTile[] _rightSide;
  public bool _areGridLayoutsInvalid;
  public bool _areSidesInvalid;
  public CanvasGroup _canvasGroup;
  public FlockadeControlPrompts _controlPrompts;
  public RectTransform _gridRectTransform;
  public Vector2 _originAnchoredPosition;
  public RectTransform _rectTransform;
  [CompilerGenerated]
  public FlockadeVirtualGameBoard \u003CCore\u003Ek__BackingField;
  [CompilerGenerated]
  public FlockadeGamePieceInformation \u003CInformation\u003Ek__BackingField;
  public List<int> _duelOrder = new List<int>();
  public int _duelProgress;
  public HashSet<int> _resolvedPairs = new HashSet<int>();
  public bool _previewEnabled;
  public Dictionary<int, int> _pairDisplayNumber = new Dictionary<int, int>();

  public bool IsFastForwarding
  {
    get => this.\u003CIsFastForwarding\u003Ek__BackingField;
    set => this.\u003CIsFastForwarding\u003Ek__BackingField = value;
  }

  public event Action<bool> FastForwarding;

  public FlockadeVirtualGameBoard Core
  {
    get => this.\u003CCore\u003Ek__BackingField;
    set => this.\u003CCore\u003Ek__BackingField = value;
  }

  public FlockadeGameBoardIndicators Indicators => this._indicators;

  public FlockadeGamePieceInformation Information
  {
    get => this.\u003CInformation\u003Ek__BackingField;
    set => this.\u003CInformation\u003Ek__BackingField = value;
  }

  public void Configure(
    FlockadePlayerBase leftPlayer,
    FlockadePlayerBase rightPlayer,
    FlockadeControlPrompts controlPrompts,
    FlockadeGamePieceInformation information,
    FlockadePassiveManager passiveManager)
  {
    this.Information = information;
    for (int index = 0; index < this._leftSide.Length; ++index)
      this._leftSide[index].Configure(this, FlockadeGameBoardSide.Left, index);
    for (int index = 0; index < this._rightSide.Length; ++index)
      this._rightSide[index].Configure(this, FlockadeGameBoardSide.Right, index);
    int num;
    switch (this._leftGrid.constraint)
    {
      case GridLayoutGroup.Constraint.FixedColumnCount:
        num = Mathf.CeilToInt((float) this._leftSide.Length / (float) this._leftGrid.constraintCount);
        break;
      case GridLayoutGroup.Constraint.FixedRowCount:
        num = Mathf.Min(this._leftGrid.constraintCount, this._leftSide.Length);
        break;
      default:
        throw new Exception("Getting the column of tile on grid layout with flexible constraint is not supported!");
    }
    int rowCount = num;
    this._controlPrompts = controlPrompts;
    this.Core = new FlockadeVirtualGameBoard(passiveManager, rowCount, (IFlockadePlayer) leftPlayer, (IFlockadePlayer) rightPlayer, Enumerable.Cast<IFlockadeGameBoardTile>(this._leftSide).ToArray<IFlockadeGameBoardTile>(), Enumerable.Cast<IFlockadeGameBoardTile>(this._rightSide).ToArray<IFlockadeGameBoardTile>(), (IFlockadeGameBoard) this);
  }

  public virtual void Awake()
  {
    this._canvasGroup = this.GetComponent<CanvasGroup>();
    this._gridRectTransform = this._leftGrid.GetComponent<RectTransform>();
    this._rectTransform = this.GetComponent<RectTransform>();
    this._originAnchoredPosition = this._rectTransform.anchoredPosition;
    this.Exit();
  }

  public void OnEnable() => this._fight.OnHit += new FlockadeFight.FlockadeFightEvent(this.Shake);

  public void OnDisable() => this._fight.OnHit -= new FlockadeFight.FlockadeFightEvent(this.Shake);

  public DG.Tweening.Sequence Enter()
  {
    return DOTween.Sequence().Append((Tween) this._canvasGroup.DOFade(1f, 0.8333333f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutCubic)).Join((Tween) this._rectTransform.DOAnchorPosY(this._originAnchoredPosition.y, 0.8333333f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.OutCubic));
  }

  public void Exit()
  {
    this._canvasGroup.alpha = 0.0f;
    this._rectTransform.anchoredPosition = this._originAnchoredPosition + new Vector2(0.0f, this._rectTransform.rect.height / 2f);
  }

  public static void Evaluate(FlockadeGameBoardTile tile)
  {
    FlockadeGameBoard.Evaluate(tile, tile.GameBoard.GetOpposingTile(tile));
  }

  public static void Evaluate(FlockadeGameBoardTile leftTile, FlockadeGameBoardTile rightTile)
  {
    leftTile.UnsetWinningStates();
    rightTile.UnsetWinningStates();
    switch (leftTile.GamePiece.Fight((IFlockadeGamePiece) rightTile.GamePiece))
    {
      case FlockadeFight.Result.Unknown:
      case FlockadeFight.Result.Tie:
        leftTile.GamePiece.Blessing.OnEvaluated((IFlockadeGameBoardTile) leftTile, (IFlockadeGameBoardTile) rightTile);
        rightTile.GamePiece.Blessing.OnEvaluated((IFlockadeGameBoardTile) rightTile, (IFlockadeGameBoardTile) leftTile);
        break;
      case FlockadeFight.Result.Defeat:
        rightTile.SetWinningPoint();
        goto case FlockadeFight.Result.Unknown;
      case FlockadeFight.Result.Win:
        leftTile.SetWinningPoint();
        goto case FlockadeFight.Result.Unknown;
      default:
        throw new ArgumentOutOfRangeException();
    }
  }

  IEnumerator IFlockadeGameBoard.StartDuelPhase()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    FlockadeGameBoard flockadeGameBoard = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    flockadeGameBoard._previewEnabled = false;
    flockadeGameBoard.AllowFastForward();
    // ISSUE: reference to a compiler-generated field
    // ISSUE: explicit non-virtual call
    this.\u003C\u003E2__current = (object) __nonvirtual (flockadeGameBoard.Core).StartDuelPhase(new Func<FlockadeVirtualGameBoard.BeforeApplyingBlessingsContext, IEnumerator>(flockadeGameBoard.\u003CFlockade\u002EIFlockadeGameBoard\u002EStartDuelPhase\u003Eg__BeforeApplyingBlessings\u007C49_0), new Func<FlockadeVirtualGameBoard.OnApplyingBlessingContext, IEnumerator>(flockadeGameBoard.\u003CFlockade\u002EIFlockadeGameBoard\u002EStartDuelPhase\u003Eg__OnApplyingBlessing\u007C49_1));
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  IEnumerator IFlockadeGameBoard.Resolve()
  {
    FlockadePointCounter leftPlayerPoints = this.GetPlayer(FlockadeGameBoardSide.Left).Points;
    FlockadePointCounter rightPlayerPoints = this.GetPlayer(FlockadeGameBoardSide.Right).Points;
    int leftPlayerPointsBeforeResolution = leftPlayerPoints.Count;
    int rightPlayerPointsBeforeResolution = rightPlayerPoints.Count;
    leftPlayerPoints.Freeze();
    rightPlayerPoints.Freeze();
    IEnumerator coroutine;
    yield return (object) (coroutine = this.Core.Resolve((Action<FlockadeVirtualGameBoard.BeforeDuelDefaultScoringContext>) (context =>
    {
      if (context.Points <= 1)
        return;
      this.GetPlayer(context.Side).Points.SetNextAsBonus(1, context.Points - 1);
    }), (Func<FlockadeVirtualGameBoard.OnDuelResolvedContext, IEnumerator>) (context =>
    {
      ++this._duelProgress;
      this._resolvedPairs.Add(context.Index);
      this.PaintDuelNumbersStable();
      this.HighlightPairCombat(context.Index, true);
      DG.Tweening.Sequence[] array = ((IEnumerable<IFlockadeBlessing.OnResolvedResult>) context.OnResolvedBlessings).Select<IFlockadeBlessing.OnResolvedResult, DG.Tweening.Sequence>((Func<IFlockadeBlessing.OnResolvedResult, DG.Tweening.Sequence>) (onResolvedBlessing => onResolvedBlessing.Sequence.Pause<DG.Tweening.Sequence>())).ToArray<DG.Tweening.Sequence>();
      foreach (DG.Tweening.Sequence t in context.OnScoringBlessings ?? Array.Empty<DG.Tweening.Sequence>())
        t.Pause<DG.Tweening.Sequence>();
      DG.Tweening.Sequence sequence = DOTween.Sequence();
      DG.Tweening.Sequence t1 = this.Indicators.SetEnabledLine(context.Index / this.RowCount > 0 ? FlockadeGameBoardIndicators.Line.Back : FlockadeGameBoardIndicators.Line.Front);
      if (t1 != null)
      {
        sequence.Append((Tween) t1);
        sequence.AppendInterval(1f);
      }
      sequence.Append((Tween) FlockadeUtils.Combine((IEnumerable<Tween>) array)).Append((Tween) this.AnimateFight(context.Index, context.Resolution, DOTween.Sequence().Append((Tween) FlockadeUtils.Combine((IEnumerable<Tween>) context.OnScoringBlessings)).PrependCallback((TweenCallback) (() => this.FadePairNumbers(context.Index, 0.35f, 0.5f))).AppendCallback((TweenCallback) (() =>
      {
        leftPlayerPoints.Unfreeze();
        rightPlayerPoints.Unfreeze();
        leftPlayerPoints.Freeze();
        rightPlayerPoints.Freeze();
        this.AnimatePlayers(leftPlayerPoints.Count - leftPlayerPointsBeforeResolution, rightPlayerPoints.Count - rightPlayerPointsBeforeResolution);
        leftPlayerPointsBeforeResolution = leftPlayerPoints.Count;
        rightPlayerPointsBeforeResolution = rightPlayerPoints.Count;
      })))).OnComplete<DG.Tweening.Sequence>((TweenCallback) (() => this.HighlightPairCombat(context.Index, false)));
      return (IEnumerator) sequence.WaitForCompletion(true);
    }), (Func<IEnumerator>) (() =>
    {
      foreach (FlockadeGameBoardTile tile in this.GetTiles())
        tile.SetActivationOrderBadge(new int?());
      this._duelOrder.Clear();
      this._duelProgress = 0;
      this._pairDisplayNumber.Clear();
      this._resolvedPairs.Clear();
      foreach (FlockadeGameBoardTile tile in this.GetTiles())
        tile.SetTargeted(false);
      this.DisallowFastForward();
      DG.Tweening.Sequence s = DOTween.Sequence();
      DG.Tweening.Sequence t = this.Indicators.SetEnabledLine(FlockadeGameBoardIndicators.Line.None);
      if (t != null)
      {
        s.Append((Tween) t);
        s.AppendInterval(1f);
      }
      return (IEnumerator) s.Join((Tween) this.CompleteRound()).WaitForCompletion(true);
    })));
    leftPlayerPoints.Unfreeze();
    rightPlayerPoints.Unfreeze();
    yield return coroutine.Current;
  }

  public void PaintTentativeDuelOrder()
  {
    if (!this._previewEnabled)
      return;
    int tileCount = this.GetTileCount(FlockadeGameBoardSide.Left);
    for (int index = 0; index < tileCount; ++index)
    {
      int num = index + 1;
      this._leftSide[index].SetActivationOrderBadge(new int?(num));
      this._rightSide[index].SetActivationOrderBadge(new int?(num));
    }
  }

  public void HighlightPair(int pairIndex, bool on)
  {
    this._leftSide[pairIndex].SetTargeted(on);
    this._rightSide[pairIndex].SetTargeted(on);
  }

  public void HighlightPairCombat(int pairIndex, bool on)
  {
    this._leftSide[pairIndex].SetEffectHighlighted(on);
    this._rightSide[pairIndex].SetEffectHighlighted(on);
  }

  public void FadePairNumbers(int pairIndex, float toAlpha, float duration)
  {
    this._leftSide[pairIndex].FadeActivationOrderBadge(toAlpha, duration);
    this._rightSide[pairIndex].FadeActivationOrderBadge(toAlpha, duration);
  }

  public bool IsResolved(int pairIndex) => this._resolvedPairs.Contains(pairIndex);

  public void PaintDuelNumbersStable()
  {
    foreach (KeyValuePair<int, int> keyValuePair in this._pairDisplayNumber)
    {
      int key = keyValuePair.Key;
      int num = keyValuePair.Value;
      this._leftSide[key].SetActivationOrderBadge(new int?(num), this.IsResolved(key));
      this._rightSide[key].SetActivationOrderBadge(new int?(num), this.IsResolved(key));
    }
  }

  public DG.Tweening.Sequence StartRound()
  {
    DG.Tweening.Sequence s = DOTween.Sequence().AppendCallback((TweenCallback) (() => AudioManager.Instance.PlayOneShot("event:/dlc/ui/flockade_minigame/round_start_board")));
    this._previewEnabled = true;
    foreach ((List<FlockadeGameBoardTile> flockadeGameBoardTileList, int num) in this.EnumerateInWave().Select<List<FlockadeGameBoardTile>, (List<FlockadeGameBoardTile>, int)>((Func<List<FlockadeGameBoardTile>, int, (List<FlockadeGameBoardTile>, int)>) ((tiles, index) => (tiles, index))))
    {
      foreach (FlockadeGameBoardTile flockadeGameBoardTile in flockadeGameBoardTileList)
      {
        FlockadeGameBoardTile tile = flockadeGameBoardTile;
        float atPosition = (float) num * 0.05f;
        s.Insert((float) num * 0.05f, (Tween) tile.SetDuelled(false));
        s.InsertCallback(atPosition, (TweenCallback) (() =>
        {
          if (!this._previewEnabled)
            return;
          tile.SetActivationOrderBadge(new int?(tile.Index + 1));
        }));
      }
    }
    return s;
  }

  public DG.Tweening.Sequence CompleteRound()
  {
    DG.Tweening.Sequence s = DOTween.Sequence();
    foreach ((List<FlockadeGameBoardTile> flockadeGameBoardTileList, int num) in this.EnumerateInWave().Select<List<FlockadeGameBoardTile>, (List<FlockadeGameBoardTile>, int)>((Func<List<FlockadeGameBoardTile>, int, (List<FlockadeGameBoardTile>, int)>) ((tiles, index) => (tiles, index))))
    {
      foreach (FlockadeGameBoardTile flockadeGameBoardTile in flockadeGameBoardTileList)
      {
        DG.Tweening.Sequence sequence = flockadeGameBoardTile.Exit(true);
        s.Insert((float) num * 0.05f, (Tween) sequence.Insert(sequence.Duration() - 0.05f, (Tween) flockadeGameBoardTile.SetDuelled(true)));
      }
    }
    return s;
  }

  public IEnumerable<List<FlockadeGameBoardTile>> EnumerateInWave()
  {
    int rows = this.RowCount;
    int columns = this.GetTileCount(FlockadeGameBoardSide.Left) / rows;
    int waves = 2 * columns + rows - 1;
    for (int wave = 0; wave < waves; ++wave)
    {
      List<FlockadeGameBoardTile> flockadeGameBoardTileList = new List<FlockadeGameBoardTile>();
      for (int index1 = 0; index1 < rows; ++index1)
      {
        for (int index2 = 0; index2 < columns; ++index2)
        {
          if (index1 + index2 == wave)
          {
            int index3 = (columns - index2 - 1) * rows + index1;
            flockadeGameBoardTileList.Add(this.GetTile(FlockadeGameBoardSide.Left, index3));
          }
        }
      }
      for (int index4 = 0; index4 < rows; ++index4)
      {
        for (int index5 = 0; index5 < columns; ++index5)
        {
          if (index4 + columns + index5 == wave)
          {
            int index6 = index5 * rows + index4;
            flockadeGameBoardTileList.Add(this.GetTile(FlockadeGameBoardSide.Right, index6));
          }
        }
      }
      yield return flockadeGameBoardTileList;
    }
  }

  public IEnumerator Wipe()
  {
    DG.Tweening.Sequence[] array = this.GetTiles().Select<FlockadeGameBoardTile, DG.Tweening.Sequence>((Func<FlockadeGameBoardTile, DG.Tweening.Sequence>) (tile => tile.Wipe().Sequence)).ToArray<DG.Tweening.Sequence>();
    foreach (FlockadePlayerBase player in this.GetPlayers())
      player.Points.Wipe();
    yield return (object) FlockadeUtils.WaitForCompletion((IEnumerable<Tween>) array);
  }

  public DG.Tweening.Sequence AnimateFight(
    int index,
    FlockadeFight.Result resolution,
    DG.Tweening.Sequence onClimax = null)
  {
    FlockadeGameBoardTile flockadeGameBoardTile1 = this._leftSide[index];
    FlockadeGameBoardTile flockadeGameBoardTile2 = this._rightSide[index];
    DG.Tweening.Sequence t = flockadeGameBoardTile1.Exit(true);
    float atPosition = t.Duration() - 0.05f;
    float y = flockadeGameBoardTile1.RectTransform.anchoredPosition.y + this._gridRectTransform.rect.height / 2f + this._gridRectTransform.anchoredPosition.y;
    return DOTween.Sequence().AppendCallback((TweenCallback) (() => AudioManager.Instance.PlayOneShot("event:/dlc/ui/flockade_minigame/battle_duel_start"))).Append((Tween) t).Join((Tween) flockadeGameBoardTile2.Exit(true)).Insert(atPosition, (Tween) flockadeGameBoardTile1.SetDuelled(true)).Insert(atPosition, (Tween) flockadeGameBoardTile2.SetDuelled(true)).AppendInterval(0.05f).Append((Tween) this._fight.Animate(this, flockadeGameBoardTile1.GamePiece.Copy(), flockadeGameBoardTile2.GamePiece.Copy(), resolution, y, onClimax));
  }

  public void AnimatePlayers(int leftPlayerPointsDifference, int rightPlayerPointsDifference)
  {
    FlockadePlayerBase player1 = this.GetPlayer(FlockadeGameBoardSide.Left);
    if (leftPlayerPointsDifference <= 0)
    {
      if (leftPlayerPointsDifference < 0)
        player1.Animate(FlockadePlayerBase.AnimationState.LosePieceOrPoints);
    }
    else
      player1.Animate(FlockadePlayerBase.AnimationState.WinPieceOrPoints);
    FlockadePlayerBase player2 = this.GetPlayer(FlockadeGameBoardSide.Right);
    if (rightPlayerPointsDifference <= 0)
    {
      if (rightPlayerPointsDifference >= 0)
        return;
      player2.Animate(FlockadePlayerBase.AnimationState.LosePieceOrPoints);
    }
    else
      player2.Animate(FlockadePlayerBase.AnimationState.WinPieceOrPoints);
  }

  public void AllowFastForward()
  {
    if (this._fastForward != null)
      return;
    MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer = (PlayerFarming) null;
    this._controlPrompts.ShowFastForwardButton();
    this._fastForward = this.StartCoroutine((IEnumerator) this.\u003CAllowFastForward\u003Eg__FastForward\u007C66_0());
  }

  public void DisallowFastForward()
  {
    if (this._fastForward == null)
      return;
    this._controlPrompts.HideFastForwardButton();
    this.StopCoroutine(this._fastForward);
    this._fastForward = (Coroutine) null;
    if (!this.IsFastForwarding)
      return;
    this.IsFastForwarding = false;
    Time.timeScale = 1f;
    Action<bool> fastForwarding = this.FastForwarding;
    if (fastForwarding == null)
      return;
    fastForwarding(false);
  }

  public bool AllTilesAreLocked => this.Core.AllTilesAreLocked;

  public FlockadePassiveManager PassiveManager => this.Core.PassiveManager;

  public int RowCount => this.Core.RowCount;

  public FlockadePlayerBase GetPlayer(FlockadeGameBoardSide side)
  {
    return this.Core.GetPlayer(side) as FlockadePlayerBase;
  }

  public IEnumerable<FlockadePlayerBase> GetPlayers()
  {
    return this.Core.GetPlayers().OfType<FlockadePlayerBase>();
  }

  public IEnumerable<FlockadeGameBoardTile> GetColumnOf(FlockadeGameBoardTile tile)
  {
    return this.Core.GetColumnOf((IFlockadeGameBoardTile) tile).OfType<FlockadeGameBoardTile>();
  }

  public IEnumerable<FlockadeGameBoardTile> GetRowOf(FlockadeGameBoardTile tile)
  {
    return this.Core.GetRowOf((IFlockadeGameBoardTile) tile).OfType<FlockadeGameBoardTile>();
  }

  public int GetTileCount(FlockadeGameBoardSide side) => this.Core.GetTileCount(side);

  public IEnumerable<FlockadeGameBoardTile> GetTiles(FlockadeGameBoardSide side)
  {
    return this.Core.GetTiles(side).OfType<FlockadeGameBoardTile>();
  }

  public IEnumerable<FlockadeGameBoardTile> GetTiles()
  {
    return this.Core.GetTiles().OfType<FlockadeGameBoardTile>();
  }

  public FlockadeGameBoardTile GetTile(FlockadeGameBoardSide side, int index)
  {
    return this.Core.GetTile(side, index) as FlockadeGameBoardTile;
  }

  public FlockadeGameBoardTile GetOpposingTile(FlockadeGameBoardTile tile)
  {
    return this.Core.GetOpposingTile((IFlockadeGameBoardTile) tile) as FlockadeGameBoardTile;
  }

  public IEnumerable<FlockadeGameBoardTile> GetAvailableTiles(
    FlockadePlayerBase player,
    FlockadeGamePieceConfiguration gamePiece)
  {
    return this.Core.GetAvailableTiles((IFlockadePlayer) player, gamePiece).OfType<FlockadeGameBoardTile>();
  }

  public IEnumerator StartDuelPhase() => ((IFlockadeGameBoard) this).StartDuelPhase();

  public IEnumerator Resolve() => ((IFlockadeGameBoard) this).Resolve();

  public void Shake() => this.transform.DOShakePosition(0.65f, 10f).SetEase<Tweener>(Ease.OutCubic);

  [CompilerGenerated]
  public IEnumerator \u003CFlockade\u002EIFlockadeGameBoard\u002EStartDuelPhase\u003Eg__BeforeApplyingBlessings\u007C49_0(
    FlockadeVirtualGameBoard.BeforeApplyingBlessingsContext context)
  {
    if (context.OrderOfDuelsAndBlessingsApplicationReversedBlessing != null)
    {
      context.OrderOfDuelsAndBlessingsApplicationReversedBlessing.PrependInterval(0.166666672f);
      yield return (object) FlockadeUtils.WaitForCompletion((IEnumerable<Tween>) new DG.Tweening.Sequence[2]
      {
        context.OrderOfDuelsAndBlessingsApplicationReversedBlessing,
        this.Indicators.ActivateIcon()
      });
    }
  }

  [CompilerGenerated]
  public IEnumerator \u003CFlockade\u002EIFlockadeGameBoard\u002EStartDuelPhase\u003Eg__OnApplyingBlessing\u007C49_1(
    FlockadeVirtualGameBoard.OnApplyingBlessingContext context)
  {
    int blessingTileIndex = context.BlessingTileIndex;
    if (!this._duelOrder.Contains(blessingTileIndex))
      this._duelOrder.Add(blessingTileIndex);
    if (!this._pairDisplayNumber.ContainsKey(blessingTileIndex))
    {
      this._pairDisplayNumber[blessingTileIndex] = this._pairDisplayNumber.Count + 1;
      this.PaintDuelNumbersStable();
    }
    FlockadeGameBoardTile tile = context.Side == FlockadeGameBoardSide.Left ? this._leftSide[blessingTileIndex] : this._rightSide[blessingTileIndex];
    if (context.HasEffect)
      tile.SetTargeted(true);
    DG.Tweening.Sequence t = this.Indicators.SetEnabledLine(context.BlessingTileIndex / this.RowCount > 0 ? FlockadeGameBoardIndicators.Line.Back : FlockadeGameBoardIndicators.Line.Front);
    if (t != null)
    {
      yield return (object) t.WaitForCompletion();
      yield return (object) new WaitForSeconds(1f);
    }
    yield return (object) context.OnDuelPhaseStartedBlessing.WaitForCompletion;
    tile.SetTargeted(false);
  }

  [CompilerGenerated]
  public IEnumerator \u003CAllowFastForward\u003Eg__FastForward\u007C66_0()
  {
    while (true)
    {
      if (this.GetPlayers().Any<FlockadePlayerBase>((Func<FlockadePlayerBase, bool>) (player => player.GetFastForwardButtonHeld())))
      {
        if (!this.IsFastForwarding)
        {
          this.IsFastForwarding = true;
          Time.timeScale = 2f;
          Action<bool> fastForwarding = this.FastForwarding;
          if (fastForwarding != null)
            fastForwarding(true);
        }
      }
      else if (this.IsFastForwarding)
      {
        this.IsFastForwarding = false;
        Time.timeScale = 1f;
        Action<bool> fastForwarding = this.FastForwarding;
        if (fastForwarding != null)
          fastForwarding(false);
      }
      yield return (object) null;
    }
  }
}
