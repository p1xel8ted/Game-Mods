// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeAnnouncement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using System;
using UnityEngine;

#nullable disable
namespace Flockade;

[RequireComponent(typeof (RectTransform), typeof (CanvasGroup))]
public class FlockadeAnnouncement : MonoBehaviour
{
  public const string _SHOW_ROUND_START_SOUND = "event:/dlc/ui/flockade_minigame/round_start_text";
  public const string _SHOW_DUEL_PHASE_START_SOUND = "event:/dlc/ui/flockade_minigame/battle_phase_start";
  public const string _SHOW_LEFT_SIDE_ROUND_VICTORY_SOUND = "event:/dlc/ui/flockade_minigame/battle_phase_end_player_victory";
  public const string _SHOW_RIGHT_SIDE_ROUND_VICTORY_SOUND = "event:/dlc/ui/flockade_minigame/battle_phase_end_opponent_victory";
  public const string _SHOW_ROUND_DRAW_SOUND = "event:/dlc/ui/flockade_minigame/battle_phase_end_draw";
  public const float _SHOW_HIDE_DURATION = 2f;
  public const Ease _HIDE_OPACITY_EASING = Ease.Linear;
  public const Ease _HIDE_POSITION_EASING = Ease.InQuart;
  public const Ease _SHOW_OPACITY_EASING = Ease.Linear;
  public const Ease _SHOW_POSITION_EASING = Ease.OutQuart;
  public const string _NAME_PARAMETER_NAME = "NAME";
  public const string _LOSER_SCORE_PARAMETER_NAME = "LOSER_SCORE";
  public const string _WINNER_SCORE_PARAMETER_NAME = "WINNER_SCORE";
  [SerializeField]
  public Localize _text;
  [SerializeField]
  public LocalizationParamsManager _textParameters;
  [Header("Translations")]
  [SerializeField]
  [TermsPopup("")]
  public string _roundStart;
  [SerializeField]
  [TermsPopup("")]
  public string _duelPhaseStart;
  [SerializeField]
  [TermsPopup("")]
  public string _roundDraw;
  [SerializeField]
  [TermsPopup("")]
  public string _roundWin;
  [SerializeField]
  [TermsPopup("")]
  public string _specialRoundWin;
  public CanvasGroup _canvasGroup;
  public string _currentlyShownName;
  public Vector2 _originAnchoredPosition;
  public RectTransform _rectTransform;
  public DG.Tweening.Sequence _sequence;

  public virtual void Awake()
  {
    this._canvasGroup = this.GetComponent<CanvasGroup>();
    this._rectTransform = this.GetComponent<RectTransform>();
    this._originAnchoredPosition = this._rectTransform.anchoredPosition;
    this._canvasGroup.alpha = 0.0f;
    this._rectTransform.anchoredPosition = new Vector2((float) -Screen.width, this._originAnchoredPosition.y);
  }

  public virtual void OnEnable()
  {
    LocalizationManager.OnLocalizeEvent += new LocalizationManager.OnLocalizeCallback(this.UpdateNames);
  }

  public virtual void OnDisable()
  {
    LocalizationManager.OnLocalizeEvent -= new LocalizationManager.OnLocalizeCallback(this.UpdateNames);
  }

  public DG.Tweening.Sequence ShowRoundStart(FlockadePlayerBase currentPlayer)
  {
    this.SetText(this._roundStart, currentPlayer);
    return this.Show().PrependCallback((TweenCallback) (() =>
    {
      AudioManager.Instance.SetFlockadeGameState(SoundConstants.FlockadeGameState.SetupPhase);
      AudioManager.Instance.PlayOneShot("event:/dlc/ui/flockade_minigame/round_start_text");
    }));
  }

  public DG.Tweening.Sequence ShowDuelPhaseStart()
  {
    this.SetText(this._duelPhaseStart);
    return this.Show().PrependCallback((TweenCallback) (() =>
    {
      AudioManager.Instance.SetFlockadeGameState(SoundConstants.FlockadeGameState.BattlePhase);
      AudioManager.Instance.PlayOneShot("event:/dlc/ui/flockade_minigame/battle_phase_start");
    }));
  }

  public DG.Tweening.Sequence ShowRoundResults(FlockadePlayerBase player, bool isSpecial = false)
  {
    if (isSpecial)
    {
      this.SetText(this._specialRoundWin, player);
      return this.Show().PrependCallback((TweenCallback) (() =>
      {
        player.Animate(FlockadePlayerBase.AnimationState.WinRound);
        player.Opponent.Animate(FlockadePlayerBase.AnimationState.LoseRound);
        AudioManager.Instance.PlayOneShot(player.Side == FlockadeGameBoardSide.Left ? "event:/dlc/ui/flockade_minigame/battle_phase_end_player_victory" : "event:/dlc/ui/flockade_minigame/battle_phase_end_opponent_victory");
      }));
    }
    (int, int) valueTuple1 = (player.Points.Count, player.Opponent.Points.Count);
    FlockadeUIController.Result result;
    if (valueTuple1.Item1 > valueTuple1.Item2)
    {
      result = FlockadeUIController.Result.Win;
    }
    else
    {
      (int, int) valueTuple2 = valueTuple1;
      result = valueTuple2.Item2 <= valueTuple2.Item1 ? FlockadeUIController.Result.Draw : FlockadeUIController.Result.Loss;
    }
    switch (result)
    {
      case FlockadeUIController.Result.Win:
        this.SetText(this._roundWin, player);
        return this.Show().PrependCallback((TweenCallback) (() =>
        {
          player.Animate(FlockadePlayerBase.AnimationState.WinRound);
          player.Opponent.Animate(FlockadePlayerBase.AnimationState.LoseRound);
          AudioManager.Instance.PlayOneShot("event:/dlc/ui/flockade_minigame/battle_phase_end_player_victory");
        }));
      case FlockadeUIController.Result.Loss:
        this.SetText(this._roundWin, player.Opponent);
        return this.Show().PrependCallback((TweenCallback) (() =>
        {
          player.Animate(FlockadePlayerBase.AnimationState.LoseRound);
          player.Opponent.Animate(FlockadePlayerBase.AnimationState.WinRound);
          AudioManager.Instance.PlayOneShot("event:/dlc/ui/flockade_minigame/battle_phase_end_opponent_victory");
        }));
      case FlockadeUIController.Result.Draw:
        this.SetText(this._roundDraw, player);
        return this.Show().PrependCallback((TweenCallback) (() =>
        {
          player.Animate(FlockadePlayerBase.AnimationState.LoseRound);
          player.Opponent.Animate(FlockadePlayerBase.AnimationState.LoseRound);
          AudioManager.Instance.PlayOneShot("event:/dlc/ui/flockade_minigame/battle_phase_end_draw");
        }));
      default:
        throw new ArgumentOutOfRangeException();
    }
  }

  public void SetText(string text, FlockadePlayerBase winnerOrMain = null)
  {
    this._text.Term = text;
    if (!(bool) (UnityEngine.Object) winnerOrMain)
      return;
    this._currentlyShownName = winnerOrMain.Name;
    this._textParameters.SetParameterValue("NAME", LocalizationManager.GetTranslation(this._currentlyShownName), false);
    LocalizationParamsManager textParameters1 = this._textParameters;
    int count = winnerOrMain.Points.Count;
    string ParamValue1 = count.ToString();
    textParameters1.SetParameterValue("WINNER_SCORE", ParamValue1, false);
    LocalizationParamsManager textParameters2 = this._textParameters;
    count = winnerOrMain.Opponent.Points.Count;
    string ParamValue2 = count.ToString();
    textParameters2.SetParameterValue("LOSER_SCORE", ParamValue2);
  }

  public DG.Tweening.Sequence Show()
  {
    DG.Tweening.Sequence sequence = this._sequence;
    if (sequence != null)
      sequence.Kill();
    this._sequence = DOTween.Sequence().Append((Tween) this._rectTransform.DOAnchorPosX(this._originAnchoredPosition.x, 2f).From<Vector2, Vector2, VectorOptions>(new Vector2((float) -Screen.width, this._originAnchoredPosition.y)).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.OutQuart)).Join((Tween) this._canvasGroup.DOFade(1f, 2f).From<float, float, FloatOptions>(0.0f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.Linear)).Append((Tween) this._rectTransform.DOAnchorPosX((float) Screen.width, 2f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.InQuart)).Join((Tween) this._canvasGroup.DOFade(0.0f, 2f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.Linear));
    return this._sequence;
  }

  public void UpdateNames()
  {
    if (string.IsNullOrEmpty(this._currentlyShownName))
      return;
    this._textParameters.SetParameterValue("NAME", LocalizationManager.GetTranslation(this._currentlyShownName), false);
  }
}
