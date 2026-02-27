// Decompiled with JetBrains decompiler
// Type: src.UI.Menus.KBGameScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using KnuckleBones;
using Lamb.UI;
using src.UINavigator;
using src.Utilities;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

#nullable disable
namespace src.UI.Menus;

public class KBGameScreen : UISubmenuBase
{
  public Action<bool> OnMatchFinished;
  [Header("Knuckle Bones")]
  [SerializeField]
  private UIMenuControlPrompts _controlPrompts;
  [SerializeField]
  private KBBackground _background;
  [SerializeField]
  private Transform _diceContainer;
  [SerializeField]
  private KBPlayer _player;
  [SerializeField]
  private KBOpponent _opponent;
  [SerializeField]
  private CanvasGroup _announcementCanvasGroup;
  [SerializeField]
  private RectTransform _announcementRectTransform;
  [SerializeField]
  private TextMeshProUGUI _announcementText;
  [SerializeField]
  private TextMeshProUGUI _winningsText;
  private KnucklebonesOpponent _opponentConfiguration;
  private Vector3 _announceTextStartingPosition;
  private UIMenuConfirmationWindow _exitConfirmationInstance;
  private bool _matchCompleted;
  private int _bet;

  public void Configure(KnucklebonesOpponent opponent, int bet)
  {
    this._opponentConfiguration = opponent;
    this._bet = bet;
  }

  public override void OnCancelButtonInput()
  {
    if (!this._parent.CanvasGroup.interactable || !this._canvasGroup.interactable || !((UnityEngine.Object) this._exitConfirmationInstance == (UnityEngine.Object) null) || this._matchCompleted)
      return;
    this._exitConfirmationInstance = this.Push<UIMenuConfirmationWindow>(MonoSingleton<UIManager>.Instance.ConfirmationWindowTemplate);
    this._exitConfirmationInstance.Configure(KnucklebonesModel.GetLocalizedString("Exit"), KnucklebonesModel.GetLocalizedString("AreYouSure"));
    this._exitConfirmationInstance.OnConfirm += (System.Action) (() =>
    {
      MonoSingleton<UINavigatorNew>.Instance.Clear();
      this.StopAllCoroutines();
      Action<bool> onMatchFinished = this.OnMatchFinished;
      if (onMatchFinished == null)
        return;
      onMatchFinished(false);
    });
    this._exitConfirmationInstance.OnCancel += (System.Action) (() => MonoSingleton<UINavigatorNew>.Instance.Clear());
    UIMenuConfirmationWindow confirmationInstance = this._exitConfirmationInstance;
    confirmationInstance.OnHide = confirmationInstance.OnHide + (System.Action) (() => this._exitConfirmationInstance = (UIMenuConfirmationWindow) null);
  }

  protected override void OnShowStarted()
  {
    this._announceTextStartingPosition = this._announcementRectTransform.position;
    this._player.Configure(new Vector2(-800f, 0.0f), new Vector2(0.0f, -800f));
    this._opponent.Configure(this._opponentConfiguration, new Vector2(800f, 0.0f), new Vector2(0.0f, 800f));
    this._controlPrompts.HideAcceptButton();
    this._winningsText.gameObject.SetActive(false);
  }

  protected override IEnumerator DoShowAnimation()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    KBGameScreen kbGameScreen = this;
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
    kbGameScreen._player.Show();
    kbGameScreen._opponent.Show();
    kbGameScreen._canvasGroup.DOFade(1f, 1.5f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutQuart).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) kbGameScreen._background.TransitionToEndValues();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  protected override void OnShowCompleted() => this.StartCoroutine((IEnumerator) this.DoGameLoop());

  protected override IEnumerator DoHideAnimation()
  {
    KBGameScreen kbGameScreen = this;
    AudioManager.Instance.PlayOneShot("event:/sermon/scroll_sermon_menu");
    kbGameScreen._player.Hide();
    kbGameScreen._opponent.Hide();
    kbGameScreen._canvasGroup.DOFade(0.0f, 1f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutQuart).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    yield return (object) kbGameScreen._background.TransitionToStartValues();
    yield return (object) new WaitForSecondsRealtime(0.5f);
  }

  private IEnumerator DoGameLoop()
  {
    this._diceContainer.DestroyAllChildren();
    bool playerTurn = BoolUtilities.RandomBool();
    if (DataManager.Instance.KnucklebonesFirstGameRatauStart)
      playerTurn = false;
    DataManager.Instance.KnucklebonesFirstGameRatauStart = false;
    this._announcementRectTransform.position = this._announceTextStartingPosition + new Vector3(800f, 0.0f);
    this._announcementRectTransform.DOMove(this._announceTextStartingPosition, 1f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    this._announcementCanvasGroup.DOFade(1f, 0.5f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    string str = playerTurn ? this._player.GetLocalizedName() : this._opponent.GetLocalizedName();
    this._announcementText.text = string.Format(KnucklebonesModel.GetLocalizedString("RollsFirst"), (object) str);
    yield return (object) new WaitForSecondsRealtime(3f);
    this._announcementRectTransform.DOMove(this._announceTextStartingPosition + new Vector3(-800f, 0.0f), 1f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    this._announcementCanvasGroup.DOFade(0.0f, 0.5f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(1f);
    bool completed = false;
    while (!completed)
    {
      for (int i = 0; i < 2; ++i)
      {
        if (playerTurn)
        {
          this._controlPrompts.ShowAcceptButton();
          yield return (object) this.PerformTurn((KBPlayerBase) this._player, new System.Action(Complete));
          this._controlPrompts.HideAcceptButton();
        }
        else
          yield return (object) this.PerformTurn((KBPlayerBase) this._opponent, new System.Action(Complete));
        if (completed)
        {
          Complete();
          break;
        }
        playerTurn = !playerTurn;
      }
    }
    yield return (object) this.DoEndGame();

    void Complete() => completed = true;
  }

  private IEnumerator PerformTurn(KBPlayerBase player, System.Action onCompleted)
  {
    KBPlayerBase oppositePlayer = this.GetOppositePlayer(player);
    player.HighlightMe();
    oppositePlayer.UnHighlightMe();
    yield return (object) new WaitForSecondsRealtime(0.5f);
    Dice currentDice = (Dice) null;
    AudioManager.Instance.PlayOneShot("event:/knuckle_bones/die_roll");
    currentDice = UnityEngine.Object.Instantiate<Dice>(player.DicePrefab, player.Position.position, Quaternion.identity, this._diceContainer);
    yield return (object) currentDice.StartCoroutine((IEnumerator) currentDice.RollRoutine(1f));
    yield return (object) new WaitForSecondsRealtime(0.5f);
    while ((UnityEngine.Object) this._exitConfirmationInstance != (UnityEngine.Object) null && (UnityEngine.Object) player == (UnityEngine.Object) this._player)
      yield return (object) null;
    yield return (object) null;
    yield return (object) player.SelectTub(currentDice);
    yield return (object) new WaitForSecondsRealtime(0.5f);
    if (this.CheckGameCompleted(player))
    {
      System.Action action = onCompleted;
      if (action != null)
        action();
    }
  }

  private IEnumerator DoEndGame()
  {
    this._matchCompleted = true;
    AudioManager.Instance.SetMusicRoomID(1, "ratau_home_id");
    int playerScore = this._player.Score;
    int opponentScore = this._opponent.Score;
    KBPlayerBase player = playerScore > opponentScore ? (KBPlayerBase) this._player : (KBPlayerBase) this._opponent;
    KBPlayerBase oppositePlayer = this.GetOppositePlayer(player);
    player.HighlightMe();
    oppositePlayer.HighlightMe();
    player.FinalizeScores();
    oppositePlayer.FinalizeScores();
    player.SetWonLoop();
    oppositePlayer.SetLostLoop();
    this._announcementRectTransform.position = this._announceTextStartingPosition + new Vector3(800f, 0.0f);
    this._announcementRectTransform.DOMove(this._announceTextStartingPosition, 1f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    this._announcementCanvasGroup.DOFade(1f, 0.5f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    this._announcementText.text = $"{string.Format(KnucklebonesModel.GetLocalizedString("Win"), (object) player.GetLocalizedName())} {player.Score} - {oppositePlayer.Score}";
    this._winningsText.gameObject.SetActive(this._bet > 0);
    this._opponent.PlayEndGameVoiceover(playerScore > opponentScore);
    if (playerScore >= opponentScore)
    {
      this._winningsText.text = $"+ {"<sprite name=\"icon_blackgold\">"} {this._bet}";
      AudioManager.Instance.PlayOneShot("event:/Stings/knucklebones_win");
    }
    else
    {
      this._winningsText.text = $"- {"<sprite name=\"icon_blackgold\">"} {this._bet}".Colour(StaticColors.RedColor);
      AudioManager.Instance.PlayOneShot("event:/Stings/knucklebones_lose");
    }
    yield return (object) new WaitForSecondsRealtime(1.5f);
    this._controlPrompts.HideCancelButton();
    this._controlPrompts.ShowAcceptButton();
    while (!InputManager.UI.GetAcceptButtonDown())
      yield return (object) null;
    Action<bool> onMatchFinished = this.OnMatchFinished;
    if (onMatchFinished != null)
      onMatchFinished(playerScore > opponentScore);
  }

  private KBPlayerBase GetOppositePlayer(KBPlayerBase player)
  {
    return !((UnityEngine.Object) player == (UnityEngine.Object) this._player) ? (KBPlayerBase) this._player : (KBPlayerBase) this._opponent;
  }

  private bool CheckGameCompleted(KBPlayerBase player) => player.CheckGameCompleted();

  public void ForceEndGame()
  {
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.DoEndGame());
  }
}
