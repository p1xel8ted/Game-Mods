// Decompiled with JetBrains decompiler
// Type: src.UI.Menus.KBPlayerBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Febucci.UI;
using KnuckleBones;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#nullable disable
namespace src.UI.Menus;

public abstract class KBPlayerBase : MonoBehaviour
{
  [SerializeField]
  protected Dice _dicePrefab;
  [SerializeField]
  protected Transform _position;
  [SerializeField]
  protected List<KBDiceTub> _diceTubs = new List<KBDiceTub>();
  [SerializeField]
  protected TextMeshProUGUI _nameText;
  [SerializeField]
  protected TextMeshProUGUI _scoreText;
  [SerializeField]
  protected TextAnimator _textAnimator;
  [SerializeField]
  protected CanvasGroup _turnOverlay;
  [SerializeField]
  protected RectTransform _content;
  [SerializeField]
  protected RectTransform _tubsRect;
  protected string _playerName;
  private Vector2 _contentOriginPosition;
  private Vector2 _tubOriginPosition;
  private Vector2 _contentOffscreenOffset;
  private Vector2 _tubOffscreenOffset;

  protected abstract string _playDiceAnimation { get; }

  protected abstract string _playerIdleAnimation { get; }

  protected abstract string _playerTakeDiceAnimation { get; }

  protected abstract string _playerLostDiceAnimation { get; }

  protected abstract string _playerWonAnimation { get; }

  protected abstract string _playerWonLoop { get; }

  protected abstract string _playerLostAnimation { get; }

  protected abstract string _playerLostLoop { get; }

  protected abstract SkeletonGraphic _spine { get; }

  public Dice DicePrefab => this._dicePrefab;

  public Transform Position => this._position;

  public int Score
  {
    get
    {
      int score = 0;
      foreach (KBDiceTub diceTub in this._diceTubs)
        score += diceTub.Score;
      return score;
    }
  }

  public virtual void Configure(Vector2 contentOffscreenOffset, Vector2 tubOffscreenOffset)
  {
    this._scoreText.text = "";
    this._contentOriginPosition = (Vector2) this._content.localPosition;
    this._tubOriginPosition = (Vector2) this._tubsRect.localPosition;
    this._contentOffscreenOffset = contentOffscreenOffset;
    this._tubOffscreenOffset = tubOffscreenOffset;
    foreach (KBDiceTub diceTub in this._diceTubs)
    {
      diceTub.OnDiceMatched += new System.Action(this.OnDiceMatched);
      diceTub.OnDiceLost += new System.Action(this.OnDiceLost);
      diceTub.OpponentTub.OnDiceLost += new System.Action(this.OnDiceMatched);
    }
    this._nameText.text = this._playerName.Wave();
    this.Hide(true);
  }

  private void OnDiceMatched()
  {
    this._spine.AnimationState.SetAnimation(0, this._playerTakeDiceAnimation, false);
    this._spine.AnimationState.AddAnimation(0, this._playerIdleAnimation, false, 0.0f);
  }

  private void OnDiceLost()
  {
    this._scoreText.text = this.Score.ToString();
    this._spine.AnimationState.SetAnimation(0, this._playerLostDiceAnimation, false);
    this._spine.AnimationState.AddAnimation(0, this._playerIdleAnimation, false, 0.0f);
  }

  public void Show(bool instant = false)
  {
    if (instant)
    {
      this._content.localPosition = (Vector3) this._contentOriginPosition;
      this._tubsRect.localPosition = (Vector3) this._tubOriginPosition;
    }
    else
    {
      this._content.DOLocalMove((Vector3) this._contentOriginPosition, 1.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      this._tubsRect.DOLocalMove((Vector3) this._tubOriginPosition, 1.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    }
  }

  public void Hide(bool instant = false)
  {
    if (instant)
    {
      this._content.localPosition = (Vector3) (this._contentOriginPosition + this._contentOffscreenOffset);
      this._tubsRect.localPosition = (Vector3) (this._tubOriginPosition + this._tubOffscreenOffset);
    }
    this._content.DOLocalMove((Vector3) (this._contentOriginPosition + this._contentOffscreenOffset), 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    this._tubsRect.DOLocalMove((Vector3) (this._tubOriginPosition + this._tubOffscreenOffset), 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
  }

  public void HighlightMe()
  {
    this._turnOverlay.DOFade(0.0f, 1f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    this._textAnimator.enabled = true;
  }

  public void UnHighlightMe()
  {
    this._turnOverlay.DOFade(1f, 1f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    this._textAnimator.enabled = false;
  }

  public abstract IEnumerator SelectTub(Dice dice);

  protected IEnumerator FinishTubSelection(Dice dice, KBDiceTub diceTub)
  {
    yield return (object) diceTub.AddDice(dice);
    this._spine.AnimationState.SetAnimation(0, this._playDiceAnimation, false);
    this._spine.AnimationState.AddAnimation(0, this._playerIdleAnimation, true, 0.0f);
    yield return (object) new WaitForSecondsRealtime(0.5f);
    this._scoreText.text = this.Score.ToString();
  }

  public bool CheckGameCompleted()
  {
    int num = 0;
    foreach (KBDiceTub diceTub in this._diceTubs)
    {
      if (diceTub.Dice.Count >= 3)
        ++num;
    }
    return num >= this._diceTubs.Count;
  }

  public void FinalizeScores()
  {
    this._scoreText.text = this.Score.ToString();
    foreach (KBDiceTub diceTub in this._diceTubs)
      diceTub.FinalizeScore();
  }

  public void SetWonLoop()
  {
    this._spine.AnimationState.SetAnimation(0, this._playerWonAnimation, false);
    this._spine.AnimationState.AddAnimation(0, this._playerWonLoop, true, 0.0f);
  }

  public void SetLostLoop()
  {
    this._spine.AnimationState.SetAnimation(0, this._playerLostAnimation, false);
    this._spine.AnimationState.AddAnimation(0, this._playerLostLoop, true, 0.0f);
  }

  public abstract string GetLocalizedName();
}
