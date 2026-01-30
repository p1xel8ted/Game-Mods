// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeEndGameAnnouncement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using Spine.Unity;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Flockade;

public class FlockadeEndGameAnnouncement : MonoBehaviour
{
  public static int _ALPHA_SHADER_PROPERTY = Shader.PropertyToID("_AlphaMultiply");
  public const float _BLACK_OVERLAY_OPACITY = 0.41f;
  public const Ease _EASING = Ease.OutQuad;
  public const float _INTRO_DURATION = 0.5f;
  public const Ease _INTRO_OPACITY_EASING = Ease.InOutQuad;
  public const string _LOSER_SCORE_PARAMETER_NAME = "LOSER_SCORE";
  public const string _NAME_PARAMETER_NAME = "NAME";
  public const float _PARTS_APPEARANCE_DELAY = 0.333333343f;
  public const float _PARTS_APPEARANCE_DURATION = 0.75f;
  public const float _PARTS_VERTICAL_TRANSLATION = -25f;
  public const string _WINNER_SCORE_PARAMETER_NAME = "WINNER_SCORE";
  public const string _GAME_ENDS_GOAT_VICTORY_SOUND = "event:/dlc/ui/flockade_minigame/game_end_goat_victory";
  public const string _GAME_ENDS_LAMB_VICTORY_SOUND = "event:/dlc/ui/flockade_minigame/game_end_lamb_victory";
  public const string _GAME_ENDS_PLAYER_VICTORY_SOUND = "event:/dlc/ui/flockade_minigame/game_end_player_victory_a";
  public const string _GAME_ENDS_PLAYER_VICTORY_TEXT_SOUND = "event:/dlc/ui/flockade_minigame/game_end_player_victory_b";
  public const string _GAME_ENDS_OPPONENT_VICTORY_SOUND = "event:/dlc/ui/flockade_minigame/game_end_opponent_victory_a";
  public const string _GAME_ENDS_OPPONENT_VICTORY_TEXT_SOUND = "event:/dlc/ui/flockade_minigame/game_end_opponent_victory_b";
  public const string _DRAW_STINGER = "event:/dlc/ui/flockade_minigame/sting_draw";
  public const string _PLAYER_LOSES_STINGER = "event:/dlc/ui/flockade_minigame/sting_lose";
  public const string _PLAYER_WINS_STINGER = "event:/dlc/ui/flockade_minigame/sting_win";
  [SerializeField]
  public RectTransform _avatar;
  [SerializeField]
  public CanvasGroup _avatarCanvasGroup;
  [SerializeField]
  public MMUIRadialGraphic _background;
  [SerializeField]
  public CanvasGroup _blackOverlay;
  [SerializeField]
  [TermsPopup("")]
  public string _draw;
  [SerializeField]
  public Image _flourish;
  [SerializeField]
  public Image _imageAvatar;
  [SerializeField]
  public SkeletonGraphic _spineAvatar;
  [SerializeField]
  public TextMeshProUGUI _subtext;
  [SerializeField]
  public LocalizationParamsManager _subtextParameters;
  [SerializeField]
  public Localize _subtextTranslation;
  [SerializeField]
  public TextMeshProUGUI _text;
  [SerializeField]
  public LocalizationParamsManager _textParameters;
  [SerializeField]
  public Localize _textTranslation;
  [SerializeField]
  [TermsPopup("")]
  public string _winner;
  [SerializeField]
  public TextMeshProUGUI _winnings;
  public string _currentlyShownName;
  public float _originAvatarAnchoredPositionY;
  public float _originOuterRadius;
  public float _originRadius;
  public float _originTextAnchoredPositionY;
  public float _originSubtextAnchoredPositionY;
  public float _originWinningsAnchoredPositionY;
  public DG.Tweening.Sequence _sequence;

  public virtual void Awake()
  {
    this._originOuterRadius = this._background.OuterRadius;
    this._originRadius = this._background.Radius;
    this._background.material = new Material(this._background.material);
    this._background.material.SetFloat(FlockadeEndGameAnnouncement._ALPHA_SHADER_PROPERTY, 0.0f);
    this._background.Radius = this._background.OuterRadius = 0.0f;
    this._flourish.color = new Color(this._flourish.color.r, this._flourish.color.g, this._flourish.color.b, 0.0f);
    this._avatarCanvasGroup.alpha = 0.0f;
    this._originAvatarAnchoredPositionY = this._avatar.anchoredPosition.y;
    this._avatar.anchoredPosition -= new Vector2(0.0f, -25f);
    this._text.alpha = 0.0f;
    this._originTextAnchoredPositionY = this._text.rectTransform.anchoredPosition.y;
    this._text.rectTransform.anchoredPosition -= new Vector2(0.0f, -25f);
    this._subtext.alpha = 0.0f;
    this._originSubtextAnchoredPositionY = this._subtext.rectTransform.anchoredPosition.y;
    this._subtext.rectTransform.anchoredPosition -= new Vector2(0.0f, -25f);
    this._winnings.alpha = 0.0f;
    this._originWinningsAnchoredPositionY = this._winnings.rectTransform.anchoredPosition.y;
    this._winnings.rectTransform.anchoredPosition -= new Vector2(0.0f, -25f);
  }

  public virtual void OnEnable()
  {
    LocalizationManager.OnLocalizeEvent += new LocalizationManager.OnLocalizeCallback(this.UpdateNames);
  }

  public virtual void OnDisable()
  {
    LocalizationManager.OnLocalizeEvent -= new LocalizationManager.OnLocalizeCallback(this.UpdateNames);
  }

  public DG.Tweening.Sequence ShowGameResults(FlockadePlayerBase player, int winnings = 0)
  {
    (int, int) valueTuple1 = (player.Victories.Count, player.Opponent.Victories.Count);
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
    FlockadeUIController.Result results = result;
    switch (results)
    {
      case FlockadeUIController.Result.Win:
        this.SetAvatar(player);
        this.SetAllTexts(player);
        this._winnings.text = string.Format(this._winnings.text, (object) "+", (object) winnings).Colour(StaticColors.GreenColor);
        return this.Show(results, winnings > 0).PrependCallback((TweenCallback) (() =>
        {
          player.Animate(FlockadePlayerBase.AnimationState.WinGame);
          player.Opponent.Animate(FlockadePlayerBase.AnimationState.LoseGame);
          if ((bool) (UnityEngine.Object) player.PlayerFarming)
          {
            AudioManager instance = AudioManager.Instance;
            PlayerFarming playerFarming = player.PlayerFarming;
            string soundPath = playerFarming == null || !playerFarming.isLamb || playerFarming.IsGoat ? "event:/dlc/ui/flockade_minigame/game_end_goat_victory" : "event:/dlc/ui/flockade_minigame/game_end_lamb_victory";
            instance.PlayOneShot(soundPath);
          }
          AudioManager.Instance.PlayOneShot("event:/dlc/ui/flockade_minigame/game_end_player_victory_a");
          AudioManager.Instance.PlayOneShot("event:/dlc/ui/flockade_minigame/sting_win");
          AudioManager.Instance.SetFlockadeGameState(SoundConstants.FlockadeGameState.StingerWin);
        }));
      case FlockadeUIController.Result.Loss:
        this.SetAvatar(player.Opponent);
        this.SetAllTexts(player.Opponent);
        this._winnings.text = string.Format(this._winnings.text, (object) "-", (object) winnings).Colour(StaticColors.RedColor);
        return this.Show(results, winnings > 0).PrependCallback((TweenCallback) (() =>
        {
          player.Animate(FlockadePlayerBase.AnimationState.LoseGame);
          player.Opponent.Animate(FlockadePlayerBase.AnimationState.WinGame);
          if ((bool) (UnityEngine.Object) player.Opponent.PlayerFarming)
          {
            AudioManager instance = AudioManager.Instance;
            PlayerFarming playerFarming = player.Opponent.PlayerFarming;
            string soundPath = playerFarming == null || !playerFarming.isLamb || playerFarming.IsGoat ? "event:/dlc/ui/flockade_minigame/game_end_goat_victory" : "event:/dlc/ui/flockade_minigame/game_end_lamb_victory";
            instance.PlayOneShot(soundPath);
          }
          AudioManager.Instance.PlayOneShot("event:/dlc/ui/flockade_minigame/game_end_opponent_victory_a");
          AudioManager.Instance.PlayOneShot("event:/dlc/ui/flockade_minigame/sting_lose");
          AudioManager.Instance.SetFlockadeGameState(SoundConstants.FlockadeGameState.StingerLose);
          if (!(player.Opponent is FlockadeNpc opponent2))
            return;
          AudioManager.Instance.PlayOneShot(opponent2.WinGameSound);
        }));
      case FlockadeUIController.Result.Draw:
        this.SetAvatar((FlockadePlayerBase) null);
        this.SetAllTexts(player, true);
        this._winnings.text = string.Empty;
        return this.Show(results, winnings > 0).PrependCallback((TweenCallback) (() =>
        {
          player.Animate(FlockadePlayerBase.AnimationState.LoseGame);
          player.Opponent.Animate(FlockadePlayerBase.AnimationState.LoseGame);
          AudioManager.Instance.PlayOneShot("event:/dlc/ui/flockade_minigame/game_end_player_victory_a");
          AudioManager.Instance.PlayOneShot("event:/dlc/ui/flockade_minigame/sting_draw");
          AudioManager.Instance.SetFlockadeGameState(SoundConstants.FlockadeGameState.StingerDraw);
        }));
      default:
        throw new ArgumentOutOfRangeException();
    }
  }

  public void SetAvatar(FlockadePlayerBase winner)
  {
    if (!(bool) (UnityEngine.Object) winner)
    {
      this._spineAvatar.gameObject.SetActive(false);
      this._imageAvatar.gameObject.SetActive(false);
    }
    else
    {
      switch (winner.Avatar)
      {
        case SkeletonGraphic skeletonGraphic:
          this._spineAvatar.skeletonDataAsset = skeletonGraphic.skeletonDataAsset;
          this._spineAvatar.material = skeletonGraphic.material;
          this._spineAvatar.initialFlipX = skeletonGraphic.initialFlipX;
          this._spineAvatar.initialSkinName = skeletonGraphic.initialSkinName;
          this._spineAvatar.startingAnimation = skeletonGraphic.AnimationState.GetCurrent(0).Animation.Name;
          this._spineAvatar.startingLoop = skeletonGraphic.startingLoop;
          this._spineAvatar.unscaledTime = skeletonGraphic.unscaledTime;
          this._spineAvatar.transform.localScale = skeletonGraphic.transform.localScale;
          this._spineAvatar.gameObject.SetActive(true);
          break;
        case Image image:
          this._imageAvatar.material = image.material;
          this._imageAvatar.sprite = image.sprite;
          this._imageAvatar.preserveAspect = image.preserveAspect;
          this._imageAvatar.transform.localScale = image.transform.localScale;
          this._imageAvatar.rectTransform.pivot = image.rectTransform.pivot;
          this._imageAvatar.rectTransform.sizeDelta = image.rectTransform.rect.size;
          this._imageAvatar.gameObject.SetActive(true);
          break;
      }
    }
  }

  public void SetAllTexts(FlockadePlayerBase winnerOrMain, bool isDraw = false)
  {
    if (isDraw)
    {
      this._textTranslation.Term = this._draw;
      this._textParameters.SetParameterValue("WINNER_SCORE", winnerOrMain.Victories.Count.ToString(), false);
      this._textParameters.SetParameterValue("LOSER_SCORE", winnerOrMain.Opponent.Victories.Count.ToString());
    }
    else
    {
      this._currentlyShownName = winnerOrMain.Name;
      string translation = LocalizationManager.GetTranslation(this._currentlyShownName);
      this._textTranslation.Term = this._winner;
      this._textParameters.SetParameterValue("NAME", translation);
      this._subtextTranslation.Term = winnerOrMain.VictorySentence;
      this._subtextParameters.SetParameterValue("NAME", translation);
    }
  }

  public DG.Tweening.Sequence Show(FlockadeUIController.Result results, bool hasWinnings)
  {
    DG.Tweening.Sequence sequence = this._sequence;
    if (sequence != null)
      sequence.Kill();
    this._sequence = DOTween.Sequence().Append((Tween) this._blackOverlay.DOFade(0.41f, 0.5f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InOutQuad)).Join((Tween) DOVirtual.Float(0.0f, 1f, 0.5f, new TweenCallback<float>(this.UpdateBackgroundOpacity)).SetEase<Tweener>(Ease.InOutQuad)).Join((Tween) DOVirtual.Float(0.0f, 1f, 0.5f, new TweenCallback<float>(this.UpdateBackgroundScaling)).SetEase<Tweener>(Ease.OutQuad)).Append((Tween) DOTween.Sequence().Append((Tween) this._avatarCanvasGroup.DOFade(1f, 0.75f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutQuad)).Join((Tween) this._avatar.DOAnchorPosY(this._originAvatarAnchoredPositionY, 0.75f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.OutQuad)).InsertCallback(0.333333343f, (TweenCallback) (() =>
    {
      switch (results)
      {
        case FlockadeUIController.Result.Win:
          AudioManager.Instance.PlayOneShot("event:/dlc/ui/flockade_minigame/game_end_player_victory_b");
          break;
        case FlockadeUIController.Result.Loss:
          AudioManager.Instance.PlayOneShot("event:/dlc/ui/flockade_minigame/game_end_opponent_victory_b");
          break;
      }
    })).Insert(0.333333343f, (Tween) ShortcutExtensionsTMPText.DOFade(this._text, 1f, 0.75f).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.OutQuad)).Insert(0.333333343f, (Tween) this._text.rectTransform.DOAnchorPosY(this._originTextAnchoredPositionY, 0.75f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.OutQuad)).Insert(0.333333343f, (Tween) ShortcutExtensionsTMPText.DOFade(this._subtext, 1f, 0.75f).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.OutQuad)).Insert(0.333333343f, (Tween) this._subtext.rectTransform.DOAnchorPosY(this._originSubtextAnchoredPositionY, 0.75f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.OutQuad)).Insert(0.6666667f, (Tween) ShortcutExtensionsTMPText.DOFade(this._winnings, hasWinnings ? 1f : 0.0f, 0.75f).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.OutQuad)).Insert(0.6666667f, (Tween) this._winnings.rectTransform.DOAnchorPosY(this._originWinningsAnchoredPositionY, 0.75f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.OutQuad)));
    return this._sequence;
  }

  public void UpdateBackgroundOpacity(float progress)
  {
    this._background.material.SetFloat(FlockadeEndGameAnnouncement._ALPHA_SHADER_PROPERTY, progress);
    this._flourish.color = new Color(this._flourish.color.r, this._flourish.color.g, this._flourish.color.b, progress);
  }

  public void UpdateBackgroundScaling(float progress)
  {
    this._background.Radius = progress * this._originRadius;
    this._background.OuterRadius = progress * this._originOuterRadius;
  }

  public void UpdateNames()
  {
    if (string.IsNullOrEmpty(this._currentlyShownName))
      return;
    string translation = LocalizationManager.GetTranslation(this._currentlyShownName);
    this._textParameters.SetParameterValue("NAME", translation, false);
    this._subtextParameters.SetParameterValue("NAME", translation, false);
  }
}
