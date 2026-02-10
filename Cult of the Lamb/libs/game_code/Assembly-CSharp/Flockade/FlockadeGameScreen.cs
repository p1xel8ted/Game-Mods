// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeGameScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using src.UI;
using src.UINavigator;
using src.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;

#nullable disable
namespace Flockade;

public class FlockadeGameScreen : UISubmenuBase, IFlockadeTwitchMetadataProvider
{
  public const string _TRANSITION_SOUND = "event:/sermon/scroll_sermon_menu";
  [Header("Flockade")]
  [SerializeField]
  public FlockadeAnnouncement _announcement;
  [SerializeField]
  public FlockadeGamePieceBag _bag;
  [SerializeField]
  public FlockadeGameBoard _bigGameBoard;
  [SerializeField]
  public FlockadeBottomContainer _bottomContainer;
  [SerializeField]
  public FlockadeControlPrompts _controlPrompts;
  [SerializeField]
  public FlockadeEndGameAnnouncement _endGameAnnouncement;
  [SerializeField]
  public FlockadePlayer _goatPlayer;
  [SerializeField]
  public FlockadeGamePieceInformation _information;
  [SerializeField]
  public FlockadePlayer _lambPlayer;
  [SerializeField]
  public FlockadePlayer _coopGoatPlayer;
  [SerializeField]
  public FlockadePlayer _coopLambPlayer;
  [SerializeField]
  public FlockadeNpc _npc;
  [SerializeField]
  public FlockadeRoundCounter _roundCounter;
  [SerializeField]
  public FlockadeGameBoard _smallGameBoard;
  [SerializeField]
  public FlockadeTwitch _twitch;
  public CancellationTokenSource _activeCancellationSource;
  public int _bet;
  public FlockadeGameBoard _gameBoard;
  public int _identifier;
  public bool _matchCompleted;
  public FlockadePassiveManager _passiveManager;
  public FlockadePlayer _player;
  public FlockadePlayer _coopPlayer;

  public static event FlockadeGameScreen.MatchCompletionEvent OnMatchCompleted;

  public event Action<FlockadeUIController.Result> MatchCompleted;

  public event System.Action MatchQuit;

  public void Configure(
    PlayerFarming playerFarming,
    IEnumerable<FlockadeGamePieceConfiguration> gamePiecesPool,
    FlockadeOpponentConfiguration opponentConfiguration,
    int bet = 0)
  {
    bool flag = (UnityEngine.Object) playerFarming == (UnityEngine.Object) null || playerFarming.isLamb && !playerFarming.IsGoat;
    this._lambPlayer.gameObject.SetActive(flag);
    this._goatPlayer.gameObject.SetActive(!flag);
    this._player = flag ? this._lambPlayer : this._goatPlayer;
    this._player.Configure(FlockadeGameBoardSide.Left, this._bag, this._controlPrompts, this._parent, playerFarming);
    this._coopLambPlayer.gameObject.SetActive(opponentConfiguration.Type == FlockadeOpponentConfiguration.OpponentType.CoopPlayer && !flag);
    this._coopGoatPlayer.gameObject.SetActive(opponentConfiguration.Type == FlockadeOpponentConfiguration.OpponentType.CoopPlayer & flag);
    this._npc.gameObject.SetActive(opponentConfiguration.Type == FlockadeOpponentConfiguration.OpponentType.Npc);
    this._twitch.gameObject.SetActive(opponentConfiguration.Type == FlockadeOpponentConfiguration.OpponentType.Twitch);
    switch (opponentConfiguration.Type)
    {
      case FlockadeOpponentConfiguration.OpponentType.CoopPlayer:
        PlayerFarming playerFarming1 = (UnityEngine.Object) playerFarming == (UnityEngine.Object) null || (UnityEngine.Object) PlayerFarming.players[0] == (UnityEngine.Object) playerFarming ? PlayerFarming.players[1] : PlayerFarming.players[0];
        this._coopPlayer = flag ? this._coopGoatPlayer : this._coopLambPlayer;
        this._coopPlayer.Configure(FlockadeGameBoardSide.Right, this._bag, this._controlPrompts, this._parent, playerFarming1);
        this._coopPlayer.Opponent = (FlockadePlayerBase) this._player;
        this._player.Opponent = (FlockadePlayerBase) this._coopPlayer;
        break;
      case FlockadeOpponentConfiguration.OpponentType.Npc:
        this._npc.Configure(opponentConfiguration.NpcConfiguration, FlockadeGameBoardSide.Right, this._bag, this._controlPrompts, this._parent);
        this._npc.Opponent = (FlockadePlayerBase) this._player;
        this._player.Opponent = (FlockadePlayerBase) this._npc;
        break;
      case FlockadeOpponentConfiguration.OpponentType.Twitch:
        this._twitch.Configure((IFlockadeTwitchMetadataProvider) this, FlockadeGameBoardSide.Right, this._bag, this._controlPrompts, this._parent);
        this._twitch.Opponent = (FlockadePlayerBase) this._player;
        this._player.Opponent = (FlockadePlayerBase) this._twitch;
        break;
    }
    this._controlPrompts.HideAcceptButton();
    this._passiveManager = new FlockadePassiveManager();
    this._bag.Configure(gamePiecesPool, this._information, this._parent, this._passiveManager);
    this._bet = bet;
    this._identifier = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
  }

  public void LateConfigure(FlockadeGameScreen.Difficulty difficulty)
  {
    if (difficulty == FlockadeGameScreen.Difficulty.Hard)
    {
      this._smallGameBoard.gameObject.SetActive(false);
      this._bigGameBoard.gameObject.SetActive(true);
      this._gameBoard = this._bigGameBoard;
    }
    else
    {
      this._smallGameBoard.gameObject.SetActive(true);
      this._bigGameBoard.gameObject.SetActive(false);
      this._gameBoard = this._smallGameBoard;
    }
    this._gameBoard.Configure((FlockadePlayerBase) this._player, this._player.Opponent, this._controlPrompts, this._information, this._passiveManager);
    this._player.LateConfigure(this._gameBoard);
    this._player.Opponent.LateConfigure(this._gameBoard);
  }

  public override IEnumerator DoShowAnimation()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    FlockadeGameScreen flockadeGameScreen = this;
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
    AudioManager.Instance.PlayOneShot("event:/sermon/scroll_sermon_menu");
    flockadeGameScreen._canvasGroup.DOFade(1f, 0.5f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InCubic);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(0.25f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public override void OnShowStarted() => this.DeactivateAllSelectables();

  public override void OnShowCompleted() => this.StartCoroutine((IEnumerator) this.PlayGame());

  public IEnumerator PlayGame()
  {
    FlockadePlayerBase currentPlayer = this.SelectFirstPlayer();
    this._gameBoard.Enter();
    this._player.Enter();
    this._player.Opponent.Enter();
    while (this._player.Victories.Count < this._player.Victories.Maximum && this._player.Opponent.Victories.Count < this._player.Opponent.Victories.Maximum)
    {
      yield return (object) this._announcement.ShowRoundStart(currentPlayer).WaitForCompletion();
      this._bag.Refill();
      yield return (object) FlockadeUtils.WaitForCompletion((IEnumerable<Tween>) new DG.Tweening.Sequence[2]
      {
        this._roundCounter.Increment(),
        this._gameBoard.StartRound()
      });
      while (!this._gameBoard.AllTilesAreLocked)
      {
        yield return (object) this.PerformTurn(currentPlayer);
        currentPlayer = currentPlayer.Opponent;
      }
      yield return (object) FlockadeUtils.WaitForCompletion((IEnumerable<Tween>) new DG.Tweening.Sequence[5]
      {
        this._player.SetTurn(true, false),
        this._player.Opponent.SetTurn(true, false),
        this._bag.Exit(),
        this._information.Exit(),
        this._announcement.ShowDuelPhaseStart()
      });
      yield return (object) this._gameBoard.StartDuelPhase();
      IEnumerator coroutine;
      yield return (object) (coroutine = this._gameBoard.Resolve());
      FlockadeGameBoardSide? current = coroutine.Current as FlockadeGameBoardSide?;
      yield return (object) this._announcement.ShowRoundResults(current.HasValue ? this._gameBoard.GetPlayer(current.Value) : (FlockadePlayerBase) this._player, current.HasValue).WaitForCompletion();
      yield return (object) this._gameBoard.Wipe();
      currentPlayer = currentPlayer.Opponent;
      coroutine = (IEnumerator) null;
    }
    yield return (object) this.EndGame();
  }

  public FlockadePlayerBase SelectFirstPlayer()
  {
    if ((UnityEngine.Object) this._player.Opponent == (UnityEngine.Object) this._npc && DataManager.Instance.FlockadeFirstGameOpponentStarts)
    {
      DataManager.Instance.FlockadeFirstGameOpponentStarts = false;
      return this._player.Opponent;
    }
    return !BoolUtilities.RandomBool() ? this._player.Opponent : (FlockadePlayerBase) this._player;
  }

  public IEnumerator PerformTurn(FlockadePlayerBase currentPlayer)
  {
    if (currentPlayer.CanPerformTurn)
    {
      this._bottomContainer.Reconfigure(currentPlayer.Side);
      currentPlayer.OnBeforeTurn();
      yield return (object) FlockadeUtils.WaitForCompletion((IEnumerable<Tween>) new DG.Tweening.Sequence[4]
      {
        currentPlayer.SetTurn(true),
        this._bag.Draw(currentPlayer),
        this._bag.Enter(),
        this._information.Enter()
      });
      do
      {
        this.TryDisposeActiveCancellationSource();
        this._activeCancellationSource = new CancellationTokenSource();
        yield return (object) currentPlayer.SelectGamePiece();
        yield return (object) currentPlayer.PlaceGamePiece(new CancellationToken?(this._activeCancellationSource.Token));
      }
      while (this._activeCancellationSource.IsCancellationRequested);
      this.TryDisposeActiveCancellationSource();
      List<Tween> tweenList = new List<Tween>()
      {
        (Tween) this._bag.ReturnUnpicked()
      };
      if (currentPlayer.Opponent.CanPerformTurn)
        tweenList.AddRange((IEnumerable<Tween>) new DG.Tweening.Sequence[3]
        {
          this._bag.Exit(),
          this._information.Exit(),
          currentPlayer.SetTurn(false)
        });
      yield return (object) FlockadeUtils.WaitForCompletion((IEnumerable<Tween>) tweenList);
      this._information.Unlock((object) currentPlayer);
    }
  }

  public IEnumerator EndGame()
  {
    FlockadeGameScreen flockadeGameScreen = this;
    flockadeGameScreen._matchCompleted = true;
    yield return (object) FlockadeUtils.WaitForCompletion((IEnumerable<Tween>) new DG.Tweening.Sequence[3]
    {
      flockadeGameScreen._player.SetTurn(true, false),
      flockadeGameScreen._player.Opponent.SetTurn(true, false),
      flockadeGameScreen._endGameAnnouncement.ShowGameResults((FlockadePlayerBase) flockadeGameScreen._player, flockadeGameScreen._bet)
    });
    flockadeGameScreen._controlPrompts.HideAcceptButton();
    MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer = (PlayerFarming) null;
    flockadeGameScreen._controlPrompts.ShowAcceptButton();
    yield return (object) new WaitUntil((Func<bool>) new Func<bool>(flockadeGameScreen.\u003CEndGame\u003Eb__43_0));
    (int, int) valueTuple1 = (flockadeGameScreen._player.Victories.Count, flockadeGameScreen._player.Opponent.Victories.Count);
    FlockadeUIController.Result result1;
    if (valueTuple1.Item1 > valueTuple1.Item2)
    {
      result1 = FlockadeUIController.Result.Win;
    }
    else
    {
      (int, int) valueTuple2 = valueTuple1;
      result1 = valueTuple2.Item2 <= valueTuple2.Item1 ? FlockadeUIController.Result.Draw : FlockadeUIController.Result.Loss;
    }
    FlockadeUIController.Result result2 = result1;
    Action<FlockadeUIController.Result> matchCompleted = flockadeGameScreen.MatchCompleted;
    if (matchCompleted != null)
      matchCompleted(result2);
    FlockadeGameScreen.MatchCompletionEvent onMatchCompleted = FlockadeGameScreen.OnMatchCompleted;
    if (onMatchCompleted != null)
      onMatchCompleted(flockadeGameScreen._player.Opponent.Name, flockadeGameScreen._bet, result2);
    ObjectiveManager.CheckObjectives(Objectives.TYPES.WIN_FLOCKADE_BET);
  }

  public override IEnumerator DoHideAnimation()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    FlockadeGameScreen flockadeGameScreen = this;
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
    AudioManager.Instance.PlayOneShot("event:/sermon/scroll_sermon_menu");
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) flockadeGameScreen._canvasGroup.DOFade(0.0f, 1f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.Linear).WaitForCompletion();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void FakeEnd(FlockadeUIController.Result result)
  {
    Action<FlockadeUIController.Result> matchCompleted = this.MatchCompleted;
    if (matchCompleted != null)
      matchCompleted(result);
    FlockadeGameScreen.MatchCompletionEvent onMatchCompleted = FlockadeGameScreen.OnMatchCompleted;
    if (onMatchCompleted != null)
      onMatchCompleted(this._player.Opponent.Name, this._bet, result);
    ObjectiveManager.CheckObjectives(Objectives.TYPES.WIN_FLOCKADE_BET);
  }

  public void ForceEndGame()
  {
    this.StopAllCoroutines();
    this.DeactivateAllSelectables();
    this.StartCoroutine((IEnumerator) this.EndGame());
  }

  public virtual void LateUpdate()
  {
    if (!(Input.mousePosition != MonoSingleton<UINavigatorNew>.Instance.PreviousMouseInput))
      return;
    Cursor.visible = true;
  }

  public void DeactivateAllSelectables()
  {
    foreach (FlockadeSelectableBase componentsInChild in this.gameObject.GetComponentsInChildren<FlockadeSelectableBase>())
      componentsInChild.SetInteractable(false);
  }

  public void TryDisposeActiveCancellationSource()
  {
    if (this._activeCancellationSource == null)
      return;
    this._activeCancellationSource.Dispose();
    this._activeCancellationSource = (CancellationTokenSource) null;
  }

  public override void OnCancelButtonInput()
  {
    if (!this._parent.CanvasGroup.interactable || !this._canvasGroup.interactable)
      return;
    if (this._controlPrompts.IsBacking)
    {
      if (this._matchCompleted)
        return;
      Time.timeScale = 0.0f;
      UIMenuConfirmationWindow confirmationWindow = this.Push<UIMenuConfirmationWindow>(MonoSingleton<UIManager>.Instance.ConfirmationWindowTemplate);
      confirmationWindow.Configure(ScriptLocalization.UI_Flockade.Exit, ScriptLocalization.UI_Flockade.AreYouSure);
      confirmationWindow.OnShow = confirmationWindow.OnShow + (System.Action) (() => UIManager.PlayAudio("event:/ui/open_menu"));
      confirmationWindow.OnCancel += new System.Action(MonoSingleton<UINavigatorNew>.Instance.Clear);
      confirmationWindow.OnConfirm += (System.Action) (() =>
      {
        this.StopAllCoroutines();
        if ((UnityEngine.Object) this._player.Opponent == (UnityEngine.Object) this._npc)
        {
          FlockadeGameScreen.MatchCompletionEvent onMatchCompleted = FlockadeGameScreen.OnMatchCompleted;
          if (onMatchCompleted != null)
            onMatchCompleted(this._player.Opponent.Name, this._bet, FlockadeUIController.Result.Loss);
          Action<FlockadeUIController.Result> matchCompleted = this.MatchCompleted;
          if (matchCompleted == null)
            return;
          matchCompleted(FlockadeUIController.Result.Loss);
        }
        else
        {
          System.Action matchQuit = this.MatchQuit;
          if (matchQuit == null)
            return;
          matchQuit();
        }
      });
      confirmationWindow.OnHide = confirmationWindow.OnHide + (System.Action) (() => UIManager.PlayAudio("event:/ui/close_menu"));
      confirmationWindow.OnHidden = confirmationWindow.OnHidden + (System.Action) (() => Time.timeScale = 1f);
    }
    else
    {
      if (!this._controlPrompts.IsCancelling)
        return;
      this._activeCancellationSource.Cancel();
    }
  }

  FlockadeTwitchMetadata IFlockadeTwitchMetadataProvider.GetTwitchMetadata()
  {
    return new FlockadeTwitchMetadata()
    {
      available_regions = this.GetAvailableRegionsForTwitch(),
      available_pieces = this.GetAvailablePiecesForTwitch()
    };
  }

  public string[] GetAvailableRegionsForTwitch()
  {
    List<string> stringList = new List<string>();
    foreach (FlockadeGameBoardTile tile in this._gameBoard.GetTiles(FlockadeGameBoardSide.Right))
    {
      if (!tile.Locked)
      {
        switch (tile.Index)
        {
          case 0:
            stringList.Add("TOP_LEFT");
            continue;
          case 1:
            stringList.Add("BOTTOM_LEFT");
            continue;
          case 2:
            stringList.Add("TOP_RIGHT");
            continue;
          case 3:
            stringList.Add("BOTTOM_RIGHT");
            continue;
          default:
            continue;
        }
      }
    }
    return stringList.ToArray();
  }

  public FlockadeAvailablePiecesMetadata[] GetAvailablePiecesForTwitch()
  {
    List<FlockadeAvailablePiecesMetadata> availablePiecesMetadataList = new List<FlockadeAvailablePiecesMetadata>();
    foreach (FlockadeGamePieceChoice choice in this._bag.Choices)
    {
      string str = (UnityEngine.Object) choice.GamePiece.Core.Configuration.BlessingConfiguration != (UnityEngine.Object) null ? LocalizationManager.GetTranslation(choice.GamePiece.Core.Configuration.BlessingConfiguration.Description) : LocalizationManager.GetTranslation(choice.GamePiece.Core.Configuration.BaseConfiguration.KindDescription);
      if (str.Contains("{[KIND]}"))
        str = Regex.Replace(str ?? "", Regex.Escape("{[KIND]}"), LocalizationManager.GetTranslation(choice.GamePiece.Core.Configuration.Name));
      availablePiecesMetadataList.Add(new FlockadeAvailablePiecesMetadata()
      {
        id = choice.GamePiece.Core.Configuration.Image.name,
        name = LocalizationManager.GetTranslation(choice.GamePiece.Core.Configuration.Name),
        lore = LocalizationManager.GetTranslation(choice.GamePiece.Core.Configuration.Description),
        description = str,
        blessing = (UnityEngine.Object) choice.GamePiece.Core.Configuration.BlessingConfiguration != (UnityEngine.Object) null ? choice.GamePiece.Core.Configuration.BlessingConfiguration.Icon.name : "",
        blessing_outline = (UnityEngine.Object) choice.GamePiece.Core.Configuration.BlessingConfiguration != (UnityEngine.Object) null ? choice.GamePiece.Core.Configuration.BlessingConfiguration.Outline.name : "",
        blessing_color = (UnityEngine.Object) choice.GamePiece.Core.Configuration.BlessingConfiguration != (UnityEngine.Object) null ? ColorUtility.ToHtmlStringRGB(choice.GamePiece.Core.Configuration.BlessingConfiguration.Color) : ""
      });
    }
    return availablePiecesMetadataList.ToArray();
  }

  public static void CompleteFlockadeMatch(string opponentName, int bet)
  {
  }

  FlockadeGameBoardTile IFlockadeTwitchMetadataProvider.GetTileFromRegionID(string region)
  {
    int num = 0;
    switch (region)
    {
      case "TOP_LEFT":
        num = 0;
        break;
      case "BOTTOM_LEFT":
        num = 1;
        break;
      case "TOP_RIGHT":
        num = 2;
        break;
      case "BOTTOM_RIGHT":
        num = 3;
        break;
    }
    foreach (FlockadeGameBoardTile tile in this._gameBoard.GetTiles(FlockadeGameBoardSide.Right))
    {
      if (tile.Index == num)
        return tile;
    }
    using (IEnumerator<FlockadeGameBoardTile> enumerator = this._gameBoard.GetTiles(FlockadeGameBoardSide.Right).GetEnumerator())
    {
      if (enumerator.MoveNext())
        return enumerator.Current;
    }
    return (FlockadeGameBoardTile) null;
  }

  [CompilerGenerated]
  public bool \u003CEndGame\u003Eb__43_0()
  {
    return this._player.GetAcceptButtonDown() || this._player.Opponent.GetAcceptButtonDown();
  }

  [CompilerGenerated]
  public void \u003COnCancelButtonInput\u003Eb__50_1()
  {
    this.StopAllCoroutines();
    if ((UnityEngine.Object) this._player.Opponent == (UnityEngine.Object) this._npc)
    {
      FlockadeGameScreen.MatchCompletionEvent onMatchCompleted = FlockadeGameScreen.OnMatchCompleted;
      if (onMatchCompleted != null)
        onMatchCompleted(this._player.Opponent.Name, this._bet, FlockadeUIController.Result.Loss);
      Action<FlockadeUIController.Result> matchCompleted = this.MatchCompleted;
      if (matchCompleted == null)
        return;
      matchCompleted(FlockadeUIController.Result.Loss);
    }
    else
    {
      System.Action matchQuit = this.MatchQuit;
      if (matchQuit == null)
        return;
      matchQuit();
    }
  }

  public enum Difficulty
  {
    Normal,
    Hard,
  }

  public delegate void MatchCompletionEvent(
    string opponentName,
    int winnings,
    FlockadeUIController.Result result);
}
